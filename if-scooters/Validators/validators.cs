using if_scooters.Exceptions;

namespace if_scooters.Validators;

public static class Validator
{
    public static void ScooterIdValidator(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new InvalidIDException();
        }
    }

    public static Scooter ReturnsScooterById(string id, List<Scooter> scooters)
    {
        var scooter = scooters.FirstOrDefault(scooter => scooter.Id == id);

        if (scooter == null)
        {
            throw new ScooterDoesNotExistsException(id);
        }

        return scooter;
    }
}
