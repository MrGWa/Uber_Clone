//Added by Tamar
namespace UberClone.Application.DTOs.Admin;

public class UpdateSupportTicketDto
{
    public string AdminResponse { get; set; } = string.Empty;
    public string Status { get; set; } = "Resolved"; // e.g., "Resolved", "Rejected"
}
