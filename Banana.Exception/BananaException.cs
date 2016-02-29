using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Exception
{
    public class BananaException : System.Exception
    {
        public BananaErrorEnum Error
        {
            get;
            private set;
        }

        public BananaException(string message, BananaErrorEnum error) : base(message)
        {
            Error = error;
        }

        public BananaException(string message, System.Exception innerException, BananaErrorEnum error) : base(message, innerException)
        {
            Error = error;
        }

        protected BananaException(SerializationInfo info, StreamingContext context, BananaErrorEnum error) : base(info, context)
        {
            Error = error;
        }

        public BananaException(BananaErrorEnum error)
        {
            Error = error;
        }
    }
}
