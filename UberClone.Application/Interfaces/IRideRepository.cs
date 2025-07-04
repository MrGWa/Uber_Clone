using UberClone.Domain.Entities;

namespace UberClone.Application.Interfaces;

public interface IRideRepository
{
    Task<Ride?> GetRideByIdAsync(Guid rideId);
    Task<List<Ride>> GetAllRidesAsync();
    Task<Ride> CreateRideAsync(Ride ride);
    Task UpdateRideAsync(Ride ride);
    Task DeleteRideAsync(Guid rideId);
}
