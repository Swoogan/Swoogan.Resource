using System;
using System.Runtime.Serialization;

namespace Swoogan.Resource
{
    [Serializable]
    public class MalformedUrlException : Exception
    {
        public MalformedUrlException()
        {
        }

        public MalformedUrlException(string message) : base(message)
        {
        }

        public MalformedUrlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MalformedUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}