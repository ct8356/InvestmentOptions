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
        public ControlPanel controlPanel;
        public TableLayoutPanel tablePanel;
        private float controlPanelWidth = 17;
        public List<InvestmentOption> OptionsList { get; set; }

        public ProjectionForm() {
            WindowState = FormWindowState.Maximized;
            Text = "Christiaan Form";
            //TABLE PANEL
            tablePanel = new TableLayoutPanel();
            tablePanel.Dock = DockStyle.Fill;
            //tablePanel.RowCount = 0; //Won't do nothing.. it is just a tracker, for your use...
            Controls.Add(tablePanel);
            //CONTROL PANEL
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, controlPanelWidth));
            controlPanel = new ControlPanel(this);
            controlPanel.realWorldTreeView.NodeMouseClick += UpdatePanels;
            //ADD CONTROLS
            tablePanel.Controls.Add(controlPanel, 0, 0); //in first column
            //tablePanel.SetRowSpan(controlPanel, 2);
        }

        public void DoStuffWithOptionsList(List<InvestmentOption> optionsList) {
            //OPTIONS DATA
            OptionsList = optionsList;
            //GRAPH PANELS
            AddProjectionPanels(); //have to do before controlPanel.Add(optionList) for some reason.
            //OPTIONS LIST PANEL
            controlPanel.ListBox.Items.AddRange(OptionsList.ToArray());
            controlPanel.ListBox.SetItemChecked(0, true);
            controlPanel.ListBox.SetItemChecked(1, true);
            //GLOBAL PARAMS PANEL
            controlPanel.globalParameters.addCheckBoxes();
            Property.includeWearAndTear.Value = true;
        }
        
        public void AddProjectionPanels() {
            int columnNo = 0;
            for (int optionNo = 0; optionNo < OptionsList.Count; optionNo++) {         
                InvestmentOption option = OptionsList[optionNo];
                if (option.showInPanel) {             
                    ProjectionPanel panel = new ProjectionPanel(option, this);
                    AddProjectionPanel(panel, columnNo, controlPanel.ListBox.checkedItemsCount); //TODO fix chart issue.
                    panel.Dock = DockStyle.Fill;
                    panel.UpdateSelf();
                    columnNo++;
                }
            }
        }//BEST just bind the checkbox to the bool!

        public void AddProjectionPanel(ProjectionPanel panel, int column, int checkedItemsCount) {
            tablePanel.ColumnStyles.Add
                (new ColumnStyle(SizeType.Percent, (100f - controlPanelWidth) / checkedItemsCount));
            tablePanel.Controls.Add(panel, 1+column, 0);
        }

        protected override void Dispose(bool disposing) {
            // Clean up any resources being used.
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
            //true if managed resources should be disposed; 
            //otherwise, false.
        }

        public void UpdatePanels(Object sender, TreeNodeMouseClickEventArgs e) {
            //DELETE OLD PANELS
            for (int controlNo = (tablePanel.Controls.Count - 1); controlNo > 0; controlNo--)
            {
                tablePanel.ColumnStyles.RemoveAt(controlNo);
                tablePanel.Controls.RemoveAt(controlNo);
            }
            //ADD NEW ONES
            AddProjectionPanels();
        }
    }
}