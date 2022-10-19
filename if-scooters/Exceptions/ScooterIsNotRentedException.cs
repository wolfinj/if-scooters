namespace if_scooters.Exceptions;

public class ScooterIsNotRentedException : Exception
{
    public ScooterIsNotRentedException(string id) : base($"Scooter with id {id} is not rented!")
    {
    }
}
