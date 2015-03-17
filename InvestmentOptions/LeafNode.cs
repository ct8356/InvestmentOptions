using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InvestmentOptions {

    public class LeafNode : MyTreeNode {
        //later, if decide treeNode is too heavyWeight, to have loads of,
        // just modify this class, to make it more lightWeight...
        //NOTE: Could make this a generic class, to hold floats, and bools...
        public String Name;
        public bool boolean;
        public bool showCumulative = false;
        public List<bool> showInChartList = new List<bool>();
        public float mv;
        public float cumulativeValue; //NOTE, not enough to set it to zero here.... Do it in RESET!
        public Series series;
        //List<Node> children; //Not needed now...

        public LeafNode() {
            //Do nothing
        }

        public LeafNode(String name) : base(name) {
            series = new Series();
            Name = name;
            series.Name = name;
            Text = name;
            initialiseSeries();
            showInChartList.Add(false);
            showInChartList.Add(false);
            showInChartList.Add(false);
            showInChartList.Add(false);
        }

        public void initialiseSeries() {
            series.ChartType = SeriesChartType.FastLine;
            series.LegendText = series.Name;
        }

        public void addChild() {
            //Not needed now.
        }

        public void removeChild() {
            //Not needed now
        }
        public void resetProjection() {
            //this is done already elsewhere.
        }

        public void resetSeries() {
            //RESET THE SERIES
            //DO it elsewhere actually.
        }

        public void traverseDescendants() {

        }

    }
}
