using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using new_GenericDBMS.project.configuration;
using new_GenericDBMS.project.repo;

namespace new_GenericDBMS.project.ctrl {
    public class OneToManyController {
        public readonly TableRepository Parent;
        public readonly TableRepository Child;
        private readonly string foreignKey;
        
        public OneToManyController(ConnectionData instance) {
            SqlConnection connection = new SqlConnection($"Server={instance.host};" +
                                                         $"Database={instance.database};" +
                                                         $"User Id={instance.user};" +
                                                         $"Password={instance.passwd}");
            Console.WriteLine($"Connected with {connection.ConnectionString}");
            this.Parent = new TableRepository(connection, instance.tableOne);
            this.Child = new TableRepository(connection, instance.tableMany);
            Console.WriteLine($"Parent is {this.Parent.getTableName()} and Child is {this.Child.getTableName()}");
            this.foreignKey = instance.foreignKey;
        }

        public DataRow[] selectFromParent(string select = null) { return this.Parent.selectWhere(select); }
        public DataRow[] selectFromChild(string select = null) { return this.Child.selectWhere(select); }

        public DataRow[] selectChildrenOfParent(string parentID, string select = null) {
            string compoundSelect = $"{this.foreignKey} = '{parentID}'";
            if (select != null) {
                compoundSelect += $" and {select}";
            }
            return this.Child.selectWhere(compoundSelect);
        }

        public int insertIntoParent(List<string> values) { return this.Parent.insertInto(values); }
        public int insertIntoChild(List<string> values) { return this.Child.insertInto(values); }

        public int updateParent(string set, string where = null) { return this.Parent.updateWhere(set, where); }
        public int updateChild(string set, string where = null) { return this.Child.updateWhere(set, where); }

        public int deleteParent(string where = null) { return this.Parent.deleteWhere(where); }
        public int deleteChild(string where = null) { return this.Child.deleteWhere(where); }

        public void close() {
            this.Parent.close();
            this.Child.close();
        }
    }
}