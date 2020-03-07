using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AdventureWorks.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application started.");
            
            try
            {
                logger.LogInformation("Running host.");
                host.Run();
            }
            catch (Exception e)
            {
                logger.LogCritical(exception: e, message: "Application started but host failed to run.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostBuilderContext, configBuilder) =>
            {
                if (!hostBuilderContext.HostingEnvironment.IsDevelopment())
                {
                    AddAzureKeyVault(configBuilder);
                }
            })
            .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
            {
                loggingBuilder
                    .ClearProviders()
                    .AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"))
                    .AddEventSourceLogger();
                
                if (hostBuilderContext.HostingEnvironment.IsDevelopment())
                {
                    loggingBuilder.AddDebug().AddConsole();
                }
            })
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        private static void AddAzureKeyVault(IConfigurationBuilder configBuilder)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var authCallback = new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);
            var keyVaultClient = new KeyVaultClient(authCallback);

            configBuilder.AddAzureKeyVault(
                vault: $"https://{configBuilder.Build()["KeyVaultName"]}.vault.azure.net/",
                client: keyVaultClient,
                manager: new DefaultKeyVaultSecretManager());
        }
    }
}
