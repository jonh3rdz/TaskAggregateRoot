using MediatR;
using ErrorOr;
using Application.TouristPackages.Common;
using Domain.ValueObjects;

namespace Application.TouristPackages.Common;

public record TouristPackageResponse(
    Guid Id,
    string Name,
    string Description,
    MoneyResponse Price,
    DateTime TravelDate,
    List<LineItemResponse> LineItems
    ) : IRequest<ErrorOr<TouristPackageResponse>>;

public record MoneyResponse(
    string Currency,
    decimal Amount
    );

public record LineItemResponse(string Name, string Ubication);

