using if_scooters.Exceptions;
using static if_scooters.Calculations;

namespace if_scooters;

public class RentalCompany : IRentalCompany
{
    private readonly List<RentedScooter> _rentalHistory;

    private readonly IScooterService _scooterService;

    public string Name { get; }

    public RentalCompany(string name, IScooterService scooterService)
    {
        Name = name;
        _scooterService = scooterService;
        _rentalHistory = new List<RentedScooter>();
    }

    public RentalCompany(string name, IScooterService scooterService, List<RentedScooter> rentalHistory)
    {
        Name = name;
        _scooterService = scooterService;
        _rentalHistory = rentalHistory;
    }

    public void StartRent(string id)
    {
        var scooter = _scooterService.GetScooterById(id);

        if (scooter.IsRented)
        {
            throw new ScooterIsRentedException(id);
        }

        scooter.IsRented = true;

        _rentalHistory.Add(new RentedScooter(scooter.Id, DateTime.UtcNow.AddHours(3), scooter.PricePerMinute));
    }

    public decimal EndRent(string id)
    {
        var scooter = _scooterService.GetScooterById(id);

        var rentedScooter = _rentalHistory.FirstOrDefault(scoot => scoot.Id == id && !scoot.RentEnd.HasValue);

        if (rentedScooter is null)
        {
            throw new ScooterIsNotRentedException(id);
        }

        rentedScooter.RentEnd = DateTime.UtcNow.AddHours(3);

        scooter.IsRented = false;

        return CalculateRentBetweenDates(rentedScooter.RentStart, rentedScooter.RentEnd, rentedScooter.PricePerMinute);
    }

    public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
    {
        IList<RentedScooter> rentsOfYear;

        if (year.HasValue)
        {
            if (year > DateTime.Now.Year)
            {
                throw new InvalidYearException();
            }

            if (includeNotCompletedRentals)
            {
                rentsOfYear = _rentalHistory
                    .Where(rental =>
                        rental.RentEnd?.Year == year
                        ||
                        rental.RentEnd == null)
                    .ToList();
            }
            else
            {
                rentsOfYear = _rentalHistory
                    .Where(rental => rental.RentEnd?.Year == year)
                    .ToList();
            }
        }
        else
        {
            rentsOfYear = _rentalHistory;
        }

        var income = rentsOfYear
            .Aggregate(0m, (income, rental) =>
                income += CalculateRentBetweenDates
                (
                    rental.RentStart,
                    rental.RentEnd,
                    rental.PricePerMinute
                ));

        return income;
    }
}
