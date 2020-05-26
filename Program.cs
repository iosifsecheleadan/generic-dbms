using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using new_GenericDBMS.project.configuration;
using new_GenericDBMS.project.ctrl;
using new_GenericDBMS.project.ui;

namespace new_GenericDBMS.project {
    internal static class Program {
        public static void Main(string[] args) {
            ConnectionDataSection connectionsData = (ConnectionDataSection) ConfigurationManager.GetSection("DBConnections");
            List<OneToManyController> controllers = new List<OneToManyController>();
            foreach (ConnectionData instance in connectionsData.connectionDataInstances) {
                OneToManyController controller = new OneToManyController(instance);
                controllers.Add(controller);
                Console.WriteLine(instance.ToString());
            }
            
            Graphic gui = new Graphic(controllers);
            Application.Run(gui.getMain());
        }
    }
}