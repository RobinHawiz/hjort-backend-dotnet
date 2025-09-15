using DataAccessLibrary.Models;

namespace DataAccessLibrary.Reservation;

public class ReservationData : IReservationData
{
    private readonly ISqliteDataAccess _db;
    private readonly string _connectionStringName = "Default";

    public ReservationData(ISqliteDataAccess db)
    {
        _db = db;
    }

    public List<ReservationModel> GetAllReservations()
    {
        string sql = "select id as Id, first_name as FirstName, last_name as LastName, phone_number as PhoneNumber, " +
                     "email as Email, message as Message, guest_amount as GuestAmount, reservation_date as ReservationDate " +
                     "from reservation ORDER BY reservation_date ASC";

        return _db.LoadData<ReservationModel, dynamic>(sql, new { }, _connectionStringName);
    }

    public void Insert(ReservationInsertModel reservation)
    {
        string sql = "insert into reservation " +
                     "(first_name, last_name, phone_number, email, message, guest_amount, reservation_date) " +
                     "values(@FirstName, @LastName, @PhoneNumber, @Email, @Message, @GuestAmount, @ReservationDate)";

        _db.SaveData<ReservationInsertModel>(sql, reservation, _connectionStringName);
    }

    public void Delete(int id)
    {
        string sql = "delete from reservation where id = @id";

        _db.SaveData<dynamic>(sql, new { id }, _connectionStringName);
    }

    public bool Exists(int id)
    {
        string sql = "select * from reservation where id = @id";

        return _db.LoadData<ReservationModel, dynamic>(sql, new { id }, _connectionStringName).FirstOrDefault() != null;
    }
}
