using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.Admin;

public interface IDriverLocationService
{
    Task<List<DriverLocationDto>> GetAllDriverLocationsAsync();
    Task<List<DriverLocationDto>> GetAllLocationsAsync();
    Task<DriverLocationDto?> GetDriverLocationByIdAsync(int id);
    Task<List<DriverLocationDto>> GetDriverLocationsByDriverIdAsync(Guid driverId);
    Task<DriverLocationDto> CreateDriverLocationAsync(DriverLocationDto dto);
    Task<DriverLocationDto> UpdateDriverLocationAsync(int id, UpdateDriverLocationDto dto);
    Task UpdateLocationAsync(UpdateDriverLocationDto dto);
    Task DeleteDriverLocationAsync(int id);
}
