using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class Shelter : BranchNode {
        public LeafNode outgoings;
        public LeafNode rent;
        public LeafNode houseCosts;
        public LeafNode houseBills;
        public LeafNode councilTax;
        public float typicalRent = 330;

        public Shelter(InvestmentOption option) : base("shelter") {
            Nodes.Add(outgoings = new LeafNode("outgoings"));
            Nodes.Add(rent = new LeafNode("rentN"));
            Nodes.Add(houseCosts = new LeafNode("houseCosts"));
            Nodes.Add(houseBills = new LeafNode("houseBills"));
            Nodes.Add(councilTax = new LeafNode("councilTax"));
            houseBills.mv = 45;
            councilTax.mv = 100;
            this.option = option;
        }

        public void resetVariables() {
            if (option.property.buyType == Property.BuyType.toLet) {
                rent.mv = typicalRent;
            }
            else if (option.property.buyType == Property.BuyType.toLiveIn) {
                rent.mv = 0;
            }
            //RESET REST
            //
        } //Could call this with an event, OR just call it in upper method...

    }
}
