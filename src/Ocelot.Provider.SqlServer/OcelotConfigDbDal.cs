using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ocelot.Provider.SqlServer {
    public class OcelotConfigDbDal {
        public string DbConnectionString { get;  }
        public OcelotConfigDbDal(string dbConnectionString) {
            this.DbConnectionString = dbConnectionString;
        }
        public List<OcelotConfigurationModel> GetAllConfigs() {
            using (IDbConnection conn = new SqlConnection(DbConnectionString)) {
                return conn.Query<OcelotConfigurationModel>("SELECT [Id],[Section],[Payload],[CreateTime],[LastUpdate] FROM [dbo].[Ocelot_Configs]").ToList();
            }
        }
        public OcelotConfigurationModel GetOcelotConfigBySection(string section) {
            using (IDbConnection conn = new SqlConnection(DbConnectionString)) {
                return conn.Query<OcelotConfigurationModel>("SELECT * FROM [dbo].[Ocelot_Configs] Where Section=@Section;",new { Section= section }).FirstOrDefault();
            }
        }
        public void InsertRequestLogs(OcelotConfigurationModel ocelotConfig) {
            using (IDbConnection conn = new SqlConnection(DbConnectionString)) {
                conn.Execute("INSERT INTO[dbo].[Ocelot_Configs]([Section],[Payload],[CreateTime],[LastUpdate])VALUES(@Section, @Payload, @CreateTime, @LastUpdate)", ocelotConfig);
            }
        }
        public int Update(OcelotConfigurationModel ocelotConfig) {
            ocelotConfig.LastUpdate = DateTime.Now;
            using (IDbConnection connection = new SqlConnection(DbConnectionString)) {
                return connection.Execute("UPDATE[dbo].[Ocelot_Configs] SET [Section] = @Section,[Payload] = @Payload,[LastUpdate] = @LastUpdate WHERE Id=@Id", ocelotConfig);
            }
        }
    }
}
