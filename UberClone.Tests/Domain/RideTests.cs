using FluentAssertions;
using UberClone.Domain.Entities;

namespace UberClone.Tests.Domain;

public class RideTests
{
    [Fact]
    public void Ride_ShouldInitializeWithDefaultValues()
    {
        // Act
        var ride = new Ride();

        // Assert
        ride.Id.Should().NotBe(Guid.Empty);
        ride.Status.Should().Be(RideStatus.Pending);
        ride.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        ride.StartedAt.Should().BeNull();
        ride.CompletedAt.Should().BeNull();
        ride.CancelledAt.Should().BeNull();
        ride.CancellationReason.Should().BeNull();
        ride.Fare.Should().BeNull();
        ride.Distance.Should().Be(0);
        ride.PickupLocation.Should().BeNull();
        ride.DropoffLocation.Should().BeNull();
    }

    [Fact]
    public void Ride_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var passengerId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var status = RideStatus.Started;
        var createdAt = DateTime.UtcNow.AddMinutes(-10);
        var startedAt = DateTime.UtcNow.AddMinutes(-5);
        var fare = 25.50m;
        var distance = 10.5m;
        var pickupLocation = "123 Main St";
        var dropOffLocation = "456 Oak Ave";

        // Act
        var ride = new Ride
        {
            Id = rideId,
            PassengerId = passengerId,
            DriverId = driverId,
            Status = status,
            CreatedAt = createdAt,
            StartedAt = startedAt,
            Fare = fare,
            Distance = distance,
            PickupLocation = pickupLocation,
            DropoffLocation = dropOffLocation
        };

        // Assert
        ride.Id.Should().Be(rideId);
        ride.PassengerId.Should().Be(passengerId);
        ride.DriverId.Should().Be(driverId);
        ride.Status.Should().Be(status);
        ride.CreatedAt.Should().Be(createdAt);
        ride.StartedAt.Should().Be(startedAt);
        ride.Fare.Should().Be(fare);
        ride.Distance.Should().Be(distance);
        ride.PickupLocation.Should().Be(pickupLocation);
        ride.DropoffLocation.Should().Be(dropOffLocation);
    }

    [Theory]
    [InlineData(RideStatus.Pending)]
    [InlineData(RideStatus.Accepted)]
    [InlineData(RideStatus.Started)]
    [InlineData(RideStatus.Completed)]
    [InlineData(RideStatus.Cancelled)]
    public void Ride_ShouldAcceptValidStatuses(string status)
    {
        // Arrange & Act
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Status = status
        };

        // Assert
        ride.Status.Should().Be(status);
    }

    [Fact]
    public void Ride_ShouldHandleNullableProperties()
    {
        // Arrange & Act
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Distance = 15.0m,
            StartedAt = null,
            CompletedAt = null,
            CancelledAt = null,
            CancellationReason = null,
            Fare = null
        };

        // Assert
        ride.StartedAt.Should().BeNull();
        ride.CompletedAt.Should().BeNull();
        ride.CancelledAt.Should().BeNull();
        ride.CancellationReason.Should().BeNull();
        ride.Fare.Should().BeNull();
    }

    [Fact]
    public void Ride_ShouldSetCancellationDetails()
    {
        // Arrange
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Status = RideStatus.Started
        };

        var cancellationTime = DateTime.UtcNow;
        var cancellationReason = "Passenger requested cancellation";

        // Act
        ride.Status = RideStatus.Cancelled;
        ride.CancelledAt = cancellationTime;
        ride.CancellationReason = cancellationReason;

        // Assert
        ride.Status.Should().Be(RideStatus.Cancelled);
        ride.CancelledAt.Should().Be(cancellationTime);
        ride.CancellationReason.Should().Be(cancellationReason);
    }

    [Fact]
    public void Ride_ShouldSetCompletionDetails()
    {
        // Arrange
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Status = RideStatus.Started,
            StartedAt = DateTime.UtcNow.AddMinutes(-10)
        };

        var completionTime = DateTime.UtcNow;
        var finalFare = 22.75m;

        // Act
        ride.Status = RideStatus.Completed;
        ride.CompletedAt = completionTime;
        ride.Fare = finalFare;

        // Assert
        ride.Status.Should().Be(RideStatus.Completed);
        ride.CompletedAt.Should().Be(completionTime);
        ride.Fare.Should().Be(finalFare);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.5)]
    [InlineData(25.75)]
    [InlineData(100.0)]
    public void Ride_ShouldAcceptValidDistances(decimal distance)
    {
        // Arrange & Act
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            Distance = distance
        };

        // Assert
        ride.Distance.Should().Be(distance);
    }
}
