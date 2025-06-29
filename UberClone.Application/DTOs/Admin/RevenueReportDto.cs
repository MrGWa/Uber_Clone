//Added by Tamar
namespace UberClone.Application.DTOs.Admin;

public class RevenueReportDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalRides { get; set; }
    public DateTime ReportStartDate { get; set; }
    public DateTime ReportEndDate { get; set; }
}
