using UberClone.Application.UseCases;
using UberClone.Infrastructure.Gateways;

namespace UberClone.Api.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly CalculateFareUseCase _calculateFareUseCase;
        private readonly ProcessPaymentUseCase _processPaymentUseCase;

        public PaymentController(CalculateFareUseCase calculateFareUseCase, ProcessPaymentUseCase processPaymentUseCase)
        {
            _calculateFareUseCase = calculateFareUseCase;
            _processPaymentUseCase = processPaymentUseCase;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                // Calculate fare
                var fare = _calculateFareUseCase.Execute(paymentRequest.RideId);

                // Process payment
                bool paymentSuccess = await _processPaymentUseCase.Execute(paymentRequest.RideId, fare, paymentRequest.PaymentMethod);

                if (!paymentSuccess)
                    return BadRequest("Payment failed.");

                return Ok("Payment successful.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
