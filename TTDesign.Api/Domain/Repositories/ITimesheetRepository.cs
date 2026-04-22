using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface ITimesheetRepository : IGenericRepository<Timesheet>
  {
    /// <summary>
    /// danh sách timesheet trong tháng của user, gồm thông tin finger printer, swap day và timesheet report
    /// </summary>
    /// <param name="user"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    Task<IEnumerable<Timesheet>> GetList( long user, long year, long month );
    /// <summary>
    /// chi tiết task trong 1 ngày
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Timesheet?> GetTimesheetDetailById( long id );
    /// <summary>
    /// thông tin timesheet, bao gồm thêm thông tin Finger Printer
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<Timesheet?> GetTimesheet( Expression<Func<Timesheet, bool>> predicate );
    /// <summary>
    /// thông tin timesheet, bao gồm thêm thông tin Finger Printer và WFH request
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<Timesheet>> GetTimesheets( Expression<Func<Timesheet, bool>> predicate );
    /// <summary>
    /// thông tin timesheet, bao gồm thêm thông tin Finger Printer, Swap Day, WFH request và Timesheet detail
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<Timesheet?> GetTimesheetDetailByCondition( Expression<Func<Timesheet, bool>> predicate );
    /// <summary>
    /// thông tin timesheet, bao gồm thêm thông tin chi tiết TimesheetDetail
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<Timesheet>> GetTimesheetDetails( Expression<Func<Timesheet, bool>> predicate );
    /// <summary>
    /// lấy thông tin chi tiết timesheet detail
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<Timesheet?> GetTimesheetDetail( Expression<Func<Timesheet, bool>> predicate );
    /// <summary>
    /// lấy thông tin team từ dữ liệu user trong Timesheet
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<long []> GetTeamId( long id );
  }
}
