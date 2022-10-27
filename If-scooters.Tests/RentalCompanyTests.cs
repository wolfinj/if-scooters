using if_scooters.core.Exceptions;
using if_scooters.core.Models;
using if_scooters.core.Services;
using if_scooters.services;
using If_scooters.Tests.Infrastructure;
using Moq;

namespace If_scooters.Tests;

public class RentalCompanyTests : IfScootersTestBase
{
    private readonly IRentalCompanyService _companyService;

    private readonly Scooter _defaultScooter;
    private readonly Mock<IScooterService> _scooterServiceMock;

    public RentalCompanyTests()
     {
         _scooterServiceMock = new Mock<IScooterService>();

         _companyService = new RentalCompanyService(_scooterServiceMock.Object, _context);


         var scooters = new[]
         {
             new Scooter(0.2m),
             new Scooter(0.2m),
             new Scooter(0.2m),
             new Scooter(0.2m)
         };
         _context.Scooters.AddRange(scooters);
         _context.SaveChanges();

         _defaultScooter = new Scooter( 0.2m);
     }
     
     [Fact]
     public void StartRentTest()
     {
         // Arrange
         _scooterServiceMock.Setup(s => s.GetScooterById(1)).Returns(_defaultScooter);

         // Act
         _companyService.StartRent(1);

         // Assert
         _defaultScooter.IsRented.Should().BeTrue();

     }
     
     [Fact]
     public void StartRent_NoSetup_Test()
     {
         // Arrange
         _scooterServiceMock.Setup(s => s.GetScooterById(1)).Throws<ScooterDoesNotExistsException>();

         // Act
         Action act = () => _companyService.StartRent(1);

         // Assert
         act.Should().Throw<ScooterDoesNotExistsException>();

     }
     

     [Fact]
     public void StartRent_RentScooter_ScooterIsRented()
     {
         // Arrange
         var scooter = _context.Scooters.First();
         _scooterServiceMock.Setup(s => s.GetScooterById(scooter.Id)).Returns(scooter);
         // Act
         _companyService.StartRent(scooter.Id);

         // Assert
         scooter.IsRented.Should().BeTrue();
     }

     [Fact]
     public void StartRent_RentNonExistingScooter_ThrowScooterDoesNotExistsException()
     {
         // Arrange
         const int id = 10;
         _scooterServiceMock.Setup(s => s.GetScooterById(id)).Throws(new ScooterDoesNotExistsException(id));
         
         // Act
         Action act = () => _companyService.StartRent(id);

         // Assert
         act.Should().Throw<ScooterDoesNotExistsException>().WithMessage("Scooter with id 10 does not exist!");
     }


     [Fact]
     public void StartRent_RentScooterWhichIsAlreadyRented_ThrowScooterIsRentedException()
     {
         //Arrange
         var scooter = _context.Scooters.First();
         _scooterServiceMock.Setup(s => s.GetScooterById(scooter.Id)).Returns(scooter);
         
         _companyService.StartRent(scooter.Id);

         // Act
         Action act = () => _companyService.StartRent(scooter.Id);

         // Assert
         act.Should().Throw<ScooterIsRentedException>().WithMessage($"Scooter with id {scooter.Id} is rented!");
     }

     [Fact]
     public void EndRent_EndRentingScooter_ScooterIsNotRented()
     {
         // Arrange
         var scooter = _context.Scooters.First();
         _scooterServiceMock.Setup(s => s.GetScooterById(scooter.Id)).Returns(scooter);
         
         _companyService.StartRent(scooter.Id);
         _context.RentedScooters.Last().RentStart=DateTime.Now.AddMinutes(-20);
         // Act 
         var act = _companyService.EndRent(scooter.Id);

         // Assert
         scooter.IsRented.Should().BeFalse();
         act.Should().BeGreaterThan(0);
     }

     [Fact]
     public void EndRent_EndRentForScooterWhichIsNotRented_ThrowScooterIsNotRentedException()
     {
         // Act
         Action act = () => _companyService.EndRent(1);

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
         var scooter = _context.Scooters.First();
         _scooterServiceMock.Setup(x => x.GetScooterById(scooter.Id)).Returns(scooter);
         _companyService.StartRent(scooter.Id);
         _context.RentedScooters.Last().RentStart=DateTime.Now.AddMinutes(subtractMin);
     
         // Act
         var rent = _companyService.EndRent(scooter.Id);
     
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

         _context.RentedScooters.AddRange(
             new RentedScooter(1, d1.AddMinutes(-10), 0.2m, d1),
             new RentedScooter(1, d2.AddMinutes(-10), 0.2m, d2),
             new RentedScooter(2, d1.AddMinutes(-10), 0.2m, d1),
             new RentedScooter(3, d2.AddMinutes(-10), 0.2m, d2), 
             new RentedScooter(3, d3.AddMinutes(-10), 0.2m, d3),
             new RentedScooter(3, now.AddMinutes(-10), 0.2m, now));
         _context.SaveChanges();

         // Act
         var income = _companyService.CalculateIncome(year, includeNotCompletedRentals);

         // Assert
         income.Should().Be(expected);
     }

     [Fact]
     public void CalculateIncome_EnterYearInFuture_ThrowInvalidYearException()
     {
         // Act
         Action act = () => _companyService.CalculateIncome(3033, true);

         // Assert
         act.Should().Throw<InvalidYearException>().WithMessage("Year can not be bigger than current year");
     }
     
     
}
