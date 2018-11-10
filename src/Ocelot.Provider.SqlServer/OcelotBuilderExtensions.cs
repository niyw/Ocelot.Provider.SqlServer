
namespace Ocelot.Provider.SqlServer {
    using Configuration.Repository;
    using DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class OcelotBuilderExtensions
    {
        public static IOcelotBuilder AddConfigStoredInSQLServer(this IOcelotBuilder builder,Action<DbContextOptionsBuilder> configStoreAction=null)
        {
            builder.Services.AddSingleton(SqlServerMiddlewareConfigurationProvider.Get);
            builder.Services.AddHostedService<FileConfigurationPoller>();
            builder.Services.AddSingleton<IFileConfigurationRepository, SqlServerFileConfigurationRepository>();
            return builder;
        }
    }
}
