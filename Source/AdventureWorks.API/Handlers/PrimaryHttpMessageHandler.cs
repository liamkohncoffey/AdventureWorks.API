using System.Net;
using System.Net.Http;

namespace AdventureWorks.Api.Handlers
{
    public class PrimaryHttpMessageHandler : HttpClientHandler
    {
        public PrimaryHttpMessageHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }
    }
}