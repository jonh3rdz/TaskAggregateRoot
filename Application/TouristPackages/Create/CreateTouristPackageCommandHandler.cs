using Application.TouristPackages.Common;
using Domain.Primitives;
using Domain.Destinations;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Domain.TouristPackages;
using Domain.Reservations;
using System.Runtime.InteropServices;

namespace Application.TouristPackages.Create;
public sealed class CreateTouristPackageCommandHandler : IRequestHandler<CreateTouristPackageCommand, ErrorOr<Unit>>
{
    private readonly ITouristPackageRepository _touristpackageRepository;
    private readonly IDestinationRepository _destinationRepository;
    private readonly IUnitOfWork _unitofwork;
    public CreateTouristPackageCommandHandler(ITouristPackageRepository touristpackageRepository, IDestinationRepository destinationRepository, IUnitOfWork unitofwork)
    {
        _touristpackageRepository = touristpackageRepository;
        _destinationRepository = destinationRepository;
        _unitofwork = unitofwork;
    }


    public async Task<ErrorOr<Unit>> Handle(CreateTouristPackageCommand command, CancellationToken cancellationToken)
    {

        //var name = new TouristPackage(new TouristPackageId(Guid.NewGuid()), command.Name);
        var touristPackage = TouristPackage.Create(
            command.Name,
            command.Description,
            command.Traveldate,
            command.Price);

        if (!command.Items.Any())
        {
            return Error.Conflict("TouristPackage.Detail", "For create at reservation you need to specify the details of the reservation");
        }

        foreach (var item in command.Items)
        {
            touristPackage.Add(new DestinationId(item.DestinationId));
        }
        _touristpackageRepository.Add(touristPackage);

        await _unitofwork.SaveChangesAsync(cancellationToken);

        // return new TouristPackageResponse();
        return Unit.Value;
    }
}