using DataAccessLibrary.Models;
using DataAccessLibrary.Reservation;
using ServiceLibrary.Exceptions;
using System.Globalization;

namespace ServiceLibrary.Reservation;

public class ReservationService : IReservationService
{
    private readonly IReservationData _data;

    public ReservationService(IReservationData data)
    {
        _data = data;
    }

    public List<ReservationModel> GetAllReservations()
    {
        return _data.GetAllReservations();
    }

    public void createReservation(ReservationInsertModel reservation)
    {
        DateTimeOffset reservationDate = DateTimeOffset.Parse(reservation.ReservationDate, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);
        if (reservationDate < DateTimeOffset.UtcNow)
        {
            throw new InvalidReservationDateException();
        }
        _data.Insert(reservation);
    }

    public void deleteReservation(int id)
    {
        bool reservationExists = _data.Exists(id);
        if (!reservationExists)
        {
            throw new InvalidReservationIdException();
        }
        _data.Delete(id);
    }
}
