namespace ConfigHelper.Configs;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
    public int AuthExpires { get; set; }
    public int RefreshExpires { get; set; }
    public int VerificationExpire { get; set; }
}