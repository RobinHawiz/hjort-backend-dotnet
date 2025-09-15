using DataAccessLibrary.Models;

namespace DataAccessLibrary.Reservation
{
    public interface IReservationData
    {
        List<ReservationModel> GetAllReservations();
        void Insert(ReservationInsertModel reservation);
        public void Delete(int id);
        public bool Exists(int id);
    }
}