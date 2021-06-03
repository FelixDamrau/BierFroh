using System;
using System.Runtime.Serialization;

namespace InsertToSql
{
    public class RawDataParserException : Exception
    {
        public RawDataParserException()
        {
        }

        public RawDataParserException(string message)
            : base(message)
        {
        }

        public RawDataParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RawDataParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
