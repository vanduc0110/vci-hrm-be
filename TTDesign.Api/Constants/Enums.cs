
namespace TTDesign.API.Constants
{
  public static class Enums
  {
    public const string GOOGLE_API_OAUTH = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=";
    public const string AZURE_TENANT_ID = "dbaf4325-5610-4294-8eee-5e59ca882a8f";
    public const string AZURE_CLIENT_ID = "";
    public const string SYSTEM_MAIL = "system@vcijsc.com";
    public const string SYSTEM_NAME = "System";
    public const long SYSTEM_ID = 1;
    public const string SYSTEM_PASS_INITIAL = "vci@123";
    public const string PASS_INITIAL = "vci@123"; // password default khi tạo user/ reset password
    public const string CLAIM_TYPE_TEAM = "Team";
    public const string NOT_FOUND = "NotFound";
    public const string AVATAR_DEFAULT = "/Upload/Images/0.png"; // avatar default
    public const long TEAM_HR = 1; // id team nhân sự
    /// <summary>
    /// unit: minutes
    /// </summary>
    public const int MINIMUM_PERIOD_CALENDAR = 15; // minutes
    /// <summary>
    /// unit: days
    /// </summary>
    public const int MINIMUM_PERIOD_HOLIDAY = 1; // days
    /// <summary>
    /// unit: hours
    /// </summary>
    public const int MINIMUM_PERIOD_WFH = 4; // hours
    /// <summary>
    /// hours
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_SELF_WEDDING = 4; // hours
    /// <summary>
    /// unit: days
    /// </summary>
    public const int MAXIMUM_PERIOD_LEAVE_SELF_WEDDING = 3; // days
    /// <summary>
    /// unit: hours
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_FAMILY_WEDDING = 4; // hours
    /// <summary>
    /// unit: days
    /// </summary>
    public const int MAXIMUM_PERIOD_LEAVE_FAMILY_WEDDING = 1; // days
    /// <summary>
    /// unit: hours
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_FAMILY_BEREAVEMENT = 4; // hours
    /// <summary>
    /// unit: days
    /// </summary>
    public const int MAXIMUM_PERIOD_LEAVE_FAMILY_BEREAVEMENT = 3; // days
    /// <summary>
    /// unit: hours
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_RELATIVE_BEREAVEMENT = 4; // hours
    /// <summary>
    /// unit: days
    /// </summary>
    public const int MAXIMUM_PERIOD_LEAVE_RELATIVE_BEREAVEMENT = 1; // days
    /// <summary>
    /// unit: months
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_SELF_MATERNITY = 4; // months
    /// <summary>
    /// unit: months
    /// </summary>
    public const int MAXIMUM_PERIOD_LEAVE_SELF_MATERNITY = 6; // months
    /// <summary>
    /// unit: hours
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_FAMILY_MATERNITY = 4; // hours
    /// <summary>
    /// unit: days
    /// </summary>
    public const int MAXIMUM_PERIOD_LEAVE_FAMILY_MATERNITY = 7; // days   
    /// <summary>
    /// unit: hours
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_ANNUAL = 4; // hours
    //public const int MAXIMUM_PERIOD_LEAVE_ANNUAL = 4; // hours
    /// <summary>
    /// unit: minutes
    /// </summary>
    public const int MINIMUM_PERIOD_LEAVE_UNPAID = 15; // minutes


    public const int MAXIUM_FINGURE_LATE = 10; // minutes

