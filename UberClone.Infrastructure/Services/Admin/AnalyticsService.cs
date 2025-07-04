using Microsoft.EntityFrameworkCore;
using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;

namespace UberClone.Infrastructure.Services.Admin;

public class AnalyticsService(AppDbContext context) : IAnalyticsService
{
    private readonly AppDbContext _context = context;

    public async Task<SystemAnalyticsDto> GetSystemAnalyticsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var completedRides = await _context.Rides.CountAsync(r => r.Status == RideStatus.Completed);
        var totalEarnings = await _context.Rides
            .Where(r => r.Status == RideStatus.Completed && r.Fare.HasValue)
            .SumAsync(r => r.Fare!.Value);
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