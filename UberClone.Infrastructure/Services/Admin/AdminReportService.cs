//added by tamar
using UberClone.Application.DTOs.Admin;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin
{
    public class AdminReportService
    {
        private readonly AppDbContext _context;

        public AdminReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RevenueReportDto> GenerateRevenueReportAsync(ReportRequestDto dto)
        {
            var rides = await _context.Rides
                .Where(r => r.CreatedAt >= dto.StartDate && r.CreatedAt <= dto.EndDate)
                .ToListAsync();

            var totalRevenue = rides.Sum(r => r.Cost);

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
