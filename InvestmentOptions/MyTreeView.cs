using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace InvestmentOptions {
    public class MyTreeView : TreeView {
        //NOTE: A TreeView HAS to have an InvestmentOption as a property.
        //Since its childNodes (for the moment) need access to an InvestmentOption... (for int intervals).
        // Create a Font object for the node tags.
        Font tagFont = new Font("Helvetica", 8, FontStyle.Bold);
        InvestmentOption option;
        //TREENODES
        public Job job;
        public Life life;
        public Mortgage mortgage;
        public Property property;
        public Shelter shelter;
        //OTHER NODES (WITH SERIES)
        public LeafNode bankAccount;
        public LeafNode netWorth;
        public LeafNode ingoings;
        public LeafNode outgoings;
        public ControlPanel controlPanel;
        Size glyphSize = Size.Empty;

        public MyTreeView(InvestmentOption option) {
            this.option = option;
            addChildren();
            DrawMode = TreeViewDrawMode.OwnerDrawText; //Change to OwnerDrawText...
            //Ahah, so maybe, since it is just a TextGlyph, that is why rectangle fudge is needed? Not sure...
            //If it is a checkBox, surely should work just the same...
            HideSelection = false;    // I like that better
            CheckBoxes = false;       // necessary!?
            FullRowSelect = false;    // necessary!?
            DrawNode += new DrawTreeNodeEventHandler(drawNode); //Ok, well for starters, should be TWO types
            //OFTree... TreeView, and the other, for clarity...
            //OR use static things...
            //NO I KNOW!!! Just make the drawing, depend SOLELY on the TREEVIEW checkBoxes!!!
            NodeMouseClick += new TreeNodeMouseClickEventHandler(updateShowInChartList);
        }

        public MyTreeView(ControlPanel controlPanel, InvestmentOption option)
            : this(option) {
            //THIS is for OPTION Trees!!!
            this.controlPanel = controlPanel;
            controlPanel.realWorldTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(updateShowInChartList);
            //OF COURSE! Won't work if don't have this for TREEVIEW!!!
            //ABOVE NOT needed, if got BINDINGS!
            //NOTE: only OPTION TREEs,
            //should be bound to the MAIN UI tree...
            //NOTE: THe below needs to go in a recursion method!!! Creating LOAD of bindings!!!
            //addBindings(Nodes);
            //NOTE: the BINDINGS, go in the CONTROL PANEL, when it is made!!!
            //OK!, should be bindings, but opens up a can of worms... (need to make INVOPTION the root node!).
            //You will just have to have a BINDING method in ProjForm, for now...
        }

        public void drawCheckBoxes(MyTreeNode node, DrawTreeNodeEventArgs e) {
            if (node is LeafNode) {
                LeafNode leafNode = (LeafNode) node;
                CheckBoxState checkBox1State = leafNode.showInChartList[0] ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                CheckBoxState bs2 = leafNode.showInChartList[1] ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                CheckBoxState bs3 = leafNode.showInChartList[2] ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                CheckBoxRenderer.DrawCheckBox(e.Graphics, getCheckBoxRectangle(e.Bounds, 0).Location, checkBox1State); //keytool
                CheckBoxRenderer.DrawCheckBox(e.Graphics, getCheckBoxRectangle(e.Bounds, 1).Location, bs2);
                CheckBoxRenderer.DrawCheckBox(e.Graphics, getCheckBoxRectangle(e.Bounds, 2).Location, bs3);
            }
        }

        protected void drawNode(Object sender, DrawTreeNodeEventArgs e) {
            //HMMMM!!!!! Problem seems to be,  this keeps getting called,
            //And the event sent, is wrong!!! so the event node sent has location zero,
            //but details of all other nodes, and they are all drawn here!!!
            //COULD put an if clause here, to protect against that...
            //NOTE: think this method will be called for each Node, everytime something changes...
            Rectangle nodeRectangle = new Rectangle(e.Bounds.Location, new Size(ClientSize.Width, e.Bounds.Height));
            glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedNormal);
            // Draw the background and node text for a selected node. 
            if ((e.State & TreeNodeStates.Selected) != 0) {
                // Draw the background of the selected node. 
                e.Graphics.FillRectangle(Brushes.Green, getNodeBounds(e.Node));
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = Font;
                // Draw the node text.
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                    Rectangle.Inflate(e.Bounds, 2, 0));
            }
            // Use the default background and node text.
            else {
                e.DrawDefault = true;
            }//if NOT selected, then redraws the whole tree, by normal rules!!!
            if (e.Node.Bounds.Y != 0) {
                drawCheckBoxes((MyTreeNode)e.Node, e);
            }
            //DELEGATES: IF don't have access to the Class with the event property, (editting access that is),
            //Then have to use delegates, and register the delegate with the event,
            // which always have access to (and method that want to call).
            //The method inputs have to match those of the delegate...
            //If DO have access, then can just call OnDrawNode()....
        }

        public void addChildren() {
            //Nodes.AddRange(option.Nodes);
            Nodes.Add(job = new Job(option));
            Nodes.Add(life = new Life(option));
            Nodes.Add(mortgage = new Mortgage(option));
            Nodes.Add(property = new Property(option));
            Nodes.Add(shelter = new Shelter(option));
            Nodes.Add(bankAccount = new LeafNode("bankAccount"));
            Nodes.Add(netWorth = new LeafNode("netWorth"));
            Nodes.Add(ingoings = new LeafNode("ingoings"));
            Nodes.Add(outgoings = new LeafNode("outgoings"));
            //NOTE: could add these children to investmentOption, THEN add them here!!!
            //BETTER!
            //Only issue is, WOULD I have to bind them together???
            //DO I call anything, here?
        }

        public void addBindings(TreeNodeCollection nodes) {
            foreach (MyTreeNode node in nodes) {
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode)node;
                    //leafNode.series.Points.AddY(leafNode.monthlyValue);
                    Binding binding = new Binding("Checked", option.realWorldTree.property, "rentARoomScheme");
                    //of course! A property, is NOT a field. It is the get/setter for the field!
                    //the properties name should NEVER change, after release, so ok to use a string!
                    binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    controlPanel.realWorldTreeView.DataBindings.Add(binding); 
                    //SHIT! Still need a INDEXING method... SO may as well just use OTHER method you had...
                    //IT CAN COME IN EITHER PLACE,
                    //But makes more sense here...
                }
                updateSeries(node.Nodes); //THIS SHOULD NOT BE HERE???!!!
            }
        }//DONT USE THIS!!!
        
        Rectangle getCheckBoxRectangle(Rectangle bounds, int check) {
            return new Rectangle(80 + bounds.Left + 2 + (glyphSize.Width + 4) * check,
                                 bounds.Y + 2,
                                 glyphSize.Width,
                                 glyphSize.Height);
        }

        private Rectangle getNodeBounds(TreeNode node) {
            // Returns the bounds of the specified node, including the region  
            // occupied by the node label and any node tag displayed. 
            //The NodeBounds method makes the highlight rectangle large enough to 
            // include the text of a node tag, if one is present.
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Tag != null) {
                // Retrieve a Graphics object from the TreeView handle 
                // and use it to calculate the display width of the tag.
                Graphics g = CreateGraphics();
                int tagWidth = (int)g.MeasureString
                    (node.Tag.ToString(), tagFont).Width + 6;
                // Adjust the node bounds using the calculated value.
                bounds.Offset(tagWidth / 2, 0);
                bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
                g.Dispose();
            }
            return bounds;
        }

        public TreeNode getNodeFromPath(TreeNodeCollection nodes, string path) {
            TreeNode foundNode = null;
            foreach (TreeNode treeNode in nodes) {
                if (treeNode.FullPath == path) {
                    return treeNode; //If found it, your done. (this is the first, or INNER thing sent)
                }
                else if (treeNode.Nodes.Count > 0) {
                    foundNode = getNodeFromPath(treeNode.Nodes, path);
                }//if not found it, AND it has kids, search them...
                if (foundNode != null)
                    return foundNode; //if found a match, your are done... keep sending it back up a level.
            }//If has no children, AND/OR no treeNode returned, then try next node...
            return null; //nothing ever found on that layer, return null...
        } //NOTE: Slow, because checks EVERY NODE, rather than following path. OK for now...
        
        public void labelTree(TreeNodeCollection nodes) {
            foreach (LeafNode node in nodes) {
                node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                labelTree(node.Nodes);
                //fortunately for this guy, all nodes here ARE leaf nodes...
            }
        }

        public void resetTree(TreeNodeCollection nodes) {
            foreach (MyTreeNode node in nodes) {
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode)node;
                    leafNode.series.Points.Clear();
                    //for (int interval = 0; interval < node.intervals; interval++) {
                    //    leafNode.projection[interval] = 0;
                    //}
                }
                resetTree(node.Nodes);
            }
        }

        public void setDefaultNodeToChartMapping() {
            bankAccount.showInChartList[0] = true;
            netWorth.showInChartList[0] = true;
            property.taxableTenantsRent.showInChartList[1] = true;
            property.tenantsRent.showInChartList[1] = true;
            property.profit.showInChartList[1] = true;
            property.taxableProfit.showInChartList[1] = true;
            property.costs.showInChartList[1] = true;
            property.taxableTenantsRent.showInChartList[1] = true;
            property.incomeTax.showInChartList[1] = true;
            //shelter.rent.showInChartList[1] = true;
            mortgage.interest.showInChartList[2] = true;
            property.wearAndTear.showInChartList[2] = true;
            property.agentsFee.showInChartList[2] = true;
            property.accountantsFee.showInChartList[2] = true;
            property.returnOnInvestment.showInChartList[3] = true;
            property.tenantCount.showInChartList[3] = true;
            property.capitalGains.showInChartList[0] = true;
        }

        public void updateSeries(TreeNodeCollection nodes) {
            foreach (MyTreeNode node in nodes) {
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode) node;
                    if (leafNode.showCumulative) {
                        leafNode.series.Points.AddY(leafNode.cumulativeValue);
                        leafNode.series.LegendText = leafNode.series.Name + leafNode.cumulativeValue;
                    }
                    else {
                        leafNode.series.Points.AddY(leafNode.mv);
                        leafNode.series.LegendText = leafNode.series.Name + leafNode.mv;
                    }
                }
                updateSeries(node.Nodes);
            }
        }

        public void updateShowInChartList(Object sender, TreeNodeMouseClickEventArgs e){          
            //base.OnNodeMouseClick(e);   
            Console.WriteLine(e.Location + " bounds:"  + e.Node.Bounds);   
            if (e == null) return;
            handleTreeNode(e.Node, e); //viewNode
        }

        public void handleTreeNode(TreeNode treeNode, TreeNodeMouseClickEventArgs e) {
            MyTreeNode myTreeNode = treeNode as MyTreeNode;
            if (myTreeNode is LeafNode) {//if it was a LeafNode
                LeafNode leafNode = (LeafNode) myTreeNode;
                TreeNode nSel = SelectedNode;
                if (getCheckBoxRectangle(leafNode.Bounds, 0).Contains(e.Location))
                    leafNode.showInChartList[0] = !leafNode.showInChartList[0];
                else if (getCheckBoxRectangle(leafNode.Bounds, 1).Contains(e.Location))
                    leafNode.showInChartList[1] = !leafNode.showInChartList[1];
                else if (getCheckBoxRectangle(leafNode.Bounds, 2).Contains(e.Location))
                    leafNode.showInChartList[2] = !leafNode.showInChartList[2];
                else {
                    if (nSel == leafNode && Control.ModifierKeys == Keys.Control)
                        SelectedNode = SelectedNode != null ? null : leafNode;
                    else SelectedNode = leafNode; //SELECT THE LEAF NODE TOO!!!
                }
                Console.WriteLine(" " + leafNode.showInChartList[0] + " " + leafNode.showInChartList[1] +
                    " " + leafNode.showInChartList[2]);
                Invalidate();
            }
        }

    }
}
