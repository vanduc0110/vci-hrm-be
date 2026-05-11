using AutoMapper;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Resources;
using Project = TTDesign.API.Domain.Models.Project;

namespace TTDesign.API.Services
{
  public class ProjectService : GenericService<Project>, IProjectService
  {
    private readonly IProjectRepository _projectRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectDocumentRepository _projectDocumentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ITimesheetDetailRepository _timesheetDetailRepository;
    private readonly ITeamCategoryRepository _categoryRepository;
    private readonly IProjectContractRepository _projectContractRepository;
    private readonly IConfigRepository _configRepository;
    private readonly ILogger<ProjectService> _logger;
    private readonly IMapper _mapper;
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly ITeamCategoryRepository _teamCategoryRepository;

    public ProjectService( IProjectRepository projectRepository,
      ITeamRepository teamRepository,
      IUserRepository userRepository,
      IProjectDocumentRepository projectDocumentRepository,
      IClientRepository clientRepository,
      ITimesheetDetailRepository timesheetDetailRepository,
      ITeamCategoryRepository categoryRepository,
      IProjectContractRepository projectContractRepository,
      IConfigRepository configRepository,
      ITimesheetRepository timesheetRepository,
      ILogger<ProjectService> logger,
      ITeamCategoryRepository teamCategoryRepository,
      IMapper mapper ) : base( projectRepository )
    {
      _projectRepository = projectRepository;
      _logger = logger;
      _mapper = mapper;
      _teamRepository = teamRepository;
      _userRepository = userRepository;
      _clientRepository = clientRepository;
      _timesheetDetailRepository = timesheetDetailRepository;
      _categoryRepository = categoryRepository;
      _projectDocumentRepository = projectDocumentRepository;
      _projectContractRepository = projectContractRepository;
      _configRepository = configRepository;
      _timesheetRepository = timesheetRepository;
      _teamCategoryRepository = teamCategoryRepository;
    }

    #region BaseServiceOption
    public async Task<IEnumerable<ProjectOption>> GetOption( long userId, long? teamId = null )
    {
      return ( await _projectRepository.GetOption( userId, teamId ) ).OrderBy( p => p.FullName );
    }
    #endregion

    #region BaseServiceDetail
    public async Task<ProjectDetailResponse?> GetDetail( long id )
    {
      var project = await _projectRepository.GetProjectDataById( id );
      if ( project is null ) {
        return null;
      }
      var response = _mapper.Map<ProjectDetailResponse>( project );
      // set team info
      var team = await _teamRepository.GetByCondition( t => t.Id == project.TeamId );
      if ( team is not null ) {
        response.TeamCode = team.Code;
        response.TeamName = team.Name;
      }
      response.Members = _mapper.Map<List<UserOption>>( project.Users );
      // set PM
      var pmStrs = response.Manager.Split( "," );
      var pmL = pmStrs.Select( long.Parse ).ToArray();
      var pms = new List<string>();
      foreach ( var pm in pmL ) {
        var projectManager = await _userRepository.GetByCondition( u => u.Id == pm );
        if ( projectManager is not null ) {
          pms.Add( projectManager.FullName );
        }
      }
      if ( pms is not null ) {
        response.ManagerName = string.Join( ",", pms );
      }
      return response;
    }
    #endregion

