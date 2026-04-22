using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  /// <summary>
  /// APIs của Group Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class NotificationController : ControllerBase
  {
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController( INotificationService notificationService,
      ILogger<NotificationController> logger )
    {
      _notificationService = notificationService;
      _logger = logger;
    }

    /// <summary>
    /// [Personal - List] Lấy danh sách notification
    /// </summary>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView" )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<NotificationResponse> ) )]
    public async Task<IEnumerable<NotificationResponse>> GetListView()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return await _notificationService.GetList( new BaseFilter()
      {
        UserId = userLogin,
      } );
    }

    /// <summary>
    /// [Personal - Other] đánh dấu đã đọc tất cả
    /// </summary>
    /// <returns>không trả về gì</returns>
    [HttpPut( "MarkReadAll" )]
    public async Task MarkReadAll()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _notificationService.MarkReadAll( userLogin );
    }

    /// <summary>
    /// [Personal - Other] xóa tất cả notification
    /// </summary>
    /// <returns>không trả về gì</returns>
    [HttpPut( "DeleteAll" )]
    public async Task DeleteAll()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _notificationService.DeleteAll( userLogin );
    }

    /// <summary>
    /// [Personal - Other] đánh dấu đã đọc cho notification chỉ định
    /// </summary>
    /// <param name="id" example="1">id notification</param>
    /// <returns>không trả về gì</returns>
    [HttpPut( "MarkRead/{id}" )]
    public async Task GetDetail(long id)
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _notificationService.MarkRead( id, userLogin );
    }

    /// <summary>
    /// [Personal - Other] xóa notification chỉ định
    /// </summary>
    /// <param name="id" example="1">id notification</param>
    /// <returns>không trả về gì</returns>
    [HttpPut( "Delete/{id}" )]
    public async Task Delete(long id)
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _notificationService.Delete( id, userLogin );
    }
  }
}
