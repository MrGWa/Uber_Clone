//Added by tamar
namespace UberClone.Domain.Entities;

public class Tariff
{
    public int Id { get; set; }
    public string Region { get; set; } = string.Empty;

    public decimal BaseFare { get; set; }
    public decimal PerMinute { get; set; }
    public decimal PerKilometer { get; set; }
    public float SurgeMultiplier { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
