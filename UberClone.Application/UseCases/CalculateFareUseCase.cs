using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases
{
    public class CalculateFareUseCase : ICalculateFareUseCase
    {
        private readonly IRideRepository _rideRepository;

        public CalculateFareUseCase(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        public async Task<decimal> ExecuteAsync(Guid rideId)
        {
            var ride = await _rideRepository.GetRideByIdAsync(rideId);
            if (ride == null) 
                throw new Exception("Ride not found.");

            // Calculate fare based on distance and base rate
            decimal baseFare = 5.0m; // Base fare
            decimal ratePerKm = 1.5m; // Rate per kilometer
            
            decimal calculatedFare = baseFare + (ride.Distance * ratePerKm);
            
            // Update ride with calculated fare
            ride.Fare = calculatedFare;
            await _rideRepository.UpdateRideAsync(ride);
            
            return calculatedFare;
        }
    }
}
