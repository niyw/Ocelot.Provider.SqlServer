namespace Ocelot.Provider.SqlServer {
    using System;
    using System.Threading.Tasks;
    using Configuration.File;
    using Configuration.Repository;
    using Logging;
    using Newtonsoft.Json;
    using Responses;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public class SqlServerFileConfigurationRepository : IFileConfigurationRepository {
        private readonly string _configurationKey;
        private readonly Cache.IOcelotCache<FileConfiguration> _cache;
        private readonly IOcelotLogger _logger;
        private readonly OcelotConfigDbContext _ocelotConfigurationContext;
        //private IServiceProvider _serviceProvider;
        public SqlServerFileConfigurationRepository(
            Cache.IOcelotCache<FileConfiguration> cache,
            IInternalConfigurationRepository repo,
            IConfiguration configuration,
            IOcelotLoggerFactory loggerFactory) {
            _logger = loggerFactory.CreateLogger<SqlServerFileConfigurationRepository>();
            _cache = cache;

            var internalConfig = repo.Get();

            _configurationKey = "InternalConfiguration";

            string token = null;

            if (!internalConfig.IsError) {
                token = internalConfig.Data.ServiceProviderConfiguration.Token;
                _configurationKey = !string.IsNullOrEmpty(internalConfig.Data.ServiceProviderConfiguration.ConfigurationKey) ?
                    internalConfig.Data.ServiceProviderConfiguration.ConfigurationKey : _configurationKey;
            }

            var nsbAuthDBConnStr = configuration.GetConnectionString("OcelotConfigDB");
            var optionsBuilder = new DbContextOptionsBuilder<OcelotConfigDbContext>();
            //var migrationsAssembly = GetType().GetTypeInfo().Assembly.GetName().Name;
            //optionsBuilder.UseSqlServer(nsbAuthDBConnStr,sql => sql.MigrationsAssembly(migrationsAssembly));
            optionsBuilder.UseSqlServer(nsbAuthDBConnStr);
            _ocelotConfigurationContext = new OcelotConfigDbContext(optionsBuilder.Options);
            //_ocelotConfigurationContext = serviceProvider.GetService<OcelotConfigDbContext>();
           
        }

        public void MigrateDb() {
           // _ocelotConfigurationContext.Database.Migrate();
        }

        public async Task<Response<FileConfiguration>> Get() {
            var config = _cache.Get(_configurationKey, _configurationKey);

            if (config != null) 
                return new OkResponse<FileConfiguration>(config);

            var ocelotCfg = (from o in _ocelotConfigurationContext.ConfigModels where o.Section == OcelotConfigurationSection.All select o).FirstOrDefault();

            if (ocelotCfg==null)
                return new OkResponse<FileConfiguration>(null);            

            var consulConfig = JsonConvert.DeserializeObject<FileConfiguration>(ocelotCfg.Payload); 

            return new OkResponse<FileConfiguration>(consulConfig);
        }

        public async Task<Response> Set(FileConfiguration ocelotConfiguration) {
            try {
                var cfgPayload = JsonConvert.SerializeObject(ocelotConfiguration, Formatting.Indented);
                var ocelotCfg = from g in _ocelotConfigurationContext.ConfigModels where g.Section == OcelotConfigurationSection.All select g.Id;
                if (ocelotCfg.Count() == 0) {
                    _ocelotConfigurationContext.ConfigModels.Add(new OcelotConfigurationModel {
                        Section = OcelotConfigurationSection.All,
                        Payload = cfgPayload
                    });
                    await _ocelotConfigurationContext.SaveChangesAsync();
                }
                else {
                    var idList = ocelotCfg.ToList();
                    var id1 = idList[0];
                    var ocelotItem = _ocelotConfigurationContext.ConfigModels.Where(o => o.Id == id1).First();
                    ocelotItem.Payload = cfgPayload;
                    for (int i = 1; i < idList.Count; i++)
                        _ocelotConfigurationContext.ConfigModels.Remove(_ocelotConfigurationContext.ConfigModels.Where(o => o.Id == idList[i]).First());
                    await _ocelotConfigurationContext.SaveChangesAsync();
                }
                return new OkResponse();
            }
            catch(Exception ex) {
                return new ErrorResponse(new SetConfigInSqlServerError($"Failed to set FileConfiguration in sql server, error message:{ex.Message}"));
            }
        }
    }
}
