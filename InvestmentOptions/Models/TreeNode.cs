using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {
    public class TreeNode : System.Windows.Forms.TreeNode {

        //public int intervals;

        public TreeNode() {
            Text = Name;
        }

        public TreeNode(String name) : this() {
            Name = name;
            //intervals = option.intervals;
        }

    }
}
