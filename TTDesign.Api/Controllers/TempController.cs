using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;
using static Google.Rpc.Context.AttributeContext.Types;

namespace TTDesign.API.Controllers
{
  ///// <summary>
  ///// APIs của Group Controller
  ///// </summary>
  //[Route( "api/[controller]" )]
  //[ApiController]
  //[Authorize]
  //[Consumes( "application/json" )]
  //[Produces( "application/json" )]
  //public class GroupController : ControllerBase
  //{
  //  private readonly IGroupService _groupService;
  //  private readonly ILogger<GroupController> _logger;
  //  private readonly IStringLocalizer<SharedResource> _stringLocalizer;
  //  private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

  //  public GroupController( IGroupService groupService,
  //    ILogger<GroupController> logger,
  //    IStringLocalizer<SharedResource> stringLocalizer, IOptions<ApiBehaviorOptions> apiBehaviorOptions )
  //  {
  //    _groupService = groupService;
  //    _logger = logger;
  //    _stringLocalizer = stringLocalizer;
  //    _apiBehaviorOptions = apiBehaviorOptions;
  //  }

  //  /// <summary>
  //  /// [Admin - Option] Lấy danh sách thu gọn Group
  //  /// </summary>
  //  /// <remarks>
  //  /// - Group được tạo theo team, nhưng khi chọn có thể nhìn thấy của tất cả
  //  /// - Dùng cho các TH:
  //  ///     - Chỉ định group thực hiện project
  //  /// </remarks>
  //  /// <returns></returns>
  //  /// <response code="200">Danh sách option</response>
  //  [HttpGet( "GetOption" )]
  //  [Authorize( Policy = Policies.POLICY_CAN_CREATE_PROJECT )]
  //  [Authorize( Policy = Policies.POLICY_CAN_EDIT_PROJECT )]
  //  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<GroupOption> ) )]
  //  public async Task<IActionResult> GetOption()
  //  {
  //  DateTime datetimeTry;
  //      if ( !DateTime.TryParse( $"{year}/{month}/01", out datetimeTry ) ) {
  //        return BadRequest( string.Format(ErrorMessageResource.DateWrongFormat ) );
  //}
  //var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
  //    return await _groupService.GetOption();
  //  }

  //  /// <summary>
  //  /// [Admin - List] Lấy danh sách Group
  //  /// </summary>
  //  /// <remarks>
  //  /// Trong response sẽ không có danh sách member của Group
  //  /// </remarks>
  //  /// <returns></returns>
  //  /// <response code="200"></response>
  //  [HttpGet( "GetListView" )]
  //  [Authorize( Policy = Policies.POLICY_CAN_VIEW_GROUP )]
  //  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<GroupResponse> ) )]
  //  public async Task<IEnumerable<GroupResponse>> GetListView()
  //  {
  //    return await _groupService.GetList();
  //  }

  //  /// <summary>
  //  /// [Admin - Detail] Lấy thông tin chi tiết của Group
  //  /// </summary>
  //  /// <param name="id">Group ID</param>
  //  /// <returns></returns>
  //  [HttpGet( "GetDetail/{id}" )]
  //  [Authorize( Policy = Policies.POLICY_CAN_VIEW_GROUP )]
  //  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( GroupViewResponse ) )]
  //  public async Task<IActionResult> GetDetail( long id )
  //  {
  //    var resource = await _groupService.GetDetail( id );
  //    if ( resource == null ) {
  //      return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Timesheet ) );
  //    }
  //    return Ok( resource );
  //  }

