using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    public class ControlPanel : FlowLayoutPanel {

        public CheckedListBox listBox = new CheckedListBox();

        public ControlPanel() {
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
            
            Controls.Add(listBox);
        }

    }
}
