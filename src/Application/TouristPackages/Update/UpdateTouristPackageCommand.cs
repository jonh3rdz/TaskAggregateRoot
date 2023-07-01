using Domain.Destinations;
using Application.TouristPackages.Common;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.TouristPackages.Update
{
    public record UpdateTouristPackageCommand(
        Guid Id,
        string Name,
        string Description,
        DateTime TravelDate,
        Money Price,
        List<UpdateLineItemCommand> Items
    ) : IRequest<ErrorOr<Unit>>;

    public record UpdateLineItemCommand(Guid DestinationId);
}