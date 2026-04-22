using System.ComponentModel.DataAnnotations;
using TTDesign.API.Domain.Security.Tokens;

namespace TTDesign.API.Resources
{
  public class CredentialResource
  {
  }

  public class UserCredentialResource
  {
    /// <summary>
    /// 
    /// </summary>
    /// <example>info@vcijsc.com</example>
    [Required]
    [DataType( DataType.EmailAddress )]
    [StringLength( 255 )]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <example>vci@123</example>
    [Required]
    [StringLength( 32 )]
    public string Password { get; set; } = string.Empty;
  }

  public class TokenResponse : BaseResponse
  {
    public AccessToken Token { get; set; }

    public TokenResponse( bool success, Dictionary<string, string> messageValid, AccessToken token ) : base( success, messageValid )
    {
      Token = token;
    }
  }

  public class AccessTokenResponse
  {
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public long Expiration { get; set; }
    public string EnviromentConfig { get; set; } = null!;
  }

  public class RefreshTokenResource
  {
    [Required]
    public string Token { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    /// <example>user@ttdesignco.com</example>
    [Required]
    [DataType( DataType.EmailAddress )]
    [StringLength( 255 )]
    public string UserEmail { get; set; } = null!;
  }

  public class RevokeTokenResource
  {
    [Required]
    public string Token { get; set; } = null!;
  }

  /// <summary>
  /// 
  /// </summary>
  public class UserAndPassResource
  {
    /// <summary>
    /// Password mới [input]
    /// </summary>
    /// <example>text</example>
    [Required]
    public string NewPassword { get; set; } = string.Empty;
  }
}
