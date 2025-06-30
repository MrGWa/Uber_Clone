using System;
using System.Linq;
using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;

namespace UberClone.Infrastructure.Services;

public class RideService : IRideService
{
    private readonly AppDbContext _context;

    public RideService(AppDbContext context)
    {
        _context = context;
    }

    public Ride RequestRide(RideRequestDto dto)
    {
        var ride = new Ride
        {
            PassengerId = dto.PassengerId,
            CreatedAt = DateTime.UtcNow,
            Status = RideStatus.Pending
        };

        _context.Rides.Add(ride);
        _context.SaveChanges();
        return ride;
    }

    public void AcceptRide(RideAcceptedDto dto)
    {
        var ride = _context.Rides.FirstOrDefault(r => r.Id == dto.RideId);
        if (ride == null) throw new InvalidOperationException("Ride not found");

        ride.DriverId = dto.DriverId;
        ride.Status = RideStatus.Accepted;
        _context.SaveChanges();
    }

    public Ride CompleteRide(RideCompletedDto dto)
    {
        var ride = _context.Rides.FirstOrDefault(r => r.Id == dto.RideId);
        if (ride == null) throw new InvalidOperationException("Ride not found");

        ride.Status = RideStatus.Completed;
        ride.Fare = 5.0m + (decimal)(DateTime.UtcNow - ride.CreatedAt).TotalMinutes;
        _context.SaveChanges();
        return ride;
    }
}
