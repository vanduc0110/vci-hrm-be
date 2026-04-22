using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Extensions;

namespace TTDesign.API.Seeds
{
  public static class DefaultRoles
  {
    public static async Task SeedDefaultRoleSystemAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_SYSTEM ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_SYSTEM,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        foreach ( var menu in Common.CollectionRole() ) {
          foreach ( var record in menu.Value ) {
            if ( record.Type == Roles.ROLE_STAFF_DASHBOARD ) {
              continue;
            }
            await roleManager.AddClaimAsync( role, new Claim( record.Type, record.Value ) );
          }
        }
      }
    }

    public static async Task SeedDefaultRoleDirectorAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_DIRECTOR ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_DIRECTOR,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        foreach ( var menu in Common.CollectionRole() ) {
          foreach ( var record in menu.Value ) {
            if ( record.Type == Roles.ROLE_STAFF_DASHBOARD ) {
              continue;
            }
            await roleManager.AddClaimAsync( role, new Claim( record.Type, record.Value ) );
          }
        }
      }
    }

    public static async Task SeedDefaultRoleLeaderAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_TEAMLEAD ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_TEAMLEAD,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        var claims = new HashSet<Claim>()
        {
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_LOCK),
          new Claim (Constants.Roles.ROLE_STAFF_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_CALENDAR, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_WFH, Constants.Roles.PERMISSION_VIEW),

          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_INACTIVE),
          //new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_EDIT),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_CONFIG, Constants.Roles.PERMISSION_VIEW),

          new Claim (Constants.Roles.ROLE_ADMIN_ANALYSIS, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_ANALYSIS, Constants.Roles.PERMISSION_REPORT),

          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_DELETE),     

          new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_APPROVE),
          new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_REPORT),
          //new Claim (Constants.Roles.ROLE_ADMIN_LEAVEFORM, Constants.Roles.PERMISSION_HOLIDAY),

          new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_APPROVE),
          new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_REPORT),
          //new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_ASSIGN),
          
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_WFH, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_WFH, Constants.Roles.PERMISSION_APPROVE),
        };
        foreach ( var claim in claims ) {
          await roleManager.AddClaimAsync( role, claim );
        }
      }
    }

    public static async Task SeedDefaultRoleSubleadAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_SUBLEAD ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_SUBLEAD,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        var claims = new HashSet<Claim>()
        {
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_LOCK),
          new Claim (Constants.Roles.ROLE_STAFF_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_CALENDAR, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_WFH, Constants.Roles.PERMISSION_VIEW),

          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_INACTIVE),
          //new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_EDIT),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_CONFIG, Constants.Roles.PERMISSION_VIEW),


          new Claim (Constants.Roles.ROLE_ADMIN_ANALYSIS, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_ANALYSIS, Constants.Roles.PERMISSION_REPORT),

          new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_DELETE),      

          new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_APPROVE),
          new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_REPORT),
          //new Claim (Constants.Roles.ROLE_ADMIN_LEAVEFORM, Constants.Roles.PERMISSION_HOLIDAY),
          
          new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_APPROVE),
          new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_REPORT),
          //new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_ASSIGN),
          
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_WFH, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_WFH, Constants.Roles.PERMISSION_APPROVE),
        };
        foreach ( var claim in claims ) {
          await roleManager.AddClaimAsync( role, claim );
        }
      }
    }

    public static async Task SeedDefaultRolePMAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_PM ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_PM,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        var claims = new HashSet<Claim>()
        {
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_LOCK),
          new Claim (Constants.Roles.ROLE_STAFF_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_CALENDAR, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_WFH, Constants.Roles.PERMISSION_VIEW),

          //new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_INACTIVE),
          //new Claim (Constants.Roles.ROLE_ADMIN_USER, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_EDIT),
          //new Claim (Constants.Roles.ROLE_ADMIN_TEAM, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_GROUP, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_EDIT),
          //new Claim (Constants.Roles.ROLE_ADMIN_ROLE, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_CREATE),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_UPDATE),
          new Claim (Constants.Roles.ROLE_ADMIN_PROJECT, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_CONFIG, Constants.Roles.PERMISSION_VIEW),


          new Claim (Constants.Roles.ROLE_ADMIN_ANALYSIS, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_ADMIN_ANALYSIS, Constants.Roles.PERMISSION_REPORT),

          //new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_OBJECT, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_CATEGORY, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_FINGERPRINT, Constants.Roles.PERMISSION_DELETE),
          
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_SWAPDAY, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_OVERTIME, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_OVERTIME, Constants.Roles.PERMISSION_APPROVE),
          //new Claim (Constants.Roles.ROLE_ADMIN_OVERTIME, Constants.Roles.PERMISSION_REPORT),

          new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_APPROVE),
          //new Claim (Constants.Roles.ROLE_ADMIN_LEAVE, Constants.Roles.PERMISSION_REPORT),
          //new Claim (Constants.Roles.ROLE_ADMIN_LEAVEFORM, Constants.Roles.PERMISSION_HOLIDAY),
          
          //new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_APPROVE),
          //new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_REPORT),
          //new Claim (Constants.Roles.ROLE_ADMIN_ASSET, Constants.Roles.PERMISSION_ASSIGN),
          
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_PRODUCT, Constants.Roles.PERMISSION_DELETE),

          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_CREATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_UPDATE),
          //new Claim (Constants.Roles.ROLE_ADMIN_BILL, Constants.Roles.PERMISSION_DELETE),

          new Claim (Constants.Roles.ROLE_ADMIN_WFH, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_ADMIN_WFH, Constants.Roles.PERMISSION_APPROVE),
        };
        foreach ( var claim in claims ) {
          await roleManager.AddClaimAsync( role, claim );
        }
      }
    }

    public static async Task SeedDefaultRoleOfficalAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_OFFICIAL ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_OFFICIAL,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        var claims = new HashSet<Claim>()
        {
          new Claim (Constants.Roles.ROLE_STAFF_DASHBOARD, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_LOCK),
          new Claim (Constants.Roles.ROLE_STAFF_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_CALENDAR, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_WFH, Constants.Roles.PERMISSION_VIEW),
        };
        foreach ( var claim in claims ) {
          await roleManager.AddClaimAsync( role, claim );
        }
      }
    }

    public static async Task SeedDefaultRoleProbationaryAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_PROBATIONARY ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_PROBATIONARY,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        var claims = new HashSet<Claim>()
        {
          new Claim (Constants.Roles.ROLE_STAFF_DASHBOARD, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_LOCK),
          new Claim (Constants.Roles.ROLE_STAFF_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_CALENDAR, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_WFH, Constants.Roles.PERMISSION_VIEW),
        };
        foreach ( var claim in claims ) {
          await roleManager.AddClaimAsync( role, claim );
        }
      }
    }

    public static async Task SeedDefaultRoleInternshipAsync( RoleManager<Role> roleManager )
    {
      if ( await roleManager.FindByNameAsync( Constants.Roles.ROLE_NAME_INTERNSHIP ) is null ) {
        var role = new Role
        {
          Name = Constants.Roles.ROLE_NAME_INTERNSHIP,
          CreatedBy = Constants.Enums.SYSTEM_CREATOR,
          ModifiedBy = Constants.Enums.SYSTEM_CREATOR,
          Type = ( int ) Constants.Enums.RoleType.Default,
        };
        await roleManager.CreateAsync( role );
        var claims = new HashSet<Claim>()
        {
          new Claim (Constants.Roles.ROLE_STAFF_DASHBOARD, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_VIEW),
          //new Claim (Constants.Roles.ROLE_STAFF_TIMESHEET_OTHER, Constants.Roles.PERMISSION_LOCK),
          //new Claim (Constants.Roles.ROLE_STAFF_LEAVE, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_CALENDAR, Constants.Roles.PERMISSION_VIEW),
          new Claim (Constants.Roles.ROLE_STAFF_WFH, Constants.Roles.PERMISSION_VIEW),
        };
        foreach ( var claim in claims ) {
          await roleManager.AddClaimAsync( role, claim );
        }
      }
    }
  }
}
