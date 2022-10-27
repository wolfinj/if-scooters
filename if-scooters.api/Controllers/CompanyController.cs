using if_scooters.core.Models;
using if_scooters.core.Services;
using Microsoft.AspNetCore.Mvc;

namespace if_scooters.api.Controllers;

[ApiController, Route("api")]
public class CompanyController : Controller
{
    // private IEntityService<Scooter> _scooters;
    private IRentalCompanyService _company;
    private IScooterService _scooter;

    // public CompanyController(IEntityService<Scooter> context)
    // {
    //     _scooters = context;
    // }

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
}