    public const long SYSTEM_CREATOR = 1;
    public const string ERROR_TEXT = "ERROR";
    public const int YEAR_MAXIMUM = 2050;
    public const int YEAR_MINIMUM = 2010;
    public const string REGEX_HOLIDAY_HOUR_INPUT = "^[0-9][0-9]:[03]0$";
    public const int MAX_AVATAR_SIZE = 5 * 1024 * 1024;
    public const int MAX_DOCUMENT_SIZE = 5 * 1024 * 1024;
    public static string [] AVATAR_EXTENSION = { ".png", ".img", ".jpg" };
    public static string [] DOCUMENT_EXTENSION = { ".pdf", ".xlsx", ".xls", ".docx", ".png", ".img", ".jpg" };
    public static int DOCUMENT_MAXIMUM = 5;
    public static int START_NO_USER = 2;
    public static string HOUR_FORMAT = "HH:mm";
    public static string DATE_FORMAT = "yyyy/MM/dd";
    public static string DATE_FORMAT_1 = "MM/dd/yyyy";
    public static string DATETIME_FORMAT = "yyyy/MM/dd HH:mm";
    public static double TimeWork = 8; // thời gian làm việc giờ hành chính theo quy định
    public static string TimeCheckDefault = "--:--";
    public static string TimeWfhCheckInDefault = "08:00";
    public static string TimeWfhCheckOutDefault = "17:00";
    public static int TimeWorkStartHour = 8; // thời gian hành chính bắt đầu làm việc trong ngày
    public static int TimeWorkStartMinute = 0;
    public static int TimeWorkEndHourMorning = 12; // thời gian hành chính kết thúc làm việc buổi sáng
    public static int TimeWorkEndMinuteMorning = 0;
    public static int TimeWorkStartHourAfternoon = 13; // thời gian hành chính bắt đầu làm việc buổi chiều
    public static int TimeWorkStartMinuteAfternoon = 0;
    public static int TimeWorkEndHour = 17; // thời gian hành chính kết thúc làm việc trong ngày
    public static int TimeWorkEndMinute = 0;
    public static int TimesheetReportPaidLeave = 0; // dành cho thẻ timesheet report leave request
    public static int TimesheetReportUnpaidLeave = -1; // dành cho thẻ timesheet report leave request
    //public static int TimesheetReportHoliday = -1; // dành cho thẻ timesheet report holiday
    public static string TimesheetReportPaidLeaveName = "Paid Leave"; // để hiển thị name project (là leave request) khi xem timesheet theo tháng
    public static string TimesheetReportUnpaidLeaveName = "Unpaid Leave"; // để hiển thị name project (là leave request) khi xem timesheet theo tháng
    //public static string TimesheetReportHolidayName = "Holiday"; // để hiển thị name project (là holiday) khi xem timesheet theo tháng
    public static int AssetCode = 00001;
    public enum RoleType
    {
      Default,
      Custom
    }

    public static List<int> ADMIN_POSITION = new List<int> { ( int ) UserPosition.System, ( int ) UserPosition.Director };

    public enum UserPosition
    {
      System,
      Director,
      TeamLead,
      SubLead,
      PM,
      Official,
      Probationary,
      Internship
    }

    public enum UserState
    {
      Available,
      WFH,
      Business,
      Busy,
      Unavailable
    }

    public enum Status
    {
      Inactive,
      Active
    }

    public enum WfhRequestStatus
    {
      Pending,
      Approve,
      Reject
    }

    public enum LeaveRequestPaid
    {
      Salary,
      SocialInsurance,
      Unpaid
    }

    public enum LeaveRequestStatus
    {
      Pending,
      Approve,
      Reject
    }

    public enum LeaveType
    {
      SelfWedding,
      FamilyWedding,
      FamilyBereavement,
      RelativeBereavement,
      SelfMaternity,
      FamilyMaternity,
      Annual,
      Unpaid,
    }

    public enum LeaveHistoryType
    {
      AddAnnualLeave,
      TakeBackAnnualLeave,
      UsingAnnualLeave,
    }

    public enum ProjectStatus
    {
      Pending,
      Active,
      End
    }
    public enum HolidayStatus
    {
      Pending,
      Apply,
      Deleting
    }

    public enum HolidayType
    {
      Holiday,
      Special
    }

    public enum DynamicOption
    {
      User,
      Team,
      //Group,
      //Position
    }
    public enum BusinessType
    {
      Increment,
      ProductRepair,
      ProductBroken,
      ProductLost,
      Edit
    }
    public enum TimesheetDetailType
    {
      /// <summary>
      /// type working log
      /// </summary>
      Project,
      /// <summary>
      /// type leave request approved, request không bắt đầu của ngày
      /// </summary>
      UnpaidLeave,
      /// <summary>
      /// type leave request approved, request không bắt đầu của ngày
      /// </summary>
      PaidLeave,
    }
    public enum AllcationType
    {
      Distribute,
      Recovery
    }

