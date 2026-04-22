using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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
  /// APIs của Project Controller
  /// </summary>
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class ProjectController : ControllerBase
  {
    private readonly IProjectService _projectService;
    private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

    public ProjectController( IProjectService projecttService, IOptions<ApiBehaviorOptions> apiBehaviorOptions )
    {
      _projectService = projecttService;
      _apiBehaviorOptions = apiBehaviorOptions;
    }

    /// <summary>
    /// [Personal - Option] Lấy danh sách thu gọn Project, theo user
    /// </summary>
    /// <remarks>
    /// - danh sách Project active mà user tham gia
    /// - dùng cho các TH:
    ///     - khai báo timesheet
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Danh sách option</response>
    [HttpGet( "GetOptionWorking" )]
    [Authorize( Policy = Policies.PROJECT_GET_OPTION_WORKING )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ProjectOption> ) )]
    public async Task<IEnumerable<ProjectOption>> GetOptionWorking()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return await _projectService.GetOptionWorking( userLogin );
    }

    /// <summary>
    /// [Admin - Option] Lấy danh sách thu gọn Project, theo user login
    /// </summary>
    /// <remarks>
    /// - danh sách Project theo team của user
    /// - dùng cho các TH:
    ///     - chọn Project Analysis
    ///     - chọn Project trên Report Timesheet
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Danh sách option</response>
    [HttpGet( "GetOption" )]
    [Authorize( Policy = Policies.PROJECT_GET_OPTION )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ProjectOption> ) )]
    public async Task<IEnumerable<ProjectOption>> GetOption()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var teamUserLogin = ( long ) 0;
      var result = new List<ProjectOption>();
      if ( parts.Length > 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var p in parts ) {
          var teamId = long.Parse( p );
          var res1 = await _projectService.GetOption( userLogin, !Common.ValidRoleAdmin( positionUserLogin ) ? teamId : null );
          result.AddRange( res1.ToList() );
        }
        return result;
      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return await _projectService.GetOption( userLogin, ( !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR ) ? teamUserLogin : null );
      }

    }

    /// <summary>
    /// [Admin - List] Lấy danh sách Project
    /// </summary>
    /// <remarks>
    /// - danh sách khác nhau phụ thuộc position của user
    /// - Trong response sẽ không có danh sách member của project
    /// </remarks>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetListView" )]
    [Authorize( Policy = Policies.PROJECT_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ProjectResponse> ) )]
    public async Task<IEnumerable<ProjectResponse>> GetListView()
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUsers = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      string [] parts = teamUsers.Split( ',' );
      var teamUserLogin = ( long ) 0;
      var result = new List<ProjectResponse>();
      if ( parts.Length > 1 && parts.Any( x => x != Enums.TEAM_HR.ToString() ) ) {
        foreach ( var p in parts ) {
          var teamId = long.Parse( p );
          var res1 = await _projectService.GetList( new BaseFilter()
          {
            TeamId = !Common.ValidRoleAdmin( positionUserLogin ) ? teamId : null,
            Position = positionUserLogin,
            UserId = userLogin
          } );
          result.AddRange( res1.ToList() );
        }
        return result;
      }
      else {
        teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
        return await _projectService.GetList( new BaseFilter()
        {
          TeamId = ( !Common.ValidRoleAdmin( positionUserLogin ) && teamUserLogin != Enums.TEAM_HR ) ? teamUserLogin : null,
          Position = positionUserLogin,
          UserId = userLogin
        } );
      }

    }

    /// <summary>
    /// [Admin - Detail] Lấy thông tin chi tiết của Project
    /// </summary>
    /// <remarks></remarks>
    /// <param name="id" example="1">Project ID</param>
    /// <param name="view" example="true">true: view detail, bao gồm danh sách memder, false: view edit, bao gồm danh sách group</param>
    /// <returns>test</returns>
    [HttpGet( "GetDetail/{id}/{view}" )]
    [Authorize( Policy = Policies.PROJECT_GET_DETAIL )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( ProjectDetailResponse ) )]
    [SwaggerResponse( StatusCodes.Status404NotFound )]
    public async Task<IActionResult> GetDetail( long id, bool view = true )
    {
      // valid: user là admin hoặc là user tham gia project và được phân đủ quyền
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && parts.Any( x => x != Enums.TEAM_HR.ToString() ) &&
        !await _projectService.Exist( p => p.Id == id && ( teamUserLogins.Contains( p.TeamId ) || EF.Functions.Like( p.ProjectManagement, $"%{userLogin}%" ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      var resource = view ? await _projectService.GetDetail( id ) : await _projectService.GetDetailForEdit( id );
      if ( resource == null ) {
        return NotFound( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Project ) );
      }
      return Ok( resource );
    }

    /// <summary>
    /// [Admin - Create] Tạo Project mới
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="200">ID của new Project</response>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPost( "Create" )]
    [Authorize( Policy = Policies.PROJECT_CREATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( long ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Create( [FromBody] ProjectResource resource )
    {
      // valid: user không là admin, chỉ được tạo project thuộc team mình quản lý
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      //var teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
      //if ( !Common.ValidRoleAdmin( positionUserLogin ) ) {
      //  resource.TeamId = teamUserLogin;
      //}
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // create
      var projectId = await _projectService.Create( resource, userLogin );
      return Ok( projectId );
    }

    /// <summary>
    /// [Admin - Update] Cập nhật thông tin Project
    /// </summary>
    /// <param name="resource">Data resource input</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// - Require
    /// - Đã tồn tại (Duplidate)
    /// - Vượt quá độ dài text (Maximum Length)
    /// </response>
    [HttpPut( "Update" )]
    [Authorize( Policy = Policies.PROJECT_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Update( [FromBody] ProjectResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      // update
      await _projectService.Update( resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// [Admin - Delete] Xóa Project
    /// </summary>
    /// <param name="id" example="1">ID project muốn xóa</param>
    /// <returns></returns>
    /// <response code="400">
    /// Lỗi khi valid resource
    /// - Object ID không tồn tại
    /// - Had member blong Project
    /// </response>
    [HttpDelete( "Delete/{id}" )]
    [Authorize( Policy = Policies.PROJECT_DELETE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
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
      // delete
      await _projectService.Delete( id );
      return Ok();
    }

    /// <summary>
    /// [Admin - Other] Lấy thông tin analysis của project
    /// </summary>
    /// <remarks>
    /// - danh sách khác nhau phụ thuộc position của user
    /// - Trong response sẽ không có danh sách member của project
    /// </remarks>
    /// <param name="projectId" example="1">ID project tìm kiếm</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "GetAnalysisView/{projectId}" )]
    [Authorize( Policy = Policies.ANALYSIS_VIEW )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( AnalysisResponse ) )]
    public async Task<IActionResult> GetAnalysisView( long projectId )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      // valid relationship between user login and project
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && parts.Any( x => x != Enums.TEAM_HR.ToString() ) &&
        !await _projectService.Exist( p => p.Id == projectId && ( teamUserLogins.Contains( p.TeamId ) || ( "," + p.ProjectManagement + "," ).Contains( "," + userLogin.ToString() + "," ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      return Ok( await _projectService.GetAnalysisView( projectId, null, userLogin ) );
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="categories"></param>
    /// <returns></returns>
    [HttpPost( "GetAnalysisView/{projectId}" )]
    [Authorize( Policy = Policies.ANALYSIS_VIEW )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( AnalysisResponse ) )]
    public async Task<IActionResult> GetAnalysisView( long projectId, [FromBody] AnalysisFilter categories )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      // valid relationship between user login and project
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && parts.Any( x => x != Enums.TEAM_HR.ToString() ) &&
        !await _projectService.Exist( p => p.Id == projectId && ( teamUserLogins.Contains( p.TeamId ) || ( "," + p.ProjectManagement + "," ).Contains( "," + userLogin.ToString() + "," ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      return Ok( await _projectService.GetAnalysisView( projectId, null, userLogin, categories.Categories.ToHashSet() ) );
    }
    /// <summary>
    /// [Admin - Report] export timesheet của user cho project
    /// </summary>
    /// <param name="id" example="1">project id</param>
    /// <returns></returns>
    /// <response code="200"></response>
    [HttpGet( "ExportAnalysis/{id}" )]
    [Authorize( Policy = Policies.ANALYSIS_VIEW )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( byte [] ) )]
    public async Task<IActionResult> ExportReport( long id )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      // valid relationship between user login and project
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && parts.Any( x => x != Enums.TEAM_HR.ToString() ) &&
        !await _projectService.Exist( p => p.Id == id && ( teamUserLogins.Contains( p.TeamId ) || ( "," + p.ProjectManagement + "," ).Contains( "," + userLogin.ToString() + "," ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      string fileName = $"AnalysisProject_{DateTime.UtcNow:yyyy_MM_dd-HH_mm_ss}.xlsx";
      return File( ( await _projectService.ExportAnalysis( id ) )!, "application/xlsx", fileName );
    }

    /// <summary>
    /// [Admin - Other] Upload contract project file
    /// </summary>
    /// <remarks>File upload type: pdf, excel, docs, image</remarks>
    /// <param name="resource"></param>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpPut( "UploadContract/{projectId}" )]
    [Authorize( Policy = Policies.CONFIG_CREATE )]
    [Consumes( "multipart/form-data" )]
    [Produces( "multipart/form-data" )]
    public async Task<IActionResult> UploadContract( long projectId, [FromForm] ProjectContractResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      // valid relationship between user login and project
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && parts.Any( x => x != Enums.TEAM_HR.ToString() ) && !await _projectService.Exist( p => p.Id == projectId && ( teamUserLogins.Contains( p.TeamId ) || ( "," + p.ProjectManagement + "," ).Contains( "," + userLogin.ToString() + "," ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      // update DB
      await _projectService.CreateContract( projectId, resource );
      return Ok();
    }

    /// <summary>
    /// Update infor contract
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPut( "UpdateContract/{projectId}" )]
    [Authorize( Policy = Policies.CONTRACT_UPDATE )]
    [Consumes( "multipart/form-data" )]
    [Produces( "multipart/form-data" )]
    public async Task<IActionResult> UpdateContract( long projectId, [FromForm] ProjectContractResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      // valid relationship between user login and project
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && !await _projectService.Exist( p => p.Id == projectId && ( teamUserLogins.Contains( p.TeamId ) || ( "," + p.ProjectManagement + "," ).Contains( "," + userLogin.ToString() + "," ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      // update DB
      await _projectService.UpdateContract( projectId, resource, userLogin );
      return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet( "GetListContractView/{projectId}" )]
    [Authorize( Policy = Policies.CONTRACT_GET_LIST )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( IEnumerable<ProjectResponse> ) )]
    public async Task<IEnumerable<ProjectContractResponse>> GetListContract( long projectId )
    {
      return await _projectService.GetProjectContract( projectId );
    }

    /// <summary>
    /// [Admin - Delete] Xóa Document project contract
    /// </summary>
    /// <param name="contractId" example="1">ID project chỉ định</param>
    /// <param name="documentId" example="1">ID document của project</param>
    /// <returns></returns>
    [HttpDelete( "DeleteDocument/{contractId}/{documentId}" )]
    [Authorize( Policy = Policies.CONTRACT_DELETE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( List<ProjectDocumentDetail> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> DeleteContractDocument( long contractId, long documentId )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      // valid relationship between user login and project
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && !await _projectService.Exist( p => p.Id == contractId && ( teamUserLogins.Contains( p.TeamId ) || ( "," + p.ProjectManagement + "," ).Contains( "," + userLogin.ToString() + "," ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      // valid document exist
      var documents = await _projectService.GetDocumentInContract( contractId );
      var doc = documents.Where( d => d.Id == documentId ).FirstOrDefault();
      if ( doc is null ) {
        return BadRequest( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Project ) );
      }
      // delete
      var result = await _projectService.DeleteContractDocument( documentId );
      // delete file
      var fileName = $"{contractId}_{doc.Name}";
      var path = Path.GetFullPath( Path.Combine( Directory.GetCurrentDirectory(), "Upload", "Documents" ) );
      // delete exist old image
      if ( System.IO.File.Exists( Path.Combine( path, fileName ) ) ) {
        System.IO.File.Delete( Path.Combine( path, fileName ) );
      }
      return Ok( result );
    }
    /// <summary>
    /// [Admin - Delete] Xóa Contract project 
    /// </summary>
    /// <param name="contractId" example="1">ID project chỉ định</param>
    /// <returns></returns>
    [HttpDelete( "DeleteContract/{contractId}" )]
    [Authorize( Policy = Policies.CONTRACT_DELETE )]
    [SwaggerResponse( StatusCodes.Status200OK )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> DeleteContract( long contractId )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value );
      // valid relationship between user login and project
      if ( !Common.ValidRoleAdmin( positionUserLogin ) && !await _projectService.Exist( p => p.Id == contractId && ( p.TeamId == teamUserLogin || ( "," + p.ProjectManagement + "," ).Contains( "," + userLogin.ToString() + "," ) ) ) ) {
        return BadRequest( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      // delete
      await _projectService.DeleteContract( contractId );
      return Ok();
    }

    [HttpPut( "AddUser/{id}" )]
    [Authorize( Policy = Policies.PROJECT_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( List<UserOption> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> AddUserProject( long id, [FromBody] ProjectAddUser usersProject )
    {
      // update
      var result = await _projectService.AddUserProject( id, usersProject.UserIds );
      return Ok( result );
    }
    [HttpPut( "Remove/{id}/{userId}" )]
    [Authorize( Policy = Policies.PROJECT_UPDATE )]
    [SwaggerResponse( StatusCodes.Status200OK, Type = typeof( List<UserOption> ) )]
    [SwaggerResponse( StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> AddUserProject( long id, long userId )
    {
      // update
      var result = await _projectService.RemoveUserProject( id, userId );
      return Ok( result );
    }

    /// <summary>
    /// valid object trước khi delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> ValidateBeforeDelete( long id )
    {
      var errors = new Dictionary<string, string>();
      var positionUserLogin = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamUserLogin = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var parts = teamUserLogin.Split( ',' );
      var teamUserLogins = parts.Select( long.Parse ).ToArray();
      // check project id
      var project = await _projectService.GetByCondition( p => p.Id == id );
      if ( project == null ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Project ) );
      }
      // valid: user không là admin, chỉ được xóa project thuộc team mình quản lý
      else if ( !Common.ValidRoleAdmin( positionUserLogin ) && !teamUserLogins.Contains( project.TeamId ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.Project ) );
      }
      // kiểm tra project đã được khai báo timesheet chưa
      else if ( await _projectService.HadTimesheet( id ) ) {
        errors.Add( Enums.ERROR_TEXT, string.Format( ErrorMessageResource.HadUsing, DisplayNameResource.Project ) );
      }
      return errors;
    }
  }
}
