using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace InvestmentOptions {

    public class ControlPanel : TableLayoutPanel {
        public ProjectionForm Form { get; set; }
        public CheckedListBox ListBox { get; set; }
        public TreeView realWorldTreeView;
        public GlobalParameters globalParameters;

        public ControlPanel(ProjectionForm form) {
            Form = form;
            //CHILDREN
            globalParameters = new GlobalParameters(form);
            InvestmentOption dudOption = new InvestmentOption("dud"); 
            //this is the DUD constructor.
            realWorldTreeView = new TreeView(dudOption); 
            //needs to be like this,
            //since atm, RWTree made inside the option.
            //needs to be, since each RWTree needs an option!
            //EXCEPT this one!
            //WELL, this one can have his own dud option!
            realWorldTreeView.ExpandAll();
            realWorldTreeView.Dock = DockStyle.Fill;
            realWorldTreeView.BorderStyle = BorderStyle.FixedSingle;
            //realWorldTreeView.Margin = new Padding(0);
            //CONTROL PANEL
            Text = "Christiaan Panel";
            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Fill;
            DefineChildrenOrder();
        }

        public void DefineChildrenOrder() {
            //sibling order is important, 
            //especially if docking...
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(ListBox = new CheckedListBox(Form));
            Controls.Add(globalParameters);
            Controls.Add(realWorldTreeView);
            //OK MATE! Pretty sure, that tableLayoutPanel is bit lame...
            //HAVE TO increment rowCount yourself!! WACK!
            //But now you get that, its fine.
            //So, Add(), with one arg, DOES add another row, essentially...
            //BUT it will not count that in rowCount, unless you tell it to... WACK!
        }

    }
}
