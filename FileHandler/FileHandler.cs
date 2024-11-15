using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileHandler.Exceptions
{
    public class InvalidNameException : Exception
    {
        public InvalidNameException(string message) : base(message) { }
    }

    public class InvalidAgeException : Exception
    {
        public InvalidAgeException(string message) : base(message) { }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message, Exception innerException) : base(message, innerException) { }
    }
}