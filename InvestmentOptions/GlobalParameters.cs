using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace InvestmentOptions {
    public class GlobalParameters : GroupBox {
        public ProjectionForm form;

        public GlobalParameters(ProjectionForm form) {
            this.form = form;
            Text = "globalParameters";
            Width = 10; //weird, if don't set this, then width stays bigger than tableLayout,
            //even with the Dock below...
            //Seems Fill will only EXPAND a control to fill... Not shrink it to fill...
            //basically, winforms is crap! Lets NEVER use it again! Don't try anything clever in it.  
            Dock = DockStyle.Fill;
            //Controls.Add(new CheckBox());
            //groupBox.Margin = new Padding(0);
            //groupBox.BorderStyle = BorderStyle.FixedSingle;
        }

        public void addCheckBox(MyBoolean myBoolean) {
            CheckBox checkBox = new CheckBox();
            Controls.Add(checkBox);
            checkBox.Name = myBoolean.name;
            checkBox.Text = checkBox.Name;
            //foreach (InvestmentOption option in form.optionsList) {
            //  addBinding(checkBox, option.realWorldTree.property.includeWearAndTear);
            //} 
            //OK, so it does not like one-to-many binding... but is there way around it?
            //No, I don't there is. Just use static variable, or variable in controlPanel...
            addBinding(checkBox, myBoolean);
            checkBox.Dock = DockStyle.Bottom;
        }

        public void addCheckBoxes() {
            addCheckBox(Property.includeWearAndTear);
            addCheckBox(InvestmentOption.countRentSavingsAsIncome);
            //Doing it from here, only way is to RETRIEVE A LIST OF BOOLEANS....
            //for each option....
            // myBool = option.includeWearAndTear...
            // add it to list...
            // then call checkBox, 
            //Must CALL IT (upward) from OnCHecked (to make sure up to date).
            //OR!!! simply, just BIND this value here, to a STATIC variable in optoins,
            //which all options refer to... YES!!! (beauty is, that was how was gonna do it,
            //BUT, did not want to REFER to form in option... (NOW don't have to!!!).
            //OTher way, would be to use Strings (yuck), or lists... (means bit of duplicate code).
            //NOTE!!! how did I do it in TreeView???
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
