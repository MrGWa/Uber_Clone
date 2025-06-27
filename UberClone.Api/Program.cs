using Microsoft.EntityFrameworkCore;
using UberClone.Application.Interfaces;
using UberClone.Application.UseCases.User;
using UberClone.Infrastructure.Persistence;
using UberClone.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<RegisterUserCommand>();

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();