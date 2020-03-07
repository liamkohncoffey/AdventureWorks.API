using System.Net.Http;
using AdventureWorks.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdventureWorks.Api.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration appConfig)
        {
            #region Local Methods
            static void ConfigureHttpClientDefaults(HttpClient httpClient, string baseUri, string apimSubKey)
            {
             
            }

            static string MissingBaseUriMsg(string settingName) => $"'{settingName}' Api Client Base Uri not defined on appsettings";
            static string MissingApimKeyMsg(string settingName) => $"'{settingName}' Apim Subscription Key not defined on appsettings";
            #endregion

            var apiClientBaseUris = appConfig.GetSection("ApiClientBaseUri").Get<ApiClientBaseUriSettings>();
            var apimSubKeys = appConfig.GetSection("ApimSubscriptionKey").Get<ApimSubscriptionKeySettings>();

          
            return services;
        }
    }
}
