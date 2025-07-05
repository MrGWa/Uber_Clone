//Added by tamar
namespace UberClone.Domain.Entities;

public class DriverLocation
{
    public int Id { get; set; }
    public Guid DriverId { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
