namespace Dashboard.Interfaces;

public interface IJwtTokenValidation
{
    public Task<bool> Validate(string token);
}