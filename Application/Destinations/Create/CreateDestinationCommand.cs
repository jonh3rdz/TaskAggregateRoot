using MediatR;
using ErrorOr;
using Application.Destinations.Common;
using Domain.ValueObjects;

namespace Application.Destinations;

public record CreateDestinationCommand(
    string Name,
    string Description,
    string Ubication
) : IRequest<ErrorOr<Unit>>;