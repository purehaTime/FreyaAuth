namespace Dashboard.Middleware.Extensions;

public static class AccessValidationExtension
{
    public static IApplicationBuilder UseAccessValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AccessValidationMiddleware>();
    }
}