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
        public enum Location { Nottingham, London };
        public Location location = Location.Nottingham;
        public MyBoolean rentARoomScheme = new MyBoolean("rentARoomScheme");
        public static MyBoolean includeWearAndTear = new MyBoolean("includeWearAndTear");
        public LeafNode incomeTax;
        public LeafNode ingoings;
        public LeafNode outgoings;
        public LeafNode tenantCount;
        public int originalPropertyCount = 1; //default
        public int propertyCount;
        public int originalBedroomCount;
        public int bedroomsPerHouse = 2;
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
        public LeafNode profitAndSavings;
        public LeafNode taxableProfit;
        public LeafNode tenantsRent;
        public LeafNode returnOnInvestment;
        public LeafNode costs;
        public LeafNode taxableTenantsRent;
        public LeafNode capitalGains;
        public LeafNode taxableCapitalGains;
        public LeafNode taxableCapitalGainsBasic;
        public LeafNode taxableCapitalGainsHigher;
        public float capitalGainsAllowance = 11000;
        public LeafNode allowableCapitalGains = new LeafNode();
        //public LeafNode incomeAndTaxableCG;
        public LeafNode capitalGainsTax;
        public LeafNode capitalGainsProfit;
        public LeafNode rentSavings;
        public float percentageGrowth;

        public Property(InvestmentOption option) : base("property") {
            Nodes.Add(ingoings = new LeafNode("propertyingoings"));
            Nodes.Add(tenantCount = new LeafNode("tenantCount"));
            Nodes.Add(tenantsRent = new LeafNode("tenantsRent"));
            Nodes.Add(taxableTenantsRent = new LeafNode("taxableTenantsRent"));
            Nodes.Add(outgoings = new LeafNode("propertyoutgoings"));
            Nodes.Add(costs = new LeafNode("propertycosts"));
            Nodes.Add(wearAndTear = new LeafNode("wearAndTear"));
            Nodes.Add(agentsFee = new LeafNode("agentsFee"));
            Nodes.Add(accountantsFee = new LeafNode("accountantsFee"));
            Nodes.Add(profitAndSavings = new LeafNode("propertyProfitAndSavings"));
            Nodes.Add(taxableProfit = new LeafNode("taxableProfit"));
            Nodes.Add(incomeTax = new LeafNode("incomeTax")); //BOOM,instant'g and assign'g same time!
            Nodes.Add(returnOnInvestment = new LeafNode("returnOnInvestment"));
            Nodes.Add(capitalGains = new LeafNode("capitalGains"));
            capitalGains.showInChartList[0] = true;
            //SO, value, should not be a float, should be an object...
            //SO, can go capitalGains.mv.showInChartsList[0]... And it shows it!
            //could go monthly.capitalGains.value.showInChartsList (BUT, then harder to link cum, and monthly...
            //could have Class called Bundle... instances are monthly, and cumulative... 
            //have all the leafNodes in them... SO, call monthly.capitalGains.value .... edit it,,,
            //then automatically, bundle.capitalGains is updated... NO WOULD NOT WORK!
            //SO that is why do it the other way.... Want to keep closely related things,
            //in the same class... means less sometin.somet.somting.value...
            //NOTE: Dotted object names (monthly.value), are NOT just a way to put more adjectives in...
            //They show when a property BELONGS to another object...
            //SO in a way, monthly, should not be an object... (only, if it represents a property/object).
            //red, should not be an object...
            //SIDE: Could do whatever... basically, comes down to, do I want to have to specify whole route,
            //if I am going to be calling it, from here? 
            //i.e. it depends on the context... assumed context, that you want...
            //AND keeping them as objects, IS a good rule... 
            //so proper name would be monthlyValueGrouping, but I would let you call it monthly.
            //NOTE object.value is the correct way! NOTE, can define how + + is interpretted!
            //NOTE also, should all be handled as data (in a database) really, not objects.
            //but objects ok actually.
            Nodes.Add(taxableCapitalGains = new LeafNode("taxableCapitalGains"));
            taxableCapitalGains.showInChartList[1] = true;
            Nodes.Add(taxableCapitalGainsBasic = new LeafNode("taxableCapitalGainsBasic"));
            taxableCapitalGainsBasic.showInChartList[0] = true;
            taxableCapitalGainsBasic.showInChartList[1] = true;
            Nodes.Add(taxableCapitalGainsHigher = new LeafNode("taxableCapitalGainsHigher"));
            taxableCapitalGainsHigher.showInChartList[0] = true;
            taxableCapitalGainsHigher.showInChartList[1] = true;
            Nodes.Add(capitalGainsTax = new LeafNode("capitalGainsTax"));
            Nodes.Add(capitalGainsProfit = new LeafNode("capitalGainsProfit"));
            capitalGainsProfit.showInChartList[0] = true;
            Nodes.Add(rentSavings = new LeafNode("rentSavings"));
            this.option = option;
        }

        public void buyNewProperty() {
            propertyCount++;
            tenantCount.mv += bedroomsPerHouse;
            updateMultiples();
        }

        public void calculatePropertyOutgoings() {
            outgoings.mv = agentsFee.mv + wearAndTear.mv + buildingsInsurance +
                accountantsFee.mv + option.realWorldTree.mortgage.interest.mv +
                option.realWorldTree.mortgage.repayment.mv;
        }

        public void calculateCapitalGainsProfit() {
            allowableCapitalGains.basic = (32010 - 12000) / option.intervals;
            //HAVE to divide by intervals, because this is split, over ALL these intervals... (only sell ONCE).
            //12000 is, say my oldAge income, when I sell the house...
            allowableCapitalGains.higher = (150000 - 12000) / option.intervals; //??? guess..
            switch (location) {
                case Location.Nottingham:
                    percentageGrowth = 0.03f / 12;
                    break;
                case Location.London:
                    percentageGrowth = 0.07f / 12;
                    break;
            }
            //WHATTTT!!! BUT I made 30,000 in capitalGains...
            //AND allowable capGains is 20,000.
            //SO SURELY SOME of my taxableCapGains falls in to the HIGHER bracket???
            //AHHH but allowable CapGains is £11,000, SO allowable capGains is 20,000 + 11,000 = £31,000,
            //SO I am JUST ABOUT OK!!! only pay 18% on most of my capGains...
            capitalGains.mvs = option.realWorldTree.mortgage.housePrice * percentageGrowth;
            capitalGains.mv = propertyCount * capitalGains.mvs;
            capitalGains.cumulativeValue += capitalGains.mv;
            taxableCapitalGains.mvs = capitalGains.mvs - capitalGainsAllowance / option.intervals;
            taxableCapitalGains.mv = propertyCount * taxableCapitalGains.mvs;
            //NOW remember, selling one house each year!!! THATS WHY got the mvs below...
            if (taxableCapitalGains.mvs < allowableCapitalGains.basic) { //Split it up...
                taxableCapitalGainsBasic.mv = taxableCapitalGains.mvs;
            }
            else if (taxableCapitalGains.mvs < allowableCapitalGains.higher) { //it gets more complicated
                //pay 28% on any amount above this...
                taxableCapitalGainsBasic.mv = allowableCapitalGains.basic;
                taxableCapitalGainsHigher.mv = taxableCapitalGains.mvs - taxableCapitalGainsBasic.mv;
            }
            capitalGainsTax.mvs = taxableCapitalGainsBasic.mv * 0.18f
                                + taxableCapitalGainsHigher.mv * 0.28f;
            capitalGainsTax.mv = propertyCount * capitalGainsTax.mvs;
            if (buyType == BuyType.toLiveIn)
                capitalGainsTax.mv = (propertyCount - 1) * capitalGainsTax.mvs;
            capitalGainsProfit.mv = capitalGains.mv - capitalGainsTax.mv;
            capitalGainsProfit.cumulativeValue += capitalGainsProfit.mv;
            taxableCapitalGainsBasic.cumulativeValue += taxableCapitalGainsBasic.mv;
            taxableCapitalGainsHigher.cumulativeValue += taxableCapitalGainsHigher.mv;
        }

        public void calculatePropertyIncomeTax() {
            taxableTenantsRent.mv = tenantsRent.mv;
            calculatePropertyOutgoings();
            taxableProfit.mv = taxableTenantsRent.mv -
                (outgoings.mv - option.realWorldTree.mortgage.repayment.mv);
            rentSavings.mv = 0;
            switch (buyType) {
                case Property.BuyType.toLiveIn: //Buy to live in
                    rentSavings.mv = option.realWorldTree.shelter.typicalRent;
                    if (rentARoomScheme.value) {
                        taxableTenantsRent.mv = tenantsRent.mv - 4250 / 12;
                        //if (taxableTenantsRent.mv < 0) taxableTenantsRent.mv = 0;
                        //not nec... profit CAN be negative... Just shows, that losing money...
                        //BUT, it is right include rent saving, as income!
                        //BUT, they cannot TAX you for it, so change that...    
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
            profitAndSavings.mv = taxableProfit.mv - incomeTax.mv + rentSavings.mv;
            if (option.zeroInvestment) {
                profitAndSavings.mv = option.realWorldTree.mortgage.repayment.cumulativeValue * option.realWorldTree.mortgage.interestRate; 
            }
        }

        public void resetCumulativeValues() {
            capitalGains.cumulativeValue = 0;
            capitalGainsProfit.cumulativeValue = 0;
            taxableCapitalGainsBasic.cumulativeValue = 0;
            taxableCapitalGainsHigher.cumulativeValue = 0;
        }

        public void resetIndependentVariables() {
            //RESET PROPERTY COUNT AND TENANT COUNT
            originalBedroomCount = bedroomsPerHouse;
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
            resetCumulativeValues();
        } //Could call this with an event, OR just call it in upper method...
        //Note, event might be easier, because just define the reaction in BASE class,
        //then every object created, will update itself automatically!!! //CBTL

        public void updateMultiples() {
            tenantsRent.mv = tenantCount.mv * oneTenantsRent;
            if (option.zeroInvestment) {
                tenantsRent.mv = 0;
            }
            agentsFee.mv = 0;
            wearAndTear.mv = propertyCount * 0;
            if (Property.includeWearAndTear.value) {
                agentsFee.mv = tenantsRent.mv * 0.15f;
                wearAndTear.mv = propertyCount * 90; //now, that's fairer.
            }
            buildingsInsurance = propertyCount * 12;
            accountantsFee.mv = propertyCount * 9; //numbers are the costs
        }

        public void updateVariables() {
            calculatePropertyIncomeTax();
            costs.mv = outgoings.mv - option.realWorldTree.mortgage.repayment.mv;
            if (option.zeroInvestment) {
                costs.mv = 0;
            }
            moneyInvested += option.realWorldTree.mortgage.repayment.mv;
            returnOnInvestment.mv = 12 * profitAndSavings.mv / moneyInvested * 100;
            calculateCapitalGainsProfit();
        }

    }
}
