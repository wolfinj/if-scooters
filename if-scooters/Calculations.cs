using if_scooters.Exceptions;

namespace if_scooters;

public static class Calculations
{
    public static decimal CalculateRentBetweenDates(DateTime start, DateTime? end, decimal rate,
        decimal maxCostPerDay = 20)
    {
        var endDate = end ?? DateTime.UtcNow.AddHours(3);

        if (endDate < start)
        {
            throw new EndTimeCantBeLesThanStartTimeException();
        }

        var daysBetween = (decimal)((endDate.Date - start.Date).TotalDays - 1);

        if (daysBetween == -1)
        {
            var price = ReturnMaxOrCap((decimal)(endDate - start).TotalMinutes * rate, maxCostPerDay);
            return Math.Round(price, 2);
        }

        var firstDay = ReturnMaxOrCap((decimal)(1440 - start.TimeOfDay.TotalMinutes) * rate, maxCostPerDay);
        var lastDay = ReturnMaxOrCap((decimal)endDate.TimeOfDay.TotalMinutes * rate, maxCostPerDay);
        var fullDays = ReturnMaxOrCap(1440m * rate, maxCostPerDay) * daysBetween;

        var sum = firstDay + lastDay + fullDays;

        return Math.Round(sum, 2);
    }

    private static decimal ReturnMaxOrCap(decimal actual, decimal max)
    {
        return actual > max ? max : actual;
    }
}
