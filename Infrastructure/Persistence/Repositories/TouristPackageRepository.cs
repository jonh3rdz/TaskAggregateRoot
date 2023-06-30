using Domain.Reservations;
using Domain.TouristPackages;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TouristPackageRepository : ITouristPackageRepository
{

    private readonly ApplicationDbContext _context;

    public TouristPackageRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Delete(TouristPackage touristPackage) => _context.TouristPackages.Remove(touristPackage);
    public void UpdateTouristPackage(TouristPackage touristPackage)
    {
        _context.TouristPackages.Update(touristPackage);
    }

    public void Update(TouristPackage touristPackage)
    {
        _context.TouristPackages.Update(touristPackage);
    }

    public async Task<bool> ExistsAsync(TouristPackageId id) => await _context.TouristPackages.AnyAsync(touristPackage => touristPackage.Id == id);
    public void Add(TouristPackage touristPackage)
    {
        _context.Add(touristPackage);
    }

    // public async Task<TouristPackage?> GetByIdAsync(TouristPackageId id) 
    // {
    //     await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
    // }

    public async Task<TouristPackage?> GetByIdWithLineItemAsync(TouristPackageId id)
    {
        return await _context.TouristPackages
            .Include(o => o.LineItems)
            .SingleOrDefaultAsync(o => o.Id == id);
    }


    public async Task<TouristPackage?> GetByIdAsync(TouristPackageId id)
    {
        return await _context.TouristPackages
            .Include(o => o.LineItems)
            .SingleOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<TouristPackage>> Search(string name, string description, DateTime? travelDate, decimal? price, string ubication)
    {
        var query = _context.TouristPackages.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrEmpty(description))
            query = query.Where(p => p.Description.Contains(description));

        if (travelDate.HasValue)
            query = query.Where(p => p.TravelDate.Date == travelDate.Value.Date);

        if (price.HasValue)
            query = query.Where(p => p.Price.Amount <= price.Value);

        if (!string.IsNullOrEmpty(ubication))
        {
            query = query.Where(p => p.LineItems.Any(li =>
                _context.Destinations.Any(d => d.Id == li.DestinationId && d.Ubication.Contains(ubication))));
        }

        return await query
            .Include(o => o.LineItems)
            .ToListAsync();
    }


    // public async Task<TouristPackage?> GetByIdAsync(TouristPackageId id) => await _context.TouristPackages.SingleOrDefaultAsync(c => c.Id == id);

    public async Task<List<TouristPackage>> GetAll()
    {
        return await _context.TouristPackages
            .Include(o => o.LineItems)
            .ToListAsync();
    }

    // public async Task<List<TouristPackage>> GetAll() => await _context.TouristPackages.ToListAsync();

    public Task<Reservation?> GetByIdWithLineItemAsync(ReservationId id)
    {
        throw new NotImplementedException();
    }

    public bool HasOneLineItem(TouristPackage touristpackage)
    {
        throw new NotImplementedException();
    }
}