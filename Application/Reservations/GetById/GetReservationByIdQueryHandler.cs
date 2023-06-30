using Application.Reservations.Common;
using Domain.Reservations;
using Domain.Destinations;
using Domain.TouristPackages;
using ErrorOr;
using MediatR;

using Domain.Primitives;
using Domain.ValueObjects;
using System.Runtime.InteropServices;
using Application.Reservations.GetById;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Application.Reservations.GetAll;

internal sealed class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ErrorOr<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ITouristPackageRepository _touristPackageRepository;
    private readonly IDestinationRepository _destinationRepository;

    public GetReservationByIdQueryHandler(IReservationRepository reservationRepository, ITouristPackageRepository touristPackageRepository, IDestinationRepository destinationRepository)
    {
        _reservationRepository = reservationRepository;
        _touristPackageRepository = touristPackageRepository;
        _destinationRepository = destinationRepository;
    }

    public async Task<ErrorOr<ReservationResponse>> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        if (await _reservationRepository.GetByIdAsync(new ReservationId(query.Id)) is not Reservation reservation)
        {
            return Error.NotFound("Reservation.NotFound", "The reservation with the provided Id was not found.");
        }

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
            new TouristPackageResponse(
                touristPackage?.Name ?? string.Empty,
                lineItemResponses
            )
        );

        return response;
    }
}
