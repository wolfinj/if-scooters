using if_scooters.core.Exceptions;
using if_scooters.core.Models;
using if_scooters.core.Services;
using if_scooters.services;
using If_scooters.Tests.Infrastructure;

namespace If_scooters.Tests;

public class ScooterServiceTests : IfScootersTestBase
{
    private readonly IScooterService _scooterService;

    public ScooterServiceTests()
    {
        _scooterService = new ScooterService(_context);
    }

    [Fact]
    public void AddScooter_AddValidShooter_ScooterAdded()
    {
        // Act 
        var act = _scooterService.AddScooter(0.2m);

        // Assert
        act.Success.Should().BeTrue();
    }

    [Fact]
    public void AddScooter_AddScooterWithPriceZeroOrLess_ShouldBeFalseWithInvalidPriceException()
    {
        // Act 
        var act = _scooterService.AddScooter(-0.2m);

        // Assert
        act.Success.Should().BeFalse();
        act.Exception.Should().BeOfType<InvalidPriceException>();
    }

    [Fact]
    public void RemoveScooter_ScooterExists_ScooterIsRemoved()
    {
        // Arrange
        var scooter = _scooterService.AddScooter(0.2m);

        // Act 
        var act = _scooterService.RemoveScooter(scooter.Entity.Id);

        // Assert
        _context.Scooters.Count().Should().Be(0);
    }

    [Fact]
    public void RemoveScooter_ScooterDoesNotExist_ShouldBeFalseWithScooterDoesNotExistException()
    {
        // Act 
        var act = _scooterService.RemoveScooter(1);

        // Assert
        act.Success.Should().BeFalse();
        act.Exception.Should().BeOfType<ScooterDoesNotExistsException>();
    }

    [Fact]
    public void RemoveScooter_ScooterIsRented_ShouldBeFalseWithScooterIsRentedException()
    {
        // Arrange
        var scooter = new Scooter(0.2m)
        {
            IsRented = true
        };
        _context.Scooters.Add(scooter);
        _context.SaveChanges();

        // Act 
        var act = _scooterService.RemoveScooter(scooter.Id);

        // Assert
        act.Success.Should().BeFalse();
        act.Exception.Should().BeOfType<ScooterIsRentedException>();
    }

    [Fact]
    public void GetScooters_ReturnListOfScooters_ListWithCorrectCount()
    {
        // Arrange
        var scooter = new Scooter(0.2m);
        var scooter2 = new Scooter(0.2m);

        _context.Scooters.Add(scooter);
        _context.Scooters.Add(scooter2);
        _context.SaveChanges();

        // Act 
        var act = _scooterService.GetScooters();

        // Assert
        act.Count.Should().Be(2);
    }

    [Fact]
    public void GetScooters_ReturnEmptyList_ListIsEmpty()
    {
        // Assert
        _scooterService.GetScooters().Should().BeEmpty();
    }

    [Fact]
    public void GetScooterById_ReturnExistingScooterById_ScooterIsReturned()
    {
        // Arrange
        var scooter = new Scooter(0.2m);
        _context.Scooters.Add(scooter);
        _context.SaveChanges();

        // Act 
        var act = _scooterService.GetScooterById(scooter.Id);

        // Assert
        act.Should().BeSameAs(scooter);
    }

    [Fact]
    public void GetScooterById_ReturnNonExistingScooterById_ThrowScooterDoesNotExistsException()
    {
        // Act 
        Action act = () => _scooterService.GetScooterById(1);

        // Assert
        act.Should().Throw<ScooterDoesNotExistsException>().WithMessage("Scooter with id 1 does not exist!");
    }
}
