using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class SupportTicketService(AppDbContext context) : ISupportTicketService
{
    public async Task<List<SupportTicketDto>> GetAllSupportTicketsAsync()
    {
        return await GetAllTicketsAsync();
    }

    public async Task<SupportTicketDto?> GetSupportTicketByIdAsync(int id)
    {
        var ticket = await context.SupportTickets.FindAsync(id);
        if (ticket == null) return null;

        return new SupportTicketDto
        {
            Id = ticket.Id,
            UserId = ticket.UserId,
            Issue = ticket.Issue,
            AdminResponse = ticket.AdminResponse,
            Status = ticket.Status.ToString(),
            CreatedAt = ticket.CreatedAt,
            ResolvedAt = ticket.ResolvedAt
        };
    }

    public async Task<SupportTicketDto> CreateSupportTicketAsync(CreateSupportTicketDto dto)
    {
        var ticket = new SupportTicket
        {
            UserId = dto.UserId,
            Issue = dto.Issue
        };

        context.SupportTickets.Add(ticket);
        await context.SaveChangesAsync();

        return new SupportTicketDto
        {
            Id = ticket.Id,
            UserId = ticket.UserId,
            Issue = ticket.Issue,
            AdminResponse = ticket.AdminResponse,
            Status = ticket.Status.ToString(),
            CreatedAt = ticket.CreatedAt,
            ResolvedAt = ticket.ResolvedAt
        };
    }

    public async Task<SupportTicketDto> UpdateSupportTicketAsync(int id, UpdateSupportTicketDto dto)
    {
        var ticket = await context.SupportTickets.FindAsync(id);
        if (ticket == null) throw new Exception("Ticket not found.");

        ticket.AdminResponse = dto.AdminResponse;
        ticket.Status = Enum.Parse<TicketStatus>(dto.Status, ignoreCase: true);
        ticket.ResolvedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new SupportTicketDto
        {
            Id = ticket.Id,
            UserId = ticket.UserId,
            Issue = ticket.Issue,
            AdminResponse = ticket.AdminResponse,
            Status = ticket.Status.ToString(),
            CreatedAt = ticket.CreatedAt,
            ResolvedAt = ticket.ResolvedAt
        };
    }

    public async Task DeleteSupportTicketAsync(int id)
    {
        var ticket = await context.SupportTickets.FindAsync(id);
        if (ticket == null) throw new Exception("Ticket not found.");

        context.SupportTickets.Remove(ticket);
        await context.SaveChangesAsync();
    }

    // Keep existing methods for backwards compatibility
    public async Task<SupportTicketDto> CreateTicketAsync(CreateSupportTicketDto dto)
    {
        return await CreateSupportTicketAsync(dto);
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

    public async Task<SupportTicketDto> UpdateTicketAsync(int id, UpdateSupportTicketDto dto)
    {
        return await UpdateSupportTicketAsync(id, dto);
    }
}
