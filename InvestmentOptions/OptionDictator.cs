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
            flatNottingham.mortgage.payment.monthlyValue = 100;
            //therefore, it should have its own real world properties, like bankAccountContents, and years... 
            //(model scale).
            //form.presentInvestmentOptions();
        }

        public void presentOptionCombination() {
            InvestmentOption interestOnlyMortgage = new InvestmentOption();
            interestOnlyMortgage.Name = "interestOnlyMortgage";
            interestOnlyMortgage.mortgage.type = Mortgage.Type.interestOnly;
            interestOnlyMortgage.property.buyType = Property.BuyType.toLet;
            options.Add(interestOnlyMortgage);
            //form.presentInvestmentOption(interestOnlyMortgage);
            //NOTE, should just make a constructor that does this stuff!
            //saves writing...

            InvestmentOption interestOnlyLiveIn = new InvestmentOption();
            interestOnlyLiveIn.Name = "interestOnlyLiveIn";
            interestOnlyLiveIn.mortgage.type = Mortgage.Type.interestOnly;
            interestOnlyLiveIn.property.buyType = Property.BuyType.toLiveIn;
            interestOnlyLiveIn.property.originalTenantCount = 1;
            options.Add(interestOnlyLiveIn);

            InvestmentOption repaymentMortgage = new InvestmentOption();
            repaymentMortgage.Name = "repaymentMortgage";
            repaymentMortgage.mortgage.type = Mortgage.Type.repayment;
            repaymentMortgage.property.buyType = Property.BuyType.toLet;
            options.Add(repaymentMortgage);
           
            InvestmentOption buyToLiveMortgage = new InvestmentOption();
            buyToLiveMortgage.Name = "buyToLiveMortgage";
            buyToLiveMortgage.mortgage.type = Mortgage.Type.repayment;
            buyToLiveMortgage.property.buyType = Property.BuyType.toLiveIn;
            buyToLiveMortgage.property.originalTenantCount = 1;
            options.Add(buyToLiveMortgage);

            InvestmentOption threeBedLiveIn = new InvestmentOption();
            threeBedLiveIn.Name = "threeBedLiveIn";
            threeBedLiveIn.mortgage.type = Mortgage.Type.repayment;
            threeBedLiveIn.property.buyType = Property.BuyType.toLiveIn;
            threeBedLiveIn.property.accountantsFee.monthlyValue = 0;
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
