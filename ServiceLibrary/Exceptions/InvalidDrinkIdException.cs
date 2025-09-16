
namespace ServiceLibrary.Exceptions;

/// <summary>Thrown when the drink id is invalid.</summary>
public class InvalidDrinkIdException : Exception
{
        /// <summary>
        /// The name of the field or issue this error refers to (used by the client)
        /// </summary>
        public string Field { get; } = "id";
        public InvalidDrinkIdException() : base("The drink with this id does not exist!") { }
    }
