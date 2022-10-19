namespace if_scooters;

public class RentedScooter
{
    public string Id;
    public DateTime RentStart;
    public DateTime? RentEnd;
    public decimal PricePerMinute;

    public RentedScooter(string id, DateTime rentStart, decimal pricePerMinute, DateTime? rentEnd = null)
    {
        Id = id;
        RentStart = rentStart;
        PricePerMinute = pricePerMinute;
        RentEnd = rentEnd;
    }
}
