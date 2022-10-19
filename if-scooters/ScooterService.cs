using if_scooters.Exceptions;
using if_scooters.Validators;

namespace if_scooters;

public class ScooterService : IScooterService
{
    private readonly List<Scooter> _scooters;

    public ScooterService(List<Scooter> inventory)
    {
        _scooters = inventory;
    }

    public void AddScooter(string id, decimal pricePerMinute)
    {
        if (_scooters.Any(scooter => scooter.Id == id))
        {
            throw new DuplicateScooterException(id);
        }

        if (pricePerMinute <= 0)
        {
            throw new InvalidPriceException(pricePerMinute);
        }

        Validator.ScooterIdValidator(id);

        _scooters.Add(new Scooter(id, pricePerMinute));
    }

    public void RemoveScooter(string id)
    {
        Validator.ScooterIdValidator(id);

        var scooter = Validator.ReturnsScooterById(id, _scooters);

        if (scooter.IsRented)
        {
            throw new ScooterIsRentedException(id);
        }

        _scooters.Remove(scooter);
    }

    public IList<Scooter> GetScooters()
    {
        return _scooters.ToList();
    }

    public Scooter GetScooterById(string scooterId)
    {
        Validator.ScooterIdValidator(scooterId);

        var scooter = Validator.ReturnsScooterById(scooterId, _scooters);

        return scooter;
    }
}
