using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.Admin;

public class CreateSupportTicketUseCase : ICreateSupportTicketUseCase
{
    private readonly IRepository<SupportTicket> _supportTicketRepository;
    private readonly IUserRepository _userRepository;

    public CreateSupportTicketUseCase(
        IRepository<SupportTicket> supportTicketRepository,
        IUserRepository userRepository)
    {
        _supportTicketRepository = supportTicketRepository;
        _userRepository = userRepository;
    }

    public async Task<int> ExecuteAsync(CreateSupportTicketDto dto)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
            throw new Exception("User not found.");

        // Business logic: Create support ticket
        var ticket = new SupportTicket
        {
            UserId = dto.UserId,
            Issue = dto.Issue,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        await _supportTicketRepository.AddAsync(ticket);
        return ticket.Id;
    }
}
