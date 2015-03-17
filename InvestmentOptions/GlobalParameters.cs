using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace InvestmentOptions {
    public class GlobalParameters : GroupBox {
        public MyBoolean includeWearAndTear = new MyBoolean("includeWearAndTear");
        public ProjectionForm form;

        public GlobalParameters(ProjectionForm form) {
            this.form = form;
            Text = "globalParameters";
            Width = 10; //weird, if don't set this, then width stays bigger than tableLayout,
            //even with the Dock below...
            //Seems Fill will only EXPAND a control to fill... Not shrink it to fill...
            //basically, winforms is crap! Lets NEVER use it again! Don't try anything clever in it.  
            Dock = DockStyle.Fill;
            //groupBox.Margin = new Padding(0);
            //groupBox.BorderStyle = BorderStyle.FixedSingle;
            addCheckBox(includeWearAndTear);
        }

        public void addCheckBox(MyBoolean myBoolean) {
            CheckBox checkBox = new CheckBox();
            Controls.Add(checkBox);
            checkBox.Name = myBoolean.name;
            checkBox.Text = checkBox.Name;
            foreach (InvestmentOption option in form.optionsList) {
                addBinding(checkBox, option.realWorldTree.property.includeWearAndTear);
            }
           
        }

        public void addBinding(CheckBox checkBox, Object dataSource) { //data member is the property, to bind.
            MyBoolean myBoolean = (MyBoolean) dataSource;
            Binding binding = new Binding("Checked", dataSource, "value");
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            checkBox.DataBindings.Add(binding);
            //Does not need to subscribe... does not change...
            //BUT the MYBOOLEANS should subscribe to THIS CHECKBOX!!!
            //NO!! BEcause we set the binding here!!!
        }
    }
}
