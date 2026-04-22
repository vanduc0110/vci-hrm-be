using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class HolidayService : GenericService<Holiday>, IHolidayService
  {
    private readonly IHolidayRepository _holidayRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ILogger<HolidayService> _logger;
    private readonly IMapper _mapper;
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly ITeamUserRepository _teamUserRepository;
    private readonly AppDbContext _context;

    public HolidayService( IHolidayRepository holidayRepository, IUserRepository userRepository, ITeamRepository teamRepository,
    ILogger<HolidayService> logger,
      IMapper mapper, ITimesheetRepository timesheetRepository, ITeamUserRepository teamUserRepository, AppDbContext context ) : base( holidayRepository )
    {
      _holidayRepository = holidayRepository;
      _logger = logger;
      _mapper = mapper;
      _userRepository = userRepository;
      _teamRepository = teamRepository;
      _timesheetRepository = timesheetRepository;
      _teamUserRepository = teamUserRepository;
      _context = context;
    }

    #region BaseServiceList
    public async Task<IEnumerable<HolidayResponse>> GetList( BaseFilter filter )
    {
      var holidays = await _holidayRepository.GetDataByCondition( h => h.StartDate.Year == filter.Year );
      var responses = _mapper.Map<IEnumerable<HolidayResponse>>( holidays.OrderBy( h => h.StartDate ) );
      var Users = new Dictionary<long, string>();
      var Teams = new Dictionary<long, string>();
      foreach ( var response in responses ) {
        var record = holidays.Where( h => h.Id == response.Id ).FirstOrDefault();
        HashSet<string> applyName = new HashSet<string>();
        foreach ( var obj in record!.HolidayApplys ) {
          switch ( obj.Type ) {
            case "User":
              if ( Users.ContainsKey( obj.ApplyId ) ) {
                applyName.Append( Users [ obj.ApplyId ] );
              }
              else {
                var user = await _userRepository.GetByConditionNoTrack( u => u.Id == obj.ApplyId );
                applyName.Add( user!.UserName );
                Users.Add( user.Id, user.UserName );
              }
              break;
            case "Team":
              if ( Teams.ContainsKey( obj.ApplyId ) ) {
                applyName.Append( Teams [ obj.ApplyId ] );
              }
              else {
                var team = await _teamRepository.GetByConditionNoTrack( u => u.Id == obj.ApplyId );
                applyName.Add( team!.Code );
                Teams.Add( team.Id, team.Code );
              }
              break;
          }
        }
        response.ApplyFor = string.Join( ", ", applyName );
      }
      return responses;
    }
    #endregion

    #region BaseServiceResource
    public async Task<long> Create( HolidayResource resource, long creator )
    {
      var holiday = _mapper.Map<Holiday>( resource );
      holiday.CreatedBy = creator;
      holiday.ModifiedBy = creator;
      holiday.Status = ( int ) Enums.HolidayStatus.Apply;
      await _holidayRepository.CreateAsync( holiday );

      var ts = await _timesheetRepository.GetAll();

      if ( holiday.Type == ( int ) Enums.HolidayType.Holiday ) {
        ts = ts.Where( t => t.Date >= holiday.StartDate && t.Date <= holiday.EndDate ).ToList();
        if ( ts.Count() > 0 ) {
          foreach ( var t in ts ) {
            t.HolidayName = holiday.Name;
          }
          _timesheetRepository.Updates( ts.ToArray() );
        }
      }
      else if ( holiday.Type == ( int ) Enums.HolidayType.Special ) {
        var userIds = new long [] { };
        foreach ( var apply in holiday.HolidayApplys ) {
          if ( apply.Type == Enum.GetName( Enums.DynamicOption.Team ) ) {
            var ids = await _context.TeamUsers.Include( t => t.User ).Where( t => t.TeamId == apply.ApplyId && t.User.IsActive )
              .Select( t => t.UserId ).ToArrayAsync();
            userIds = userIds.Union( ids ).ToArray();
          }
          else if ( apply.Type == Enum.GetName( Enums.DynamicOption.User ) && !userIds.Contains( apply.ApplyId ) ) {
            userIds.Append( apply.ApplyId );
          }
        }
        ts = ts.Where( t => t.Date >= holiday.StartDate && t.Date <= holiday.EndDate && userIds.Contains( t.UserId ) ).ToList();
        if ( ts.Count() > 0 ) {
          foreach ( var t in ts ) {
            t.HolidayName = holiday.Name;
          }
          _context.Timesheets.UpdateRange( ts );
        }
      }
      return holiday.Id;
    }

    public async Task Update( HolidayResource resource, long editor )
    {
      var record = await _holidayRepository.GetByCondition( h => h.Id == resource.Id );
      if ( record!.Status == ( int ) Enums.HolidayStatus.Apply ) {
        record!.Status = ( int ) Enums.HolidayStatus.Deleting;
        record.ModifiedBy = editor;
        _holidayRepository.Update( record );
      }
      else {
        _holidayRepository.Delete( record );
      }
    }
    #endregion

    #region other
    public async Task<IEnumerable<HolidayResponse>> GetListInfor( BaseFilter filter )
    {
      var holidays = await _holidayRepository.GetDataByCondition( h => h.StartDate.Year == filter.Year && h.Status == ( int ) Enums.HolidayStatus.Apply );
      holidays = holidays.Where( h => h.HolidayApplys == null || h.HolidayApplys.Count() == 0 ||
        ( filter.TeamId == 0 || h.HolidayApplys.Any( o => ( o.Type == "User" && o.ApplyId == filter.UserId ) || ( o.Type == "Team" && o.ApplyId == filter.TeamId ) ) ) ).ToList();
      var responses = _mapper.Map<IEnumerable<HolidayResponse>>( holidays.OrderBy( h => h.StartDate ) );
      return responses;
    }
    #endregion
  }
}
