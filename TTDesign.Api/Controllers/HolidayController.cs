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
  /// APIs của Holiday Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class HolidayController : ControllerBase
  {
    private readonly IHolidayService _holidayService;
    private readonly ILogger<HolidayController> _logger;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public HolidayController( IHolidayService holidayService,
      ILogger<HolidayController> logger,
      IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _holidayService = holidayService;
      _logger = logger;
      _apiBehaviorOptions = apiBehaviorOptions;
    }

    /// <summary>
    /// [Personal - List] Lấy danh sách holiday
    /// </summary>
    /// <remarks>
    /// gồm:
    /// - type holiday
    /// - type special apply cho user login
    /// </remarks>
    /// <param name="year" example="2024">năm tìm kiếm</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetInfor/{year}" )]
    [Authorize( Policy = Policies.HOLIDAY_INFOR )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<HolidayResponse> ) )]
    public async Task<IEnumerable<HolidayResponse>> GetInfor( long year )
    {
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var result = new List<HolidayResponse>();
      var teamUserLogin = ( long ) 0;
      if ( parts.Length > 1 ) {
        foreach ( var item in parts ) {
          var teamId = long.Parse( item );
          var res1 = await _holidayService.GetListInfor( new BaseFilter()
          {
            Year = year,
            UserId = userLogin,
            TeamId = !Common.ValidRoleAdmin( positionUserLogin ) ? teamId : 0,
          } );
          result.AddRange( res1.ToList() );
        }
        return result;
      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return await _holidayService.GetListInfor( new BaseFilter()
        {
          Year = year,
          UserId = userLogin,
          TeamId = !Common.ValidRoleAdmin( positionUserLogin ) ? teamUserLogin : 0,
        } );
      }
    }

    /// <summary>
    /// [Admin - List] Lấy danh sách Holiday kèm trạng thái
    /// </summary>
    /// <param name="year" example="2024">năm tìm kiếm</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView/{year}" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<HolidayResponse> ) )]
    public async Task<IEnumerable<HolidayResponse>> GetListView( long year )
    {
      return await _holidayService.GetList( new BaseFilter() { Year = year } );
    }

    /// <summary>
    /// [Admin - Create] Tạo Holiday mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new Holiday</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.HOLIDAY_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] HolidayResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // create
      var holidayId = await _holidayService.Create( resource, userLogin );
      return Ok( holidayId );
    }

    /// <summary>
    /// [Admin - Delete] Xóa Holiday
    /// </summary>
    /// <param name="id" example="1">ID Holiday muốn xóa</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// - Had member belong Team
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.HOLIDAY_DELETE )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Delete( long id )
    {
      // valid
      var errors = await ValidateBeforeDelete( id );
      if ( errors.Count > 0 ) {
        foreach ( var error in errors ) {
          ModelState.AddModelError( error.Key, error.Value );
        }
        return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory( ControllerContext );
      }
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _holidayService.Update( new HolidayResource() { Id = id }, userLogin );
      return Ok();
    }

    /// <summary>
    /// valid object trước khi delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeDelete( long id )
    {
      var errors = new Dictionary<string, string>();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // valid
      if ( !await _holidayService.Exist( g => g.Id == id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Holiday ) );
      }
      // valid status 
      else if ( await _holidayService.Exist( g => g.Id == id && g.Status == ( int ) Enums.HolidayStatus.Deleting ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Holiday ) );
      }
      return errors;
    }
  }
}
