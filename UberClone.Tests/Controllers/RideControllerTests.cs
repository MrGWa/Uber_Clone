using UberClone.Tests.TestControllers;
using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces.UseCases;

namespace UberClone.Tests.Controllers;

public class RideControllerTests
{
    private readonly Mock<IStartRideUseCase> _mockStartRideUseCase;
    private readonly Mock<ICompleteRideUseCase> _mockCompleteRideUseCase;
    private readonly Mock<ICancelRideUseCase> _mockCancelRideUseCase;
    private readonly TestRideController _controller;

    public RideControllerTests()
    {
        _mockStartRideUseCase = new Mock<IStartRideUseCase>();
        _mockCompleteRideUseCase = new Mock<ICompleteRideUseCase>();
        _mockCancelRideUseCase = new Mock<ICancelRideUseCase>();
        _controller = new TestRideController(
            _mockStartRideUseCase.Object,
            _mockCompleteRideUseCase.Object,
            _mockCancelRideUseCase.Object);
    }

    [Fact]
    public async Task StartRide_WithValidDto_ReturnsOkResult()
    {
        // Arrange
        var startRideDto = new StartRideDto
        {
            RideId = Guid.NewGuid(),
            DriverId = Guid.NewGuid()
        };

        _mockStartRideUseCase
            .Setup(x => x.ExecuteAsync(It.IsAny<StartRideDto>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Start(startRideDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be("Ride started.");
        
        _mockStartRideUseCase.Verify(x => x.ExecuteAsync(startRideDto), Times.Once);
    }

    [Fact]
    public async Task CompleteRide_WithValidDto_ReturnsOkResult()
    {
        // Arrange
        var completeRideDto = new CompleteRideDto
        {
            RideId = Guid.NewGuid()
        };

        _mockCompleteRideUseCase
            .Setup(x => x.ExecuteAsync(It.IsAny<CompleteRideDto>()))
            .ReturnsAsync(25.50m);

        // Act
        var result = await _controller.Complete(completeRideDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be("Ride completed.");
        
        _mockCompleteRideUseCase.Verify(x => x.ExecuteAsync(completeRideDto), Times.Once);
    }

    [Fact]
    public async Task CancelRide_WithValidDto_ReturnsOkResult()
    {
        // Arrange
        var cancelRideDto = new CancelRideDto
        {
            RideId = Guid.NewGuid(),
            Reason = "Passenger requested cancellation"
        };

        _mockCancelRideUseCase
            .Setup(x => x.ExecuteAsync(It.IsAny<CancelRideDto>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Cancel(cancelRideDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be("Ride cancelled.");
        
        _mockCancelRideUseCase.Verify(x => x.ExecuteAsync(cancelRideDto), Times.Once);
    }

    [Fact]
    public async Task StartRide_WhenUseCaseThrowsException_ShouldPropagate()
    {
        // Arrange
        var startRideDto = new StartRideDto
        {
            RideId = Guid.NewGuid(),
            DriverId = Guid.NewGuid()
        };

        var exceptionMessage = "Driver not available";
        _mockStartRideUseCase
            .Setup(x => x.ExecuteAsync(It.IsAny<StartRideDto>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Start(startRideDto));
        exception.Message.Should().Be(exceptionMessage);
    }

    [Fact]
    public async Task CompleteRide_WhenUseCaseThrowsException_ShouldPropagate()
    {
        // Arrange
        var completeRideDto = new CompleteRideDto
        {
            RideId = Guid.NewGuid()
        };

        var exceptionMessage = "Ride not found";
        _mockCompleteRideUseCase
            .Setup(x => x.ExecuteAsync(It.IsAny<CompleteRideDto>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Complete(completeRideDto));
        exception.Message.Should().Be(exceptionMessage);
    }

    [Fact]
    public async Task CancelRide_WhenUseCaseThrowsException_ShouldPropagate()
    {
        // Arrange
        var cancelRideDto = new CancelRideDto
        {
            RideId = Guid.NewGuid(),
            Reason = "System error"
        };

        var exceptionMessage = "Cannot cancel completed ride";
        _mockCancelRideUseCase
            .Setup(x => x.ExecuteAsync(It.IsAny<CancelRideDto>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Cancel(cancelRideDto));
        exception.Message.Should().Be(exceptionMessage);
    }
}
