using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class Shelter : BranchNode {
        public InvestmentOption option;
        public LeafNode outgoings;
        public LeafNode rent;
        public LeafNode houseCosts;
        public LeafNode houseBills;
        public LeafNode councilTax;

        public Shelter(InvestmentOption option) : base("shelter", option) {
            this.option = option;
            Nodes.Add(outgoings = new LeafNode("outgoings", option));
            Nodes.Add(rent = new LeafNode("rentN", option));
            Nodes.Add(houseCosts = new LeafNode("houseCosts", option));
            Nodes.Add(houseBills = new LeafNode("houseBills", option));
            Nodes.Add(councilTax = new LeafNode("councilTax", option));
            houseBills.mv = 45;
            councilTax.mv = 100;
        }

        public void resetVariables() {
            if (option.realWorldTree.property.buyType == Property.BuyType.toLet) {
                rent.mv = 330;
            }
            else if (option.realWorldTree.property.buyType == Property.BuyType.toLiveIn) {
                rent.mv = 0;
                if (option.countRentSavingsAsIncome)
                    rent.mv = 330;
            }
            //RESET REST
            //
        } //Could call this with an event, OR just call it in upper method...

    }
}
