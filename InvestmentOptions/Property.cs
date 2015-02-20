using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel; //yeah, because this is now the model for a component! A viewModel!
using System.Threading;

namespace InvestmentOptions {
    public class Property : BranchNode, INotifyPropertyChanged {
        //public SynchronizationContext context = SynchronizationContext.Current;
        public event PropertyChangedEventHandler PropertyChanged;
        InvestmentOption option;
        public float price;
        public enum BuyType { toLiveIn, toLet }; //buyToLive or buyToLet
        public BuyType buyType;
        private bool _rentARoomScheme = false;
        public bool rentARoomScheme {
            get {
                return _rentARoomScheme;
            }
            set {
                _rentARoomScheme = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RentARoomScheme")); //note, name the set method,
                //not the field. In c#, call these methods "properties".
                option.projectionPanel.presentInvestmentOption(option);
                //use form or panel, to try and just update the panel?
            }
        }
        public LeafNode incomeTax;
        public LeafNode ingoings;
        public LeafNode outgoings;
        public float tenantCount;
        public float originalTenantCount = 2; //default
        public float oneTenantsRent = 330;
        public LeafNode agentsFee;
        //Not really worth it! That's 8-10 hours a month! That is easy. (or is it?)
        //No, not easy, BUT won't take that much time in reality (or will it? per tenant?).
        //Unlikely, but what can you do. I am really constrained! Sure, if want to, can manage one room self,
        //and see how it goes... //Could do self in less, easy! //Or pay Dad to do it!
        public LeafNode wearAndTear;
        public float buildingsInsurance;
        public LeafNode accountantsFee;
        //PROPERTY MAINTENANCE
        //15%. 10% for finding tenant, 5% for all else...
        //BUT hey, 10% is fine, since cost of not finding tenant for one month is 8%!
        //OR could find a nice student/smart kid, and employ him to do it! Pay him very well!
        public LeafNode profit;
        public LeafNode tenantsRent;
        public LeafNode returnOnInvestment;
        public LeafNode costs;
        public LeafNode taxableTenantsRent;

        public Property(InvestmentOption option) {
            profit = new LeafNode("propertyProfit", intervals);
            this.option = option;
            Nodes.Add(incomeTax = new LeafNode("incomeTax", intervals)); //BOOM,
            //YES!, instantiating and assigning at same time!
            Nodes.Add(ingoings = new LeafNode("ingoings", intervals));
            Nodes.Add(outgoings = new LeafNode("outgoings", intervals));
            Nodes.Add(wearAndTear = new LeafNode("wearAndTear", intervals));
            Nodes.Add(agentsFee = new LeafNode("agentsFee", intervals));
            Nodes.Add(accountantsFee = new LeafNode("accountantsFee", intervals));
            Nodes.Add(profit = new LeafNode("profit", intervals));
            Nodes.Add(tenantsRent = new LeafNode("tenantsRent", intervals));
            Nodes.Add(returnOnInvestment = new LeafNode("returnOnInvestment", intervals));
            Nodes.Add(costs = new LeafNode("costs", intervals));
            Nodes.Add(taxableTenantsRent = new LeafNode("taxableTenantsRent", intervals));
        }

        public void calculatePropertyOutgoings() {
            outgoings.monthlyValue = agentsFee.monthlyValue + wearAndTear.monthlyValue + buildingsInsurance +
                accountantsFee.monthlyValue + option.mortgage.interest.monthlyValue + 
                option.mortgage.repayment.monthlyValue;
        }

        public void calculatePropertyIncomeTax() {
            //Note, not quite right, if exceeds rent a room scheme limit!
            taxableTenantsRent.monthlyValue = tenantsRent.monthlyValue;
            calculatePropertyOutgoings();
            profit.monthlyValue = taxableTenantsRent.monthlyValue - 
                (outgoings.monthlyValue - option.mortgage.repayment.monthlyValue);
            switch (buyType) {
                case Property.BuyType.toLiveIn: //Buy to live in
                    if (rentARoomScheme) {
                        taxableTenantsRent.monthlyValue = tenantsRent.monthlyValue - 4250 / 12;
                        profit.monthlyValue = taxableTenantsRent.monthlyValue;
                    }
                    else {
                        //do nothing.
                    }
                    //calculated here, because if use it, CANNOT claim back tax on your letting expenses!
                    //As gen rule, if expenses are LESS than £4250, then rent room scheme is better for you.
                    if (taxableTenantsRent.monthlyValue < 0) taxableTenantsRent.monthlyValue = 0;
                    break;
                case Property.BuyType.toLet: //Buy to let
                    //do nothing
                    break;
            }
            incomeTax.monthlyValue = profit.monthlyValue * 0.20f;
            if (incomeTax.monthlyValue < 0) incomeTax.monthlyValue = 0;
        }
    }
}
