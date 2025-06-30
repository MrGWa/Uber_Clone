using Microsoft.EntityFrameworkCore;
using UberClone.Application.Interfaces;
using UberClone.Application.UseCases.User;
using UberClone.Infrastructure.Persistence;
using UberClone.Infrastructure.Repositories;
using UberClone.Infrastructure.Services.Admin; // Added by Tamar
using UberClone.Application.Interfaces.Admin;
using UberClone.Infrastructure.Services.Admin;
using UberClone.Infrastructure.Services;
using UberClone.Application.UseCases;
using UberClone.Application.Interfaces;



var builder = WebApplication.CreateBuilder(args);


// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

builder.Services.AddScoped<AdminReportService>(); // Added by Tamar
builder.Services.AddScoped<SupportTicketService>(); //Added by Tamar
builder.Services.AddScoped<TariffService>(); //Added by Tamar
builder.Services.AddScoped<AuditLogService>(); //Added by Tamar
builder.Services.AddScoped<PromoCodeService>(); // Added by Tamar
builder.Services.AddScoped<DriverLocationService>(); // Added by Tamar
builder.Services.AddScoped<UserActivityReportService>(); // Added by Tamar



builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();



builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IRideService, RideService>();


builder.Services.AddScoped<RegisterUserCommand>();



builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();