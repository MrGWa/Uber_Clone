namespace UberClone.Application.DTOs.Ride;

public class CancelRideDto
{
    public Guid RideId { get; set; }
    public string Reason { get; set; } = string.Empty;
}