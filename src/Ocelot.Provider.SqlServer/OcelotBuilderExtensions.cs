
namespace Ocelot.Provider.SqlServer {
    using Configuration.Repository;
    using DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    public static class OcelotBuilderExtensions
    {
        public static IOcelotBuilder AddConfigStoredInSQLServer(this IOcelotBuilder builder)
        {
            builder.Services.AddSingleton(SqlServerMiddlewareConfigurationProvider.Get);
            builder.Services.AddHostedService<FileConfigurationPoller>();
            builder.Services.AddSingleton<IFileConfigurationRepository, SqlServerFileConfigurationRepository>();
            return builder;
        }
    }
}
