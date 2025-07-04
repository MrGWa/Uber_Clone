using Microsoft.Extensions.DependencyInjection;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Application.UseCases;

namespace UberClone.Tests;

public static class TestServiceConfiguration
{
    public static IServiceProvider ConfigureTestServices()
    {
        var services = new ServiceCollection();

        // Register real use cases
        services.AddScoped<ICalculateFareUseCase, CalculateFareUseCase>();
        services.AddScoped<IProcessPaymentUseCase, ProcessPaymentUseCase>();

        // Register mocked dependencies
        services.AddScoped(_ => new Mock<IRideRepository>().Object);
        services.AddScoped(_ => new Mock<IPaymentGateway>().Object);
        services.AddScoped(_ => new Mock<ITransactionRepository>().Object);
        services.AddScoped(_ => new Mock<IUserRepository>().Object);

        return services.BuildServiceProvider();
    }
}

public static class TestDataBuilder
{
    public static class Users
    {
        public static RegisterUserDto CreateValidRegisterUserDto()
        {
            return new RegisterUserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };
        }

        public static User CreateValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                FirstName = "John",
                LastName = "Doe",
                Role = "Passenger"
            };
        }
    }

    public static class Rides
    {
        public static Ride CreateValidRide()
        {
            return new Ride
            {
                Id = Guid.NewGuid(),
                PassengerId = Guid.NewGuid(),
                DriverId = Guid.NewGuid(),
                Distance = 10.0m,
                Status = RideStatus.Started,
                PickupLocation = "123 Main St",
                DropoffLocation = "456 Oak Ave",
                CreatedAt = DateTime.UtcNow,
                StartedAt = DateTime.UtcNow.AddMinutes(-5)
            };
        }

        public static Ride CreateRideWithDistance(decimal distance)
        {
            var ride = CreateValidRide();
            ride.Distance = distance;
            return ride;
        }
    }

    public static class Payments
    {
        public static PaymentRequest CreateValidPaymentRequest()
        {
            return new PaymentRequest
            {
                RideId = Guid.NewGuid(),
                PaymentMethod = "Credit Card",
                Amount = 25.50m
            };
        }

        public static PaymentRequest CreatePaymentRequestWithMethod(string paymentMethod)
        {
            var request = CreateValidPaymentRequest();
            request.PaymentMethod = paymentMethod;
            return request;
        }
    }

    public static class Transactions
    {
        public static Transaction CreateValidTransaction()
        {
            return new Transaction
            {
                Id = 1,
                RideId = Guid.NewGuid(),
                Amount = 25.50m,
                PaymentMethod = "Credit Card",
                IsSuccessful = true,
                TransactionDate = DateTime.UtcNow
            };
        }

        public static Transaction CreateFailedTransaction()
        {
            var transaction = CreateValidTransaction();
            transaction.IsSuccessful = false;
            return transaction;
        }
    }
}

public static class TestConstants
{
    public static class FareCalculation
    {
        public const decimal BaseFare = 5.0m;
        public const decimal RatePerKm = 1.5m;
    }

    public static class PaymentMethods
    {
        public const string CreditCard = "Credit Card";
        public const string PayPal = "PayPal";
        public const string Cash = "Cash";
        public const string DigitalWallet = "Digital Wallet";
    }
}
