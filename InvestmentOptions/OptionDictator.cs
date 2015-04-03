using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    class OptionDictator {
        ProjectionForm form; //Can only be ONE form, because only 1 form can run at a time...
        public List<InvestmentOption> options = new List<InvestmentOption>();

        public OptionDictator() {
            form = new ProjectionForm();
            createOptionCombination();
            form.doStuffWithOptionsList(options);
            //better, because can change it easily, just by calling a different method...
            Application.Run(form);
            //seems like it does not continue with the rest of code until this is closed..
            //try putting it at the end...
        }

        public void createOptionCombination() {
            InvestmentOption interestOnlyMortgage = new InvestmentOption("interestOnlyMortgage", form);
            interestOnlyMortgage.realWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyMortgage.realWorldTree.property.buyType = Property.BuyType.toLet;
            options.Add(interestOnlyMortgage);
            //NOTE, should just make a constructor that does this stuff!
            //saves writing...

            InvestmentOption interestOnlyLiveIn = new InvestmentOption("interestOnlyLiveIn", form);
            interestOnlyLiveIn.realWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLiveIn.realWorldTree.property.buyType = Property.BuyType.toLiveIn;
            interestOnlyLiveIn.realWorldTree.property.originalTenantCount = 1;
            options.Add(interestOnlyLiveIn);

            InvestmentOption repaymentMortgage = new InvestmentOption("repaymentMortgage", form);
            repaymentMortgage.realWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            repaymentMortgage.realWorldTree.property.buyType = Property.BuyType.toLet;
            options.Add(repaymentMortgage);

            InvestmentOption buyToLiveMortgage = new InvestmentOption("buyToLiveMortgage", form);
            buyToLiveMortgage.realWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            buyToLiveMortgage.realWorldTree.property.buyType = Property.BuyType.toLiveIn;
            buyToLiveMortgage.realWorldTree.property.originalTenantCount = 1;
            options.Add(buyToLiveMortgage);

            InvestmentOption interestOnlyLondon = new InvestmentOption("interestOnlyLondon", form);
            interestOnlyLondon.realWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            interestOnlyLondon.realWorldTree.property.buyType = Property.BuyType.toLet;
            interestOnlyLondon.realWorldTree.property.location = Property.Location.London;
            options.Add(interestOnlyLondon);

            InvestmentOption threeBedLiveIn = new InvestmentOption("threeBedLiveIn", form);
            threeBedLiveIn.realWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            threeBedLiveIn.realWorldTree.property.buyType = Property.BuyType.toLiveIn;
            threeBedLiveIn.realWorldTree.mortgage.housePrice = 125000;
            options.Add(threeBedLiveIn);

            InvestmentOption zeroInvestment = new InvestmentOption("zeroInvestment", form);
            zeroInvestment.realWorldTree.mortgage.type = Mortgage.BuyType.repayment;
            zeroInvestment.realWorldTree.property.buyType = Property.BuyType.toLiveIn;
            zeroInvestment.zeroInvestment = true;
            options.Add(zeroInvestment);

            InvestmentOption hundredGrandFromStart = new InvestmentOption("hundredGrandFromStart", form);
            hundredGrandFromStart.realWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrandFromStart.realWorldTree.property.buyType = Property.BuyType.toLet;
            hundredGrandFromStart.noMortgageNeeded = true;
            options.Add(hundredGrandFromStart);

            InvestmentOption hundredGrand4houses = new InvestmentOption("hundredGrand4houses", form);
            hundredGrand4houses.realWorldTree.mortgage.type = Mortgage.BuyType.interestOnly;
            hundredGrand4houses.realWorldTree.property.buyType = Property.BuyType.toLet;
            hundredGrand4houses.realWorldTree.property.originalPropertyCount = 4;
            options.Add(hundredGrand4houses);
        }

    }
}
