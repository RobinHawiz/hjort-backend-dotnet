using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Exceptions
{
    /// <summary>Thrown when login credentials are invalid.</summary>
    public sealed class InvalidCredException : Exception
    {
        /// <summary>
        /// The name of the field or issue this error refers to (used by the client)
        /// </summary>
        public string Field { get; } = "login";
        public InvalidCredException() : base("An admin user with this username or password does not exist!") { }
    }
}
