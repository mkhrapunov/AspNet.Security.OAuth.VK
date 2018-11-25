using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace AspNet.Security.OAuth.VK
{
    public class VKAuthenticationHandler : OAuthHandler<VKAuthenticationOptions>
    {
        public VKAuthenticationHandler(IOptionsMonitor<VKAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, Microsoft.AspNetCore.Authentication.AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            address = QueryHelpers.AddQueryString(address, "v", Options.ApiVersion);

            if (Options.Fields.Count != 0)
            {
                address = QueryHelpers.AddQueryString(address, "fields", string.Join(",", Options.Fields));
            }

            var response = await Backchannel.GetAsync(address, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            var user = (JObject)payload["response"][0];

            foreach (var scope in Options.Scope)
            {
                var scope_value = tokens.Response.Value<string>(scope);
                if (!string.IsNullOrEmpty(scope_value))
                {
                    user.Add(scope, scope_value);
                }
            }

            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, user);

            context.RunClaimActions();


            await Options.Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}