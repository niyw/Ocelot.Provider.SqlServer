using Microsoft.EntityFrameworkCore;

namespace Ocelot.Provider.SqlServer {
    public class OcelotConfigDbContext : DbContext {
        private readonly OcelotConfigDbConfiguration _ocelotConfigDbConfiguration = null;
        public OcelotConfigDbContext(DbContextOptions<OcelotConfigDbContext> options, OcelotConfigDbConfiguration ocelotConfigDbConfiguration)
            : base(options) {
            _ocelotConfigDbConfiguration = ocelotConfigDbConfiguration;
        }
        public DbSet<OcelotConfigurationModel> ConfigModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<OcelotConfigurationModel>().ToTable(_ocelotConfigDbConfiguration.ConfigTableName);
        }
    }
}
