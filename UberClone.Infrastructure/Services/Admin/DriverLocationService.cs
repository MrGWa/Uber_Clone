//Added by tamar
using UberClone.Application.DTOs.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class DriverLocationService
{
    private readonly AppDbContext _context;

    public DriverLocationService(AppDbContext context)
    {
        _context = context;
    }

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
