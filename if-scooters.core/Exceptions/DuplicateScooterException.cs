namespace if_scooters.core.Exceptions;

public class DuplicateScooterException : Exception
{
    public DuplicateScooterException(int id) : base($"Scooter with id {id} already exists!")
    {
    }
}
