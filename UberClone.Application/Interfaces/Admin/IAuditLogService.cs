using UberClone.Domain.Entities;

namespace UberClone.Application.Interfaces.Admin;

public interface IAuditLogService
{
    Task<List<AuditLog>> GetAllAuditLogsAsync();
    Task<AuditLog?> GetAuditLogByIdAsync(int id);
    Task<AuditLog> CreateAuditLogAsync(AuditLog auditLog);
    Task LogActionAsync(string action, string userId, string? details = null);
    Task LogAsync(string action, string userId, string? details = null);
}
