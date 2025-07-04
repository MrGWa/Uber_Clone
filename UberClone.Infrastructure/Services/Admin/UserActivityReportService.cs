// Added by tamar

using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class UserActivityReportService(AppDbContext context) : IUserActivityReportService
{
    private readonly AppDbContext _context = context;

    public async Task<List<UserActivityDto>> GetUserActivityReportAsync()
    {
        // Get all users
        var users = await _context.Users.ToListAsync();

        // Get ride statistics grouped by passenger
        var rideStats = await _context.Rides
            .GroupBy(r => r.PassengerId)
            .Select(g => new
            {
                PassengerId = g.Key,
                TotalRides = g.Count(),
                TotalSpent = g.Where(r => r.Fare.HasValue).Sum(r => r.Fare!.Value),
                LastRideDate = g.Max(r => r.CreatedAt)
            })
            .ToListAsync();

        // Combine the data
        var userActivities = users.Select(user =>
        {
            var stats = rideStats.FirstOrDefault(r => r.PassengerId == user.Id);
            return new UserActivityDto
            {
                UserId = user.Id,
                Email = user.Email,
                TotalRides = stats?.TotalRides ?? 0,
                TotalSpent = stats?.TotalSpent ?? 0,
                LastRideDate = stats?.LastRideDate
            };
        }).ToList();

        return userActivities;
    }
}
