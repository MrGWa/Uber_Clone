//Added by tamar
namespace UberClone.Domain.Entities;

public class SupportTicket
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public string Issue { get; set; } = string.Empty;
    public string? AdminResponse { get; set; }

    public TicketStatus Status { get; set; } = TicketStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
}

public enum TicketStatus
{
    Open,
    InProgress,
    Resolved,
    Rejected
}
