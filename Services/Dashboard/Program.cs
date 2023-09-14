using ConfigHelper;
using Dashboard.Interfaces;
using Dashboard.Middleware.Extensions;
using Dashboard.Services;
using GrpcHelper;
using LogsHelper;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();
    
    builder.Services.AddGrpcHelper(builder.Configuration);
    builder.Services.AddConfigHelper();

    builder.Host.UseSerilogHelper();

    builder.Services.AddScoped<IJwtTokenValidation, JwtTokenValidation>();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseAccessValidation();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception err)
{
    Console.WriteLine(err.Message);
    Console.WriteLine(err.InnerException);
}