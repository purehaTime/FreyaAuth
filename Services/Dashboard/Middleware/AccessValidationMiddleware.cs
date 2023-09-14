using ConfigHelper.Configs;
using Dashboard.Interfaces;
using GrpcHelper.Clients.Interfaces;
using Microsoft.Extensions.Options;

namespace Dashboard.Middleware;

public class AccessValidationMiddleware
{
    private readonly RequestDelegate _next;

    public AccessValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtTokenValidation jwtValidation, IOptions<Service> services, IAuthServiceClient authClient)
    {
        bool shouldRedirect = true;
        if (context.Request.Cookies.TryGetValue("_userId", out var authToken))
        {
            var valid = await jwtValidation.Validate(authToken);
            if (!valid)
            {
                if (context.Request.Cookies.TryGetValue("_userValidation", out var refreshToken))
                {
                    var result = await authClient.ValidateUser(refreshToken);
                    if (result.Error == null)
                    {
                        context.Response.Cookies.Delete("_userId");
                        context.Response.Cookies.Delete("_userValidation");
                        context.Response.Cookies.Append("_userId", result.AuthToken);
                        context.Response.Cookies.Append("_userValidation", result.RefreshToken);

                        shouldRedirect = false;
                    }
                }
            }
            else
            {
                shouldRedirect = false;
            }
        }

        if (shouldRedirect)
        {
            context.Response.Redirect(services.Value.AuthPageLink);
            return;
        }

        await _next(context);
    }
}