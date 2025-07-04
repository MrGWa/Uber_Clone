using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces.UseCases;

namespace UberClone.Tests.TestControllers;

// Test-specific controller implementations to avoid circular dependencies
public class TestAuthController : ControllerBase
{
    private readonly IRegisterUserCommand _registerUserCommand;

    public TestAuthController(IRegisterUserCommand registerUserCommand)
    {
        _registerUserCommand = registerUserCommand;
    }

    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            await _registerUserCommand.ExecuteAsync(dto);
            return Ok("User registered successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class TestPaymentController : ControllerBase
{
    private readonly ICalculateFareUseCase _calculateFareUseCase;
    private readonly IProcessPaymentUseCase _processPaymentUseCase;

    public TestPaymentController(ICalculateFareUseCase calculateFareUseCase, IProcessPaymentUseCase processPaymentUseCase)
    {
        _calculateFareUseCase = calculateFareUseCase;
        _processPaymentUseCase = processPaymentUseCase;
    }

    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
    {
        try
        {
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
            return BadRequest(new { Error = ex.Message });
        }
    }
}

public class TestRideController : ControllerBase
{
    private readonly IStartRideUseCase _startRideUseCase;
    private readonly ICompleteRideUseCase _completeRideUseCase;
    private readonly ICancelRideUseCase _cancelRideUseCase;

    public TestRideController(
        IStartRideUseCase startRideUseCase,
        ICompleteRideUseCase completeRideUseCase,
        ICancelRideUseCase cancelRideUseCase)
    {
        _startRideUseCase = startRideUseCase;
        _completeRideUseCase = completeRideUseCase;
        _cancelRideUseCase = cancelRideUseCase;
    }

    public async Task<IActionResult> Start([FromBody] UberClone.Application.DTOs.Ride.StartRideDto dto)
    {
        await _startRideUseCase.ExecuteAsync(dto);
        return Ok("Ride started.");
    }

    public async Task<IActionResult> Complete([FromBody] UberClone.Application.DTOs.Ride.CompleteRideDto dto)
    {
        await _completeRideUseCase.ExecuteAsync(dto);
        return Ok("Ride completed.");
    }

    public async Task<IActionResult> Cancel([FromBody] UberClone.Application.DTOs.Ride.CancelRideDto dto)
    {
        await _cancelRideUseCase.ExecuteAsync(dto);
        return Ok("Ride cancelled.");
    }
}
