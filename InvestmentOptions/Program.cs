using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InvestmentOptions {
    
    static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //NOW, can treat this as the conductor... (I think)...
            OptionDictator optionDictator = new OptionDictator(); //This is basically the conductor now...
            //Can't make this the conductor, because cannot call non-static methods. 
            //Need to create the conductor.                    
        }
    }
}
