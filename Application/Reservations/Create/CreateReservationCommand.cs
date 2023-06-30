using Application.Reservations.Common;
using Domain.Reservations;
using Domain.TouristPackages;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Reservations.Create;

public record CreateReservationCommand(
    string Name,
    string Email,
    string PhoneNumber,
    TouristPackageId TouristPackageId,
    DateTime Traveldate
    ) : IRequest<ErrorOr<Unit>>;