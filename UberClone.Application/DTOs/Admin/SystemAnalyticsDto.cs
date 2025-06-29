namespace UberClone.Application.DTOs.Admin;

public class SystemAnalyticsDto
{
    public int TotalUsers { get; set; }
    public int CompletedRides { get; set; }
    public decimal TotalEarnings { get; set; }
    public int ActiveDrivers { get; set; }
}