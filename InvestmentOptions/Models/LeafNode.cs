using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InvestmentOptions {

    public class LeafNode : TreeNode {
        //later, if decide treeNode is too heavyWeight, to have loads of,
        // just modify this class, to make it more lightWeight...
        //NOTE: Could make this a generic class, to hold floats, and bools...
        public bool boolean;
        public List<bool> ShowInChartList { get; set; } = new List<bool>();
        public float mv; //monthyValue
        public float mvs; //mv for a single property...
        public float additional;
        public float cumulativeValue;
        public Series monthlySeries;
        public Series cumulativeSeries;

        public LeafNode() {
        }

        public LeafNode(String name) : base(name) {
            Name = name;
            Text = name;
            monthlySeries = new Series();
            monthlySeries.Name = name;
            cumulativeSeries = new Series(); 
            cumulativeSeries.Name = name;
            //Want to get rid of cumSeries,
            //And just initialise two leafNodes, if need it.
            InitialiseSeries();
            ShowInChartList.Add(false);
            ShowInChartList.Add(false);
            ShowInChartList.Add(false);
            ShowInChartList.Add(false);
        }

        public void InitialiseSeries() {
            monthlySeries.ChartType = SeriesChartType.FastLine;
            monthlySeries.LegendText = monthlySeries.Name;
            cumulativeSeries.ChartType = SeriesChartType.FastLine;
            cumulativeSeries.LegendText = cumulativeSeries.Name;
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
