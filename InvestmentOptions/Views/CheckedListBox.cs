using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    public class CheckedListBox : System.Windows.Forms.CheckedListBox {
        //Solutions in binding a datasource to a CheckedListBox aren't very elegant. 
        //Use a DataGridView with a Checkbox column instead.
        ProjectionForm form;
        public int checkedItemsCount = 0;

        public CheckedListBox(ProjectionForm form) {
            this.form = form;
            Dock = DockStyle.Fill;
            //Width = Parent.Width;
            //Height = 150;
            //BorderStyle = BorderStyle.FixedSingle; //does funny things...
            //Margin = new Padding(0);
        }

        protected override void OnItemCheck(ItemCheckEventArgs checkEvent) {
            base.OnItemCheck(checkEvent);
            if (checkEvent.NewValue == CheckState.Checked) {
                form.OptionsList[checkEvent.Index].showInPanel = true;
                checkedItemsCount++;
            }
            if (checkEvent.NewValue == CheckState.Unchecked) {
                form.OptionsList[checkEvent.Index].showInPanel = false;
                checkedItemsCount--;
            }
            form.UpdatePanels(this, null);
            //TBH, SHOULD not have to do this HERE! 
            //the things that need updating, should listen for the cue, and update themselves!
            //fits better with the idea that objects do stuff to themselves, and look after themselves!
            //i.e. objects DON'T NEED conducting, or orchestrating...
        } 
    }
}
