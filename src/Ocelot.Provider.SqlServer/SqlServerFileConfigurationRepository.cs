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

    public class SqlServerFileConfigurationRepository : IFileConfigurationRepository {
        private readonly string _configurationKey;
        private readonly Cache.IOcelotCache<FileConfiguration> _cache;
        private readonly IOcelotLogger _logger;
        private readonly OcelotConfigDbContext _ocelotConfigurationContext;
        public SqlServerFileConfigurationRepository(
            Cache.IOcelotCache<FileConfiguration> cache,
            IInternalConfigurationRepository repo,
            IConfiguration configuration,
            OcelotConfigDbConfiguration ocelotConfigDbConfiguration,
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

            var nsbAuthDBConnStr = configuration.GetConnectionString(ocelotConfigDbConfiguration.ConnectionName);
            if (string.IsNullOrWhiteSpace(nsbAuthDBConnStr))
                nsbAuthDBConnStr = ocelotConfigDbConfiguration.ConnectionString;

            var optionsBuilder = new DbContextOptionsBuilder<OcelotConfigDbContext>();
            optionsBuilder.UseSqlServer(nsbAuthDBConnStr);
            _ocelotConfigurationContext = new OcelotConfigDbContext(optionsBuilder.Options, ocelotConfigDbConfiguration);
        }

        public async Task<Response<FileConfiguration>> Get() {
            _logger.LogInformation("Get route rules");
            var config = _cache.Get(_configurationKey, _configurationKey);
            if (config == null) {
                var ocelotCfg = await (from o in _ocelotConfigurationContext.ConfigModels where o.Section == OcelotConfigurationSection.All select o).FirstOrDefaultAsync();
                _logger.LogInformation("Get route rules from DB");
                if (ocelotCfg != null) {
                    config = JsonConvert.DeserializeObject<FileConfiguration>(ocelotCfg.Payload);
                    _cache.AddAndDelete(_configurationKey, config, new TimeSpan(0, 0, 15), null);
                }
            }
            return new OkResponse<FileConfiguration>(config);
        }

        public async Task<Response> Set(FileConfiguration ocelotConfiguration) {
            try {
                _logger.LogInformation("Set route rules");
                var cfgPayload = JsonConvert.SerializeObject(ocelotConfiguration, Formatting.Indented);
                var ocelotCfg = from g in _ocelotConfigurationContext.ConfigModels where g.Section == OcelotConfigurationSection.All select g.Id;
                if (ocelotCfg.Count() == 0) {
                    _ocelotConfigurationContext.ConfigModels.Add(new OcelotConfigurationModel
                    {
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
                _cache.AddAndDelete(_configurationKey, ocelotConfiguration, new TimeSpan(0, 0, 15), null);
                _logger.LogInformation("Set route rules cache");
                return new OkResponse();
            }
            catch (Exception ex) {
                return new ErrorResponse(new SetConfigInSqlServerError($"Failed to set FileConfiguration in sql server, error message:{ex.Message}"));
            }
        }
    }
}
