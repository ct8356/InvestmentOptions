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
            interestOnlyMortgage.mortgageType = 1;
            interestOnlyMortgage.propertyType = 1;
            options.Add(interestOnlyMortgage);
            //form.presentInvestmentOption(interestOnlyMortgage);

            InvestmentOption repaymentMortgage = new InvestmentOption();
            repaymentMortgage.Name = "repaymentMortgage";
            repaymentMortgage.mortgageType = 0;
            repaymentMortgage.propertyType = 1;
            options.Add(repaymentMortgage);
           
            InvestmentOption buyToLiveMortgage = new InvestmentOption();
            buyToLiveMortgage.Name = "buyToLiveMortgage";
            buyToLiveMortgage.mortgageType = 0;
            buyToLiveMortgage.propertyType = 0;
            buyToLiveMortgage.tenantsRent = buyToLiveMortgage.tenantsRent/2;
            buyToLiveMortgage.agentsFee = buyToLiveMortgage.agentsFee/2;
            buyToLiveMortgage.wearAndTear = buyToLiveMortgage.wearAndTear;
            buyToLiveMortgage.accountantsFee = 0;
            buyToLiveMortgage.rent = 0;
            options.Add(buyToLiveMortgage);

            InvestmentOption threeBedBTLM = new InvestmentOption();
            threeBedBTLM.Name = "threeBedBTLM";
            threeBedBTLM.mortgageType = 0;
            threeBedBTLM.propertyType = 0;
            threeBedBTLM.tenantsRent = threeBedBTLM.tenantsRent;
            threeBedBTLM.agentsFee = threeBedBTLM.agentsFee;
            threeBedBTLM.wearAndTear = threeBedBTLM.wearAndTear;
            threeBedBTLM.accountantsFee = 0;
            threeBedBTLM.rent = 0;
            threeBedBTLM.housePrice = 140000;
            options.Add(threeBedBTLM);

            //form.presentInvestmentOptions(repaymentMortgage, buyToLiveMortgage);
            //form.presentInvestmentOptions(buyToLiveMortgage, threeBedBTLM);
            form.presentInvestmentOptions(options);
            Application.Run(form); 
            //seems like it does not continue with the rest of code until this is closed..
            //try putting it at the end...
        }

    }
}
