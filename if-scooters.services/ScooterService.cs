using if_scooters.core.Exceptions;
using if_scooters.core.Models;
using if_scooters.core.Services;
using if_scooters.data;

namespace if_scooters.services;

public class ScooterService : EntityService<Scooter>, IScooterService 
{
    public ScooterService(IScooterDbContext context) : base(context)
    {
    }

    public IServiceResult AddScooter( decimal pricePerMinute)
    {
        try
        {
            if (pricePerMinute <= 0)
            {
                throw new InvalidPriceException(pricePerMinute);
            }
            
            var newScooter = new Scooter(pricePerMinute);
            Context.Scooters.Add(newScooter);
            Context.SaveChanges();

            return new ServiceResult(true).SetEntity(newScooter);
        }
        catch (Exception e)
        {
            
            return new ServiceResult(false).AddError(e.Message).SetExecption(e);
        }
        
    }

    public IServiceResult RemoveScooter(int id)
    {
        try
        {
            var scooter = GetScooterById(id);

            if (scooter.IsRented)
            {
                throw new ScooterIsRentedException(id);
            }

            Context.Scooters.Remove(scooter);
            Context.SaveChanges();

            return new ServiceResult(true);

        }
        catch (Exception e)
        {
            return new ServiceResult(false).AddError(e.Message).SetExecption(e);
        }
    }

    public IList<Scooter> GetScooters()
    {
        return Context.Scooters.ToList();
    }

    public Scooter GetScooterById(int scooterId)
    {
        return Context.Scooters.SingleOrDefault(scooter => scooter.Id == scooterId) 
               ?? throw new ScooterDoesNotExistsException(scooterId);
    }
}
