using MediatR;
using ErrorOr;
using Application.TouristPackages.Common;
using Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.TouristPackages;
using Domain.Destinations;

namespace Application.TouristPackages.Search
{
    public class SearchTouristPackagesQueryHandler : IRequestHandler<SearchTouristPackagesQuery, ErrorOr<List<TouristPackageResponse>>>
    {
        private readonly ITouristPackageRepository _touristPackageRepository;
        private readonly IDestinationRepository _destinationRepository;

        public SearchTouristPackagesQueryHandler(ITouristPackageRepository touristPackageRepository, IDestinationRepository destinationRepository)
        {
            _touristPackageRepository = touristPackageRepository;
            _destinationRepository = destinationRepository;
        }

        public async Task<ErrorOr<List<TouristPackageResponse>>> Handle(SearchTouristPackagesQuery query, CancellationToken cancellationToken)
        {
            var touristPackages = await _touristPackageRepository.Search(query.Name, query.Description, query.TravelDate, query.Price, query.Ubication);

            var touristPackageResponses = new List<TouristPackageResponse>();

            foreach (var touristPackage in touristPackages)
            {
                var lineItemResponses = new List<LineItemResponse>();

                foreach (var lineItem in touristPackage.LineItems)
                {
                    var destination = await _destinationRepository.GetByIdAsync(lineItem.DestinationId);
                    var name = destination?.Name ?? string.Empty;
                    var ubication = destination?.Ubication ?? string.Empty;

                    var lineItemResponse = new LineItemResponse(name, ubication);
                    lineItemResponses.Add(lineItemResponse);
                }

                var touristPackageResponse = new TouristPackageResponse(
                    (Guid)touristPackage.Id.Value,
                    touristPackage.Name,
                    touristPackage.Description,
                    new MoneyResponse(touristPackage.Price.Currency, touristPackage.Price.Amount),
                    touristPackage.TravelDate,
                    lineItemResponses);

                touristPackageResponses.Add(touristPackageResponse);
            }

            return touristPackageResponses;
        }
    }
}
