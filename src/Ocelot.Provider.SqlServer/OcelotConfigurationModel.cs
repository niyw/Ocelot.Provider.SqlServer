using System;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.Provider.SqlServer {
    public class OcelotConfigurationModel {
        public int Id { get; set; }
        public string Section { get; set; } = OcelotConfigurationSection.All;
        public string Payload { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
