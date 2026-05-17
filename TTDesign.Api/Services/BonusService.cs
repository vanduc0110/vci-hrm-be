using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class BonusService : GenericService<Bonus>, IBonusService
  {
    private readonly IBonusRepository _bonusRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<BonusService> _logger;

    public BonusService( IBonusRepository bonusRepository, IUserRepository userRepository,
      INotificationService notificationService, ILogger<BonusService> logger ) : base( bonusRepository )
    {
      _bonusRepository = bonusRepository;
      _userRepository = userRepository;
      _notificationService = notificationService;
      _logger = logger;
    }

    public async Task<IEnumerable<BonusResponse>> GetList( int month, int year, long[]? allowedUserIds = null )
    {
      var bonuses = await _bonusRepository.GetListWithUser( month, year );
      if ( allowedUserIds != null && allowedUserIds.Length > 0 )
        bonuses = bonuses.Where( b => allowedUserIds.Contains( b.UserId ) );
      return bonuses;
    }

    public async Task<long> Create( BonusResource resource, long creator )
    {
      var bonus = new Bonus
      {
        UserId = resource.UserId,
        Month = resource.Month,
        Year = resource.Year,
        Amount = resource.Amount,
        Reason = resource.Reason,
        CreatedBy = creator,
        CreatedDate = DateTime.UtcNow,
      };
      await _bonusRepository.CreateAsync( bonus );

      try {
        var admins = await _userRepository.GetListByCondition( u => u.IsActive &&
          ( u.Position == (int)Enums.UserPosition.System || u.Position == (int)Enums.UserPosition.Director ) );
        var adminIds = string.Join( ",", admins.Select( u => u.Id ) );
        if ( !string.IsNullOrEmpty( adminIds ) ) {
          var creatorUser = await _userRepository.GetByConditionNoTrack( u => u.Id == creator );
          var employee = await _userRepository.GetByConditionNoTrack( u => u.Id == resource.UserId );
          var kind = resource.Amount >= 0 ? "thưởng" : "phạt";
          await _notificationService.Create( 0, new Dictionary<string, string>
          {
            { "Title", $"Thưởng/phạt chờ phê duyệt" },
            { "Content", $"{creatorUser?.FullName ?? "HR"} vừa thêm khoản {kind} cho {employee?.FullName ?? "nhân viên"} tháng {resource.Month}/{resource.Year}, cần phê duyệt." },
            { "ObjectId", bonus.Id.ToString() },
            { "CreatedBy", creator.ToString() },
            { "UserName", creatorUser?.FullName ?? "" },
            { "To", adminIds },
          } );
        }
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi tạo thưởng/phạt {Id}", bonus.Id );
      }

      return bonus.Id;
    }

    public async Task Approve( long id, long approvedBy )
    {
      var bonus = await _bonusRepository.GetByCondition( b => b.Id == id );
      if ( bonus == null ) throw new Exception( "Không tìm thấy thưởng/phạt" );
      bonus.ApprovedBy = approvedBy;
      bonus.ApprovedDate = DateTime.UtcNow;
      bonus.ModifiedBy = approvedBy;
      bonus.ModifiedDate = DateTime.UtcNow;
      _bonusRepository.Update( bonus );

      try {
        var approver = await _userRepository.GetByConditionNoTrack( u => u.Id == approvedBy );
        var employee = await _userRepository.GetByConditionNoTrack( u => u.Id == bonus.UserId );
        var kind = bonus.Amount >= 0 ? "thưởng" : "phạt";
        await _notificationService.Create( 0, new Dictionary<string, string>
        {
          { "Title", $"Thưởng/phạt đã được phê duyệt" },
          { "Content", $"{approver?.FullName ?? "Admin"} đã phê duyệt khoản {kind} cho {employee?.FullName ?? "nhân viên"}." },
          { "ObjectId", bonus.Id.ToString() },
          { "CreatedBy", approvedBy.ToString() },
          { "UserName", approver?.FullName ?? "" },
          { "To", bonus.CreatedBy.ToString() },
        } );
      }
      catch ( Exception ex ) {
        _logger.LogError( ex, "Lỗi gửi notification khi duyệt thưởng/phạt {Id}", id );
      }
    }

    public async Task Delete( long id )
    {
      var bonus = await _bonusRepository.GetByCondition( b => b.Id == id );
      if ( bonus == null ) throw new Exception( "Không tìm thấy thưởng/phạt" );
      _bonusRepository.Delete( bonus );
    }
  }
}
