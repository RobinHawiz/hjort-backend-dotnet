namespace DataAccessLibrary.Models;

public class ReservationInsertModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int GuestAmount { get; set; }
    public string ReservationDate { get; set; } = null!;
}
