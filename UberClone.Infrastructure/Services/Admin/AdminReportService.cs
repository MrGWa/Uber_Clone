//added by tamar
using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Infrastructure.Persistence;
using UberClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin
{
    public class AdminReportService : IAdminReportService
    {
        private readonly AppDbContext _context;

        public AdminReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RevenueReportDto> GenerateRevenueReportAsync(ReportRequestDto dto)
        {
            if (dto.StartDate > dto.EndDate)
                throw new ArgumentException("Start date cannot be greater than end date.");

            var rides = await _context.Rides
                .Where(r => r.CreatedAt >= dto.StartDate && 
                           r.CreatedAt <= dto.EndDate && 
                           r.Status == RideStatus.Completed && 
                           r.Fare.HasValue)
                .ToListAsync();

            var totalRevenue = rides.Sum(r => r.Fare!.Value);

            return new RevenueReportDto
            {
                TotalRevenue = totalRevenue,
                TotalRides = rides.Count,
                ReportStartDate = dto.StartDate,
                ReportEndDate = dto.EndDate
            };
        }
    }
}
