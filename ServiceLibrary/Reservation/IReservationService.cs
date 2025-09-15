using DataAccessLibrary.Models;

namespace ServiceLibrary.Reservation
{
    public interface IReservationService
    {
        List<ReservationModel> GetAllReservations();
        void createReservation(ReservationInsertModel reservation);
        void deleteReservation(int id);
    }
}