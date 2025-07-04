using FluentAssertions;
using UberClone.Application.DTOs;

namespace UberClone.Tests.DTOs;

public class PaymentRequestTests
{
    [Fact]
    public void PaymentRequest_ShouldInitializeWithDefaultValues()
    {
        // Act
        var request = new PaymentRequest();

        // Assert
        request.RideId.Should().Be(Guid.Empty);
        request.PaymentMethod.Should().BeNull();
        request.Amount.Should().Be(0);
    }

    [Fact]
    public void PaymentRequest_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentMethod = "Credit Card";
        var amount = 25.50m;

        // Act
        var request = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = paymentMethod,
            Amount = amount
        };

        // Assert
        request.RideId.Should().Be(rideId);
        request.PaymentMethod.Should().Be(paymentMethod);
        request.Amount.Should().Be(amount);
    }

    [Theory]
    [InlineData("Credit Card")]
    [InlineData("PayPal")]
    [InlineData("Cash")]
    [InlineData("Digital Wallet")]
    [InlineData("Bank Transfer")]
    public void PaymentRequest_ShouldAcceptValidPaymentMethods(string paymentMethod)
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 15.75m;

        // Act
        var request = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = paymentMethod,
            Amount = amount
        };

        // Assert
        request.PaymentMethod.Should().Be(paymentMethod);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.50)]
    [InlineData(25.75)]
    [InlineData(100.00)]
    [InlineData(999.99)]
    public void PaymentRequest_ShouldAcceptValidAmounts(decimal amount)
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentMethod = "Credit Card";

        // Act
        var request = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = paymentMethod,
            Amount = amount
        };

        // Assert
        request.Amount.Should().Be(amount);
    }

    [Fact]
    public void PaymentRequest_ShouldHandleUniqueRideIds()
    {
        // Arrange
        var rideId1 = Guid.NewGuid();
        var rideId2 = Guid.NewGuid();
        var paymentMethod = "Credit Card";
        var amount = 20.00m;

        // Act
        var request1 = new PaymentRequest
        {
            RideId = rideId1,
            PaymentMethod = paymentMethod,
            Amount = amount
        };

        var request2 = new PaymentRequest
        {
            RideId = rideId2,
            PaymentMethod = paymentMethod,
            Amount = amount
        };

        // Assert
        request1.RideId.Should().Be(rideId1);
        request2.RideId.Should().Be(rideId2);
        request1.RideId.Should().NotBe(request2.RideId);
    }

    [Fact]
    public void PaymentRequest_ShouldHandleZeroAmount()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentMethod = "Credit Card";
        var amount = 0.0m;

        // Act
        var request = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = paymentMethod,
            Amount = amount
        };

        // Assert
        request.RideId.Should().Be(rideId);
        request.PaymentMethod.Should().Be(paymentMethod);
        request.Amount.Should().Be(0.0m);
    }

    [Fact]
    public void PaymentRequest_ShouldHandleCompletePaymentScenario()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var paymentMethod = "PayPal";
        var amount = 42.75m;

        // Act
        var request = new PaymentRequest
        {
            RideId = rideId,
            PaymentMethod = paymentMethod,
            Amount = amount
        };

        // Assert
        request.RideId.Should().Be(rideId);
        request.RideId.Should().NotBe(Guid.Empty);
        request.PaymentMethod.Should().Be(paymentMethod);
        request.PaymentMethod.Should().NotBeNullOrEmpty();
        request.Amount.Should().Be(amount);
        request.Amount.Should().BeGreaterThan(0);
    }
}
