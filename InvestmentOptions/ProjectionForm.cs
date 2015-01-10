using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InvestmentOptions {

    public class ProjectionForm : Form {
        private IContainer components = null;
        private ControlPanel controlPanel = new ControlPanel();
        private TableLayoutPanel tablePanel = new TableLayoutPanel();
        private float controlPanelWidth = 15;

        public ProjectionForm() {
            ClientSize = new Size(700, 600);
            WindowState = FormWindowState.Maximized;
            Text = "Christiaan Form";
            //Screw it, lets just use the tableLayoutPanel...
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.RowCount = 0;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, controlPanelWidth));
            tablePanel.Controls.Add(controlPanel, 0, 0); //in first column
            Controls.Add(tablePanel);
        }
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void presentInvestmentOptions(List<InvestmentOption> options) {
            controlPanel.listBox.Items.AddRange(options.ToArray());
            for (int optionNo = 0; optionNo < options.Count; optionNo++) {
                ProjectionPanel panel = new ProjectionPanel();
                addProjectionPanel(panel, optionNo, options.Count);
                panel.Dock = DockStyle.Fill;
                panel.presentInvestmentOption(options[optionNo]);
            }
        }

        public void addProjectionPanel(ProjectionPanel panel, int column, int count) {
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (100f-controlPanelWidth)/(count)));
            tablePanel.Controls.Add(panel, 1+column, 0);
        }

    }
}