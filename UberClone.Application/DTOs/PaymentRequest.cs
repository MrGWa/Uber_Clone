using System.ComponentModel.DataAnnotations;

namespace UberClone.Application.DTOs;

public class PaymentRequest
{
    [Required]
    public Guid RideId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = default!;
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
}
