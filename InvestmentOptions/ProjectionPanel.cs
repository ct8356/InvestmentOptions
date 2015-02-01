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
        private Chart chart1 = new Chart();
        public InvestmentOption option;
        public List<Node> nodeList;

        public ProjectionPanel() {
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
        }

        public void presentInvestmentOption(InvestmentOption option) {
            this.option = option;
            this.nodeList = option.nodeList;
            showCharts();
            showDetails();
        }

        public void setupChart(Chart chart) {
            ChartArea chartArea = new ChartArea();
            Legend legend = new Legend();
            chart.ChartAreas.Add(chartArea);
            chart.Size = new Size(500, 225);
            chart.Text = "chart1";      
            Controls.Add(chart);
            Title title1 = new Title(option.Name, Docking.Top, new Font("Verdana", 12), Color.Black);
            chart1.Titles.Add(title1);
            chart.Legends.Add(legend);
        }

        public void setupSeries(Chart chart1, Series series, float[] projection) {
            series.ChartType = SeriesChartType.FastLine;
            chart1.Series.Add(series);
            series.LegendText = series.Name;
            series.Points.AddY(projection[0]); //Just to initialise Points...
        }

        public void showDetails() {
            option.treeView.Size = new Size(500, 250); //might be better to make it property of panel,
            //then pass it to option, to have stuff added to it...
            Controls.Add(option.treeView);
        }

        public void showCharts() {
            //Make projection
            option.makeProjection();
            //INSTANTIATE
            Chart chart1 = new Chart();
            Chart chart2 = new Chart();
            //SETUP CHARTS
            setupChart(chart1);
            chart1.ChartAreas[0].AxisY.Maximum = 300000;
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            setupChart(chart2);
            //chart2.Titles[0].DockedToChartArea = chart2.ChartAreas[0].Name;
            //SETUP SERIES
            List<Node> nodeList = this.nodeList;
            for (int node = 0; node < nodeList.Count; node++) {
                if (node < 2) {
                    setupSeries(chart1, nodeList[node].series, option.netWorthN.projection);
                } else {
                    setupSeries(chart2, nodeList[node].series, option.bankAccountN.projection);
                }
            }
            //PLOT STUFF
            int[] intervals = new int[option.intervals];
            for (int interval = 1; interval < intervals.Count(); interval++) {
                //create points, because it is easier to watch.
                for (int node = 1; node < nodeList.Count; node++) {
                    nodeList[node].series.Points.AddY(nodeList[node].projection[interval]);
                }
            }
        }

    }
}
