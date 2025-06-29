//Added by tamar
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;

namespace UberClone.Infrastructure.Services.Admin;

public class AuditLogService
{
    private readonly AppDbContext _context;

    public AuditLogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string actionType, string targetEntity, string? details = null)
    {
        var log = new AuditLog
        {
            ActionType = actionType,
            TargetEntity = targetEntity,
            Details = details
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
