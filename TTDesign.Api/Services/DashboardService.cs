using AutoMapper;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class DashboardService : IDashboardService
  {
    private readonly IProjectRepository _projectRepository;
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ITimesheetDetailRepository _timesheetDetailRepository;
    private readonly IConfigRepository _configRepository;
    private readonly IMapper _mapper;
    private readonly ITeamCategoryRepository _categoryRepository;
    public DashboardService( IProjectRepository projectRepository, ITimesheetRepository timesheetRepository,
      IUserRepository userRepository, ITeamRepository teamRepository, ITimesheetDetailRepository timesheetDetailRepository,
      IConfigRepository configRepository, IMapper mapper, ITeamCategoryRepository categoryRepository )
    {
      _projectRepository = projectRepository;
      _timesheetRepository = timesheetRepository;
      _userRepository = userRepository;
      _teamRepository = teamRepository;
      _timesheetDetailRepository = timesheetDetailRepository;
      _configRepository = configRepository;
      _mapper = mapper;
      _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dashboard"></param>
    /// <returns></returns>
    public async Task<DashboardSummary> GetSummary( DashboardResource dashboard )
    {
      var result = new DashboardSummary();
      var users = await _userRepository.GetListByConditionTrack( x => x.IsActive );
      var projects = await _projectRepository.GetAll();
      var projectActives = projects.Where( x => x.Status == 1 );
      var projectEnds = projects.Where( x => x.Status == 1 );
      if ( dashboard.TeamId is not null ) {
        users = await _userRepository.GetUsersDataByCondition( dashboard.TeamId );
        projects = projects.Where( x => x.TeamId == dashboard.TeamId );
        projectActives = projects.Where( x => x.Status == 1);
        projectEnds = projects.Where( x => x.Status == 1 );
      }
      result.TotalEmployee = users.Count();
      result.TotalProject = projects.Count();
      result.TotalActiveProject = projectActives.Count();
      result.TotalCompletedProject = projectEnds.Count();
      return result;
    }
    public async Task<DashboardTimeTracking> GetDashboardTimeTracking( DashboardResource dashboard )
    {
      var result = new DashboardTimeTracking();
      var timesheets = await _timesheetRepository.GetTimesheetDetails( t => t.Date.Year == dashboard.DateCheck.Year && t.Date.Month == dashboard.DateCheck.Month );
      var dateNow = DateTime.UtcNow;
      if ( dashboard.DateCheck.Month == dateNow.Month && dashboard.DateCheck.Year == dateNow.Year ) {
        timesheets = timesheets.Where( x => x.Date.Date <= dateNow.Date );
      }
      else {
        dateNow = new DateTime( dashboard.DateCheck.Year, dashboard.DateCheck.Month, dashboard.DateCheck.Day );
      }
      if ( dashboard.TeamId is not null ) {
        timesheets = timesheets.Where( x => x.User.TeamUsers.Any( y => y.TeamId == dashboard.TeamId ) );
      }

      if ( true ) {
        var timesheetNow = timesheets.Where( x => x.Date.Date == dateNow.Date );
        var tsDetailNow = timesheetNow.SelectMany( x => x.TimesheetDetails );

        var tsLate = timesheetNow.Select( x => x.FingerPrinter ).Where( x => x.DateIn.Hour >= 8 && x.DateIn.Minute > 0 );
        var tsLeave = tsDetailNow.Where( x => x.Type == ( int ) Enums.TimesheetDetailType.PaidLeave );

        result.TotalLateDay = tsLate.Count();
        result.TotalLeaveDay = tsLeave.Count();
        foreach ( var tsl in tsLate ) {
          var ts = await _timesheetRepository.GetByConditionNoTrack( x => x.Id == tsl.TimesheetId );
          if ( ts is null )
            continue;
          var user = await _userRepository.GetByConditionNoTrack( x => x.Id == ts!.UserId );
          var userLate = new UserLate
          {
            Id = user!.Id,
            FullName = user.FullName,
            Number = 1,
            Position = Enum.GetName( ( Enums.UserPosition ) user.Position ) ?? string.Empty
          };
          result.UserLatesToday.Add( userLate );
        }
        foreach ( var tsl in tsLeave ) {
          var ts = await _timesheetRepository.GetByConditionNoTrack( x => x.Id == tsl.TimesheetId );
          if ( ts is null )
            continue;
          var user = await _userRepository.GetByConditionNoTrack( x => x.Id == ts!.UserId );
          var userLeave = new UserLeave
          {
            Id = user!.Id,
            FullName = user.FullName,
            Days = 1,
            Position = Enum.GetName( ( Enums.UserPosition ) user.Position ) ?? string.Empty
          };
          result.UserLeavesToday.Add( userLeave );
        }
      }

      var tsDetail = timesheets.SelectMany( t => t.TimesheetDetails ).GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.ToList() );
      var fgLateMonth = timesheets.Count() > 0 ? timesheets.Select( x => x.FingerPrinter ).Where( x => x.DateIn.Hour >= 8 && x.DateIn.Minute > 0 ) : new List<FingerPrinter>();
      var tsLeaveMonth = tsDetail.ContainsKey( ( int ) Enums.TimesheetDetailType.PaidLeave ) ? tsDetail [ ( int ) Enums.TimesheetDetailType.PaidLeave ].ToList() : new List<TimesheetDetail>();

      var totalWorkLogs = await _configRepository.GetByCondition( x => x.Description == DateTime.UtcNow.Month.ToString() );
      result.TotalLateMonth = fgLateMonth.Count();
      result.TotalLeaveMonth = tsLeaveMonth!.DistinctBy( x => x.TimesheetId ).Count();
      result.TotalWorkingLog = Common.NetworkDays( new DateTime( dashboard.DateCheck.Year, dashboard.DateCheck.Month, 1 ), dateNow );
      if ( fgLateMonth.Count() > 0 ) {
        foreach ( var fg in fgLateMonth ) {
          var ts = await _timesheetRepository.GetByConditionNoTrack( x => x.Id == fg.TimesheetId );
          if ( ts is null )
            continue;
          var user = await _userRepository.GetByConditionNoTrack( x => x.Id == ts!.UserId );
          var userLate = new UserLate
          {
            Id = user!.Id,
            FullName = user.FullName,
            Number = 1,
            Position = Enum.GetName( ( Enums.UserPosition ) user.Position ) ?? string.Empty
          };
          if ( result.UserLates.Any( x => x.Id == userLate.Id ) ) {
            var oldUserLate = result.UserLates.Where( u => u.Id == userLate.Id ).First();
            userLate.Number = oldUserLate.Number + 1;
            result.UserLates.Remove( oldUserLate );
          }
          result.UserLates.Add( userLate );
        }
      }

      if ( tsLeaveMonth.Count() > 0 ) {
        foreach ( var tsl in tsLeaveMonth ) {
          var ts = await _timesheetRepository.GetByConditionNoTrack( x => x.Id == tsl.TimesheetId );
          if ( ts is null )
            continue;
          var user = await _userRepository.GetByConditionNoTrack( x => x.Id == ts!.UserId );
          var userLeave = new UserLeave
          {
            Id = user!.Id,
            FullName = user.FullName,
            Days = ( double ) tsl.Hours / 8,
            Position = Enum.GetName( ( Enums.UserPosition ) user.Position ) ?? string.Empty
          };
          if ( result.UserLeaves.Any( x => x.Id == userLeave.Id ) ) {
            var oldUserLeave = result.UserLeaves.Where( u => u.Id == userLeave.Id ).First();
            userLeave.Days += oldUserLeave.Days;
            result.UserLeaves.Remove( oldUserLeave );
          }
          result.UserLeaves.Add( userLeave );
        }
      }
      var users = await _userRepository.GetListByConditionTrack( x => x.IsActive );
      if ( dashboard.TeamId is not null ) {
        users = await _userRepository.GetUsersDataByCondition( dashboard.TeamId );
      }
      result.TotalEmps = users.Count();
      var config = ( totalWorkLogs == null || totalWorkLogs.Code == "0" ) ?
                               Common.NetworkDays( new DateTime( dashboard.DateCheck.Year, dashboard.DateCheck.Month, 1 ),
                                new DateTime( dashboard.DateCheck.Year, dashboard.DateCheck.Month,
                                DateTime.DaysInMonth( dashboard.DateCheck.Year, dashboard.DateCheck.Month ) ) )
                                : double.Parse( totalWorkLogs!.Code );
      result.TotalWorkLogs = config;
      return result;
    }

    public async Task<IEnumerable<ProjectResponse>> GetListView( DashboardResource dashboard, BaseFilter filter )
    {
      var projects = await _projectRepository.GetProjects( ( long ) filter.UserId!, ( int ) filter.Position!, dashboard.TeamId );
      var projectResponse = _mapper.Map<IEnumerable<ProjectResponse>>( projects.OrderByDescending( p => p.Code ) );
      var teams = await _teamRepository.GetOption( dashboard.TeamId );
      foreach ( var project in projectResponse ) {
        // set team info
        var team = teams.FirstOrDefault( t => t.Id == project.TeamId );
        if ( team is not null ) {
          project.TeamCode = team.Code;
          project.TeamName = team.Name;
        }
        var tsDetails = await _timesheetDetailRepository.GetListByCondition( t => t.ProjectId == project.Id &&
            t.Type == ( int ) Enums.TimesheetDetailType.Project );
        project.CurrentDay = tsDetails.Count();
        project.QuotationDay = project.QuotationHour > 0 ? ( long ) Math.Ceiling( ( double ) project.QuotationHour / 8 ) : 0;
        project.TotalHour = tsDetails.Sum( x => x.Hours );
        var projectType = await _configRepository.GetByCondition( c => c.Code == project.Type );
        project.ConfigName = projectType != null ? projectType!.Name : "";
      }
      return projectResponse;
    }

    public async Task<List<GetActiveProjectHoursResponse>> GetAnalysisDetailView( DashboardResource dashboard )
    {
      var projects = await _projectRepository.GetListByCondition( x => x.Status == 1 );
      var result = new List<GetActiveProjectHoursResponse>();
      foreach ( var pr in projects ) {

        var projectId = pr.Id;
        var tsDetails = await _timesheetDetailRepository.GetListByCondition( t => t.ProjectId == projectId &&
          t.Type != ( int ) Enums.TimesheetDetailType.PaidLeave && t.Type != ( int ) Enums.TimesheetDetailType.UnpaidLeave );

        var project = await _projectRepository.GetProjectDocument( projectId );
        if ( project == null )
          continue;
        var totalWorkingHours = tsDetails.Where( t => t.Type == ( int ) Enums.TimesheetDetailType.Project ).Sum( t => t.Hours );

        var categories = new HashSet<ProjectObjectDetail>();

        foreach ( var category in tsDetails.GroupBy( t => ( long ) t.TimesheetCategoryId! ).ToDictionary( k => k.Key, v => v.Sum( t => t.Hours ) ) ) {
          var cate = await _categoryRepository.GetByConditionNoTrack( c => c.Id == category.Key );
          if ( cate != null ) {
            categories.Add( new ProjectObjectDetail() { Name = cate.Name, WorkingHours = category.Value } );
          }
        }
        result.Add( new GetActiveProjectHoursResponse
        {
          Id = project.Id,
          ProjectName = project.Name,
          TotalHour = totalWorkingHours,
          Categories = categories.ToArray()
        } );
      }
      return result;
    }

  }

}
