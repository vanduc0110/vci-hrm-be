using HRM.Infrastructure.Email.Implements;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Email;
using TTDesign.API.Extensions.Swagger;
using TTDesign.API.Persistence.Repositories;
using TTDesign.API.Services;

namespace TTDesign.API.Extensions
{
  public static class ServiceCollectionExtension
  {
    public static void ConfigureRepositoryManager( this IServiceCollection services )
    {

      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IClientRepository, ClientRepository>();
      services.AddScoped<IGoogleTimesheetKeyRepository, GoogleTimesheetKeyRepository>();
      services.AddScoped<ILeaveRepository, LeaveRepository>();
      services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
      services.AddScoped<IProjectRepository, ProjectRepository>();
      services.AddScoped<ITeamUserRepository, TeamUserRepository>();
      services.AddScoped<ITeamCategoryRepository, TeamCategoryRepository>();
      services.AddScoped<ITimesheetDetailRepository, TimesheetDetailRepository>();
      services.AddScoped<ITimesheetRepository, TimesheetRepository>();
      services.AddScoped<ITeamRepository, TeamRepository>();
      services.AddScoped<IUserSettingRepository, UserSettingRepository>();
      services.AddScoped<IUserInfoRepository, UserInfoRepository>();
      services.AddScoped<IRoleRepository, RoleRepository>();
      services.AddScoped<IHolidayRepository, HolidayRepository>();
      services.AddScoped<IFingerPrinterRepository, FingerPrinterRepository>();
      services.AddScoped<ILeaveInformationRepository, LeaveInformationRepository>();
      services.AddScoped<IWfhRequestRepository, WfhRequestRepository>();
      services.AddScoped<ILeaveHistoryRepository, LeaveHistoryRepository>();
      services.AddScoped<ITimesheetReportRepository, TimesheetReportRepository>();
      services.AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>();
      services.AddScoped<ISystemRequestRepository, SystemRequestRepository>();
      services.AddScoped<ILeaveHistoryUsingRepository, LeaveHistoryUsingRepository>();
      services.AddScoped<ILeaveRequestDetailRepository, LeaveRequestDetailRepository>();
      services.AddScoped<INotificationRepository, NotificationRepository>();
      services.AddScoped<INotificationAssignRepository, NotificationAssignRepository>();
      services.AddScoped<ISwapDayRepository, SwapDayRepository>();
      services.AddScoped<ISwapDayUserRepository, SwapDayUserRepository>();
      services.AddScoped<ISwapDayReferRepository, SwapDayReferRepository>();
      services.AddScoped<IConfigRepository, ConfigRepository>();
      services.AddScoped<IProjectContractRepository, ProjectContractRepository>();
      services.AddScoped<IUserTaskRepository, UserTaskRepository>();
      services.AddScoped<IUserReportRepository, UserReportRepository>();
      services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();
      services.AddScoped<IAssetRepository, AssetRepository>();
      services.AddScoped<IAssetAllocateRepository, AssetAllocateRepository>();
      services.AddScoped<IComponentRepository, ComponentRepository>();
      services.AddScoped<IAssetComponentRepository, AssetComponentRepository>();
      services.AddScoped<IFingerPrinterLogRepository, FingerPrinterLogRepository>();

      // Payroll
      services.AddScoped<ISalaryRepository, SalaryRepository>();
      services.AddScoped<IPayrollRepository, PayrollRepository>();
      services.AddScoped<IPayrollDetailRepository, PayrollDetailRepository>();
      services.AddScoped<IBonusRepository, BonusRepository>();
      services.AddScoped<ITaxBracketRepository, TaxBracketRepository>();
      services.AddScoped<ISocialInsuranceRateRepository, SocialInsuranceRateRepository>();

    }

    public static void ConfigureServiceManager( this IServiceCollection services )
    {
      services.AddScoped<IAuthenticationService, AuthenticationService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<ITeamService, TeamService>();
      services.AddScoped<ILeaveService, LeaveService>();
      services.AddScoped<ITimesheetService, TimesheetService>();
      services.AddScoped<IProjectService, ProjectService>();
      services.AddScoped<IHolidayService, HolidayService>();
      services.AddScoped<IRoleService, RoleService>();
      services.AddScoped<IOtherService, OtherService>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<IFingerPrinterService, FingerPrinterService>();
      services.AddScoped<IWfhService, WfhService>();
      services.AddScoped<INotificationService, NotificationService>();
      services.AddScoped<IClientService, ClientService>();
      services.AddScoped<IClientRepository, ClientRepository>();
      services.AddScoped<IConfigService, ConfigService>();
      services.AddScoped<IUserTaskService, UserTaskService>();
      services.AddScoped<IDashboardService, DashboardService>();
      services.AddScoped<IUserReportService, UserReportService>();
      services.AddScoped<IAssetCategoryService, AssetCategoryService>();
      services.AddScoped<IAssetService, AssetService>();
      services.AddScoped<IEmailSenderService, EmailSenderService>();

      // Payroll
      services.AddScoped<ISalaryService, SalaryService>();
      services.AddScoped<IPayrollService, PayrollService>();
      services.AddScoped<IBonusService, BonusService>();
      services.AddScoped<ITaxBracketService, TaxBracketService>();
      services.AddScoped<ISocialInsuranceRateService, SocialInsuranceRateService>();
    }

