using if_scooters.core.Services;
using Microsoft.AspNetCore.Mvc;

namespace if_scooters.api.Controllers;

[ApiController, Route("api")]
public class CompanyController : Controller
{
    private IRentalCompanyService _company;
    private IScooterService _scooter;

    public CompanyController(IRentalCompanyService company, IScooterService scooter)
    {
        _company = company;
        _scooter = scooter;
    }

    [HttpGet, Route("scooters")]
    public IActionResult GetAllScooters()
    {
        var scooters = _scooter.GetScooters();
        return Ok(scooters);
    }

    [HttpPost, Route("scooter")]
    public IActionResult AddScooter(decimal price)
    {
        var newScooter = _scooter.AddScooter(price);

        if (newScooter.Success)
        {
            var uri = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}/{newScooter.Entity.Id}";

            return Created(uri, newScooter.Entity);
        }

        return Problem(newScooter.FormattedErrors);
    }

    [HttpDelete, Route("scooter/{id:int}")]
    public IActionResult RemoveScooter([FromRoute] int id)
    {
        var result = _scooter.RemoveScooter(id);

        if (result.Success)
        {
            return Ok($"Scooter with id: \"{id}\" deleted.");
        }

        return Problem(result.FormattedErrors);
    }

    [HttpPost, Route("start-rent/{id:int}")]
    public IActionResult StartRent([FromRoute] int id)
    {
        try
        {
            _company.StartRent(id);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }

        return Ok($"Rent for scooter id: \"{id}\" started.");
    }

    [HttpPost, Route("end-rent/{id:int}")]
    public IActionResult EndRent([FromRoute] int id)
    {
        decimal endPrice;

        try
        {
            endPrice = _company.EndRent(id);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }

        return Ok($"Rent ended for scooter id: \"{id}\". Final cost: {endPrice}");
    }

    [HttpPost, Route("yearly-report")]
    public IActionResult CalculateYearlyIncome([FromQuery] int year, [FromQuery] bool includeIncomplete)
    {
        decimal income;
        try
        {
            income = _company.CalculateIncome(year, includeIncomplete);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }

        return Ok(value: income);
    }
}
