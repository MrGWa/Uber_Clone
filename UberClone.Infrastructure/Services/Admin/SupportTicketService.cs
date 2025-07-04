using UberClone.Application.DTOs.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class SupportTicketService(AppDbContext context)
{
    public async Task CreateTicketAsync(CreateSupportTicketDto dto)
    {
        var ticket = new SupportTicket
        {
            UserId = dto.UserId,
            Issue = dto.Issue
        };

        context.SupportTickets.Add(ticket);
        await context.SaveChangesAsync();
    }

    public async Task<List<SupportTicketDto>> GetAllTicketsAsync()
    {
        var tickets = await context.SupportTickets.ToListAsync();

        return tickets.Select(t => new SupportTicketDto
        {
            Id = t.Id,
            UserId = t.UserId,
            Issue = t.Issue,
            AdminResponse = t.AdminResponse,
            Status = t.Status.ToString(),
            CreatedAt = t.CreatedAt,
            ResolvedAt = t.ResolvedAt
        }).ToList();
    }

    public async Task UpdateTicketAsync(int ticketId, UpdateSupportTicketDto dto)
    {
        var ticket = await context.SupportTickets.FindAsync(ticketId);
        if (ticket == null) throw new Exception("Ticket not found.");

        ticket.AdminResponse = dto.AdminResponse;
        ticket.Status = Enum.Parse<TicketStatus>(dto.Status, ignoreCase: true);
        ticket.ResolvedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
