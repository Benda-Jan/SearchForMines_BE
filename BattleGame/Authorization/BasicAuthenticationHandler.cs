using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BattleGame.Authorization
{
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
		public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder)
		{
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.TryGetValue("Authorization", out var value))
				return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

			var header = value.ToString();
			if (!header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
				return Task.FromResult(AuthenticateResult.Fail("Authorization have to start with 'Basic' keyword"));

			var decoded = Encoding.UTF8.GetString(
				Convert.FromBase64String(
					header.Replace("Basic ", String.Empty, StringComparison.OrdinalIgnoreCase)));
			var split = decoded.Split(':');
			if (split.Length != 2)
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header format"));

			var userName = split[0];
			var password = split[1];

			if (userName != "JanBenda" || password != "Password123")
                return Task.FromResult(AuthenticateResult.Fail("Username or Password is incorrect"));

            var claimsPrincipal = new ClaimsPrincipal(
				new ClaimsIdentity(
					new BasicAuthenticationIdentity(userName), new[] { new Claim(ClaimTypes.Name, userName)}));

			return Task.FromResult(AuthenticateResult.Success(
				new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
		}
	}
}

