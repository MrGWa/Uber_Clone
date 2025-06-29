//Added by tamar
namespace UberClone.Application.DTOs.Admin;

public class UserActivityDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;

    public int TotalRides { get; set; }
    public decimal TotalSpent { get; set; }

    public DateTime? LastRideDate { get; set; }
}
