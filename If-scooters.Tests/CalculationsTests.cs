using if_scooters.core.Exceptions;
using static if_scooters.services.Calculations;

namespace If_scooters.Tests;

public class CalculationsTests
{
    private readonly decimal _max = 20;
    private readonly decimal _rate = 0.2m;
    private readonly DateTime _d1 = new(2020, 6, 1, 1, 0, 0);
    private readonly DateTime _d2 = new(2021, 6, 1, 1, 0, 0);
    private readonly DateTime _now = DateTime.UtcNow.AddHours(3);

    [Fact]
    public void CalculateRentBetweenDates_GiveDifferenceLessThanHour_ReturnCorrectPrice()
    {
        // Act
        var price = CalculateRentBetweenDates(_d1.AddMinutes(-10), _d1, _rate, _max);
        // Assert
        price.Should().Be(2);
    }

    [Fact]
    public void CalculateRentBetweenDates_GiveDifferenceTwoHour_ReturnCorrectPrice()
    {
        // Act
        var price = CalculateRentBetweenDates(_d1, _d1.AddHours(2), _rate, _max);
        // Assert
        price.Should().Be(20);
    }

    [Fact]
    public void CalculateRentBetweenDates_GiveDifferenceBetweenTwoDays_ReturnCorrectPrice()
    {
        // Act
        var price = CalculateRentBetweenDates(_d1.AddHours(-3), _d1, _rate, _max);
        // Assert
        price.Should().Be(32);
    }

    [Fact]
    public void CalculateRentBetweenDates_GiveDifferenceBetweenFiveDays_ReturnCorrectPrice()
    {
        // Act
        var price = CalculateRentBetweenDates(_d1.AddDays(-5), _d1, _rate, _max);
        // Assert
        price.Should().Be(112);
    }

    [Fact]
    public void CalculateRentBetweenDates_GiveDifferenceFromNowAndTenMinutesBefore_ReturnCorrectPrice()
    {
        // Act
        var price = CalculateRentBetweenDates(_now.AddMinutes(-10), null, _rate, _max);
        // Assert
        price.Should().Be(2);
    }

    [Fact]
    public void CalculateRentBetweenDates_EndTimeIsLessThanStartTime_ThrowEndTimeCantBeLesThanStartTimeException()
    {
        // Act
        Action act = () => CalculateRentBetweenDates(_d2, _d1, _rate, _max);

        // Assert
        act.Should()
            .Throw<EndTimeCantBeLesThanStartTimeException>()
            .WithMessage("End time can't be les than start time!");
    }
}
