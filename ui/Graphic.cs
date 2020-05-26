using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using new_GenericDBMS.project.ctrl;

namespace new_GenericDBMS.project.ui {
    public class Graphic {
        private List<OneToManyPanel> panels;
        private Form mainForm;
        private TableLayoutPanel mainPanel;
        
        public Graphic(IReadOnlyList<OneToManyController> controllers, string title = null) {
            this.initializePanel(controllers);
            this.initializeForm(title);
        }

        private void initializePanel(IReadOnlyList<OneToManyController> controllers) {
            this.mainPanel = new TableLayoutPanel { 
                Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle, 
                Dock = DockStyle.Fill,
                ColumnCount = controllers.Count,
                RowCount = 1,
            };

            this.panels = new List<OneToManyPanel>();
            for(int index = 0; index < controllers.Count; index += 1) {
                this.mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (int) (100 / controllers.Count)));
                this.panels.Add(new OneToManyPanel(controllers[index]));
                this.mainPanel.Controls.Add(this.panels[index], index, 0);
            }
        }
        
        private void initializeForm(string title) {
            this.mainForm = new Form() {Text = title ?? "Generic DataBase Management System"};
            this.mainForm.Controls.Add(this.mainPanel);
            this.mainForm.Shown += this.openingMessage;
            this.mainForm.FormClosing += this.onClose;
            this.mainForm.Size = new Size(1000, 500);
            this.mainForm.MinimumSize = new Size(800, 400);
        }

        private void onClose(object sender, FormClosingEventArgs e) {
            foreach (OneToManyPanel panel in this.panels) {
                panel.close();
            }
        }

        public Form getMain() {
            return this.mainForm;
        }

        private void openingMessage(object sender, EventArgs e) {
            MessageBox.Show($"For Table Add, input data like" +
                            $"\n\t\"value,value,value\" - without ID column" +
                            $"\nFor update Table Update, input data like" +
                            $"\n\t\"column=newValue,column=newValue,...\" " +
                            $"\n\tfor the columns you want to update");
        }
    }
}