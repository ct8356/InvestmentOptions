using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    public class MyCheckedListBox : CheckedListBox {
        ProjectionForm form;
        public int checkedItemsCount = 0;

        public MyCheckedListBox(ProjectionForm form) {
            this.form = form;
            //This is a real mess. Let us not use this again...
            //Let us just have objects, that have properties, like show, or not.
            //Let us put them in lists...
            //Let us find a way of showing them, perhaps in a listView, or just a form,
            //which can hold check boxes etc.
            //Just like you did in Android.
            //Much more flexible that way...
        }

        protected override void OnItemCheck(ItemCheckEventArgs checkEvent) {
            base.OnItemCheck(checkEvent);
            if (checkEvent.NewValue == CheckState.Checked) {
                form.optionsList[checkEvent.Index].showInPanel = true;
                checkedItemsCount++;
            }
            if (checkEvent.NewValue == CheckState.Unchecked) {
                form.optionsList[checkEvent.Index].showInPanel = false;
                checkedItemsCount--;
            }
            form.updatePanels(this, null);
            //TBH, SHOULD not have to do this HERE! 
            //the things that need updating, should listen for the cue, and update themselves!
            //fits better with the idea that objects do stuff to themselves, and look after themselves!
            //i.e. objects DON'T NEED conducting, or orchestrating...
        } 
    }
}
