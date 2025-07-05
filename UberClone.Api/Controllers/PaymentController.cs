using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces.UseCases;

namespace UberClone.Api.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly ICalculateFareUseCase _calculateFareUseCase;
        private readonly IProcessPaymentUseCase _processPaymentUseCase;

        public PaymentController(ICalculateFareUseCase calculateFareUseCase, IProcessPaymentUseCase processPaymentUseCase)
        {
            _calculateFareUseCase = calculateFareUseCase;
            _processPaymentUseCase = processPaymentUseCase;
        }

        [HttpPost("process")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                // Add model validation
                if (paymentRequest == null)
                {
                    return BadRequest(new { Error = "Request body is null or invalid" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Error = "Invalid model state", Details = ModelState });
                }

                // Log the received request for debugging
                Console.WriteLine($"Processing payment: RideId={paymentRequest.RideId}, PaymentMethod={paymentRequest.PaymentMethod}, Amount={paymentRequest.Amount}");

                // Calculate fare
                var fare = await _calculateFareUseCase.ExecuteAsync(paymentRequest.RideId);

                // Process payment
                bool paymentSuccess = await _processPaymentUseCase.ExecuteAsync(paymentRequest.RideId, fare, paymentRequest.PaymentMethod);

                if (!paymentSuccess)
                    return BadRequest(new { Error = "Payment failed." });

                return Ok(new { Message = "Payment successful.", Amount = fare });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ProcessPayment: {ex.Message}");
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
