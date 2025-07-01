namespace UberClone.Tests
{
    public class CalculateFareUseCaseTests
    {
        private readonly Mock<IRideRepository> _rideRepositoryMock;
        private readonly CalculateFareUseCase _calculateFareUseCase;

        public CalculateFareUseCaseTests()
        {
            _rideRepositoryMock = new Mock<IRideRepository>();
            _calculateFareUseCase = new CalculateFareUseCase(_rideRepositoryMock.Object);
        }

        [Fact]
        public void Execute_CalculateFare_ReturnsCorrectFare()
        {
            // Arrange
            _rideRepositoryMock.Setup(r => r.GetRideById(It.IsAny<int>())).Returns(new Ride { Distance = 10 });

            // Act
            var fare = _calculateFareUseCase.Execute(1);

            // Assert
            Assert.Equal(15m, fare); // Example: 10 * 1.5
        }
    }
}
