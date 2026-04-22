using Microsoft.EntityFrameworkCore;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class FingerPrinterRepository : GenericRepository<FingerPrinter>, IFingerPrinterRepository
  {
    public FingerPrinterRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<IEnumerable<FingerPrinterResponse>> GetFingerPrinterByFilter( BaseFilter filter )
    {
      return await _context.Timesheets.Include( t => t.FingerPrinter ).Include( t => t.User ).ThenInclude( t => t.TeamUsers )
        .Where( t => ( filter.UserId == 0 || filter.UserId == t.User.Id ) && ( filter.TeamId == null || t.User.TeamUsers.Select( x => x.TeamId ).Any( x => x == filter.TeamId ) ) &&
          t.Date >= filter.Start.Date && t.Date <= filter.End.Date && t.User.Id != Enums.SYSTEM_ID )
        .Select( t => new FingerPrinterResponse()
        {
          Id = t.FingerPrinter!.Id,
          Date = t.FingerPrinter.DateIn.Date,
          TimeIn = t.FingerPrinter.DateIn == t.FingerPrinter.DateIn.Date ? null : t.FingerPrinter.DateIn.ToString( Enums.HOUR_FORMAT ),
          TimeOut = t.FingerPrinter.DateOut == t.FingerPrinter.DateOut.Date ? null : t.FingerPrinter.DateOut.ToString( Enums.HOUR_FORMAT ),
          UserId = t.User.Id,
          UserName = t.User.FullName,
          Note = t.FingerPrinter.Note,
          ModifiedBy = t.FingerPrinter.ModifiedBy ?? 0,
          ModifiedDate = t.FingerPrinter.ModifiedDate ?? DateTime.MinValue,
        } ).OrderBy( t => t.Date ).ToListAsync();
    }
  }
}
