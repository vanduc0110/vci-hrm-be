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
/// APIs của UserTask Controller
/// </summary>
[Route( "api/[controller]" )]
[ApiController]
[Authorize]
[Consumes( "application/json" )]
[Produces( "application/json" )]
public class UserTaskController : ControllerBase
{
  private readonly IUserTaskService _userTaskService;
  public UserTaskController( IUserTaskService UserTaskService )
  {
    _userTaskService = UserTaskService;
  }
  /// <summary>
  /// [Personal - List] lấy danh sách UserTask kèm thông tin
  /// </summary>
  /// <returns></returns>
  [HttpGet( "GetListView/{year}/{month}" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<UserTaskResponse> ) )]
  public async Task<IActionResult> GetListView( int year, int month )
  {
    DateTime datetimeTry;
    if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
      return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
    }
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    return Ok( await _userTaskService.GetList( new BaseFilter() { DateCheck = datetimeTry, UserId = userLogin } ) );
  }
  /// <summary>
  /// [Personal - List] lấy danh sách UserTask hiện tại
  /// </summary>
  /// <returns></returns>
  [HttpGet( "GetOption/{year}/{month}" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( UserTaskResponse ) )]
  public async Task<IActionResult> GetTaskCurrentView( int year, int month )
  {
    DateTime datetimeTry;
    if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
      return BadRequest( string.Format( ErrorMessageResource.DateWrongFormat ) );
    }
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    return Ok( await _userTaskService.GetCurrentTask( datetimeTry, userLogin ) );
  }
  /// <summary>
  /// [Personal - Create] Tạo mới UserTask
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPost( "Create" )]
  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Create( [FromBody] UserTaskResource resource )
  {
    var errors = await ValidateBeforeCreate( resource );
    if ( errors.Count > 0 ) {
      foreach ( var error in errors ) {
        ModelState.AddModelError( error.Key, error.Value );
      }
    }
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );

    resource.Id = null;
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // create
    var teamId = await _userTaskService.Create( resource, userLogin );
    return Ok( teamId );
  }
  /// <summary>
  /// [Personal - Update] Cập nhật thông tin UserTask
  /// </summary>
  /// <param name="resource"></param>
  /// <returns></returns>
  [HttpPut( "Update" )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  public async Task<IActionResult> Update( [FromBody] UserTaskResource resource )
  {
    var errors = await ValidateBeforeUpdate( resource );
    if ( errors.Count > 0 ) {
      foreach ( var error in errors ) {
        ModelState.AddModelError( error.Key, error.Value );
      }
    }
    if ( !ModelState.IsValid )
      return BadRequest( ModelState );
    if ( resource.Id is null ) {
      return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Team ) );
    }
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    // update
    await _userTaskService.Update( resource, userLogin );
    return Ok();
  }
  /// <summary>
  /// [Personal - Delete] Xóa UserTask
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpDelete( "Delete/{id}" )]
  [SwaggerResponse( StatusCodes.Status200OK )]
  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  [SwaggerResponse( StatusCodes.Status404NotFound )]
  public async Task<IActionResult> Delete( long id )
  {
    // check UserTask id
    if ( !await _userTaskService.Exist( t => t.Id == id ) ) {
      return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.UserTask ) );
    }
    // check UserTask 
    if ( !await _userTaskService.CheckUserTaskBeforeDelete( id ) ) {
      return BadRequest( string.Format( ErrorMessageResource.ErrorDeleteTask, DisplayNameResource.UserTask ) );
    }
    // delete
    await _userTaskService.DeleteByCondition( t => t.Id == id );
    return Ok();
  }
  private async Task<Dictionary<string, string>> ValidateBeforeUpdate( UserTaskResource resource )
  {
    var errors = new Dictionary<string, string>();
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    var userTasks = await _userTaskService.GetAll();
    if ( userTasks.Any( x => x.CreatedBy == userLogin && x.Id != resource.Id && x.Status == Enums.StatusMark.Current && resource.Status == Enums.StatusMark.Current.ToString() ) ) {
      errors.Add( nameof( UserTaskResource.Status ), string.Format( ErrorMessageResource.ChangeStatusError, DisplayNameResource.UserTask ) );
    }
    return errors;
  }
  private async Task<Dictionary<string, string>> ValidateBeforeCreate( UserTaskResource resource )
  {
    var errors = new Dictionary<string, string>();
    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
    var userTasks = await _userTaskService.GetAll();
    if ( userTasks.Any( x => x.CreatedBy == userLogin && x.Status == Enums.StatusMark.Current && resource.Status == Enums.StatusMark.Current.ToString() ) ) {
      errors.Add( nameof( UserTaskResource.Status ), string.Format( ErrorMessageResource.UserTaskStatus, DisplayNameResource.UserTask ) );
    }
    return errors;
  }
}
