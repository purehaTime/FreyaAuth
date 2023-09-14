using ConfigHelper.Configs;
using GrpcHelper.Clients;
using GrpcHelper.Clients.Db;
using GrpcHelper.Clients.Interfaces;
using GrpcHelper.Proto.Auth;
using GrpcHelper.Proto.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcHelper;

public static class GrpcHelperExtension
{
    public static void AddGrpcHelper(this IServiceCollection services, IConfiguration configs)
    {
        var settings = configs.GetSection(nameof(Service)).Get<Service>();
        
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcInterceptor>();
        });

        services.AddGrpcClient<Auth.AuthClient>(options =>
        {
            options.Address = new Uri(settings.AuthLink);
        });
        
        services.AddGrpcClient<User.UserClient>(options =>
        {
            options.Address = new Uri(settings.DatabaseLink);
        });

        services.AddScoped<IAuthServiceClient, AuthServiceClient>();
        services.AddScoped<IUserDatabaseClient, UserDatabaseClient>();
    }
}