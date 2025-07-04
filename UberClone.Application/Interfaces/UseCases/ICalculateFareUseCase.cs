namespace UberClone.Application.Interfaces.UseCases;

public interface ICalculateFareUseCase
{
    Task<decimal> ExecuteAsync(Guid rideId);
}
