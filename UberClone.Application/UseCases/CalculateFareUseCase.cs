namespace UberClone.Application.UseCases
{
    public class CalculateFareUseCase
    {
        private readonly IRideRepository _rideRepository;

        public CalculateFareUseCase(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        public decimal Execute(int rideId)
        {
            var ride = _rideRepository.GetRideById(rideId);
            if (ride == null) throw new Exception("Ride not found.");

            return ride.Distance * 1.5m; // Example: 1.5 is the fare per km
        }
    }
}
