using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class NotificationAssignRepository : GenericRepository<NotificationAssign>, INotificationAssignRepository
  {
    public NotificationAssignRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<NotificationAssign>> GetListNotification( Expression<Func<NotificationAssign, bool>> predicate )
    {
      return await _context.NotificationAssigns.Include( n => n.Notification ).Where( predicate ).AsNoTracking().OrderByDescending( n => n.Notification.ModifiedDate ).ToListAsync();
    }
  }
}
