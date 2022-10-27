using if_scooters.data;
using Microsoft.EntityFrameworkCore;

namespace If_scooters.Tests.Infrastructure;

public class IfScootersTestBase :IDisposable
{
    protected readonly ScooterDbContext _context;

    public IfScootersTestBase()
    {
        var options = new DbContextOptionsBuilder<ScooterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ScooterDbContext(options);

        _context.Database.EnsureCreated();

    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();

        _context.Dispose();
    }
}
