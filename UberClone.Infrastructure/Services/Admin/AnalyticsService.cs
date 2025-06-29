using Microsoft.EntityFrameworkCore;
using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Infrastructure.Persistence;

namespace UberClone.Infrastructure.Services.Admin;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _context;

    public AnalyticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SystemAnalyticsDto> GetSystemAnalyticsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var completedRides = await _context.Rides.CountAsync(r => r.Status == "Completed");
        var totalEarnings = await _context.Rides
            .Where(r => r.Status == "Completed")
            .SumAsync(r => r.Fare);
        var activeDrivers = await _context.Users.CountAsync(u => u.Role == "Driver");

        return new SystemAnalyticsDto
        {
            TotalUsers = totalUsers,
            CompletedRides = completedRides,
            TotalEarnings = totalEarnings,
            ActiveDrivers = activeDrivers
        };
    }
}