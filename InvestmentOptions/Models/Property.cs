using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace InvestmentOptions {
    public class Property : BranchNode {
        //NOTE: You can express which LEAFNODES are DEPENDENT, and INDEPENDENT,
        //with private and public!
        //or setters, vs no setters!
        public float price;
        public enum BuyType { toLiveIn, toLet }; //buyToLive or buyToLet
        public BuyType buyType;
        public enum Location { Nottingham, London };
        public Location location = Location.Nottingham;
        public Boolean rentARoomScheme = new Boolean("rentARoomScheme");
        public static Boolean includeWearAndTear = new Boolean("includeWearAndTear");
        public LeafNode IncomeTax
        {
            get
            {
                return new LeafNode("incomeTax")
                { mv = TaxableProfit.mv > 0 ? TaxableProfit.mv * 0.20f : 0 };
            }
        }
        public LeafNode Ingoings
        {
            get
            {
                return new LeafNode("ingoings")
                { mv = TenantsRent.mv - IncomeTax.mv };
            }
        }
        public LeafNode Outgoings
        {
            get
            {
                return new LeafNode("ingoings")
                {
                    mv =
                    AgentsFee.mv + WearAndTear.mv + BuildingsInsurance +
                    AccountantsFee.mv + Option.Mortgage.interest.mv +
                    Option.Mortgage.repayment.mv
                };
            }
        }
        public float OriginalHousePrice { get; set; } = 100000;
        public float housePrice;
        public int OriginalPropertyCount { get; set; }
        public int PropertyCount { get; set; }
        public int BedroomsPerHouse { get; set; } = 2;
        public LeafNode TenantCount {
            get
            {
                if (buyType == BuyType.toLet)
                    return new LeafNode("tenantCount")
                    { mv = BedroomsPerHouse * PropertyCount };
                else
                    return new LeafNode("tenantCount")
                    { mv = BedroomsPerHouse * PropertyCount - 1 };
            }
        }
        public float OneTenantsRent { get; set; } = 330;
        public LeafNode TenantsRent {
            get
            {
                return new LeafNode("tenantsRent")
                { mv = TenantCount.mv * OneTenantsRent };
            }
        }
        public float moneyInvested;
        public LeafNode AgentsFee
        {
            get
            {
                if (includeWearAndTear.Value)
                    return new LeafNode("agentsFee")
                    { mv = TenantsRent.mv * 0.15f };
                else
                    return new LeafNode("agentsFee")
                    { mv = 0 };

            }
            
        }
        //Not really worth it! That's 8-10 hours a month! That is easy. (or is it?)
        //No, not easy, BUT won't take that much time in reality (or will it? per tenant?).
        //Unlikely, but what can you do. I am really constrained! Sure, if want to, can manage one room self,
        //and see how it goes... //Could do self in less, easy! //Or pay Dad to do it!
        //PROPERTY MAINTENANCE
        //15%. 10% for finding tenant, 5% for all else...
        //BUT hey, 10% is fine, since cost of not finding tenant for one month is 8%!
        //OR could find a nice student/smart kid, and employ him to do it! Pay him very well!
        public LeafNode WearAndTear
        {
            get
            {
                if (includeWearAndTear.Value)
                    return new LeafNode("wearAndTear")
                    { mv = PropertyCount * 90 };
                else
                    return new LeafNode("wearAndTear")
                    { mv = 0 };
            }
        }
        public float BuildingsInsurance
        {
            get { return PropertyCount * 12; }
        }
        public LeafNode AccountantsFee
        {
            get
            {
                return new LeafNode("accountantsFee")
                { mv = 9 * PropertyCount }; //9 pounds
            }
        }
        public LeafNode profitAndSavings;
        public LeafNode TaxableProfit
        {
            get
            {
                if (buyType == BuyType.toLiveIn && rentARoomScheme.Value)
                    return new LeafNode("taxableProfit")
                    { mv = TaxableTenantsRent.mv };
                else
                    return new LeafNode("taxableProfit")
                    { mv = TaxableTenantsRent.mv - (Outgoings.mv - Option.Mortgage.repayment.mv) };
                //NOTE: Not sure taxableTenantsRent really needed... confuses things?
                //calculated here, because if use it, CANNOT claim back tax on your letting expenses!
                //As gen rule, if expenses are LESS than £4250, then rent room scheme is better for you.
            }
        }
        public LeafNode returnOnInvestment;
        public LeafNode costs;
        public LeafNode TaxableTenantsRent
        { 
            get
            {
                if (buyType == BuyType.toLiveIn && rentARoomScheme.Value)
                    return new LeafNode("taxableTenantsRent")
                    { mv = TenantsRent.mv - 4250 / 12 };
                //if (taxableTenantsRent.mv < 0) taxableTenantsRent.mv = 0;
                //not nec... profit CAN be negative... Just shows, that losing money...
                //BUT, it is right include rent saving, as income!
                //BUT, they cannot TAX you for it, so change that...   
                else return TenantsRent;
            }
        }
        public LeafNode capitalGains;
        public LeafNode taxableCapitalGains;
        public LeafNode taxableCapitalGainsBasic;
        public LeafNode taxableCapitalGainsHigher;
        public float capitalGainsAllowance = 11000;
        public LeafNode allowableCapitalGains;
        //public LeafNode incomeAndTaxableCG;
        public LeafNode capitalGainsTax;
        public LeafNode capitalGainsProfit;
        public LeafNode RentSavings
        {
            get
            {
                if (buyType == BuyType.toLiveIn)
                    return new LeafNode("rentSavings")
                    { mv = Option.Shelter.typicalRent };
                else
                    return new LeafNode("rentSavings")
                    { mv = 0 };
            }
        }
        public float percentageGrowth;

        public Property(InvestmentOption option) : base("property") {
            Option = option;
            Nodes.Add(Ingoings);
            Nodes.Add(TenantCount); //Somewhat pointless passing this reference now,
            //As it gets reinstantiated each time its accessed?
            //WELL that is fine! each time nodes accesses it,
            //it gets a fresh up-to-date object, nice!
            TenantCount.ShowInChartList[3] = true;
            Nodes.Add(TenantsRent);
            TenantsRent.ShowInChartList[1] = true;
            Nodes.Add(TaxableTenantsRent);
            TaxableTenantsRent.ShowInChartList[1] = true;
            Nodes.Add(Outgoings);
            Nodes.Add(costs = new LeafNode("costs"));
            costs.ShowInChartList[1] = true;
            Nodes.Add(WearAndTear);
            WearAndTear.ShowInChartList[2] = true;
            Nodes.Add(AgentsFee);
            AgentsFee.ShowInChartList[2] = true;
            Nodes.Add(AccountantsFee);
            AccountantsFee.ShowInChartList[2] = true;
            Nodes.Add(profitAndSavings = new LeafNode("profitAndSavings"));
            profitAndSavings.ShowInChartList[1] = true;
            Nodes.Add(TaxableProfit);
            TaxableProfit.ShowInChartList[1] = true;
            Nodes.Add(IncomeTax); 
            IncomeTax.ShowInChartList[1] = true;
            Nodes.Add(returnOnInvestment = new LeafNode("returnOnInvestment"));
            returnOnInvestment.ShowInChartList[3] = true;
            Nodes.Add(capitalGains = new LeafNode("capitalGains"));
            capitalGains.ShowInChartList[0] = true; 
            Nodes.Add(taxableCapitalGains = new LeafNode("taxableCapitalGains"));
            taxableCapitalGains.ShowInChartList[1] = true;
            Nodes.Add(taxableCapitalGainsBasic = new LeafNode("taxableCapitalGainsBasic"));
            taxableCapitalGainsBasic.ShowInChartList[0] = true;
            taxableCapitalGainsBasic.ShowInChartList[1] = true;
            Nodes.Add(taxableCapitalGainsHigher = new LeafNode("taxableCapitalGainsHigher"));
            taxableCapitalGainsHigher.ShowInChartList[0] = true;
            taxableCapitalGainsHigher.ShowInChartList[1] = true;
            Nodes.Add(allowableCapitalGains = new LeafNode("allowableCapitalGains"));
            Nodes.Add(capitalGainsTax = new LeafNode("capitalGainsTax"));
            Nodes.Add(capitalGainsProfit = new LeafNode("capitalGainsProfit"));
            capitalGainsProfit.ShowInChartList[0] = true;
            Nodes.Add(RentSavings);
        }

        public void BuyNewProperty() {
            PropertyCount++;
            TenantCount.mv += BedroomsPerHouse;
        }

        public void CalculateCapitalGainsProfit() {
            allowableCapitalGains.basic = (32010 - 12000) / Option.Intervals;
            //HAVE to divide by intervals, because this is split, over ALL these intervals... (only sell ONCE).
            //12000 is, say my oldAge income, when I sell the house...
            allowableCapitalGains.higher = (150000 - 12000) / Option.Intervals; //??? guess..
            switch (location) {
                case Location.Nottingham:
                    percentageGrowth = 0.035f / 12;
                    break;
                case Location.London:
                    percentageGrowth = 0.045f / 12;
                    break;
            }
            //WHATTTT!!! BUT I made 30,000 in capitalGains...
            //AND allowable capGains is 20,000.
            //SO SURELY SOME of my taxableCapGains falls in to the HIGHER bracket???
            //AHHH but allowable CapGains is £11,000, SO allowable capGains is 20,000 + 11,000 = £31,000,
            //SO I am JUST ABOUT OK!!! only pay 18% on most of my capGains...
            capitalGains.mvs = housePrice * percentageGrowth;
            capitalGains.mv = PropertyCount * capitalGains.mvs;
            capitalGains.cumulativeValue += capitalGains.mv;
            taxableCapitalGains.mvs = capitalGains.mvs - capitalGainsAllowance / Option.Intervals;
            taxableCapitalGains.mv = PropertyCount * taxableCapitalGains.mvs;
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
            capitalGainsTax.mv = PropertyCount * capitalGainsTax.mvs;
            if (buyType == BuyType.toLiveIn)
                capitalGainsTax.mv = (PropertyCount - 1) * capitalGainsTax.mvs;
            capitalGainsProfit.mv = capitalGains.mv - capitalGainsTax.mv;
            capitalGainsProfit.cumulativeValue += capitalGainsProfit.mv;
            taxableCapitalGainsBasic.cumulativeValue += taxableCapitalGainsBasic.mv;
            taxableCapitalGainsHigher.cumulativeValue += taxableCapitalGainsHigher.mv;
        }

        public void CalculatePropertyIncomeTax() {
            
            profitAndSavings.mv = TaxableProfit.mv - IncomeTax.mv + RentSavings.mv;
            if (Option.zeroInvestment) {
                profitAndSavings.mv = 
                    Option.Mortgage.repayment.cumulativeValue * Option.Mortgage.interestRate; 
            }
        }

        public void ResetCumulativeValues() {
            capitalGains.cumulativeValue = 0;
            capitalGainsProfit.cumulativeValue = 0;
            taxableCapitalGainsBasic.cumulativeValue = 0;
            taxableCapitalGainsHigher.cumulativeValue = 0;
        }

        public void ResetIndependentVariables() {
            PropertyCount = OriginalPropertyCount; 
            //Independent, but changes after certain events. Needs a setter.
            //If dependent, all logic done in getter. No setter required.
            //If no change at all, but independent, then again no setter required. 
            housePrice = OriginalHousePrice;
            ResetCumulativeValues();
            //Could call this with an event, OR just call it in upper method...
            //Note, event might be easier, because just define the reaction in BASE class,
            //then every object created, will update itself automatically!!! //CBTL
        }

        public void UpdateVariables() {
            CalculatePropertyIncomeTax();
            costs.mv = Outgoings.mv - Option.Mortgage.repayment.mv;
            if (Option.zeroInvestment) {
                costs.mv = 0;
            }
            moneyInvested += Option.Mortgage.repayment.mv;
            returnOnInvestment.mv = 12 * profitAndSavings.mv / moneyInvested * 100;
            CalculateCapitalGainsProfit();
            housePrice = housePrice * (1 + percentageGrowth);
        }

    }
}
