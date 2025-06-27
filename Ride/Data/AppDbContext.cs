using Microsoft.EntityFrameworkCore;
using RideApp.Models;

namespace RideApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Ride> Rides { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
