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
        public InvestmentOption Option { get; set; }
        public ProjectionForm Form { get; set; }
        public FlowLayoutPanel OptionsPanel { get; set; }
        private List<Chart> Charts { get; set; } = new List<Chart>();
        //public delegate void PropertyChangedEventHandler();

        public ProjectionPanel(InvestmentOption option, ProjectionForm form) {
            Option = option;
            Form = form;
            //PANEL STUFF
            Text = Option.Name;
            FlowDirection = FlowDirection.TopDown;
            //OTHER
            InitialiseChildren();
            BorderStyle = BorderStyle.FixedSingle;
            //SUBCRIBE TO EVENTS (of the static booleans).
            InvestmentOption.countRentSavingsAsIncome.PropertyChanged += HandlePropertyChanged;
            Property.includeWearAndTear.PropertyChanged += HandlePropertyChanged;
        }

        private void InitialiseChildren() {
            //INSTANTIATE CHARTS
            Charts.Add(new Chart());
            Charts.Add(new Chart());
            Charts.Add(new Chart());
            Charts.Add(new Chart());
            //SETUP OPTIONS
            InitialiseOptionsPanel();
            //SETUP CHARTS
            InitialiseChart(Charts[0]);
            Charts[0].ChartAreas[0].AxisY.Maximum = 140000;            
            InitialiseChart(Charts[1]); Charts[1].ChartAreas[0].AxisY.Maximum = 2000;
            InitialiseChart(Charts[2]); Charts[2].ChartAreas[0].AxisY.Maximum = 2000;
            //InitialiseChart(Charts[3]);
            //Charts[3].ChartAreas[0].AxisY.Maximum = 10; Charts[3].ChartAreas[0].AxisY.Minimum = -4;
            foreach (Chart chart in Charts) {
                //chart.Dock = DockStyle.Fill;
            }
        }

        public void InitialiseChart(Chart chart) {
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            chart.ChartAreas.Add(chartArea);
            chart.Size = new Size(500, 150);
            chart.Text = "chart";        
            Title title = new Title
                (Option.Name, Docking.Top, new Font("Verdana", 12), Color.Black);
            //title.DockedToChartArea = chart.ChartAreas[0].Name;
            //chart.Titles.Add(title);
            chart.Legends.Add(legend);
            Controls.Add(chart);
        }

        public void InitialiseOptionsPanel() {
            OptionsPanel = new FlowLayoutPanel();
            OptionsPanel.BorderStyle = BorderStyle.FixedSingle;
            OptionsPanel.FlowDirection = FlowDirection.LeftToRight;
            OptionsPanel.Dock = DockStyle.Top;
            OptionsPanel.Height = 60;
            Controls.Add(OptionsPanel);
            AddCheckBox(Option.Property.rentARoomScheme);
            AddCheckBox(Option.autoInvest);
            addLabel(Option.Property.buyType.ToString());
            addLabel(Option.Mortgage.type.ToString());
            addLabel(Option.Property.location.ToString());
            addLabel("BdRms/Hse: " + Option.Property.BedroomsPerHouse);
        }

        private void AddCheckBox(Boolean myBoolean) { 
            //myBoolean is dataSource.
            //data member is the property, to bind.
            //CHECK BOX
            CheckBox checkBox = new CheckBox();    
            checkBox.Name = myBoolean.Name;
            checkBox.Text = myBoolean.Name;
            OptionsPanel.Controls.Add(checkBox);
            //BINDING
            Binding binding = new Binding("Checked", myBoolean, "value");
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            checkBox.DataBindings.Add(binding);
            //SUBSCRIBE
            myBoolean.PropertyChanged += HandlePropertyChanged;
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
            OptionsPanel.Controls.Add(label);
            label.Name = dataSource;
            label.Text = label.Name;
        }

        public void HandlePropertyChanged(Object sender, EventArgs args) {
            UpdateSelf();
        }

        public void RemoveAllSeriesFromCharts() {
            for (int chart = 0; chart < Charts.Count; chart++) {
                Charts[chart].Series.Clear();
            }
        }

        public void UpdateCharts() {
            Option.MakeProjection();
            RemoveAllSeriesFromCharts();
            AddCheckedSeriesToCharts(Option.Nodes);
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
        }

    }
}
