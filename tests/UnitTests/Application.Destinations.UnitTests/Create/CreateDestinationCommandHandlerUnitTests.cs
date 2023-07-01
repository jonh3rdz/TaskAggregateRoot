using Domain.Destinations;
using Domain.Primitives;
using Domain.DomainErrors;

namespace Application.Destinations.UnitTests.Create;

public class CreateDestinationCommandHandlerUnitTests
{
    private readonly Mock<IDestinationRepository> _mockDestinationRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    private readonly CreateDestinationCommandHandler _handler;
    public CreateDestinationCommandHandlerUnitTests()
    {
        _mockDestinationRepository = new Mock<IDestinationRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new CreateDestinationCommandHandler(_mockDestinationRepository.Object, _mockUnitOfWork.Object);
    }
    [Fact]
    public async void HandleCreateDestination_WhenUbicationHasBadFormat_ShouldReturnValidationError()
    {
        CreateDestinationCommand command = new(
            "Francia",
            "Paris",
            ""
            );

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Validation);
        result.FirstError.Code.Should().Be(Errors.Destination.UbicationWhitBadFormat.Code);
        result.FirstError.Description.Should().Be(Errors.Destination.UbicationWhitBadFormat.Description);
    }
}