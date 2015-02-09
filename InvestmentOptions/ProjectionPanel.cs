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
        //CHART 1
        public InvestmentOption option;
        public List<Node> nodeList;

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
            //SETUP OPTIONS
            initialiseOptions();
            //INSTANTIATE
            Chart chart1 = new Chart();
            Chart chart2 = new Chart();
            Chart chart3 = new Chart();
            //SETUP CHARTS
            initialiseChart(chart1);
            chart1.ChartAreas[0].AxisY.Maximum = 300000;
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            initialiseChart(chart2);
            chart2.ChartAreas[0].AxisY.Maximum = 4000;
            //chart2.Titles[0].DockedToChartArea = chart2.ChartAreas[0].Name;
            initialiseChart(chart3);
            chart3.ChartAreas[0].AxisY.Maximum = 4000;
            //SETUP SERIES
            List<Node> nodeList = this.nodeList;
            for (int node = 0; node < nodeList.Count; node++) {
                if (node < 2) {
                    initialiseSeries(chart1, nodeList[node].series, nodeList[node].projection);
                }
                else if (2 <= node && node < 6) {
                    initialiseSeries(chart2, nodeList[node].series, nodeList[node].projection);
                }
                else {
                    initialiseSeries(chart3, nodeList[node].series, nodeList[node].projection);
                }
            }
        }

        public void initialiseChart(Chart chart) {
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            chart.ChartAreas.Add(chartArea);
            chart.Size = new Size(500, 170);
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
            Binding binding = new Binding("Checked", option, "rentARoomScheme");
            //of course! A property, is NOT a field. It is the get/setter for the field!
            //the properties name should NEVER change, after release, so ok to use a string!
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            rentARoomSchemeCheckBox.DataBindings.Add(binding);  
        }

        public void initialiseSeries(Chart chart, Series series, float[] projection) {
            series.ChartType = SeriesChartType.FastLine;
            chart.Series.Add(series);
            series.LegendText = series.Name;
            series.Points.AddY(projection[0]); //Just to initialise Points...
        }

        public void showDetails() {
            option.treeView.Size = new Size(500, 150); //might be better to make it property of panel,
            //then pass it to option, to have stuff added to it...
            Controls.Add(option.treeView);
        }

        public void showCharts() {
            //Make projection
            option.makeProjection(this);
            //RESET THE SERIES
            for (int node = 0; node < nodeList.Count; node++) {
                nodeList[node].series.Points.Clear();
            }
            //PLOT STUFF
            int[] intervals = new int[option.intervals];
            for (int interval = 1; interval < intervals.Count(); interval++) {
                //create points, because it is easier to watch.
                for (int node = 0; node < nodeList.Count; node++) {
                    nodeList[node].series.Points.AddY(nodeList[node].projection[interval]);
                }
            }
        }

    }
}
