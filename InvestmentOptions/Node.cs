using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    public class Node : TreeNode {
        //later, if decide treeNode is too heavyWeight, to have loads of,
        // just modify this class, to make it more lightWeight...

        public float monthlyValue;
        public float cumulativeValue = 0;
        //List<Node> children; //Not needed now...

        public void addChild() {
            //Not needed now.
        }

        public void removeChild() {
            //Not needed now
        }

        public void traverseDescendants() {

        }
    }
}
