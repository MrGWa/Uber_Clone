namespace UberClone.Domain.Entities;

public class Ride
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public Guid DriverId { get; set; }
    public string Status { get; set; } = RideStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? Fare { get; set; }
    public decimal Distance { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropoffLocation { get; set; }
}