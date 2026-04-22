using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
  {
    public NotificationRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<Notification?> getNotification( Expression<Func<Notification, bool>> predicate )
    {
      return await _context.Notifications.Include( n => n.NotificationAssigns ).Where( predicate ).FirstOrDefaultAsync();
    }
  }
}
