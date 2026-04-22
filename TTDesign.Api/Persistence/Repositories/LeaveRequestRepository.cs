using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
  {
    public LeaveRequestRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestDetails( Expression<Func<LeaveRequest, bool>> predicate )
    {
      return await _context.LeaveRequests.Include( l => l.LeaveRequestDetails ).AsNoTracking().Where( predicate ).ToListAsync();
    }

    public async Task<List<LeaveRequest>> GetListByLeaves( List<long> leaveIds, long leaveRequestId )
    {
      var leaveRequests = new Dictionary<long, LeaveRequest>();
      foreach ( var id in leaveIds ) {
        var requests = await _context.LeaveHistoryUsings.Include( l => l.LeaveRequest ).Where( l => l.LeaveId == id ).OrderBy( l => l.Id ).Select( l => l.LeaveRequest ).ToListAsync();
        foreach ( var request in requests ) {
          if ( request.Id != leaveRequestId && !leaveRequests.ContainsKey( request.Id ) ) {
            leaveRequests.Add( request.Id, request );
          }
        }
      }
      return leaveRequests.Values.ToList();
    }

    public async Task RemoveLeaveHistoryUsing( List<long> leaveRequestIds )
    {
      var histories = await _context.LeaveHistoryUsings.Where( l => leaveRequestIds.Contains( l.LeaveRequestId ) ).ToListAsync();
      _context.RemoveRange( histories );
      _context.SaveChanges();
    }
  }
}
