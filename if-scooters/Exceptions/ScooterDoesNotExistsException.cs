namespace if_scooters.Exceptions;

public class ScooterDoesNotExistsException : Exception
{
    public ScooterDoesNotExistsException(string id) : base($"Scooter with id {id} does not exist!")
    {
    }
    public ScooterDoesNotExistsException() : base("Scooter with this id does not exist!")
    {
    }
}
