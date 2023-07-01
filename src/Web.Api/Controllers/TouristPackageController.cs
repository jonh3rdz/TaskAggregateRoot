using Application.TouristPackages.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.TouristPackages.GetAll;
using Application.TouristPackages.Update;
using ErrorOr;
using Application.TouristPackages.GetById;
using Application.TouristPackages.Delete;
using Application.TouristPackages.Search;

namespace Web.Api.Controllers;

[Route("TouristPackage")]
public class TouristPackages : ApiController
{
    private readonly ISender _mediator;

    public TouristPackages(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var touristPackagesResult = await _mediator.Send(new GetAllTouristPackagesQuery());

        return touristPackagesResult.Match(
            TouristPackage => Ok(touristPackagesResult.Value),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTouristPackageCommand command)
    {
        var createTouristPackageResult = await _mediator.Send(command);

        return createTouristPackageResult.Match(
            customer => Ok(),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTouristPackageCommand command)
    {
        if (command.Id != id)
        {
            List<Error> errors = new()
            {
                Error.Validation("TouristPackage.UpdateInvalid", "The request Id does not match with the url Id.")
            };
            return Problem(errors);
        }

        var updateTouristPackageResult = await _mediator.Send(command);

        return updateTouristPackageResult.Match(
            TouristPackageId => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var touristPackageResult = await _mediator.Send(new GetTouristPackageByIdQuery(id));

        return touristPackageResult.Match(
            touristPackage => Ok(touristPackage),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteTouristPackageResult = await _mediator.Send(new DeleteTouristPackageCommand(id));

        return deleteTouristPackageResult.Match(
            ReservationId => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpGet("search")]
    public async Task<IActionResult> Get(string? name, string? description, DateTime? travelDate, decimal? price, string? ubication)
    {
        var searchTouristPackagesResult = await _mediator.Send(new SearchTouristPackagesQuery(name, description, travelDate, price, ubication));

        return searchTouristPackagesResult.Match(
            touristPackages => Ok(touristPackages),
            errors => Problem(errors)
        );
    }

}