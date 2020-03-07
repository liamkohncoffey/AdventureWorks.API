using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiClients.Common.Extensions
{
    public static class HttpContentExtensions
    {
        private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true
        };

        public static async Task<T> ReadJsonAsync<T>(this HttpContent httpContent)
        {
            return JsonSerializer.Deserialize<T>(json: await httpContent.ReadAsStringAsync(), options: DefaultJsonSerializerOptions);
        }
    }
}