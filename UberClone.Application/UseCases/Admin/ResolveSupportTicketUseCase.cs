using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.Admin;

public class ResolveSupportTicketUseCase(
    IRepository<SupportTicket> supportTicketRepository,
    IRepository<AuditLog> auditLogRepository) : IResolveSupportTicketUseCase
{
    public async Task<bool> ExecuteAsync(int ticketId, UpdateSupportTicketDto dto, string adminUserId)
    {
        var ticket = await supportTicketRepository.GetByIdAsync(ticketId);
        if (ticket == null)
            throw new Exception("Support ticket not found.");

        if (ticket.Status == TicketStatus.Resolved)
            throw new Exception("Ticket is already resolved.");

        // Business logic: Update ticket status and add response
        if (Enum.TryParse<TicketStatus>(dto.Status, out var newStatus))
        {
            ticket.Status = newStatus;
        }
        else
        {
            throw new Exception($"Invalid status: {dto.Status}");
        }

        ticket.AdminResponse = dto.AdminResponse;
        ticket.ResolvedAt = ticket.Status == TicketStatus.Resolved ? DateTime.UtcNow : null;
        ticket.UpdatedAt = DateTime.UtcNow;

        await supportTicketRepository.UpdateAsync(ticket);

        // Create audit log
        var auditLog = new AuditLog
        {
            ActionType = "Resolve Support Ticket",
            TargetEntity = $"SupportTicket:{ticketId}",
            PerformedBy = adminUserId,
            Details = $"Status changed to '{dto.Status}' with response: {dto.AdminResponse}",
            Timestamp = DateTime.UtcNow
        };

        await auditLogRepository.AddAsync(auditLog);
        return true;
    }
}
