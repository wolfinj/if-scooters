using if_scooters.core.Models;

namespace If_scooters.Tests;

public class ScooterTests
{
    [Fact]
    public void Scooter_CreateNewScooter_CheckIdAndPricePerMinute()
    {
        // Act 
        var scooter = new Scooter(1, 0.17m);

        // Assert
        scooter.Id.Should().Be(1);
        scooter.PricePerMinute.Should().Be(0.17m);
    }

    [Fact]
    public void Scooter_CreateNewScooterAndSetAsRented_ReturnRentedToBeTrue()
    {
        // Act 
        var scooter = new Scooter(1, 0.17m)
        {
            IsRented = true
        };

        // Assert
        scooter.IsRented.Should().BeTrue();
    }
}
