using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Exceptions
{
    /// <summary>Thrown when the drink menu id is invalid.</summary>
    public class InvalidDrinkMenuIdException : Exception
    {
        /// <summary>
        /// The name of the field or issue this error refers to (used by the client)
        /// </summary>
        public string Field { get; } = "id";
        public InvalidDrinkMenuIdException() : base("The drink menu with this id does not exist!") { }
    }
}
