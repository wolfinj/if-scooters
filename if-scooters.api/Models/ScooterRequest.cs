namespace if_scooters.api.Models;

public class ScooterRequest
{
    /// <summary>
    /// Create new instance of the scooter.
    /// </summary>
    /// <param name="id">ID of the scooter.</param>
    /// <param name="pricePerMinute">Rental price of the scooter per one minute.</param>
    public ScooterRequest( decimal pricePerMinute)
    {
        PricePerMinute = pricePerMinute;
        IsRented = false;
    }

    public ScooterRequest()
    {
        
    }

    /// <summary>
    /// Unique ID of the scooter.
    /// </summary>

    /// <summary>
    /// Rental price of the scooter per one minute.
    /// </summary>
    public decimal PricePerMinute { get; set; }

    /// <summary>
    /// Identify if someone is renting this scooter.
    /// </summary>
    public bool IsRented { get; set; }
}
