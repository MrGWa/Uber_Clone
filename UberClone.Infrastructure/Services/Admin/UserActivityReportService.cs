// Added by tamar

using UberClone.Application.DTOs.Admin;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class UserActivityReportService
{
    private readonly AppDbContext _context;

    public UserActivityReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserActivityDto>> GetUserActivityReportAsync()
    {
        var users = await _context.Users.ToListAsync();

        var ridesGrouped = await _context.Rides
            .GroupBy(r => r.PassengerId)
            .Select(g => new
            {
                PassengerId = g.Key,
                TotalRides = g.Count(),
                TotalSpent = g.Sum(r => r.Cost),
                LastRideDate = g.Max(r => r.CreatedAt)
            })
            .ToListAsync();

        var report = new List<UserActivityDto>();

        foreach (var user in users)
        {
            var rideData = ridesGrouped.FirstOrDefault(r => r.PassengerId == user.Id);

            report.Add(new UserActivityDto
            {
                UserId = user.Id,
                Email = user.Email,
                TotalRides = rideData?.TotalRides ?? 0,
                TotalSpent = rideData?.TotalSpent ?? 0,
                LastRideDate = rideData?.LastRideDate
            });
        }

        return report;
    }
}
