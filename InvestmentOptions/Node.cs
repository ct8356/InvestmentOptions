using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InvestmentOptions {

    public class Node : TreeNode {
        //later, if decide treeNode is too heavyWeight, to have loads of,
        // just modify this class, to make it more lightWeight...

        public float monthlyValue;
        public float cumulativeValue = 0;
        public float[] projection;
        public Series series;
        //List<Node> children; //Not needed now...

        public Node() {
            //Do nothing
        }

        public Node(int intervals) {
            projection = new float[intervals];
            series = new Series();
        }

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