    public enum SystemRequestType
    {
      ActiveUser, // có user được active, cần thêm timesheet cho user này
      InactiveUser, // có user bị inactive, cần xóa timesheet của user này đi
      DefineTimesheetNextMonth, // định kì tạo timesheet cho các user đang active
      DefineAnnualLeaveNextMonth, // định kì tạo ngày nghỉ tự động cho user
      TakeBackLeave, // định kì thu hồi ngày nghỉ hết hạn
      DefineAnnualLeaveToOffical, // định kì thu hồi ngày nghỉ hết hạn
    }
    public enum Week
    {
      Monday,
      Tuesday,
      Wednesday,
      Thursday,
      Friday,
      Saturday,
      Sunday
    }

    public enum NotificationType
    {
      Information,
      Approved,
      Rejected
    }

    /// <summary>
    /// apply cho object type = 1
    /// </summary>
    public enum NotificationObjectId
    {
      WelcomeNewStaff,
      RequestResetPassword,
    }

    public enum NotificationObjectType
    {
      Notification,
      LeaveRequest,
      WfhRequest,
    }

    public enum NotificationAssignStatus
    {
      Unread,
      Read
    }

    public enum NotificationInvitedStatus
    {
      Pending,
      Accept,
      Reject
    }
    public enum ProductStatus
    {
      Using,
      UnUsed,
      Lost,
      Broken,
      Repair
    }

    public enum StatusMark
    {
      Current,
      Next,
      Done,
      Pending,
      None
    }

    public enum ReportType
    {
      Bug,
      New,
      Update,
      Refer
    }
    public enum ReportStatus
    {
      Open,
      Process,
      Done,
      Pending,
      Closed
    }
    public enum AssetStatus
    {
      InStorage,
      InUse,
      UnderMaintenance,
      Disposed,
      Damaged,
      Null
    }

    public enum AssetCondition
    {
      New,
      Good,
      Fair,
      Poor,
      Damaged,
      Excellent
    }

    public enum AllocationEventType
    {
      Allocate,
      Transfer,
      Return,
      Dispose,
      Destroy,
    }
    public enum MaintenanceType
    {
      Repair,
      Maintenance,
      Upgrade,
      Inspection
    }

    public enum MaintenanceStatus
    {
      Pending,
      InProgress,
      Completed,
      Cancelled
    }

    public enum ComponentCategory
    {
      RAM,
      SSD,
      HDD,
      CPU,
      GPU,
      Adapter,
      Battery,
      Cable,
      Other
    }

    public enum StockMovementType
    {
      StockIn,
      StockOut,
      Adjustment,
      Return
    }
    public enum AssetITEquipmentType
    {
      PC,
      Laptop
    }

    public enum PayrollStatus
    {
      Draft = 0,             // HR tạo / tính lương
      LeadConfirmed = 1,     // Lead/Manager xác nhận công & OT
      HRApproved = 2,        // HR tổng hợp, chốt lương
      DirectorApproved = 3,  // Giám đốc duyệt ngân sách
      Paid = 4,              // Đã thanh toán
      Canceled = 5,          // Đã hủy
      Rejected = 6,          // Bị từ chối (Lead hoặc Director) — trả lại cho HR xem xét
    }

    public enum DeductionType
    {
      SocialInsurance,
      HealthInsurance,
      UnemploymentInsurance,
      IncomeTax,
      Other
    }

    public enum OvertimeDayType
    {
      Normal,
      Weekend,
      Holiday
    }
  }

  /// <summary>
  /// bộ role của hệ thống, tương đương với các action 
  /// </summary>
  public static class Roles
  {
    public const string ROLE_NAME_SYSTEM = "System";
    public const string ROLE_NAME_DIRECTOR = "Director";
    public const string ROLE_NAME_TEAMLEAD = "Team Lead";
    public const string ROLE_NAME_SUBLEAD = "Sub Lead";
    public const string ROLE_NAME_PM = "PM";
    public const string ROLE_NAME_OFFICIAL = "Official Staff";
    public const string ROLE_NAME_PROBATIONARY = "Probationary Staff";
    public const string ROLE_NAME_INTERNSHIP = "Internship";
    public const string ROLE_NAME_HR = "HR";

