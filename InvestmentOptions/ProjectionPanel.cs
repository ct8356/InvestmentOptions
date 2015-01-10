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
        private Legend legend1 = new Legend();
        private ChartArea chartArea1 = new ChartArea();
        private Series series1 = new Series();
        private Title title1;

        public ProjectionPanel() {
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
        }

        public void presentInvestmentOption(InvestmentOption option) {
            showChart1(option);
            showChart2(option);
            showDetails(option);
        }

        public void showDetails(InvestmentOption option) {
            option.treeView.Size = new Size(500, 250); //might be better to make it property of panel,
            //then pass it to option, to have stuff added to it...
            Controls.Add(option.treeView);
        }

        public void showChart1(InvestmentOption option) {
            //SETUP
            series1.ChartType = SeriesChartType.FastLine;
            chart1.ChartAreas.Add(chartArea1);
            chart1.Series.Add(series1);
            chart1.Size = new Size(500, 250);
            chart1.Text = "chart1";
            chart1.ChartAreas[0].AxisY.Maximum = 300000;
            Controls.Add(chart1);
            legend1.Name = "legend1";
            chart1.Legends.Add(legend1);
            //series1.Legend = "legend1";   
            title1 = new Title(option.Name, Docking.Top, new Font("Verdana", 12), Color.Black);
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            chart1.Titles.Add(title1);

            //PLOT STUFF
            option.makeProjection();
            float[] projection = option.netWorthProjection;
            series1.LegendText = "netWorthProjection";
            int[] intervals = new int[option.intervals];
            series1.Points.AddY(projection[0]); //Just to initialise Points...
            for (int interval = 1; interval < intervals.Count(); interval++) {
                DataPointCollection points = series1.Points; //create points, because it is easier to watch.
                points.AddY(projection[interval]); //problem is, this points is null until something is added..
            }
        }

        public void showChart2(InvestmentOption option) {
            //INSTANTIATE
            Chart chart2 = new Chart();
            Legend legend2 = new Legend();
            ChartArea chartArea2 = new ChartArea();
            Series series2 = new Series();
            Title title2 = new Title("Something", Docking.Top, new Font("Verdana", 12), Color.Black);
            //SETUP
            series2.ChartType = SeriesChartType.FastLine;
            chart2.ChartAreas.Add(chartArea2);
            chart2.Series.Add(series2);
            chart2.Size = new Size(500, 200);
            chart2.Text = "chart2";
            chart2.ChartAreas[0].AxisY.Maximum = 300000;
            Controls.Add(chart2);
            legend2.Name = "legend2";
            chart2.Legends.Add(legend2);
            title2.DockedToChartArea = chart2.ChartAreas[0].Name;
        }

    }
}
