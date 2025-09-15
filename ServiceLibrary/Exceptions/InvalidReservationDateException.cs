using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Exceptions;

/// <summary>Thrown when the reservation date is invalid.</summary>
public class InvalidReservationDateException : Exception
{
    /// <summary>
    /// The name of the field or issue this error refers to (used by the client)
    /// </summary>
    public string Field { get; } = "reservationDate";
    public InvalidReservationDateException() : base("Reservation date must be after todays date and time.") { }
}
