using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiClients.Common
{
    public abstract class Client
    {
        protected async Task<string> GenerateErrorMessageForResponseAsync(HttpResponseMessage response)
        {
            var uri = response.RequestMessage.RequestUri.AbsoluteUri;
            var sb = new StringBuilder($"Unexpected response from {uri}. {response.ReasonPhrase}. ");
            if (response.Content.Headers.ContentLength > 0)
            {
                sb.AppendLine(await response.Content.ReadAsStringAsync());
            }

            return sb.ToString();
        }
    }
}
