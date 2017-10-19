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
            interestOnlyMortgage.RealWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyMortgage.RealWorldTree.property.buyType = Property.BuyType.toLet;
            Options.Add(interestOnlyMortgage);
            //Should just make a constructor that does this stuff!
            //saves writing...

            InvestmentOption interestOnlyLiveIn = new InvestmentOption("interestOnlyLiveIn", Form);
            interestOnlyLiveIn.RealWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLiveIn.RealWorldTree.property.buyType = Property.BuyType.toLiveIn;
            interestOnlyLiveIn.RealWorldTree.property.originalTenantCount = 1;
            Options.Add(interestOnlyLiveIn);

            InvestmentOption repaymentMortgage = new InvestmentOption("repaymentMortgage", Form);
            repaymentMortgage.RealWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            repaymentMortgage.RealWorldTree.property.buyType = Property.BuyType.toLet;
            Options.Add(repaymentMortgage);

            InvestmentOption buyToLiveMortgage = new InvestmentOption("buyToLiveMortgage", Form);
            buyToLiveMortgage.RealWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            buyToLiveMortgage.RealWorldTree.property.buyType = Property.BuyType.toLiveIn;
            buyToLiveMortgage.RealWorldTree.property.originalTenantCount = 1;
            Options.Add(buyToLiveMortgage);

            InvestmentOption interestOnlyLondon = new InvestmentOption("interestOnlyLondon", Form);
            interestOnlyLondon.RealWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLondon.RealWorldTree.property.buyType = Property.BuyType.toLet;
            interestOnlyLondon.RealWorldTree.property.location = Property.Location.London;
            Options.Add(interestOnlyLondon);

            InvestmentOption threeBedLiveIn = new InvestmentOption("threeBedLiveIn", Form);
            threeBedLiveIn.RealWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            threeBedLiveIn.RealWorldTree.property.buyType = Property.BuyType.toLiveIn;
            threeBedLiveIn.RealWorldTree.property.housePrice = 125000;
            Options.Add(threeBedLiveIn);

            InvestmentOption zeroInvestment = new InvestmentOption("zeroInvestment", Form);
            zeroInvestment.RealWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            zeroInvestment.RealWorldTree.property.buyType = Property.BuyType.toLiveIn;
            zeroInvestment.zeroInvestment = true;
            Options.Add(zeroInvestment);

            InvestmentOption hundredGrandFromStart = new InvestmentOption("hundredGrandFromStart", Form);
            hundredGrandFromStart.RealWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrandFromStart.RealWorldTree.property.buyType = Property.BuyType.toLet;
            hundredGrandFromStart.noMortgageNeeded = true;
            Options.Add(hundredGrandFromStart);

            InvestmentOption hundredGrand4houses = new InvestmentOption("hundredGrand4houses", Form);
            hundredGrand4houses.RealWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrand4houses.RealWorldTree.property.buyType = Property.BuyType.toLet;
            hundredGrand4houses.RealWorldTree.property.originalPropertyCount = 4;
            Options.Add(hundredGrand4houses);
        }

    }
}
