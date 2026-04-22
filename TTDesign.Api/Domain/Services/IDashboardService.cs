using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IDashboardService
  {
    Task<DashboardSummary> GetSummary( DashboardResource dashboard );
    Task<DashboardTimeTracking> GetDashboardTimeTracking( DashboardResource dashboard );
    Task<IEnumerable<ProjectResponse>> GetListView( DashboardResource dashboard, BaseFilter filter );
    Task<List<GetActiveProjectHoursResponse>> GetAnalysisDetailView( DashboardResource dashboard );
  }
}
