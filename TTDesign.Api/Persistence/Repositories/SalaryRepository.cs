using Microsoft.EntityFrameworkCore;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Resources;

namespace TTDesign.API.Persistence.Repositories
{
  public class SalaryRepository : GenericRepository<Salary>, ISalaryRepository
  {
    public SalaryRepository( AppDbContext context ) : base( context ) { }

    public async Task<IEnumerable<SalaryResponse>> GetActiveSalaries()
    {
      return await _context.Salaries
        .AsNoTracking()
        .Include( s => s.User )
        .Where( s => s.IsActive )
        .OrderBy( s => s.User.FullName )
        .Select( s => new SalaryResponse
        {
          Id = s.Id,
          UserId = s.UserId,
          FullName = s.User.FullName,
          StaffId = s.User.StaffId,
          BasicSalary = s.BasicSalary,
          AllowanceHousing = s.AllowanceHousing,
          AllowanceTransport = s.AllowanceTransport,
          AllowanceFood = s.AllowanceFood,
          AllowanceOther = s.AllowanceOther,
          EffectiveDate = s.EffectiveDate,
          IsActive = s.IsActive,
          Notes = s.Notes,
          ApprovedBy = s.ApprovedBy,
          ApprovedDate = s.ApprovedDate,
        } )
        .ToListAsync();
    }

    public async Task<SalaryResponse?> GetActiveByUser( long userId )
    {
      return await _context.Salaries
        .AsNoTracking()
        .Include( s => s.User )
        .Where( s => s.UserId == userId && s.IsActive )
        .Select( s => new SalaryResponse
        {
          Id = s.Id,
          UserId = s.UserId,
          FullName = s.User.FullName,
          StaffId = s.User.StaffId,
          BasicSalary = s.BasicSalary,
          AllowanceHousing = s.AllowanceHousing,
          AllowanceTransport = s.AllowanceTransport,
          AllowanceFood = s.AllowanceFood,
          AllowanceOther = s.AllowanceOther,
          EffectiveDate = s.EffectiveDate,
          IsActive = s.IsActive,
          Notes = s.Notes,
          ApprovedBy = s.ApprovedBy,
          ApprovedDate = s.ApprovedDate,
        } )
        .FirstOrDefaultAsync();
    }
  }
}
