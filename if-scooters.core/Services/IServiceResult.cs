using FlightPlanner.Core.Interfaces;

namespace if_scooters.core.Services;

public interface IServiceResult
{
    bool Success { get; }
    IEntity Entity { get; }
    IList<string> Errors { get; set; }
    string FormattedErrors { get; }
    ServiceResult SetEntity(IEntity entity);
    ServiceResult AddError(string error);
    Exception Exception { get; set; }
    ServiceResult SetExecption(Exception exception);
}