    #region BaseServiceList
    public async Task<IEnumerable<ProjectResponse>> GetList( BaseFilter filter )
    {
      var projects = await _projectRepository.GetProjects( ( long ) filter.UserId!, ( int ) filter.Position!, filter.TeamId );
      var projectResponse = _mapper.Map<IEnumerable<ProjectResponse>>( projects.OrderByDescending( p => p.Code ) );
      var teams = await _teamRepository.GetOption( filter.TeamId );
      foreach ( var project in projectResponse ) {
        // set team info
        var team = teams.FirstOrDefault( t => t.Id == project.TeamId );
        if ( team is not null ) {
          project.TeamCode = team.Code;
          project.TeamName = team.Name;
        }
        var tsDetails = await _timesheetDetailRepository.GetListByCondition( t => t.ProjectId == project.Id &&
            t.Type == ( int ) Enums.TimesheetDetailType.Project );
        var timesheetIds = tsDetails.Select( x => x.TimesheetId ).ToList();
        var timeSheet = tsDetails.Count() > 0 ? await _timesheetRepository.GetListByCondition( t => timesheetIds.Any( x => x == t.Id ) ) : null;
        project.CurrentDay = timeSheet != null ? timeSheet.Count() : 0;
        project.QuotationDay = project.QuotationHour > 0 ? ( long ) Math.Ceiling( ( double ) project.QuotationHour / 8 ) : 0;
        project.TotalHour = tsDetails!.Sum( x => x.Hours );
        var projectType = await _configRepository.GetByCondition( c => c.Code == project.Type );
        project.ConfigName = projectType != null ? projectType!.Name : "";
      }
      return projectResponse;
    }
    #endregion

    #region BaseServiceResource
    public async Task<long> Create( ProjectResource resource, long creator )
    {
      if ( resource.QuotationHour is null )
        resource.QuotationHour = 0;
      var project = _mapper.Map<Project>( resource );
      var lastProject = await _projectRepository.GetLastProject();
      project.FiscalYear = Common.GetFiscalYear();
      project.ProjectNumber = lastProject == null ? 1 : lastProject.ProjectNumber + 1;
      project.CreatedBy = creator;
      project.ModifiedBy = creator;
      project.Status = resource.Status switch
      {
        "Pending" => 0,
        "Active" => 1,
        "End" => 2,
        _ => 1 // default
      };
      project.ProjectManagement = string.Join( ",", resource.Manager.Select( x => x.ToString() ) );
      project.Code = await GenerateProjectCode( project );
      // set Users
      project.Users = new List<User>();
      if ( resource.MemberIds is not null && resource.MemberIds.Count > 0 ) {
        foreach ( var memId in resource.MemberIds! ) {
          var user = await _userRepository.GetUserDataByConditionNoTracking( x => x.Id == memId );
          if ( !project.Users.Any( u => u.Id == user!.Id ) )
            project.Users.Add( user! );
        }
      }
      //setPm
      foreach ( var pm in resource.Manager ) {
        var userManagement = await _userRepository.GetUserDataByConditionNoTracking( x => x.Id == pm );
        project.Users.Add( userManagement! );
      }

      await _projectRepository.CreateAsync( project );

      var projectType = await _configRepository.GetByCondition( c => c.Code == resource.Type );
      if ( projectType is not null ) {
        projectType!.IsUsing = true;
        _configRepository.Update( projectType );
      }
      return project.Id;
    }

    public async Task Update( ProjectResource resource, long editor )
    {
      var old = await _projectRepository.GetByCondition( p => p.Id == resource.Id );
      _mapper.Map( resource, old! );
      old!.ModifiedBy = editor;
      old.Code = await GenerateProjectCode( old );
      old.ProjectManagement = string.Join( ",", resource.Manager );
      _projectRepository.Update( old! );
      // set member
      var members = new List<User>();
      if ( resource.MemberIds is not null && resource.MemberIds.Count > 0 ) {
        foreach ( var memberId in resource.MemberIds! ) {
          var user = await _userRepository.GetUserDataByConditionNoTracking( u => u.Id == memberId );
          members.Add( user! );
        }
        // get PM
        if ( !new HashSet<long>( members.Select( x => x.Id ) ).SetEquals( resource.Manager ) ) {
          foreach ( var item in resource.Manager ) {
            var pm = await _userRepository.GetUserDataByConditionNoTracking( u => u.Id == item );
            members.Add( pm! );
          }

        }
        await _projectRepository.UpdateMemberAsync( old.Id, members );

      }
    }
    #endregion

