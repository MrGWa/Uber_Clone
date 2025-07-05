//Added by tamar
using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class DriverLocationService : IDriverLocationService
{
    private readonly AppDbContext _context;

    public DriverLocationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DriverLocationDto>> GetAllDriverLocationsAsync()
    {
        return await GetAllLocationsAsync();
    }

    public async Task<DriverLocationDto?> GetDriverLocationByIdAsync(int id)
    {
        var location = await _context.DriverLocations.FindAsync(id);
        if (location == null) return null;

        return new DriverLocationDto
        {
            DriverId = location.DriverId,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            UpdatedAt = location.UpdatedAt
        };
    }

    public async Task<List<DriverLocationDto>> GetDriverLocationsByDriverIdAsync(Guid driverId)
    {
        return await _context.DriverLocations
            .Where(l => l.DriverId == driverId)
            .Select(l => new DriverLocationDto
            {
                DriverId = l.DriverId,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                UpdatedAt = l.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<DriverLocationDto> CreateDriverLocationAsync(DriverLocationDto dto)
    {
        var location = new DriverLocation
        {
            DriverId = dto.DriverId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };

        _context.DriverLocations.Add(location);
        await _context.SaveChangesAsync();

        return new DriverLocationDto
        {
            DriverId = location.DriverId,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            UpdatedAt = location.UpdatedAt
        };
    }

    public async Task<DriverLocationDto> UpdateDriverLocationAsync(int id, UpdateDriverLocationDto dto)
    {
        var location = await _context.DriverLocations.FindAsync(id);
        if (location == null) throw new Exception("Driver location not found.");

        location.Latitude = dto.Latitude;
        location.Longitude = dto.Longitude;
        location.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new DriverLocationDto
        {
            DriverId = location.DriverId,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            UpdatedAt = location.UpdatedAt
        };
    }

    public async Task DeleteDriverLocationAsync(int id)
    {
        var location = await _context.DriverLocations.FindAsync(id);
        if (location == null) throw new Exception("Driver location not found.");

        _context.DriverLocations.Remove(location);
        await _context.SaveChangesAsync();
    }

    // Keep existing methods for backwards compatibility
    public async Task UpdateLocationAsync(UpdateDriverLocationDto dto)
    {
        var location = await _context.DriverLocations
            .FirstOrDefaultAsync(l => l.DriverId == dto.DriverId);

        if (location == null)
        {
            location = new DriverLocation
            {
                DriverId = dto.DriverId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };
            _context.DriverLocations.Add(location);
        }
        else
        {
            location.Latitude = dto.Latitude;
            location.Longitude = dto.Longitude;
            location.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<DriverLocationDto>> GetAllLocationsAsync()
    {
        return await _context.DriverLocations
            .Select(l => new DriverLocationDto
            {
                DriverId = l.DriverId,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                UpdatedAt = l.UpdatedAt
            })
            .ToListAsync();
    }
}
