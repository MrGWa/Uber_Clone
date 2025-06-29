//added by tamar
namespace UberClone.Domain.Entities;

public class Ride
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Cost { get; set; }
    public Guid PassengerId { get; set; }
    
    public decimal Fare { get; set; }

    public Guid DriverId { get; set; }
    
    public string Status { get; set; } = "Pending";


}
