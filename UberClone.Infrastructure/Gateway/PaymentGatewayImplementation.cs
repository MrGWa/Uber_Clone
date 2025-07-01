namespace UberClone.Infrastructure.Gateways
{
    public class PaymentGateway : IPaymentGateway
    {
        public async Task<bool> ProcessPayment(PaymentDetails paymentDetails)
        {
            // Mock implementation - Integrate with real payment provider like Stripe/PayPal
            return true; // Assume payment is successful for now
        }

        public async Task RefundPayment(int transactionId)
        {
            // Logic for processing refunds
        }
    }
}