    public const string ROLE_STAFF_DASHBOARD = "staff:dashboard";
    public const string ROLE_STAFF_TIMESHEET = "staff:timesheet";
    public const string ROLE_STAFF_TIMESHEET_OTHER = "staff:timesheet:other";
    public const string ROLE_STAFF_LEAVE = "staff:leave";
    public const string ROLE_STAFF_CALENDAR = "staff:calendar";
    public const string ROLE_STAFF_ASSET = "staff:asset";
    public const string ROLE_STAFF_WFH = "staff:wfh";
    public const string ROLE_STAFF_USER_REPORT = "staff:report";

    public const string ROLE_ADMIN_USER = "admin:user";
    public const string ROLE_ADMIN_TEAM = "admin:team";
    public const string ROLE_ADMIN_GROUP = "admin:group";
    public const string ROLE_ADMIN_ROLE = "admin:role";
    public const string ROLE_ADMIN_PROJECT = "admin:project";
    public const string ROLE_ADMIN_PROJECT_CONTRACT = "admin:contract";
    public const string ROLE_ADMIN_ANALYSIS = "admin:analysis";
    public const string ROLE_ADMIN_CATEGORY = "admin:category";
    public const string ROLE_ADMIN_OBJECT = "admin:object";
    public const string ROLE_ADMIN_FINGERPRINT = "admin:fingerprint";
    public const string ROLE_ADMIN_SWAPDAY = "admin:swapday";
    public const string ROLE_ADMIN_LEAVE = "admin:leaveform";
    public const string ROLE_ADMIN_ASSET = "admin:asset";
    public const string ROLE_ADMIN_PRODUCT = "admin:product";
    public const string ROLE_ADMIN_BILL = "admin:bill";
    public const string ROLE_ADMIN_WFH = "admin:wfh";
    public const string ROLE_ADMIN_PAYROLL = "admin:payroll";
    public const string ROLE_ADMIN_CONFIG = "admin:config";
    public const string ROLE_ADMIN_DASHBOARD = "admin:dashboard";
    public const string ROLE_ADMIN_USER_REPORT = "admin:report";

    public const string PERMISSION_VIEW = "view";
    public const string PERMISSION_CREATE = "create";
    public const string PERMISSION_UPDATE = "edit";
    public const string PERMISSION_DELETE = "delete";
    public const string PERMISSION_INACTIVE = "inactive";
    public const string PERMISSION_APPROVE = "approve";
    public const string PERMISSION_LOCK = "lock";
    public const string PERMISSION_HOLIDAY = "holiday";
    public const string PERMISSION_REPORT = "report";
    public const string PERMISSION_PRODUCT = "product";
    public const string PERMISSION_BILL = "bill";
    public const string PERMISSION_ASSIGN = "assign";
    public const string PERMISSION_SETTING = "setting";

    public const string PERMISSION_VIEW_NAME = "view";
    public const string PERMISSION_CREATE_NAME = "create";
    public const string PERMISSION_UPDATE_NAME = "edit";
    public const string PERMISSION_DELETE_NAME = "delete";
    public const string PERMISSION_INACTIVE_NAME = "inactive";
    public const string PERMISSION_APPROVE_NAME = "approve";
    public const string PERMISSION_LOCK_NAME = "lock";
    public const string PERMISSION_HOLIDAY_NAME = "holiday";
    public const string PERMISSION_REPORT_NAME = "report";
    public const string PERMISSION_PRODUCT_NAME = "product";
    public const string PERMISSION_BILL_NAME = "bill";
    public const string PERMISSION_ASSIGN_NAME = "assign";
    public const string PERMISSION_SETTING_NAME = "setting";
  }

