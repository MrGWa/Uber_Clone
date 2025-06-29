// Added by tamar
namespace UberClone.Application.DTOs.Admin;

public class CreatePromoCodeDto
{
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
}
