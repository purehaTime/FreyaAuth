namespace Models.Models.AuthService;

public class UserAuthResult
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public string RefreshToken { get; set; }
    public string AuthToken { get; set; }
}