    public static void ConfigureAuthorizationPolicy( this IServiceCollection services )
    {
      services.AddAuthorization( options =>
      {
        #region user
        options.AddPolicy( Policies.USER_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_GROUP && ( claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ||
              ( claim.Type == Roles.ROLE_ADMIN_FINGERPRINT && ( claim.Value == Roles.PERMISSION_VIEW || claim.Value == Roles.PERMISSION_UPDATE ) ) ||
              ( claim.Type == Roles.ROLE_STAFF_TIMESHEET_OTHER && claim.Value == Roles.PERMISSION_VIEW ) ) ) );
        options.AddPolicy( Policies.USER_GET_DYNAMIC_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_STAFF_CALENDAR && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_STAFF_ASSET && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_HOLIDAY ) ) ) );
        options.AddPolicy( Policies.USER_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.USER_GET_DETAIL, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.USER_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.USER_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.USER_CHANGE_STATUS, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_INACTIVE ) ) );
        options.AddPolicy( Policies.USER_RESET_PASS, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.USER_GET_PM_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
            claim.Type == Roles.ROLE_ADMIN_PROJECT && ( claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ) );
        #endregion

        #region team
        options.AddPolicy( Policies.TEAM_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_ANALYSIS && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_OBJECT && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_CATEGORY && claim.Value == Roles.PERMISSION_VIEW ) ||
              //( claim.Type == Roles.ROLE_ADMIN_OVERTIME && claim.Value == Roles.PERMISSION_VIEW ) ||
              //( claim.Type == Roles.ROLE_ADMIN_OVERTIME && claim.Value == Roles.PERMISSION_REPORT ) ||
              ( claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_REPORT ) ||
              ( claim.Type == Roles.ROLE_ADMIN_WFH && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_ASSET && claim.Value == Roles.PERMISSION_VIEW ) ) ) );
        options.AddPolicy( Policies.TEAM_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_TEAM && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.TEAM_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_TEAM && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.TEAM_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_TEAM && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.TEAM_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_TEAM && claim.Value == Roles.PERMISSION_DELETE ) ) );
        #endregion

        #region role
        options.AddPolicy( Policies.ROLE_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_USER && ( claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ) ) );
        options.AddPolicy( Policies.ROLE_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_ROLE && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_VIEW ) ) ) );
        options.AddPolicy( Policies.ROLE_GET_DETAIL, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_ROLE && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_VIEW ) ) ) );
        options.AddPolicy( Policies.ROLE_GET_COLLECTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_ROLE && ( claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ) ) );
        options.AddPolicy( Policies.ROLE_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_ROLE && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.ROLE_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_ROLE && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.ROLE_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_ROLE && claim.Value == Roles.PERMISSION_DELETE ) ) );
        #endregion

        #region project
        options.AddPolicy( Policies.PROJECT_GET_OPTION_WORKING, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_STAFF_TIMESHEET && claim.Value == Roles.PERMISSION_VIEW ) ) ) );
        options.AddPolicy( Policies.PROJECT_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_ANALYSIS && ( claim.Value == Roles.PERMISSION_VIEW || claim.Value == Roles.PERMISSION_REPORT ) ) ) ) );
        options.AddPolicy( Policies.PROJECT_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PROJECT_GET_DETAIL, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PROJECT_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.PROJECT_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.PROJECT_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_DELETE ) ) );
        #endregion

        #region analysis
        options.AddPolicy( Policies.ANALYSIS_VIEW, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_ANALYSIS && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.ANALYSIS_REPORT, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_ANALYSIS && claim.Value == Roles.PERMISSION_REPORT ) ) );
        #endregion

        #region other
        options.AddPolicy( Policies.OTHER_GET_OPTION_STATUS, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_USER && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.OTHER_GET_OPTION_CLIENT, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              claim.Type == Roles.ROLE_ADMIN_PROJECT && ( claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ) );
        options.AddPolicy( Policies.OTHER_GET_OPTION_TYPE_PROJECT, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              claim.Type == Roles.ROLE_ADMIN_PROJECT && ( claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ) );
        options.AddPolicy( Policies.OTHER_GET_OPTION_STATUS_PROJECT, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              claim.Type == Roles.ROLE_ADMIN_PROJECT && ( claim.Value == Roles.PERMISSION_VIEW || claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ) );
        options.AddPolicy( Policies.OTHER_GET_NEW_PROJECT_NUMBER, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.OTHER_GET_FISCAL_NUMBER, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT && claim.Value == Roles.PERMISSION_CREATE ) ) );
        #endregion     

        #region category
        options.AddPolicy( Policies.CATEGORY_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_TIMESHEET && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CATEGORY_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CATEGORY && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CATEGORY_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CATEGORY && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.CATEGORY_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CATEGORY && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.CATEGORY_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CATEGORY && claim.Value == Roles.PERMISSION_DELETE ) ) );
        #endregion

        #region group
        options.AddPolicy( Policies.GROUP_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_PROJECT && ( claim.Value == Roles.PERMISSION_CREATE || claim.Value == Roles.PERMISSION_UPDATE ) ) ) ) );
        options.AddPolicy( Policies.GROUP_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_GROUP && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.GROUP_GET_DETAIL, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_GROUP && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.GROUP_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_GROUP && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.GROUP_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_GROUP && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.GROUP_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_GROUP && claim.Value == Roles.PERMISSION_DELETE ) ) );
        #endregion

        #region finger printer
        options.AddPolicy( Policies.FINGERPRINTER_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_FINGERPRINT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.FINGERPRINTER_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_FINGERPRINT && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.FINGERPRINTER_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_FINGERPRINT && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.FINGERPRINTER_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_FINGERPRINT && claim.Value == Roles.PERMISSION_DELETE ) ) );
        #endregion      

        #region wfh
        options.AddPolicy( Policies.WFH_GET_REQUEST_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_WFH && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.WFH_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_WFH && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.WFH_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_WFH && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.WFH_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_WFH && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.WFH_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_WFH && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.WFH_APPROVE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_WFH && claim.Value == Roles.PERMISSION_APPROVE ) ) );
        #endregion

        #region holiday
        options.AddPolicy( Policies.HOLIDAY_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_HOLIDAY ) ) );
        options.AddPolicy( Policies.HOLIDAY_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_HOLIDAY ) ) );
        options.AddPolicy( Policies.HOLIDAY_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_HOLIDAY ) ) );
        options.AddPolicy( Policies.HOLIDAY_INFOR, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_LEAVE && claim.Value == Roles.PERMISSION_VIEW ) ) );
        #endregion

        #region leave
        options.AddPolicy( Policies.LEAVE_GET_REQUEST_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_LEAVE && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.LEAVE_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.LEAVE_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_LEAVE && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.LEAVE_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_LEAVE && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.LEAVE_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_LEAVE && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.LEAVE_APPROVE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_APPROVE ) ) );
        options.AddPolicy( Policies.LEAVE_REPORT, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_LEAVE && claim.Value == Roles.PERMISSION_REPORT ) ) );
        #endregion

        #region timesheet
        options.AddPolicy( Policies.TIMESHEET_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_TIMESHEET && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.TIMESHEET_GET_DETAIL, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_TIMESHEET && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.TIMESHEET_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_TIMESHEET && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.TIMESHEET_LOCK, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_TIMESHEET_OTHER && claim.Value == Roles.PERMISSION_LOCK ) ) );
        options.AddPolicy( Policies.TIMESHEET_VIEW_OTHER, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_STAFF_TIMESHEET_OTHER && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.TIMESHEET_REPORT, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_ANALYSIS && claim.Value == Roles.PERMISSION_REPORT ) ) );
        #endregion

        #region swap day
        options.AddPolicy( Policies.SWAPDAY_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_SWAPDAY && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.SWAPDAY_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_SWAPDAY && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.SWAPDAY_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_SWAPDAY && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.SWAPDAY_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_SWAPDAY && claim.Value == Roles.PERMISSION_VIEW ) ) );
        #endregion

        #region config
        options.AddPolicy( Policies.CONFIG_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CONFIG && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CONFIG_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CONFIG && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CONFIG_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CONFIG && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CONFIG_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_CONFIG && claim.Value == Roles.PERMISSION_VIEW ) ) );
        #endregion

        #region contract
        options.AddPolicy( Policies.CONTRACT_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CONTRACT_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CONTRACT_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.CONTRACT_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PROJECT_CONTRACT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        #endregion  

        #region Asset
        options.AddPolicy( Policies.PRODUCT_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim =>
              ( claim.Type == Roles.ROLE_ADMIN_ASSET && claim.Value == Roles.PERMISSION_VIEW ) ||
              ( claim.Type == Roles.ROLE_STAFF_ASSET && claim.Value == Roles.PERMISSION_VIEW ) ) ) );
        options.AddPolicy( Policies.PRODUCT_TYPE_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PRODUCT_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PRODUCT_GET_DETAIL, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.ASSET_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_ASSET && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.PRODUCT_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.PRODUCT_CHANGE_STATUS, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PRODUCT_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_DELETE ) ) );
        #endregion

        #region Product Item
        options.AddPolicy( Policies.PRODUCT_ITEM_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PRODUCT_ITEM_GET_OPTION, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PRODUCT_ITEM_HISTORY, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.PRODUCT_ITEM_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_PRODUCT && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        #endregion       
        #region Bill
        options.AddPolicy( Policies.BILL_GET_LIST, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_BILL && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.BILL_GET_DETAIL, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_BILL && claim.Value == Roles.PERMISSION_VIEW ) ) );
        options.AddPolicy( Policies.BILL_CREATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_BILL && claim.Value == Roles.PERMISSION_CREATE ) ) );
        options.AddPolicy( Policies.BILL_UPDATE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_BILL && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        options.AddPolicy( Policies.BILL_DELETE, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_BILL && claim.Value == Roles.PERMISSION_DELETE ) ) );
        options.AddPolicy( Policies.BILL_SYNC, policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim( claim => claim.Type == Roles.ROLE_ADMIN_BILL && claim.Value == Roles.PERMISSION_UPDATE ) ) );
        #endregion
      } );
    }

    public static void ConfigureLocalization( this IServiceCollection services )
    {
      services.AddLocalization( options => options.ResourcesPath = "Languages" );

      var supportedCultures = new List<CultureInfo> { new CultureInfo( "en" ), new CultureInfo( "vi" ) };
      services.Configure<RequestLocalizationOptions>( options =>
      {
        options.DefaultRequestCulture = new RequestCulture( "en" );
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
      } );
    }

    /// <summary>
    /// Configure the API versioning properties of the project, such as return headers, version format, etc.
    /// </summary>
    /// <param name="services"></param>
    public static void AddApiVersioningConfigured( this IServiceCollection services )
    {
      services.AddApiVersioning( options =>
      {
        // ReportApiVersions will return the "api-supported-versions" and "api-deprecated-versions" headers.

        options.ReportApiVersions = true;

        // Set a default version when it's not provided,

        // e.g., for backward compatibility when applying versioning on existing APIs

        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion( 1, 0 );

        // Combine (or not) API Versioning Mechanisms:

        options.ApiVersionReader = ApiVersionReader.Combine(
            // The Default versioning mechanism which reads the API version from the "api-version" Query String paramater.

            new QueryStringApiVersionReader( "api-version" ),
            // Use the following, if you would like to specify the version as a custom HTTP Header.

            new HeaderApiVersionReader( "Accept-Version" ),
            // Use the following, if you would like to specify the version as a Media Type Header.

            new MediaTypeApiVersionReader( "api-version" )
        );
      } );

      // Here, we will add another service, e.g., to support versioning on our documentation.

    }

    /// <summary>
    /// Configure the Swagger generator with XML comments, bearer authentication, etc.
    /// Additional configuration files:
    /// <list type="bullet">
    ///     <item>ConfigureSwaggerSwashbuckleOptions.cs</item>
    /// </list>
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwaggerSwashbuckleConfigured( this IServiceCollection services )
    {
      //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerSwashbuckleOptions>();

      // Configures ApiExplorer (needed from ASP.NET Core 6.0).
      services.AddEndpointsApiExplorer();

      // Register the Swagger generator, defining one or more Swagger documents.
      // Read more here: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
      services.AddSwaggerGen( options =>
      {
        // add a custom operation filter which sets default values
        options.OperationFilter<SwaggerDefaultValues>();
        options.OperationFilter<SwaggerLanguageHeader>();
        options.ParameterFilter<CustomParameterFilter>();

        // If we would like to provide request and response examples (Part 1/2)
        // Enable the Automatic (or Manual) annotation of the [SwaggerRequestExample] and [SwaggerResponseExample].
        // Read more here: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters
        options.ExampleFilters();

        // If we would like to include documentation comments in the OpenAPI definition file and SwaggerUI.
        // Set the comments path for the XmlComments file.
        // Read more here: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio#xml-comments
        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine( AppContext.BaseDirectory, xmlFile );
        options.IncludeXmlComments( xmlPath, true );

        // If we would like to provide security information about the authorization scheme that we are using (e.g. Bearer).
        // Add Security information to each operation for bearer tokens and define the scheme.
        options.OperationFilter<SecurityRequirementsOperationFilter>( true, "Bearer" );
        options.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
        {
          Description = "Standard Authorization header using the Bearer scheme (JWT). Example: \"bearer {token}\"",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
        } );

        // If we use the [Authorize] attribute to specify which endpoints require Authorization, then we can
        // Show an "(Auth)" info to the summary so that we can easily see which endpoints require Authorization.
        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

        options.SwaggerDoc( "v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "API HRM VCI",
          Description = "Danh sách APIs cung cấp cho project API HRM VCI Web (Phase II) <br> " +
          "Design: <a href=\"https://www.figma.com/file/FlYOOreOPzS48o8iN310pq/TTDWebsite2_upgraded?type=design&t=dz5QuKHbCNJetJdK-6\">Figma</a><br><br>" +
          "Diagram Flow: <a href=\"https://drive.google.com/file/d/1xrqkCJHDYvoBCH-fW7kKu_O005zXnyag/view?usp=sharing\">DrawIO</a><br><br>" +
          "Code BE: <a href=\"https://github.com/vci-jsc-vietnam/vci-hrm-be\">Github</a><br><br>" +
          "<strong>Chú thích:</strong><br>" +
          "Thẻ field model" +
          "<ul><li>[input]: field nhập</li><li>[hide]: field ẩn</li><li>[require]: field phải có giá trị</li><li>[unique]: field check trùng</li><li>[format]: field sẽ được format</li></ul>" +
          "Thẻ api" +
          "<ul><li>[Option]: api option combobox</li><li>[Get List]: api list object</li><li>[Get Detail]: api get detail</li><li>[Create]: api create new</li><li>[Update]: api update</li><li>[Delete]: api delete</li><li>[Other]: api khác</li></ul>",
          //TermsOfService = new Uri( "https://example.com/terms" ),
          //Contact = new OpenApiContact
          //{
          //  Name = "Example Contact",
          //  Url = new Uri( "https://example.com/contact" )
          //},
          //License = new OpenApiLicense
          //{
          //  Name = "Example License",
          //  Url = new Uri( "https://example.com/license" )
          //}
        } );
      } );

      // If we would like to provide request and response examples (Part 2/2)
      // Register examples with the ServiceProvider based on the location assembly or example type.
      services.AddSwaggerExamplesFromAssemblies( Assembly.GetEntryAssembly() );

      // If we are using FluentValidation, then we can register the following service to add the  fluent validation rules to swagger.
      // Adds FluentValidationRules staff to Swagger. (Minimal configuration)
      services.AddFluentValidationRulesToSwagger();
    }
  }

  /// <summary>
  /// Configures the Swagger generation options.
  /// </summary>
  /// <remarks>This allows API versioning to define a Swagger document per API version after the
  /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
  public class ConfigureSwaggerSwashbuckleOptions : IConfigureOptions<SwaggerGenOptions>
  {
    readonly IApiVersionDescriptionProvider provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerSwashbuckleOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    public ConfigureSwaggerSwashbuckleOptions( IApiVersionDescriptionProvider provider ) => this.provider = provider;

    /// <inheritdoc />
    public void Configure( SwaggerGenOptions options )
    {
      // Add a swagger document for each discovered API version.
      // Note: you might choose to skip or document deprecated API versions differently.
      foreach ( var description in provider.ApiVersionDescriptions ) {
        options.SwaggerDoc( description.GroupName, CreateInfoForApiVersion( description ) );
      }
    }

    private static OpenApiInfo CreateInfoForApiVersion( ApiVersionDescription description )
    {
      var info = new OpenApiInfo()
      {
        Title = "Web API Documentation Tutorial",
        Version = description.ApiVersion.ToString(),
        Description = "A tutorial project to provide documentation for our existing APIs.",
        Contact = new OpenApiContact() { Name = "Ioannis Kyriakidis", Email = "info@dotnetnakama.com" },
        License = new OpenApiLicense() { Name = "MIT License", Url = new Uri( "https://opensource.org/licenses/MIT" ) }
      };

      if ( description.IsDeprecated ) {
        info.Description += " [This API version has been deprecated]";
      }

      return info;
    }
  }
}
