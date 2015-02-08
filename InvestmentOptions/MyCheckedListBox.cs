using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    public class MyCheckedListBox : CheckedListBox {
        //This is a real mess. Let us not use this again...
        //Let us just have objects, that have properties, like show, or not.
        //Let us put them in lists...
        //Let us find a way of showing them, perhaps in a listView, or just a form,
        //which can hold check boxes etc.
        //Just like you did in Android.
        //Much more flexible that way...
        ProjectionForm form;

        public MyCheckedListBox(ProjectionForm form) {
            this.form = form;
        }

        protected override void OnItemCheck(ItemCheckEventArgs checkEvent) {
            base.OnItemCheck(checkEvent);
            List<InvestmentOption> myCheckedItems = new List<InvestmentOption>();
            //CheckedItemCollection myCheckedItems = CheckedItems;
            foreach (InvestmentOption item in CheckedItems) {
                myCheckedItems.Add(item);
            } //pain, but necessary, because cannot convert CheckedItemCollection into a List, annoyingly!
            if (checkEvent.NewValue == CheckState.Checked) {
                //now we need bit of code to put it in right place! (i.e. not just at the end).
                //basically, needs to be arranged like excel sheet. All in a set order, some are hidden though
                //real botch, just because CheckedItems is RUBBISH!
                InvestmentOption newItem = (InvestmentOption) Items[checkEvent.Index];
                int previousItemsCount = checkEvent.Index;
                for (int i = 1; i <= previousItemsCount; i++) {
                    InvestmentOption potentialLink = (InvestmentOption) Items[checkEvent.Index - i];
                    if (myCheckedItems.Contains(potentialLink)) {//check if it is in the checkedList!
                        //myCheckedItems.Insert(previousItemsCount + 1 - i, newItem);
                        myCheckedItems.Insert(myCheckedItems.IndexOf(potentialLink) + 1, newItem);
                        break; //of course, still need to break here!
                    } else {
                        if (i == previousItemsCount) { //if reached end, and no potential link, then
                            myCheckedItems.Insert(0, newItem); //means is at top of list, so put in at front.
                        }
                    }
                }
            } //great, seems to work, now just need to define what happens when UNCHECKED:
            if (checkEvent.NewValue == CheckState.Unchecked) {
                //need to update myCheckedItems here to, because CheckedItems not changed yet...
                myCheckedItems.Remove((InvestmentOption) Items[checkEvent.Index]);
            }
            for (int controlNo = (form.tablePanel.Controls.Count - 1); controlNo > 0; controlNo--) {
                form.tablePanel.ColumnStyles.RemoveAt(controlNo);
                form.tablePanel.Controls.RemoveAt(controlNo);
            } //PROBLEM! it deletes one, then goes by old index method! must start from end!
            //int one = CheckedItems.Count;
            form.presentCheckedOptions((List<InvestmentOption>) myCheckedItems); //one issue is,
            //Collection CheckedItems has not been updated yet...
        }

    }
}
