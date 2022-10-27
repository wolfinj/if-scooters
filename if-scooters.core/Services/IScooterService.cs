using if_scooters.core.Models;

namespace if_scooters.core.Services;

public interface IScooterService
{
    /// <summary>
    /// Add scooter.
    /// </summary>
    /// <param name="id">Unique ID of the scooter</param>
    /// <param name="pricePerMinute">Rental price of the scooter per one minute</param>
    IServiceResult AddScooter( decimal pricePerMinute);

    /// <summary>
    /// Remove scooter. This action is not allowed for scooters if the rental is in progress.
    /// </summary>
    /// <param name="id">Unique ID of the scooter</param>
    IServiceResult RemoveScooter(int id);

    /// <summary>
    /// List of scooters that belong to the company.
    /// </summary>
    /// <returns>Return a list of available scooters.</returns>
    IList<Scooter> GetScooters();

    /// <summary>
    /// Get particular scooter by ID.
    /// </summary>
    /// <param name="scooterId">Unique ID of the scooter.</param>
    /// <returns>Return a particular scooter.</returns>
    Scooter GetScooterById(int scooterId);
}
