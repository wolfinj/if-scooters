namespace if_scooters.Exceptions;

public class InvalidPriceException : Exception
{
    public InvalidPriceException(decimal price) : base($"Given price {price} is not valid!")
    {
    }
}
