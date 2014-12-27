using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {

    public class InvestmentOption {
        //This is the class to change, if you want to change the investment option!
        //Well actually, eventually want to do that from option dictator...
        //But if want to make it more sophisticated, start here (and inside its properties)...
        public String name;
        public int years = 25;
        public int intervals; //(interval is one month)...
        public float[] bankAccountProjection;
        public BankAccount bankAccount = new BankAccount();
        //Note, I would say, don't split things into new classes, unless you really have to.
        //InvestmentOptions is PROBABLY as small as you need to go, for your purposes...
        //Do NOT want bankAccount to have outgoings inside it. That is a characteristic of 
        //investment options... I think...
        // but if it gets complicated, put stuff in BankAccount. So leave it there...
        public float[] netWorthProjection; //Floats and ints fine into millions. Be careful when reach billions.
        public float netWorth;
        //INGOINGS
        public float ingoings;
        public float jobIncome = 1500;
        public float jobIngoings;
        public float bankInterest;
        public float bankInterestRate = 0.000f; //0, since bIR only covers inflation, not accounted for here..
        public float tenantsRent = 700; //Sure to always be fairly HIGH amount, coz would buy in SouthWest...
        //OUTGOINGS
        public float outgoings;
        public float houseCosts;
        public float rent = 350; //Could actually be less. But nah. I should have bit of luxury...
        public float houseBills = 45;
        public float livingCosts;
        public float charity = 45;
        public float propertyIncomeTax;
        public float councilTax = 100;
        public float studentLoanPayment;
        public float mortgagePayment = 0;
        public float propertyCosts;
        //LIVING COSTS
        public float phoneBill = 10;
        public float food = 140;
        public float randomObjects = 120;
        public float danceLessons = 18;
        public float randomEventsAndServices = 120;
        //MORTGAGE PAYMENTS
        public float housePrice = 100000;
        public float deposit = 25000;
        public float moneyBorrowed;
        public float mortgageRepayment;
        public float moneyOwed;
        public float mortgageInterestRate = 0.065f/12; //apparently, 6.5% annual is NOT a cumulative rate...
            //0.00408f; //monthly interest rate (number works well for 5% annual).
        //try 487 for 6% annual.
        public float mortgageInterest;
        public int mortgageType;
        //INCOME TAX
        public float incomeTaxRate = 0.20f;
        //PROPERTY MAINTENANCE
        public float agentsFee = 100; //Not really worth it! That's 10 hours a month!
        //Could do self in less, easy!
        //Or pay Dad to do it!
        public float wearAndTear = 90;
        public float buildingsInsurance = 12;
        public float accountantsFee = 9;
        //NOTE: If wanted to get rid of all the floats above, could turn some of them into methods...
        //SWITCHES
        public int propertyType; //buyToLive or buyToLet
        //INDICES
        //These perhaps should be stored somewhere else, but for now, just store them here...
        public Dictionary<String, float> indexDictionary = new Dictionary<String, float>();
        //Dictionary so much better than list of your own KeyValuePairs, because can
        //search the dictionary by key, whereas can only search List by numbers!
        //Although, actually, that does not matter, because would just print ALL of list anyway...
        //SO, not THAT much better, but does mean I don't need all these cumIngoingsI things!
        //Well, actually, would not need that either. Just make them anonymous, by instantiating in fill method.
        //SO, not much better, BUT means don't need to instantiate each one... Tiny bit tidier...
        public float propertyProfit = 0;
        public float cumIngoings = 0;
        public float cumJobIngoings = 0;
        public float cumPropertyIngoings = 0;
        public float cumOutgoings = 0;
        public float cumHouseCosts = 0;
        public float cumLivingCosts = 0;
        public float cumMortageRepayments = 0;

        public float cumPropertyCosts = 0;
        public float cumAgentsFee = 0;
        public float cumWearAndTear = 0;

        public float cumMortgageInterest = 0;
        public float cumStudentLoanRepayments = 0;
        public float cumPropertyIncomeTax = 0;

        public void fillDictionary() {
            indexDictionary.Add("cumIngoings", cumIngoings);
            indexDictionary.Add("cumJobIngoings", cumJobIngoings);
            indexDictionary.Add("cumPropertyIngoings", cumPropertyIngoings); //after removing income tax...
            indexDictionary.Add("cumOutgoings", cumOutgoings);
            indexDictionary.Add("cumHouseCosts", cumHouseCosts);
            indexDictionary.Add("cumLivingCosts", cumLivingCosts);
            indexDictionary.Add("cumMortageRepayments", cumMortageRepayments);
            indexDictionary.Add("cumPropertyCosts", cumPropertyCosts);
            indexDictionary.Add("cumAgentsFee", cumAgentsFee);
            indexDictionary.Add("cumMortgageInterest", cumMortgageInterest);
            indexDictionary.Add("cumWearAndTear", cumWearAndTear);
            indexDictionary.Add("cumStudentLoanRepayments", cumStudentLoanRepayments);
            indexDictionary.Add("cumIncomeTax", cumPropertyIncomeTax);
        }

        public InvestmentOption() {
            //Initial setup:
            intervals = years * 12;
            moneyBorrowed = housePrice - deposit;
            moneyOwed = moneyBorrowed;
            netWorth = bankAccount.contents;
            calculateStudentLoanPayment();
            jobIngoings = jobIncome - studentLoanPayment;
        }

        public void calculatePropertyCosts() {
            propertyCosts = agentsFee + wearAndTear + buildingsInsurance + accountantsFee +
                mortgageInterest + mortgageRepayment;
        }

        public void calculatePropertyIncomeTax() {
            calculatePropertyCosts();
            propertyProfit = tenantsRent - (propertyCosts - mortgageRepayment); 
            //Note, not quite right, if exceeds rent a room scheme limit!
            if (propertyProfit < 0) propertyProfit = 0;
            propertyIncomeTax = propertyProfit * 0.20f; 
            switch (propertyType) {
                case 0: //Buy to live in
                    if (tenantsRent <= 4250) propertyIncomeTax = 0; //Rent a room scheme...
                    break;
                case 1: //Buy to let
                    break;
            }
        }

        public void calculateIngoings() {
            //just have to make sure all equations are well defined here...
            bankInterest = bankAccount.contents * bankInterestRate; //Monthly interest...
            calculatePropertyIncomeTax();
            ingoings = jobIngoings + bankInterest + tenantsRent - propertyIncomeTax;
        }

        public void calculateMortgagePayments(int interval) {
            switch (mortgageType) {
                case 0: //repaymentMortgage.
                    mortgagePayment = moneyBorrowed * mortgageInterestRate * 
                        ((float) Math.Pow(1 + mortgageInterestRate, intervals)) / 
                        ((float) Math.Pow(1 + mortgageInterestRate,intervals) - 1); //How is this figure obtained? trial and error?
                    //mortgagePayment = 1100;
                    //Apparently, can use this equation:
                    //P = L*c*[(1 + c)^n]/[(1 + c)^n - 1]
                    mortgageInterest = moneyOwed * mortgageInterestRate; 
                    //this done after or before money Owed and mortgage repayment calculated?
                    //Ahah, if this calc is done first, then all works out easy!
                    mortgageRepayment = mortgagePayment -  mortgageInterest; 
                    //How is this decided? with some kind of simultaneous eq'n... or numerical method?
                    //note, if simultaneous eq'ns used, could just find the final one on web!
                    //as long as average comes out at £285, its ok).
                    moneyOwed = moneyOwed - mortgageRepayment;
                    //Console.WriteLine("" + interest);
                    break;
                case 1: //interestOnlyMortgage
                    mortgageRepayment = 0;
                    //moneyOwed = moneyOwed;
                    mortgageInterest = moneyOwed * mortgageInterestRate;
                    mortgagePayment = mortgageInterest;
                    break;
            }
        }

        public void calculateOutgoings() {
            livingCosts = food + phoneBill + danceLessons + randomObjects +
                randomEventsAndServices + charity;
            calculatePropertyIncomeTax();
            houseCosts = rent + houseBills + councilTax;
            outgoings = houseCosts + livingCosts + propertyCosts;
        }

        public void calculateStudentLoanPayment() {
            studentLoanPayment = jobIncome * 0.05f;
        }

        public void makeProjection() {
            bankAccountProjection = new float[intervals];
            netWorthProjection = new float[intervals];
            for (int interval = 0; interval < intervals; interval++) {
                //calculations:
                calculateMortgagePayments(interval);
                calculateIngoings();
                calculateOutgoings();
                bankAccount.contents = bankAccount.contents + ingoings - outgoings;
                netWorth = netWorth + ingoings - outgoings + mortgageRepayment;
                //STORE IN ARRAYS:
                bankAccountProjection[interval] = bankAccount.contents;
                netWorthProjection[interval] = netWorth;
                //STORE TOTALS:
                cumPropertyIncomeTax += propertyIncomeTax;
                cumHouseCosts += houseCosts;
                cumJobIngoings += jobIngoings;
                cumIngoings += ingoings;
                cumOutgoings += outgoings;
                cumAgentsFee += agentsFee;
                cumLivingCosts += livingCosts;
                cumMortageRepayments += mortgageRepayment;
                cumPropertyCosts += propertyCosts;
                cumPropertyIngoings += tenantsRent - propertyIncomeTax;
                cumStudentLoanRepayments += studentLoanPayment;
                cumMortgageInterest += mortgageInterest;
                cumWearAndTear += wearAndTear;
                //REPEATS
                //writeLine("mP", mortgagePayment);
                //writeLine("In", ingoings);
                //writeLine("", jobIngoings);
                //writeLine("", tenantsRent);
            }
            fillDictionary();
        }

        public void writeLine(String name, float value) {
            Console.WriteLine(name + ": " + String.Format("{0:n}", value));
        }
    }
}
