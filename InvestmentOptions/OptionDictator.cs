using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    class OptionDictator {

        ProjectionForm form = new ProjectionForm();
        //Can only be ONE form, because only 1 form can run at a time...
        List<InvestmentOption> options = new List<InvestmentOption>();

        public OptionDictator() {
            presentOptionCombination();
            //better, because can change it easily, just by calling a different method...
        }

        public void presentOptionCombination1() {
            //This is where lionShare of conducting is done!
            ProjectionForm form = new ProjectionForm();
            InvestmentOption statusQuo = new InvestmentOption(); //this is a hypothetical scenario/world.
            InvestmentOption flatNottingham = new InvestmentOption();
            flatNottingham.rent = 0;
            flatNottingham.mortgagePayment = 100;
            //therefore, it should have its own real world properties, like bankAccountContents, and years... 
            //(model scale).
            //form.presentInvestmentOptions();
        }

        public void presentOptionCombination() {
            InvestmentOption interestOnlyMortgage = new InvestmentOption();
            interestOnlyMortgage.Name = "interestOnlyMortgage";
            interestOnlyMortgage.mortgageType = InvestmentOption.MortgageType.interestOnly;
            interestOnlyMortgage.buyType = InvestmentOption.BuyType.toLet;
            options.Add(interestOnlyMortgage);
            //form.presentInvestmentOption(interestOnlyMortgage);

            InvestmentOption interestOnlyLiveIn = new InvestmentOption();
            interestOnlyLiveIn.Name = "interestOnlyLiveIn";
            interestOnlyLiveIn.mortgageType = InvestmentOption.MortgageType.interestOnly;
            interestOnlyLiveIn.buyType = InvestmentOption.BuyType.toLiveIn;
            interestOnlyLiveIn.tenantCount = 1;
            interestOnlyLiveIn.rent = 0;
            options.Add(interestOnlyLiveIn);

            InvestmentOption repaymentMortgage = new InvestmentOption();
            repaymentMortgage.Name = "repaymentMortgage";
            repaymentMortgage.mortgageType = InvestmentOption.MortgageType.repayment;
            repaymentMortgage.buyType = InvestmentOption.BuyType.toLet;
            options.Add(repaymentMortgage);
           
            InvestmentOption buyToLiveMortgage = new InvestmentOption();
            buyToLiveMortgage.Name = "buyToLiveMortgage";
            buyToLiveMortgage.mortgageType = InvestmentOption.MortgageType.repayment;
            buyToLiveMortgage.buyType = InvestmentOption.BuyType.toLiveIn;
            buyToLiveMortgage.tenantCount = 1;
            buyToLiveMortgage.rent = 0;
            options.Add(buyToLiveMortgage);

            InvestmentOption threeBedLiveIn = new InvestmentOption();
            threeBedLiveIn.Name = "threeBedLiveIn";
            threeBedLiveIn.mortgageType = InvestmentOption.MortgageType.repayment;
            threeBedLiveIn.buyType = InvestmentOption.BuyType.toLiveIn;
            threeBedLiveIn.accountantsFee = 0;
            threeBedLiveIn.rent = 0;
            threeBedLiveIn.mortgage.housePrice = 125000;
            options.Add(threeBedLiveIn);

            //form.presentInvestmentOptions(repaymentMortgage, buyToLiveMortgage);
            //form.presentInvestmentOptions(buyToLiveMortgage, threeBedBTLM);
            form.presentInvestmentOptions(options);
            Application.Run(form); 
            //seems like it does not continue with the rest of code until this is closed..
            //try putting it at the end...
        }

    }
}
