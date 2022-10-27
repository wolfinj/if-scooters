using if_scooters.core.Interfaces;

namespace if_scooters.core.Services;

public class ServiceResult : IServiceResult
{
    public bool Success { get; private set; }

    public ServiceResult(bool succes)
    {
        Success = succes;
        Errors = new List<string>();
    }

    public ServiceResult SetEntity(IEntity entity)
    {
        Entity = entity;
        return this;
    }

    public IEntity Entity { get; private set; }

    public IList<string> Errors { get; set; }

    public ServiceResult AddError(string error)
    {
        Errors.Add(error);
        return this;
    }

    public Exception Exception { get; set; }

    public ServiceResult SetExecption(Exception exception)
    {
        Exception = exception;

        return this;
    }

    public string FormattedErrors => string.Join(",", Errors);
}
