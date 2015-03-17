using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {
    public class MyTreeNode : TreeNode {

        //public int intervals;
        public string label { get; set; }
        //public new string Text {
        //    get { return label; }
        //    set { label = value; base.Text = ""; }
        //}

        public MyTreeNode() {
            //do nothing
            Text = Name;
        }

        public MyTreeNode(String name) {
            Text = name;
            //intervals = option.intervals;
        }
    }
}
