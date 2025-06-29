//Added by Tamar
using System;

namespace UberClone.Application.DTOs.Admin;

public class SupportTicketDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Issue { get; set; } = string.Empty;
    public string? AdminResponse { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
