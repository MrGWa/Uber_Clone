namespace UberClone.Application.UseCases
{
    public class ProcessPaymentUseCase
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ITransactionRepository _transactionRepository;

        public ProcessPaymentUseCase(IPaymentGateway paymentGateway, ITransactionRepository transactionRepository)
        {
            _paymentGateway = paymentGateway;
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> Execute(int rideId, decimal amount, string paymentMethod)
        {
            var transaction = new Transaction
            {
                RideId = rideId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                IsSuccessful = false,
                TransactionDate = DateTime.Now
            };

            bool paymentSuccess = await _paymentGateway.ProcessPayment(new PaymentDetails { Amount = amount, PaymentMethod = paymentMethod });
            transaction.IsSuccessful = paymentSuccess;

            await _transactionRepository.SaveAsync(transaction);

            return paymentSuccess;
        }
    }
}
