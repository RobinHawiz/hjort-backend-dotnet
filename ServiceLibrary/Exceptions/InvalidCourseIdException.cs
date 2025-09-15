using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Exceptions;

public class InvalidCourseIdException : Exception
{
    public string Field { get; } = "id";
    public InvalidCourseIdException() : base("The course with this id does not exist!") { }
}
