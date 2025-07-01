using UberClone.Application.DTOs.Ride;

namespace UberClone.Application.Interfaces.Services;

public interface IRideLifecycleService
{
    Task StartRideAsync(StartRideDto dto);
    Task CompleteRideAsync(CompleteRideDto dto);
    Task CancelRideAsync(CancelRideDto dto);
}