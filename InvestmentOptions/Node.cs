using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    class Node : TreeNode {

        float monthlyValue;
        float cumulativeValue;
        //List<Node> children; //Not needed now...

        public void addChild() {
            //Not needed now.
        }

        public void removeChild() {
            //Not needed now
        }
    }
}
