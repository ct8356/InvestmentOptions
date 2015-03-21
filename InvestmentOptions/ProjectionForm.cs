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
        public TableLayoutPanel tablePanel = new TableLayoutPanel();
        private float controlPanelWidth = 17;
        public List<InvestmentOption> optionsList;
        public List<ProjectionPanel> panelList;

        public ProjectionForm() {
            //ClientSize = new Size(700, 600);
            WindowState = FormWindowState.Maximized;
            Text = "Christiaan Form";
            tablePanel.Dock = DockStyle.Fill;
            //tablePanel.RowCount = 0; //Won't do nothing.. it is just a tracker, for your use...
            //CONTROL PANEL
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, controlPanelWidth));
            controlPanel = new ControlPanel(this);
            controlPanel.realWorldTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(updatePanels);
            //YOU KNOW! I think it makes sense to make an object SET ITS SELF UP,
            //in the constructor...
            //PANEL LIST
            panelList = new List<ProjectionPanel>();
            //ADD CONTROLS
            tablePanel.Controls.Add(controlPanel, 0, 0); //in first column
            //tablePanel.SetRowSpan(controlPanel, 2);
            Controls.Add(tablePanel);
            //controlPanel.listBox.SetItemChecked(0, true); //ensures first one always checked.     
            //remember, was always an error with first code for optionsListControlBox (first checkbox)...
            //again, probs something to do with WEIRD calls, outside your control...
            //Could fight it with a if statement...       
        }

        public void doStuffWithOptionsList(List<InvestmentOption> optionsList) {
            this.optionsList = optionsList;
            controlPanel.listBox.Items.AddRange(optionsList.ToArray());
            addPanels();
            controlPanel.listBox.SetItemChecked(0, true);
            //controlPanel.listBox.SetItemChecked(2, true);
            
            //NOTE: THIS IS VERY IMPORTANT.
            //MUST create form first, because it sets tree structure for the options...
            //Then must create options, before can bind the form to the options!!!
            //ACTUALLY, should make OPTIONS first... They will all have identical structure...
            //Create Data first, THEN decide how to display it... CBTL
            //I like this plan. think more tomorrow...
            //LEAVE AS IS FOR NOW! To big a change to change it (knock on effects).
            controlPanel.globalParameters.addCheckBoxes();
            Property.includeWearAndTear.value = true;
        }

        
        public void addPanels() {
            //int checkedItemsCount = controlPanel.listBox.CheckedItems.Count;//lets not use. Unreliable.
            int columnNo = 0;
            for (int optionNo = 0; optionNo < optionsList.Count; optionNo++) {         
                InvestmentOption option = optionsList[optionNo];
                if (option.showInPanel) {             
                    ProjectionPanel panel = new ProjectionPanel(option, this);
                    addProjectionPanel(panel, columnNo, controlPanel.listBox.checkedItemsCount);
                    panel.Dock = DockStyle.Fill;
                    panel.updateSelf();
                    columnNo++;
                }
            }
        }//BEST just bind the checkbox, to the bool!

        public void addProjectionPanel(ProjectionPanel panel, int column, int checkedItemsCount) {
            int colCount = tablePanel.ColumnStyles.Count;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (100f - controlPanelWidth) / checkedItemsCount));
            tablePanel.Controls.Add(panel, 1+column, 0);
        }

        protected override void Dispose(bool disposing) {
            // Clean up any resources being used.
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);//true if managed resources should be disposed; otherwise, false.
        }

        public void updatePanels(Object sender, TreeNodeMouseClickEventArgs e) {
            //DELETE OLD PANELS
            for (int controlNo = (tablePanel.Controls.Count - 1); controlNo > 0; controlNo--) {
                tablePanel.ColumnStyles.RemoveAt(controlNo);
                tablePanel.Controls.RemoveAt(controlNo);
            }
            //ADD NEW ONES
            addPanels();
        }
    }
}