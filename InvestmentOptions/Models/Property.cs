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
        public enum BuyType { toLiveIn, toLet }; //buyToLive or buyToLet
        public BuyType buyType;
        public enum Location { Nottingham, London };
        public Location location { get; set; } = Location.Nottingham;
        public Boolean rentARoomScheme = new Boolean("rentARoomScheme");
        public static Boolean includeWearAndTear = new Boolean("includeWearAndTear");
        private LeafNode _incomeTax;
        public LeafNode IncomeTax
        {
            get
            {
                _incomeTax.mv = TaxableProfit.mv > 0 ? TaxableProfit.mv * 0.20f : 0;
                return _incomeTax;
            }
            set
            {
                _incomeTax = value;
            }
        }
        private LeafNode _ingoings;
        public LeafNode Ingoings
        {
            get
            {
                _ingoings.mv = TenantsRent.mv - IncomeTax.mv;
                return _ingoings;
            }
            set
            {
                _ingoings = value;
            }
        }
        private LeafNode _outgoings;
        public LeafNode Outgoings
        {
            get
            {
                _outgoings.mv =
                    AgentsFee.mv + WearAndTear.mv + BuildingsInsurance +
                    AccountantsFee.mv +
                    (Option.Mortgage != null ? Option.Mortgage.interest.mv : 0) +
                    (Option.Mortgage != null ? Option.Mortgage.repayment.mv : 0);
                return _outgoings;
            }
            set
            {
                _outgoings = value;
            }

        }
        public float OriginalHousePrice { get; set; } = 100000;
        public float housePrice;
        public int OriginalPropertyCount { get; set; } = 1;
        public int PropertyCount { get; set; }
        public int BedroomsPerHouse { get; set; } = 2;
        private LeafNode _tenantCount;
        public LeafNode TenantCount {
            get
            {
                if (buyType == BuyType.toLet)
                    _tenantCount.mv = BedroomsPerHouse * PropertyCount;
                else
                    _tenantCount.mv = BedroomsPerHouse * PropertyCount - 1;
                return _tenantCount;
            }
            set
            {
                _tenantCount = value;
            }
        }
        public float SingleTenantsRent { get; set; } = 330;
        private LeafNode _tenantsRent;
        public LeafNode TenantsRent {
            get
            {
                _tenantsRent.mv = TenantCount.mv * SingleTenantsRent;
                return _tenantsRent;
            }
            set
            {
                _tenantsRent = value;
            }
        }
        public float moneyInvested;
        private LeafNode _agentsFee;
        public LeafNode AgentsFee
        {
            get
            {
                if (includeWearAndTear.Value)
                    _agentsFee.mv = TenantsRent.mv * 0.15f;
                else
                    _agentsFee.mv = 0;
                return _agentsFee;
            }
            set
            {
                _agentsFee = value;
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
        private LeafNode _wearAndTear;
        public LeafNode WearAndTear
        {
            get
            {
                if (includeWearAndTear.Value)
                    _wearAndTear.mv = PropertyCount * 90;
                else
                    _wearAndTear.mv = 0;
                return _wearAndTear;
            }
            set
            {
                _wearAndTear = value;
            }
        }
        public float BuildingsInsurance
        {
            get { return PropertyCount * 12; }
        }
        private LeafNode _accountantsFee;
        public LeafNode AccountantsFee
        {
            get
            {
                _accountantsFee.mv = 9 * PropertyCount; //9 pounds
                return _accountantsFee;
            }
            set { _accountantsFee = value; }
        }
        private LeafNode _profitAndSavings;
        public LeafNode ProfitAndSavings
        {
            get
            {
                if (Option.zeroInvestment)
                    _profitAndSavings.mv = Option.Mortgage.repayment.cumulativeValue *
                    Option.Mortgage.interestRate;
                else
                    _profitAndSavings.mv = TaxableProfit.mv - IncomeTax.mv + RentSavings.mv;
                return _profitAndSavings;
            }
            set { _profitAndSavings = value; }
        }
        private LeafNode _TaxableProfit;
        public LeafNode TaxableProfit
        {
            get
            {
                if (buyType == BuyType.toLiveIn && rentARoomScheme.Value)
                    _TaxableProfit.mv = TaxableTenantsRent.mv;
                else
                    _TaxableProfit.mv = TaxableTenantsRent.mv - 
                        (Outgoings.mv - (Option.Mortgage != null ? Option.Mortgage.repayment.mv : 0));
                //NOTE: Not sure taxableTenantsRent really needed... confuses things?
                //calculated here, because if use it, CANNOT claim back tax on your letting expenses!
                //As gen rule, if expenses are LESS than £4250, then rent room scheme is better for you.
                return _TaxableProfit;
            }
            set { _TaxableProfit = value; }
        }
        private LeafNode _ReturnOnInvestment;
        public LeafNode ReturnOnInvestment
        {
            get
            {
                _ReturnOnInvestment.mv = 12 * ProfitAndSavings.mv / moneyInvested * 100;
                return _ReturnOnInvestment;
            }
            set { _ReturnOnInvestment = value; }
        }
        private LeafNode _Costs;
        public LeafNode Costs
        {
            get {
                if (Option.zeroInvestment)
                    _Costs.mv = 0;
                else
                    _Costs.mv = Outgoings.mv - Option?.Mortgage?.repayment?.mv?? 0;
                return _Costs;
            }
            set { _Costs = value; }
        }
        private LeafNode _TaxableTenantsRent;
        public LeafNode TaxableTenantsRent
        {
            get
            {
                if (buyType == BuyType.toLiveIn && rentARoomScheme.Value)
                    _TaxableTenantsRent.mv = TenantsRent.mv - 4250 / 12;
                //if (taxableTenantsRent.mv < 0) taxableTenantsRent.mv = 0;
                //not nec... profit CAN be negative... Just shows, that losing money...
                //BUT, it is right include rent saving, as income!
                //BUT, they cannot TAX you for it, so change that...   
                else
                    _TaxableTenantsRent.mv = TenantsRent.mv;
                return _TaxableTenantsRent;
            }
            set { _TaxableTenantsRent = value; }
        }
        private LeafNode _CapitalGains;
        public LeafNode CapitalGains
        {
            get
            {
                _CapitalGains.mvs = housePrice * PercentageGrowth;
                _CapitalGains.mv = PropertyCount * housePrice * PercentageGrowth;
                return _CapitalGains;
            }
            set { _CapitalGains = value; }
        }
        private LeafNode _TaxableCapitalGains;
        public LeafNode TaxableCapitalGains
        {
            get
            {
                if (CapitalGains.mvs >= capitalGainsAllowance)
                {
                    _TaxableCapitalGains.mvs =
                        CapitalGains.mvs - capitalGainsAllowance / Option.Intervals;
                    _TaxableCapitalGains.mv =
                        PropertyCount * CapitalGains.mvs - capitalGainsAllowance / Option.Intervals;
                }
                else
                {
                    _TaxableCapitalGains.mvs = 0;
                    _TaxableCapitalGains.mv = PropertyCount * 0;
                }
                return _TaxableCapitalGains;
            }
            set { _TaxableCapitalGains = value; }
        }
        private LeafNode _BasicTaxableCapitalGains;
        public LeafNode BasicTaxableCapitalGains {
            get
            {
                //NOW remember, selling one house each year!!! THATS WHY got the mvs below...
                if (TaxableCapitalGains.mvs < BasicAllowableCapitalGains.mv)
                    _BasicTaxableCapitalGains.mv = TaxableCapitalGains.mvs;
                else
                    //it gets more complicated
                    //pay 28% on any amount above this...
                    _BasicTaxableCapitalGains.mv = BasicAllowableCapitalGains.mv;
                return _BasicTaxableCapitalGains;
            }
            set { _BasicTaxableCapitalGains = value; }
        }
        private LeafNode _HigherTaxableCapitalGains;
        public LeafNode HigherTaxableCapitalGains {
            get
            {
                if (TaxableCapitalGains.mvs < HigherAllowableCapitalGains)
                    //it gets more complicated
                    //pay 28% on any amount above this...
                    _HigherTaxableCapitalGains.mv = TaxableCapitalGains.mvs - BasicTaxableCapitalGains.mv;
                else
                    _HigherTaxableCapitalGains.mv = 0;
                return _HigherTaxableCapitalGains;
            }
            set { _HigherTaxableCapitalGains = value; }
        }
        public float capitalGainsAllowance = 11000;
        private LeafNode _BasicAllowableCapitalGains;
        public LeafNode BasicAllowableCapitalGains
        {
            get
            {
                _BasicAllowableCapitalGains.mv = (32010 - 12000) / Option.Intervals;
                return _BasicAllowableCapitalGains;
                //HAVE to divide by intervals, because this is split, over ALL these intervals.
                //(only sell ONCE).
                //32010 is the cut off point (yearly) for basic allowable CG.
                //12000 is, say my oldAge income, when I sell the house...
            }
            set { _BasicAllowableCapitalGains = value; }
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
        private LeafNode _CapitalGainsTax;
        public LeafNode CapitalGainsTax
        {
            get
            {
                if (buyType == BuyType.toLiveIn)
                {
                    _CapitalGainsTax.mvs =
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f;
                    _CapitalGainsTax.mv =
                        (PropertyCount - 1) *
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f;
                }
                else
                {
                    _CapitalGainsTax.mvs =
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f;
                    _CapitalGainsTax.mv =
                        PropertyCount *
                        BasicTaxableCapitalGains.mv * 0.18f
                        + HigherTaxableCapitalGains.mv * 0.28f;
                }
                return _CapitalGainsTax;
                //WHATTTT!!! BUT I made 30,000 in capitalGains...
                //AND allowable capGains is 20,000.
                //SO SURELY SOME of my taxableCapGains falls in to the HIGHER bracket???
                //AHHH but allowable CapGains is £11,000, SO allowable capGains is 20,000 + 11,000 = £31,000,
                //SO I am JUST ABOUT OK!!! only pay 18% on most of my capGains...    
            }
            set { _CapitalGainsTax = value; }
        }
        private LeafNode _CapitalGainsProfit;
        public LeafNode CapitalGainsProfit
        {
            get
            {
                _CapitalGainsProfit.mv = CapitalGains.mv - CapitalGainsTax.mv;
                return _CapitalGainsProfit;
            }
            set { _CapitalGainsProfit = value; }
        }
        private LeafNode _RentSavings;
        public LeafNode RentSavings
        {
            get
            {
                if (buyType == BuyType.toLiveIn)
                    _RentSavings.mv = Option.Shelter.typicalRent;
                else
                    _RentSavings.mv = 0;
                return _RentSavings;
            }
            set { _RentSavings = value; }
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
            //Cumulative Values
            CapitalGains.cumulativeValue += CapitalGains.mv;
            CapitalGainsProfit.cumulativeValue += CapitalGainsProfit.mv;
            BasicTaxableCapitalGains.cumulativeValue += BasicTaxableCapitalGains.mv;
            HigherTaxableCapitalGains.cumulativeValue += HigherTaxableCapitalGains.mv;
        }

    }
}
