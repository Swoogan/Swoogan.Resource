using System;

namespace Swoogan.Resource
{
    public class GetException : Exception
    {
        public GetException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
