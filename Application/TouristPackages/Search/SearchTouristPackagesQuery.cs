using MediatR;
using ErrorOr;
using Application.TouristPackages.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Domain.TouristPackages;
using Domain.Destinations;

namespace Application.TouristPackages.Search;
public record SearchTouristPackagesQuery(
    string Name,
    string Description,
    DateTime? TravelDate,
    decimal? Price,
    string Ubication
) : IRequest<ErrorOr<List<TouristPackageResponse>>>;