namespace if_scooters.core.Exceptions;

public class InvalidIDException : Exception
{
    public InvalidIDException() : base("ID can not be empty or Null!")
    {
    }
}
