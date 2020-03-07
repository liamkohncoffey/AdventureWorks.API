using System;
using System.Net;

namespace ApiClients.Common.Exceptions
{
    public class ClientException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public ClientException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
