using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using new_GenericDBMS.project.ctrl;
using new_GenericDBMS.project.ui.graphic;

namespace new_GenericDBMS.project.ui {
    public class OneToManyPanel : TableLayoutPanel {
        private readonly OneToManyController controller;
        private readonly string parentName;
        private readonly string childName;

        private BasicDataGridView parentTable;
        private BasicLabel parentDataLabel;
        private BasicTextBox parentDataEdit;

        private BasicButton parentAddButton;
        private BasicButton parentUpdateButton;
        private BasicButton parentRemoveButton;
        
        
        private BasicDataGridView childTable;
        private BasicLabel childDataLabel;
        private BasicTextBox childDataEdit;

        private BasicButton childAddButton;
        private BasicButton childUpdateButton;
        private BasicButton childRemoveButton;
        
        public OneToManyPanel(OneToManyController controller) {
            this.controller = controller;
            this.parentName = this.controller.Parent.getTableName();
            this.childName = this.controller.Child.getTableName();
            this.initializePanel();
            this.initializeTable();
            this.initializeButtons();
            
            this.refreshTables();
        }
        
        private void initializePanel() {
            this.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Dock = DockStyle.Fill;
            this.ColumnCount = 2;
            this.RowCount = 6;
            
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
        }
        
        private void initializeTable() {
            this.parentTable = new BasicDataGridView(); // {DataSource = this.controller.Parent.table};
            this.parentTable.SelectionChanged += this.parentSelected;
            this.parentDataLabel = new BasicLabel($"{this.parentName} Data");
            string parentToolTip =
                $"For {this.parentName} Add, input data like" +
                $"\n\t\"value,value,value\" - without ID column" +
                $"\nFor update {this.parentName} Update, input data like" +
                $"\n\t\"column=newValue,column=newValue,...\" " +
                $"\n\tfor the columns you want to update";
            new ToolTip().SetToolTip(this.parentDataLabel, parentToolTip);
            this.parentDataEdit = new BasicTextBox($"Input {this.parentName} data ...");
            new ToolTip().SetToolTip(this.parentDataEdit, parentToolTip);
            
            this.Controls.Add(this.parentDataLabel, 0, 0);
            this.Controls.Add(this.parentTable, 0, 1);
            this.Controls.Add(this.parentDataEdit, 0, 2);

            this.childTable = new BasicDataGridView(); // {DataSource = this.controller.Child.table};
            this.childDataLabel = new BasicLabel($"{this.childName} Data");
            string childToolTip =
                $"For {this.childName} Add, input data like" +
                $"\n\t\"value,value,value\" - without ID column" +
                $"\nFor update {this.childName} Update, input data like" +
                $"\n\t\"column=newValue,column=newValue,...\" " +
                $"\n\tfor the columns you want to update";
            new ToolTip().SetToolTip(this.childDataLabel, childToolTip);
            this.childDataEdit = new BasicTextBox($"Input {this.childName} data ...");
            new ToolTip().SetToolTip(this.childDataEdit, childToolTip);
            
            this.Controls.Add(this.childDataLabel, 1, 0);
            this.Controls.Add(this.childTable, 1, 1);
            this.Controls.Add(this.childDataEdit, 1, 2);
        }

        private void initializeButtons() {
            this.parentAddButton = new BasicButton($"Add {this.parentName}");
            this.parentAddButton.Click += this.addParent;
            new ToolTip().SetToolTip(this.parentAddButton, 
                $"Add new {this.parentName}");
            this.parentUpdateButton = new BasicButton($"Update {this.parentName}");
            this.parentUpdateButton.Click += this.updateParent;
            new ToolTip().SetToolTip(this.parentUpdateButton,
                $"Update selected {this.parentName} with new {this.parentName}");
            this.parentRemoveButton = new BasicButton($"Remove {this.parentName}");
            this.parentRemoveButton.Click += this.removeParent;
            new ToolTip().SetToolTip(this.parentRemoveButton,
                $"Remove selected {this.parentName}");
            
            this.Controls.Add(this.parentAddButton, 0, 3);
            this.Controls.Add(this.parentUpdateButton, 0, 4);
            this.Controls.Add(this.parentRemoveButton, 0, 5);
            
            this.childAddButton = new BasicButton($"Add {this.childName}");
            this.childAddButton.Click += this.addChild;
            new ToolTip().SetToolTip(this.childAddButton,
                $"Add new {this.childName}");
            this.childUpdateButton = new BasicButton($"Update {this.childName}");
            this.childUpdateButton.Click += this.updateChild;
            new ToolTip().SetToolTip(this.childUpdateButton,
                $"Update selected {this.childName} with new {this.childName}");
            this.childRemoveButton = new BasicButton($"Remove {this.childName}");
            this.childRemoveButton.Click += this.removeChild;
            new ToolTip().SetToolTip(this.childRemoveButton,
                $"Remove selected {this.childName}");
            
            this.Controls.Add(this.childAddButton, 1, 3);
            this.Controls.Add(this.childUpdateButton, 1, 4);
            this.Controls.Add(this.childRemoveButton, 1, 5);
        }

        private void refreshTables() {
            this.refreshParent();
            this.refreshChild();
        }

