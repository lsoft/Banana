using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenCL.Net.Wrapper
{
    public class OpenCLException : Exception
    {
        public OpenCLException()
        {
        }

        public OpenCLException(string message) : base(message)
        {
        }

        public OpenCLException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OpenCLException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
