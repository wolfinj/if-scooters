using if_scooters;
using if_scooters.Exceptions;
using Moq;
using Moq.AutoMock;

namespace If_scooters.Tests;

public class RentalCompanyTests
{
    private readonly IRentalCompany _staavdraazis;
    private readonly List<RentedScooter> _rentedScooters;
    private readonly List<Scooter> _inventory;
    private readonly IScooterService _scooterService;
    private readonly string _companyName;
    // Mocker
    private IRentalCompany _company;
    private AutoMocker _mocker;
    private Mock<IScooterService> _scooterServiceMock;
    private Scooter _defaultScooter;


    public RentalCompanyTests()
    {
        _companyName = "Stāvdrāzis";
        _inventory = new List<Scooter>();
        _scooterService = new ScooterService(_inventory);
        _rentedScooters = new List<RentedScooter>();

        _staavdraazis = new RentalCompany(_companyName, _scooterService, _rentedScooters);

        _inventory.Add(new Scooter("1", 0.2m));
        _inventory.Add(new Scooter("2", 0.2m));
        _inventory.Add(new Scooter("3", 0.2m));
        _inventory.Add(new Scooter("4", 0.2m));
        
        //Mocker 
        _defaultScooter = new Scooter("1", 0.2m);
        _mocker = new AutoMocker();
        _scooterServiceMock = _mocker.GetMock<IScooterService>();
        _company = new RentalCompany("insurance",_scooterServiceMock.Object);
    }
    
    //Mocker
    [Fact]
    public void StartRentTest()
    {
        // Arrange
        _scooterServiceMock.Setup(s => s.GetScooterById("1")).Returns(_defaultScooter);

        // Act
        _company.StartRent("1");

        // Assert
        _defaultScooter.IsRented.Should().BeTrue();

    }
    
    [Fact]
    public void StartRent_NoSetup_Test()
    {
        // Arrange
        _scooterServiceMock.Setup(s => s.GetScooterById("1")).Throws<ScooterDoesNotExistsException>();

        // Act
        Action act = () => _company.StartRent("1");

        // Assert
        act.Should().Throw<ScooterDoesNotExistsException>();

    }
    
    //Mocker end

    [Fact]
    public void RentalCompany_CompanyIsCreated_ReturnName()
    {
        // Arrange
        var testCompany = new RentalCompany(_companyName, _scooterService);

        // Assert
        testCompany.Name.Should().Be(_companyName);
    }

    [Fact]
    public void StartRent_RentScooter_ScooterIsRented()
    {
        // Act
        _staavdraazis.StartRent("1");

        // Assert
        _inventory[1].IsRented.Should().BeFalse();
    }

    [Fact]
    public void StartRent_RentNonExistingScooter_ThrowScooterDoesNotExistsException()
    {
        // Act
        Action act = () => _staavdraazis.StartRent("10");

        // Assert
        act.Should().Throw<ScooterDoesNotExistsException>().WithMessage("Scooter with id 10 does not exist!");
    }

    [Fact]
    public void StartRent_RentScooterWithInvalidID_ThrowInvalidIDException()
    {
        // Act
        Action act = () => _staavdraazis.StartRent(null);

        // Assert
        act.Should().Throw<InvalidIDException>().WithMessage("ID can not be empty or Null!");
    }

    [Fact]
    public void StartRent_RentScooterWhichIsAlreadyRented_ThrowScooterIsRentedException()
    {
        //Arrange
        _staavdraazis.StartRent("1");

        // Act
        Action act = () => _staavdraazis.StartRent("1");

        // Assert
        act.Should().Throw<ScooterIsRentedException>().WithMessage("Scooter with id 1 is rented!");
    }

    [Fact]
    public void EndRent_EndRentingScooter_ScooterIsNotRented()
    {
        // Arrange
        var testScooter = _scooterService.GetScooterById("2");
        testScooter.IsRented = true;
        var testRentedScooter = new RentedScooter("2", DateTime.Now, 0.2M);
        _rentedScooters.Add(testRentedScooter);

        // Act 
        _staavdraazis.EndRent("2");

        // Assert
        testScooter.IsRented.Should().BeFalse();
        testRentedScooter.RentEnd.Should().HaveValue();
    }

    [Fact]
    public void EndRent_EndRentForScooterWhichIsNotRented_ThrowScooterIsNotRentedException()
    {
        // Act
        Action act = () => _staavdraazis.EndRent("1");

        // Assert
        act.Should().Throw<ScooterIsNotRentedException>().WithMessage("Scooter with id 1 is not rented!");
    }

    [Theory]
    [InlineData(-10, 2)]
    [InlineData(-100, 20)]
    [InlineData(-30, 6)]
    [InlineData(-320, 20)]
    public void EndRent_ScooterRentEnded_ReturnExpectedPrice(int subtractMin, decimal expected)
    {
        // Arrange
        _scooterService.AddScooter("r", 0.2m);
        _rentedScooters.Add(new RentedScooter("r", DateTime.UtcNow.AddHours(3).AddMinutes(subtractMin), 0.2m));

        // Act
        var rent = _staavdraazis.EndRent("r");

        // Assert
        rent.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, true, 12)]
    [InlineData(2020, true, 4)]
    [InlineData(2022, false, 4)]
    [InlineData(1984, false, 0)]
    public void CalculateIncome_ReturnExpectedPrice(int? year, bool includeNotCompletedRentals, decimal expected)
    {
        // Arrange
        var d1 = new DateTime(2020, 6, 1, 1, 0, 0);
        var d2 = new DateTime(2021, 6, 1, 1, 0, 0);
        var d3 = new DateTime(2022, 6, 1, 1, 0, 0);
        var now = DateTime.UtcNow.AddHours(3);

        _rentedScooters.Add(new("1", d1.AddMinutes(-10), 0.2m, d1));
        _rentedScooters.Add(new("1", d2.AddMinutes(-10), 0.2m, d2));
        _rentedScooters.Add(new("2", d1.AddMinutes(-10), 0.2m, d1));
        _rentedScooters.Add(new("3", d2.AddMinutes(-10), 0.2m, d2));
        _rentedScooters.Add(new("3", d3.AddMinutes(-10), 0.2m, d3));
        _rentedScooters.Add(new("3", now.AddMinutes(-10), 0.2m, now));

        // Act
        var income = _staavdraazis.CalculateIncome(year, includeNotCompletedRentals);

        // Assert
        income.Should().Be(expected);
    }

    [Fact]
    public void CalculateIncome_EnterYearInFuture_ThrowInvalidYearException()
    {
        // Act
        Action act = () => _staavdraazis.CalculateIncome(3033, true);

        // Assert
        act.Should().Throw<InvalidYearException>().WithMessage("Year can not be bigger than current year");
    }
    
    
}
