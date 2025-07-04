using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces.UseCases;

namespace UberClone.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RideController(
    IStartRideUseCase startRideUseCase,
    ICompleteRideUseCase completeRideUseCase,
    ICancelRideUseCase cancelRideUseCase) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartRideDto dto)
    {
        await startRideUseCase.ExecuteAsync(dto);
        return Ok("Ride started.");
    }

    [HttpPost("complete")]
    public async Task<IActionResult> Complete([FromBody] CompleteRideDto dto)
    {
        await completeRideUseCase.ExecuteAsync(dto);
        return Ok("Ride completed.");
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelRideDto dto)
    {
        await cancelRideUseCase.ExecuteAsync(dto);
        return Ok("Ride cancelled.");
    }
}
