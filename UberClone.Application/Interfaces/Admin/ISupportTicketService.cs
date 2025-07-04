using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.Admin;

public interface ISupportTicketService
{
    Task<List<SupportTicketDto>> GetAllSupportTicketsAsync();
    Task<List<SupportTicketDto>> GetAllTicketsAsync();
    Task<SupportTicketDto?> GetSupportTicketByIdAsync(int id);
    Task<SupportTicketDto> CreateSupportTicketAsync(CreateSupportTicketDto dto);
    Task<SupportTicketDto> CreateTicketAsync(CreateSupportTicketDto dto);
    Task<SupportTicketDto> UpdateSupportTicketAsync(int id, UpdateSupportTicketDto dto);
    Task<SupportTicketDto> UpdateTicketAsync(int id, UpdateSupportTicketDto dto);
    Task DeleteSupportTicketAsync(int id);
}
