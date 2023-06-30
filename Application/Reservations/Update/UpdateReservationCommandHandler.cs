using Application.Reservations.Common;
using Domain.TouristPackages;
using Domain.Reservations;
using Domain.Primitives;
using Domain.Destinations;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Application.Reservations.Update;

namespace Application.Reservations.Update;
public sealed class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ErrorOr<Unit>>
{

    private readonly IReservationRepository _reservationRepository;
    private readonly ITouristPackageRepository _touristPackageRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateReservationCommandHandler(IReservationRepository reservationRepository, ITouristPackageRepository touristPackageRepository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _touristPackageRepository = touristPackageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        if (!await _reservationRepository.ExistsAsync(new ReservationId(command.Id)))
        {
            return Error.NotFound("Reservation.NotFound", "The Reservation with the provide Id was not found.");
        }

        if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
        {
            return Error.Validation("Reservation.PhoneNumber", "Phone number has not valid format.");
        }

        Reservation reservation = Reservation.UpdateReservation(command.Id, command.Name, command.Email, phoneNumber, command.TouristPackageId, command.Traveldate);

        _reservationRepository.Update(reservation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}