//Added by Tamar
namespace UberClone.Application.DTOs.Admin;

public class CreateSupportTicketDto
{
    public Guid UserId { get; set; }
    public string Issue { get; set; } = string.Empty;
}
