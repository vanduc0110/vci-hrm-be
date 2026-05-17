using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model input api [Create]/[Update]
  /// </summary>
  [Index( nameof( Name ), IsUnique = true, Name = "Ix_RoleName" )]
  public class RoleResource : IValidatableObject
  {
    /// <summary>
    /// Team ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Role Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "RoleName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Roles: danh sách quyền [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "Roles" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public Dictionary<string, RoleModelView []> Roles { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( Name ) ) {
        Name = Common.RemoveMultiBlank( Name );
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.Roles.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Role ), new [] { Enums.ERROR_TEXT } ) );
      }
      //else if ( Id is not null && dbContext.Roles.Any( t => t.Id == Id && t.Type == ( int ) Enums.RoleType.Default ) ) {
      //  results.Add( new ValidationResult( ErrorMessageResource.UserNotPermission, new [] { Enums.ERROR_TEXT } ) );
      //}
      else {
        if ( dbContext.Roles.Any( t => t.Id != Id && t.Name == Name ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.RoleName ), new [] { nameof( Name ) } ) );
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class RoleResponse
  {
    /// <summary>
    /// Role ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Role Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Role Type: Default/Custom
    /// </summary>
    /// <example>Default</example>
    [Required]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// User Modify
    /// </summary>
    public long ModifiedBy { get; set; }
    /// <summary>
    /// User Modify Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string ModifierName { get; set; } = string.Empty;
    /// <summary>
    /// Modified Date
    /// </summary>
    [Required]
    public DateTime ModifiedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string ModifiedDateText => ModifiedDate.ToString( Enums.DATE_FORMAT );
  }

  /// <summary>
  /// Class cho model output api [Option]
  /// </summary>
  public class RoleOption
  {
    /// <summary>
    /// Role ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Role Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
  }

  /// <summary>
  /// Class cho model output api [Get View]
  /// </summary>
  public class RoleDetailResponse
  {
    /// <summary>
    /// Role ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Role Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Roles: danh sách quyền
    /// </summary>
    public Dictionary<string, RoleModelView []> Roles { get; set; } = null!;
  }

  /// <summary>
  /// Model định nghĩa 1 role, sử dụng ở BE
  /// </summary>
  public class RoleModel
  {
    /// <summary>
    /// Hành động: View/Create/Update/Delete/... (để hiển thị)
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Bộ role: User/Team/Group...
    /// </summary>
    [Required]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// Hành động: View/Create/Update/Delete/... (giá trị role)
    /// </summary>
    [Required]
    public string Value { get; set; } = string.Empty;
    ///// <summary>
    ///// Policy Name khai báo trong project BE
    ///// </summary>
    //[Required] public string PolicyName { get; set; } = string.Empty;
    /// <summary>
    /// True: Cho phép/ False: Không cho phép
    /// </summary>
    public bool IsActive { get; set; } = false;
  }

  /// <summary>
  /// Model định nghĩa 1 role, tương tác FE và BE
  /// </summary>
  public class RoleModelView
  {
    /// <summary>
    /// Hành động: View/Create/Update/Delete/...
    /// </summary>
    /// <example>View</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Bộ role: User/Team/Group...
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// True: Cho phép/ False: Không cho phép
    /// </summary>
    /// <example>false</example>
    public bool IsActive { get; set; } = false;
  }

  /// <summary>
  /// Model phục vụ logic check menu access, phục vụ cho FE generate giao diện menu và tab menu
  /// </summary>
  public class MenuAccess
  {
    public MenuStaff Staff => new( Claims, TeamHR );
    public MenuAdmin Admin => new( Claims, TeamHR );
    /// <summary>
    /// danh sách claim dùng để check truy cập menu/tab menu không
    /// </summary>
    [JsonIgnore]
    public List<Claim> Claims { get; set; } = null!;
    [JsonIgnore]
    public bool TeamHR { get; set; } = false;
  }

  /// <summary>
  /// hệ thống bao gồm 2 block menu chính: cho staff và cho admin
  /// </summary>
  public class MenuStaff
  {
    public MenuStaff( List<Claim> Claims, bool TeamHR )
    {
      Dashboard = new MenuDetailStaffDashboard( Claims );
      Timesheet = new MenuDetailStaffTimesheet( Claims );
      TimesheetOther = new MenuDetailStaffTimesheetOther( Claims );
      Leave = new MenuDetailStaffLeave( Claims );
      Calendar = new MenuDetailStaffCalendar( Claims );
      WFH = new MenuDetailStaffWfh( Claims );
      Report = new MenuDetailStaffReport( Claims );
    }
    /// <summary>
    /// danh sách menu
    /// </summary>
    public MenuDetailStaffDashboard Dashboard { get; set; }
    public MenuDetailStaffTimesheet Timesheet { get; set; }
    public MenuDetailStaffTimesheetOther TimesheetOther { get; set; }
    public MenuDetailStaffLeave Leave { get; set; }
    public MenuDetailStaffCalendar Calendar { get; set; }
    public MenuDetailStaffWfh WFH { get; set; }
    public MenuDetailStaffReport Report { get; set; }
  }
  public class MenuAdmin
  {
    public MenuAdmin( List<Claim> Claims, bool TeamHR )
    {
      User = new MenuDetailAdminUser( Claims, TeamHR );
      Timesheet = new MenuDetailAdminTimesheet( Claims, TeamHR );
      Leave = new MenuDetailAdminLeave( Claims, TeamHR );
      Asset = new MenuDetailAdminAsset( Claims, TeamHR );
      WFH = new MenuDetailAdminWfh( Claims, TeamHR );
      Dashboard = new MenuDetailAdminDashboard( Claims, TeamHR );
      Payroll = new MenuDetailAdminPayroll( Claims );
    }
    public MenuDetailAdminUser User { get; set; }
    public MenuDetailAdminTimesheet Timesheet { get; set; }
    public MenuDetailAdminLeave Leave { get; set; }
    public MenuDetailAdminAsset Asset { get; set; }
    public MenuDetailAdminWfh WFH { get; set; }
    public MenuDetailAdminDashboard Dashboard { get; set; }
    public MenuDetailAdminPayroll Payroll { get; set; }
  }

  public class MenuDetailStaffDashboard
  {
    public MenuDetailStaffDashboard( List<Claim> Claims )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_STAFF_DASHBOARD && c.Value == Roles.PERMISSION_VIEW );
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
  }
  public class MenuDetailStaffTimesheet
  {
    public MenuDetailStaffTimesheet( List<Claim> Claims )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_STAFF_TIMESHEET && c.Value == Roles.PERMISSION_VIEW );
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
  }
  public class MenuDetailStaffTimesheetOther
  {
    public MenuDetailStaffTimesheetOther( List<Claim> Claims )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_STAFF_TIMESHEET_OTHER && c.Value == Roles.PERMISSION_VIEW );
      IsLock = Claims.Any( c => c.Type == Roles.ROLE_STAFF_TIMESHEET_OTHER && c.Value == Roles.PERMISSION_LOCK );
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
    /// <summary>
    /// đủ quyền lock timesheet
    /// </summary>
    public bool IsLock { get; set; }
  }
  public class MenuDetailStaffLeave
  {
    public MenuDetailStaffLeave( List<Claim> Claims )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_STAFF_LEAVE && c.Value == Roles.PERMISSION_VIEW );
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
  }
  public class MenuDetailStaffCalendar
  {
    public MenuDetailStaffCalendar( List<Claim> Claims )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_STAFF_CALENDAR && c.Value == Roles.PERMISSION_VIEW );
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
  }
  public class MenuDetailStaffWfh
  {
    public MenuDetailStaffWfh( List<Claim> Claims )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_STAFF_WFH && c.Value == Roles.PERMISSION_VIEW );
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
  }
  public class MenuDetailStaffReport
  {
    public MenuDetailStaffReport( List<Claim> Claims )
    {
    }
    public bool IsView => true;
    public bool IsCreate => true;
    public bool IsUpdate => true;
    public bool IsDelete => true;
  }


  /// <summary>
  /// chi tiết các action được truy cập
  /// </summary>
  public class MenuDetailAdminUser
  {
    public MenuDetailAdminUser( List<Claim> Claims, bool TeamHR )
    {
      TabUser = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_VIEW ),
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_UPDATE ),
        IsInactive = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_INACTIVE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_USER && c.Value == Roles.PERMISSION_DELETE ),
        Index = 0,
      };
      TabTeam = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_TEAM && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_TEAM && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_TEAM && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_TEAM && c.Value == Roles.PERMISSION_DELETE ),
        Index = 1,
      };
      TabGroup = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_GROUP && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_GROUP && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_GROUP && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_GROUP && c.Value == Roles.PERMISSION_DELETE ),
        Index = 2,
      };
      TabRole = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ROLE && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ROLE && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ROLE && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ROLE && c.Value == Roles.PERMISSION_DELETE ),
        Index = 3,
      };
      TabDashboard = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ROLE && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        Index = 4
      };
      TabReport = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_USER_REPORT && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        Index = 5
      };
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView => TabUser!.IsView || TabTeam!.IsView || TabGroup!.IsView || TabRole!.IsView;
    public MenuTab TabUser { get; set; }
    public MenuTab TabTeam { get; set; }
    public MenuTab TabGroup { get; set; }
    public MenuTab TabRole { get; set; }
    public MenuTab TabDashboard { get; set; }
    public MenuTab TabReport { get; set; }
  }
  public class MenuDetailAdminDashboard
  {
    public MenuDetailAdminDashboard( List<Claim> Claims, bool TeamHR )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_DASHBOARD && c.Value == Roles.PERMISSION_VIEW ) || TeamHR;
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
  }
  public class MenuDetailAdminTimesheet
  {
    public MenuDetailAdminTimesheet( List<Claim> Claims, bool TeamHR )
    {
      TabProject = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT && c.Value == Roles.PERMISSION_DELETE ),
        Index = 0,
      };
      TabAnalysis = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ANALYSIS && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsReport = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ANALYSIS && c.Value == Roles.PERMISSION_REPORT ),
        Index = 1,
      };
      TabCategory = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CATEGORY && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CATEGORY && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CATEGORY && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CATEGORY && c.Value == Roles.PERMISSION_DELETE ),
        Index = 3,
      };
      TabFingerPrinter = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_FINGERPRINT && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_FINGERPRINT && c.Value == Roles.PERMISSION_UPDATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_FINGERPRINT && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_FINGERPRINT && c.Value == Roles.PERMISSION_DELETE ),
        Index = 4,
      };
      TabSwapDay = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_SWAPDAY && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_SWAPDAY && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_SWAPDAY && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_SWAPDAY && c.Value == Roles.PERMISSION_DELETE ),
        Index = 5,
      };
      TabReport = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ANALYSIS && c.Value == Roles.PERMISSION_REPORT ) || TeamHR,
        Index = 6,
      };
      TabConfig = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CONFIG && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CONFIG && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CONFIG && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_CONFIG && c.Value == Roles.PERMISSION_DELETE ),
        Index = 7,
      };
      TabContract = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && c.Value == Roles.PERMISSION_CREATE ),
        IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && c.Value == Roles.PERMISSION_UPDATE ),
        IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && c.Value == Roles.PERMISSION_DELETE ),
        Index = 8,
      };
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView => TabProject!.IsView || TabAnalysis!.IsView || TabCategory!.IsView || TabFingerPrinter!.IsView
      || TabSwapDay!.IsView || TabReport!.IsView || TabConfig.IsView || TabConfig.IsView;
    public MenuTab TabProject { get; set; }
    public MenuTab TabAnalysis { get; set; }
    public MenuTab TabObject { get; set; }
    public MenuTab TabCategory { get; set; }
    public MenuTab TabFingerPrinter { get; set; }
    public MenuTab TabSwapDay { get; set; }
    public MenuTab TabReport { get; set; }
    public MenuTab TabConfig { get; set; }
    public MenuTab TabContract { get; set; }
  }
  public class MenuDetailAdminLeave
  {
    public MenuDetailAdminLeave( List<Claim> Claims, bool TeamHR )
    {
      TabRequest = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_LEAVE && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsApprove = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_LEAVE && c.Value == Roles.PERMISSION_APPROVE ),
        Index = 0,
      };
      TabSummary = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_LEAVE && c.Value == Roles.PERMISSION_REPORT ) || TeamHR,
        Index = 1,
      };
      TabHoliday = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_LEAVE && c.Value == Roles.PERMISSION_HOLIDAY ) || TeamHR,
        Index = 2,
      };
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView => TabRequest!.IsView || TabSummary!.IsView || TabHoliday!.IsView;
    public MenuTab TabRequest { get; set; }
    public MenuTab TabSummary { get; set; }
    public MenuTab TabHoliday { get; set; }
  }
  public class MenuDetailAdminAsset
  {
    public MenuDetailAdminAsset( List<Claim> Claims, bool TeamHR )
    {
      IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ASSET && c.Value == Roles.PERMISSION_VIEW ) || TeamHR;
      IsCreate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ASSET && c.Value == Roles.PERMISSION_CREATE );
      IsUpdate = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ASSET && c.Value == Roles.PERMISSION_UPDATE );
      IsDelete = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_ASSET && c.Value == Roles.PERMISSION_DELETE );
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; }
    public bool IsCreate { get; set; }
    public bool IsUpdate { get; set; }
    public bool IsDelete { get; set; }
  }
  public class MenuDetailAdminWfh
  {
    public MenuDetailAdminWfh( List<Claim> Claims, bool TeamHR )
    {
      TabWfhRequest = new()
      {
        IsView = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_WFH && c.Value == Roles.PERMISSION_VIEW ) || TeamHR,
        IsApprove = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_WFH && c.Value == Roles.PERMISSION_APPROVE ),
        Index = 0,
      };
    }
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView => TabWfhRequest!.IsView;
    public MenuTab TabWfhRequest { get; set; }
  }

  public class MenuDetailAdminPayroll
  {
    public MenuDetailAdminPayroll( List<Claim> Claims )
    {
      IsView    = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_VIEW );
      IsCreate  = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_CREATE );
      IsUpdate  = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_UPDATE );
      IsApprove = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_APPROVE );
      IsReport  = Claims.Any( c => c.Type == Roles.ROLE_ADMIN_PAYROLL && c.Value == Roles.PERMISSION_REPORT );
    }
    public bool IsView    { get; set; }
    public bool IsCreate  { get; set; }
    public bool IsUpdate  { get; set; }
    public bool IsApprove { get; set; }
    public bool IsReport  { get; set; }
  }

  public class MenuTab
  {
    /// <summary>
    /// đủ quyền truy truy cập menu và action view (list + detail)
    /// </summary>
    public bool IsView { get; set; } = false;
    /// <summary>
    /// index hiển thị của tab trong menu
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// đủ quyền action create
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsCreate { get; set; } = null;
    /// <summary>
    /// đủ quyền action update
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsUpdate { get; set; } = null;
    /// <summary>
    /// đủ quyền action delete
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsDelete { get; set; } = null;
    /// <summary>
    /// đủ quyền action change status active và inaction
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsInactive { get; set; } = null;
    /// <summary>
    /// đủ quyền action approve
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsApprove { get; set; } = null;
    /// <summary>
    /// đủ quyền action create
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsLock { get; set; } = null;
    /// <summary>
    /// đủ quyền truy cập tab holiday
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsHoliday { get; set; } = null;
    /// <summary>
    /// đủ quyền action report/summary
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsReport { get; set; } = null;
    /// <summary>
    /// đủ quyền action config
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public bool? IsSetting { get; set; } = null;
  }
}
