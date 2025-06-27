using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RideApp.Data;
using RideApp.Hubs;
using RideApp.Models;
using RideApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace RideApp.Controllers
{
    [ApiController]
    [Route("api")]
    public class RideController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<RideHub> _hub;

        public RideController(AppDbContext context, IHubContext<RideHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        [HttpPost("request-ride")]
        public async Task<IActionResult> RequestRide([FromBody] Ride ride)
        {
            ride.Status = "Requested";
            ride.CreatedAt = DateTime.UtcNow;
            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();

            var driver = DriverAssigner.AssignDriver();
            ride.DriverId = driver.Id;
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("RideStatusUpdated", ride.RideId.ToString(), ride.Status);

            return Ok(new
            {
                ride.RideId,
                Driver = driver,
                EstimatedArrival = "5 minutes"
            });
        }

        [HttpPost("accept-ride")]
        public async Task<IActionResult> AcceptRide([FromBody] AcceptRideDto dto)
        {
            var ride = await _context.Rides.FindAsync(dto.RideId);
            if (ride == null || ride.Status != "Requested")
                return BadRequest("Invalid ride");

            ride.DriverId = dto.DriverId;
            ride.Status = "Accepted";
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("RideStatusUpdated", dto.RideId.ToString(), "Accepted");

            return Ok(ride);
        }

        [HttpPost("complete-ride")]
        public async Task<IActionResult> CompleteRide([FromBody] int rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null) return NotFound();

            ride.Status = "Completed";
            ride.CompletedAt = System.DateTime.UtcNow;
            ride.Fare = FareCalculator.Calculate(ride);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("RideStatusUpdated", rideId.ToString(), "Completed");

            return Ok(new { ride.Fare, ride.CompletedAt });
        }

        [HttpGet("ride-history/{passengerId}")]
        public IActionResult RideHistory(int passengerId)
        {
            var rides = _context.Rides
                .Where(r => r.PassengerId == passengerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return Ok(rides);
        }

        [HttpPost("cancel-ride/{id}")]
        public async Task<IActionResult> CancelRide(int id)
        {
            var ride = await _context.Rides.FindAsync(id);
            if (ride == null) return NotFound();

            ride.Status = "Cancelled";
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("RideStatusUpdated", id.ToString(), "Cancelled");

            return Ok();
        }
    }
}
