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
        private Chart chart1;
        private Legend legend1;
        //private IContainer components = null;
        private ChartArea chartArea1;
        private Series series1;
        private Title title1;
        public ProjectionPanel() {
            //PANEL STUFF
            Text = "Christiaan Panel";
            FlowDirection = FlowDirection.TopDown;
            //CHART STUFF
            chartArea1 = new ChartArea();
            series1 = new Series();
            series1.ChartType = SeriesChartType.FastLine;
            //series1.Points = data; cannot do, Points is read only... 
            chart1 = new Chart();
            chart1.ChartAreas.Add(chartArea1);
            //chart1.Location = new Point(12, 12);
            chart1.Series.Add(series1);
            chart1.Size = new Size(400, 300);
            //chart1.Dock = DockStyle.Top; //Doesn't work for some reason.
            //chart1.Dock = DockStyle.Fill; //Doesn't work for some reason.
            chart1.Text = "chart1";
            chart1.ChartAreas[0].AxisY.Maximum = 300000;
            Controls.Add(chart1);
            legend1 = new Legend();
            legend1.Name = "legend1";
            chart1.Legends.Add(legend1);
            //series1.Legend = "legend1";
            
        }

        public void presentInvestmentOption(InvestmentOption option) {
            title1 = new Title(option.name, Docking.Top, new Font("Verdana", 12), Color.Black);
            //title1.DockedToChartArea = chart1.ChartAreas[0].Name;
            chart1.Titles.Add(title1);
            option.makeProjection();
            float[] projection = option.netWorthProjection;
            series1.LegendText = "netWorthProjection";
            int[] intervals = new int[option.intervals];
            //Note, good to use arrays when can, they are faster than lists...
            //Use them when you don't need to add to the list later...
            //ALSO good to create local variables, quicker to access, I think...
            series1.Points.AddY(projection[0]); //Just to initialise Points...
            for (int interval = 1; interval < intervals.Count(); interval++) {
                DataPointCollection points = series1.Points; //create points, because it is easier to watch.
                points.AddY(projection[interval]); //problem is, this points is null until something is added..
            }
            
            showDetails(option);
        }

        public void showDetails(InvestmentOption option) {
            //OTHER STUFF
            int i = 0;
            String tab;
            foreach (KeyValuePair<String, float> index in option.indexDictionary) {
                Label label = new Label();
                tab = "";
                if (new int[] {1,2,4,5,6,7}.Contains(i)) tab = "     ";
                if (new int[] {8,9,10}.Contains(i)) tab = "          ";
                label.Text = tab + index.Key + ": £" + String.Format("{0:n}", index.Value);
                label.Dock = DockStyle.Fill;
                Controls.Add(label);
                i++;
            }
        }

    }
}
