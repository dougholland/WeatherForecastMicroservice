namespace WeatherForecastMicroservice
{
    using Microsoft.AspNetCore.Authentication;

    using Microsoft.Extensions.Options;

    using System.Security.Claims;

    using System.Text.Encodings.Web;

    /// <summary>
    /// An opinionated implementation of an authentication handler that mocks authentication in the development environment. 
    /// </summary>
    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Creates an instance of the <see cref="T:MockAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication scheme options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="encoder">The encoder.</param>
        public MockAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        /// <summary>
        /// Handles the authentication within the development environment.
        /// </summary>
        /// <returns>A task that represents the asynchronous authentication operation. The task result contains an instance of the <see cref="T:AuthenticateResult"/> class providing the result of the authentication.</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "user"),
                new Claim(ClaimTypes.Name, "user"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
