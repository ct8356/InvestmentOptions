using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InvestmentOptions {

    static class Program {

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new ProjectionForm();
            //Only 1 form can run at a time...
            OptionDictator optionDictator = new OptionDictator(form);
            //MUST create form first, because controlPanel sets tree structure for the options...
            //Then must create options, before can bind the form to the options!!!
            //ACTUALLY, should make OPTIONS first... They will all have identical structure...
            //Create Data first, THEN decide how to display it... REVISIT
            form.DoStuffWithOptionsList(optionDictator.Options);
            //Can't make Program the conductor, because it cannot call non-static methods.
            //Except that it can?
            Application.Run(form);
            //Does not continue with the rest of code until form is closed.
            //Put it at the end.
        }
    }
}
