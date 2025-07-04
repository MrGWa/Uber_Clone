using UberClone.Domain.Entities;

namespace UberClone.Tests.Helpers;

public static class TestDataBuilder
{
    public static User CreateValidUser(string? username = null, string? email = null)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username ?? "testuser",
            Email = email ?? "test@example.com",
            PasswordHash = "hashedpassword123",
            FirstName = "John",
            LastName = "Doe",
            Role = "Passenger"
        };
    }

    public static Ride CreateValidRide(Guid? passengerId = null, Guid? driverId = null, decimal distance = 10.0m)
    {
        return new Ride
        {
            Id = Guid.NewGuid(),
            PassengerId = passengerId ?? Guid.NewGuid(),
            DriverId = driverId ?? Guid.NewGuid(),
            Distance = distance,
            Status = RideStatus.Pending,
            PickupLocation = "123 Main St",
            DropoffLocation = "456 Oak Ave",
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Transaction CreateValidTransaction(Guid? rideId = null, decimal amount = 25.50m)
    {
        return new Transaction
        {
            Id = 1,
            RideId = rideId ?? Guid.NewGuid(),
            Amount = amount,
            PaymentMethod = "Credit Card",
            IsSuccessful = true,
            TransactionDate = DateTime.UtcNow
        };
    }

    public static class RideStatuses
    {
        public static readonly string[] All = 
        {
            RideStatus.Pending,
            RideStatus.Accepted,
            RideStatus.Started,
            RideStatus.Completed,
            RideStatus.Cancelled
        };
    }

    public static class PaymentMethods
    {
        public static readonly string[] All = 
        {
            "Credit Card",
            "PayPal",
            "Cash",
            "Digital Wallet",
            "Bank Transfer"
        };
    }
}
