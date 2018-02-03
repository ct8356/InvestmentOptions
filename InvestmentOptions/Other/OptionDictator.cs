using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    class OptionDictator {

        public List<InvestmentOption> Options { get; set; }

        public OptionDictator(ProjectionForm form) {
            Options = new List<InvestmentOption>();
            CreateOptions(form);
        }

        public void CreateOptions(ProjectionForm Form) {
            InvestmentOption interestOnlyLet = new InvestmentOption("interestOnlyLet", Form);
            interestOnlyLet.Mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLet.Property.buyType = Property.BuyType.toLet;
            Options.Add(interestOnlyLet);
            //Should just make a constructor that does this stuff!
            //saves writing.

            InvestmentOption garages = new InvestmentOption("garages", Form);
            garages.Mortgage.type = Mortgage.BuyType.interestOnly;
            garages.Mortgage.SingleDeposit = 5000;
            garages.Property.buyType = Property.BuyType.toLet;
            garages.Property.OriginalPropertyCount = 5;
            garages.Property.OriginalHousePrice = 20000;
            garages.Property.BedroomsPerHouse = 1;
            garages.Property.SingleTenantsRent = 80;

            Options.Add(garages);

            InvestmentOption interestOnlyLiveIn = new InvestmentOption("interestOnlyLiveIn", Form);
            interestOnlyLiveIn.Mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLiveIn.Property.buyType = Property.BuyType.toLiveIn;
            Options.Add(interestOnlyLiveIn);

            InvestmentOption repaymentLet = new InvestmentOption("repaymentLet", Form);
            repaymentLet.Mortgage.type = Mortgage.BuyType.repayment;
            repaymentLet.Property.buyType = Property.BuyType.toLet;
            Options.Add(repaymentLet);

            InvestmentOption repaymentLive = new InvestmentOption("repaymentLive", Form);
            repaymentLive.Mortgage.type = Mortgage.BuyType.repayment;
            repaymentLive.Property.buyType = Property.BuyType.toLiveIn;
            Options.Add(repaymentLive);

            InvestmentOption interestOnlyLetLondon = new InvestmentOption("interestOnlyLetLondon", Form);
            interestOnlyLetLondon.Mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLetLondon.Property.buyType = Property.BuyType.toLet;
            interestOnlyLetLondon.Property.location = Property.Location.London;
            Options.Add(interestOnlyLetLondon);

            InvestmentOption repaymentLive3Bed = new InvestmentOption("repaymentLive3Bed", Form);
            repaymentLive3Bed.Mortgage.type = Mortgage.BuyType.repayment;
            repaymentLive3Bed.Property.buyType = Property.BuyType.toLiveIn;
            repaymentLive3Bed.Property.OriginalHousePrice = 125000;
            Options.Add(repaymentLive3Bed);

            InvestmentOption zeroInvestment = new InvestmentOption("zeroInvestment", Form);
            zeroInvestment.Mortgage.type = Mortgage.BuyType.repayment;
            zeroInvestment.Property.buyType = Property.BuyType.toLiveIn;
            zeroInvestment.Property.OriginalPropertyCount = 0;
            zeroInvestment.zeroInvestment = true; //TODO Want to get rid of this!
            Options.Add(zeroInvestment);

            InvestmentOption hundredGrandFromStart = new InvestmentOption("hundredGrandFromStart", Form);
            hundredGrandFromStart.Mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrandFromStart.Property.buyType = Property.BuyType.toLet;
            hundredGrandFromStart.noMortgageNeeded = true;
            Options.Add(hundredGrandFromStart);

            InvestmentOption hundredGrand4houses = new InvestmentOption("hundredGrand4houses", Form);
            hundredGrand4houses.Mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrand4houses.Property.buyType = Property.BuyType.toLet;
            hundredGrand4houses.Property.OriginalPropertyCount = 4;
            Options.Add(hundredGrand4houses);

        }

    }
}
