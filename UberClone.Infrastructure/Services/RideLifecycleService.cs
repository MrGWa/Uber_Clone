using UberClone.Application.DTOs.Ride;
using UberClone.Application.Interfaces.Services;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services;

public class RideLifecycleService : IRideLifecycleService
{
    private readonly AppDbContext _context;

    public RideLifecycleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task StartRideAsync(StartRideDto dto)
    {
        var ride = await _context.Rides.FindAsync(dto.RideId)
            ?? throw new Exception("Ride not found.");

        if (ride.DriverId != dto.DriverId)
            throw new Exception("Unauthorized driver.");

        if (ride.Status != RideStatus.Accepted)
            throw new Exception("Ride must be accepted first.");

        ride.Status = RideStatus.Started;
        ride.StartedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task CompleteRideAsync(CompleteRideDto dto)
    {
        var ride = await _context.Rides.FindAsync(dto.RideId)
            ?? throw new Exception("Ride not found.");

        if (ride.Status != RideStatus.Started)
            throw new Exception("Ride must be started first.");

        ride.Status = RideStatus.Completed;
        ride.CompletedAt = DateTime.UtcNow;
        ride.Fare = CalculateFare(ride);
        await _context.SaveChangesAsync();
    }

    public async Task CancelRideAsync(CancelRideDto dto)
    {
        var ride = await _context.Rides.FindAsync(dto.RideId)
            ?? throw new Exception("Ride not found.");

        if (ride.Status == RideStatus.Completed)
            throw new Exception("Cannot cancel a completed ride.");

        ride.Status = RideStatus.Cancelled;
        ride.CancellationReason = dto.Reason;
        ride.CancelledAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    private decimal CalculateFare(Ride ride)
    {
        return 5.0m + (decimal)((ride.CompletedAt - ride.StartedAt)?.TotalMinutes ?? 0);
    }
}
