using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.Admin;

public interface IAnalyticsService
{
    Task<SystemAnalyticsDto> GetSystemAnalyticsAsync();
}