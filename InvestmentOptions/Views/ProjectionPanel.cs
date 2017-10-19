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
        private List<Chart> Charts { get; set; } = new List<Chart>();
        public ProjectionForm Form { get; set; }
        public FlowLayoutPanel optionsPanel;
        //public delegate void PropertyChangedEventHandler();

        public ProjectionPanel(InvestmentOption option, ProjectionForm form) {
            Form = form;
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
            //OTHER
            this.option = option;
            InitialiseChildren();
            BorderStyle = BorderStyle.FixedSingle;
            //SUBCRIBE TO EVENTS (of the static booleans).
            InvestmentOption.countRentSavingsAsIncome.PropertyChanged += 
                new PropertyChangedEventHandler(handlePropertyChanged);
            Property.includeWearAndTear.PropertyChanged += 
                new PropertyChangedEventHandler(handlePropertyChanged);
        }

        private void InitialiseChildren() {
            //INSTANTIATE CHARTS
            Charts.Add(new Chart());
            Charts.Add(new Chart());
            Charts.Add(new Chart());
            Charts.Add(new Chart());
            //SETUP OPTIONS
            initialiseOptionsPanel();
            //SETUP CHARTS
            InitialiseChart(Charts[0]);
            Charts[0].ChartAreas[0].AxisY.Maximum = 140000;
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            InitialiseChart(Charts[1]);
            Charts[1].ChartAreas[0].AxisY.Maximum = 2000;
            //chart2.Titles[0].DockedToChartArea = chart2.ChartAreas[0].Name;
            InitialiseChart(Charts[2]);
            Charts[2].ChartAreas[0].AxisY.Maximum = 2000;
            InitialiseChart(Charts[3]);
            Charts[3].ChartAreas[0].AxisY.Maximum = 10;
            Charts[3].ChartAreas[0].AxisY.Minimum = -4;
            foreach (Chart chart in Charts) {
                //chart.Dock = DockStyle.Fill;
            }
        }

        public void InitialiseChart(Chart chart) {
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            chart.ChartAreas.Add(chartArea);
            chart.Size = new Size(500, 150);
            chart.Text = "chart1";
            Controls.Add(chart);
            Title title1 = new Title
                (option.Name, Docking.Top, new Font("Verdana", 12), Color.Black);
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
            addCheckBox(option.RealWorldTree.property.rentARoomScheme);
            addCheckBox(option.autoInvest);
            addLabel(option.RealWorldTree.property.buyType.ToString());
            addLabel(option.RealWorldTree.mortgage.type.ToString());
            addLabel(option.RealWorldTree.property.location.ToString());
            addLabel("BdRms/Hse: " + option.RealWorldTree.property.bedroomsPerHouse);
        }

        public void addCheckBox(Object dataSource) { //data member is the property, to bind.
            Boolean myBoolean = (Boolean)dataSource;
            CheckBox checkBox = new CheckBox();
            optionsPanel.Controls.Add(checkBox);
            checkBox.Name = myBoolean.Name;
            checkBox.Text = checkBox.Name;
            Binding binding = new Binding("Checked", dataSource, "value");
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            checkBox.DataBindings.Add(binding);
            //SUBSCRIBE TO ITS EVENTS
            myBoolean.PropertyChanged += new PropertyChangedEventHandler(handlePropertyChanged);
        }

        public void AddCheckedSeriesToCharts(TreeNodeCollection nodes) {
            foreach (TreeNode node in nodes) {
                //node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode) node;
                    //NEED TO DO SOME MAGIC HERE!
                    for (int chart = 0; chart < Charts.Count; chart++) {
                        String path = leafNode.FullPath;
                        System.Windows.Forms.TreeNode viewNode =
                            Form.controlPanel.realWorldTreeView.GetNodeFromPath(Form.controlPanel.realWorldTreeView.Nodes, path);
                        LeafNode viewLeafNode = (LeafNode)viewNode;
                        if (viewLeafNode.ShowInChartList[chart]) {
                            //initialiseSeries(chart1, node.series, node.projection);
                            //leafNode.monthlySeries.Points.Count();
                            if (chart == 0) {
                                Charts[chart].Series.Add(leafNode.cumulativeSeries);
                            }
                            else {
                                Charts[chart].Series.Add(leafNode.monthlySeries);
                            }
                        }
                    }
                }
                AddCheckedSeriesToCharts(node.Nodes);
            }
        }

        public void addLabel(String dataSource) {
            Label label = new Label();
            optionsPanel.Controls.Add(label);
            label.Name = dataSource;
            label.Text = label.Name;
        }

        public void handlePropertyChanged(Object sender, EventArgs args) {
            UpdateSelf();
        }

        public void removeAllSeriesFromCharts() {
            for (int chart = 0; chart < Charts.Count; chart++) {
                Charts[chart].Series.Clear();
            }
        }

        public void UpdateDetails() {
            //option.treeView.Size = new Size(500, 50); //might be better to make it property of panel,
            ////then pass it to option, to have stuff added to it...
            //Controls.Add(option.treeView);
        }

        public void UpdateCharts() {
            option.MakeProjection();
            removeAllSeriesFromCharts();
            AddCheckedSeriesToCharts(option.RealWorldTree.Nodes);
            //ITS OK! if refering to dataTree, just put in Option one here... ALL is fine!!!
            //THIS treeView, is only needed for the VIEW!!!
            //Thanks to GETNodePath method, all is ok... or is it? Path might differ slightly...
            //Mod it??? (Probs not now... Just do PROPERTY RESEARCH!!!)
            //THIS WAS A MESS!
            //Basically, whenever makeProjection is called,
            //as a result, all projections, and and series, should be reset, and then updated.
        }

        public void UpdateSelf() {
            UpdateCharts();
            UpdateDetails();
        }

    }
}
