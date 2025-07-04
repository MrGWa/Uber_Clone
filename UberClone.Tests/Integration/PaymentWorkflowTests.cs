using FluentAssertions;
using Moq;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces;
using UberClone.Application.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Tests.Integration;

public class PaymentWorkflowTests
{
    [Fact]
    public async Task CompletePaymentWorkflow_ShouldProcessSuccessfully()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var distance = 12.5m;
        var expectedFare = 5.0m + (distance * 1.5m); // 23.75m

        // Setup ride
        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Distance = distance,
            Status = RideStatus.Started
        };

        // Setup mocks
        var mockRideRepository = new Mock<IRideRepository>();
        var mockPaymentGateway = new Mock<IPaymentGateway>();
        var mockTransactionRepository = new Mock<ITransactionRepository>();

        mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync(ride);

        mockRideRepository
            .Setup(x => x.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);

        mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(true);

        mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Create use cases
        var calculateFareUseCase = new CalculateFareUseCase(mockRideRepository.Object);
        var processPaymentUseCase = new ProcessPaymentUseCase(mockPaymentGateway.Object, mockTransactionRepository.Object);

        // Act
        // Step 1: Calculate fare
        var calculatedFare = await calculateFareUseCase.ExecuteAsync(rideId);
        
        // Step 2: Process payment
        var paymentSuccess = await processPaymentUseCase.ExecuteAsync(rideId, calculatedFare, "Credit Card");

        // Assert
        calculatedFare.Should().Be(expectedFare);
        paymentSuccess.Should().BeTrue();
        
        // Verify fare was updated on the ride
        ride.Fare.Should().Be(expectedFare);
        
        // Verify all repositories were called correctly
        mockRideRepository.Verify(x => x.GetRideByIdAsync(rideId), Times.Once);
        mockRideRepository.Verify(x => x.UpdateRideAsync(ride), Times.Once);
        mockPaymentGateway.Verify(x => x.ProcessPayment(It.Is<PaymentDetails>(p => 
            p.Amount == expectedFare && p.PaymentMethod == "Credit Card")), Times.Once);
        mockTransactionRepository.Verify(x => x.SaveAsync(It.Is<Transaction>(t => 
            t.RideId == rideId && t.Amount == expectedFare && t.IsSuccessful)), Times.Once);
    }

    [Fact]
    public async Task PaymentWorkflow_WhenRideNotFound_ShouldFailGracefully()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        
        var mockRideRepository = new Mock<IRideRepository>();
        var mockPaymentGateway = new Mock<IPaymentGateway>();
        var mockTransactionRepository = new Mock<ITransactionRepository>();

        mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync((Ride?)null);

        var calculateFareUseCase = new CalculateFareUseCase(mockRideRepository.Object);
        var processPaymentUseCase = new ProcessPaymentUseCase(mockPaymentGateway.Object, mockTransactionRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => calculateFareUseCase.ExecuteAsync(rideId));
        exception.Message.Should().Be("Ride not found.");
        
        // Verify payment was never attempted
        mockPaymentGateway.Verify(x => x.ProcessPayment(It.IsAny<PaymentDetails>()), Times.Never);
        mockTransactionRepository.Verify(x => x.SaveAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task PaymentWorkflow_WhenPaymentFails_ShouldRecordFailedTransaction()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var distance = 8.0m;
        var expectedFare = 5.0m + (distance * 1.5m); // 17.0m

        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Distance = distance,
            Status = RideStatus.Started
        };

        var mockRideRepository = new Mock<IRideRepository>();
        var mockPaymentGateway = new Mock<IPaymentGateway>();
        var mockTransactionRepository = new Mock<ITransactionRepository>();

        mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync(ride);

        mockRideRepository
            .Setup(x => x.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);

        mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(false); // Payment fails

        mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        var calculateFareUseCase = new CalculateFareUseCase(mockRideRepository.Object);
        var processPaymentUseCase = new ProcessPaymentUseCase(mockPaymentGateway.Object, mockTransactionRepository.Object);

        // Act
        var calculatedFare = await calculateFareUseCase.ExecuteAsync(rideId);
        var paymentSuccess = await processPaymentUseCase.ExecuteAsync(rideId, calculatedFare, "Credit Card");

        // Assert
        calculatedFare.Should().Be(expectedFare);
        paymentSuccess.Should().BeFalse();
        
        // Verify failed transaction was recorded
        mockTransactionRepository.Verify(x => x.SaveAsync(It.Is<Transaction>(t => 
            t.RideId == rideId && 
            t.Amount == expectedFare && 
            t.PaymentMethod == "Credit Card" && 
            t.IsSuccessful == false)), Times.Once);
    }

    [Theory]
    [InlineData(0.0, 5.0)]
    [InlineData(1.0, 6.5)]
    [InlineData(10.0, 20.0)]
    [InlineData(25.5, 43.25)]
    [InlineData(100.0, 155.0)]
    public async Task PaymentWorkflow_WithVariousDistances_ShouldCalculateCorrectFares(decimal distance, decimal expectedFare)
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var ride = new Ride
        {
            Id = rideId,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Distance = distance,
            Status = RideStatus.Started
        };

        var mockRideRepository = new Mock<IRideRepository>();
        var mockPaymentGateway = new Mock<IPaymentGateway>();
        var mockTransactionRepository = new Mock<ITransactionRepository>();

        mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync(ride);

        mockRideRepository
            .Setup(x => x.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);

        mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(true);

        mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        var calculateFareUseCase = new CalculateFareUseCase(mockRideRepository.Object);
        var processPaymentUseCase = new ProcessPaymentUseCase(mockPaymentGateway.Object, mockTransactionRepository.Object);

        // Act
        var calculatedFare = await calculateFareUseCase.ExecuteAsync(rideId);
        var paymentSuccess = await processPaymentUseCase.ExecuteAsync(rideId, calculatedFare, "Credit Card");

        // Assert
        calculatedFare.Should().Be(expectedFare);
        paymentSuccess.Should().BeTrue();
        ride.Fare.Should().Be(expectedFare);
    }
}
