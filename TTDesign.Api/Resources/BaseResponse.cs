using System.Text.Json.Serialization;
using TTDesign.API.Constants;

namespace TTDesign.API.Resources
{
  public abstract class BaseResponse
  {
    public bool Success { get; protected set; }
    public Dictionary<string, string> MessageValid { get; protected set; }

    public BaseResponse( bool success, Dictionary<string, string> messageValid )
    {
      Success = success;
      MessageValid = messageValid;
    }
  }

  public class BaseMember
  {
    /// <summary>
    /// id của đối tượng
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// loại của đối tượng: User, Team
    /// </summary>
    /// <example>User</example>
    public string Type { get; set; } = Enum.GetName( Enums.DynamicOption.User )!;
  }

  /// <summary>
  /// thông tin mở rộng của model response api GetList
  /// </summary>
  public class BaseModelResponse
  {
    /// <summary>
    /// request ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// User create request
    /// </summary>
    public long CreatedBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public string CreatedName { get; set; } = null!;
    /// <summary>
    /// User Approve/Reject Request
    /// </summary>
    public long Reviewer { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    public string ReviewerName { get; set; } = string.Empty;

    public List<TeamUserOptionResponse> Teams { get; set; } = new List<TeamUserOptionResponse>();
  }

  public class TeamUserOptionResponse
  {
    /// <summary>
    /// thông tin về team
    /// </summary>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public long TeamId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public string TeamCode { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    /// <example>text</example>
    [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
    public string TeamName { get; set; } = null!;
  }
}
