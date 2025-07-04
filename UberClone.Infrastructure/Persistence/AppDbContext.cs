using Microsoft.EntityFrameworkCore;
using UberClone.Domain.Entities;

namespace UberClone.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ride> Rides => Set<Ride>();  //Added by tamar
    public DbSet<Tariff> Tariffs => Set<Tariff>(); //Added by tamar
    public DbSet<PromoCode> PromoCodes => Set<PromoCode>(); //Added by tamar
    public DbSet<DriverLocation> DriverLocations => Set<DriverLocation>(); //Added by tamar
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>(); //Added by tamar
    public DbSet<Transaction> Transactions => Set<Transaction>(); //Added for payment functionality
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>(); //Added by tamar

}
