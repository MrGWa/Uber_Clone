using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.Ride;

public class StartRideUseCase : IStartRideUseCase
{
    private readonly IRideRepository _rideRepository;

    public StartRideUseCase(IRideRepository rideRepository)
    {
        _rideRepository = rideRepository;
    }

    public async Task<bool> ExecuteAsync(StartRideDto dto)
    {
        var ride = await _rideRepository.GetRideByIdAsync(dto.RideId);
        if (ride == null)
            throw new Exception("Ride not found.");

        if (ride.DriverId != dto.DriverId)
            throw new Exception("Unauthorized driver.");

        if (ride.Status != RideStatus.Accepted)
            throw new Exception("Ride must be accepted first.");

        ride.Status = RideStatus.Started;
        ride.StartedAt = DateTime.UtcNow;
        
        await _rideRepository.UpdateRideAsync(ride);
        return true;
    }
}
