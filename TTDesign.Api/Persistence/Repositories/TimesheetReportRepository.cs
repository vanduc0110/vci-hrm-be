using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class TimesheetReportRepository : GenericRepository<TimesheetReport>, ITimesheetReportRepository
  {
    public TimesheetReportRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
