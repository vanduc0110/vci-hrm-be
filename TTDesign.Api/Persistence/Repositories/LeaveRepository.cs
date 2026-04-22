using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class LeaveRepository : GenericRepository<Leave>, ILeaveRepository
  {
    public LeaveRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<List<Leave>> GetListBeginLeaveRequest( int type, long leaveRequestId )
    {
      // lấy leave id xuất phát điểm mà leave request id sử dụng
      var leaveHistory = await _context.LeaveHistoryUsings.Include( x => x.LeaveRequest ).Where( l => l.LeaveRequestId == leaveRequestId ).ToListAsync();
      var startLeaveId = leaveHistory.Min( x => x.LeaveId );
      var leaves = await _context.Leaves.Where( l => l.Type == type && l.Id >= startLeaveId && l.UserId == leaveHistory [ 0 ].LeaveRequest.CreatedBy ).OrderBy( l => l.Id ).ToListAsync(); // danh sách leave kể từ leave id nhỏ nhất quan hệ với leave request
      leaves.ForEach( l => l.Using = 0 );
      // tính toán lại số giờ còn lại của leave đầu tiên trước khi tạo quan hệ với leave request id
      var leaveHistoryOfLeaveFirst = await _context.LeaveHistoryUsings.Where( l => l.LeaveId == startLeaveId ).OrderBy( l => l.Id ).ToListAsync(); // danh leave request sử dụng leave id nhỏ nhất
      double hour = 0;
      foreach ( var history in leaveHistoryOfLeaveFirst ) {
        if ( history.LeaveRequestId == leaveRequestId ) {
          leaves [ 0 ].Using = hour;
          break;
        }
        hour += history.Hours;
      }
      return leaves;
    }
  }
}
