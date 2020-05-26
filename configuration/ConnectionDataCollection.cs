using System.Configuration;

namespace new_GenericDBMS.project.configuration {
    public class ConnectionDataCollection : ConfigurationElementCollection {
        public ConnectionData this[int index] {
            get { return (ConnectionData) BaseGet(index);  }
            set {
                if (BaseGet(index) != null) {
                    BaseRemoveAt(index);
                } BaseAdd(index, value);
            }
        }

        public new ConnectionData this[string key] {
            get { return (ConnectionData) BaseGet(key); }
            set {
                if (BaseGet(key) != null) {
                    BaseRemoveAt(BaseIndexOf((BaseGet(key))));
                } BaseAdd(value);
            }
        }
        
        protected override ConfigurationElement CreateNewElement() {
            return new ConnectionData();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            return ((ConnectionData) element).id;
        }
    }
}