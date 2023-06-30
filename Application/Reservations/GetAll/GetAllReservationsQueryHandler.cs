using Application.Reservations.Common;
using Domain.Reservations;
using Domain.Destinations;
using Domain.TouristPackages;
using ErrorOr;
using MediatR;

using Domain.Primitives;
using Domain.ValueObjects;
using System.Runtime.InteropServices;


namespace Application.Reservations.GetAll;

internal sealed class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, ErrorOr<IReadOnlyList<ReservationResponse>>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ITouristPackageRepository _touristPackageRepository;
    private readonly IDestinationRepository _destinationRepository;

    public GetAllReservationsQueryHandler(IReservationRepository reservationRepository, ITouristPackageRepository touristPackageRepository, IDestinationRepository destinationRepository)
    {
        _reservationRepository = reservationRepository;
        _touristPackageRepository = touristPackageRepository;
        _destinationRepository = destinationRepository;
    }

    public async Task<ErrorOr<IReadOnlyList<ReservationResponse>>> Handle(GetAllReservationsQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<Reservation> reservations = await _reservationRepository.GetAll();

        var responses = new List<ReservationResponse>();

        foreach (var reservation in reservations)
        {
            var touristPackage = await _touristPackageRepository.GetByIdWithLineItemAsync(reservation.TouristPackageId);

            var lineItemResponses = new List<LineItemResponse>();

            foreach (var lineItem in touristPackage.LineItems)
            {
                var destination = await _destinationRepository.GetByIdAsync(lineItem.DestinationId);
                string Name = destination != null ? destination.Name : string.Empty;
                string Ubication = destination != null ? destination.Ubication : string.Empty;

                var lineItemResponse = new LineItemResponse(Name, Ubication);
                lineItemResponses.Add(lineItemResponse);
            }

            var response = new ReservationResponse(
                reservation.Id.Value,
                reservation.Name,
                reservation.Email,
                reservation.PhoneNumber.Value,
                touristPackage?.TravelDate ?? DateTime.Now,
                reservation.TravelDate,
                // reservation.TouristPackageId,
                new TouristPackageResponse(
                    touristPackage?.Name ?? string.Empty,
                    lineItemResponses // Agrega los LineItems a la respuesta
                    )
            );

            responses.Add(response);
        }

        return responses;
    }

}