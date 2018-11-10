using Microsoft.EntityFrameworkCore;

namespace Ocelot.Provider.SqlServer {
    public class OcelotConfigDbContext : DbContext {
        public OcelotConfigDbContext(DbContextOptions<OcelotConfigDbContext> options)
            : base(options) { }
        public DbSet<OcelotConfigurationModel> ConfigModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<OcelotConfigurationModel>().ToTable("Ocelot_Configs");
        }
    }
}
