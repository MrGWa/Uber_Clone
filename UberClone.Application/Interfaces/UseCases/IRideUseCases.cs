using UberClone.Application.DTOs.Ride;

namespace UberClone.Application.Interfaces.UseCases;

public interface IStartRideUseCase
{
    Task<bool> ExecuteAsync(StartRideDto dto);
}

public interface ICompleteRideUseCase
{
    Task<decimal> ExecuteAsync(CompleteRideDto dto);
}

public interface ICancelRideUseCase
{
    Task<bool> ExecuteAsync(CancelRideDto dto);
}
