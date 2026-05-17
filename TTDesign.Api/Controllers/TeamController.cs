using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  /// <summary>
  /// APIs của Team Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class TeamController : ControllerBase
  {
    private readonly ITeamService _teamService;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public TeamController( ITeamService teamService, IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _teamService = teamService;
      _apiBehaviorOptions = apiBehaviorOptions;
    }

    /// <summary>
    /// [Admin - Option] Lấy danh sách thu gọn Team
    /// </summary>
    /// <remarks>
    /// - Danh sách trả về phụ thuộc vào position của User Login
    /// - Dùng cho các TH:
    ///     - option filter view List Admin User
    ///     - option filter view List Admin object
    ///     - option filter view List Admin category
    ///     - option filter view List Admin project
    ///     - option filter view List Admin analysis report
    ///     - option filter view List Admin overtime
    ///     - option filter view List Admin overtime summary
    ///     - option filter view List Admin leave
    ///     - option filter view List Admin leave summary
    ///     - option filter view List Admin wfh
    ///     - option filter view List Admin asset
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Danh sách option</response>
    [HttpGet( "GetOption" )]
    [Authorize( Policy = Policies.TEAM_GET_OPTION )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<TeamOption> ) )]
    public async Task<IEnumerable<TeamOption>> GetOption()
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )?.Value ?? "";
      var teamUserLogins = teamStr.Length > 0 ? teamStr.Split( ',' ).Select( long.Parse ).ToArray() : Array.Empty<long>();
      bool isLeadRole = positionUserLogin >= ( int ) Enums.UserPosition.TeamLead && positionUserLogin <= ( int ) Enums.UserPosition.PM;
      var result = new List<TeamOption>();
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && ( isLeadRole || !teamUserLogins.Contains( Enums.TEAM_HR ) ) ) {
        foreach ( var teamId in teamUserLogins ) {
          var res1 = await _teamService.GetOption( teamId );
          result.AddRange( res1.ToList() );
        }
        return result.GroupBy( t => t.Id ).Select( g => g.First() ).ToList();
      }
      else {
        return await _teamService.GetOption( null );
      }

    }

    /// <summary>
    /// [Admin - List] Lấy danh sách Team
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<TeamResponse> ) )]
    public async Task<IEnumerable<TeamResponse>> GetListView()
    {
      return await _teamService.GetList( new BaseFilter() );
    }

    // [Note]: Team Controller không có api Detail

    /// <summary>
    /// [Admin - Create] Tạo Team mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new Team</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.TEAM_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] TeamResource resource )
    {
      if ( !ModelState.IsValid )
        return BadRequest( ModelState );
      resource.Id = null;
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // create
      var teamId = await _teamService.Create( resource, userLogin );
      return Ok( teamId );
    }

    /// <summary>
    /// [Admin - Update] Cập nhật thông tin Team
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - ID không tồn tại
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPut( "Update" )]
    [Authorize( Policy = Policies.TEAM_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] TeamResource resource )
    {
      if ( !ModelState.IsValid )
        return BadRequest( ModelState );
      if ( resource.Id is null ) {
        return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Team ) );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // update
      await _teamService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Admin - Delete] Xóa Team
    /// </summary>
    /// <param name="id" example="1">Team ID muốn xóa</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Team vẫn còn member
    /// </response>
    /// <response code="404">
    /// Lỗi khi valid resource
    /// - ID không tồn tại
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.TEAM_DELETE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    [SwaggerResponse( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> Delete( long id )
    {
      // check team id
      if ( !await _teamService.Exist( t => t.Id == id ) ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Team ) );
      }
      // check user belong team
      if ( !await _teamService.CheckTeamBeforeDelete( id ) ) {
        return BadRequest( string.Format( ErrorMessageResource.TeamHadUserInclude, DisplayNameResource.Team ) );
      }
      // delete
      await _teamService.DeleteByCondition( t => t.Id == id );
      return Ok();
    }
  }
}
