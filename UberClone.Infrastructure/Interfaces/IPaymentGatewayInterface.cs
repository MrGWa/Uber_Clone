namespace UberClone.Infrastructure.Interfaces
{
    public interface IPaymentGateway
    {
        Task<bool> ProcessPayment(PaymentDetails paymentDetails);
        Task RefundPayment(int transactionId);
    }
}
