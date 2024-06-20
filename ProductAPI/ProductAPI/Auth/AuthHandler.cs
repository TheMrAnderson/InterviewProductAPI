using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace ProductAPI.Auth
{
	/// <summary>
	/// Handler to the API authentication
	/// </summary>
	public class AuthHandler : AuthenticationHandler<AuthSchemeOptions>
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="options"></param>
		/// <param name="logger"></param>
		/// <param name="encoder"></param>
		/// <param name="clock"></param>
		[Obsolete]
		public AuthHandler(
			IOptionsMonitor<AuthSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock) : base(options, logger, encoder, clock) { }

		/// <summary>
		/// Handle authentication request
		/// </summary>
		/// <returns></returns>
		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			bool tokenMatches = false;
			Roles role;

			if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
				return Task.FromResult(AuthenticateResult.Fail("Header not found"));

			var token = Request.Headers[HeaderNames.Authorization].ToString();
			switch (token)
			{
				case "Bearer 123":
					tokenMatches = true;
					role = Roles.User;
					break;
				case "Bearer 456":
					tokenMatches = true;
					role = Roles.Admin;
					break;
				case "Bearer 789":
					tokenMatches = true;
					role = Roles.SuperAdmin;
					break;
				default:
					tokenMatches = false;
					role = Roles.Unknown;
					break;
			}
			tokenMatches = true;

			if (tokenMatches)
			{
				// create claims array from the model
				var claims = new[] {
					new Claim(ClaimTypes.Name, "Injected User") ,
					new Claim(ClaimTypes.Role, role.ToString())
				};

				// generate claimsIdentity on the name of the class
				var claimsIdentity = new ClaimsIdentity(claims, nameof(AuthHandler));

				// generate AuthenticationTicket from the Identity
				// and current authentication scheme
				var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

				// pass on the ticket to the middleware
				return Task.FromResult(AuthenticateResult.Success(ticket));
			}

			return Task.FromResult(AuthenticateResult.Fail("Incorrect token"));
		}
	}
}
