using ErrorOr;

namespace Domain.DomainErrors;

public static partial class Errors
{
    public static class Destination
    {
        public static Error UbicationWhitBadFormat =>
            Error.Validation("Destination.Ubication", " is not a valid Ubication format");

    }
}