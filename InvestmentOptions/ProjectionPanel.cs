using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InvestmentOptions {

    public class ProjectionPanel : FlowLayoutPanel {
        public InvestmentOption option;
        public List<LeafNode> nodeList;
        //INSTANTIATE
        List<Chart> charts = new List<Chart>();

        public ProjectionPanel(InvestmentOption option) {
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
            //OTHER
            this.option = option;
            this.nodeList = option.nodeList;  
            initialiseChildren();
        }

        public void presentInvestmentOption(InvestmentOption option) {
            showCharts();
            showDetails();
        }

        public void initialiseChildren() {
            //INSTANTIATE CHARTS
            charts.Add(new Chart());
            charts.Add(new Chart());
            charts.Add(new Chart());
            charts.Add(new Chart());
            //SETUP OPTIONS
            initialiseOptions();
            //SETUP CHARTS
            initialiseChart(charts[0]);
            charts[0].ChartAreas[0].AxisY.Maximum = 100000;
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            initialiseChart(charts[1]);
            charts[1].ChartAreas[0].AxisY.Maximum = 2000;
            //chart2.Titles[0].DockedToChartArea = chart2.ChartAreas[0].Name;
            initialiseChart(charts[2]);
            charts[2].ChartAreas[0].AxisY.Maximum = 2000;
            initialiseChart(charts[3]);
            charts[3].ChartAreas[0].AxisY.Maximum = 15;
            //SETUP SERIES
            List<LeafNode> nodeList = this.nodeList;
            //Look through the tree...
            //If you find a leafNode, with graph prop 1 checked,
            //then show it...
            //Now check for graph prop 2. if find it, show it...
            //SHOULD BE ONE TREE! BUT, treenodes, have properties...
            //Such as, show in graph 1, and show in graph 2.
            MyTreeView realWorldTree = option.realWorldTree;
            plotTree(realWorldTree.Nodes);

        }

        public void initialiseChart(Chart chart) {
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            chart.ChartAreas.Add(chartArea);
            chart.Size = new Size(500, 140);
            chart.Text = "chart1";
            Controls.Add(chart);
            Title title1 = new Title(option.Name, Docking.Top, new Font("Verdana", 12), Color.Black);
            chart.Titles.Add(title1);
            chart.Legends.Add(legend);
        }

        public void initialiseOptions() {
            CheckBox rentARoomSchemeCheckBox = new CheckBox(); 
            Controls.Add(rentARoomSchemeCheckBox);
            rentARoomSchemeCheckBox.Name = "rentARoomScheme";
            rentARoomSchemeCheckBox.Text = rentARoomSchemeCheckBox.Name;
            Binding binding = new Binding("Checked", option.property, "rentARoomScheme");
            //of course! A property, is NOT a field. It is the get/setter for the field!
            //the properties name should NEVER change, after release, so ok to use a string!
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            rentARoomSchemeCheckBox.DataBindings.Add(binding);  
        }

        //public void initialiseSeries(Chart chart, Series series, float[] projection) {
        //    series.ChartType = SeriesChartType.FastLine;
        //    //chart.Series.Add(series);
        //    series.LegendText = series.Name;
        //    series.Points.AddY(projection[0]); //Just to initialise Points...
        //} //Do this in LeafNode...

        public void plotTree(TreeNodeCollection nodes) {
            foreach (MyTreeNode node in nodes) {
                //node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode) node;
                    leafNode.showInChartBooleans = new List<bool>();
                    leafNode.showInChartBooleans.Add(false);
                    leafNode.showInChartBooleans.Add(false);
                    leafNode.showInChartBooleans.Add(false);
                    leafNode.showInChartBooleans.Add(false);
                    //NEED TO DO SOME MAGIC HERE!
                    for (int chart = 0; chart < charts.Count; chart++) {
                        if (leafNode.showInChartBooleans[chart]) {
                            //initialiseSeries(chart1, node.series, node.projection);
                            charts[chart].Series.Add(leafNode.series);
                        }
                    }
                }
                plotTree(node.Nodes);
            }
        }

        public void showDetails() {
            option.treeView.Size = new Size(500, 50); //might be better to make it property of panel,
            //then pass it to option, to have stuff added to it...
            Controls.Add(option.treeView);
        }

        public void showCharts() {
            //Make projection
            option.makeProjection(this);
            //RESET THE SERIES
            //for (int node = 0; node < nodeList.Count; node++) {
            //    nodeList[node].series.Points.Clear();
            //}
            //PLOT STUFF
            //for (int interval = 1; interval < option.intervals; interval++) {
            //    //create points, because it is easier to watch.
            //    for (int node = 0; node < nodeList.Count; node++) {
            //        nodeList[node].series.Points.AddY(nodeList[node].projection[interval]);
            //    }
            //}
            //THIS IS A MESS!
            //Basically, whenever makeProjection is called,
            //as a result, all projections, and and series, should be reset, and then updated.

            //initialising the series, should be done, inside the LEafNode, when LeafNode created!!!
            //SHOULD NOT be initialised to a chart.
            //should be added to a chart later...
        }

    }
}
