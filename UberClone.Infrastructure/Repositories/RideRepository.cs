using Microsoft.EntityFrameworkCore;
using UberClone.Application.Interfaces;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;

namespace UberClone.Infrastructure.Repositories
{
    public class RideRepository : IRideRepository
    {
        private readonly AppDbContext _context;

        public RideRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ride?> GetRideByIdAsync(Guid rideId)
        {
            return await _context.Rides.FirstOrDefaultAsync(r => r.Id == rideId);
        }

        public async Task<List<Ride>> GetAllRidesAsync()
        {
            return await _context.Rides.ToListAsync();
        }

        public async Task<Ride> CreateRideAsync(Ride ride)
        {
            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        public async Task UpdateRideAsync(Ride ride)
        {
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRideAsync(Guid rideId)
        {
            var ride = await GetRideByIdAsync(rideId);
            if (ride != null)
            {
                _context.Rides.Remove(ride);
                await _context.SaveChangesAsync();
            }
        }
    }
}
