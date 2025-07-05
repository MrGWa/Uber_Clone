//Added by tamar
using UberClone.Application.Interfaces.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class AuditLogService : IAuditLogService
{
    private readonly AppDbContext _context;

    public AuditLogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuditLog>> GetAllAuditLogsAsync()
    {
        return await _context.AuditLogs.ToListAsync();
    }

    public async Task<AuditLog?> GetAuditLogByIdAsync(int id)
    {
        return await _context.AuditLogs.FindAsync(id);
    }

    public async Task<AuditLog> CreateAuditLogAsync(AuditLog auditLog)
    {
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
        return auditLog;
    }

    public async Task LogActionAsync(string action, string userId, string? details = null)
    {
        await LogAsync(action, userId, details);
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
