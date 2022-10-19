namespace if_scooters.Exceptions;

public class InvalidYearException : Exception
{
    public InvalidYearException() : base("Year can not be bigger than current year")
    {
    }
}
