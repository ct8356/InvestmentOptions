using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    class OptionDictator {

        ProjectionForm form = new ProjectionForm(); 
        //Can only be ONE form, because only 1 form can run at a time...

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
            form.presentInvestmentOptions(flatNottingham, flatNottingham);
        }

        public void presentOptionCombination() {
            InvestmentOption interestOnlyMortgage = new InvestmentOption();
            interestOnlyMortgage.name = "interestOnlyMortgage";
            interestOnlyMortgage.mortgageType = 1;
            interestOnlyMortgage.propertyType = 1;
            //form.presentInvestmentOption(interestOnlyMortgage);

            InvestmentOption repaymentMortgage = new InvestmentOption();
            repaymentMortgage.name = "repaymentMortgage";
            repaymentMortgage.mortgageType = 0;
            repaymentMortgage.propertyType = 1;
           
            InvestmentOption buyToLiveMortgage = new InvestmentOption();
            buyToLiveMortgage.name = "buyToLiveMortgage";
            buyToLiveMortgage.mortgageType = 0;
            interestOnlyMortgage.propertyType = 0;
            buyToLiveMortgage.tenantsRent = 350;
            buyToLiveMortgage.agentsFee = buyToLiveMortgage.agentsFee/2;
            buyToLiveMortgage.wearAndTear = buyToLiveMortgage.wearAndTear;
            buyToLiveMortgage.accountantsFee = 0;
            buyToLiveMortgage.rent = 0;
            //graph.presentInvestmentOption(buyToLiveMortgage);

            form.presentInvestmentOptions(repaymentMortgage, buyToLiveMortgage);
            Application.Run(form); 
            //seems like it does not continue with the rest of code until this is closed..
            //try putting it at the end...
        }

    }
}
