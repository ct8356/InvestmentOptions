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
        private ControlPanel controlPanel;
        public TableLayoutPanel tablePanel = new TableLayoutPanel();
        private float controlPanelWidth = 15;

        public ProjectionForm() {
            ClientSize = new Size(700, 600);
            WindowState = FormWindowState.Maximized;
            Text = "Christiaan Form";
            //Screw it, lets just use the tableLayoutPanel...
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.RowCount = 0;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, controlPanelWidth));
            controlPanel = new ControlPanel(this);
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

        public void presentInvestmentOptions(List<InvestmentOption> optionsList) {
            controlPanel.listBox.Items.AddRange(optionsList.ToArray());
            controlPanel.listBox.SetItemChecked(0, true); //ensures first one always checked.
            List<InvestmentOption> checkedOptionsList = optionsList.GetRange(0, 1); 
            //paired with above line //(awks, but nec coz CheckedItems is crap!)
            presentCheckedOptions(checkedOptionsList);
        }

        public void presentCheckedOptions(List<InvestmentOption> checkedOptions) {
            for (int optionNo = 0; optionNo < checkedOptions.Count; optionNo++) {
                ProjectionPanel panel = new ProjectionPanel((InvestmentOption) checkedOptions[optionNo]);
                addProjectionPanel(panel, optionNo, checkedOptions.Count);
                panel.Dock = DockStyle.Fill;
                panel.presentInvestmentOption((InvestmentOption) checkedOptions[optionNo]);
            }
        }

        public void addProjectionPanel(ProjectionPanel panel, int column, int count) {
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (100f-controlPanelWidth)/(count)));
            //tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85f));
            tablePanel.Controls.Add(panel, 1+column, 0);
        }

    }
}