    #region Others
    public async Task<IEnumerable<ProjectOption>> GetOptionWorking( long userId )
    {
      return ( await _projectRepository.GetOptionWorking( userId ) ).OrderBy( p => p.FullName );
    }

    public async Task<ProjectDetailResponse?> GetDetailForEdit( long id )
    {
      var project = await _projectRepository.GetProjectDataById( id );
      if ( project is null ) {
        return null;
      }
      var response = _mapper.Map<ProjectDetailResponse>( project );
      // set team info
      var team = await _teamRepository.GetByCondition( t => t.Id == project.TeamId );
      if ( team is not null ) {
        response.TeamCode = team.Code;
        response.TeamName = team.Name;
      }
      // set PM
      var pmStrs = response.Manager.Split( "," );
      var pmL = pmStrs.Select( long.Parse ).ToArray();
      var pms = new List<string>();
      foreach ( var pm in pmL ) {
        var projectManager = await _userRepository.GetByCondition( u => u.Id == pm );
        if ( projectManager is not null ) {
          pms.Add( projectManager.FullName );
        }
      }
      if ( pms is not null ) {
        response.ManagerName = string.Join( ",", pms );
      }
      return response;
    }

    public async Task<bool> HadTimesheet( long id )
    {
      return await _timesheetDetailRepository.Exist( t => t.ProjectId == id );
    }

    public async Task Delete( long id )
    {
      await _projectRepository.DeleteAsync( id );
    }

    public async Task<AnalysisResponse> GetAnalysisView( long projectId, long? teamId = null, long? userId = null, HashSet<ProjectObjectDetail>? categoriesFilter = null )
    {
      // get timesheet detail refer project
      var tsDetails = await _timesheetDetailRepository.GetListByCondition( t => t.ProjectId == projectId &&
        t.Type != ( int ) Enums.TimesheetDetailType.PaidLeave && t.Type != ( int ) Enums.TimesheetDetailType.UnpaidLeave );
      // summary by category
      var categories = new HashSet<ProjectObjectDetail>();
      var project = await _projectRepository.GetProjectDocument( projectId );
      var team = await _teamRepository.GetByConditionNoTrack( t => t.Id == project!.TeamId );

      if ( categoriesFilter == null ) {
        foreach ( var category in tsDetails.GroupBy( t => ( long ) t.TimesheetCategoryId! ).ToDictionary( k => k.Key, v => v.Sum( t => t.Hours ) ) ) {
          var cate = await _categoryRepository.GetByConditionNoTrack( c => c.Id == category.Key );
          if ( cate != null ) {
            categories.Add( new ProjectObjectDetail() { Name = cate.Name, WorkingHours = category.Value } );
          }
        }
      }
      else {
        categories = categoriesFilter;
        var categoriesIds = new List<long>();
        foreach ( var item in categories ) {
          var teamCategory = await _teamCategoryRepository.GetByConditionNoTrack( x => x.TeamId == project!.TeamId && x.Name == item.Name );
          categoriesIds.Add( teamCategory!.Id );
        }
        tsDetails = tsDetails.Where( x => categoriesIds.Contains( x.TimesheetCategoryId!.Value ) );

      }
      // summary by user
      var users = new HashSet<ProjectUserDetail>();
      foreach ( var user in tsDetails.GroupBy( t => t.CreatedBy ).ToDictionary( k => k.Key, v => v.ToList().GroupBy( t => t.Type ).ToDictionary( key => key.Key, val => val.Sum( t => t.Hours ) ) ) ) {
        var userData = await _userRepository.GetByConditionNoTrack( u => u.Id == user.Key );
        if ( userData != null ) {
          users.Add( new ProjectUserDetail()
          {
            Name = userData.UserName,
            WorkingHours = ( categories.Count() > 0 && user.Value.ContainsKey( ( int ) Enums.TimesheetDetailType.Project ) ) ? user.Value [ ( int ) Enums.TimesheetDetailType.Project ] : 0,
          } );
        }
      }
      // get data project
      var timesheetIds = tsDetails.Select( x => x.TimesheetId ).ToList();
      var timeSheet = tsDetails.Count() > 0 ? await _timesheetRepository.GetListByCondition( t => timesheetIds.Any( x => x == t.Id ) ) : null;
      var result = new AnalysisResponse()
      {
        ProjectId = projectId,
        ProjectName = Common.FormatProjectName( project! ),
        Status = project!.Status == 1 ? "Active" : "Inactive",
        Summary = new ProjectSummary()
        {
          TeamName = team!.Name,
          Manager = await GetManagers( project ),
          TotalMember = users.Count(),
          Start = project!.StartedDate,
          End = project.FinishedDate,
          TotalWorkingHour = tsDetails.Where( t => t.Type == ( int ) Enums.TimesheetDetailType.Project ).Sum( t => t.Hours ),
          Quotation = project.QuotationHour,
          QuotationDay = project.QuotationHour > 0 ? ( long ) Math.Ceiling( ( double ) project.QuotationHour / 8 ) : 0,
          CurrentDay = timeSheet != null ? timeSheet.Count() : 0
        },
        Categories = categories.ToArray(),
        Users = users.ToArray(),
        ProjectType = ( await _configRepository.GetByConditionNoTrack( c => c.Code == project.Type ) )?.Name ?? string.Empty,
        Client = project.Client!.Name
      };
      return result;
    }

