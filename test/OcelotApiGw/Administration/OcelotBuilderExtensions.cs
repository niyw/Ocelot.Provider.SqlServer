namespace Ocelot.Administration {
    using System;
    using DependencyInjection;
    using IdentityServer4.AccessTokenValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class OcelotBuilderExtensions {
        public static IOcelotAdministrationBuilder AddAdministration(this IOcelotBuilder builder, string path, Action<IdentityServerAuthenticationOptions> configureOptions) {
            var administrationPath = new AdministrationPath(path);
            builder.Services.AddSingleton(IdentityServerMiddlewareConfigurationProvider.Get);

            if (configureOptions != null) {

                builder.Services
                    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(configureOptions);
            }

            builder.Services.AddSingleton<IAdministrationPath>(administrationPath);

            return new OcelotAdministrationBuilder(builder.Services, builder.Configuration);
        }

    }
}
