namespace TTDesign.API.Resources
{
  public class AssetUserResponse
  {
    public long Id { get; set; }
    public string Fullname { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Computers { get; set; }
    public int Screens { get; set; }
    public int Components { get; set; }
    public int Electronics { get; set; }
    public int Funiture { get; set; }
    public int Others { get; set; }
  }

  public class AssetByUserResponse
  {
    public long Id { get; set; }
    public string Fullname { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public List<AssetResponse> Assets { get; set; } = new List<AssetResponse>();
  }
}
