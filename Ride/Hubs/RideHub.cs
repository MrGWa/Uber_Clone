using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RideApp.Hubs
{
    public class RideHub : Hub
    {
        public async Task UpdateRideStatus(string rideId, string status)
        {
            await Clients.All.SendAsync("RideStatusUpdated", rideId, status);
        }
    }
}
