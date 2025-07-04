using Microsoft.EntityFrameworkCore;
using UberClone.Application.Interfaces;
using UberClone.Application.UseCases.User;
using UberClone.Infrastructure.Persistence;
using UberClone.Infrastructure.Repositories;
using UberClone.Infrastructure.Services.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Infrastructure.Services;
using UberClone.Application.UseCases;
using UberClone.Application.Interfaces.Services;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Application.UseCases.Admin;
using UberClone.Application.UseCases.Ride;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register legacy services (kept for compatibility)
builder.Services.AddScoped<AdminReportService>();
builder.Services.AddScoped<SupportTicketService>();
builder.Services.AddScoped<TariffService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<PromoCodeService>();
builder.Services.AddScoped<DriverLocationService>();
builder.Services.AddScoped<UserActivityReportService>();
builder.Services.AddScoped<IRideLifecycleService, RideLifecycleService>();

// Register service interfaces
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IAdminReportService, AdminReportService>();
builder.Services.AddScoped<IUserActivityReportService, UserActivityReportService>();

// Register UseCases
builder.Services.AddScoped<RegisterUserCommand>();
builder.Services.AddScoped<ICalculateFareUseCase, CalculateFareUseCase>();
builder.Services.AddScoped<IProcessPaymentUseCase, ProcessPaymentUseCase>();
builder.Services.AddScoped<IStartRideUseCase, StartRideUseCase>();
builder.Services.AddScoped<ICompleteRideUseCase, CompleteRideUseCase>();
builder.Services.AddScoped<ICancelRideUseCase, CancelRideUseCase>();
builder.Services.AddScoped<ICreateSupportTicketUseCase, CreateSupportTicketUseCase>();
builder.Services.AddScoped<IResolveSupportTicketUseCase, ResolveSupportTicketUseCase>();
builder.Services.AddScoped<IManageTariffUseCase, ManageTariffUseCase>();
builder.Services.AddScoped<IManagePromoCodeUseCase, ManagePromoCodeUseCase>();

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();