
using Models.Models.AuthService;

namespace AuthService.Interfaces;

public interface IUserService
{
    public Task<UserAuthResult> Register(AuthModel authData);
    public Task<UserAuthResult> Login(AuthModel loginData);
}