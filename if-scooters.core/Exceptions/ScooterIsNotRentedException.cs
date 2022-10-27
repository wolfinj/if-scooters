namespace if_scooters.core.Exceptions;

public class ScooterIsNotRentedException : Exception
{
    public ScooterIsNotRentedException(int id) : base($"Scooter with id {id} is not rented!")
    {
    }
}
