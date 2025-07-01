using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.Admin;

public interface IUserActivityReportService
{
    Task<List<UserActivityDto>> GetUserActivityReportAsync();
}
