using Microsoft.AspNetCore.Identity;

namespace TTDesign.API.Domain.Models
{
  /// <summary>
  /// các bộ role
  /// </summary>
  public partial class Role : IdentityRole<long>
  {
    public Role()
    {
      //RoleClaims = new HashSet<RoleClaim>();
      //Users = new HashSet<User>();
    }

    /// <summary>
    /// loại: system (role mặc định, ko được sửa)/ custom (role do user tạo trong quá trình vận hành)
    /// </summary>
    public int Type { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
  }
}
