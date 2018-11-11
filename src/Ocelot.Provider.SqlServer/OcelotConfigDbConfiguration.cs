using System;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.Provider.SqlServer {
    public class OcelotConfigDbConfiguration {
        public string ConnectionName { get; set; } = "OcelotConfigDB";
        public string ConnectionString { get; set; } = string.Empty;
        public string ConfigTableName { get; set; } = "Ocelot_Configs";
    }
}
