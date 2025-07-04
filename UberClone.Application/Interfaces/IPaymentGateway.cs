using UberClone.Application.DTOs;

namespace UberClone.Application.Interfaces;

public interface IPaymentGateway
{
    Task<bool> ProcessPayment(PaymentDetails paymentDetails);
    Task RefundPayment(int transactionId);
}
