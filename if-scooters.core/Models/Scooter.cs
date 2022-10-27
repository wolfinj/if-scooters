namespace if_scooters.core.Models;

public class Scooter : Entity
{
    /// <summary>
    /// Create new instance of the scooter.
    /// </summary>
    /// <param name="id">ID of the scooter.</param>
    /// <param name="pricePerMinute">Rental price of the scooter per one minute.</param>
    public Scooter(int id, decimal pricePerMinute)
    {
        Id = id;
        PricePerMinute = pricePerMinute;
        IsRented = false;
    }

    public Scooter()
    {
    }

    public Scooter(decimal pricePerMinute)
    {
        PricePerMinute = pricePerMinute;
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
