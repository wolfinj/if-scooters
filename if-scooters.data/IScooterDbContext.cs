using if_scooters.core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace if_scooters.data;

public interface IScooterDbContext
{
    DbSet<Scooter> Scooters { get; set; }
    DbSet<RentedScooter> RentedScooters { get; set; }

    DbSet<T> Set<T>() where T : class;
    EntityEntry<T> Entry<T>(T entity) where T : class;

    int SaveChanges();
    Task<int> SaveChangesAsync();
}
