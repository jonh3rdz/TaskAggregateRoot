// using Domain.Customers;
using Domain.Primitives;
using Domain.ValueObjects;
using Domain.TouristPackages;
using Domain.Reservations;

namespace Domain.Reservations;

public sealed class Reservation : AgregateRoot
{
    public Reservation(ReservationId id, string name, string email, PhoneNumber phoneNumber, TouristPackageId touristPackageId, DateTime traveldate)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        TouristPackageId = touristPackageId;
        TravelDate = traveldate;
    }
    private Reservation()
    {

    }
    public ReservationId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public PhoneNumber PhoneNumber { get; private set; }
    public TouristPackageId TouristPackageId { get; private set; }
    public DateTime TravelDate { get; private set; }

    public static Reservation Create(string name, string email, PhoneNumber phoneNumber, TouristPackageId touristPackageId, DateTime traveldate)
    {
        var reservation = new Reservation
        {
            Id = new ReservationId(Guid.NewGuid()),
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            TouristPackageId = touristPackageId,
            TravelDate = traveldate
        };

        return reservation;
    }

    public void Update(string name, string email, PhoneNumber phoneNumber, TouristPackageId touristPackageId, DateTime traveldate)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        TouristPackageId = touristPackageId;
        TravelDate = traveldate;
    }

    public static Reservation UpdateReservation(
        Guid id,
        string name,
        string email,
        PhoneNumber phoneNumber,
        TouristPackageId touristPackageId,
        DateTime traveldate)
    {
        return new Reservation(new ReservationId(id), name, email, phoneNumber, touristPackageId, traveldate);
    }
}