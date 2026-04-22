using AutoMapper;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Security.Tokens;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Mapping
{
  public class ModelToResourceProfile : Profile
  {
    public ModelToResourceProfile()
    {
      #region Team
      CreateMap<Team, TeamResponse>();
      #endregion    
      #region User
      CreateMap<User, UserResponse>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( a.IsActive ? Enums.Status.Active : Enums.Status.Inactive ) ) )
        .ForMember( a => a.Position, opt => opt.MapFrom( a => Enum.GetName( ( Enums.UserPosition ) a.Position ) ) );
      CreateMap<UserInfo, UserResponse>();
      CreateMap<User, UserOption>()
        .ForMember( a => a.State, opt => opt.MapFrom( a => Enum.GetName( ( Enums.UserState ) a.State ) ) )
        .ForMember( a => a.Position, opt => opt.MapFrom( a => Enum.GetName( ( Enums.UserPosition ) a.Position ) ) )
        .ForMember( a => a.Teams, opt => opt.MapFrom( a => a.TeamUsers.Select( tu => new TeamUserOption
        {
          TeamId = tu.TeamId,
          TeamCode = tu.Team.Code,
          TeamName = tu.Team.Name
        } ).ToList() ) )
        .ForMember( a => a.Name, opt => opt.MapFrom( a => a.UserName ) );
      CreateMap<User, UserDetailResponse>()
       .ForMember( a => a.Teams, opt => opt.MapFrom( a => a.TeamUsers.Select( tu => new TeamUserOption
       {
         TeamId = tu.TeamId,
         TeamCode = tu.Team.Code,
         TeamName = tu.Team.Name
       } ).ToList() ) )
        .ForMember( a => a.State, opt => opt.MapFrom( a => Enum.GetName( ( Enums.UserState ) a.State ) ) )
        .ForMember( a => a.Position, opt => opt.MapFrom( a => Enum.GetName( ( Enums.UserPosition ) a.Position ) ) );
      CreateMap<UserInfo, UserDetailResponse>();
      #endregion
      #region Role
      CreateMap<Role, RoleResponse>()
        .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.RoleType ) a.Type ) ) );
      CreateMap<RoleModel, RoleModelView>();
      #endregion
      #region Category
      CreateMap<TeamCategory, CategoryResponse>();
      #endregion     
      #region Project
      CreateMap<Project, ProjectResponse>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => a.Status ) )
        .ForMember( a => a.FullName, opt => opt.MapFrom( a => Common.FormatProjectName( a ) ) )
        .ForMember( a => a.Amount, opt => opt.MapFrom( a => a.Users.Count() ) )
        .ForMember( a => a.Contracts, opt => opt.MapFrom( a => a.ProjectContracts.Count() ) )
        .ForMember( a => a.Users, opt => opt.MapFrom( a => a.Users.Select( u => new UserShort()
        {
          UserId = u.Id,
          UserName = u.FullName,
          Avatar = u.Avatar,
          TeamName = string.Join( ", ", u.TeamUsers.Select( tu => tu.Team.Name ) ),
          Position = Enum.GetName( typeof( Enums.UserPosition ), u.Position ) ?? string.Empty,
        } ) ) );
      CreateMap<Project, ProjectDetailResponse>()
        .ForMember( a => a.ClientName, opt => opt.MapFrom( a => a.Client!.Name ) )
        .ForMember( a => a.Status, opt => opt.MapFrom( a => a.Status ) ) 
        .ForMember( a => a.FullName, opt => opt.MapFrom( a => Common.FormatProjectName( a ) ) )
        .ForMember( a => a.Manager, opt => opt.MapFrom( a => a.ProjectManagement ) );
      #endregion
      #region leave
      CreateMap<LeaveInformation, LeaveInformationResponse>();
      #endregion
      #region overtime
      //CreateMap<OvertimeRequest, OvertimeRequestResponse>()
      //  //.ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.TimesheetDetailType ) a.Type ) ) )
      //  //.ForMember( a => a.Paid, opt => opt.MapFrom( a => Enum.GetName( ( Enums.OvertimeRequestPaid ) a.Paid ) ) )
      //  .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.OvertimeRequestStatus ) a.Status ) ) )
      //  .ForMember( a => a.ProjectName, opt => opt.MapFrom( a => Common.FormatProjectName( a.Project ) ) )
      //  .ForMember( a => a.Date, opt => opt.MapFrom( a => a.StartDate.Date ) )
      //  .ForMember( a => a.Hours, opt => opt.MapFrom( a => Common.MatchOvertimeRequestHour( a.StartDate, a.EndDate ) ) )
      //  .ForMember( a => a.ActualHours, opt => opt.MapFrom( a => a.OvertimeRequestDetails.Sum( o => o.ActualHour ) ) );     
      #endregion
      #region leave
      CreateMap<LeaveRequest, LeaveRequestResponse>()
        .ForMember( a => a.PaidBy, opt => opt.MapFrom( a => Common.PaidByLeaveRequest( a.Type ) ) )
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.LeaveRequestStatus ) a.Status ) ) )
        .ForMember( a => a.StartDate, opt => opt.MapFrom( a => a.StartDate.Date ) )
        .ForMember( a => a.StartTime, opt => opt.MapFrom( a => a.StartDate.ToString( Enums.HOUR_FORMAT ) ) )
        .ForMember( a => a.EndDate, opt => opt.MapFrom( a => a.EndDate.Date ) )
        .ForMember( a => a.EndTime, opt => opt.MapFrom( a => a.EndDate.ToString( Enums.HOUR_FORMAT ) ) )
        .ForMember( a => a.TimeLeave, opt => opt.MapFrom( a => a.Hour ) )
        .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.LeaveType ) a.Type ) ) );
      CreateMap<LeaveHistory, RemainLeave>()
        .ForMember( a => a.Annual, opt => opt.MapFrom( a => a.AnnualLeave ) );
      CreateMap<LeaveHistory, LeaveHistoryResponse>()
        .ForMember( a => a.State, opt => opt.MapFrom( a => a.Unit == null || a.Unit == 0 ? string.Empty : a.Unit > 0 ? "+" : "-" ) )
        .ForMember( a => a.Unit, opt => opt.MapFrom( a => a.Unit == null || a.Unit == 0 ? 0 : a.Unit > 0 ? a.Unit : 0 - a.Unit ) )
        .ForMember( a => a.Annual, opt => opt.MapFrom( a => a.AnnualLeave ) );
      #endregion
      #region wfh
      CreateMap<WfhRequest, WfhResponse>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.WfhRequestStatus ) a.Status ) ) )
        .ForMember( a => a.StartTime, opt => opt.MapFrom( a => a.StartTime.ToString( Enums.DATE_FORMAT ) ) )
        .ForMember( a => a.EndTime, opt => opt.MapFrom( a => a.EndTime.ToString( Enums.DATE_FORMAT ) ) );
      #endregion
      #region holiday
      CreateMap<Holiday, HolidayResponse>()
        .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.HolidayType ) a.Type ) ) )
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.HolidayStatus ) a.Status ) ) )
        .ForMember( a => a.Duration, opt => opt.MapFrom( a => ( a.EndDate - a.StartDate ).TotalDays + 1 ) );
      #endregion
      #region timesheet
      //CreateMap<Timesheet, TimesheetResponse>()
      //  .ForMember( a => a.TimeIn, opt => opt.MapFrom( a => a.WfhRequest == null ? a.FingerPrinter.DateIn.ToString( Enums.HOUR_FORMAT ) : a.WfhRequest.StartTime.ToString( Enums.HOUR_FORMAT ) ) )
      //  .ForMember( a => a.TimeOut, opt => opt.MapFrom( a => a.WfhRequest == null ? a.FingerPrinter.DateOut.ToString( Enums.HOUR_FORMAT ) : a.WfhRequest.EndTime.ToString( Enums.HOUR_FORMAT ) ) );
      CreateMap<TimesheetReport, TimesheetData>()
        .ForMember( a => a.Hours, opt => opt.MapFrom( a => Common.FormatHoursDoubleToString( a.Hours ) ) );
      CreateMap<TimesheetDetail, TimesheetDetailData>()
        .ForMember( a => a.CategoryId, opt => opt.MapFrom( a => a.TimesheetCategoryId ) )
        .ForMember( a => a.Hours, opt => opt.MapFrom( a => Common.FormatHoursDoubleToString( a.Hours ) ) )
        .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.TimesheetDetailType ) a.Type ) ) );
      #endregion 
      #region Notification
      CreateMap<NotificationAssign, NotificationResponse>()
        .ForMember( a => a.Id, opt => opt.MapFrom( a => a.Notification.Id ) )
        .ForMember( a => a.Title, opt => opt.MapFrom( a => a.Notification.Title ) )
        .ForMember( a => a.Content, opt => opt.MapFrom( a => Common.FormatContentNotification( a.Notification ) ) )
        .ForMember( a => a.Status, opt => opt.MapFrom( a => a.Status ) )
        .ForMember( a => a.CreatedDate, opt => opt.MapFrom( a => a.Notification.ModifiedDate ) )
        .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.NotificationType ) a.Notification.Type ) ) )
        .ForMember( a => a.Path, opt => opt.MapFrom( a => Common.DefinePathNotification( a ) ) );
      CreateMap<Notification, NotificationResponse>()
        .ForMember( a => a.Content, opt => opt.MapFrom( a => Common.FormatContentNotification( a ) ) )
        .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.NotificationType ) a.Type ) ) )
        .ForMember( a => a.CreatedDate, opt => opt.MapFrom( a => a.ModifiedDate ) );
      #endregion
     

 
      CreateMap<AccessToken, AccessTokenResponse>()
          .ForMember( a => a.AccessToken, opt => opt.MapFrom( a => a.Token ) )
          .ForMember( a => a.RefreshToken, opt => opt.MapFrom( a => a.RefreshToken.Token ) )
          .ForMember( a => a.Expiration, opt => opt.MapFrom( a => a.Expiration ) );
      CreateMap<User, UserShort>()
        .ForMember( a => a.UserId, opt => opt.MapFrom( a => a.Id ) )
        .ForMember( a => a.UserType, opt => opt.MapFrom( a => Enum.GetName( a.LockoutEnabled ? Enums.Status.Active : Enums.Status.Inactive ) ) )
        .ForMember( a => a.Position, opt => opt.MapFrom( a => Enum.GetName( ( Enums.UserPosition ) a.Position ) ) );
      CreateMap<UserSetting, UserSettingResponse>();
      #region client
      CreateMap<Client, ClientResponse>();
      #endregion   

     

      #region Config
      CreateMap<Config, ConfigResponse>();
      #endregion

      CreateMap<ProjectContract, ProjectContractResponse>()
        .ForMember( a => a.ContractCode, opt => opt.MapFrom( a => a.Code ) )
        .ForMember( a => a.ContractDate, opt => opt.MapFrom( a => a.Date ) )
        .ForMember( a => a.ContractName, opt => opt.MapFrom( a => a.Name ) );
      CreateMap<ProjectDocument, ProjectDocumentDetail>()
        .ForMember( a => a.Id, opt => opt.MapFrom( a => a.Id ) )
        .ForMember( a => a.Name, opt => opt.MapFrom( a => a.Name ) )
        .ForMember( a => a.Link, opt => opt.MapFrom( a => a.Link ) )
        .ForMember( a => a.CreatedDate, opt => opt.MapFrom( a => a.CreatedDate ) );

      CreateMap<UserTask, UserTaskResponse>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.StatusMark ) a.Status ) ) );
      CreateMap<UserReport, UserReportResponse>()
       .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.ReportStatus ) a.Status ) ) )
       .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.GetName( ( Enums.ReportType ) a.Type ) ) );

      CreateMap<AssetCategory, AssetCategoryResponse>();
      CreateMap<Asset, AssetResponse>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.AssetStatus ) a.Status ) ) )
        .ForMember( a => a.Condition, opt => opt.MapFrom( a => Enum.GetName( ( Enums.AssetCondition ) a.Condition ) ) );

      CreateMap<Asset, AssetDetailResponse>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.GetName( ( Enums.AssetStatus ) a.Status ) ) )
        .ForMember( a => a.Condition, opt => opt.MapFrom( a => Enum.GetName( ( Enums.AssetCondition ) a.Condition ) ) );
      CreateMap<Asset, AssetStorageResponse>()
        .ForMember( a => a.Condition, opt => opt.MapFrom( a => Enum.GetName( ( Enums.AssetCondition ) a.Condition ) ) );

      CreateMap<AssetAllocation, AssetAllocationResponse>()
       .ForMember( a => a.EventType, opt => opt.MapFrom( a => a.EventType! != null ? Enum.GetName( ( Enums.AllocationEventType ) a.EventType! ) : null ) )
       .ForMember( a => a.Note, opt => opt.MapFrom( a => a.StatusNotes ) );

      CreateMap<Asset, AssetStorageOption>();
    }
  }
}
