using MediatR;
using ErrorOr;
using Domain.TouristPackages;
using Application.Reservations.Common;
using Domain.ValueObjects;

namespace Application.Reservations.Common;

public record ReservationResponse(
    Guid CodigoReserva,
    string Name,
    string Email,
    string PhoneNumber,
    DateTime TravelDate,
    DateTime FechaReserva,
    // TouristPackageId TouristPackageId,
    TouristPackageResponse paqueteTuristico
) : IRequest<ErrorOr<ReservationResponse>>;

public record TouristPackageResponse(
    string Nombre,
    List<LineItemResponse> Destinos  // Propiedad para los LineItems
    );

public record LineItemResponse(
    string Nombre,
    string Ubicacion);

