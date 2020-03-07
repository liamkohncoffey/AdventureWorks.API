using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AdventureWorks.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AdventureWorks.Api
{
    public class SwaggerConfig
    {
        internal class SwaggerDocumentFilter : IDocumentFilter
        {
            private readonly string _swaggerDocHost;

            public SwaggerDocumentFilter(IHttpContextAccessor httpContextAccessor)
            {
                var host = httpContextAccessor.HttpContext.Request.Host.Value;
                var scheme = httpContextAccessor.HttpContext.Request.Scheme;
                _swaggerDocHost = $"{scheme}://{host}";
            }

            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Servers.Add(new OpenApiServer { Url = _swaggerDocHost });
            }
        }

        internal static void SetUpSwaggerGen(SwaggerGenOptions options, SwaggerSettings swaggerSettings, bool addBasicAuth)
        {
            options.DocumentFilter<SwaggerDocumentFilter>();
            options.SwaggerDoc(swaggerSettings.ApiName, new OpenApiInfo { Title = swaggerSettings.Title, Version = swaggerSettings.ApiVersion });
            options.CustomSchemaIds(type => $"{type?.Namespace?.Split('.').Last()}.{type?.Name}"); //E.g. .Dtos.Gas.Meter.cs --> Gas.Meter

            AddXmlComments(options);

            if (addBasicAuth)
            {
                AddBasicAuth(options);
            }
        }

        internal static void SetUpSwaggerUi(SwaggerUIOptions options, string? swaggerRelativeUrl, string? apiName)
        {
            options.SwaggerEndpoint(swaggerRelativeUrl, apiName);
        }

        private static void AddBasicAuth(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                Description = "Basic Authorization using username and password",
                Name = "Authorization",
                In = ParameterLocation.Header
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                        {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "basicAuth"}},
                    Array.Empty<string>()
                }
            });
        }

        private static void AddXmlComments(SwaggerGenOptions options)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }
    }
}
