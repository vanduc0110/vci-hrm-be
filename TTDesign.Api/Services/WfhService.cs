using AutoMapper;
using System.Globalization;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class WfhService : GenericService<WfhRequest>, IWfhService
  {
    private readonly IWfhRequestRepository _wfhRepository;
    private readonly ILogger<WfhService> _logger;
    private readonly IMapper _mapper;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITimesheetService _timesheetService;
    private readonly INotificationService _notificationService;

    public WfhService( IWfhRequestRepository wfhRepository,
      ILogger<WfhService> logger,
      IMapper mapper, INotificationService notificationService,
      ITeamRepository teamRepository, IUserRepository userRepository,
      ITimesheetService timesheetService ) : base( wfhRepository )
    {
      _wfhRepository = wfhRepository;
      _logger = logger;
      _mapper = mapper;
      _teamRepository = teamRepository;
      _userRepository = userRepository;
      _timesheetService = timesheetService;
      _notificationService = notificationService;
    }

    #region BaseServiceList
    public async Task<IEnumerable<WfhResponse>> GetRequestList( long userRequest, long inYear )
    {
      var requests = await _wfhRepository.GetListByCondition( o => o.CreatedBy == userRequest && o.StartTime.Year == inYear );
      var responses = _mapper.Map<IEnumerable<WfhResponse>>( requests.OrderByDescending( w => w.StartTime ) );
      var users = new Dictionary<long, string>();
      // set reviewer
      foreach ( var response in responses ) {
        if ( response.Reviewer > 0 ) {
          if ( users.ContainsKey( response.Reviewer ) ) {
            response.ReviewerName = users [ response.Reviewer ];
          }
          else {
            var user = await _userRepository.GetByConditionNoTrack( u => u.Id == response.Reviewer );
            response.ReviewerName = user!.FullName;
            users.Add( user.Id, user.FullName );
          }
        }
        response.Days = CountDays( DateTime.ParseExact( response.StartTime, Enums.DATE_FORMAT, CultureInfo.InvariantCulture ), DateTime.ParseExact( response.EndTime, Enums.DATE_FORMAT, CultureInfo.InvariantCulture ) );
      }
      return responses;
    }
    public async Task<int> GetTotalWfhRequest( long? teamId = null )
    {
      var requests = ( List<WfhRequest> ) await _wfhRepository.GetListByCondition( o => o.Status == ( int ) Enums.WfhRequestStatus.Pending );
      if ( teamId is not null ) {
        var users = await _teamRepository.GetTeamUser( ( long ) teamId );
        var userIds = users.Select( u => u.Id ).ToList();
        requests = ( List<WfhRequest> ) await _wfhRepository.GetListByCondition( o => o.Status == ( int ) Enums.LeaveRequestStatus.Pending &&
          userIds.Contains( o.CreatedBy ) );
      }
      return requests != null ? requests.Count() : 0;
    }
    /// <summary>
    /// - leader: danh sách user request OT trong team leader quản lý
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IEnumerable<WfhResponse>> GetList( BaseFilter filter )
    {
      var requests = new List<WfhRequest>();
      if ( filter.TeamId is null ) {
        if ( filter.Year == 1 && filter.Month == 1 ) {
          var dataAll = await _wfhRepository.GetAll();
          requests = dataAll.ToList();
        }
        else
          requests = ( List<WfhRequest> ) await _wfhRepository.GetListByCondition( o => o.StartTime.Year == filter.Year && o.StartTime.Month == filter.Month );
      }
      else {
        // lấy danh sách user thuộc team mà user login quản lý
        var members = await _teamRepository.GetTeamUser( ( long ) filter.TeamId );
        var memberIds = members.Select( u => u.Id );
        if ( members.Any() ) {

          requests = ( List<WfhRequest> ) await _wfhRepository.GetListByCondition( o => memberIds.Any( p => p == o.CreatedBy ) &&
            o.StartTime.Year == filter.Year && o.StartTime.Month == filter.Month );
        }
        if ( filter.Year == 1 && filter.Month == 1 ) {
          var dataAll = await _wfhRepository.GetAll();
          requests = dataAll.Where( o => memberIds.Contains( o.CreatedBy ) ).ToList();
        }
      }
      var responses = _mapper.Map<IEnumerable<WfhResponse>>( requests.OrderByDescending( w => w.StartTime ) );
      var users = new Dictionary<long, User>();
      foreach ( var response in responses ) {
        response.Days = CountDays( DateTime.ParseExact( response.StartTime, Enums.DATE_FORMAT, null ), DateTime.ParseExact( response.EndTime, Enums.DATE_FORMAT, null ) );

        // set reviewer
        if ( response.Reviewer > 0 ) {
          if ( users.ContainsKey( response.Reviewer ) ) {
            response.ReviewerName = users [ response.Reviewer ].FullName;
          }
          else {
            var user = await _userRepository.GetUserDataByCondition( u => u.Id == response.Reviewer );
            response.ReviewerName = user!.FullName;
            users.Add( user.Id, user );
          }
        }
        // set user request
        if ( users.ContainsKey( response.CreatedBy ) ) {
          response.CreatedName = users [ response.CreatedBy ].FullName;
          var teamUser = users [ response.CreatedBy ].TeamUsers;
          foreach ( var user in teamUser ) {
            response.Teams.Add( new TeamUserOptionResponse
            {
              TeamId = user.Team.Id,
              TeamCode = user.Team.Code,
              TeamName = user.Team.Name
            } );
          }
        }
        else {
          var user = await _userRepository.GetUserDataByCondition( u => u.Id == response.CreatedBy );
          response.CreatedName = user!.FullName;

          var teamUser = user.TeamUsers;
          foreach ( var teamU in teamUser ) {
            response.Teams.Add( new TeamUserOptionResponse
            {
              TeamId = teamU.Team.Id,
              TeamCode = teamU.Team.Code,
              TeamName = teamU.Team.Name
            } );
          }
          users.Add( user.Id, user );
        }
      }
      return responses;
    }
    #endregion

    #region BaseServiceResource
    public async Task<long> Create( WfhResource resource, long creator )
    {
      var request = _mapper.Map<WfhRequest>( resource );
      // TODO
      request.CreatedBy = creator;
      request.ModifiedBy = creator;
      // create
      await _wfhRepository.CreateAsync( request );
      // tạo noti yêu cầu approve/reject request
      await _notificationService.Create( ( int ) Enums.NotificationObjectType.WfhRequest, request );
      return request.Id;
    }

    public async Task Update( WfhResource resource, long editor )
    {
      var old = await _wfhRepository.GetByCondition( o => o.Id == resource.Id );
      _mapper.Map( resource, old! );
      old!.Status = ( int ) Enums.WfhRequestStatus.Pending;
      old.Reviewer = null;
      old.ModifiedBy = editor;
      _wfhRepository.Update( old );
      // assign notification
      await _notificationService.ChangeUserAssign( ( int ) Enums.NotificationObjectType.WfhRequest, old.Id, ( int ) Enums.NotificationType.Information, old.CreatedBy );
    }
    #endregion

    #region others
    public async Task Approve( long id, bool isApprove, long reviewer )
    {
      var old = await _wfhRepository.GetByCondition( o => o.Id == id );
      var nextAction = 0;
      if ( old!.Status == ( int ) Enums.LeaveRequestStatus.Pending && isApprove ) {
        nextAction = 1;
      }
      else if ( old.Status == ( int ) Enums.LeaveRequestStatus.Approve && !isApprove ) {
        nextAction = 2;
      }
      old!.Status = ( int ) ( isApprove ? Enums.WfhRequestStatus.Approve : Enums.WfhRequestStatus.Reject );
      old.Reviewer = reviewer;
      old.ModifiedBy = reviewer;
      _wfhRepository.Update( old );
      if ( nextAction == 1 ) {
        // update timesheet with date WFH
        await _timesheetService.ApplyWfhRequestApproved( old );
      }
      // approve to reject: remove timesheet
      if ( nextAction == 2 ) {
        await _timesheetService.RemoveWfhRequestReject( old );
      }
      // assign notification
      await _notificationService.ChangeUserAssign( ( int ) Enums.NotificationObjectType.WfhRequest, id,
        isApprove ? ( int ) Enums.NotificationType.Approved : ( int ) Enums.NotificationType.Rejected, old.CreatedBy );
    }
    #endregion

    private int CountDays( DateTime start, DateTime end )
    {
      var count = 0;
      for ( var i = start; i <= end; i = i.AddDays( 1 ) ) {
        if ( i.DayOfWeek != DayOfWeek.Sunday ) {
          count++;
        }

      }
      return count;
    }
  }
}
