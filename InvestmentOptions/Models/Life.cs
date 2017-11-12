using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {
    public class Life : BranchNode {

        public LeafNode outgoings;
        LeafNode _costs;
        public LeafNode costs {
            get {
                _costs.mv = phoneBill + food + randomObjects + danceLessons + randomEventsAndServices
                    + charity;
                return _costs;
            }
            set {
                _costs = value;
            }
        }
        public float phoneBill = 10;
        public float food = 140;
        public float randomObjects = 120;
        public float danceLessons = 18;
        public float randomEventsAndServices = 120;
        public float charity = 45;
        //I figure, if you want names, just have to put in two places...
        //once to declare the name,
        //once to define the relationship... or put it into a list...
        //unless, structs or unions allow auto definitions...
        //I will just have to put up with it for today...
        //it is a small cost...

        public Life(InvestmentOption option) : base("life") {
            Nodes.Add(outgoings = new LeafNode("lifeOutgoings"));
            Nodes.Add(costs = new LeafNode("lifecosts"));
            this.Option = option;
        }

        //public float sum {
        //    get {
        //        return phoneBill + food + randomObjects + danceLessons + randomEventsAndServices + charity;
        //    }
        //}
    }
}
