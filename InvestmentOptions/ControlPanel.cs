using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    public class ControlPanel : FlowLayoutPanel {

        public MyCheckedListBox listBox;
        public Button refreshButton = new Button();

        public ControlPanel(ProjectionForm form) {
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
            listBox = new MyCheckedListBox(form);
            Controls.Add(listBox);
            Controls.Add(refreshButton);
        }

    }
}
