namespace TTDesign.API.Security.Tokens
{
  public class TokenOptionsConfig
  {
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public long AccessTokenExpiration { get; set; }
    public long RefreshTokenExpiration { get; set; }
    public string Secret { get; set; } = null!;
  }
}
