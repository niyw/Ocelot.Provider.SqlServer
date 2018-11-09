using Microsoft.EntityFrameworkCore;

namespace Ocelot.Provider.SqlServer {
    public class OcelotConfigurationContext : DbContext {
        public OcelotConfigurationContext(DbContextOptions<OcelotConfigurationContext> options)
            : base(options) { }
        public DbSet<OcelotConfigurationModel> ConfigModels { get; set; }
    }
}
