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
        List<Chart> charts = new List<Chart>();
        public ProjectionForm form;

        public ProjectionPanel(InvestmentOption option, ProjectionForm form) {
            this.form = form;
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
            //OTHER
            this.option = option;
            initialiseChildren();
            BorderStyle = BorderStyle.FixedSingle;
        }

        public void updateSelf() {
            updateCharts();
            updateDetails();
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
            //charts[0].ChartAreas[0].AxisY.Maximum = 100000;
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            initialiseChart(charts[1]);
            //charts[1].ChartAreas[0].AxisY.Maximum = 2000;
            //chart2.Titles[0].DockedToChartArea = chart2.ChartAreas[0].Name;
            initialiseChart(charts[2]);
            //charts[2].ChartAreas[0].AxisY.Maximum = 2000;
            initialiseChart(charts[3]);
            charts[3].ChartAreas[0].AxisY.Maximum = 10;
            charts[3].ChartAreas[0].AxisY.Minimum = -10;
        }

        public void initialiseChart(Chart chart) {
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            chart.ChartAreas.Add(chartArea);
            chart.Size = new Size(500, 160);
            chart.Text = "chart1";
            Controls.Add(chart);
            Title title1 = new Title(option.Name, Docking.Top, new Font("Verdana", 12), Color.Black);
            //chart.Titles.Add(title1);
            chart.Legends.Add(legend);
        }

        public void initialiseOptions() {
            CheckBox rentARoomSchemeCheckBox = new CheckBox(); 
            Controls.Add(rentARoomSchemeCheckBox);
            rentARoomSchemeCheckBox.Name = "rentARoomScheme";
            rentARoomSchemeCheckBox.Text = rentARoomSchemeCheckBox.Name;
            Binding binding = new Binding("Checked", option.realWorldTree.property, "rentARoomScheme");
            //of course! A property, is NOT a field. It is the get/setter for the field!
            //the properties name should NEVER change, after release, so ok to use a string!
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            rentARoomSchemeCheckBox.DataBindings.Add(binding);
            //COUNT SAVINGS AS INCOME
            CheckBox countSavingsAsIncome = new CheckBox();
            Controls.Add(countSavingsAsIncome);
            countSavingsAsIncome.Name = "countRentSavingsAsIncome";
            countSavingsAsIncome.Text = countSavingsAsIncome.Name;
            Binding binding2 = new Binding("Checked", option, "countRentSavingsAsIncome");
            binding2.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            countSavingsAsIncome.DataBindings.Add(binding2);
        }

        public void addCheckedSeriesToCharts(TreeNodeCollection nodes) {
            foreach (MyTreeNode node in nodes) {
                //node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode) node;
                    //NEED TO DO SOME MAGIC HERE!
                    for (int chart = 0; chart < charts.Count; chart++) {
                        String path = leafNode.FullPath;
                        TreeNode viewNode =
                            form.controlPanel.realWorldTreeView.getNodeFromPath(form.controlPanel.realWorldTreeView.Nodes, path);
                        LeafNode viewLeafNode = (LeafNode)viewNode;
                        if (viewLeafNode.showInChartList[chart]) {
                            //initialiseSeries(chart1, node.series, node.projection);
                            leafNode.series.Points.Count();
                            charts[chart].Series.Add(leafNode.series);
                        }
                    }
                }
                addCheckedSeriesToCharts(node.Nodes);
            }
        }

        public void removeAllSeriesFromCharts() {
            for (int chart = 0; chart < charts.Count; chart++) {
                charts[chart].Series.Clear();
            }
        }

        public void updateDetails() {
            //option.treeView.Size = new Size(500, 50); //might be better to make it property of panel,
            ////then pass it to option, to have stuff added to it...
            //Controls.Add(option.treeView);
        }

        public void updateCharts() {
            option.makeProjection(this);
            MyTreeView realWorldTree = option.realWorldTree;
            removeAllSeriesFromCharts();
            addCheckedSeriesToCharts(realWorldTree.Nodes);
            //THIS WAS A MESS!
            //Basically, whenever makeProjection is called,
            //as a result, all projections, and and series, should be reset, and then updated.
        }

    }
}
