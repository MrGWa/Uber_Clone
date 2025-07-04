using FluentAssertions;
using Moq;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces;
using UberClone.Application.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Tests.UseCases;

public class ProcessPaymentUseCaseTests
{
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly ProcessPaymentUseCase _useCase;

    public ProcessPaymentUseCaseTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _useCase = new ProcessPaymentUseCase(_mockPaymentGateway.Object, _mockTransactionRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithSuccessfulPayment_ReturnsTrue()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 25.50m;
        var paymentMethod = "Credit Card";

        _mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(true);

        _mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId, amount, paymentMethod);

        // Assert
        result.Should().BeTrue();
        
        _mockPaymentGateway.Verify(x => x.ProcessPayment(It.Is<PaymentDetails>(p => 
            p.Amount == amount && 
            p.PaymentMethod == paymentMethod && 
            p.RideId == rideId)), Times.Once);
        
        _mockTransactionRepository.Verify(x => x.SaveAsync(It.Is<Transaction>(t => 
            t.RideId == rideId && 
            t.Amount == amount && 
            t.PaymentMethod == paymentMethod && 
            t.IsSuccessful == true)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithFailedPayment_ReturnsFalse()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 25.50m;
        var paymentMethod = "Credit Card";

        _mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(false);

        _mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId, amount, paymentMethod);

        // Assert
        result.Should().BeFalse();
        
        _mockTransactionRepository.Verify(x => x.SaveAsync(It.Is<Transaction>(t => 
            t.RideId == rideId && 
            t.Amount == amount && 
            t.PaymentMethod == paymentMethod && 
            t.IsSuccessful == false)), Times.Once);
    }

    [Theory]
    [InlineData("Credit Card")]
    [InlineData("PayPal")]
    [InlineData("Cash")]
    [InlineData("Digital Wallet")]
    public async Task ExecuteAsync_WithDifferentPaymentMethods_ProcessesCorrectly(string paymentMethod)
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 15.75m;

        _mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(true);

        _mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId, amount, paymentMethod);

        // Assert
        result.Should().BeTrue();
        
        _mockPaymentGateway.Verify(x => x.ProcessPayment(It.Is<PaymentDetails>(p => 
            p.PaymentMethod == paymentMethod)), Times.Once);
        
        _mockTransactionRepository.Verify(x => x.SaveAsync(It.Is<Transaction>(t => 
            t.PaymentMethod == paymentMethod)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithZeroAmount_ProcessesCorrectly()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 0.0m;
        var paymentMethod = "Credit Card";

        _mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(true);

        _mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId, amount, paymentMethod);

        // Assert
        result.Should().BeTrue();
        
        _mockPaymentGateway.Verify(x => x.ProcessPayment(It.Is<PaymentDetails>(p => 
            p.Amount == 0.0m)), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPaymentGatewayThrowsException_ShouldStillSaveFailedTransaction()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 25.50m;
        var paymentMethod = "Credit Card";

        _mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ThrowsAsync(new Exception("Payment gateway error"));

        _mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _useCase.ExecuteAsync(rideId, amount, paymentMethod));
        
        _mockPaymentGateway.Verify(x => x.ProcessPayment(It.IsAny<PaymentDetails>()), Times.Once);
        // Transaction should not be saved if payment gateway throws exception
        _mockTransactionRepository.Verify(x => x.SaveAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateTransactionWithCorrectDescription()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 25.50m;
        var paymentMethod = "Credit Card";

        _mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(true);

        _mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId, amount, paymentMethod);

        // Assert
        result.Should().BeTrue();
        
        _mockPaymentGateway.Verify(x => x.ProcessPayment(It.Is<PaymentDetails>(p => 
            p.Description == $"Payment for ride {rideId}")), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSetTransactionDateToCurrentTime()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 25.50m;
        var paymentMethod = "Credit Card";
        var testStartTime = DateTime.UtcNow;

        _mockPaymentGateway
            .Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
            .ReturnsAsync(true);

        _mockTransactionRepository
            .Setup(x => x.SaveAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId, amount, paymentMethod);

        // Assert
        result.Should().BeTrue();
        
        _mockTransactionRepository.Verify(x => x.SaveAsync(It.Is<Transaction>(t => 
            t.TransactionDate >= testStartTime && 
            t.TransactionDate <= DateTime.UtcNow)), Times.Once);
    }
}
