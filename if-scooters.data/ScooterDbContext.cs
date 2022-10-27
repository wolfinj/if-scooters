using if_scooters.core.Models;
using Microsoft.EntityFrameworkCore;

namespace if_scooters.data;

public class ScooterDbContext : DbContext, IScooterDbContext
{
    public DbSet<Scooter> Scooters { get; set; }
    public DbSet<RentedScooter> RentedScooters { get; set; }

    public ScooterDbContext(DbContextOptions<ScooterDbContext> options) : base(options)
    {
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}
