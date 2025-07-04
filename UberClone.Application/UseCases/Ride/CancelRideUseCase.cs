using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.Ride;

public class CancelRideUseCase : ICancelRideUseCase
{
    private readonly IRideRepository _rideRepository;

    public CancelRideUseCase(IRideRepository rideRepository)
    {
        _rideRepository = rideRepository;
    }

    public async Task<bool> ExecuteAsync(CancelRideDto dto)
    {
        var ride = await _rideRepository.GetRideByIdAsync(dto.RideId);
        if (ride == null)
            throw new Exception("Ride not found.");

        if (ride.Status == RideStatus.Completed)
            throw new Exception("Cannot cancel a completed ride.");

        if (ride.Status == RideStatus.Cancelled)
            throw new Exception("Ride is already cancelled.");

        ride.Status = RideStatus.Cancelled;
        ride.CancelledAt = DateTime.UtcNow;
        ride.CancellationReason = dto.Reason;
        
        await _rideRepository.UpdateRideAsync(ride);
        return true;
    }
}