    public async Task<byte []?> ExportAnalysis( long id )
    {
      // get data timesheets
      var timesheets = await _timesheetDetailRepository.GetDataTimesheet( t => t.ProjectId == id &&
        ( t.Type == ( int ) Enums.TimesheetDetailType.Project ) );

      Dictionary<long, User> users = new Dictionary<long, User>();
      Dictionary<long, string> categories = new Dictionary<long, string>();
      Dictionary<long, string> objects = new Dictionary<long, string>();
      byte [] workbookBytes;
      var pathFileTemp = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Excel", "AnalysisTimesheet.xlsx" );
      using ( MemoryStream templateStream = new() ) {
        using ( FileStream fileStream = new( pathFileTemp, FileMode.Open, FileAccess.Read ) ) {
          fileStream.CopyTo( templateStream );
          templateStream.Position = 0;
        }
        var workBook = new XLWorkbook( templateStream );
        var workSheetDetail = workBook.Worksheet( "Sheet1" );
        var rowIndex = 2;
        var projects = new Dictionary<long, string>();

        foreach ( var ts in timesheets.OrderBy( t => t.Timesheet.Date ).ThenBy( t => t.Timesheet.UserId ) ) {
          // cell STT
          workSheetDetail.Cell( $"A{rowIndex}" ).Value = rowIndex - 1;
          // cell Date
          workSheetDetail.Cell( $"B{rowIndex}" ).Value = ts.Timesheet.Date.ToString( Enums.DATE_FORMAT );

          if ( !users.ContainsKey( ts.Timesheet.UserId ) ) {
            var user = await _userRepository.GetByConditionNoTrack( u => u.Id == ts.Timesheet.UserId );
            users.Add( ts.Timesheet.UserId, user! );
          }
          // cell User
          workSheetDetail.Cell( $"C{rowIndex}" ).Value = users [ ts.Timesheet.UserId ].FullName;
          // cell Email
          workSheetDetail.Cell( $"D{rowIndex}" ).Value = users [ ts.Timesheet.UserId ].Email;

          // cell Hour
          workSheetDetail.Cell( $"E{rowIndex}" ).Value = ts.Hours;
          // cell Type
          workSheetDetail.Cell( $"F{rowIndex}" ).Value = ts.Type == ( int ) Enums.TimesheetDetailType.Project ? string.Empty : string.Empty;

          if ( !categories.ContainsKey( ( int ) ts.TimesheetCategoryId! ) ) {
            var category = await _categoryRepository.GetByConditionNoTrack( c => c.Id == ts.TimesheetCategoryId );
            categories.Add( ( int ) ts.TimesheetCategoryId!, category!.Name );
          }
          // cell Category
          workSheetDetail.Cell( $"G{rowIndex}" ).Value = categories [ ( int ) ts.TimesheetCategoryId! ];
          // cell Description
          workSheetDetail.Cell( $"H{rowIndex}" ).Value = ts.Description;
          rowIndex += 1;
        }
        workBook.Save();
        workbookBytes = templateStream.ToArray();
      }
      return workbookBytes;
    }

