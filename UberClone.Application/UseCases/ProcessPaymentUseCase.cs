using UberClone.Application.DTOs;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases
{
    public class ProcessPaymentUseCase : IProcessPaymentUseCase
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ITransactionRepository _transactionRepository;

        public ProcessPaymentUseCase(IPaymentGateway paymentGateway, ITransactionRepository transactionRepository)
        {
            _paymentGateway = paymentGateway;
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> ExecuteAsync(Guid rideId, decimal amount, string paymentMethod)
        {
            var transaction = new Transaction
            {
                RideId = rideId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                IsSuccessful = false,
                TransactionDate = DateTime.UtcNow
            };

            var paymentDetails = new PaymentDetails 
            { 
                Amount = amount, 
                PaymentMethod = paymentMethod,
                RideId = rideId,
                Description = $"Payment for ride {rideId}"
            };

            bool paymentSuccess = await _paymentGateway.ProcessPayment(paymentDetails);
            transaction.IsSuccessful = paymentSuccess;

            await _transactionRepository.SaveAsync(transaction);

            return paymentSuccess;
        }
    }
}
