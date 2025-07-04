using FluentAssertions;
using UberClone.Domain.Entities;

namespace UberClone.Tests;

public class DomainEntityTests
{
    [Fact]
    public void User_Should_Have_Default_Role_As_Passenger()
    {
        // Arrange & Act
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword"
        };

        // Assert
        user.Role.Should().Be("Passenger");
        user.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Ride_Should_Have_Default_Status_As_Pending()
    {
        // Arrange & Act
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Distance = 10.5m,
            PickupLocation = "Location A",
            DropoffLocation = "Location B"
        };

        // Assert
        ride.Status.Should().Be(RideStatus.Pending);
        ride.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Transaction_Should_Initialize_With_Correct_Properties()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var amount = 25.50m;
        var paymentMethod = "Credit Card";

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
}
