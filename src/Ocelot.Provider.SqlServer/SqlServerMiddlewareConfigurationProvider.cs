namespace Ocelot.Provider.SqlServer {
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Configuration.Creator;
    using Configuration.File;
    using Configuration.Repository;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Middleware;
    using Responses;
    public static class SqlServerMiddlewareConfigurationProvider
    {
        public static OcelotMiddlewareConfigurationDelegate Get = async builder =>
        {
            var fileConfigRepo = builder.ApplicationServices.GetService<IFileConfigurationRepository>();
            var fileConfig = builder.ApplicationServices.GetService<IOptionsMonitor<FileConfiguration>>();
            var internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
            var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();

            if (UsingSqlServer(fileConfigRepo))
            {
                await SetFileConfigInSqlServer(builder, fileConfigRepo, fileConfig, internalConfigCreator, internalConfigRepo);
            }
        };

        private static bool UsingSqlServer(IFileConfigurationRepository fileConfigRepo)
        {
            return fileConfigRepo.GetType() == typeof(SqlServerFileConfigurationRepository);
        }

        private static async Task SetFileConfigInSqlServer(IApplicationBuilder builder,
            IFileConfigurationRepository fileConfigRepo, IOptionsMonitor<FileConfiguration> fileConfig,
            IInternalConfigurationCreator internalConfigCreator, IInternalConfigurationRepository internalConfigRepo)
        {
            var fileConfigFromSqlServer = await fileConfigRepo.Get();

            if (IsError(fileConfigFromSqlServer))
            {
                ThrowToStopOcelotStarting(fileConfigFromSqlServer);
            }
            else if (ConfigNotStoredInConsul(fileConfigFromSqlServer))
            {
                await fileConfigRepo.Set(fileConfig.CurrentValue);
            }
            else
            {
                var internalConfig = await internalConfigCreator.Create(fileConfigFromSqlServer.Data);

                if (IsError(internalConfig))
                {
                    ThrowToStopOcelotStarting(internalConfig);
                }
                else
                {
                    var response = internalConfigRepo.AddOrReplace(internalConfig.Data);

                    if (IsError(response))
                    {
                        ThrowToStopOcelotStarting(response);
                    }
                }

                if (IsError(internalConfig))
                {
                    ThrowToStopOcelotStarting(internalConfig);
                }
            }
        }

        private static void ThrowToStopOcelotStarting(Response config)
        {
            throw new Exception($"Unable to start Ocelot, errors are: {string.Join(",", config.Errors.Select(x => x.ToString()))}");
        }

        private static bool IsError(Response response)
        {
            return response == null || response.IsError;
        }

        private static bool ConfigNotStoredInConsul(Response<FileConfiguration> fileConfigFromSqlServer)
        {
            return fileConfigFromSqlServer.Data == null;
        }
    }
}
