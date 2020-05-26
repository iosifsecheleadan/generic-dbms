using System.Configuration;

namespace new_GenericDBMS.project.configuration {
    public class ConnectionDataSection : ConfigurationSection {
        
        [ConfigurationProperty("instances")]
        [ConfigurationCollection(typeof(ConnectionDataCollection))]
        public ConnectionDataCollection connectionDataInstances {
            get { return (ConnectionDataCollection) this["instances"]; }
        }
        
    }
}