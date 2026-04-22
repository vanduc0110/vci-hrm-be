using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface ILeaveService : IGenericService<LeaveRequest>,
    BaseServiceResource<LeaveRequestResource>, BaseServiceList<LeaveRequestResponse>
  {
    /// <summary>
    /// lấy danh sách request leave của user trong 1 tháng
    /// </summary>
    /// <param name="userRequest"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<IEnumerable<LeaveRequestResponse>> GetRequestList( long userRequest, long year );
    /// <summary>
    /// chuyển trạng thái request leave
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isApprove"></param>
    /// <param name="reviewer"></param>
    /// <returns></returns>
    Task Approve( long id, bool isApprove, long reviewer );
    /// <summary>
    /// lấy thông tin các hình thức leave request của hệ thống
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<LeaveInformationResponse>> GetLeaveInformationResponses();
    /// <summary>
    /// lấy thông tin số lượng ngày nghỉ còn lại của user 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<RemainLeave> GetRemainLeave( long userId );
    /// <summary>
    /// danh sách lịch sử leave và các thông số
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<IEnumerable<LeaveHistoryResponse>> GetLeaveHistories( long userId, long year );
    /// <summary>
    /// check leave request có rơi vào ngày bị lock không
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    Task<bool> CheckTimesheetHadLock( DateTime start, DateTime end );
    /// <summary>
    /// xóa request
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete( long id );
    /// <summary>
    /// report của leave
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<LeaveReport> GetReport( long? teamId, long year );
    /// <summary>
    /// xuất report thành file excel
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<byte []?> ExportLeaveDayStaff( long? teamId, long year );
    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    Task<int> GetTotalLeaveRequest( long? teamId = null );
    /// <summary>
    /// Update số ngày nghỉ hàng năm của nhân viên
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="annualLeave"></param>
    /// <param name="notes"></param>
    /// <param name="editor"></param>
    /// <returns></returns>
    Task UpdateAnunalLeave( long userId, double annualLeave, string notes );
  }
}
