namespace UberClone.Application.DTOs;

public class PaymentRequest
{
    public Guid RideId { get; set; }
    public string PaymentMethod { get; set; } = default!;
    public decimal Amount { get; set; }
}