  //  /// <summary>
  //  /// [Admin - Create] Tạo Group mới
  //  /// </summary>
  //  /// <param name="resource">Data resource input</param>
  //  /// <returns></returns>
  //  /// <response code="200">ID của new Group</response>
  //  /// <response code="400">
  //  /// Lỗi khi valid resource
  //  /// - Require
  //  /// - Đã tồn tại (Duplidate)
  //  /// - Vượt quá độ dài text (Maximum Length)
  //  /// </response>
  //  [HttpPost( "Create" )]
  //  [Authorize( Policy = Policies.POLICY_CAN_CREATE_GROUP )]
  //  [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
  //  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  //  public async Task<IActionResult> Create( [FromBody] GroupResource resource )
  //  {
  //    // valid
  //    if ( !ModelState.IsValid ) {
  //      return BadRequest( ModelState.GetErrorMessages() );
  //    }
  // // valid
//  var errors = await ValidateBeforeCreate( resource );
//      if (errors.Count > 0 ) {
//        foreach (var error in errors ) {
//          ModelState.AddModelError(error.Key, error.Value);
//        }
//return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      //}
  //    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
  //    // create
  //    var groupId = await _groupService.Create( resource, userLogin );
  //    return Ok( groupId );
  //  }

  //  /// <summary>
  //  /// [Admin - Update] Cập nhật thông tin Group
  //  /// </summary>
  //  /// <param name="resource">Data resource input</param>
  //  /// <returns></returns>
  //  /// <response code="400">
  //  /// Lỗi khi valid resource
  //  /// - Object ID không tồn tại
  //  /// - Require
  //  /// - Đã tồn tại (Duplidate)
  //  /// - Vượt quá độ dài text (Maximum Length)
  //  /// </response>
  //  [HttpPut( "Update" )]
  //  [Authorize( Policy = Policies.POLICY_CAN_EDIT_GROUP )]
  //  [SwaggerResponse( StatusCodes.Status200OK )]
  //  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  //  public async Task<IActionResult> Update( [FromBody] GroupResource resource )
  //  {
  //    // valid
  //    if ( !ModelState.IsValid )
  //      return BadRequest( ModelState.GetErrorMessages() );
  //    if ( resource.Id is null ) {
  //      ModelState.AddModelError( Enums.ERROR_TEXT, _stringLocalizer.GetString( LocalizationKey.GROUP_NOT_FOUND ).Value );
  //      return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
  //    }
  //    var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
  //    // update
  //    await _groupService.Update( resource, userLogin );
  //    return Ok();
  //  }

  //  /// <summary>
  //  /// [Admin - Delete] Xóa Group
  //  /// </summary>
  //  /// <param name="id">ID Group muốn xóa</param>
  //  /// <returns></returns>
  //  /// <response code="400">
  //  /// Lỗi khi valid resource
  //  /// - Object ID không tồn tại
  //  /// - Had member belong Team
  //  /// </response>
  //  [HttpDelete( "Delete/{id}" )]
  //  [Authorize( Policy = Policies.POLICY_CAN_DELETE_GROUP )]
  //  [SwaggerResponse( StatusCodes.Status400BadRequest )]
  //  public async Task<IActionResult> Delete( long id )
  //  {
  //    // valid
  //    if ( !await _groupService.Exist( g => g.Id == id ) ) {
  //      return NotFound( _stringLocalizer.GetString( LocalizationKey.GROUP_NOT_FOUND ).Value );
  //    }
  //    await _groupService.Delete( id );
  //    return Ok();
  //  }

  ///// <summary>
  ///// valid resource trước khi create
  ///// </summary>
  ///// <param name="resource"></param>
  ///// <returns></returns>
  //private async Task<Dictionary<string, string>> ValidateBeforeCreate( OvertimeRequestResource resource )
  //{
  //  var errors = new Dictionary<string, string>();
  //  return errors;
  //}

  ///// <summary>
  ///// valid resource trước khi update
  ///// </summary>
  ///// <param name="resource"></param>
  ///// <returns></returns>
  //private async Task<Dictionary<string, string>> ValidateBeforeUpdate( OvertimeRequestResource resource )
  //{
  //  var errors = new Dictionary<string, string>();
  //  return errors;
  //}

  ///// <summary>
  ///// valid object trước khi delete
  ///// </summary>
  ///// <param name="id"></param>
  ///// <returns></returns>
  //private async Task<Dictionary<string, string>> ValidateBeforeDelete( long id )
  //{
  //  var errors = new Dictionary<string, string>();
  //  return errors;
  //}
  //}
}
