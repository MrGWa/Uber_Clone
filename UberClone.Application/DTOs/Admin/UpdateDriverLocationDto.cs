//Added by tamar
namespace UberClone.Application.DTOs.Admin;

public class UpdateDriverLocationDto
{
    public Guid DriverId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
