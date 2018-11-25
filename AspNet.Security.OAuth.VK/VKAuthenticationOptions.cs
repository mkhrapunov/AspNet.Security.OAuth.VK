using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.VK
{
    public class VKAuthenticationOptions : OAuthOptions
    {
        public VKAuthenticationOptions()
        {
            ClaimsIssuer = "VK";

            CallbackPath = new PathString("/signin-vkontakte");

            AuthorizationEndpoint = "https://oauth.vk.com/authorize";
            TokenEndpoint = "https://oauth.vk.com/access_token";
            UserInformationEndpoint = "https://api.vk.com/method/users.get.json";
        }

        public ISet<string> Fields { get; } = new HashSet<string>();

        public string ApiVersion { get; set; } = "8.57";
    }
}