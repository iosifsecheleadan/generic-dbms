using System.Configuration;

namespace new_GenericDBMS.project.configuration {
    public class ConnectionData : ConfigurationElement {
        public ConnectionData() { }

        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string id {
            get { return (string) base["id"]; }
        }
        
        [ConfigurationProperty("host", IsRequired = false, DefaultValue = "127.0.0.1")]
        public string host {
            get { return (string) base["host"]; }
        }

        [ConfigurationProperty("database", IsRequired = true)]
        public string database {
            get { return (string) base["database"]; }
        }

        [ConfigurationProperty("user", IsRequired = true)]
        public string user {
            get { return (string) base["user"]; }
        }

        [ConfigurationProperty("passwd", IsRequired = true)]
        public string passwd {
            get { return (string) base["passwd"]; }
        }

        [ConfigurationProperty("tableOne", IsRequired = true)]
        public string tableOne {
            get { return (string) base["tableOne"]; }
        }
        
        [ConfigurationProperty("tableMany", IsRequired = true)]
        public string tableMany {
            get { return (string) base["tableMany"]; }
        }

        [ConfigurationProperty("foreignKey", IsRequired = true)]
        public string foreignKey {
            get { return (string) base["foreignKey"];  }
        }
        
        public override string ToString() {
            return $"{this.id} - {this.host} - {this.database} - {this.tableOne} - {this.tableMany} - {this.foreignKey}";
        }
    }
}
