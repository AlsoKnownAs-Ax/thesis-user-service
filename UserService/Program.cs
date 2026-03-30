using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Infrastructure;
using UserService.Infrastructure.Repositories;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserApplicationService>();

var app = builder.Build();

app.MapGrpcService<UserGrpcService>();
app.MapGet("/", () => "This is a gRPC User Service using SQLite.");

app.Run();