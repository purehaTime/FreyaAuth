using AuthService.Interfaces;
using AuthService.Services;
using ConfigHelper;
using GrpcHelper;
using LogsHelper;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;


try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddRazorPages();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddMudServices();
    
    builder.Services.AddConfigHelper();
    builder.Services.AddGrpcHelper(builder.Configuration);
    builder.Host.UseSerilogHelper();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
    builder.Services.AddScoped<IAuthSeverHandler, AuthServerHandler>();
    builder.Services.AddScoped<IPasswordService, PasswordService>();
    builder.Services.AddScoped<IEmailSender, EmailSender>();
    
    builder.Services.AddTransient<IPasswordHasher<string>, PasswordHasher<string>>();
    
    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    
    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always,
    });

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}

catch (Exception err)
{
    Console.WriteLine(err.Message);
    Console.WriteLine(err.InnerException?.ToString());
}