using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Hubs;
using TTDesign.API.Hubs.Intefaces;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class NotificationService : GenericService<Notification>, INotificationService
  {
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationAssignRepository _notificationAssignRepository;
    private readonly ILogger<NotificationService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;

    private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
    private readonly IUserConnectionManager _userConnectionManager;

    public NotificationService( INotificationRepository notificationRepository,
      INotificationAssignRepository notificationAssignRepository,
      ILogger<NotificationService> logger,
      IMapper mapper, IUserRepository userRepository, ITeamRepository teamRepository,
      IHubContext<NotificationUserHub> notificationUserHubContext, IUserConnectionManager userConnectionManager ) : base( notificationRepository )
    {
      _notificationRepository = notificationRepository;
      _notificationAssignRepository = notificationAssignRepository;
      _userRepository = userRepository;
      _teamRepository = teamRepository;
      _logger = logger;
      _mapper = mapper;

      _notificationUserHubContext = notificationUserHubContext;
      _userConnectionManager = userConnectionManager;
    }

    #region BaseServiceList
    public async Task<IEnumerable<NotificationResponse>> GetList( BaseFilter filter )
    {
      var notis = await _notificationAssignRepository.GetListNotification( n => n.UserId == filter.UserId && n.Notification.ModifiedDate >= DateTime.UtcNow.Date.AddDays( -20 ) );
      return _mapper.Map<IEnumerable<NotificationResponse>>( notis.OrderByDescending( n => n.Notification.ModifiedDate ) );
    }
    #endregion

    #region others
    public async Task Create( int objectType, object obj )
    {
      Notification? notiRecord = null;
      switch ( objectType ) {
        case 0: // Notification
          var infor = ( Dictionary<string, string> ) obj!;
          notiRecord = new Notification()
          {
            Title = infor.ContainsKey( "Title" ) ? infor [ "Title" ] : string.Empty,
            Content = infor.ContainsKey( "Content" ) ? infor [ "Content" ] : string.Empty,
            Type = ( int ) Enums.NotificationType.Information,
            ObjectType = objectType,
            ObjectId = int.Parse( infor [ "ObjectId" ] ),
            CreatedBy = int.Parse( infor [ "CreatedBy" ] ),
          };
          if ( infor.ContainsKey( "UserName" ) ) {
            notiRecord.UserName = infor [ "UserName" ];
          }
          if ( infor.ContainsKey( "To" ) ) {
            notiRecord.NotificationAssigns = infor [ "To" ].Split( "," ).Select( t => new NotificationAssign()
            {
              UserId = int.Parse( t ),
            } ).ToList();
          }
          break;
        case 1: // LeaveRequest
          var leaveRequest = ( LeaveRequest ) obj!;
          var userData = await _userRepository.GetUserWithLeader( leaveRequest.CreatedBy );
          notiRecord = new Notification()
          {
            Title = "Leave Request",
            Content = string.Format( "Leave request at {0} ~ {1}", leaveRequest.StartDate.ToString( Enums.DATETIME_FORMAT ), leaveRequest.EndDate.ToString( Enums.HOUR_FORMAT ) ),
            Type = ( int ) Enums.NotificationType.Information,
            ObjectId = leaveRequest.Id,
            ObjectType = objectType,
            CreatedBy = leaveRequest.CreatedBy,
            UserName = userData [ "User" ] [ 0 ].UserName,
          };
          if ( userData [ "Leader" ].Count() > 0 )
            notiRecord.NotificationAssigns = userData [ "Leader" ].Select( u => new NotificationAssign()
            {
              UserId = u.Id,
            } ).ToList();
          break;
        case 2: // WfhRequest
          var wfhRequest = ( WfhRequest ) obj!;
          userData = await _userRepository.GetUserWithLeader( wfhRequest.CreatedBy );
          notiRecord = new Notification()
          {
            Title = "WFH Request",
            Content = string.Format( "WFH request at {0} ~ {1}", wfhRequest.StartTime.ToString( Enums.DATE_FORMAT ), wfhRequest.EndTime.ToString( Enums.DATE_FORMAT ) ),
            Type = ( int ) Enums.NotificationType.Information,
            ObjectId = wfhRequest.Id,
            ObjectType = objectType,
            CreatedBy = wfhRequest.CreatedBy,
            UserName = userData [ "User" ] [ 0 ].UserName,
          };
          if ( userData [ "Leader" ].Count() > 0 )
            notiRecord.NotificationAssigns = userData [ "Leader" ].Select( u => new NotificationAssign()
            {
              UserId = u.Id,
            } ).ToList();
          break;
        default:
          break;
      }
      if ( notiRecord is not null ) {
        await _notificationRepository.CreateAsync( notiRecord );
        // [TODO] send notification hub
        var notiHub = new NotificationAsync( _notificationUserHubContext, _userConnectionManager, _mapper.Map<NotificationResponse>( notiRecord ),
          notiRecord.NotificationAssigns.Select( n => n.UserId ).ToList(), _logger );
        Thread t1 = new Thread( new ThreadStart( notiHub.SendNotification ) );
        t1.Start();
      }
    }

    public async Task MarkReadAll( long userId )
    {
      var notis = await _notificationAssignRepository.GetListByCondition( n => n.UserId == userId && !n.Status );
      if ( notis.Any() ) {
        foreach ( var noti in notis ) {
          noti.Status = true;
        }
        _notificationAssignRepository.Updates( notis.ToArray() );
      }
    }

    public async Task DeleteAll( long userId )
    {
      await _notificationAssignRepository.DeleteByCondition( n => n.UserId == userId );
    }

    public async Task MarkRead( long id, long userId )
    {
      var noti = await _notificationAssignRepository.GetByCondition( n => n.NotificationId == id && n.UserId == userId );
      if ( noti is not null ) {
        noti.Status = true;
        _notificationAssignRepository.Update( noti );
      }
    }

    public async Task Delete( long id, long? userId )
    {
      if ( userId is null )
        await _notificationAssignRepository.DeleteByCondition( n => n.NotificationId == id );
      else
        await _notificationAssignRepository.DeleteByCondition( n => n.UserId == userId && n.NotificationId == id );
    }

    public async Task ChangeUserAssign( int objectType, long objectId, int type, long creator )
    {
      var noti = await _notificationRepository.getNotification( n => n.ObjectId == objectId && n.ObjectType == objectType );
      if ( noti is not null ) {
        // remove all old assign
        _notificationAssignRepository.Deletes( noti.NotificationAssigns );
        // update noti
        noti.Type = type;
        if ( type == ( int ) Enums.NotificationType.Information ) {
          var userData = await _userRepository.GetUserWithLeader( creator );
          noti.NotificationAssigns = userData [ "Leader" ].Select( u => new NotificationAssign()
          {
            NotificationId = noti.Id,
            UserId = u.Id
          } ).ToList();
        }
        else {
          noti.NotificationAssigns = new List<NotificationAssign>(){ new NotificationAssign()
          {
            NotificationId = noti.Id,
            UserId = creator
          } };
          // [TODO] send notification hub
          var notiHub = new NotificationAsync( _notificationUserHubContext, _userConnectionManager, _mapper.Map<NotificationResponse>( noti ), noti.NotificationAssigns.Select( n => n.UserId ).ToList(), _logger );
          Thread t1 = new Thread( new ThreadStart( notiHub.SendNotification ) );
          t1.Start();
        }
        _notificationRepository.Update( noti );
      }
    }
    #endregion
  }

  public class NotificationAsync
  {
    private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
    private readonly IUserConnectionManager _userConnectionManager;
    private NotificationResponse _notiRecord;
    private List<long> _userIds;
    private readonly ILogger _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notificationUserHubContext"></param>
    /// <param name="userConnectionManager"></param>
    /// <param name="notification"></param>
    /// <param name="userIds"></param>
    /// <param name="logger"></param>
    public NotificationAsync( IHubContext<NotificationUserHub> notificationUserHubContext, IUserConnectionManager userConnectionManager,
      NotificationResponse notification, List<long> userIds, ILogger logger )
    {
      _notificationUserHubContext = notificationUserHubContext;
      _userConnectionManager = userConnectionManager;
      _notiRecord = notification;
      _userIds = userIds;
      _logger = logger;
    }

    public void SendNotification()
    {
      foreach ( var id in _userIds ) {
        var connections = _userConnectionManager.GetUserConnections( id.ToString() );
        if ( connections is not null && connections.Any() ) {
          _logger.LogInformation( $"[TEST] (notification) user ID: {id}, connection: {string.Join( ',', connections )}" );
        }
        else {
          _logger.LogInformation( $"[TEST] (notification) user ID: {id}, connection: blank" );
        }
        if ( connections != null && connections.Count > 0 ) {
          foreach ( var connectionId in connections ) {
            _notificationUserHubContext.Clients.Client( connectionId ).SendAsync( "Notification", _notiRecord );
          }
        }
      }
    }
  }
}
