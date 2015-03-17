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
        public FlowLayoutPanel optionsPanel;
        //public delegate void PropertyChangedEventHandler();

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

        public void initialiseChildren() {
            //INSTANTIATE CHARTS
            charts.Add(new Chart());
            charts.Add(new Chart());
            charts.Add(new Chart());
            charts.Add(new Chart());
            //SETUP OPTIONS
            initialiseOptionsPanel();
            //SETUP CHARTS
            initialiseChart(charts[0]);
            charts[0].ChartAreas[0].AxisY.Maximum = 140000;
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            initialiseChart(charts[1]);
            charts[1].ChartAreas[0].AxisY.Maximum = 2000;
            //chart2.Titles[0].DockedToChartArea = chart2.ChartAreas[0].Name;
            initialiseChart(charts[2]);
            charts[2].ChartAreas[0].AxisY.Maximum = 2000;
            initialiseChart(charts[3]);
            charts[3].ChartAreas[0].AxisY.Maximum = 10;
            charts[3].ChartAreas[0].AxisY.Minimum = -4;
            foreach (Chart chart in charts) {
                //chart.Dock = DockStyle.Fill;
            }
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

        public void initialiseOptionsPanel() {
            optionsPanel = new FlowLayoutPanel();
            optionsPanel.BorderStyle = BorderStyle.FixedSingle;
            optionsPanel.FlowDirection = FlowDirection.LeftToRight;
            optionsPanel.Dock = DockStyle.Top;
            optionsPanel.Height = 60;
            Controls.Add(optionsPanel);
            addCheckBox(option.realWorldTree.property.rentARoomScheme);
            addCheckBox(option.countRentSavingsAsIncome);
            addCheckBox(option.autoInvest);
        }

        public void addCheckBox(Object dataSource) { //data member is the property, to bind.
            MyBoolean myBoolean = (MyBoolean)dataSource;
            CheckBox checkBox = new CheckBox();
            optionsPanel.Controls.Add(checkBox);
            checkBox.Name = myBoolean.name;
            checkBox.Text = checkBox.Name;
            Binding binding = new Binding("Checked", dataSource, "value");
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            checkBox.DataBindings.Add(binding);
            //SUBSCRIBE TO ITS EVENTS
            myBoolean.PropertyChanged += new PropertyChangedEventHandler(handlePropertyChanged);
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

        public void handlePropertyChanged(Object sender, EventArgs args) {
            updateSelf();
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
            removeAllSeriesFromCharts();
            addCheckedSeriesToCharts(option.realWorldTree.Nodes);
            //ITS OK! if refering to dataTree, just put in Option one here... ALL is fine!!!
            //THIS treeView, is only needed for the VIEW!!!
            //Thanks to GETNodePath method, all is ok... or is it? Path might differ slightly...
            //Mod it??? (Probs not now... Just do PROPERTY RESEARCH!!!)
            //THIS WAS A MESS!
            //Basically, whenever makeProjection is called,
            //as a result, all projections, and and series, should be reset, and then updated.
        }

        public void updateSelf() {
            updateCharts();
            updateDetails();
        }

    }
}
