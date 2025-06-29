//Added by tamar
namespace UberClone.Application.DTOs.Admin;

public class CreateOrUpdateTariffDto
{
    public string Region { get; set; } = string.Empty;
    public decimal BaseFare { get; set; }
    public decimal PerMinute { get; set; }
    public decimal PerKilometer { get; set; }
    public float SurgeMultiplier { get; set; }
}
