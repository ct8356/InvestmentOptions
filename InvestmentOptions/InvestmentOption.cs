using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {

    public class InvestmentOption {
        //This is the class to change, if you want to change the investment option!
        //Well actually, eventually want to do that from option dictator...
        //But if want to make it more sophisticated, start here (and inside its properties)...
        public String Name;
        public int years = 10;
        public int intervals; //(interval is one month)...
        public BankAccount bankAccount = new BankAccount();
        //Note, I would say, don't split things into new classes, unless you really have to.
        //InvestmentOptions is PROBABLY as small as you need to go, for your purposes...
        //Do NOT want bankAccount to have outgoings inside it. That is a characteristic of 
        //investment options... I think...
        // but if it gets complicated, put stuff in BankAccount. So leave it there...
        //Floats and ints fine into millions. Be careful when reach billions.
        public float netWorth;
        public TreeView treeView = new TreeView();
        public List<Node> nodeList = new List<Node>();

        //INGOINGS
        public float ingoings;
        public float jobIncome = 1500;
        public float jobIngoings;
        public float bankInterest;
        public float bankInterestRate = 0.000f; //0, since bIR only covers inflation, not accounted for here..
        public float tenantCount;
        public float oneTenantsRent = 330;
        public float tenantsRent; //Sure to always be fairly HIGH amount, coz would buy in SouthWest...
        public float taxableTenantsRent;
        //OUTGOINGS
        public float outgoings;
        public float houseCosts;
        public float rent = 330; //Could actually be less. But nah. I should have bit of luxury...
        public float houseBills = 45;
        public float livingCosts;
        public float charity = 45;
        public float propertyIncomeTax;
        public float councilTax = 100;
        public float studentLoanPayment;
        public float mortgagePayment = 0;
        public float propertyOutgoings;
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
        //15%. 10% for finding tenant, 5% for all else...
        //BUT hey, 10% is fine, since cost of not finding tenant for one month is 8%!
        //OR could find a nice student/smart kid, and employ him to do it! Pay him very well!
        public float agentsFee; //Not really worth it! That's 8-10 hours a month! That is easy. (or is it?)
        //No, not easy, BUT won't take that much time in reality (or will it? per tenant?).
        //Unlikely, but what can you do. I am really constrained! Sure, if want to, can manage one room self,
        //and see how it goes...
        //Could do self in less, easy!
        //Or pay Dad to do it!
        public float wearAndTear;
        public float buildingsInsurance;
        public float accountantsFee;
        //NOTE: If wanted to get rid of all the floats above, could turn some of them into methods...
        //SWITCHES
        public int buyType; //buyToLive or buyToLet
        //INDICES
        //These perhaps should be stored somewhere else, but for now, just store them here...
        public float propertyProfit;
        public Dictionary<String, float> indexDictionary = new Dictionary<String, float>();
        //Dictionary so much better than list of your own KeyValuePairs, because can
        //search the dictionary by key, whereas can only search List by numbers!
        //Although, actually, that does not matter, because would just print ALL of list anyway...
        //SO, not THAT much better, but does mean I don't need all these cumIngoingsI things!
        //Well, actually, would not need that either. Just make them anonymous, by instantiating in fill method.
        //SO, not much better, BUT means don't need to instantiate each one... Tiny bit tidier...
        public Node cumIngoings = new Node() {Name = "cumIngoings"};
            public Node cumJobIngoings = new Node() {Name = "cumJobIngoings"};
            public Node cumPropertyIngoings = new Node() { Name = "cumPropertyIngoings" };
        public Node cumOutgoings = new Node() { Name = "cumOutgoings"};
            public Node cumShelterOutgoings = new Node() { Name = "cumShelterOutgoings" };
            public Node cumLivingOutgoings = new Node() { Name = "cumLivingOutgoings" };
            public Node cumPropertyOutgoings = new Node() { Name = "cumPropertyOutgoings" };
                public Node cumMortgageInterest = new Node() { Name = "cumMortgageInterest" };
                public Node cumAgentsFee = new Node() { Name = "cumAgentsFee" };
                public Node cumWearAndTear = new Node() { Name = "cumWearAndTear" };
                public Node cumMortageRepayments = new Node() { Name = "cumMortageRepayments" };
                public Node cumAccountantsFee = new Node() { Name = "cumAccountantsFee" };
        public Node cumPropertyIncomeTax = new Node() { Name = "cumPropertyIncomeTax" };
        public Node cumStudentLoanRepayments = new Node() { Name = "cumStudentLoanRepayments" };
        public Node mortgagePaymentNode = new Node() { Name = "mortgagePayment" };
        //OTHER NODES
        public Node bankAccountN; 
        public Node netWorthN; 
        public Node propertyProfitN;
        public Node tenantsRentN;
        public Node taxableTenantsRentN;
        //interesting alternative to above, might be one class, with a load of nested-classes...
        //BUT I think the above is best way... want the ONLY relationship defined, to be defined by the TREE.

        public void fillTree() {
            treeView.Nodes.Add(cumIngoings);
                cumIngoings.Nodes.Add(cumJobIngoings);
                cumIngoings.Nodes.Add(cumPropertyIngoings);
            treeView.Nodes.Add(cumOutgoings);
                cumOutgoings.Nodes.Add(cumShelterOutgoings);
                cumOutgoings.Nodes.Add(cumLivingOutgoings);
                cumOutgoings.Nodes.Add(cumPropertyOutgoings);
                    cumPropertyOutgoings.Nodes.Add(cumMortgageInterest);
                    cumPropertyOutgoings.Nodes.Add(cumAgentsFee);
                    cumPropertyOutgoings.Nodes.Add(cumWearAndTear);
                    cumPropertyOutgoings.Nodes.Add(cumMortageRepayments);
                    cumPropertyOutgoings.Nodes.Add(cumAccountantsFee);
            treeView.Nodes.Add(cumPropertyIncomeTax);
            treeView.Nodes.Add(cumStudentLoanRepayments);
            treeView.Nodes.Add(mortgagePaymentNode);
            //labelTree(treeView.Nodes); // ahah, NOW, fill tree mean Initialise tree!
        }

        public void labelTree(TreeNodeCollection nodes) {
            foreach (Node node in nodes) {
                node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                labelTree(node.Nodes);
            }
        }

        public void resetTree(TreeNodeCollection nodes) {
            foreach (Node node in nodes) {
                node.cumulativeValue = 0;
                resetTree(node.Nodes);
            }
        }

        public InvestmentOption() {
            fillTree(); //unfortunately, fillTree has to come at end of make projection.
            //but there are initialisation things in fill tree. Need to separate them. 
            //create an intialiseTree method...
            //Or a bind to tree method...
            //OR use the tree itself as main variables.
            //OR bind the tree to these variables...
            //OR any of above, just try one... then will see strengths and weaknesses.
            //FORTUNATELY, labelTree can come at end of make projection!
            createNodeList();
        }

        public void initialSetup() {
            //Initial setup:
            intervals = years * 12;
            moneyBorrowed = housePrice - deposit;
            moneyOwed = moneyBorrowed;         
            calculateStudentLoanPayment();
            jobIngoings = jobIncome - studentLoanPayment;
            bankAccount.contents = 17000;
            netWorth = bankAccount.contents;
            tenantCount = 2;
            updateMultiples();
        }

        public void autoInvest() {
            //check
            if (bankAccount.contents > deposit) {
                //invest
                moneyBorrowed += housePrice - deposit; // not that important.
                moneyOwed += housePrice - deposit;
                bankAccount.contents -= deposit;
                //Benefits
                tenantCount += 2;
                updateMultiples();
            } //something you forgetting to update. what is it? YES the extra features that make you money!
        }

        public void createNodeList() {
            bankAccountN = new Node(intervals) { Name = "bankAccount" };
            netWorthN = new Node(intervals) { Name = "netWorth" };
            propertyProfitN = new Node(intervals) { Name = "propertyProfit" };
            tenantsRentN = new Node(intervals) { Name = "tenantsRent" };
            taxableTenantsRentN = new Node(intervals) { Name = "taxableTenantsRent" };
            nodeList.Add(bankAccountN);
            nodeList.Add(netWorthN);
            nodeList.Add(propertyProfitN);
            nodeList.Add(tenantsRentN);
            nodeList.Add(taxableTenantsRentN);
        }

        public void updateMultiples() {
            tenantsRent = tenantCount * oneTenantsRent;
            agentsFee = tenantsRent * 0.15f;
            wearAndTear = (tenantCount)/2 * 90;
            buildingsInsurance = (tenantCount)/2 * 12;
            accountantsFee = (tenantCount)/2 * 9;
        }

        public void calculatePropertyOutgoings() {
            propertyOutgoings = agentsFee + wearAndTear + buildingsInsurance + accountantsFee +
                mortgageInterest + mortgageRepayment;
        }

        public void calculatePropertyIncomeTax() {
            //Note, not quite right, if exceeds rent a room scheme limit!
            taxableTenantsRent = tenantsRent;
            switch (buyType) {
                case 0: //Buy to live in
                    taxableTenantsRent = tenantsRent - 4250; //Rent a room scheme...
                    //calculated here, because if use it, CANNOT claim back tax on your letting expenses!
                    //As gen rule, if expenses are LESS than £4250, then rent room scheme is better for you.
                    if (taxableTenantsRent < 0) taxableTenantsRent = 0;
                    break;
                case 1: //Buy to let
                    break;
            }
            calculatePropertyOutgoings();
            propertyProfit = taxableTenantsRent - (propertyOutgoings - mortgageRepayment);
            propertyIncomeTax = propertyProfit * 0.20f;
            if (propertyIncomeTax < 0) propertyIncomeTax = 0;
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
            outgoings = houseCosts + livingCosts + propertyOutgoings;
        }

        public void calculateStudentLoanPayment() {
            studentLoanPayment = jobIncome * 0.05f;
        }

        public void makeProjection() {
            initialSetup(); //makeProjection is runningMethod, not intialisingMethod, so this can't come here.
            //actually, it can... only CERTAIN initialisation steps that CANNOT be repeated...
            resetTree(treeView.Nodes);
            
            for (int interval = 0; interval < intervals; interval++) {
                //calculations:
                calculateMortgagePayments(interval);
                calculateIngoings();
                calculateOutgoings();
                bankAccount.contents = bankAccount.contents + ingoings - outgoings;
                netWorth = netWorth + ingoings - outgoings + mortgageRepayment;
                //STORE IN ARRAYS:
                bankAccountN.projection[interval] = bankAccount.contents;
                netWorthN.projection[interval] = netWorth;
                propertyProfitN.projection[interval] = propertyProfit;
                tenantsRentN.projection[interval] = tenantsRent;
                taxableTenantsRentN.projection[interval] = taxableTenantsRent;
                //STORE TOTALS:
                cumIngoings.cumulativeValue += ingoings;
                    cumJobIngoings.cumulativeValue += jobIngoings;
                    cumPropertyIngoings.cumulativeValue += tenantsRent - propertyIncomeTax;
                cumOutgoings.cumulativeValue += outgoings;
                    cumShelterOutgoings.cumulativeValue += houseCosts; 
                    cumLivingOutgoings.cumulativeValue += livingCosts; 
                    cumPropertyOutgoings.cumulativeValue += propertyOutgoings;
                        cumMortageRepayments.cumulativeValue += mortgageRepayment;
                        cumMortgageInterest.cumulativeValue += mortgageInterest;
                        cumWearAndTear.cumulativeValue += wearAndTear;
                        cumAgentsFee.cumulativeValue += agentsFee;
                        cumAccountantsFee.cumulativeValue += accountantsFee;
                cumPropertyIncomeTax.cumulativeValue += propertyIncomeTax;
                cumStudentLoanRepayments.cumulativeValue += studentLoanPayment;
                if (mortgageType == 1) autoInvest();
                //REPEATS
                //writeLine("mP", mortgagePayment);
                //writeLine("In", ingoings);
                //writeLine("", jobIngoings);
                //writeLine("", tenantsRent);
            }
            mortgagePaymentNode.cumulativeValue = mortgagePayment;
            labelTree(treeView.Nodes);
        }

        public override String ToString() {
            String name = Name;
            return name;
        }

        public void writeLine(String name, float value) {
            Console.WriteLine(name + ": " + String.Format("{0:n}", value));
        }
    }
}
