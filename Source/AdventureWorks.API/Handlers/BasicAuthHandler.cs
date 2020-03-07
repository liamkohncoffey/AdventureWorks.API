using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AdventureWorks.Common.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdventureWorks.Api.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly BasicAuthSettings _basicAuthSettings;

        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<BasicAuthSettings> basicAuthSettings) : base(options, logger, encoder, clock)
        {
            _basicAuthSettings = basicAuthSettings.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();

            if (_basicAuthSettings.IsEnabled)
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return await Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
                }

                try
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];
                    if (username.Equals(_basicAuthSettings.UserName) && password.Equals(_basicAuthSettings.Password))
                    {
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, _basicAuthSettings.UserName));
                        claims.Add(new Claim(ClaimTypes.Name, _basicAuthSettings.UserName));
                    }
                    else
                    {
                        Logger.LogWarning("Request not authorized. Invalid Basic Auth credentials for User {UserName}", username);
                        return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization"));
                    }
                }
                catch
                {
                    const string warningMessage = "Request denied. Invalid authorization header";
                    Logger.LogWarning(warningMessage);
                    return await Task.FromResult(AuthenticateResult.Fail(warningMessage));
                }
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
