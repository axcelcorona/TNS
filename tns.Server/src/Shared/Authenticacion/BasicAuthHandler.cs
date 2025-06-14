// BasicAuthenticationHandler.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using tns.Server.src.Shared.Authenticacion;

namespace tns.Server.src.Shared.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly BasicAuthConfig _basicAuthConfig;

        public BasicAuthenticationHandler(
            IOptions<BasicAuthConfig> basicAuthConfig,  
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _basicAuthConfig = basicAuthConfig.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            string authorizationHeader = Request.Headers.Authorization;
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic "))
                return AuthenticateResult.Fail("Invalid Authorization Header");

            try
            {
                var encodedCredentials = authorizationHeader["Basic ".Length..].Trim();
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var credentials = decodedCredentials.Split(':', 2);

                if (credentials.Length != 2 || !await IsValidUser(credentials[0], credentials[1]))
                    return AuthenticateResult.Fail("Invalid Credentials");

                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, credentials[0]),
                    new Claim(ClaimTypes.Name, credentials[0]),
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }

        private async Task<bool> IsValidUser(string username, string password)
        {
            var result = _basicAuthConfig.Credentials.Any(c => c.Username.Equals(username, StringComparison.Ordinal) && c.Password.Equals(password, StringComparison.Ordinal));
            return result;
        }
    }
}