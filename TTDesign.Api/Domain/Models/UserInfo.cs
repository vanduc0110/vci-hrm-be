using System.ComponentModel;

namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// thông tin bổ sung của user
  /// </summary>
  public partial class UserInfo
  {
    public long UserId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string? IdNo { get; set; }
    public DateTime? IssuedTo { get; set; }
    public string? IssuedBy { get; set; }
    public string? Address { get; set; }
    public string? SocialInsuranceBookNo { get; set; }
    public string? AboutMe { get; set; }
    public int FingerId { get; set; }
    public string? BankName { get; set; }
    public string? AccountBank { get; set; }
    [DefaultValue( 0 )]
    public int Dependent { get; set; } = 0;
  }
}
