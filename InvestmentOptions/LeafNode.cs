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
        //NOTE: Could make this a generic class, 
        //to hold floats, and bools...
        public String Name;
        public bool boolean;
        public List<bool> showInChartBooleans = new List<bool>();
        public float monthlyValue;
        public float cumulativeValue = 0;
        public float[] projection;
        public Series series;
        //List<Node> children; //Not needed now...

        public LeafNode() {
            //Do nothing
        }

        public LeafNode(String name, int intervals) {
            projection = new float[intervals];
            series = new Series();
            Name = name;
            series.Name = name;
            initialiseSeries();
        }

        public void initialiseSeries() {
            series.ChartType = SeriesChartType.FastLine;
            //chart.Series.Add(series);
            series.LegendText = series.Name;
            //series.Points.AddY(projection[0]); //Just to initialise Points...
        } //Do this in LeafNode...

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
