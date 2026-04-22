using AutoMapper;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class CategoryService : GenericService<TeamCategory>, ICategoryService
  {
    private readonly ITeamCategoryRepository _categoryRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ITimesheetDetailRepository _timesheetDetailRepository;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    public CategoryService( ITeamCategoryRepository categoryRepository,
      ILogger<CategoryService> logger,
      ITeamRepository teamRepository,
      ITimesheetDetailRepository timesheetDetailRepository,
      IProjectRepository projectRepository,
      IMapper mapper ) : base( categoryRepository )
    {
      _categoryRepository = categoryRepository;
      _logger = logger;
      _mapper = mapper;
      _teamRepository = teamRepository;
      _timesheetDetailRepository = timesheetDetailRepository;
      _projectRepository = projectRepository;
    }

    #region BaseServiceList
    public async Task<IEnumerable<CategoryResponse>> GetList( BaseFilter filter )
    {
      var categories = await _categoryRepository.GetListByCondition( t => filter.TeamId == null || t.TeamId == filter.TeamId );
      var categoryResponse = _mapper.Map<IEnumerable<CategoryResponse>>( categories.OrderBy( c => c.Name ) );
      var teams = new Dictionary<long, Team>();
      foreach ( var category in categoryResponse ) {
        // set team info
        if ( teams.ContainsKey( category.TeamId ) ) {
          category.TeamName = teams [ category.TeamId ].Name;
          category.TeamCode = teams [ category.TeamId ].Code;
        }
        else {
          var team = await _teamRepository.GetByConditionNoTrack( t => t.Id == category.TeamId );
          if ( team != null ) {
            teams.Add( category.TeamId, team );
            category.TeamName = team.Name;
            category.TeamCode = team.Code;
          }
        }
      }
      return categoryResponse;
    }
    #endregion

    #region BaseServiceOption
    public async Task<IEnumerable<CategoryOption>> GetOption( long? teamId = null )
    {
      return ( await _categoryRepository.GetOption( teamId ) ).OrderBy( c => c.Name );
    }
    #endregion

    #region BaseServiceResource
    public async Task<long> Create( CategoryResource resource, long creator )
    {
      var category = _mapper.Map<TeamCategory>( resource );
      category.CreatedBy = creator;
      await _categoryRepository.CreateAsync( category );
      return category.Id;
    }

    public async Task Update( CategoryResource resource, long editor )
    {
      var oldCategory = await _categoryRepository.GetByCondition( g => g.Id == resource.Id );
      oldCategory!.Name = resource.Name;
      oldCategory!.CreatedBy = editor;
      _mapper.Map( resource, oldCategory );
      _categoryRepository.Update( oldCategory );
    }
    #endregion

    #region Others
    public async Task<bool> CheckCategoryBeforeDelete( long id )
    {
      var category = await _categoryRepository.GetByCondition( c => c.Id == id );
      if ( category!.IsUsing )
        return true;
      if ( await _timesheetDetailRepository.Exist( c => c.TimesheetCategoryId == id ) ) {
        category.IsUsing = true;
        _categoryRepository.Update( category );
        return true;
      }
      return false;
    }
    #endregion
  }
}
