using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Exceptions;

/// <summary>Thrown when the course menu id is invalid.</summary>
public class InvalidCourseMenuIdException : Exception
{
    /// <summary>
    /// The name of the field or issue this error refers to (used by the client)
    /// </summary>
    public string Field { get; } = "id";
    public InvalidCourseMenuIdException() : base("The course menu with this id does not exist!") { }
}
