// Added by tamar

using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class UserActivityReportService : IUserActivityReportService
{
    private readonly AppDbContext _context;

    public UserActivityReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserActivityDto>> GetUserActivityReportAsync()
    {
        var userActivities = await _context.Users
            .Select(user => new UserActivityDto
            {
                UserId = user.Id,
                Email = user.Email,
                TotalRides = _context.Rides.Count(r => r.PassengerId == user.Id),
                TotalSpent = _context.Rides
                    .Where(r => r.PassengerId == user.Id && r.Fare.HasValue)
                    .Sum(r => r.Fare!.Value),
                LastRideDate = _context.Rides
                    .Where(r => r.PassengerId == user.Id)
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => r.CreatedAt)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return userActivities;
    }
}
