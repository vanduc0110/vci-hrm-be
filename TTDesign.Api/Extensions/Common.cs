using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Extensions
{
  public static class Common
  {
    public static int NetworkDays( DateTime startDate, DateTime endDate, List<DateTime> holidays = null )
    {
      if ( startDate > endDate ) {
        var temp = startDate;
        startDate = endDate;
        endDate = temp;
      }

      int networkDays = 0;
      var currentDate = startDate;

      var holidaySet = new HashSet<DateTime>();
      if ( holidays != null ) {
        foreach ( var holiday in holidays ) {
          holidaySet.Add( holiday.Date );
        }
      }
      while ( currentDate <= endDate ) {
        if ( IsWorkingDay( currentDate, holidaySet ) ) {
          networkDays++;
        }
        currentDate = currentDate.AddDays( 1 );
      }

      return networkDays;
    }
    private static bool IsWorkingDay( DateTime date, HashSet<DateTime> holidays )
    {
      if ( date.DayOfWeek == DayOfWeek.Sunday ) {
        return false;
      }
      if ( holidays != null && holidays.Contains( date.Date ) ) {
        return false;
      }
      return true;
    }
    /// <summary>
    /// tạo format nội dung product history
    /// </summary>
    /// <param name="status"></param>
    /// <param name="price"></param>
    /// <param name="unit"></param>
    /// <param name="user"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string GenerateContentProductHistory( int status, double price = 0, string unit = null!, string user = null!, string content = null! )
    {
      switch ( status ) {
        case 0: // Enums.ProductItemStatus.New
          return $"Instock {price} {unit}";
        case 2: // Enums.ProductItemStatus.Using
          return $"{user} had using";
        default:
          return content;
      }
    }

    /// <summary>
    /// khởi tạo thông tin notification path
    /// </summary>
    /// <param name="noti"></param>
    /// <returns></returns>
    public static NotificationPath DefinePathNotification( NotificationAssign noti )
    {
      if ( noti.Notification.ObjectType == ( int ) Enums.NotificationObjectType.Notification )
        return null!;
      return new NotificationPath()
      {
        ObjectId = noti.Notification.ObjectId,
        PathUrl = noti.Notification.Type == ( int ) Enums.NotificationType.Information && noti.Notification.ObjectType == ( int ) Enums.NotificationObjectType.LeaveRequest ? "/admin/leaveform" :
          noti.Notification.Type != ( int ) Enums.NotificationType.Information && noti.Notification.ObjectType == ( int ) Enums.NotificationObjectType.LeaveRequest ? "/leaveform" :
          noti.Notification.Type == ( int ) Enums.NotificationType.Information && noti.Notification.ObjectType == ( int ) Enums.NotificationObjectType.WfhRequest ? "/admin/wfh" :
          noti.Notification.Type != ( int ) Enums.NotificationType.Information && noti.Notification.ObjectType == ( int ) Enums.NotificationObjectType.WfhRequest ? "/wfh" :
          null!,
        TabIndex = 0,
      };
    }

    /// <summary>
    /// format nội dung notification theo đối tượng Object và trạng thái
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    public static string FormatContentNotification( Notification notification )
    {
      if ( notification.ObjectType == ( int ) Enums.NotificationObjectType.LeaveRequest ||
        notification.ObjectType == ( int ) Enums.NotificationObjectType.WfhRequest ) {
        return notification.Type == ( int ) Enums.NotificationType.Approved ? $"{notification.Content} had approved" :
          notification.Type == ( int ) Enums.NotificationType.Rejected ? $"{notification.Content} had rejected" :
          $"({notification.UserName}) {notification.Content}";
      }
      else {
        return notification.Content;
      }
    }

    /// <summary>
    /// mapping leave history type tương ứng với leave request type khi thực hiện approve/reject request leave
    /// </summary>
    /// <param name="leaveRequestType"></param>
    /// <returns></returns>
    public static int MappingLeaveRequestTypeToLeaveHistoryTypeWhenCreateHistoryApproveRequest( int leaveRequestType )
    {
      if ( leaveRequestType == ( int ) Enums.LeaveType.Annual ) {
        return ( int ) Enums.LeaveHistoryType.UsingAnnualLeave;
      }
      return -1;
    }

    /// <summary>
    /// format lại giá trị hour input theo block 15'
    /// </summary>
    /// <param name="hour"></param>
    /// <returns></returns>
    public static double FormatHourInputToData( double hour )
    {
      var roundUp = Math.Floor( hour * 100 );
      if ( roundUp % 100 >= 0 && roundUp % 100 < 25 ) {
        return ( int ) roundUp / 100;
      }
      else if ( roundUp % 100 >= 25 && roundUp % 100 < 50 ) {
        return ( int ) roundUp / 100 + 0.25;
      }
      else if ( roundUp % 100 >= 50 && roundUp % 100 < 75 ) {
        return ( int ) roundUp / 100 + 0.50;
      }
      else {
        return ( int ) roundUp / 100 + 0.75;
      }
    }

    /// <summary>
    /// format từ số giờ kiểu double ra format HH:mm
    /// </summary>
    /// <param name="hours"></param>
    /// <returns></returns>
    public static string FormatHoursDoubleToString( double hours )
    {
      var minutes = hours * 60;
      var hour = ( int ) minutes / 60;
      return hour.ToString( "00" ) + ":" + ( minutes - hour * 60 ).ToString( "00" );
    }

    /// <summary>
    /// lấy năm tài chính hiện tại
    /// </summary>
    /// <returns></returns>
    public static int GetFiscalYear()
    {
      // 2025/6 là bắt đầu của kì tài chính số 1
      if ( DateTime.UtcNow.Month <= 6 ) {
        return DateTime.UtcNow.Year - 2025;
      }
      return DateTime.UtcNow.Year - 2025 + 1;
    }

    /// <summary>
    /// format project name đầy đủ
    /// </summary>
    /// <param name="project"></param>  

    public static string FormatProjectName( Project project )
    {
      return $"{project.Code}-{project.Name}";
    }
    public static double TotalTimeWorkExcludeWeekend( DateTime start, DateTime end )
    {
      double minutes;
      start = start.AddSeconds( -start.Second );
      end = end.AddSeconds( -end.Second );
      if ( start.Hour < Enums.TimeWorkEndHourMorning ) {
        minutes = end.Hour < Enums.TimeWorkEndHourMorning ? ( end - start ).TotalMinutes :
          end.Hour < Enums.TimeWorkStartHourAfternoon ? ( end.Date.AddHours( Enums.TimeWorkEndHourMorning ) - start ).TotalMinutes :
          ( end - start ).TotalMinutes - 60;
      }
      else if ( start.Hour < Enums.TimeWorkStartHourAfternoon ) {
        minutes = end.Hour < Enums.TimeWorkStartHourAfternoon ? 0 : ( end - start.Date.AddHours( Enums.TimeWorkStartHourAfternoon ) ).TotalMinutes;
      }
      else {
        minutes = ( end - start ).TotalMinutes;
      }
      int partInt = ( int ) minutes / Enums.MINIMUM_PERIOD_CALENDAR;
      return ( double ) ( partInt * Enums.MINIMUM_PERIOD_CALENDAR ) / 60;
    }
    /// <summary>
    /// tính thời gian làm việc, phân theo từng ngày, đơn vị phút
    /// </summary>
    /// <param name="start">đơn vị tới phút</param>
    /// <param name="end">đơn vị tới phút</param>
    /// <param name="type">minute/ hour/ day</param>
    /// <param name="holiday">bỏ qua các ngày nghỉ lễ</param>
    /// <returns></returns>
    public static List<LeaveRequestDetailData> TotalTimeWorkExcludeWeekend( DateTime start, DateTime end, string type, Holiday? holiday = null )
    {
      start = start.AddSeconds( -start.Second );
      end = end.AddSeconds( -end.Second );
      List<LeaveRequestDetailData> dates = new();
      var startCheck = start.Hour < Enums.TimeWorkStartHour ? start.Date.AddHours( Enums.TimeWorkStartHour ) : start;
      var endCheck = start;
      do {
        if ( startCheck.DayOfWeek == DayOfWeek.Sunday ||
          ( holiday is not null && startCheck >= holiday.StartDate && startCheck <= holiday.EndDate ) ) {
          startCheck = endCheck = startCheck.Date.AddDays( 1 ).AddHours( Enums.TimeWorkStartHour );
          continue;
        }
        endCheck = endCheck.Date == end.Date ? end : startCheck.Date.AddHours( Enums.TimeWorkEndHour );
        if ( startCheck.Hour >= Enums.TimeWorkStartHour && startCheck.Hour <= Enums.TimeWorkEndHourMorning ) {
          if ( endCheck.Hour >= Enums.TimeWorkStartHour && endCheck.Hour <= Enums.TimeWorkEndHourMorning ) {
            dates.Add( new LeaveRequestDetailData
            {
              Date = startCheck.Date,
              Hours = ( endCheck - startCheck ).TotalMinutes
            } );
          }
          else if ( endCheck.Hour > Enums.TimeWorkEndHourMorning && endCheck.Hour < Enums.TimeWorkStartHourAfternoon ) {
            if ( ( startCheck.Date.AddHours( Enums.TimeWorkEndHourMorning ) - startCheck ).TotalMinutes == 0 )
              continue;
            dates.Add( new LeaveRequestDetailData
            {
              Date = startCheck.Date,
              Hours = ( startCheck.Date.AddHours( Enums.TimeWorkEndHourMorning ) - startCheck ).TotalMinutes
            } );
          }
          else {
            if ( ( endCheck - startCheck ).TotalMinutes - 60 == 0 )
              continue;
            dates.Add( new LeaveRequestDetailData
            {
              Date = startCheck.Date,
              Hours = ( endCheck - startCheck ).TotalMinutes - 60
            } );
          }
        }
        else if ( startCheck.Hour > Enums.TimeWorkEndHourMorning && startCheck.Hour < Enums.TimeWorkStartHourAfternoon ) {
          if ( endCheck.Hour > Enums.TimeWorkEndHourMorning && endCheck.Hour < Enums.TimeWorkStartHourAfternoon ) {
            continue;
          }
          dates.Add( new LeaveRequestDetailData
          {
            Date = startCheck.Date,
            Hours = ( endCheck - startCheck.Date.AddHours( Enums.TimeWorkStartHourAfternoon ) ).TotalMinutes
          } );
        }
        else {
          dates.Add( new LeaveRequestDetailData
          {
            Date = startCheck.Date,
            Hours = ( endCheck - startCheck ).TotalMinutes
          } );
        }
        startCheck = endCheck = startCheck.Date.AddDays( 1 ).AddHours( Enums.TimeWorkStartHour );
      } while ( endCheck < end );
      foreach ( var date in dates ) {
        switch ( type ) {
          case "day":
            int partInt = ( int ) date.Hours / 240;
            date.Hours = FormatHourInputToData( ( double ) ( partInt * 240 ) / 480 );
            break;
          case "hour":
            partInt = ( int ) date.Hours / 15;
            date.Hours = FormatHourInputToData( ( double ) ( partInt * 15 ) / 60 );
            break;
          default:
            partInt = ( int ) date.Hours / 15;
            date.Hours = FormatHourInputToData( partInt * 15 );
            break;
        }
      }
      return dates;
    }

    /// <summary>
    /// từ type leave suy ra paid by
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string PaidByLeaveRequest( int type )
    {
      switch ( type ) {
        case 0: // "SelfWedding":
          return Enum.GetName( Enums.LeaveRequestPaid.Salary )!;
        case 1: // "FamilyWedding":
          return Enum.GetName( Enums.LeaveRequestPaid.Salary )!;
        case 2: // "FamilyBereavement":
          return Enum.GetName( Enums.LeaveRequestPaid.Salary )!;
        case 3: // "RelativeBereavement":
          return Enum.GetName( Enums.LeaveRequestPaid.Salary )!;
        case 4: // "SelfMaternity": // trường hợp đặc biệt nghỉ sinh, đơn vị là month
          return Enum.GetName( Enums.LeaveRequestPaid.SocialInsurance )!;
        case 5: // "FamilyMaternity":
          return Enum.GetName( Enums.LeaveRequestPaid.SocialInsurance )!;
        case 7: // "Annual":
          return Enum.GetName( Enums.LeaveRequestPaid.Salary )!;
        case 8: // "Unpaid":
          return Enum.GetName( Enums.LeaveRequestPaid.Unpaid )!;
      }
      return string.Empty;
    }

    public static Dictionary<string, RoleModel []> CollectionRole()
    {
      return new Dictionary<string, RoleModel []>
      {
        { "(Personal) Dashboard", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_STAFF_DASHBOARD, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
        } },
        { "(Personal) Timesheet", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_STAFF_TIMESHEET, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
        } },
        { "(Personal) Timesheet.Other", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_STAFF_TIMESHEET_OTHER, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_STAFF_TIMESHEET_OTHER, Value = Roles.PERMISSION_LOCK, Name = Roles.PERMISSION_LOCK_NAME },
        } },
        { "(Personal) Leave", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_STAFF_LEAVE, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
        } },
        { "(Personal) WFH", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_STAFF_WFH, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
        } },
        { "(Personal) Report", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_STAFF_USER_REPORT, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
        } },
        { "(Admin) Dashboard", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_DASHBOARD, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
        } },
        { "(Admin) User", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER, Value = Roles.PERMISSION_INACTIVE, Name = Roles.PERMISSION_INACTIVE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) Team", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_TEAM, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_TEAM, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_TEAM, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_TEAM, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) Group", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_GROUP, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_GROUP, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_GROUP, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_GROUP, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) Role", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ROLE, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ROLE, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ROLE, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ROLE, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) Project", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) Analysis", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ANALYSIS, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME }, // quyền vào tab analysis của admin:timesheet
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ANALYSIS, Value = Roles.PERMISSION_REPORT, Name = Roles.PERMISSION_REPORT_NAME }, // quyền vào tab report của admin:timesheet
        } },
        { "(Admin) Category", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CATEGORY, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CATEGORY, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CATEGORY, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CATEGORY, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) Fingerprint", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_FINGERPRINT, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_FINGERPRINT, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_FINGERPRINT, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) SwapDay", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_SWAPDAY, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_SWAPDAY, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_SWAPDAY, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_SWAPDAY, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) Leaveform", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_LEAVE, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_LEAVE, Value = Roles.PERMISSION_APPROVE, Name = Roles.PERMISSION_APPROVE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_LEAVE, Value = Roles.PERMISSION_REPORT, Name = Roles.PERMISSION_REPORT_NAME }, // quyền vào tab summary của admin:leave
          new RoleModel(){ Type = Roles.ROLE_ADMIN_LEAVE, Value = Roles.PERMISSION_HOLIDAY, Name = Roles.PERMISSION_HOLIDAY_NAME }, // quyền vào tab holiday của admin:leave
        } },
        { "(Admin) Asset", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ASSET, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME }, // quyền vào tab request của admin:asset
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ASSET, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ASSET, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME }, // màn hình tổng hợp theo từng user
          new RoleModel(){ Type = Roles.ROLE_ADMIN_ASSET, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME }, // cho phép phân product cho request approve
        } },
        { "(Admin) Product", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PRODUCT, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PRODUCT, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PRODUCT, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PRODUCT, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
        { "(Admin) WFH", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_WFH, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_WFH, Value = Roles.PERMISSION_APPROVE, Name = Roles.PERMISSION_APPROVE_NAME },
        } },
          { "(Admin) Config", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CONFIG, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CONFIG, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CONFIG, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_CONFIG, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
          { "(Admin) Contract", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT_CONTRACT, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT_CONTRACT, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT_CONTRACT, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_PROJECT_CONTRACT, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
          { "(Admin) Report", new RoleModel[] {
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER_REPORT, Value = Roles.PERMISSION_VIEW, Name = Roles.PERMISSION_VIEW_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER_REPORT, Value = Roles.PERMISSION_CREATE, Name = Roles.PERMISSION_CREATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER_REPORT, Value = Roles.PERMISSION_UPDATE, Name = Roles.PERMISSION_UPDATE_NAME },
          new RoleModel(){ Type = Roles.ROLE_ADMIN_USER_REPORT, Value = Roles.PERMISSION_DELETE, Name = Roles.PERMISSION_DELETE_NAME },
        } },
      };
    }

    public static string FormatEmail( string str )
    {
      return Regex.Replace( str.Trim(), @"\s+", string.Empty );
    }

    public static string RemoveMultiBlank( string str )
    {
      return Regex.Replace( str.Trim(), @"\s+", " " );
    }

    public static string FormatCodeInput( string input )
    {
      return Regex.Replace( input.Trim(), @"\s+", string.Empty ).ToUpper();
    }

    public static string UpperFirstLetter( string input )
    {
      return char.ToUpper( input [ 0 ] ) + input.Substring( 1 );
    }

    public static string FormatUserNameFromFullName( string fullName )
    {
      var temp = RemoveSign4VietnameseString( Regex.Replace( fullName.Trim(), @"\s+", " " ) ).Split( " " ).ToList();
      var name = temp.Last().ToLower();
      temp = temp.Select( s => s [ 0 ].ToString().ToLower() ).ToList();
      temp.RemoveAt( temp.Count - 1 );
      return ( temp.Count() > 0 ? string.Join( ".", temp ) + "." : string.Empty ) + name;
    }

    private static readonly string [] VietnameseSigns = new string []
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

    public static string RemoveSign4VietnameseString( string str )
    {
      for ( int i = 1; i < VietnameseSigns.Length; i++ ) {
        for ( int j = 0; j < VietnameseSigns [ i ].Length; j++ )
          str = str.Replace( VietnameseSigns [ i ] [ j ], VietnameseSigns [ 0 ] [ i - 1 ] );
      }
      return str;
    }

    /// <summary>
    /// Valid user admin
    /// </summary>
    /// <param name="position"></param>
    /// <returns>true: thuộc nhóm admin, false: not</returns>
    public static bool ValidRoleAdmin( int position )
    {
      return Enums.ADMIN_POSITION.Contains( position );
    }

    public static string GeneratedCode( string name )
    {
      if ( string.IsNullOrWhiteSpace( name ) )
        return string.Empty;

      string normalized = RemoveDiacritics( name.ToLower().Trim() );

      var parts = normalized.Split( ' ', StringSplitOptions.RemoveEmptyEntries );
      StringBuilder code = new StringBuilder();

      foreach ( var part in parts ) {
        if ( !string.IsNullOrEmpty( part ) ) {
          if ( Regex.IsMatch( part, @"^\d+$" ) ) {
            code.Append( part );
          }
          else if ( char.IsLetter( part [ 0 ] ) ) {
            code.Append( char.ToUpper( part [ 0 ] ) );
          }
        }
      }

      return code.ToString();
    }
    public static string RemoveDiacritics( string text )
    {
      string normalized = text.Normalize( NormalizationForm.FormD );
      StringBuilder result = new StringBuilder();

      foreach ( char c in normalized ) {
        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory( c );
        if ( unicodeCategory != UnicodeCategory.NonSpacingMark ) {
          result.Append( c );
        }
      }
      return result.ToString().Normalize( NormalizationForm.FormC );
    }
    //public static bool IsWorkingSaturday( this DateTime date )
    //{
    //  DateTime referenceDate = new DateTime( 2025, 6, 1 );
    //  DayOfWeek startOfWeek = DayOfWeek.Sunday;

    //  if ( date.DayOfWeek != DayOfWeek.Saturday )
    //    return false;

    //  var startOfReferenceWeek = GetStartOfWeek( referenceDate, startOfWeek );

    //  var startOfTargetWeek = GetStartOfWeek( date, startOfWeek );

    //  int weeksDiff = ( int ) ( ( startOfTargetWeek - startOfReferenceWeek ).TotalDays / 7 );

    //  return weeksDiff % 2 == 0;
    //}

    //public static DateTime GetStartOfWeek( DateTime date, DayOfWeek startOfWeek )
    //{
    //  int diff = ( 7 + ( date.DayOfWeek - startOfWeek ) ) % 7;
    //  return date.AddDays( -diff ).Date;
    //}
    public static bool IsProjectManager( string projectManagement, long userId )
    {
      var pm = projectManagement.Split( ',' ).Select( long.Parse ).ToList();
      return pm.Contains( userId );
    }
    public static double GetValue( Dictionary<int, double> source, int key )
    {
      return source.TryGetValue( key, out var value ) ? value : 0;
    }
  }
}
