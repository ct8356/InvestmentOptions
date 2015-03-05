using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel; //yeah, because this is now the model for a component! A viewModel!

namespace InvestmentOptions {
    public class Property : BranchNode, INotifyPropertyChanged {
        //NOTE: You can express which LEAFNODES are DEPENDENT, and INDEPENDENT, with private and public!
        //or setters, vs no setters!!!
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
                PropertyChanged(this, new PropertyChangedEventArgs("RentARoomScheme"));
                //note, name the set method,
                //not the field. In c#, call these methods "properties".
                option.projectionPanel.updateSelf();
                //use form or panel, to try and just update the panel?
            }
        }
        public LeafNode incomeTax;
        public LeafNode ingoings;
        public LeafNode outgoings;
        public LeafNode tenantCount;
        public int originalPropertyCount = 1; //default
        public int propertyCount;
        public int originalBedroomCount;
        public float originalTenantCount; //default
        public float oneTenantsRent = 330;
        public float moneyInvested;
        public LeafNode agentsFee;
        //Not really worth it! That's 8-10 hours a month! That is easy. (or is it?)
        //No, not easy, BUT won't take that much time in reality (or will it? per tenant?).
        //Unlikely, but what can you do. I am really constrained! Sure, if want to, can manage one room self,
        //and see how it goes... //Could do self in less, easy! //Or pay Dad to do it!
        //PROPERTY MAINTENANCE
        //15%. 10% for finding tenant, 5% for all else...
        //BUT hey, 10% is fine, since cost of not finding tenant for one month is 8%!
        //OR could find a nice student/smart kid, and employ him to do it! Pay him very well!
        public LeafNode wearAndTear;
        public float buildingsInsurance;
        public LeafNode accountantsFee;
        public LeafNode profit;
        public LeafNode taxableProfit;
        public LeafNode tenantsRent;
        public LeafNode returnOnInvestment;
        public LeafNode costs;
        public LeafNode taxableTenantsRent;

        public Property(InvestmentOption option) : base("property", option){
            this.option = option;
            Nodes.Add(ingoings = new LeafNode("propertyingoings", option));
            Nodes.Add(tenantCount = new LeafNode("tenantCount", option));
            Nodes.Add(tenantsRent = new LeafNode("tenantsRent", option));
            Nodes.Add(taxableTenantsRent = new LeafNode("taxableTenantsRent", option));
            Nodes.Add(outgoings = new LeafNode("propertyoutgoings", option));
            Nodes.Add(costs = new LeafNode("propertycosts", option));
            Nodes.Add(wearAndTear = new LeafNode("wearAndTear", option));
            Nodes.Add(agentsFee = new LeafNode("agentsFee", option));
            Nodes.Add(accountantsFee = new LeafNode("accountantsFee", option));
            Nodes.Add(profit = new LeafNode("propertyProfit", option));
            Nodes.Add(taxableProfit = new LeafNode("taxableProfit", option));
            Nodes.Add(incomeTax = new LeafNode("incomeTax", option)); //BOOM,instant'g and assign'g same time!
            Nodes.Add(returnOnInvestment = new LeafNode("returnOnInvestment", option));
        }

        public void buyNewProperty() {
            propertyCount += 1;
            tenantCount.mv += 2;
            updateMultiples();
        }

        public void calculatePropertyOutgoings() {
            outgoings.mv = agentsFee.mv + wearAndTear.mv + buildingsInsurance +
                accountantsFee.mv + option.realWorldTree.mortgage.interest.mv +
                option.realWorldTree.mortgage.repayment.mv;
        }

        public void calculatePropertyIncomeTax() {
            taxableTenantsRent.mv = tenantsRent.mv;
            calculatePropertyOutgoings();
            taxableProfit.mv = taxableTenantsRent.mv -
                (outgoings.mv - option.realWorldTree.mortgage.repayment.mv);
            switch (buyType) {
                case Property.BuyType.toLiveIn: //Buy to live in
                    if (rentARoomScheme) {
                        taxableTenantsRent.mv = tenantsRent.mv - 4250 / 12;
                        if (taxableTenantsRent.mv < 0)
                            taxableTenantsRent.mv = 0;
                        taxableProfit.mv = taxableTenantsRent.mv;
                    } //NOTE: Not sure taxableTenantsRent really needed... confuses things?
                    //calculated here, because if use it, CANNOT claim back tax on your letting expenses!
                    //As gen rule, if expenses are LESS than £4250, then rent room scheme is better for you.
                    break;
                case Property.BuyType.toLet: //Buy to let
                    //do nothing
                    break;
            }
            incomeTax.mv = taxableProfit.mv * 0.20f;
            if (incomeTax.mv < 0)
                incomeTax.mv = 0;
            profit.mv = taxableProfit.mv - incomeTax.mv;
            if (option.zeroInvestment) {
                profit.mv = option.realWorldTree.mortgage.repayment.cumulativeValue * option.realWorldTree.mortgage.interestRate; 
            }
        }

        public void resetVariables() {
            //RESET PROPERTY COUNT AND TENANT COUNT
            originalBedroomCount = 2;
            propertyCount = originalPropertyCount;
            if (buyType == Property.BuyType.toLet) {
                originalTenantCount = originalBedroomCount * originalPropertyCount;
            }
            else if (buyType == Property.BuyType.toLiveIn) {
                originalTenantCount = originalBedroomCount * originalPropertyCount - 1;
            }
            if (option.zeroInvestment) {
                propertyCount = 0;
                originalBedroomCount = 0;
            }
            //RESET REST
            tenantCount.mv = originalTenantCount;
            moneyInvested = propertyCount * option.realWorldTree.mortgage.deposit;
            if (option.zeroInvestment) {
                moneyInvested = 0;
            }
            updateMultiples();
        } //Could call this with an event, OR just call it in upper method...
        //Note, event might be easier, because just define the reaction in BASE class,
        //then every object created, will update itself automatically!!! //CBTL

        public void updateMultiples() {
            tenantsRent.mv = tenantCount.mv * oneTenantsRent;
            if (option.zeroInvestment) {
                tenantsRent.mv = 0;
            }
            agentsFee.mv = tenantsRent.mv * 0.15f;
            if (option.countRentSavingsAsIncome) {
                tenantsRent.mv = (tenantCount.mv + 1) * oneTenantsRent; //extra one, for the saving...
            }//comes after agents Fee, because ME won't need an agentsFee...
            wearAndTear.mv = propertyCount * 90; //now, that's fairer...
            buildingsInsurance = propertyCount * 12;
            accountantsFee.mv = propertyCount * 9; //numbers are the costs
        }

    }
}
