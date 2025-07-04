using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UberClone.Tests.TestControllers;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces.UseCases;

namespace UberClone.Tests.Controllers;

public class PaymentControllerTests
{
    private readonly Mock<ICalculateFareUseCase> _mockCalculateFareUseCase;
    private readonly Mock<IProcessPaymentUseCase> _mockProcessPaymentUseCase;
    private readonly TestPaymentController _controller;

    public PaymentControllerTests()
    {
        _mockCalculateFareUseCase = new Mock<ICalculateFareUseCase>();
        _mockProcessPaymentUseCase = new Mock<IProcessPaymentUseCase>();
        _controller = new TestPaymentController(_mockCalculateFareUseCase.Object, _mockProcessPaymentUseCase.Object);
    }

    [Fact]
    public async Task ProcessPayment_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentRequest = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = "Credit Card",
            Amount = 25.50m
        };

        var calculatedFare = 25.50m;
        _mockCalculateFareUseCase
            .Setup(x => x.ExecuteAsync(rideId))
            .ReturnsAsync(calculatedFare);

        _mockProcessPaymentUseCase
            .Setup(x => x.ExecuteAsync(rideId, calculatedFare, paymentRequest.PaymentMethod))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.ProcessPayment(paymentRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        
        var responseValue = okResult!.Value;
        responseValue.Should().NotBeNull();
        
        _mockCalculateFareUseCase.Verify(x => x.ExecuteAsync(rideId), Times.Once);
        _mockProcessPaymentUseCase.Verify(x => x.ExecuteAsync(rideId, calculatedFare, paymentRequest.PaymentMethod), Times.Once);
    }

    [Fact]
    public async Task ProcessPayment_WhenPaymentFails_ReturnsBadRequest()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentRequest = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = "Credit Card",
            Amount = 25.50m
        };

        var calculatedFare = 25.50m;
        _mockCalculateFareUseCase
            .Setup(x => x.ExecuteAsync(rideId))
            .ReturnsAsync(calculatedFare);

        _mockProcessPaymentUseCase
            .Setup(x => x.ExecuteAsync(rideId, calculatedFare, paymentRequest.PaymentMethod))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.ProcessPayment(paymentRequest);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task ProcessPayment_WhenCalculateFareThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentRequest = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = "Credit Card",
            Amount = 25.50m
        };

        var exceptionMessage = "Ride not found";
        _mockCalculateFareUseCase
            .Setup(x => x.ExecuteAsync(rideId))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.ProcessPayment(paymentRequest);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().NotBeNull();
        
        _mockProcessPaymentUseCase.Verify(x => x.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ProcessPayment_WithDifferentPaymentMethods_ProcessesCorrectly()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentMethods = new[] { "Credit Card", "PayPal", "Cash", "Digital Wallet" };
        var calculatedFare = 15.75m;

        _mockCalculateFareUseCase
            .Setup(x => x.ExecuteAsync(rideId))
            .ReturnsAsync(calculatedFare);

        _mockProcessPaymentUseCase
            .Setup(x => x.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        foreach (var method in paymentMethods)
        {
            var paymentRequest = new PaymentRequest
            {
                RideId = rideId,
                PaymentMethod = method,
                Amount = calculatedFare
            };

            // Act
            var result = await _controller.ProcessPayment(paymentRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        _mockProcessPaymentUseCase.Verify(x => x.ExecuteAsync(rideId, calculatedFare, It.IsAny<string>()), Times.Exactly(paymentMethods.Length));
    }
}
