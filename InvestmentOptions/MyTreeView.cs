using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {
    public class MyTreeView : TreeView {

        public MyTreeView() {
            //do nought
        }

        public void labelTree(TreeNodeCollection nodes) {
            foreach (LeafNode node in nodes) {
                node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                labelTree(node.Nodes);
            }
        }

        public void lookThroughTree(TreeNodeCollection nodes) {
            foreach (LeafNode node in nodes) {
                node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                lookThroughTree(node.Nodes);
            }
        }

        public void resetTree(TreeNodeCollection nodes) {
            foreach (MyTreeNode node in nodes) {
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode) node;
                    leafNode.series.Points.Clear();
                    for (int interval = 0; interval < node.intervals; interval++) {
                        leafNode.projection[interval] = 0;
                    }
                }
                resetTree(node.Nodes);
            }
        }

        public void updateProjectionsAndSeries(TreeNodeCollection nodes) {
            foreach (MyTreeNode node in nodes) {
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode) node;
                    //node.series.Points.Clear();
                    //for (int interval = 0; interval < node.intervals; interval++) {
                    //    node.projection[interval] = 0;
                    //}
                    //node.projection[] // I DO BELIEVE, PROJECTION IS NO LONGER NEEDED!
                    leafNode.series.Points.AddY(leafNode.monthlyValue);
                    updateProjectionsAndSeries(leafNode.Nodes);
                }
            }
        }
    }
}
