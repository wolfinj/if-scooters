namespace if_scooters.core.Exceptions;

public class ScooterIsRentedException : Exception
{
    public ScooterIsRentedException(int id) : base($"Scooter with id {id} is rented!")
    {
    }
}
