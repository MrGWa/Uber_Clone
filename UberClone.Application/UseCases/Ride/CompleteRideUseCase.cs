using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.Ride;

public class CompleteRideUseCase : ICompleteRideUseCase
{
    private readonly IRideRepository _rideRepository;

    public CompleteRideUseCase(IRideRepository rideRepository)
    {
        _rideRepository = rideRepository;
    }

    public async Task<decimal> ExecuteAsync(CompleteRideDto dto)
    {
        var ride = await _rideRepository.GetRideByIdAsync(dto.RideId);
        if (ride == null)
            throw new Exception("Ride not found.");

        if (ride.Status != RideStatus.Started)
            throw new Exception("Ride must be started first.");

        ride.Status = RideStatus.Completed;
        ride.CompletedAt = DateTime.UtcNow;
        
        // Calculate fare based on time and distance
        decimal fare = CalculateFare(ride);
        ride.Fare = fare;
        
        await _rideRepository.UpdateRideAsync(ride);
        return fare;
    }

    private decimal CalculateFare(Domain.Entities.Ride ride)
    {
        // Business logic for fare calculation
        decimal baseFare = 5.0m;
        decimal ratePerMinute = 0.5m;
        decimal ratePerKm = 1.5m;

        if (ride.StartedAt.HasValue && ride.CompletedAt.HasValue)
        {
            var duration = (ride.CompletedAt.Value - ride.StartedAt.Value).TotalMinutes;
            var timeFare = (decimal)duration * ratePerMinute;
            return baseFare + timeFare + (ride.Distance * ratePerKm);
        }

        return baseFare;
    }
}
