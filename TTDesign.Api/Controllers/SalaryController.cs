using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  [Authorize]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class SalaryController : ControllerBase
  {
    private readonly ISalaryService _salaryService;

    public SalaryController( ISalaryService salaryService )
    {
      _salaryService = salaryService;
    }

    private bool CanManageSalary()
    {
      var position = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr  = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )?.Value ?? "";
      var teamIds  = teamStr.Length > 0 ? teamStr.Split( ',' ).Select( long.Parse ).ToArray() : Array.Empty<long>();
      bool hasPayrollFull = HttpContext.User.HasClaim( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_CREATE );
      return Common.ValidRoleAdmin( position ) || teamIds.Contains( Enums.TEAM_HR ) || hasPayrollFull;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
      if ( !CanManageSalary() ) return Forbid();
      return Ok( await _salaryService.GetList() );
    }

    [HttpGet( "user/{userId}" )]
    public async Task<IActionResult> GetByUser( long userId )
    {
      if ( !CanManageSalary() ) return Forbid();
      var result = await _salaryService.GetByUser( userId );
      if ( result == null ) return NotFound();
      return Ok( result );
    }

    [HttpPost]
    public async Task<IActionResult> Create( [FromBody] SalaryResource resource )
    {
      if ( !CanManageSalary() ) return Forbid();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var id = await _salaryService.Create( resource, userLogin );
      return Ok( id );
    }

    [HttpPut]
    public async Task<IActionResult> Update( [FromBody] SalaryResource resource )
    {
      if ( !CanManageSalary() ) return Forbid();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _salaryService.Update( resource, userLogin );
      return NoContent();
    }

    [HttpPut( "{id}/approve" )]
    public async Task<IActionResult> Approve( long id )
    {
      var position = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      if ( !Common.ValidRoleAdmin( position ) ) return Forbid();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _salaryService.Approve( id, userLogin );
      return NoContent();
    }

    [HttpDelete( "{id}" )]
    public async Task<IActionResult> Deactivate( long id )
    {
      if ( !CanManageSalary() ) return Forbid();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _salaryService.Deactivate( id, userLogin );
      return NoContent();
    }
  }
}
