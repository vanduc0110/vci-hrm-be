using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class DashboardController : ControllerBase
  {
    private readonly IDashboardService _dashboardService;
    private readonly IProjectService _projectService;
    public DashboardController( IDashboardService dashboardService, IProjectService projectService )
    {
      _dashboardService = dashboardService;
      _projectService = projectService;
    }
    [HttpPost( "GetSummary" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( DashboardSummary ) )]
    public async Task<DashboardSummary> GetListView( [FromBody] DashboardResource dashboard )
    {
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      if ( teamUserLogin.Any( x => x == Enums.TEAM_HR ) && dashboard.TeamId == null ) {
        dashboard.TeamId = null;
      }
      return await _dashboardService.GetSummary( dashboard );
    }
    [HttpPost( "GetDashboardTimeTracking" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( DashboardTimeTracking ) )]
    public async Task<DashboardTimeTracking> GetDashboardTimeTracking( [FromBody] DashboardResource dashboard )
    {
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      if ( teamUserLogin.Any( x => x == Enums.TEAM_HR && dashboard.TeamId == null ) ) {
        dashboard.TeamId = null;
      }
      return await _dashboardService.GetDashboardTimeTracking( dashboard );
    }

    [HttpPost( "GetListView" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ProjectResponse> ) )]
    public async Task<IEnumerable<ProjectResponse>> GetListProjectView( [FromBody] DashboardResource dashboard )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      if ( teamUserLogin.Any( x => x == Enums.TEAM_HR ) && dashboard.TeamId == 1 ) {
        dashboard.TeamId = null;
      }
      return await _projectService.GetList( new BaseFilter()
      {
        Position = positionUserLogin,
        UserId = userLogin,
        TeamId = dashboard.TeamId
      } );
    }
    [HttpPost( "GetTotalList" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( List<GetActiveProjectHoursResponse> ) )]
    public async Task<IActionResult> GetTotalProject( [FromBody] DashboardResource dashboard )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogins = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUserLogins.Split( "," );
      var teamUserLogin = parts.Select( long.Parse ).ToArray();
      if ( teamUserLogin.Any( x => x == Enums.TEAM_HR ) && dashboard.TeamId == 1 ) {
        dashboard.TeamId = null;
      }
      return Ok( await _dashboardService.GetAnalysisDetailView( dashboard ) );
    }
  }
}
