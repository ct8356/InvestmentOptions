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
            interestOnlyMortgage.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyMortgage.property.buyType = Property.BuyType.toLet;
            Options.Add(interestOnlyMortgage);
            //Should just make a constructor that does this stuff!
            //saves writing...

            InvestmentOption interestOnlyLiveIn = new InvestmentOption("interestOnlyLiveIn", Form);
            interestOnlyLiveIn.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLiveIn.property.buyType = Property.BuyType.toLiveIn;
            interestOnlyLiveIn.property.originalTenantCount = 1;
            Options.Add(interestOnlyLiveIn);

            InvestmentOption repaymentMortgage = new InvestmentOption("repaymentMortgage", Form);
            repaymentMortgage.mortgage.type = Mortgage.BuyType.repayment;
            repaymentMortgage.property.buyType = Property.BuyType.toLet;
            Options.Add(repaymentMortgage);

            InvestmentOption buyToLiveMortgage = new InvestmentOption("buyToLiveMortgage", Form);
            buyToLiveMortgage.mortgage.type = Mortgage.BuyType.repayment;
            buyToLiveMortgage.property.buyType = Property.BuyType.toLiveIn;
            buyToLiveMortgage.property.originalTenantCount = 1;
            Options.Add(buyToLiveMortgage);

            InvestmentOption interestOnlyLondon = new InvestmentOption("interestOnlyLondon", Form);
            interestOnlyLondon.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLondon.property.buyType = Property.BuyType.toLet;
            interestOnlyLondon.property.location = Property.Location.London;
            Options.Add(interestOnlyLondon);

            InvestmentOption threeBedLiveIn = new InvestmentOption("threeBedLiveIn", Form);
            threeBedLiveIn.mortgage.type = Mortgage.BuyType.repayment;
            threeBedLiveIn.property.buyType = Property.BuyType.toLiveIn;
            threeBedLiveIn.property.housePrice = 125000;
            Options.Add(threeBedLiveIn);

            InvestmentOption zeroInvestment = new InvestmentOption("zeroInvestment", Form);
            zeroInvestment.mortgage.type = Mortgage.BuyType.repayment;
            zeroInvestment.property.buyType = Property.BuyType.toLiveIn;
            zeroInvestment.zeroInvestment = true;
            Options.Add(zeroInvestment);

            InvestmentOption hundredGrandFromStart = new InvestmentOption("hundredGrandFromStart", Form);
            hundredGrandFromStart.mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrandFromStart.property.buyType = Property.BuyType.toLet;
            hundredGrandFromStart.noMortgageNeeded = true;
            Options.Add(hundredGrandFromStart);

            InvestmentOption hundredGrand4houses = new InvestmentOption("hundredGrand4houses", Form);
            hundredGrand4houses.mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrand4houses.property.buyType = Property.BuyType.toLet;
            hundredGrand4houses.property.originalPropertyCount = 4;
            Options.Add(hundredGrand4houses);
        }

    }
}
