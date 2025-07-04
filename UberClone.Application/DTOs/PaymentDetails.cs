namespace UberClone.Application.DTOs;

public class PaymentDetails
{
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? RideId { get; set; }
}
