namespace if_scooters.core.Exceptions;

public class ScooterDoesNotExistsException : Exception
{
    public ScooterDoesNotExistsException(int id) : base($"Scooter with id {id} does not exist!")
    {
    }
    public ScooterDoesNotExistsException() : base("Scooter with this id does not exist!")
    {
    }
}
