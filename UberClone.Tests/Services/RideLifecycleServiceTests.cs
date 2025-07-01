using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UberClone.Application.DTOs.Ride;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using UberClone.Infrastructure.Services;
using Xunit;

namespace UberClone.Tests.Services;

public class RideLifecycleServiceTests
{
    private readonly RideService _service;
    private readonly AppDbContext _context;

    public RideLifecycleServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _service = new RideService(_context);
    }

    [Fact]
    public void RequestRide_ShouldCreateRide()
    {
        var dto = new RideRequestDto
        {
            PassengerId = Guid.NewGuid()
        };

        var ride = _service.RequestRide(dto);

        Assert.NotNull(ride);
        Assert.Equal(dto.PassengerId, ride.PassengerId);
        Assert.Equal(RideStatus.Pending, ride.Status);
    }

    [Fact]
    public void AcceptRide_ShouldUpdateRideStatus()
    {
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Status = RideStatus.Pending
        };
        _context.Rides.Add(ride);
        _context.SaveChanges();

        var dto = new RideAcceptedDto
        {
            RideId = ride.Id,
            DriverId = Guid.NewGuid()
        };

        _service.AcceptRide(dto);

        var updated = _context.Rides.First(r => r.Id == ride.Id);
        Assert.Equal(dto.DriverId, updated.DriverId);
        Assert.Equal(RideStatus.Accepted, updated.Status);
    }

    [Fact]
    public void CompleteRide_ShouldMarkRideCompletedAndCalculateFare()
    {
        var ride = new Ride
        {
            PassengerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow.AddMinutes(-10),
            Status = RideStatus.Accepted
        };
        _context.Rides.Add(ride);
        _context.SaveChanges();

        var dto = new RideCompletedDto
        {
            RideId = ride.Id
        };

        var completedRide = _service.CompleteRide(dto);

        Assert.Equal(RideStatus.Completed, completedRide.Status);
        Assert.True(completedRide.Fare > 5.0m);
    }

    [Fact]
    public void AcceptRide_InvalidRide_ShouldThrow()
    {
        var dto = new RideAcceptedDto
        {
            RideId = 999,
            DriverId = Guid.NewGuid()
        };

        Assert.Throws<InvalidOperationException>(() => _service.AcceptRide(dto));
    }
}