    //public async Task<bool> IsMemberOfProject( long id, long userId, long projectManager )
    //{
    //  return await _projectRepository.Exist( p => p.Id == id && Common.IsProjectManager( p.ProjectManagement, projectManager ) && p.Status == ( int ) Enums.ProjectStatus.Active ) &&
    //    ( await _projectRepository.GetMemberOfProjects( projectManager ) ).Contains( userId );
    //}
    public async Task<string> GenerateProjectCode( Project project )
    {
      var team = await _teamRepository.GetByCondition( t => t.Id == project.TeamId );
      var charFirst = team!.Code.Substring( 0, 1 ).ToUpper();
      var lastProject = await _projectRepository.GetLastProject();
      var projectNumber = project.ProjectNumber;
      return $"{Common.GetFiscalYear():00}{project.Type}{projectNumber}{charFirst}";
    }

    public async Task<List<ProjectContractResponse>> GetProjectContract( long projectId )
    {
      var contracts = await _projectContractRepository.GetListDetail( projectId );
      var response = contracts!.Select( x => new ProjectContractResponse
      {
        Id = x.Id,
        ContractCode = x.Code,
        ContractDate = x.Date,
        ContractName = x.Name,
        Docs = _mapper.Map<ProjectDocumentDetail []>( x.ProjectDocuments )
      } );
      return response.ToList();
    }

    public async Task<long> CreateContract( long projectId, ProjectContractResource resource )
    {
      var contract = _mapper.Map<ProjectContract>( resource );
      contract.CreatedDate = DateTime.UtcNow;
      contract.ProjectId = projectId;
      await _projectContractRepository.CreateAsync( contract );
      if ( resource.Document is not null && resource.Document.Count > 0 ) {
        foreach ( var file in resource.Document ) {
          await UploadDocument( contract.Id, file, contract.CreatedBy );
        }
      }

      return contract.Id;
    }

    private async Task UploadDocument( long contractId, IFormFile file, long uploader )
    {
      if ( file is null || file.Length == 0 ) {
        throw new Exception( ErrorMessageResource.DocumentNull );
      }
      if ( file.Length > Enums.MAX_DOCUMENT_SIZE ) {
        throw new Exception( ErrorMessageResource.DocumentOverSize );
      }
      if ( !Enums.DOCUMENT_EXTENSION.Contains( Path.GetExtension( file.FileName ) ) ) {
        throw new Exception( ErrorMessageResource.DocumentExtension );
      }
      var fileName = $"{contractId}_{file.FileName}";
      var path = Path.GetFullPath( Path.Combine( Directory.GetCurrentDirectory(), "Upload", "Documents" ) );
      // create folder storage
      if ( !Directory.Exists( path ) ) {
        Directory.CreateDirectory( path );
      }
      // save new file
      if ( System.IO.File.Exists( Path.Combine( path, fileName ) ) ) {
        System.IO.File.Delete( Path.Combine( path, fileName ) );
      }
      using ( var fileStream = new FileStream( Path.Combine( path, fileName ), FileMode.Create ) ) {
        await file.CopyToAsync( fileStream );
      }
      var doc = await _projectDocumentRepository.GetByCondition( p => p.ContractId == contractId && p.Name == fileName );
      var document = new ProjectDocument()
      {
        ContractId = contractId,
        Name = fileName,
        Link = $"/Upload/Documents/{fileName}",
        CreatedBy = uploader,
      };
      if ( doc is not null ) {
        _projectDocumentRepository.Delete( document );
      }

      await _projectDocumentRepository.CreateAsync( document );
    }

