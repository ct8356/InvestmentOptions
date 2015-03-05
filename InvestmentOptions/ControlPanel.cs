using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace InvestmentOptions {

    public class ControlPanel : FlowLayoutPanel {
        public ProjectionForm form;
        public MyCheckedListBox listBox;
        //public Button refreshButton = new Button();
        public MyTreeView realWorldTreeView;

        public ControlPanel(ProjectionForm form) {
            //PANEL STUFF
            this.form = form;
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
            listBox = new MyCheckedListBox(form);
            Controls.Add(listBox);
            listBox.Size = new Size(280,130);
            //Controls.Add(refreshButton);
            InvestmentOption dudOption = new InvestmentOption(); //this is the DUD constructor.
            realWorldTreeView = new MyTreeView(dudOption); //needs to be like this,
            realWorldTreeView.setDefaultNodeToChartMapping();
            realWorldTreeView.ExpandAll();
            //since atm, RWT made inside the option...
            //needs to be, since each RWT needs an option!
            //EXCEPT this one!
            //WELL, this one can have his own dud option!
            initialiseRealWorldTreeView();
            Controls.Add(realWorldTreeView);
            BorderStyle = BorderStyle.FixedSingle;
            Size = new Size(300, 740);
        }

        public void initialiseRealWorldTreeView() {
            realWorldTreeView.Size = new Size(280, 580); //might be better to make it property of panel,
            //then pass it to option, to have stuff added to it...
        }
    }
}
