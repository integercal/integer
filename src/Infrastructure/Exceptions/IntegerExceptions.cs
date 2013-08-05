using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integer.Infrastructure.Exceptions
{
    public class IntegerException : Exception
    {
        public IntegerException() : base() { }
        public IntegerException(string message) : base(message) { }
        public IntegerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
