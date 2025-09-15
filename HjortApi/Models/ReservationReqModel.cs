using System.ComponentModel.DataAnnotations;

namespace HjortApi.Models;

public class ReservationReqModel
{
    [Required(ErrorMessage = "First name is required.")]
    [MinLength(1, ErrorMessage = "The first name has to be at least 1 character long.")]
    [MaxLength(50, ErrorMessage = "The first name cannot exceed 50 characters.")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last name is required.")]
    [MinLength(1, ErrorMessage = "The last name has to be at least 1 character long.")]
    [MaxLength(50, ErrorMessage = "The last name cannot exceed 50 characters.")]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Phone number is required.")]
    [MinLength(1, ErrorMessage = "The phone number has to be at least 1 character long.")]
    [MaxLength(20, ErrorMessage = "The phone number cannot exceed 20 characters.")]
    public string PhoneNumber { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [MinLength(1, ErrorMessage = "The email has to be at least 1 character long.")]
    [MaxLength(128, ErrorMessage = "The email cannot exceed 128 characters.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Message is required.")]
    [MaxLength(1000, ErrorMessage = "The message cannot exceed 1000 characters.")]
    public string Message { get; set; } = "";

    [Range(1, 6, ErrorMessage = "Guest amount is required and has to be between 1 and 6.")]
    public int GuestAmount { get; set; }

    [RegularExpression(@"\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z)", ErrorMessage = "The reservation date must be in ISO 8601 format.")]
    public string ReservationDate { get; set; }
}
