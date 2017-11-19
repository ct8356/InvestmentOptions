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
        public Location location { get; set; } = Location.Nottingham;
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
        public LeafNode profitAndSavings
        {
            get
            {
                if (Option.zeroInvestment)
                    return new LeafNode("profitAndSavings")
                    {
                        mv = Option.Mortgage.repayment.cumulativeValue *
                        Option.Mortgage.interestRate
                    };
                else
                    return new LeafNode("profitAndSavings")
                    { mv = TaxableProfit.mv - IncomeTax.mv + RentSavings.mv };
            }
        }
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
        public LeafNode ReturnOnInvestment
        {
            get
            {
                return new LeafNode("returnOnInvestment")
                { mv = 12 * profitAndSavings.mv / moneyInvested * 100 };
            }
        }
        public LeafNode Costs
        {
            get {
                if (Option.zeroInvestment)
                    return new LeafNode("costs")
                    { mv = 0 };
                else
                    return new LeafNode("costs")
                    { mv = Outgoings.mv - Option.Mortgage.repayment.mv };
            }
        }
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
        public LeafNode CapitalGains
        {
            get
            {
                return new LeafNode("capitalGains")
                {
                    mvs = housePrice * PercentageGrowth,
                    mv = PropertyCount * housePrice * PercentageGrowth,
                };
            }
        }
        public LeafNode TaxableCapitalGains
        {
            get
            {
                return new LeafNode("taxableCapitalGains")
                {
                    mvs = CapitalGains.mvs - capitalGainsAllowance / Option.Intervals,
                    mv = PropertyCount * CapitalGains.mvs - capitalGainsAllowance / Option.Intervals,
                };
            }
        }
        public LeafNode BasicTaxableCapitalGains {
            get
            {
                //NOW remember, selling one house each year!!! THATS WHY got the mvs below...
                if (TaxableCapitalGains.mvs < BasicAllowableCapitalGains.mv)
                    return new LeafNode("BasicTaxableCapitalGains")
                    { mv = TaxableCapitalGains.mvs };
                else
                    //it gets more complicated
                    //pay 28% on any amount above this...
                    return new LeafNode("BasicTaxableCapitalGains")
                    { mv = BasicAllowableCapitalGains.mv };
            }
        }
        public LeafNode HigherTaxableCapitalGains {
            get
            {
                if (TaxableCapitalGains.mvs < HigherAllowableCapitalGains)
                    //it gets more complicated
                    //pay 28% on any amount above this...
                    return new LeafNode("HigherTaxableCapitalGains")
                    { mv = TaxableCapitalGains.mvs - BasicTaxableCapitalGains.mv };
                else
                    return new LeafNode("HigherTaxableCapitalGains")
                    { mv = 0 };
            }
        }
        public float capitalGainsAllowance = 11000;
        public LeafNode BasicAllowableCapitalGains
        {
            get
            {
                return new LeafNode("basicAllowableCapitalGains")
                { mv = (32010 - 12000) / Option.Intervals };
                //HAVE to divide by intervals, because this is split, over ALL these intervals.
                //(only sell ONCE).
                //32010 is the cut off point (yearly) for basic allowable CG.
                //12000 is, say my oldAge income, when I sell the house...
            }
        }
        public float HigherAllowableCapitalGains
        {
            get
            {
                return (150000 - 12000) / Option.Intervals;
                //??? guess..
            }
        }
        //public LeafNode incomeAndTaxableCG;
        public LeafNode CapitalGainsTax
        {
            get
            {
                if (buyType == BuyType.toLiveIn)
                    return new LeafNode("CapitalGainsTax")
                    {
                        mvs =
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f,
                        mv = 
                        (PropertyCount - 1) *
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f,
                    };
                else
                    return new LeafNode("CapitalGainsTax")
                    {
                        mvs =
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f,
                        mv =
                        PropertyCount *
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f,
                    };
                //WHATTTT!!! BUT I made 30,000 in capitalGains...
                //AND allowable capGains is 20,000.
                //SO SURELY SOME of my taxableCapGains falls in to the HIGHER bracket???
                //AHHH but allowable CapGains is £11,000, SO allowable capGains is 20,000 + 11,000 = £31,000,
                //SO I am JUST ABOUT OK!!! only pay 18% on most of my capGains...    
            }
        }
        public LeafNode CapitalGainsProfit
        {
            get
            {
                return new LeafNode("capitalGainsProfit")
                { mv = CapitalGains.mv - CapitalGainsTax.mv };
            }
        }
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
        public float PercentageGrowth
        {
            get
            {
                switch (location)
                {
                    case Location.Nottingham:
                        return 0.035f / 12;
                    case Location.London:
                        return 0.045f / 12;
                }
                return 0.035f / 12;
            }
        }

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
            Nodes.Add(Costs);
            Costs.ShowInChartList[1] = true;
            Nodes.Add(WearAndTear);
            WearAndTear.ShowInChartList[2] = true;
            Nodes.Add(AgentsFee);
            AgentsFee.ShowInChartList[2] = true;
            Nodes.Add(AccountantsFee);
            AccountantsFee.ShowInChartList[2] = true;
            Nodes.Add(profitAndSavings);
            profitAndSavings.ShowInChartList[1] = true;
            Nodes.Add(TaxableProfit);
            TaxableProfit.ShowInChartList[1] = true;
            Nodes.Add(IncomeTax); 
            IncomeTax.ShowInChartList[1] = true;
            Nodes.Add(ReturnOnInvestment);
            ReturnOnInvestment.ShowInChartList[3] = true;
            Nodes.Add(CapitalGains);
            CapitalGains.ShowInChartList[0] = true; 
            Nodes.Add(TaxableCapitalGains);
            TaxableCapitalGains.ShowInChartList[1] = true;
            Nodes.Add(BasicTaxableCapitalGains);
            BasicTaxableCapitalGains.ShowInChartList[0] = true;
            BasicTaxableCapitalGains.ShowInChartList[1] = true;
            Nodes.Add(HigherTaxableCapitalGains);
            HigherTaxableCapitalGains.ShowInChartList[0] = true;
            HigherTaxableCapitalGains.ShowInChartList[1] = true;
            Nodes.Add(CapitalGainsTax);
            Nodes.Add(CapitalGainsProfit);
            CapitalGainsProfit.ShowInChartList[0] = true;
            Nodes.Add(RentSavings);
        }

        public void BuyNewProperty() {
            PropertyCount++;
        }

        public void ResetCumulativeValues() {
            CapitalGains.cumulativeValue = 0;
            CapitalGainsProfit.cumulativeValue = 0;
            BasicTaxableCapitalGains.cumulativeValue = 0;
            HigherTaxableCapitalGains.cumulativeValue = 0;
        }

        public void ResetIndependentVariables() {
            PropertyCount = OriginalPropertyCount; 
            housePrice = OriginalHousePrice;
            ResetCumulativeValues();
        }

        public void UpdateIndependentVariables() {
            moneyInvested += Option.Mortgage.repayment.mv;
            housePrice = housePrice * (1 + PercentageGrowth);
            //cumValues
            CapitalGains.cumulativeValue += CapitalGains.mv;
            CapitalGainsProfit.cumulativeValue += CapitalGainsProfit.mv;
            BasicTaxableCapitalGains.cumulativeValue += BasicTaxableCapitalGains.mv;
            HigherTaxableCapitalGains.cumulativeValue += HigherTaxableCapitalGains.mv;
        }

    }
}
