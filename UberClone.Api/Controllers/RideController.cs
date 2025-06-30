using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces;
using UberClone.Domain.Entities;

namespace UberClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RideController : ControllerBase
{
    private readonly IRideService _rideService;

    public RideController(IRideService rideService)
    {
        _rideService = rideService;
    }

    [HttpPost("request-ride")]
    public ActionResult<Ride> RequestRide([FromBody] RideRequestDto dto)
    {
        var ride = _rideService.RequestRide(dto);
        return Ok(ride);
    }

    [HttpPost("accept-ride")]
    public IActionResult AcceptRide([FromBody] RideAcceptedDto dto)
    {
        _rideService.AcceptRide(dto);
        return Ok(new { message = "Ride accepted" });
    }

    [HttpPost("complete-ride")]
    public ActionResult<Ride> CompleteRide([FromBody] RideCompletedDto dto)
    {
        var ride = _rideService.CompleteRide(dto);
        return Ok(ride);
    }
}
