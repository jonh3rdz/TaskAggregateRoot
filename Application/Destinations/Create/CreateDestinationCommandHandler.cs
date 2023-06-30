using Application.Destinations.Common;
using Domain.Primitives;
using Domain.Destinations;
using ErrorOr;
using MediatR;
using Domain.ValueObjects;

namespace Application.Destinations;
internal class CreateDestinationCommandHandler : IRequestHandler<CreateDestinationCommand, ErrorOr<Unit>>
{
    private readonly IDestinationRepository _destinationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDestinationCommandHandler(IDestinationRepository destinationRepository, IUnitOfWork unitOfWork)
    {
        _destinationRepository = destinationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Unit>> Handle(CreateDestinationCommand command, CancellationToken cancellationToken)
    {
        var destination = new Destination(
            new DestinationId(Guid.NewGuid()), command.Name, command.Description, command.Ubication);

        _destinationRepository.Add(destination);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // return new DestinationResponse();
        return Unit.Value;
    }
}