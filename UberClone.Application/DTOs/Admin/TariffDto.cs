//Added by Tamar
namespace UberClone.Application.DTOs.Admin;

public class TariffDto
{
    public int Id { get; set; }
    public string Region { get; set; } = string.Empty;
    public decimal BaseFare { get; set; }
    public decimal PerMinute { get; set; }
    public decimal PerKilometer { get; set; }
    public float SurgeMultiplier { get; set; }
    public DateTime UpdatedAt { get; set; }
}
