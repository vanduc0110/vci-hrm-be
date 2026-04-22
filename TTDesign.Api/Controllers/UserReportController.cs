using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Languages;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers;
/// <summary>
/// APIs của UserReport Controller
/// </summary>
[Route( "api/[controller]" )]
[ApiController]
[Authorize]
[Consumes( "application/json" )]
[Produces( "application/json" )]
public class UserReportController : ControllerBase
{
  private readonly IUserReportService _userReportService;
  public UserReportController( IUserReportService userReportService )
  {
    _userReportService = userReportService;
  }
  /// <summary>
  /// lấy danh sách UserReport kèm thông tin
  /// </summary>
  /// <returns></returns>
  [HttpGet( "GetListView/{year}/{month}" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserReportResponse> ) )]
  public async Task<IActionResult> GetListView( int year, int month )
  {
    DateTime datetimeTry;
    if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
      return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
    }
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    return Ok( await _userReportService.GetList( new BaseFilter() { DateCheck = datetimeTry, UserId = userLogin } ) );
  }
  [HttpGet( "GetListView/{year}/{month}/{status}" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserReportResponse> ) )]
  public async Task<IActionResult> GetListStatusView( int year, int month, int? status = -1 )
  {
    DateTime datetimeTry;
    if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
      return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
    }
    var stt = Enum.IsDefined( typeof( Enums.ReportStatus ), status! ) ? status : -1;
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    return Ok( await _userReportService.GetList( new BaseFilter() { DateCheck = datetimeTry, UserId = userLogin, Status = stt } ) );
  }
  /// <summary>
  /// Tạo mới UserReport
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPost( "Create" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Create( [FromBody] UserReportResource resource )
  {
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    resource.Id = null;
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // create
    var teamId = await _userReportService.Create( resource, userLogin );
    return Ok( teamId );
  }
  /// <summary>
  /// Cập nhật thông tin UserReport
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPut( "Update" )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Update( [FromBody] UserReportResource resource )
  {
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    if ( resource.Id is null ) {
      return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.UserReport ) );
    }
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // update
    await _userReportService.Update( resource, userLogin );
    return Ok();
  }
  /// <summary>
  ///  Xóa UserReport
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpDelete( "Delete/{id}" )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  [SwaggerResponse( StatusCodes.Status404NotFound )]
  public async Task<IActionResult> Delete( long id )
  {
    // delete
    await _userReportService.DeleteByCondition( t => t.Id == id );
    return Ok();
  }
  [HttpPut( "UpdateStatus/{id}/{status}" )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> UpdateStatus( long id, int status )
  {
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // update
    await _userReportService.UpdateStatus( id, userLogin, status );
    return Ok();
  }
}
