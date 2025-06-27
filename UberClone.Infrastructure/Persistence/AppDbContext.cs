using Microsoft.EntityFrameworkCore;
using UberClone.Domain.Entities;

namespace UberClone.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
}