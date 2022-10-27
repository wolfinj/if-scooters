namespace if_scooters.core.Exceptions;

public class EndTimeCantBeLesThanStartTimeException : Exception
{
    public EndTimeCantBeLesThanStartTimeException() : base("End time can't be les than start time!")
    {
    }
}
