using if_scooters.core.Exceptions;
using if_scooters.core.Models;
using if_scooters.core.Services;
using if_scooters.data;
using static if_scooters.services.Calculations;

namespace if_scooters.services;

public class RentalCompanyService : IRentalCompanyService
{
    private readonly IScooterDbContext _dbContext;

    private readonly IScooterService _scooterService;

    public string Name { get; }

    public RentalCompanyService(IScooterService scooterService, IScooterDbContext context)
    {
        _scooterService = scooterService;
        _dbContext = context;
    }

    public void StartRent(int id)
    {
        var scooter = _scooterService.GetScooterById(id);

        if (scooter.IsRented)
        {
            throw new ScooterIsRentedException(id);
        }

        scooter.IsRented = true;

        _dbContext.RentedScooters.Add(
            new RentedScooter(scooter.Id, DateTime.UtcNow.AddHours(3), scooter.PricePerMinute));
        _dbContext.SaveChanges();
    }

    public decimal EndRent(int id)
    {
        var scooter = _scooterService.GetScooterById(id);

        var rentedScooters = _dbContext.RentedScooters.Where(scoot => scoot.ScooterId == id);


        if (!rentedScooters.Any())
        {
            throw new ScooterIsNotRentedException(id);
        }

        var rentedScooter = rentedScooters.OrderBy(s => s.RentStart).Last();

        rentedScooter.RentEnd = DateTime.UtcNow.AddHours(3);

        scooter.IsRented = false;
        _dbContext.SaveChanges();

        return CalculateRentBetweenDates(rentedScooter.RentStart, rentedScooter.RentEnd, rentedScooter.PricePerMinute);
    }

    public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
    {
        IList<RentedScooter> rentsOfYear;

        if (year.HasValue && year is not 0)
        {
            if (year > DateTime.Now.Year)
            {
                throw new InvalidYearException();
            }

            if (includeNotCompletedRentals)
            {
                rentsOfYear = _dbContext.RentedScooters
                    .Where(rental =>
                        rental.RentEnd == null ||
                        rental.RentEnd.Value.Year == year)
                    .ToList();
            }
            else
            {
                rentsOfYear = _dbContext.RentedScooters
                    .Where(rental => rental.RentEnd!.Value.Year == year)
                    .ToList();
            }
        }
        else
        {
            rentsOfYear = _dbContext.RentedScooters.ToList();
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
