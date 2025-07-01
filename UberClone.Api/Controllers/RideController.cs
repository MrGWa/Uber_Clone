using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces;
using UberClone.Domain.Entities;
using UberClone.Application.Interfaces.Services;

namespace UberClone.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RideController : ControllerBase
{
    private readonly IRideLifecycleService _rideLifecycleService;

    public RideController(IRideLifecycleService rideLifecycleService)
    {
        _rideLifecycleService = rideLifecycleService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartRideDto dto)
    {
        await _rideLifecycleService.StartRideAsync(dto);
        return Ok("Ride started.");
    }

    [HttpPost("complete")]
    public async Task<IActionResult> Complete([FromBody] CompleteRideDto dto)
    {
        await _rideLifecycleService.CompleteRideAsync(dto);
        return Ok("Ride completed.");
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelRideDto dto)
    {
        await _rideLifecycleService.CancelRideAsync(dto);
        return Ok("Ride cancelled.");
    }
}
