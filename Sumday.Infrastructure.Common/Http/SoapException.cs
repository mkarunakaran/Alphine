using System;

namespace Sumday.Infrastructure.Common.Http
{
    public class SoapException : Exception
    {
        public SoapException(string errorCode, string message)
            : this(errorCode, message, null)
        {
        }

        public SoapException(string errorCode, string message, Exception innerException)
            : base($"Error: {errorCode} - {message}", innerException)
        {
            this.ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; }
    }
}