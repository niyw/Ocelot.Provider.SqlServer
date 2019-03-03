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
            modelBuilder.Entity<OcelotConfigurationModel>().Property(o => o.Section).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<OcelotConfigurationModel>().Property(o => o.Payload).IsRequired();
            modelBuilder.Entity<OcelotConfigurationModel>().Property(o => o.CreateTime).HasDefaultValue(DateTime.Now).IsRequired();
            modelBuilder.Entity<OcelotConfigurationModel>().Property(o => o.LastUpdate).HasDefaultValue(DateTime.Now).IsRequired();
        }
    }
}
