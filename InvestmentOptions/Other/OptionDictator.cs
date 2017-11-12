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
            InvestmentOption interestOnlyMortgage = new InvestmentOption("interestOnlyMortgage", Form);
            interestOnlyMortgage.Mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyMortgage.Property.buyType = Property.BuyType.toLet;
            Options.Add(interestOnlyMortgage);
            //Should just make a constructor that does this stuff!
            //saves writing...

            InvestmentOption interestOnlyLiveIn = new InvestmentOption("interestOnlyLiveIn", Form);
            interestOnlyLiveIn.Mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLiveIn.Property.buyType = Property.BuyType.toLiveIn;
            Options.Add(interestOnlyLiveIn);

            InvestmentOption repaymentMortgage = new InvestmentOption("repaymentMortgage", Form);
            repaymentMortgage.Mortgage.type = Mortgage.BuyType.repayment;
            repaymentMortgage.Property.buyType = Property.BuyType.toLet;
            Options.Add(repaymentMortgage);

            InvestmentOption buyToLiveMortgage = new InvestmentOption("buyToLiveMortgage", Form);
            buyToLiveMortgage.Mortgage.type = Mortgage.BuyType.repayment;
            buyToLiveMortgage.Property.buyType = Property.BuyType.toLiveIn;
            Options.Add(buyToLiveMortgage);

            InvestmentOption interestOnlyLondon = new InvestmentOption("interestOnlyLondon", Form);
            interestOnlyLondon.Mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLondon.Property.buyType = Property.BuyType.toLet;
            interestOnlyLondon.Property.location = Property.Location.London;
            Options.Add(interestOnlyLondon);

            InvestmentOption threeBedLiveIn = new InvestmentOption("threeBedLiveIn", Form);
            threeBedLiveIn.Mortgage.type = Mortgage.BuyType.repayment;
            threeBedLiveIn.Property.buyType = Property.BuyType.toLiveIn;
            threeBedLiveIn.Property.housePrice = 125000;
            Options.Add(threeBedLiveIn);

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

            InvestmentOption garages = new InvestmentOption("garages", Form);
            garages.Mortgage.type = Mortgage.BuyType.interestOnly;
            garages.Property.buyType = Property.BuyType.toLet;
            garages.Property.OriginalHousePrice = 20000;
            garages.Property.OriginalPropertyCount = 2;
            Options.Add(garages);
        }

    }
}
