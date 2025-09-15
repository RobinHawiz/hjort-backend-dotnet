namespace DataAccessLibrary.Models;

public class ReservationModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public int GuestAmount { get; set; }
    public string ReservationDate { get; set; }
}
