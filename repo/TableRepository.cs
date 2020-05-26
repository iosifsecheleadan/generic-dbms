using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using new_GenericDBMS.project.domain;

namespace new_GenericDBMS.project.repo {
    public class TableRepository {
        private readonly SqlConnection connection;
        public readonly DataTable table;
        private readonly string arguments;
        private readonly string primaryKey;

        public TableRepository(SqlConnection connection, string name) {
            this.connection = connection;
            try { this.connection.Open();
            } catch(InvalidOperationException ignored) {}

            this.table = new DataTable(name);
            Console.Write($"New Table {name}");
            this.refreshTable();

            if (this.table.PrimaryKey.Length > 1) throw new TableException("Cannot handle Tables with Compound Primary Keys.");
            else if (this.table.PrimaryKey.Length == 1) this.primaryKey = this.table.PrimaryKey[0].ColumnName;
            Console.Write($" with primary key {this.primaryKey}\n");
            
            this.arguments = "(";
            foreach (DataColumn column in this.table.Columns) {
                if (column.ColumnName == this.primaryKey) continue;
                this.arguments += column.ColumnName + ", ";
            }
            if (this.arguments != "(") {
                this.arguments = this.arguments.Substring(0, this.arguments.Length - 2);
                this.arguments += ")";
            } else this.arguments = "";
            
            Console.WriteLine(this.arguments);
        }

        public DataRow[] selectWhere(string select = null) {
            this.refreshTable();
            return table.Select(select);
        }

        public DataRow selectByIDWhere(string ID) {
            this.refreshTable();
            DataRow[] rows = table.Select($"{this.primaryKey} = {ID}");
            if (rows.Length != 1) throw new TableException("No Row with given ID");
            return rows[0];
        }

        public int insertInto(List<string> values) {
            string commandText = $"insert into {this.table.TableName + this.arguments} values ";
            for (int index = 0; index < values.Count - 1; index += 1) {
                commandText += $"('{values[index].Replace(",", "','")}'), ";
            } 
            commandText += $"('{values[values.Count - 1].Replace(",", "','")}');";
            /*
                // todo delete printing here
                Console.WriteLine(commandText);
                return 0;
            */
            return this.executeCommand(commandText);
        }

        public int updateWhere(string set, string where = null) {
            string commandText = $"update {this.table.TableName}" +
                $" set {set.Replace("=", "='").Replace(",", "','")}'";
            if (where != null) {
                commandText += $" where {where}";
            }
            /*
                // todo delete printing here
                Console.WriteLine(commandText);
                return 0;
            */
            return this.executeCommand(commandText);
        }

        public int deleteWhere(string where = null) {
            string commandText = $"delete from {this.table.TableName}";
            if (where != null) {
                commandText += $" where {where}";
            }
            /*
                // todo delete printing hereF
                Console.WriteLine(commandText);
                return 0;
            */
            return this.executeCommand(commandText);
        }

        public string getForeignKey(string tableName) {
            ForeignKeyConstraint foreignKey = null;
            foreach (Constraint constraint in this.table.Constraints) {
                if (! (constraint is ForeignKeyConstraint)) continue;
                ForeignKeyConstraint foreignKeyConstraint = (ForeignKeyConstraint) constraint;
                
                if (foreignKeyConstraint.RelatedTable.TableName != tableName) continue;
                
                if (foreignKey != null) throw new 
                    TableException("Cannot handle Tables with multiple ForeignKeys to same table.");
                foreignKey = foreignKeyConstraint;
            }
            if (foreignKey == null) throw new TableException("No Foreign Key");
            if (foreignKey.Columns.Length > 1) throw new TableException("Cannot handle Tables with Compound Foreign Keys.");
            return foreignKey.Columns[0].ColumnName.ToString();
        }

        public string getPrimaryKey() {
            return this.primaryKey;
        }
        
        public string getTableName() {
            return this.table.TableName;
        }

        public void close() {
            this.connection.Close();
        }
        
        private int executeCommand(string text) {
            int noAffectedRows = new SqlCommand(text, this.connection).ExecuteNonQuery();
            this.refreshTable();
            return noAffectedRows;
        }

        public void refreshTable() {
            string commandText = $"select * from {this.table.TableName}";
            SqlDataAdapter adapter = new SqlDataAdapter(commandText, this.connection);
            this.table.Clear();
            adapter.Fill(this.table);
            adapter.FillSchema(this.table, SchemaType.Source);
        }
    }
}