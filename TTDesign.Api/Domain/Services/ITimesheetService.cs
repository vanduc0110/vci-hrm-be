using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface ITimesheetService : IGenericService<Timesheet>,
   BaseServiceList<TimesheetResponse>, BaseServiceDetail<TimesheetDetailResponse>, BaseServiceResource<TimesheetResource>
  {
    /// <summary>
    /// request lock timesheet
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="userId"></param>
    /// <param name="isLock">true: request lock, false: unlock</param>
    /// <returns></returns>
    Task LockTimesheet( DateTime start, DateTime end, long userId, bool isLock = true );
    /// <summary>
    /// lấy record timesheet gồm đầy đủ thông tin, gồm timesheet, finger printer, wfh request và timesheet detail
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Timesheet?> GetTimesheet( long id );
    /// <summary>
    /// tạo timesheet detail liên quan tới request leave khi approve request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task ApplyLeaveRequestApproved( LeaveRequest request );
    /// <summary>
    /// xóa timesheet detail liên quan tới request leave khi approve chuyển từ trạng thái approve sang reject
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task RemoveLeaveRequestReject( LeaveRequest request );
    /// <summary>
    /// áp dụng wfh request vào timesheet
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task ApplyWfhRequestApproved( WfhRequest request );
    /// <summary>
    /// xóa dữ liệu liên quan wfh request trên timesheet
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task RemoveWfhRequestReject( WfhRequest request );
    /// <summary>
    /// khi sửa thủ công finger printer thì cập nhật lại trạng thái overtime requesst
    /// </summary>
    /// <param name="fingerPrinter"></param>
    /// <returns></returns>
   // Task UpdateFingerPrinterApply( FingerPrinter fingerPrinter );
    /// <summary>
    /// check request có rơi vào ngày bị lock không
    /// </summary>
    /// <param name="date"></param>
    /// <param name="endDate">nếu có value, check từ date tới endDate</param>
    /// <returns>true: bị lock, false: chưa bị lock</returns>
    Task<bool> CheckTimesheetHadLock( DateTime date, DateTime? endDate = null );
    /// <summary>
    /// khởi tạo thông tin dashboard: checkin-checkout
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<DashboardTimesheet> GetDashboardTimesheet( long userId );
    /// <summary>
    /// khởi tạo thông tin dashboard: project
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    Task<DashboardProject> GetDashboardProject( long userId, DateTime month );
    /// <summary>
    /// report của timesheet request
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<IEnumerable<TimesheetReportDetail>> GetReport( long? teamId, DateTime start, DateTime end );
    /// <summary>
    /// xuất report tổng hợp timesheet thành file excel
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="startFilter"></param>
    /// <param name="endFilter"></param>
    /// <param name="teamIds"></param>
    /// <returns></returns>
    public Task<byte []?> ExportSummaryTimesheet( long? teamId, DateTime startFilter, DateTime endFilter, long []? teamIds );
    /// <summary>
    /// xuất report tổng hợp timesheet theo team thành file excel
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="startFilter"></param>
    /// <param name="endFilter"></param>
    /// <param name="teamIds"></param>
    /// <returns></returns>
    public Task<byte []?> ExportSummaryTimesheetByTeam( long? teamId, DateTime startFilter, DateTime endFilter, long []? teamIds );
    /// <summary>
    /// lấy danh sách record ngày hoán đổi
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public Task<IEnumerable<SwapDayResponse>> SwapDayGetListView( int year );
    /// <summary>
    /// tạo record ngày hoán đổi
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="creator"></param>
    /// <returns></returns>
    public Task<long> SwapDayCreate( SwapDayResource resource, long creator );
    /// <summary>
    /// cập nhật record ngày hoán đổi
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="modifier"></param>
    /// <returns></returns>
    public Task SwapDayUpdate( SwapDayResource resource, long modifier );
    /// <summary>
    /// xóa record ngày hoán đổi
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task SwapDayDelete( long id );
    /// <summary>
    /// kiểm tra ngày đã được đăng ký là ngày hoán đổi chưa
    /// </summary>
    /// <param name="date"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<bool> ExistSwapDay( DateTime date, long userId );
    /// <summary>
    /// lấy thông tin team ID từ thông tin user trong Timesheet
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<long []> GetTeamId( long id );
    /// <summary>
    /// lấy thông tin dashboard thời gian làm, nghỉ phép và nghỉ không phép của user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="date"></param> 
    /// <returns></returns>
    Task<DashboardTime> GetDashboardTime( long userId, DateTime date );

    Task<byte []?> ExportTimesheetByMonth( long year, long month );
  }
}
