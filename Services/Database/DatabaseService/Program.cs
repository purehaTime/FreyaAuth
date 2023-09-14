using ConfigHelper;
using DatabaseService.Grpc;
using DatabaseService.Handlers;
using DatabaseService.Interfaces;
using DatabaseService.Interfaces.Handlers;
using DatabaseService.Interfaces.Repository;
using DatabaseService.Interfaces.Services;
using DatabaseService.Repository;
using DatabaseService.Services;
using GrpcHelper;
using LogsHelper;
using Models.DbModels;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<IMongoFactory, MongoFactory>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IMongoFactory, MongoFactory>();

builder.Services.AddScoped<IUserDbServerHandler, UserDbServerHandler>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Host.UseSerilogHelper();
builder.Services.AddConfigHelper();
builder.Services.AddGrpcHelper(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<UserDbServer>();

app.Run();