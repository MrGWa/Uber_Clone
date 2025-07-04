using FluentAssertions;
using UberClone.Domain.Entities;

namespace UberClone.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Transaction_ShouldInitializeWithDefaultValues()
    {
        // Act
        var transaction = new Transaction();

        // Assert
        transaction.Id.Should().Be(0);
        transaction.RideId.Should().Be(Guid.Empty);
        transaction.Amount.Should().Be(0);
        transaction.PaymentMethod.Should().BeNull();
        transaction.IsSuccessful.Should().BeFalse();
        transaction.TransactionDate.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void Transaction_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = 1;
        var rideId = Guid.NewGuid();
        var amount = 25.50m;
        var paymentMethod = "Credit Card";
        var isSuccessful = true;
        var transactionDate = DateTime.UtcNow;

        // Act
        var transaction = new Transaction
        {
            Id = id,
            RideId = rideId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            IsSuccessful = isSuccessful,
            TransactionDate = transactionDate
        };

        // Assert
        transaction.Id.Should().Be(id);
        transaction.RideId.Should().Be(rideId);
        transaction.Amount.Should().Be(amount);
        transaction.PaymentMethod.Should().Be(paymentMethod);
        transaction.IsSuccessful.Should().Be(isSuccessful);
        transaction.TransactionDate.Should().Be(transactionDate);
    }

    [Theory]
    [InlineData("Credit Card")]
    [InlineData("PayPal")]
    [InlineData("Cash")]
    [InlineData("Digital Wallet")]
    [InlineData("Bank Transfer")]
    public void Transaction_ShouldAcceptValidPaymentMethods(string paymentMethod)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            RideId = Guid.NewGuid(),
            Amount = 15.75m,
            PaymentMethod = paymentMethod,
            IsSuccessful = true,
            TransactionDate = DateTime.UtcNow
        };

        // Assert
        transaction.PaymentMethod.Should().Be(paymentMethod);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.50)]
    [InlineData(25.75)]
    [InlineData(100.00)]
    [InlineData(999.99)]
    public void Transaction_ShouldAcceptValidAmounts(decimal amount)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            RideId = Guid.NewGuid(),
            Amount = amount,
            PaymentMethod = "Credit Card",
            IsSuccessful = true,
            TransactionDate = DateTime.UtcNow
        };

        // Assert
        transaction.Amount.Should().Be(amount);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Transaction_ShouldTrackSuccessStatus(bool isSuccessful)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            RideId = Guid.NewGuid(),
            Amount = 25.50m,
            PaymentMethod = "Credit Card",
            IsSuccessful = isSuccessful,
            TransactionDate = DateTime.UtcNow
        };

        // Assert
        transaction.IsSuccessful.Should().Be(isSuccessful);
    }

    [Fact]
    public void Transaction_ShouldHandleFailedTransactions()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 30.00m;
        var paymentMethod = "Credit Card";

        // Act
        var transaction = new Transaction
        {
            RideId = rideId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            IsSuccessful = false,
            TransactionDate = DateTime.UtcNow
        };

        // Assert
        transaction.RideId.Should().Be(rideId);
        transaction.Amount.Should().Be(amount);
        transaction.PaymentMethod.Should().Be(paymentMethod);
        transaction.IsSuccessful.Should().BeFalse();
    }

    [Fact]
    public void Transaction_ShouldHandleSuccessfulTransactions()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 18.25m;
        var paymentMethod = "PayPal";

        // Act
        var transaction = new Transaction
        {
            RideId = rideId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            IsSuccessful = true,
            TransactionDate = DateTime.UtcNow
        };

        // Assert
        transaction.RideId.Should().Be(rideId);
        transaction.Amount.Should().Be(amount);
        transaction.PaymentMethod.Should().Be(paymentMethod);
        transaction.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Transaction_ShouldTrackTransactionDate()
    {
        // Arrange
        var testDate = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var transaction = new Transaction
        {
            RideId = Guid.NewGuid(),
            Amount = 20.00m,
            PaymentMethod = "Credit Card",
            IsSuccessful = true,
            TransactionDate = testDate
        };

        // Assert
        transaction.TransactionDate.Should().Be(testDate);
    }
}