  /// <summary>
  /// bộ Policy của hệ thống, tương ứng với các api
  /// </summary>
  public static class Policies
  {
    #region user
    public const string USER_GET_OPTION = "UserGetOption";
    public const string USER_GET_DYNAMIC_OPTION = "UserGetDynamicOption";
    public const string USER_GET_LIST = "UserGetList";
    public const string USER_GET_DETAIL = "UserGetDetail";
    public const string USER_CREATE = "UserCreate";
    public const string USER_UPDATE = "UserUpdate";
    public const string USER_CHANGE_STATUS = "UserChangeStatus";
    public const string USER_RESET_PASS = "UserResetPass";
    public const string USER_GET_PM_OPTION = "UserGetPmOption";
    #endregion

    #region team
    public const string TEAM_GET_OPTION = "TeamGetOption";
    public const string TEAM_GET_LIST = "TeamGetList";
    public const string TEAM_CREATE = "TeamCreate";
    public const string TEAM_UPDATE = "TeamUpdate";
    public const string TEAM_DELETE = "TeamDelete";
    #endregion

    #region role
    public const string ROLE_GET_OPTION = "RoleGetOption";
    public const string ROLE_GET_LIST = "RoleGetList";
    public const string ROLE_GET_DETAIL = "RoleGetDetail";
    public const string ROLE_GET_COLLECTION = "RoleGetCollection";
    public const string ROLE_CREATE = "RoleCreate";
    public const string ROLE_UPDATE = "RoleUpdate";
    public const string ROLE_DELETE = "RoleDelete";
    #endregion

    #region project
    public const string PROJECT_GET_OPTION_WORKING = "ProjectGetOptionWorking";
    public const string PROJECT_GET_OPTION = "ProjectGetOption";
    public const string PROJECT_GET_LIST = "ProjectGetList";
    public const string PROJECT_GET_DETAIL = "ProjectGetDetail";
    public const string PROJECT_CREATE = "ProjectCreate";
    public const string PROJECT_UPDATE = "ProjectUpdate";
    public const string PROJECT_DELETE = "ProjectDelete";
    #endregion

    #region analysis
    public const string ANALYSIS_VIEW = "AnalysisView";
    public const string ANALYSIS_REPORT = "AnalysisReport";
    #endregion

    #region other
    public const string OTHER_GET_OPTION_STATUS = "OtherGetOptionStatus";
    public const string OTHER_GET_OPTION_CLIENT = "OtherGetOptionClient";
    public const string OTHER_GET_OPTION_STATE_USER = "OtherGetOptionStateUser";
    public const string OTHER_GET_OPTION_TYPE_PROJECT = "OtherGetOptionTypeProject";
    public const string OTHER_GET_OPTION_STATUS_PROJECT = "OtherGetOptionStatusProject";
    public const string OTHER_GET_NEW_PROJECT_NUMBER = "OtherGetNewProjectNumber";
    public const string OTHER_GET_FISCAL_NUMBER = "OtherGetFiscalNumber";
    #endregion  

    #region category
    public const string CATEGORY_GET_OPTION = "CategoryGetOption";
    public const string CATEGORY_GET_LIST = "CategoryGetList";
    public const string CATEGORY_CREATE = "CategoryCreate";
    public const string CATEGORY_UPDATE = "CategoryUpdate";
    public const string CATEGORY_DELETE = "CategoryDelete";
    #endregion

    #region group
    public const string GROUP_GET_OPTION = "GroupGetOption";
    public const string GROUP_GET_LIST = "GroupGetList";
    public const string GROUP_GET_DETAIL = "GroupGetDetail";
    public const string GROUP_CREATE = "GroupCreate";
    public const string GROUP_UPDATE = "GroupUpdate";
    public const string GROUP_DELETE = "GroupDelete";
    #endregion

    #region finger printer
    public const string FINGERPRINTER_GET_LIST = "FingerPrinterGetList";
    public const string FINGERPRINTER_CREATE = "FingerPrinterCreate";
    public const string FINGERPRINTER_UPDATE = "FingerPrinterUpdate";
    public const string FINGERPRINTER_DELETE = "FingerPrinterDelete";
    #endregion

