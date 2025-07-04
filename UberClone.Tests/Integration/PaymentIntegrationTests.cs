using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UberClone.Tests.TestControllers;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Tests.Integration;

public class PaymentIntegrationTests
{
    private readonly Mock<ICalculateFareUseCase> _mockCalculateFareUseCase;
    private readonly Mock<IProcessPaymentUseCase> _mockProcessPaymentUseCase;
    private readonly TestPaymentController _controller;

    public PaymentIntegrationTests()
    {
        _mockCalculateFareUseCase = new Mock<ICalculateFareUseCase>();
        _mockProcessPaymentUseCase = new Mock<IProcessPaymentUseCase>();
        _controller = new TestPaymentController(_mockCalculateFareUseCase.Object, _mockProcessPaymentUseCase.Object);
    }

    [Fact]
    public async Task ProcessPayment_WithValidData_ShouldReturnSuccess()
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
        okResult!.Value.Should().NotBeNull();
        
        _mockCalculateFareUseCase.Verify(x => x.ExecuteAsync(rideId), Times.Once);
        _mockProcessPaymentUseCase.Verify(x => x.ExecuteAsync(rideId, calculatedFare, paymentRequest.PaymentMethod), Times.Once);
    }

    [Fact]
    public async Task ProcessPayment_WithInvalidRideId_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidRideId = Guid.NewGuid();
        var paymentRequest = new PaymentRequest
        {
            RideId = invalidRideId,
            PaymentMethod = "Credit Card",
            Amount = 25.50m
        };

        var exceptionMessage = "Ride not found";
        _mockCalculateFareUseCase
            .Setup(x => x.ExecuteAsync(invalidRideId))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.ProcessPayment(paymentRequest);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().NotBeNull();
        
        _mockCalculateFareUseCase.Verify(x => x.ExecuteAsync(invalidRideId), Times.Once);
        _mockProcessPaymentUseCase.Verify(x => x.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ProcessPayment_WithFailedPayment_ShouldReturnBadRequest()
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
        
        _mockCalculateFareUseCase.Verify(x => x.ExecuteAsync(rideId), Times.Once);
        _mockProcessPaymentUseCase.Verify(x => x.ExecuteAsync(rideId, calculatedFare, paymentRequest.PaymentMethod), Times.Once);
    }
}
