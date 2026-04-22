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
  [Index( nameof( Email ), IsUnique = true, Name = "Ix_UserCode" )]
  public class UserResource : IValidatableObject
  {
    /// <summary>
    /// User ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// User Full Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserFullName" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string FullName { get; set; } = null!;
    /// <summary>
    /// Email [input] [format]
    /// </summary>
    /// <example>user@ttdesignco.com</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserEmail" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    [DataType( DataType.EmailAddress )]
    public string Email { get; set; } = null!;
    /// <summary>
    /// Staff ID [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserStaffId" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [StringLength( 30, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string StaffId { get; set; } = null!;
    /// <summary>
    /// Position [input]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserPosition" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public string Position { get; set; } = null!;
    /// <summary>
    /// Role Name [input] [format]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserRole" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long RoleId { get; set; }
    [JsonIgnore]
    public string Role { get; set; } = string.Empty;
    /// <summary>
    /// Date Start Working [input]
    /// </summary>
    /// <example>định dạng: yyyy-MM-dd</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserStartWork" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [DataType( DataType.Date )]
    public DateTime DateStartWork { get; set; }

    /// <summary>
    /// Finger Printer ID [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserFingerId" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    [Range( 1, 1000000, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public int FingerId { get; set; }
    /// <summary>
    /// Phone Number [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserPhoneNumber" )]
    [StringLength( 20, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// Gender [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserGender" )]
    [StringLength( 20, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? Gender { get; set; }
    /// <summary>
    /// Birthday [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserBirthday" )]
    [DataType( DataType.Date )]
    public DateTime? Birthday { get; set; }
    /// <summary>
    /// ID No [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserIdNo" )]
    [StringLength( 20, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? IdNo { get; set; }
    /// <summary>
    /// Issued To [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserIssuedTo" )]
    [DataType( DataType.Date )]
    public DateTime? IssuedTo { get; set; }
    /// <summary>
    /// IssuedBy [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserIssuedBy" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? IssuedBy { get; set; }
    /// <summary>
    /// Address [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserAddress" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? Address { get; set; }
    /// <summary>
    /// Social Insurance Book No [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserSocialInsuranceBookNo" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? SocialInsuranceBookNo { get; set; }
    /// <summary>
    /// About Me [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserAboutMe" )]
    [StringLength( 500, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? AboutMe { get; set; }
    /// <summary>
    /// Belong Team ID [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserTeam" )]
    [Required( ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "RequiredError" )]
    public long [] TeamIds { get; set; } = new long [] { 1 };

    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserAccountBank" )]
    [StringLength( 50, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? AccountBank { get; set; }

    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserBankName" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? BankName { get; set; }
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserDependent" )]
    public int Dependent { get; set; } = 0;
    /// <summary>
    /// valid unique TeamCode
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      if ( !string.IsNullOrEmpty( StaffId ) ) {
        StaffId = Common.FormatCodeInput( StaffId );
      }
      if ( !string.IsNullOrEmpty( FullName ) ) {
        FullName = Common.RemoveMultiBlank( FullName );
      }
      if ( !string.IsNullOrEmpty( Email ) ) {
        Email = Common.FormatEmail( Email );
      }
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ||
        ( Id is not null && !dbContext.Users.Any( t => t.Id == Id ) ) ||
        ( Id is not null && dbContext.Users.Any( t => t.Id == Id && t.IsActive ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ), new [] { Enums.ERROR_TEXT } ) );
      }
      else {
        if ( dbContext.Users.Any( t => t.Id != Id && t.StaffId == StaffId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.UserStaffId ), new [] { nameof( StaffId ) } ) );
        }
        if ( dbContext.Users.Any( t => t.Id != Id && t.Email == Email ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.UserEmail ), new [] { nameof( Email ) } ) );
        }
        if ( !string.IsNullOrEmpty( Position ) && !Enum.IsDefined( typeof( Enums.UserPosition ), Position ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.UserPosition ), new [] { nameof( Position ) } ) );
        }
        // hệ thống chỉ có duy nhất 1 user position system
        else if ( Position == Enum.GetName( Enums.UserPosition.System ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.UserPosition ), new [] { nameof( Position ) } ) );
        }
        // 1 user thường chỉ có trong 1 team, trừ team lead và director
        else if ( ( Position != Enum.GetName( Enums.UserPosition.TeamLead ) && Position != Enum.GetName( Enums.UserPosition.Director ) ) && TeamIds.Count() > 1 ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.SetUserTeam, DisplayNameResource.UserPosition ), new [] { nameof( Position ) } ) );
        }
        var role = dbContext.Roles.FirstOrDefault( t => t.Id == RoleId );
        if ( role is null ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.UserRole ), new [] { nameof( Role ) } ) );
        }
        else if ( role.Name == Roles.ROLE_NAME_SYSTEM ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UserNotPermission, DisplayNameResource.UserRole ), new [] { nameof( Role ) } ) );
        }
        else {
          Role = role.Name;
        }
        if ( FingerId > 0 && dbContext.UserInfos.Any( t => t.UserId != Id && t.FingerId == FingerId ) ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.UniquedError, DisplayNameResource.UserFingerId ), new [] { nameof( FingerId ) } ) );
        }
        if ( TeamIds == null || TeamIds.Count() == 0 ) {
          results.Add( new ValidationResult( string.Format( ErrorMessageResource.RequiredError, DisplayNameResource.UserTeam ), new [] { nameof( TeamIds ) } ) );
        }
        else {
          foreach ( var teamId in TeamIds ) {
            if ( !dbContext.Teams.Any( u => u.Id == teamId ) ) {
              results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.UserTeam ), new [] { nameof( TeamIds ) } ) );
              break;
            }
          }
        }
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model tự update
  /// </summary>
  public class YourSelfResource : IValidatableObject
  {
    /// <summary>
    /// User ID 
    /// </summary>
    [JsonIgnore]
    public long? Id { get; set; }
    /// <summary>
    /// Phone Number [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserPhoneNumber" )]
    [StringLength( 20, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// Gender [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserGender" )]
    [StringLength( 20, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? Gender { get; set; }
    /// <summary>
    /// Birthday [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserBirthday" )]
    [DataType( DataType.Date )]
    public DateTime? Birthday { get; set; }
    /// <summary>
    /// ID No [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserIdNo" )]
    [StringLength( 20, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? IdNo { get; set; }
    /// <summary>
    /// Issued To [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserIssuedTo" )]
    [DataType( DataType.Date )]
    public DateTime? IssuedTo { get; set; }
    /// <summary>
    /// IssuedBy [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserIssuedBy" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? IssuedBy { get; set; }
    /// <summary>
    /// Address [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserAddress" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? Address { get; set; }
    /// <summary>
    /// Social Insurance Book No [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserSocialInsuranceBookNo" )]
    [StringLength( 200, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? SocialInsuranceBookNo { get; set; }
    /// <summary>
    /// About Me [input]
    /// </summary>
    /// <example></example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserAboutMe" )]
    [StringLength( 500, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? AboutMe { get; set; }

    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserAccountBank" )]
    [StringLength( 50, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? AccountBank { get; set; }

    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserBankName" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string? BankName { get; set; }
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserDependent" )]
    public int Dependent { get; set; } = 0;
    /// <summary>
    /// valid unique TeamCode
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ||
        ( Id is not null && !dbContext.Users.Any( t => t.Id == Id ) ) ||
        ( Id is not null && dbContext.Users.Any( t => t.Id == Id && t.IsActive ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ), new [] { Enums.ERROR_TEXT } ) );
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api [Get List]
  /// </summary>
  public class UserResponse
  {
    /// <summary>
    /// User ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// Staff ID
    /// </summary>
    /// <example>text</example>
    [Required]
    public string StaffId { get; set; } = string.Empty;
    /// <summary>
    /// User Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// Full Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Email
    /// </summary>
    /// <example>user@ttdesignco.com</example>
    [Required]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Team User
    /// </summary>
    public List<TeamUserOption> Teams { get; set; } = new List<TeamUserOption>();
    /// <summary>
    /// Position
    /// </summary>
    /// <example>Official</example>
    [Required]
    public string Position { get; set; } = string.Empty;
    /// <summary>
    /// Start Work
    /// </summary>
    [Required]
    public DateTime DateStartWork { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string DateStartWorkText => DateStartWork.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Modified By
    /// </summary>
    public long ModifiedBy { get; set; }
    /// <summary>
    /// Modified By
    /// </summary>
    /// <example>text</example>
    [Required]
    public string ModifierName { get; set; } = string.Empty;
    /// <summary>
    /// Modified Date
    /// </summary>
    [Required]
    public DateTime ModifiedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string ModifiedDateText => ModifiedDate.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Statis
    /// </summary>
    /// <example>Active</example>
    [Required]
    public string Status { get; set; } = string.Empty;
    public string? AccountBank { get; set; }
    public string? BankName { get; set; }
    public int Dependent { get; set; } = 0;
    public long RoleId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? Avatar { get; set; } = string.Empty;
  }

  /// <summary>
  /// Class cho model output api [Option]
  /// </summary>
  public class UserOption
  {
    /// <summary>
    /// User ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// User Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// User FullName
    /// </summary>
    /// <example>text</example>
    [Required]
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Team ID
    /// </summary>
    [Required]
    public long [] TeamIds { get; set; } = Array.Empty<long>();
    public List<TeamUserOption> Teams { get; set; } = new List<TeamUserOption>();
    /// <summary>
    /// Position
    /// </summary>
    /// <example>Official</example>
    [Required]
    public string Position { get; set; } = string.Empty;
    /// <summary>
    /// Avatar, file name => url api/Upload/file name
    /// </summary>
    /// <example>path file</example>
    [Required]
    public string? Avatar { get; set; } = string.Empty;
    /// <summary>
    /// State
    /// </summary>
    /// <example>Available</example>
    [Required]
    public string State { get; set; } = string.Empty;
  }

  /// <summary>
  /// Class cho model output api [Get View]
  /// </summary>
  public class UserDetailResponse
  {
    /// <summary>
    /// User ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// User Full Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string FullName { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// Email
    /// </summary>
    /// <example>user@ttdesignco.com</example>
    [Required]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Staff ID
    /// </summary>
    /// <example>text</example>
    [Required]
    public string StaffId { get; set; } = string.Empty;
    /// <summary>
    /// Position
    /// </summary>
    /// <example>Official</example>
    [Required]
    public string Position { get; set; } = string.Empty;
    /// <summary>
    /// Role Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public long RoleId { get; set; }
    public string Role { get; set; } = string.Empty;
    /// <summary>
    /// User State
    /// </summary>
    /// <example>Available</example>
    [Required]
    public string State { get; set; } = string.Empty;
    /// <summary>
    /// Date Start Working
    /// </summary>
    [Required]
    public DateTime DateStartWork { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string DateStartWorkText => DateStartWork.ToString( Enums.DATE_FORMAT );
    /// <summary>
    /// Avatar, file name => url api/Upload/file name
    /// </summary>
    /// <example></example>
    [Required]
    public string? Avatar { get; set; } = null!;

    /// <summary>
    /// Finger Printer ID
    /// </summary>
    public int FingerId { get; set; }
    /// <summary>
    /// Phone Number
    /// </summary>
    /// <example></example>
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// Gender
    /// </summary>
    /// <example></example>
    public string? Gender { get; set; }
    /// <summary>
    /// Birthday
    /// </summary>
    public DateTime? Birthday { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example></example>
    public string BirthdayText => Birthday != null ? ( ( DateTime ) Birthday ).ToString( Enums.DATE_FORMAT ) : string.Empty;
    /// <summary>
    /// ID No
    /// </summary>
    /// <example></example>
    public string? IdNo { get; set; }
    /// <summary>
    /// Issued To
    /// </summary>
    public DateTime? IssuedTo { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example></example>
    public string IssuedToText => IssuedTo != null ? ( ( DateTime ) IssuedTo ).ToString( Enums.DATE_FORMAT ) : string.Empty;
    /// <summary>
    /// IssuedBy
    /// </summary>
    /// <example></example>
    public string? IssuedBy { get; set; }
    /// <summary>
    /// Address
    /// </summary>
    /// <example></example>
    public string? Address { get; set; }
    /// <summary>
    /// Social Insurance Book No
    /// </summary>
    /// <example></example>
    public string? SocialInsuranceBookNo { get; set; }
    /// <summary>
    /// About Me
    /// </summary>
    /// <example></example>
    public string? AboutMe { get; set; }
    public string? AccountBank { get; set; }
    public string? BankName { get; set; }
    public int Dependent { get; set; } = 0;
    public List<TeamUserOption> Teams { get; set; } = new List<TeamUserOption>();
  }

  public class UserShort
  {
    public long UserId { get; set; }
    /// <summary>
    /// dữ liệu null, trường không trả về
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
    public string UserName { get; set; } = null!;
    /// <summary>
    /// dữ liệu null, trường không trả về
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
    public string? FullName { get; set; } = null!;

    /// <summary>
    /// Start Work
    /// </summary>
    public DateTime? DateStartWork { get; set; }
    /// <summary>
    /// dữ liệu null, trường không trả về
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
    public string UserType { get; set; } = null!;
    /// <summary>
    /// dữ liệu null, trường không trả về
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
    public string Position { get; set; } = null!;
    /// <summary>
    /// dữ liệu null, trường không trả về
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
    public string TeamName { get; set; } = null!;
    /// <summary>
    /// dữ liệu null, trường không trả về
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
    public string? Avatar { get; set; }
    public string? AccountBank { get; set; }
    public string? BankName { get; set; }
    public int Dependent { get; set; } = 0;

  }

  /// <summary>
  /// Class cho model input api User Setting [Create]/[Update]
  /// </summary>
  public class UserSettingResource : IValidatableObject
  {
    /// <summary>
    /// User ID khi update [hide]
    /// </summary>
    public long? Id { get; set; }
    /// <summary>
    /// Email Notification [input]
    /// </summary>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserSettingEmailNotification" )]
    public ulong EmailNotification { get; set; }
    /// <summary>
    /// Timezone [input]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserSettingTimeZone" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Timezone { get; set; } = null!;
    /// <summary>
    /// Language [input]
    /// </summary>
    /// <example>text</example>
    [Display( ResourceType = typeof( DisplayNameResource ), Name = "UserSettingLanguage" )]
    [StringLength( 100, ErrorMessageResourceType = typeof( ErrorMessageResource ), ErrorMessageResourceName = "MaxLengthError" )]
    public string Language { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
    {
      var results = new List<ValidationResult>();
      var dbContext = validationContext.GetService( typeof( AppDbContext ) ) as AppDbContext;
      if ( dbContext is null ||
        ( Id is not null && !dbContext.Users.Any( t => t.Id == Id ) ) ||
        ( Id is not null && dbContext.Users.Any( t => t.Id == Id && t.IsActive ) ) ) {
        results.Add( new ValidationResult( string.Format( ErrorMessageResource.ExistFieldError, DisplayNameResource.User ) ) );
      }
      return results;
    }
  }

  /// <summary>
  /// Class cho model output api User Setting
  /// </summary>
  public class UserSettingResponse
  {
    /// <summary>
    /// User ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Email Notification
    /// </summary>
    public ulong EmailNotification { get; set; }
    /// <summary>
    /// Timezone
    /// </summary>
    [Required]
    public string Timezone { get; set; } = null!;
    /// <summary>
    /// Language
    /// </summary>
    [Required]
    public string Language { get; set; } = null!;
  }

  /// <summary>
  /// kiểu option chung cho user/team/group
  /// </summary>
  public class DynamicOption
  {
    /// <summary>
    /// option id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// option name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// user, team, group, position
    /// </summary>
    public string Type { get; set; } = string.Empty;
  }

  /// <summary>
  /// Class cho view dashboard: users
  /// </summary>
  public class DashboardUser
  {
    /// <summary>
    /// User ID
    /// </summary>
    [Required]
    public long Id { get; set; }
    /// <summary>
    /// User Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Avatar, file name => url api/Upload/file name
    /// </summary>
    /// <example></example>
    [Required]
    public string? Avatar { get; set; } = string.Empty;
    /// <summary>
    /// State
    /// </summary>
    /// <example>Active</example>
    [Required]
    public string State { get; set; } = string.Empty;
  }

  /// <summary>
  /// model phục vụ view thông tin user khác từ view là user staff bình thường
  /// </summary>
  public class DetailOtherUser
  {
    /// <summary>
    /// User ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Họ tên đầy đủ
    /// </summary>
    /// <example>text</example>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// định danh trong hệ thống
    /// </summary>
    /// <example>text</example>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// ngày tháng năm sinh
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>định dạng: yyyy/MM/dd</example>
    public string DateOfBirthText => DateOfBirth != null ? ( ( DateTime ) DateOfBirth ).ToString( Enums.DATE_FORMAT ) : string.Empty;
    /// <summary>
    /// số điện thoại
    /// </summary>
    /// <example></example>
    public string? Phone { get; set; }

    /// <summary>
    /// thuộc team
    /// </summary>
    /// <example>text</example>
    public List<TeamUserOption> Teams { get; set; }
    /// <summary>
    /// chức vụ
    /// </summary>
    /// <example>Official</example>
    public string Position { get; set; } = string.Empty;
    /// <summary>
    /// Email
    /// </summary>
    /// <example>user@ttdesignco.com</example>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// địa chỉ
    /// </summary>
    /// <example></example>
    public string? Address { get; set; }
    /// <summary>
    /// thông tin trích ngang
    /// </summary>
    /// <example></example>
    public string? About { get; set; }
    /// <summary>
    /// link avatar
    /// </summary>
    /// <example></example>
    public string? Avatar { get; set; }
  }

  public class UserChangeState
  {
    public string State { get; set; }
  }
  public class TeamUserOption
  {
    /// <summary>
    /// team id
    /// </summary>
    public long? TeamId { get; set; }
    /// <summary>
    /// Team Code
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TeamCode { get; set; } = string.Empty;
    /// <summary>
    /// Team Name
    /// </summary>
    /// <example>text</example>
    [Required]
    public string TeamName { get; set; } = string.Empty;
  }
}
