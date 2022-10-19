using if_scooters;
using if_scooters.Exceptions;

namespace If_scooters.Tests;

public class ScooterServiceTests
{
    private readonly IScooterService _scooterService;
    private readonly List<Scooter> _inventory;

    public ScooterServiceTests()
    {
        _inventory = new List<Scooter>();
        _scooterService = new ScooterService(_inventory);
    }

    [Fact]
    public void AddScooter_AddValidShooter_ScooterAdded()
    {
        // Act 
        _scooterService.AddScooter("1", 0.2m);

        // Assert
        _inventory.Count.Should().Be(1);
    }

    [Fact]
    public void AddScooter_AddValidShooter_NewScooterIsInList()
    {
        // Act 
        _scooterService.AddScooter("1", 0.2m);
        
        // Assert
        _inventory.Last().Should().BeEquivalentTo(new Scooter("1", 0.2m));
    }

    [Fact]
    public void AddScooter_AddValidShooterTwice_ThrowsDuplicateScooterException()
    {
        // Arrange
        _scooterService.AddScooter("1", 0.2m);

        // Act 
        Action act = () => _scooterService.AddScooter("1", 0.2m);

        // Assert
        act.Should().Throw<DuplicateScooterException>().WithMessage("Scooter with id 1 already exists!");
    }

    [Fact]
    public void AddScooter_AddScooterWithPriceZeroOrLess_ThrowInvalidPriceException()
    {
        // Act 
        Action act = () => _scooterService.AddScooter("1", -0.2m);

        // Assert
        act.Should().Throw<InvalidPriceException>().WithMessage("Given price -0.2 is not valid!");
    }

    [Fact]
    public void AddScooter_AddScooterWithNullOrEmptyID_ThrowInvalidIDException()
    {
        // Act 
        Action act = () => _scooterService.AddScooter("", 0.2m);

        // Assert
        act.Should().Throw<InvalidIDException>().WithMessage("ID can not be empty or Null!");
    }

    [Fact]
    public void RemoveScooter_ScooterExists_ScooterIsRemoved()
    {
        // Arrange
        _inventory.Add(new Scooter("1", 0.2m));
        
        // Act 
        _scooterService.RemoveScooter("1");

        // Assert
        _inventory.Count.Should().Be(0);
    }

    [Fact]
    public void RemoveScooter_ScooterDoesNotExist_ThrowsScooterDoesNotExistException()
    {
        // Act 
        Action act = () => _scooterService.RemoveScooter("1");

        // Assert
        act.Should().Throw<ScooterDoesNotExistsException>().WithMessage("Scooter with id 1 does not exist!");
    }

    [Fact]
    public void RemoveScooter_ScooterIsRented_ThrowsScooterIsRentedException()
    {
        // Arrange
        _inventory.Add(new Scooter("1", 0.2m));
        _inventory[0].IsRented = true;

        // Act 
        Action act = () => _scooterService.RemoveScooter("1");

        // Assert
        act.Should().Throw<ScooterIsRentedException>().WithMessage("Scooter with id 1 is rented!");
    }

    [Fact]
    public void GetScooters_ReturnListOfScooters_ListIsCorrect()
    {
        // Act 
        _inventory.Add(new Scooter("1", 0.2m));
        _inventory.Add(new Scooter("2", 0.2m));

        // Assert
        _scooterService.GetScooters().Should().BeEquivalentTo(_inventory);
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
        // Act 
        _inventory.Add(new Scooter("2", 0.2m));

        // Assert
        _scooterService.GetScooterById("2").Should().BeSameAs(_inventory[0]);
    }

    [Fact]
    public void GetScooterById_ReturnNonExistingScooterById_ThrowScooterDoesNotExistsException()
    {
        // Act 
        Action act = () => _scooterService.GetScooterById("1");
        
        // Assert
        act.Should().Throw<ScooterDoesNotExistsException>().WithMessage("Scooter with id 1 does not exist!");
    }

    [Fact]
    public void GetScooterById_ReturnEmptyId_ThrowInvalidIDException()
    {
        // Act 
        Action act = () => _scooterService.GetScooterById("");
        
        // Assert
        act.Should().Throw<InvalidIDException>().WithMessage("ID can not be empty or Null!");
    }
}