    public async Task UpdateContract( long projectId, ProjectContractResource resource, long editor )
    {
      var old = await _projectContractRepository.GetByCondition( x => x.Id == resource.Id );
      _mapper.Map( resource, old );
      _projectContractRepository.Update( old! );
      if ( resource.Document is not null && resource.Document.Count() > 0 ) {
        foreach ( var file in resource.Document ) {
          await UploadDocument( old!.Id, file, old!.CreatedBy );
        }
      }
    }
    public async Task<List<ProjectDocumentDetail>> GetDocumentInContract( long contractId )
    {
      var documents = await _projectDocumentRepository.GetListByCondition( x => x.ContractId == contractId );
      return _mapper.Map<List<ProjectDocumentDetail>>( documents );
    }

    public async Task<List<ProjectDocumentDetail>> DeleteContractDocument( long documentId )
    {
      var document = await _projectDocumentRepository.GetByCondition( x => x.Id == documentId );

      await _projectDocumentRepository.DeleteByCondition( p => p.Id == documentId );

      var documents = await _projectDocumentRepository.GetListByCondition( x => x.ContractId == document!.ContractId );
      return _mapper.Map<List<ProjectDocumentDetail>>( documents );
    }
    public async Task DeleteContract( long contractId )
    {
      var documents = await _projectDocumentRepository.GetListByCondition( x => x.ContractId == contractId );
      if ( documents.Count() > 0 ) {
        foreach ( var doc in documents ) {
          var fileName = $"{contractId}_{doc.Name}";
          var path = Path.Combine( Directory.GetCurrentDirectory(), "Upload", "Documents", fileName );
          if ( System.IO.File.Exists( path ) ) {
            System.IO.File.Delete( path );
          }
        }
        _projectDocumentRepository.Deletes( documents );

      }
      await _projectContractRepository.DeleteByCondition( p => p.Id == contractId );
    }

    public async Task<List<UserOption>> AddUserProject( long id, List<long> userIds )
    {
      var project = await _projectRepository.GetByConditionNoTrack( x => x.Id == id );
      var members = new List<User>();
      if ( project != null ) {
        if ( userIds is not null && userIds.Count > 0 ) {
          foreach ( var memId in userIds ) {
            var user = await _userRepository.GetUserDataByConditionNoTracking( u => u.Id == memId );
            members.Add( user! );
          }
        }
        await _projectRepository.UpdateMemberAsync( id, members );
      }
      var result = await _projectRepository.GetProjectDataById( id );
      return _mapper.Map<List<UserOption>>( result!.Users );
    }
    public async Task<List<UserOption>> RemoveUserProject( long id, long userId )
    {
      var project = await _projectRepository.GetByConditionNoTrack( x => x.Id == id );
      if ( project is not null ) {
        var user = await _userRepository.GetUserDataByConditionNoTracking( u => u.Id == userId );
        await _projectRepository.UpdateMemberAsync( id, new List<User>() { user! }, false );
      }
      var result = await _projectRepository.GetProjectDataById( id );
      return _mapper.Map<List<UserOption>>( result!.Users );
    }

    private async Task<string> GetManagers( Project project )
    {
      var parts = project.ProjectManagement.Split( ',' );
      var pms = parts.Select( long.Parse ).ToList();
      var list = new List<string>();
      foreach ( var id in pms ) {
        var userManagement = await _userRepository.GetUserDataByConditionNoTracking( u => u.Id == id );
        list.Add( userManagement!.FullName );
      }
      return string.Join( ",", list );
    }
    #endregion

  }
}