using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.Admin;

public interface IAdminReportService
{
    Task<RevenueReportDto> GenerateRevenueReportAsync(ReportRequestDto dto);
}
