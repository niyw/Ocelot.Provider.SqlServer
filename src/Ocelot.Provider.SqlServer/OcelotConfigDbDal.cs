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
        public string OcelotConfigTableName { get; }
        public OcelotConfigDbDal(OcelotConfigDbConfiguration configDbConfiguration) {
            this.DbConnectionString = configDbConfiguration.ConnectionString;
            OcelotConfigTableName = configDbConfiguration.ConfigTableName;
            Sql_QueryAllConfig = $"SELECT [Id],[Section],[Payload],[CreateTime],[LastUpdate] FROM [dbo].[{OcelotConfigTableName}]";
            Sql_QueryBySection = $"SELECT * FROM [dbo].[{OcelotConfigTableName}] Where Section=@Section;";
            Sql_Insert = $"INSERT INTO[dbo].[{OcelotConfigTableName}]([Section],[Payload],[CreateTime],[LastUpdate])VALUES(@Section, @Payload, @CreateTime, @LastUpdate)";
            Sql_Update = $"UPDATE[dbo].[{OcelotConfigTableName}] SET [Section] = @Section,[Payload] = @Payload,[LastUpdate] = @LastUpdate WHERE Id=@Id";
        }
        private string Sql_QueryAllConfig = string.Empty;
        public List<OcelotConfigurationModel> GetAllConfigs() {
            using (IDbConnection conn = new SqlConnection(DbConnectionString)) {
                return conn.Query<OcelotConfigurationModel>(Sql_QueryAllConfig).ToList();
            }
        }
        private string Sql_QueryBySection = string.Empty;
        public OcelotConfigurationModel GetOcelotConfigBySection(string section) {
            using (IDbConnection conn = new SqlConnection(DbConnectionString)) {
                return conn.Query<OcelotConfigurationModel>(Sql_QueryBySection, new { Section= section }).FirstOrDefault();
            }
        }
        private string Sql_Insert = string.Empty;
        public void InsertRequestLogs(OcelotConfigurationModel ocelotConfig) {
            using (IDbConnection conn = new SqlConnection(DbConnectionString)) {
                conn.Execute(Sql_Insert, ocelotConfig);
            }
        }
        private string Sql_Update = string.Empty;
        public int Update(OcelotConfigurationModel ocelotConfig) {
            ocelotConfig.LastUpdate = DateTime.Now;
            using (IDbConnection connection = new SqlConnection(DbConnectionString)) {
                return connection.Execute(Sql_Update, ocelotConfig);
            }
        }
    }
}
