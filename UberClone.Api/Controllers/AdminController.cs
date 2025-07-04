//Added by tamar
using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Application.Interfaces.UseCases;

namespace UberClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(
    IAnalyticsService analyticsService,
    IAdminReportService adminReportService,
    ISupportTicketService supportTicketService,
    IManageTariffUseCase manageTariffUseCase,
    IAuditLogService auditLogService,
    IManagePromoCodeUseCase managePromoCodeUseCase,
    IDriverLocationService driverLocationService,
    IUserActivityReportService userActivityReportService,
    ICreateSupportTicketUseCase createSupportTicketUseCase,
    IResolveSupportTicketUseCase resolveSupportTicketUseCase) : ControllerBase
{


    [HttpGet("analytics")]
    public async Task<IActionResult> GetSystemAnalytics()
    {
        try
        {
            var data = await analyticsService.GetSystemAnalyticsAsync();
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
            var report = await adminReportService.GenerateRevenueReportAsync(dto);
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
            await createSupportTicketUseCase.ExecuteAsync(dto);
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
        var tickets = await supportTicketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpPut("tickets/{ticketId}")]
    public async Task<IActionResult> UpdateTicket(int ticketId, [FromBody] UpdateSupportTicketDto dto)
    {
        try
        {
            await resolveSupportTicketUseCase.ExecuteAsync(ticketId, dto, "admin"); // TODO: Get actual admin user ID

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
            var result = await manageTariffUseCase.CreateOrUpdateAsync(dto, "admin"); // TODO: Get actual admin user ID

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
        var tariffs = await manageTariffUseCase.GetAllTariffsAsync();
        return Ok(tariffs);
    }

    [HttpGet("audit-logs")]
    public async Task<IActionResult> GetAuditLogs()
    {
        var logs = await auditLogService.GetAllAuditLogsAsync();
        return Ok(logs);
    }

    [HttpPost("promocodes")]
    public async Task<IActionResult> CreatePromoCode([FromBody] CreatePromoCodeDto dto)
    {
        try
        {
            var result = await managePromoCodeUseCase.CreatePromoCodeAsync(dto, "admin"); // TODO: Get actual admin user ID
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
        var codes = await managePromoCodeUseCase.GetAllPromoCodesAsync();
        return Ok(codes);
    }

    [HttpPut("promocodes/{id}/deactivate")]
    public async Task<IActionResult> DeactivatePromoCode(int id)
    {
        try
        {
            await managePromoCodeUseCase.DeactivatePromoCodeAsync(id, "admin"); // TODO: Get actual admin user ID
            return Ok("Promo code deactivated.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("health")]
    public IActionResult GetSystemHealth()
    {
        // For now, return a simple health check
        // In a real application, you might want to create a health check service
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow
        });
    }
    [HttpPost("drivers/location")]
    public async Task<IActionResult> UpdateDriverLocation([FromBody] UpdateDriverLocationDto dto)
    {
        try
        {
            await driverLocationService.UpdateLocationAsync(dto);

            await auditLogService.LogAsync(
                "Update Driver Location",
                dto.DriverId.ToString(),
                $"Lat: {dto.Latitude}, Lng: {dto.Longitude}"
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
        var locations = await driverLocationService.GetAllLocationsAsync();
        return Ok(locations);
    }

    [HttpGet("reports/user-activity")]
    public async Task<IActionResult> GetUserActivityReport()
    {
        var report = await userActivityReportService.GetUserActivityReportAsync();
        return Ok(report);
    }



}
