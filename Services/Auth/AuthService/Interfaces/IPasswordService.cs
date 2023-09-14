namespace AuthService.Interfaces;

public interface IPasswordService
{
    public string GetHashedPassword(string email, string password);
    public bool VerifyPassword(string email, string hash, string password);
}