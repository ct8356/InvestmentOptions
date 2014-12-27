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
        private TableLayoutPanel mainPanel = new TableLayoutPanel();

        public ProjectionForm() {
            ClientSize = new Size(700, 600);
            Text = "Christiaan Form";
            //Screw it, lets just use the tableLayoutPanel...
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 0;
            mainPanel.ColumnCount = 2; //Not needed apparently, but whatever...
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); //YES, two of these are needed!
            Controls.Add(mainPanel);
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

        public void presentInvestmentOptions(InvestmentOption option1, InvestmentOption option2) {
            ProjectionPanel panel1 = new ProjectionPanel();
            setUpPanel(panel1, 0); //dont think it matters when you do this...
            panel1.Dock = DockStyle.Fill;
            //panel1.Anchor = AnchorStyles.Left;
            //panel1.Anchor = AnchorStyles.Top;
            //panel1.Anchor = AnchorStyles.Right; //Anchors are weird. I dont really get them.     
            panel1.presentInvestmentOption(option1);

            ProjectionPanel panel2 = new ProjectionPanel();
            setUpPanel(panel2, 1);
            panel2.Dock = DockStyle.Fill;
            panel2.presentInvestmentOption(option2);         
        }

        public void setUpPanel(ProjectionPanel panel, int column) {
            mainPanel.Controls.Add(panel, column, 0);
        }

    }
}