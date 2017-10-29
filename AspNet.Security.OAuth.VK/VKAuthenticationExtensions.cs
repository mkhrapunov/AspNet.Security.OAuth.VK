using System;
using AspNet.Security.OAuth.VK;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VKAuthenticationExtensions
    {
        public static AuthenticationBuilder AddVK(this AuthenticationBuilder builder)
            => builder.AddVK("VK", _ => { });

        public static AuthenticationBuilder AddVK(this AuthenticationBuilder builder, Action<VKAuthenticationOptions> configureOptions)
            => builder.AddVK("VK", configureOptions);

        public static AuthenticationBuilder AddVK(this AuthenticationBuilder builder, string authenticationScheme, Action<VKAuthenticationOptions> configureOptions)
            => builder.AddVK(authenticationScheme, "VK", configureOptions);

        public static AuthenticationBuilder AddVK(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<VKAuthenticationOptions> configureOptions)
            => builder.AddOAuth<VKAuthenticationOptions, VKAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}