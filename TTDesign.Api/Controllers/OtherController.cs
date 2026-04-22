using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;

namespace TTDesign.API.Controllers
{
  /// <summary>
  /// APIs cung cấp các thông tin/option setup của hệ thống
  /// </summary>
  [ApiController]
  [Authorize]
  public class OtherController : ControllerBase
  {
    private readonly IOtherService _otherService;

    public OtherController( IOtherService otherService )
    {
      _otherService = otherService;
    }

    /// <summary>
    /// [Option] Danh sách option status
    /// </summary>
    /// <remarks>
    /// sử dụng cho TH:
    /// - màn hình admin, danh sách user
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route( "api/GetOptionStatus" )]
    [Authorize( Policy = Policies.OTHER_GET_OPTION_STATUS )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( List<string> ) )]
    public IActionResult GetOptionStatus()
    {
      return Ok( Enum.GetNames( typeof( Enums.Status ) ).ToList() );
    }

    /// <summary>
    /// [Option] Danh sách option Client của Project
    /// </summary>
    /// <remarks>
    /// sử dụng trong TH:
    /// - màn hình project, create/update project
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route( "api/GetOptionClient" )]
    [Authorize( Policy = Policies.OTHER_GET_OPTION_CLIENT )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( List<Client> ) )]
    public async Task<IActionResult> GetOptionClient()
    {
      return Ok( await _otherService.GetClients() );
    }

    /// <summary>
    /// [Option] Danh sách option Status của Project
    /// </summary>
    /// <remarks>
    /// sử dụng trong TH:
    /// - màn hình project, create/update project
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route( "api/GetOptionStatusProject" )]
    [Authorize( Policy = Policies.OTHER_GET_OPTION_STATUS_PROJECT )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( List<string> ) )]
    public IActionResult GetOptionStatusProject()
    {
      return Ok( Enum.GetNames( typeof( Enums.ProjectStatus ) ).ToList() );
    }

    /// <summary>
    /// [Option] Get new Project Number
    /// </summary>
    /// <remarks>
    /// sử dụng trong TH:
    /// - màn hình project, create project
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route( "api/GetNewProjectNumber" )]
    [Authorize( Policy = Policies.OTHER_GET_NEW_PROJECT_NUMBER )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( int ) )]
    public async Task<IActionResult> GetNewProjectNumber()
    {
      return Ok( await _otherService.GetNewProjectNumber() );
    }

    /// <summary>
    /// [Option] Lấy năm tài chính hiện tại
    /// </summary>
    /// <remarks>
    /// sử dụng trong TH:
    /// - màn hình project, create project
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route( "api/GetFiscalNumber" )]
    [Authorize( Policy = Policies.OTHER_GET_FISCAL_NUMBER )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( int ) )]
    public IActionResult GetFiscalNumber()
    {
      return Ok( Common.GetFiscalYear() );
    }
  }
}
