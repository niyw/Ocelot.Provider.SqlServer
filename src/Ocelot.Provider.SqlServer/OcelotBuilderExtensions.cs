
namespace Ocelot.Provider.SqlServer {
    using Configuration.Repository;
    using DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Ocelot.Cache;
    using Ocelot.Configuration.File;
    using System;

    public static class OcelotBuilderExtensions {
        public static IOcelotBuilder AddConfigStoredInSQLServer(this IOcelotBuilder builder, Action<OcelotConfigDbConfiguration> ocelotCfgDbOptions = null) {
            var options = new OcelotConfigDbConfiguration();
            builder.Services.AddSingleton(options);
            ocelotCfgDbOptions?.Invoke(options);

            builder.Services.AddSingleton(SqlServerMiddlewareConfigurationProvider.Get);
            builder.Services.AddHostedService<FileConfigurationPoller>();
            builder.Services.AddSingleton<IFileConfigurationRepository, SqlServerFileConfigurationRepository>();
            builder.Services.AddSingleton<IOcelotCache<FileConfiguration>, OcelotConfigCache>();
            builder.Services.Remove(new ServiceDescriptor(typeof(IFileConfigurationRepository), typeof(DiskFileConfigurationRepository)));
            builder.Services.Remove(new ServiceDescriptor(typeof(IOcelotCache<FileConfiguration>), typeof(InMemoryCache<FileConfiguration>)));
            return builder;
        }
    }
}
