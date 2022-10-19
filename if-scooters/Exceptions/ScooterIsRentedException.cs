namespace if_scooters.Exceptions;

public class ScooterIsRentedException : Exception
{
    public ScooterIsRentedException(string id) : base($"Scooter with id {id} is rented!")
    {
    }
}