    #region wfh
    public const string WFH_GET_REQUEST_LIST = "WfhGetRequestList";
    public const string WFH_GET_LIST = "WfhGetList";
    public const string WFH_CREATE = "WfhCreate";
    public const string WFH_UPDATE = "WfhUpdate";
    public const string WFH_DELETE = "WfhDelete";
    public const string WFH_APPROVE = "WfhApprove";
    #endregion

    #region holiday
    public const string HOLIDAY_INFOR = "HolidayGetInfor";
    public const string HOLIDAY_GET_LIST = "HolidayGetList";
    public const string HOLIDAY_CREATE = "HolidayCreate";
    public const string HOLIDAY_DELETE = "HolidayDelete";
    #endregion

    #region leave form
    public const string LEAVE_GET_REQUEST_LIST = "LeaveGetRequestList";
    public const string LEAVE_GET_LIST = "LeaveGetList";
    public const string LEAVE_CREATE = "LeaveCreate";
    public const string LEAVE_UPDATE = "LeaveUpdate";
    public const string LEAVE_DELETE = "LeaveDelete";
    public const string LEAVE_APPROVE = "LeaveApprove";
    public const string LEAVE_REPORT = "LeaveReport";
    #endregion

    #region timesheet
    public const string TIMESHEET_GET_LIST = "TimesheetGetList";
    public const string TIMESHEET_GET_DETAIL = "TimesheetGetDetail";
    public const string TIMESHEET_CREATE = "TimesheetCreate";
    public const string TIMESHEET_LOCK = "TimesheetLock";
    public const string TIMESHEET_VIEW_OTHER = "TimesheetViewOther";
    public const string TIMESHEET_REPORT = "TimesheetReport";
    #endregion

    #region swap day
    public const string SWAPDAY_GET_LIST = "SwapDayGetList";
    public const string SWAPDAY_CREATE = "SwapDayCreate";
    public const string SWAPDAY_UPDATE = "SwapDayUpdate";
    public const string SWAPDAY_DELETE = "SwapDayDelete";
    #endregion   

    #region Product
    public const string PRODUCT_GET_OPTION = "ProductGetOption";
    public const string PRODUCT_TYPE_GET_OPTION = "ProductTypeGetOption";
    public const string PRODUCT_GET_LIST = "ProductGetList";
    public const string PRODUCT_GET_DETAIL = "ProductGetDetail";
    public const string ASSET_CREATE = "ProductCreate";
    public const string PRODUCT_UPDATE = "ProductUpdate";
    public const string PRODUCT_CHANGE_STATUS = "ProductChangeStatus";
    public const string PRODUCT_DELETE = "ProductDelete";
    #endregion

    #region Product Item
    public const string PRODUCT_ITEM_GET_LIST = "ProductItemGetList";
    public const string PRODUCT_ITEM_GET_OPTION = "ProductItemGetOption";
    public const string PRODUCT_ITEM_HISTORY = "ProductItemHistory";
    public const string PRODUCT_ITEM_UPDATE = "ProductItemUpdate";
    #endregion

    #region bill
    public const string BILL_GET_LIST = "BillGetList";
    public const string BILL_GET_DETAIL = "BillGetDetail";
    public const string BILL_CREATE = "BillCreate";
    public const string BILL_UPDATE = "BillUpdate";
    public const string BILL_DELETE = "BillDelete";
    public const string BILL_SYNC = "BillSync";
    #endregion

    #region config
    public const string CONFIG_GET_LIST = "ConfigGetList";
    public const string CONFIG_CREATE = "ConfigCreate";
    public const string CONFIG_UPDATE = "ConfigUpdate";
    public const string CONFIG_DELETE = "ConfigDelete";
    #endregion

    #region contract
    public const string CONTRACT_GET_LIST = "ContractGetList";
    public const string CONTRACT_CREATE = "ContractCreate";
    public const string CONTRACT_UPDATE = "ContractUpdate";
    public const string CONTRACT_DELETE = "ContractDelete";
    #endregion

    public enum EmailTemplates
    {
      AboutProject
    }
  }
}
