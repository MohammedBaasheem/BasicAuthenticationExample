using BasicAuthentication.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BasicAuthentication.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DBcontext _dbcontext;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, DBcontext dbcontext) : base(options, logger, encoder, clock)
        {
            _dbcontext = dbcontext;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Unknown scheme"));
            }
            var encodedCredentials = authHeader["Basic ".Length..];
            var decodedCredentials=Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var userNameAndPassword = decodedCredentials.Split(':');
            var userName= userNameAndPassword[0].ToString();
            var password= userNameAndPassword[1].ToString();
            var user = _dbcontext.Users.FirstOrDefault(user => user.Username == userName && user.Password == password);
            if (user is not null )
            {
                var claims = new[] { new Claim(ClaimTypes.Name, userName),new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
            }
            
        }
    }
}
