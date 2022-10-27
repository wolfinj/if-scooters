namespace if_scooters.core.Models;

public class RentedScooter : Entity
{
    public DateTime RentStart {get;set;}
    public DateTime? RentEnd {get;set;}
    public decimal PricePerMinute {get;set;}
    public int ScooterId {get;set;}

    public RentedScooter()
    {
        
    }

    public RentedScooter(int scooterId, DateTime rentStart, decimal pricePerMinute, DateTime? rentEnd = null)
    {
        ScooterId = scooterId;
        RentStart = rentStart;
        PricePerMinute = pricePerMinute;
        RentEnd = rentEnd;
    }
}
