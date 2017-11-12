using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace InvestmentOptions {
    public class TreeView : System.Windows.Forms.TreeView {
        //NOTE: A TreeView HAS to have an InvestmentOption as a property.
        //Since its childNodes (for the moment) need access to an InvestmentOption... 
        //(for int intervals)..
        Font tagFont = new Font("Helvetica", 8, FontStyle.Bold);
        InvestmentOption Option { get; set; }
        public ControlPanel ControlPanel { get; set; }
        private Size GlyphSize { get; set; } = Size.Empty;

        public TreeView(InvestmentOption option) {
            Option = option;
            Option.AddNodes();
            AddNodes();
            DrawMode = TreeViewDrawMode.OwnerDrawText; //Change to OwnerDrawText.
            //Ahah, so maybe, since it is just a TextGlyph, that is why rectangle fudge is needed? Not sure...
            //If it is a checkBox, surely should work just the same.
            HideSelection = false;    // I like that better
            CheckBoxes = false;       // necessary!?
            FullRowSelect = false;    // necessary!?
            DrawNode += OnDrawNode; //Ok, well for starters, should be TWO types
            //OFTree... TreeView, and the other, for clarity.
            //OR use static things.
            //NO I KNOW!!! Just make the drawing, depend SOLELY on the TREEVIEW checkBoxes!!!
            NodeMouseClick += UpdateShowInChartList;
        }

        public TreeView(ControlPanel controlPanel, InvestmentOption option)
            : this(option) {
            //THIS is for OPTION Trees!
            ControlPanel = controlPanel;
            controlPanel.realWorldTreeView.NodeMouseClick += UpdateShowInChartList;
            //Above must be done, for booleans to be switched when click checkboxes.
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

        public void DrawCheckBoxes(TreeNode node, DrawTreeNodeEventArgs e) {
            if (node is LeafNode) {
                LeafNode leafNode = (LeafNode) node;
                CheckBoxState checkBox1State = leafNode.ShowInChartList[0] ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                CheckBoxState bs2 = leafNode.ShowInChartList[1] ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                CheckBoxState bs3 = leafNode.ShowInChartList[2] ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                CheckBoxRenderer.DrawCheckBox(e.Graphics, GetCheckBoxRectangle(e.Bounds, 0).Location, checkBox1State); //keytool
                CheckBoxRenderer.DrawCheckBox(e.Graphics, GetCheckBoxRectangle(e.Bounds, 1).Location, bs2);
                CheckBoxRenderer.DrawCheckBox(e.Graphics, GetCheckBoxRectangle(e.Bounds, 2).Location, bs3);
            }
        }

        protected void OnDrawNode(Object sender, DrawTreeNodeEventArgs e) {
            //HMMMM!!!!! Problem seems to be,  this keeps getting called,
            //And the event sent, is wrong!!! so the event node sent has location zero,
            //but details of all other nodes, and they are all drawn here!!!
            //COULD put an if clause here, to protect against that...
            //NOTE: think this method will be called for each Node, everytime something changes...
            Rectangle nodeRectangle = new Rectangle(e.Bounds.Location, new Size(ClientSize.Width, e.Bounds.Height));
            GlyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedNormal);
            // Draw the background and node text for a selected node. 
            if ((e.State & TreeNodeStates.Selected) != 0) {
                // Draw the background of the selected node. 
                e.Graphics.FillRectangle(Brushes.Green, GetNodeBounds(e.Node));
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
                DrawCheckBoxes((TreeNode)e.Node, e);
            }
            //DELEGATES: IF don't have access to the Class with the event property, (editting access that is),
            //Then have to use delegates, and register the delegate with the event,
            // which always have access to (and method that want to call).
            //The method inputs have to match those of the delegate...
            //If DO have access, then can just call OnDrawNode()....
        }

        public void AddNodes() {
            foreach (TreeNode node in Option.Nodes)
            {
                Nodes.Add(node);
            }
        }

        public void AddBindings(TreeNodeCollection nodes) {
            foreach (TreeNode node in nodes) {
                if (node is LeafNode) {
                    LeafNode leafNode = (LeafNode)node;
                    Binding binding = new Binding("Checked", Option.Property, "rentARoomScheme");
                    binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    ControlPanel.realWorldTreeView.DataBindings.Add(binding); 
                }
                //UpdateSeries(node.Nodes); //THIS SHOULD NOT BE HERE?
            }
        }//DONT USE THIS!!!
        
        Rectangle GetCheckBoxRectangle(Rectangle bounds, int check) {
            return new Rectangle(80 + bounds.Left + 2 + (GlyphSize.Width + 4) * check,
                                 bounds.Y + 2,
                                 GlyphSize.Width,
                                 GlyphSize.Height);
        }

        private Rectangle GetNodeBounds(System.Windows.Forms.TreeNode node) {
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

        public TreeNode GetNodeFromPath(TreeNodeCollection nodes, string path) {
            TreeNode foundNode = null;
            foreach (TreeNode treeNode in nodes) {
                if (treeNode.FullPath == path) {
                    return treeNode; //If found it, your done. (this is the first, or INNER thing sent)
                }
                else if (treeNode.Nodes.Count > 0) {
                    foundNode = GetNodeFromPath(treeNode.Nodes, path);
                }//if not found it, AND it has kids, search them...
                if (foundNode != null)
                    return foundNode; //if found a match, your are done... keep sending it back up a level.
            }//If has no children, AND/OR no treeNode returned, then try next node...
            return null; //nothing ever found on that layer, return null...
        }
        //NOTE: Slow, because checks EVERY NODE, rather than following path. OK for now...

        public void LabelTree(TreeNodeCollection nodes)
        {
            foreach (LeafNode node in nodes)
            {
                node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                LabelTree(node.Nodes);
            }
        }

        public void UpdateShowInChartList(Object sender, TreeNodeMouseClickEventArgs e){          
            if (e == null)
                return;
            if (e.Node is LeafNode leafNode)
            {
                //If clicked checkBox 0
                if (GetCheckBoxRectangle(leafNode.Bounds, 0).Contains(e.Location))
                    leafNode.ShowInChartList[0] = !leafNode.ShowInChartList[0];
                //If clicked checkBox 1
                else if (GetCheckBoxRectangle(leafNode.Bounds, 1).Contains(e.Location))
                    leafNode.ShowInChartList[1] = !leafNode.ShowInChartList[1];
                //Of clicked checkBox 2
                else if (GetCheckBoxRectangle(leafNode.Bounds, 2).Contains(e.Location))
                    leafNode.ShowInChartList[2] = !leafNode.ShowInChartList[2];
                Invalidate();
            }
        }

    }
}
