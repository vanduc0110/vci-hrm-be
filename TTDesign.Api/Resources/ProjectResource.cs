using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  /// <summary>
  /// Class cho model input api [Create]/[Update]
  /// </summary>
  [Index( nameof( Code ), IsUnique = true, Name = "Ix_ProjectCode" )]
  public class ProjectResource : IValidatableObject
  {
    /// <summary>
    /// Project ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// Project Type [input]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectType" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string Type { get; set; } = null!;
    /// <summary>
    /// Project Code [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectCode" )]
    [StringLength( 30, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? Code { get; set; } = null!;
    /// <summary>
    /// Team ID [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectTeam" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long TeamId { get; set; }
    /// <summary>
    /// Project Client [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectClient" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long ClientId { get; set; }
    /// <summary>
    /// Project Status [input]
    /// </summary>
    /// <example>Pending</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectStatus" )]
    public string? Status { get; set; }
    /// <summary>
    /// Quotation Hours [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectQuotation" )]
    public long? QuotationHour { get; set; }
    /// <summary>
    /// Project Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Project Manager [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectManager" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long [] Manager { get; set; }
    /// <summary>
    /// Start Date [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectStart" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public DateTime StartedDate { get; set; }
    /// <summary>
    /// Finish Date [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectFinish" )]
    public DateTime? FinishedDate { get; set; }
    /// <summary>
    /// Danh sách Group thực hiện Project [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "ProjectGroup" )]
    public List<long>? MemberIds { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( Code ) ) {
        Code = Common.FormatCodeInput( Code );
      }
      if ( !string.IsNullOrEmpty( Name ) ) {
        Name = Common.RemoveMultiBlank( Name );
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.Projects.Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.Project ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( !dbContext.Configs.Any( t => t.Code == Type && t.Type == "ProjectType" ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.ProjectType ), new [] { nameof( Type ) } ) );
        }
        if ( dbContext.Projects.Any( t => t.Id != Id && t.Code == Code ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.ProjectCode ), new [] { nameof( Code ) } ) );
        }
        if ( !dbContext.Teams.Any( t => t.Id == TeamId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.ProjectTeam ), new [] { nameof( TeamId ) } ) );
        }
        if ( !dbContext.Clients.Any( t => t.Id == ClientId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.ProjectClient ), new [] { nameof( ClientId ) } ) );
        }
        if ( Id > 0 && ( string.IsNullOrEmpty( Status ) || !Enum.IsDefined( typeof( Enums.ProjectStatus ), Status ) ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.ProjectStatus ), new [] { nameof( Status ) } ) );
        }
        //if ( !dbContext.Users.Any( t => t.Id == Manager && t.Position == ( int ) Enums.UserPosition.PM ) ) {
        //  results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.ProjectManager ), new [] { nameof( Manager ) } ) );
        //}
        if ( FinishedDate != null && FinishedDate < StartedDate ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.DatePastError, DisplayNameResource.ProjectStart ), new [] { nameof( StartedDate ) } ) );
        }
        if ( MemberIds == null || MemberIds.Count == 0 ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.RequiredError, DisplayNameResource.User ), new [] { nameof( MemberIds ) } ) );
        }
        else {
          foreach ( var memId in MemberIds ) {
            if ( !dbContext.Users.Any( u => u.Id == memId ) ) {
              results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ), new [] { nameof( MemberIds ) } ) );
              break;
            }
          }
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class ProjectResponse
  {
    /// <summary>
    /// Project ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Project Code
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// Project Full Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Project Team
    /// </summary>
    public long TeamId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TeamCode { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TeamName { get; set; } = string.Empty;
    /// <summary>
    /// Project Quotation Hours
    /// </summary>
    public long QuotationHour { get; set; }
    /// <summary>
    /// Project Total Working Hours
    /// </summary>
    public double TotalHour { get; set; }

    /// <summary>
    /// Project Quotation Day
    /// </summary>
    public long QuotationDay { get; set; } = 0;
    /// <summary>
    /// Project Total Working Day
    /// </summary>
    public double CurrentDay { get; set; } = 0;
    /// <summary>
    /// Project Usage Percentage
    /// </summary>
    public double UsagePercentage => TotalHour > 0 && QuotationHour > 0 ? ( Math.Round( ( TotalHour / QuotationHour ) * 100, 2 ) ) : 0;
    /// <summary>
    /// Số member trong Project
    /// </summary>
    public long Amount { get; set; }
    /// <summary>
    /// Ngày tạo Project
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string CreatedDateText => CreatedDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Project Status
    /// </summary>
    /// <example>Pending</example>
    [Required]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Number of Contracts
    /// </summary>
    public int Contracts { get; set; }
    /// <summary>
    /// Project Type
    /// </summary>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// Project Type Name
    /// </summary>
    public string ConfigName { get; set; } = string.Empty;
    /// <summary>
    /// danh sách user tham gia Project
    /// </summary>
    public UserShort [] Users { get; set; } = null!;
  }

  /// <summary>
  /// Class cho model output api [Option]
  /// </summary>
  public class ProjectOption
  {
    /// <summary>
    /// Project ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Project Code
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// Project Full Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string FullName { get; set; } = string.Empty;

    public long? TeamId { get; set; }

    public List<CategoryProject> CategoryProjects { get; set; } = new List<CategoryProject>();
  }

  /// <summary>
  /// Class cho model ouput api [Get View]
  /// </summary>
  public class ProjectDetailResponse
  {
    /// <summary>
    /// Project ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// năm tài chính
    /// </summary>
    public int FiscalYear { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ProjectNumber { get; set; }
    /// <summary>
    /// Project Type
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// Project Code
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// Team ID
    /// </summary>
    public long TeamId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TeamCode { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TeamName { get; set; } = string.Empty;
    /// <summary>
    /// Project Client
    /// </summary>
    public long ClientId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [Required]
    public string ClientName { get; set; } = string.Empty;
    /// <summary>
    /// Project Status
    /// </summary>
    /// <example>Pending</example>
    [Required]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// Quotation Hours
    /// </summary>
    public long QuotationHour { get; set; }
    /// <summary>
    /// Project Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Project Full Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Project Manager
    /// </summary>
    public string Manager { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [Required]
    public string ManagerName { get; set; } = string.Empty;
    /// <summary>
    /// Start Date
    /// </summary>
    [Required]
    public DateTime StartedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string StartedDateText => StartedDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Finish Date
    /// </summary>
    public DateTime? FinishedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string FinishDateText => FinishedDate != null ? ( ( DateTime ) FinishedDate ).ToString( Enums.DATE_FORMAT ) : string.Empty;
    /// <summary>
    /// Danh sách User trong các Group thực hiện Project
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public List<UserOption>? Members { get; set; }
  }

  public class ProjectAddUser
  {
    public List<long> UserIds { get; set; } = new List<long>();
  }
}
