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
using UberClone.Infrastructure.Gateway;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRideRepository, RideRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Register gateway services
builder.Services.AddScoped<IPaymentGateway, PaymentGatewayImplementation>();

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
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IDriverLocationService, DriverLocationService>();

// Register UseCases
builder.Services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();
builder.Services.AddScoped<ICalculateFareUseCase, CalculateFareUseCase>();
builder.Services.AddScoped<IProcessPaymentUseCase, ProcessPaymentUseCase>();
builder.Services.AddScoped<IStartRideUseCase, StartRideUseCase>();
builder.Services.AddScoped<ICompleteRideUseCase, CompleteRideUseCase>();
builder.Services.AddScoped<ICancelRideUseCase, CancelRideUseCase>();
builder.Services.AddScoped<ICreateSupportTicketUseCase, CreateSupportTicketUseCase>();
builder.Services.AddScoped<IResolveSupportTicketUseCase, ResolveSupportTicketUseCase>();
builder.Services.AddScoped<IManageTariffUseCase, ManageTariffUseCase>();
builder.Services.AddScoped<IManagePromoCodeUseCase, ManagePromoCodeUseCase>();

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keep original property names
    options.JsonSerializerOptions.WriteIndented = true; // Pretty print JSON
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Case insensitive
});

// Add explicit JSON formatting support
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseRouting();

// Add middleware to handle requests properly
app.Use(async (context, next) =>
{
    // Log request details for debugging
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    Console.WriteLine($"Content-Type: {context.Request.ContentType}");
    await next();
});

app.MapControllers();
app.Run();