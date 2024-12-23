using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base() { }

        public InvalidTokenException(string message) : base(message) { }

        public InvalidTokenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
