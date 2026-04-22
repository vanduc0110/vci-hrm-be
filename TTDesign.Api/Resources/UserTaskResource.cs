using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TTDesign.API.Constants;
using TTDesign.API.Extensions;
using TTDesign.API.Languages;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Resources
{
  public class UserTaskResource : IValidatableObject
  {
    /// <summary>
    /// Task ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Name of the task [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "TaskName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Mô tả chi tiết về task [input]
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Trang thái của task [input] 
    /// </summary>
    public string Status { get; set; } = "None";
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( Name ) ) {
        Name = Common.RemoveMultiBlank( Name );
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null || ( Id is not null && !dbContext.UserTasks.AsNoTracking().Any( t => t.Id == Id ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.UserTask ), new [] { Enums.ERROR_TEXT } ) );
      }
      return results;
    }
  }

  public class UserTaskResponse
  {
    /// <summary>
    /// Task ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Name of the task
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Mô tả chi tiết về task
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Trang thái của task
    /// </summary>
    public string Status { get; set; } = string.Empty;
    public long CreateBy { get; set; }
  }
}