        private void refreshParent() {
            this.parentTable.Columns.Clear();
            this.parentTable.Rows.Clear();
            foreach (DataColumn column in this.controller.Parent.table.Columns) {
                this.parentTable.Columns.Add(column.ColumnName, column.ColumnName);
            }
            foreach (DataRow row in this.controller.Parent.table.Rows) {
                this.parentTable.Rows.Add(row.ItemArray);
            }
        }

        private void refreshChild() {
            this.childTable.Columns.Clear();
            this.childTable.Rows.Clear();
            foreach (DataColumn column in this.controller.Child.table.Columns) {
                this.childTable.Columns.Add(column.ColumnName, column.ColumnName);
            }
            foreach (DataRow row in this.controller.Child.table.Rows) {
                this.childTable.Rows.Add(row.ItemArray);
            }
        }

        private void parentSelected(object sender, EventArgs e) {
            try {
                List<string> parentIDs = this.getParentSelectedIDs();
                
                if (parentIDs.Count > 0) {
                    this.childTable.Rows.Clear();
                    foreach (string id in parentIDs) {
                        foreach (DataRow row in this.controller.selectChildrenOfParent(id)) {
                            this.childTable.Rows.Add(row.ItemArray);
                        }
                    }
                } else { this.refreshChild(); }
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
            }
        }

        private void addParent(object sender, EventArgs e) {
            try {
                this.controller.insertIntoParent(
                    new List<string>() {this.parentDataEdit.Text});
                this.refreshTables();
            } catch (SqlException exception) {
                this.Alert(exception.Message);
            }
        }

        private void updateParent(object sender, EventArgs e) {
            try {
                List<string> parentIDs = this.getParentSelectedIDs();
                if (parentIDs.Count > 1) {
                    this.Alert("Can't update multiple rows at once.");
                    return;
                }

                this.controller.updateParent(
                    this.parentDataEdit.Text,
                    $"{this.controller.Parent.getPrimaryKey()}={parentIDs[0]}");
                this.refreshParent();
            } catch (SqlException exception) { this.Alert(exception.Message); }
            catch (ArgumentOutOfRangeException) { this.Alert("No Row to Update Selected"); }
        }

        private void removeParent(object sender, EventArgs e) {
            try {
                List<string> parentIDs = this.getParentSelectedIDs();
                if (parentIDs.Count > 1) {
                    this.Alert("Can't update multiple rows at once.");
                    return;
                }

                this.controller.deleteParent($"{this.controller.Parent.getPrimaryKey()}='{parentIDs[0]}'");
                this.refreshTables();
            } catch (SqlException exception) { this.Alert(exception.Message); }
            catch (ArgumentOutOfRangeException) { this.Alert("No Row to Remove Selected"); }
        }

        private void addChild(object sender, EventArgs e) {
            try {
                this.controller.insertIntoChild(
                    new List<string>() {this.childDataEdit.Text});
                this.refreshChild();
            } catch (SqlException exception) {
                this.Alert(exception.Message);
            }
        }

        private void updateChild(object sender, EventArgs e) {
            try {
                List<string> childIDs = this.getChildSelectedIDs();
                if (childIDs.Count > 1) {
                    this.Alert("Can't update multiple rows at once.");
                    return;
                }

                this.controller.updateChild(
                    this.childDataEdit.Text,
                    $"{this.controller.Child.getPrimaryKey()}={childIDs[0]}");
                this.refreshChild();
            } catch (SqlException exception) { this.Alert(exception.Message); }
            catch (ArgumentOutOfRangeException) { this.Alert("No Row to Update Selected"); }
        }

        private void removeChild(object sender, EventArgs e) {
            try {
                List<string> childIDs = this.getChildSelectedIDs();
                if (childIDs.Count > 1) {
                    this.Alert("Can't update multiple rows at once.");
                    return;
                }

                this.controller.deleteChild($"{this.controller.Child.getPrimaryKey()}='{childIDs[0]}'");
                this.refreshChild();
            } catch (SqlException exception) { this.Alert(exception.Message); } 
            catch (ArgumentOutOfRangeException) { this.Alert("No Row to Remove Selected"); }
        }

        private List<string> getParentSelectedIDs() {
            string primaryKey = this.controller.Parent.getPrimaryKey();
            int index = 0;
            foreach (DataGridViewColumn column in this.parentTable.Columns) {
                if (column.Name == primaryKey) break;
                index += 1;
            }
            
            List<string> parentIDs = new List<string>();
            try {
                foreach (DataGridViewRow row in this.parentTable.SelectedRows) {
                    parentIDs.Add(row.Cells[index].Value.ToString().Trim());
                }
            } catch (ArgumentOutOfRangeException) {}

            return parentIDs;
        }

        private List<string> getChildSelectedIDs() {
            string primaryKey = this.controller.Child.getPrimaryKey();
            int index = 0;
            foreach (DataGridViewColumn column in this.childTable.Columns) {
                if (column.Name == primaryKey) break;
                index += 1;
            }
            
            List<string> childIDs = new List<string>();
            try {
                foreach (DataGridViewRow row in this.childTable.SelectedRows) {
                    childIDs.Add(row.Cells[index].Value.ToString().Trim());
                }
            } catch(ArgumentOutOfRangeException) {}

            return childIDs;
        }


        private void Alert(string message) {
            MessageBox.Show(message);
        }
        
        public void close() {
            this.controller.close();
        }
    }
}