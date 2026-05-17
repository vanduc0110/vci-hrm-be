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
  public class BonusController : ControllerBase
  {
    private readonly IBonusService _bonusService;
    private readonly IUserService _userService;

    public BonusController( IBonusService bonusService, IUserService userService )
    {
      _bonusService = bonusService;
      _userService  = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList( [FromQuery] int month, [FromQuery] int year )
    {
      var position = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr  = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var teamIds  = teamStr.Split( ',' ).Select( long.Parse ).ToArray();

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

      return Ok( await _bonusService.GetList( month, year, allowedUserIds ) );
    }

    [HttpPost]
    public async Task<IActionResult> Create( [FromBody] BonusResource resource )
    {
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      var position  = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      var teamStr   = HttpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;
      var teamIds   = teamStr.Split( ',' ).Select( long.Parse ).ToArray();

      bool hasPayrollFullCreate = HttpContext.User.HasClaim( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_CREATE );
      if ( !Common.ValidRoleAdmin( position ) && !teamIds.Contains( Enums.TEAM_HR ) && !hasPayrollFullCreate ) {
        var teamUsers = new List<UserResponse>();
        foreach ( var teamId in teamIds ) {
          var res = await _userService.GetList( new BaseFilter { TeamId = teamId } );
          teamUsers.AddRange( res );
        }
        var allowedUserIds = teamUsers.Select( u => (long)u.Id ).Distinct().ToArray();
        if ( !allowedUserIds.Contains( resource.UserId ) )
          return BadRequest( "Chỉ được tạo thưởng/phạt cho nhân viên trong team của mình" );
      }

      var id = await _bonusService.Create( resource, userLogin );
      return Ok( id );
    }

    [HttpPut( "{id}/approve" )]
    public async Task<IActionResult> Approve( long id )
    {
      var position = int.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.Role )!.Value );
      if ( !Common.ValidRoleAdmin( position ) ) return Forbid();
      var userLogin = long.Parse( HttpContext.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value );
      await _bonusService.Approve( id, userLogin );
      return NoContent();
    }

    [HttpDelete( "{id}" )]
    public async Task<IActionResult> Delete( long id )
    {
      var bonus = await _bonusService.GetByCondition( b => b.Id == id );
      if ( bonus == null ) return NotFound();
      if ( bonus.ApprovedBy.HasValue ) return BadRequest( "Không thể xóa thưởng/phạt đã được duyệt" );
      await _bonusService.Delete( id );
      return NoContent();
    }
  }
}
