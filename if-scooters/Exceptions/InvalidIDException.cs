namespace if_scooters.Exceptions;

public class InvalidIDException : Exception
{
    public InvalidIDException() : base("ID can not be empty or Null!")
    {
    }
}
