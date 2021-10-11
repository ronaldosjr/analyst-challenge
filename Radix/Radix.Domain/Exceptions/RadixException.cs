using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Domain.Exceptions
{
    public class RadixException : Exception
    {
        public RadixException(string message) : base(message)
        {
        }
    }
}
