//Added by tamar
namespace UberClone.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = "Admin"; // hardcoded for now
    public string? TargetEntity { get; set; }
    public string? Details { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
