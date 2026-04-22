using Microsoft.AspNetCore.Identity;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Seeds
{
  public static class DefaultUsers
  {
    public static async Task SeedSuperAdminAsync( UserManager<User> userManager )
    {
      // Seed Default System User
      if ( await userManager.FindByEmailAsync( Enums.SYSTEM_MAIL ) is null ) {
        var defaultUser = new User
        {
          UserName = Enums.SYSTEM_NAME,
          FullName = Enums.SYSTEM_NAME,
          Email = Enums.SYSTEM_MAIL,
          Position = ( int ) Enums.UserPosition.System,
          StaffId = "0",
          CreatedBy = Enums.SYSTEM_CREATOR,
          ModifiedBy = Enums.SYSTEM_CREATOR
        };
        var result = await userManager.CreateAsync( defaultUser, Enums.SYSTEM_PASS_INITIAL );
        // set role system
        await userManager.AddToRoleAsync( defaultUser, Roles.ROLE_NAME_SYSTEM );
        await userManager.AddClaimAsync( defaultUser, new System.Security.Claims.Claim( Roles.ROLE_ADMIN_USER, Roles.PERMISSION_UPDATE ) );
      }

    }
  }
}
