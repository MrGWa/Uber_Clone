﻿using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Application.Interfaces;
using UberClone.Domain.Entities;

namespace UberClone.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RideController(
    IStartRideUseCase startRideUseCase,
    ICompleteRideUseCase completeRideUseCase,
    ICancelRideUseCase cancelRideUseCase,
    IRideRepository rideRepository) : ControllerBase
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

    [HttpPost("create-test-ride")]
    public async Task<IActionResult> CreateTestRide()
    {
        var ride = new Ride
        {
            Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"), // Fixed GUID for testing
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Status = RideStatus.Pending,
            Distance = 10.5m, // 10.5 km
            PickupLocation = "Test Pickup Location",
            DropoffLocation = "Test Dropoff Location",
            CreatedAt = DateTime.UtcNow
        };

        await rideRepository.CreateRideAsync(ride);
        
        return Ok(new { 
            Message = "Test ride created successfully", 
            RideId = ride.Id,
            Distance = ride.Distance 
        });
    }

    [HttpPost("request")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> RequestRide([FromBody] RideRequestDto dto)
    {
        try
        {
            // Create a new ride request
            var ride = new Ride
            {
                Id = Guid.NewGuid(),
                PassengerId = dto.PassengerId,
                DriverId = Guid.Empty, // Will be assigned when a driver accepts
                Status = RideStatus.Pending,
                PickupLocation = dto.PickupLocation,
                DropoffLocation = dto.DropoffLocation,
                CreatedAt = DateTime.UtcNow,
                Distance = 0 // Will be calculated later
            };

            await rideRepository.CreateRideAsync(ride);
            
            return Ok(new { 
                Message = "Ride request created successfully", 
                RideId = ride.Id,
                Status = ride.Status,
                PickupLocation = ride.PickupLocation,
                DropoffLocation = ride.DropoffLocation
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
