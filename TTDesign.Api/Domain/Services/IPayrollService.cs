using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IPayrollService : IGenericService<Payroll>
  {
    Task<IEnumerable<PayrollResponse>> GetList( int month, int year, long[]? allowedUserIds = null );
    Task<PayrollResponse?> GetDetail( long id );
    Task Calculate( PayrollCalculateRequest request, long creator );
    Task Update( PayrollUpdateRequest request, long editor );
    Task LeadConfirm( long id, long confirmedBy );
    Task HRApprove( long id, long approvedBy );
    Task DirectorApprove( long id, long approvedBy );
    Task Reject( long id, string reason, long rejectedBy );
    Task MarkPaid( long id, long editor );
    Task Cancel( long id, long editor );
    Task<byte[]?> Export( int month, int year );
    Task<int> GetTotalPending( int month, int year, int position, long[] teamIds, bool hasPayrollFull );
  }
}
