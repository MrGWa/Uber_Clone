//Added by tamar
using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs.Admin;
using UberClone.Infrastructure.Services.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace UberClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly AdminReportService _adminReportService;
    private readonly SupportTicketService _supportTicketService;
    private readonly TariffService _tariffService;
    private readonly AuditLogService _auditLogService;
    private readonly PromoCodeService _promoCodeService;
    private readonly DriverLocationService _driverLocationService;
    private readonly UserActivityReportService _userActivityReportService;



    public AdminController(
        IAnalyticsService analyticsService,
        AdminReportService adminReportService,
        SupportTicketService supportTicketService,
        TariffService tariffService,
        AuditLogService auditLogService,
        PromoCodeService promoCodeService,
        DriverLocationService driverLocationService,
        UserActivityReportService userActivityReportService)
    {
        _analyticsService = analyticsService;
        _adminReportService = adminReportService;
        _supportTicketService = supportTicketService;
        _tariffService = tariffService;
        _auditLogService = auditLogService;
        _promoCodeService = promoCodeService;
        _driverLocationService = driverLocationService;
        _userActivityReportService = userActivityReportService;
    }


    [HttpGet("analytics")]
    public async Task<IActionResult> GetSystemAnalytics()
    {
        try
        {
            var data = await _analyticsService.GetSystemAnalyticsAsync();
            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    
    
    [HttpPost("reports/revenue")]
    public async Task<IActionResult> GetRevenueReport([FromBody] ReportRequestDto dto)
    {
        try
        {
            var report = await _adminReportService.GenerateRevenueReportAsync(dto);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("tickets")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateSupportTicketDto dto)
    {
        try
        {
            await _supportTicketService.CreateTicketAsync(dto);
            return Ok("Support ticket created.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("tickets")]
    public async Task<IActionResult> GetAllTickets()
    {
        var tickets = await _supportTicketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpPut("tickets/{ticketId}")]
    public async Task<IActionResult> UpdateTicket(int ticketId, [FromBody] UpdateSupportTicketDto dto)
    {
        try
        {
            await _supportTicketService.UpdateTicketAsync(ticketId, dto);

            await _auditLogService.LogAsync(
                actionType: "Resolve Ticket",
                targetEntity: $"SupportTicket:{ticketId}",
                details: $"Status changed to '{dto.Status}' with response: {dto.AdminResponse}"
            );

            return Ok("Support ticket updated.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("tariffs")]
    public async Task<IActionResult> CreateOrUpdateTariff([FromBody] CreateOrUpdateTariffDto dto)
    {
        try
        {
            var result = await _tariffService.CreateOrUpdateAsync(dto);

            await _auditLogService.LogAsync(
                actionType: "Update Tariff",
                targetEntity: $"Tariff:{dto.Region}",
                details: $"Base: {dto.BaseFare}, Min: {dto.PerMinute}, Km: {dto.PerKilometer}, Surge: {dto.SurgeMultiplier}"
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("tariffs")]
    public async Task<IActionResult> GetAllTariffs()
    {
        var tariffs = await _tariffService.GetAllTariffsAsync();
        return Ok(tariffs);
    }

    [HttpGet("audit-logs")]
    public async Task<IActionResult> GetAuditLogs([FromServices] AppDbContext context)
    {
        var logs = await context.AuditLogs
        .OrderByDescending(log => log.Timestamp)
        .ToListAsync();

        return Ok(logs);
    }

    [HttpPost("promocodes")]
    public async Task<IActionResult> CreatePromoCode([FromBody] CreatePromoCodeDto dto)
    {
        try
        {
            var result = await _promoCodeService.CreatePromoCodeAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("promocodes")]
    public async Task<IActionResult> GetAllPromoCodes()
    {
        var codes = await _promoCodeService.GetAllPromoCodesAsync();
        return Ok(codes);
    }

    [HttpPut("promocodes/{id}/deactivate")]
    public async Task<IActionResult> DeactivatePromoCode(int id)
    {
        try
        {
            await _promoCodeService.DeactivatePromoCodeAsync(id);
            return Ok("Promo code deactivated.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("health")]
    public IActionResult GetSystemHealth([FromServices] AppDbContext context)
    {
        try
        {
            var canConnect = context.Database.CanConnect();
            var status = canConnect ? "Healthy" : "Database Unreachable";

            return Ok(new
            {
                status,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Unhealthy",
                message = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
    [HttpPost("drivers/location")]
    public async Task<IActionResult> UpdateDriverLocation([FromBody] UpdateDriverLocationDto dto)
    {
        try
        {
            await _driverLocationService.UpdateLocationAsync(dto);

            await _auditLogService.LogAsync(
                actionType: "Update Driver Location",
                targetEntity: $"Driver:{dto.DriverId}",
                details: $"Lat: {dto.Latitude}, Lng: {dto.Longitude}"
            );

            return Ok("Driver location updated.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("drivers/location")]
    public async Task<IActionResult> GetAllDriverLocations()
    {
        var locations = await _driverLocationService.GetAllLocationsAsync();
        return Ok(locations);
    }

    [HttpGet("reports/user-activity")]
    public async Task<IActionResult> GetUserActivityReport()
    {
        var report = await _userActivityReportService.GetUserActivityReportAsync();
        return Ok(report);
    }



}
