using FluentAssertions;
using Moq;
using UberClone.Application.Interfaces;
using UberClone.Application.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Tests.UseCases;

public class CalculateFareUseCaseTests
{
    private readonly Mock<IRideRepository> _mockRideRepository;
    private readonly CalculateFareUseCase _useCase;

    public CalculateFareUseCaseTests()
    {
        _mockRideRepository = new Mock<IRideRepository>();
        _useCase = new CalculateFareUseCase(_mockRideRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRide_CalculatesCorrectFare()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var ride = new Ride
        {
            Id = rideId,
            Distance = 10.0m,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid()
        };

        _mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync(ride);

        _mockRideRepository
            .Setup(x => x.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId);

        // Expected: 5.0 (base fare) + (10.0 * 1.5) = 20.0
        var expectedFare = 20.0m;

        // Assert
        result.Should().Be(expectedFare);
        ride.Fare.Should().Be(expectedFare);
        
        _mockRideRepository.Verify(x => x.GetRideByIdAsync(rideId), Times.Once);
        _mockRideRepository.Verify(x => x.UpdateRideAsync(ride), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithZeroDistance_ReturnsBaseFareOnly()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var ride = new Ride
        {
            Id = rideId,
            Distance = 0.0m,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid()
        };

        _mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync(ride);

        _mockRideRepository
            .Setup(x => x.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId);

        // Expected: 5.0 (base fare) + (0.0 * 1.5) = 5.0
        var expectedFare = 5.0m;

        // Assert
        result.Should().Be(expectedFare);
        ride.Fare.Should().Be(expectedFare);
    }

    [Fact]
    public async Task ExecuteAsync_WithLargeDistance_CalculatesCorrectFare()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var ride = new Ride
        {
            Id = rideId,
            Distance = 50.5m,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid()
        };

        _mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync(ride);

        _mockRideRepository
            .Setup(x => x.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId);

        // Expected: 5.0 (base fare) + (50.5 * 1.5) = 80.75
        var expectedFare = 80.75m;

        // Assert
        result.Should().Be(expectedFare);
        ride.Fare.Should().Be(expectedFare);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentRide_ThrowsException()
    {
        // Arrange
        var rideId = Guid.NewGuid();
        
        _mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync((Ride?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.ExecuteAsync(rideId));
        exception.Message.Should().Be("Ride not found.");
        
        _mockRideRepository.Verify(x => x.GetRideByIdAsync(rideId), Times.Once);
        _mockRideRepository.Verify(x => x.UpdateRideAsync(It.IsAny<Ride>()), Times.Never);
    }

    [Theory]
    [InlineData(1.0, 6.5)]
    [InlineData(2.5, 8.75)]
    [InlineData(15.0, 27.5)]
    [InlineData(100.0, 155.0)]
    public async Task ExecuteAsync_WithVariousDistances_CalculatesCorrectFares(decimal distance, decimal expectedFare)
    {
        // Arrange
        var rideId = Guid.NewGuid();
        var ride = new Ride
        {
            Id = rideId,
            Distance = distance,
            PassengerId = Guid.NewGuid(),
            DriverId = Guid.NewGuid()
        };

        _mockRideRepository
            .Setup(x => x.GetRideByIdAsync(rideId))
            .ReturnsAsync(ride);

        _mockRideRepository
            .Setup(x => x.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(rideId);

        // Assert
        result.Should().Be(expectedFare);
        ride.Fare.Should().Be(expectedFare);
    }
}
