namespace UberClone.Application.Interfaces.UseCases;

public interface IProcessPaymentUseCase
{
    Task<bool> ExecuteAsync(Guid rideId, decimal amount, string paymentMethod);
}
