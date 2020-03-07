using System;
using System.Reflection;
using AdventureWorks.Api.Extensions;
using AdventureWorks.Api.Filters;
using AdventureWorks.Api.Handlers;
using AdventureWorks.Client;
using AdventureWorks.Common.Settings;
using AdventureWorks.Service;
using ApiClients.Common.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AdventureWorks.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(ConfigureControllers);
            var connectionString = _configuration.GetSection("ConnectionString").Get<ConnectionStringSetting>();
            services
                .AddAutoMapper(Assembly.Load("AdventureWorks.Service"))
                .AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString.AdventureWorksDbContext), ServiceLifetime.Scoped)
                .AddHttpClients(_configuration)
                .AddTransient<IAdventureWorksService, AdventureWorksService>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSwaggerGen(SetupUpSwaggerGen)
                .Configure(ConfigureApiBehaviourOptionsDelegate)
                .Configure<BasicAuthSettings>(_configuration.GetSection("BasicAuth"))
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                });
            
                //Use this for Basic Authentiction
                //.AddAuthentication("BasicAuthentication")
                //.AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", configureOptions: null);
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment, ILoggerFactory loggerFactory, IMapper mapper)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseExceptionHandler(appBuilder => ConfigureExceptionHandling(appBuilder, loggerFactory));
            }

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            application
                .UseHttpsRedirection()
                .UseSwagger()
                .UseSwaggerUI(SetUpSwaggerUi)
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        #region Helpers
        private static void ConfigureControllers(MvcOptions options)
        {
            options.Filters.Add<OperationCancelledExceptionFilter>();
        }

        private static void ConfigureExceptionHandling(IApplicationBuilder appBuilder, ILoggerFactory loggerFactory)
        {
            appBuilder.Run(async httpContext =>
            {
                // TODO: Seperate out exception handle to middleware
                var exception = httpContext.Features.Get<IExceptionHandlerPathFeature>().Error;
                var logger = loggerFactory.CreateLogger(exception.Source);
                logger.LogError(exception, exception.Message);

                ProblemDetails problemDetails;
                if (exception is ClientException) {
                    var clientException = exception as ClientException;
                    problemDetails = new ProblemDetails
                    {
                        Title = clientException.Message,
                        Status = clientException.HttpStatusCode.GetHashCode(),
                        Detail = "An unexpected error has occured",
                        Instance = httpContext.Request.GetEncodedPathAndQuery(),
                        Type = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{clientException.HttpStatusCode.GetHashCode()}"
                    };
                } else {
                    problemDetails = new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error has occured",
                        Instance = httpContext.Request.GetEncodedPathAndQuery(),
                        Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500"
                    };
                }

                httpContext.Response.ContentType = "application/problem+json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            });
        }

        private static Action<ApiBehaviorOptions> ConfigureApiBehaviourOptionsDelegate
        {
            get
            {
                return options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var validationProblemDetails = new ValidationProblemDetails(actionContext.ModelState)
                        {
                            Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400",
                            Status = StatusCodes.Status400BadRequest,
                            Detail = "Refer to the errors property for additional details",
                            Instance = actionContext.HttpContext.Request.Path
                        };

                        return new BadRequestObjectResult(validationProblemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                };
            }
        }

        private void SetupUpSwaggerGen(SwaggerGenOptions options)
        {
            var swaggerSettings = _configuration.GetSection("Swagger").Get<SwaggerSettings>();
            var basicAuthSettings = _configuration.GetSection("BasicAuth").Get<BasicAuthSettings>();
            SwaggerConfig.SetUpSwaggerGen(options, swaggerSettings, addBasicAuth: basicAuthSettings.IsEnabled);
        }

        private void SetUpSwaggerUi(SwaggerUIOptions options)
        {
            var swaggerSettings = _configuration.GetSection("Swagger").Get<SwaggerSettings>();
            SwaggerConfig.SetUpSwaggerUi(options, swaggerSettings.SwaggerRelativeUrl, swaggerSettings.ApiName);
        }
        #endregion
    }
}
