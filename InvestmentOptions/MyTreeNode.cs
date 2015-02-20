using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {
    public class MyTreeNode : TreeNode {

        public int intervals;

        public MyTreeNode() {
            //do nothing
        }

        public MyTreeNode(int intervals) {
            this.intervals = intervals;
        }
    }
}
