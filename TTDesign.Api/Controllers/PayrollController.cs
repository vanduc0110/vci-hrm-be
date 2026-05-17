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
  public class PayrollController : ControllerBase
  {
    private readonly IPayrollService _payrollService;
    private readonly ITaxBracketService _taxBracketService;
    private readonly ISocialInsuranceRateService _rateService;
    private readonly IUserService _userService;

    public PayrollController( IPayrollService payrollService,
      ITaxBracketService taxBracketService,
      ISocialInsuranceRateService rateService,
      IUserService userService )
    {
      _payrollService = payrollService;
      _taxBracketService = taxBracketService;
      _rateService = rateService;
      _userService = userService;
    }

    // ── Payroll ───────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetList( [FromQuery] int month, [FromQuery] int year )
    {
      var position = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr  = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var teamIds  = teamStr.Split( ',' ).Select( long.Parse ).ToArray();

      // Director/System: xem tất cả; HR (teamId == TEAM_HR hoặc có claim payroll:create): xem tất cả
      // Lead/Sublead/PM: chỉ xem nhân viên trong team của họ
      bool hasPayrollFull = HttpContext.User.HasClaim( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_CREATE );
      long[]? allowedUserIds = null;
      if ( !Common.ValidRoleAdmin( position ) && !teamIds.Contains( Enums.TEAM_HR ) && !hasPayrollFull ) {
        var users = new List<UserResponse>();
        foreach ( var teamId in teamIds ) {
          var res = await _userService.GetList( new BaseFilter { TeamId = teamId } );
          users.AddRange( res );
        }
        allowedUserIds = users.Select( u => (long)u.Id ).Distinct().ToArray();
      }

      return Ok( await _payrollService.GetList( month, year, allowedUserIds ) );
    }

    [HttpGet( "{id}" )]
    public async Task<IActionResult> GetDetail( long id )
    {
      var result = await _payrollService.GetDetail( id );
      if ( result == null ) return NotFound();
      return Ok( result );
    }

    [HttpPost( "calculate" )]
    public async Task<IActionResult> Calculate( [FromBody] PayrollCalculateRequest request )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.Calculate( request, userLogin );
      return Ok();
    }

    [HttpPut( "update" )]
    public async Task<IActionResult> Update( [FromBody] PayrollUpdateRequest request )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.Update( request, userLogin );
      return NoContent();
    }

    [HttpPut( "{id}/lead-confirm" )]
    public async Task<IActionResult> LeadConfirm( long id )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.LeadConfirm( id, userLogin );
      return NoContent();
    }

    [HttpPut( "{id}/hr-approve" )]
    public async Task<IActionResult> HRApprove( long id )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.HRApprove( id, userLogin );
      return NoContent();
    }

    [HttpPut( "{id}/director-approve" )]
    public async Task<IActionResult> DirectorApprove( long id )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.DirectorApprove( id, userLogin );
      return NoContent();
    }

    [HttpPut( "{id}/paid" )]
    public async Task<IActionResult> MarkPaid( long id )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.MarkPaid( id, userLogin );
      return NoContent();
    }

    [HttpPut( "{id}/reject" )]
    public async Task<IActionResult> Reject( long id, [FromBody] string reason )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.Reject( id, reason ?? string.Empty, userLogin );
      return NoContent();
    }

    [HttpPut( "{id}/cancel" )]
    public async Task<IActionResult> Cancel( long id )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _payrollService.Cancel( id, userLogin );
      return NoContent();
    }

    [HttpGet( "GetTotalPayrollPending" )]
    public async Task<IActionResult> GetTotalPayrollPending()
    {
      var position = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr  = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )?.Value ?? "";
      var teamIds  = teamStr.Length > 0 ? teamStr.Split( ',' ).Select( long.Parse ).ToArray() : Array.Empty<long>();
      bool hasPayrollFull = HttpContext.User.HasClaim( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_CREATE );
      var now   = DateTime.UtcNow;
      var count = await _payrollService.GetTotalPending( now.Month, now.Year, position, teamIds, hasPayrollFull );
      return Ok( count );
    }

    [HttpGet( "export" )]
    public async Task<IActionResult> Export( [FromQuery] int month, [FromQuery] int year )
    {
      var bytes = await _payrollService.Export( month, year );
      if ( bytes == null ) return NoContent();
      var fileName = $"Payroll_{month:D2}_{year}.xlsx";
      return File( bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName );
    }

    // ── Tax brackets ─────────────────────────────────────────────────────────

    [HttpGet( "tax-brackets" )]
    public async Task<IActionResult> GetTaxBrackets( [FromQuery] int year )
    {
      return Ok( await _taxBracketService.GetList( year ) );
    }

    [HttpPost( "tax-brackets" )]
    public async Task<IActionResult> CreateTaxBracket( [FromBody] TaxBracketResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _taxBracketService.Create( resource, userLogin ) );
    }

    [HttpPut( "tax-brackets" )]
    public async Task<IActionResult> UpdateTaxBracket( [FromBody] TaxBracketResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _taxBracketService.Update( resource, userLogin );
      return NoContent();
    }

    [HttpDelete( "tax-brackets/{id}" )]
    public async Task<IActionResult> DeleteTaxBracket( long id )
    {
      await _taxBracketService.Delete( id );
      return NoContent();
    }

    // ── Insurance rates ───────────────────────────────────────────────────────

    [HttpGet( "insurance-rates" )]
    public async Task<IActionResult> GetInsuranceRates()
    {
      return Ok( await _rateService.GetList() );
    }

    [HttpGet( "insurance-rates/active" )]
    public async Task<IActionResult> GetActiveInsuranceRate()
    {
      var result = await _rateService.GetActive();
      if ( result == null ) return NotFound();
      return Ok( result );
    }

    [HttpPost( "insurance-rates" )]
    public async Task<IActionResult> CreateInsuranceRate( [FromBody] SocialInsuranceRateResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      return Ok( await _rateService.Create( resource, userLogin ) );
    }
  }
}
