using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel; //yeah, because this is now the model for a component! A viewModel!
using System.Threading;

namespace InvestmentOptions {

    public class InvestmentOption : INotifyPropertyChanged {
        //NEED TO SHRINK THIS FILE!
        // The class has to be initialized from the UI thread
        public ProjectionPanel projectionPanel;
        public SynchronizationContext context = SynchronizationContext.Current;
        public event PropertyChangedEventHandler PropertyChanged;
        //This is the class to change, if you want to change the investment option!
        //Well actually, eventually want to do that from option dictator...
        //But if want to make it more sophisticated, start here (and inside its properties)...
        public String Name;
        public int years = 10;
        public int intervals; //(interval is one month)...
        //Note, I would say, don't split things into new classes, unless you really have to.
        //InvestmentOptions is PROBABLY as small as you need to go, for your purposes...
        //Do NOT want bankAccount to have outgoings inside it. That is a characteristic of 
        //investment options... I think...
        // but if it gets complicated, put stuff in BankAccount. So leave it there...
        //Floats and ints fine into millions. Be careful when reach billions.
        public TreeView treeView = new TreeView();
        List<String> keyList = new List<String>();
        //NOTE, since will need to reset all the below,
        //and since that is pain in bum, better to put them all in a list... maybe...
        //INGOINGS
        public float ingoings;
        public float jobIncome = 1500;
        public float jobIngoings;
        public float bankInterest;
        public float bankInterestRate = 0.000f; //0, since bIR only covers inflation, not accounted for here..
        public float tenantCount;
        public float oneTenantsRent = 330;
        //public float tenantsRent; //Sure to always be fairly HIGH amount, coz would buy in SouthWest...
        //OUTGOINGS
        //NOTE I THINK YOU SHOULD MAKE A PROPERTY CLASS???? OR STRUCT???
        public float outgoings;
        public float houseCosts;
        public float rent = 330; //Could actually be less. But nah. I should have bit of luxury...
        public float houseBills = 45;
        public float propertyIncomeTax;
        public float councilTax = 100;
        public float studentLoanPayment;
        public float mortgagePayment = 0;
        public float propertyOutgoings;
        //LIVING COSTS
        public LivingCosts livingCosts = new LivingCosts();
        //a struct or union may be better for grouping living costs, but for now, we'll just use a class
        //MORTGAGE
        public Mortgage mortgage = new Mortgage();
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
        public enum MortgageType { repayment, interestOnly };
        public MortgageType mortgageType;
        public enum BuyType { toLiveIn, toLet }; //buyToLive or buyToLet
        public BuyType buyType;
        private bool _rentARoomScheme = false;
        public bool rentARoomScheme {
            get {
                return _rentARoomScheme;
            }
            set {
                _rentARoomScheme = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RentARoomScheme")); //note, name the set method,
                //not the field. In c#, call these methods "properties".
                projectionPanel.presentInvestmentOption(this);//use form or panel, to try and just update the panel?
            }
        }
        //INDICES
        //These perhaps should be stored somewhere else, but for now, just store them here...
        public Dictionary<String, Node> nodeDictionary = new Dictionary<String, Node>();
        public List<Node> nodeList = new List<Node>();
        //For now, I'm gonna say, don't use it for the tree nodes... just the list...
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
        //interesting alternative to above, might be one class, with a load of nested-classes...
        //BUT I think the above is best way... want the ONLY relationship defined, to be defined by the TREE.
        //OTHER NODES (WITH SERIES)
        public Node bankAccountN;
        public Node netWorthN;
        public Node propertyProfitN;
        public Node tenantsRentN;
        public Node taxableTenantsRentN;
        public Node propertyCostsN;

        public InvestmentOption() {
            //INSTANTIATE
            initialiseVariables();
            //From now on, init specifically means, has to be done at start of program!
            //Reset means, has to be done at start of some other internal process...
            initialiseTree(); //unfortunately, fillTree has to come at end of make projection.
            //but there are initialisation things in fill tree. Need to separate them. 
            //create an intialiseTree method...
            //Or a bind to tree method...
            //OR use the tree itself as main variables.
            //OR bind the tree to these variables...
            //OR any of above, just try one... then will see strengths and weaknesses.
            //FORTUNATELY, labelTree can come at end of make projection!
            initialiseKeyList();
            initialiseNodeDictionary();
            initialiseNodeList();
        }

        public void initialiseTree() {
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

        public void autoInvest() {
            //check
            if (bankAccountN.monthlyValue > mortgage.deposit) {
                //invest
                mortgage.moneyBorrowed += mortgage.housePrice - mortgage.deposit; // not that important.
                mortgage.moneyOwed += mortgage.housePrice - mortgage.deposit;
                bankAccountN.monthlyValue -= mortgage.deposit;
                //Benefits
                tenantCount += 2;
                updateMultiples();
            } //something you forgetting to update. what is it? YES the extra features that make you money!
        }

        public void calculatePropertyOutgoings() {
            propertyOutgoings = agentsFee + wearAndTear + buildingsInsurance + accountantsFee +
                mortgage.interest + mortgage.repayment;
        }

        public void calculatePropertyIncomeTax() {
            //Note, not quite right, if exceeds rent a room scheme limit!
            taxableTenantsRentN.monthlyValue = tenantsRentN.monthlyValue;
            calculatePropertyOutgoings();
            propertyProfitN.monthlyValue = taxableTenantsRentN.monthlyValue - (propertyOutgoings - mortgage.repayment);
            switch (buyType) {
                case BuyType.toLiveIn: //Buy to live in
                    if (rentARoomScheme) {
                        taxableTenantsRentN.monthlyValue = tenantsRentN.monthlyValue - 4250 / 12;
                        propertyProfitN.monthlyValue = taxableTenantsRentN.monthlyValue;
                    } else {
                        //do nothing.
                    }
                    //calculated here, because if use it, CANNOT claim back tax on your letting expenses!
                    //As gen rule, if expenses are LESS than £4250, then rent room scheme is better for you.
                    if (taxableTenantsRentN.monthlyValue < 0) taxableTenantsRentN.monthlyValue = 0;
                    break;
                case BuyType.toLet: //Buy to let
                    //do nothing
                    break;
            }
            propertyIncomeTax = propertyProfitN.monthlyValue * 0.20f;
            if (propertyIncomeTax < 0) propertyIncomeTax = 0;
        }

        public void calculateIngoings() {
            //just have to make sure all equations are well defined here...
            bankInterest = nodeDictionary["bankAccount"].monthlyValue * bankInterestRate; //Monthly interest...
            calculatePropertyIncomeTax();
            ingoings = jobIngoings + bankInterest + tenantsRentN.monthlyValue - propertyIncomeTax;
        }

        public void calculateMortgagePayments(int interval) {
            switch (mortgageType) {
                case MortgageType.repayment: //repaymentMortgage.
                    mortgagePayment = mortgage.moneyBorrowed * mortgage.interestRate *
                        ((float)Math.Pow(1 + mortgage.interestRate, intervals)) / 
                        ((float) Math.Pow(1 + mortgage.interestRate,intervals) - 1); //How is this figure obtained? trial and error?
                    //mortgagePayment = 1100;
                    //Apparently, can use this equation:
                    //P = L*c*[(1 + c)^n]/[(1 + c)^n - 1]
                    mortgage.interest = mortgage.moneyOwed * mortgage.interestRate;
                    //this done after or before money Owed and mortgage repayment calculated?
                    //Ahah, if this calc is done first, then all works out easy!
                    mortgage.repayment = mortgagePayment - mortgage.interest;
                    //How is this decided? with some kind of simultaneous eq'n... or numerical method?
                    //note, if simultaneous eq'ns used, could just find the final one on web!
                    //as long as average comes out at £285, its ok).
                    mortgage.moneyOwed = mortgage.moneyOwed - mortgage.repayment;
                    //Console.WriteLine("" + interest);
                    break;
                case MortgageType.interestOnly: //interestOnlyMortgage
                    mortgage.repayment = 0;
                    //moneyOwed = moneyOwed;
                    mortgage.interest = mortgage.moneyOwed * mortgage.interestRate;
                    mortgagePayment = mortgage.interest;
                    break;
            }
        }

        public void calculateOutgoings() {
            calculatePropertyIncomeTax();
            houseCosts = rent + houseBills + councilTax;
            outgoings = houseCosts + livingCosts.sum + propertyOutgoings;
        }

        public void calculateStudentLoanPayment() {
            studentLoanPayment = jobIncome * 0.05f;
        }

        public void initialiseKeyList() {
            keyList.Add("bankAccount");
            keyList.Add("netWorth");
            keyList.Add("propertyProfit");
            keyList.Add("tenantsRent");
            keyList.Add("taxableTenantsRent");
            keyList.Add("propertyCosts");
        }

        public void initialiseNodeDictionary() {
            //actually, MIGHT want to have these as member variables,
            //somewhere, otherwise, have no develop/compile-time name suggestion/checking...
            //Try and leave both options open for now...
            foreach (String str in keyList) {
                nodeDictionary.Add(str, new Node(str, intervals));
            }
        }

        public void initialiseNodeList() {
            nodeList.Add(bankAccountN = new Node("bankAccount", intervals));
            nodeList.Add(netWorthN = new Node("netWorthN", intervals));
            nodeList.Add(propertyProfitN = new Node("propertyProfitN", intervals));
            nodeList.Add(tenantsRentN = new Node("tenantsRentN", intervals));
            nodeList.Add(taxableTenantsRentN = new Node("taxableTenantsRentN", intervals));
            nodeList.Add(propertyCostsN = new Node("propertyCostsN", intervals));
        }

        public void initialiseVariables() {
            intervals = years * 12;
        }

        public void labelTree(TreeNodeCollection nodes) {
            foreach (Node node in nodes) {
                node.Text = node.Name + ": £" + String.Format("{0:n}", node.cumulativeValue);
                labelTree(node.Nodes);
            }
        }

        public void makeProjection(ProjectionPanel panel) {
            this.projectionPanel = panel;
            resetVariables();
            resetTree(treeView.Nodes);
            resetNodeList();
            //createNodeList();
            for (int interval = 0; interval < intervals; interval++) {
                //calculations:
                calculateMortgagePayments(interval);
                calculateIngoings();
                calculateOutgoings();
                bankAccountN.monthlyValue += ingoings - outgoings;
                netWorthN.monthlyValue += + ingoings - outgoings + mortgage.repayment;
                propertyCostsN.monthlyValue = propertyOutgoings - mortgage.repayment;
                //STORE IN ARRAYS:
                foreach (Node node in nodeList) {
                    node.projection[interval] = node.monthlyValue;
                }
                //STORE TOTALS:
                cumIngoings.cumulativeValue += ingoings;
                    cumJobIngoings.cumulativeValue += jobIngoings;
                    cumPropertyIngoings.cumulativeValue += tenantsRentN.monthlyValue - propertyIncomeTax;
                cumOutgoings.cumulativeValue += outgoings;
                    cumShelterOutgoings.cumulativeValue += houseCosts; 
                    cumLivingOutgoings.cumulativeValue += livingCosts.sum; 
                    cumPropertyOutgoings.cumulativeValue += propertyOutgoings;
                        cumMortageRepayments.cumulativeValue += mortgage.repayment;
                        cumMortgageInterest.cumulativeValue += mortgage.interest;
                        cumWearAndTear.cumulativeValue += wearAndTear;
                        cumAgentsFee.cumulativeValue += agentsFee;
                        cumAccountantsFee.cumulativeValue += accountantsFee;
                cumPropertyIncomeTax.cumulativeValue += propertyIncomeTax;
                cumStudentLoanRepayments.cumulativeValue += studentLoanPayment;
                if (mortgageType == MortgageType.interestOnly) autoInvest();
            }
            mortgagePaymentNode.cumulativeValue = mortgagePayment;
            labelTree(treeView.Nodes);
        }

        public void resetNodeList() {
            foreach (Node node in nodeDictionary.Values) {
                for (int interval = 0; interval < intervals; interval++) {
                    node.projection[interval] = 0;
                }
            }
        }

        public void resetTree(TreeNodeCollection nodes) {
            foreach (Node node in nodes) {
                node.cumulativeValue = 0;
                resetTree(node.Nodes);
            }
        }

        public void resetVariables() {
            mortgage.moneyBorrowed = mortgage.housePrice - mortgage.deposit;
            mortgage.moneyOwed = mortgage.moneyBorrowed;
            calculateStudentLoanPayment();
            jobIngoings = jobIncome - studentLoanPayment;
            bankAccountN.monthlyValue = 0;
            netWorthN.monthlyValue = bankAccountN.monthlyValue;
            tenantCount = 2;
            updateMultiples();
        }

        public void updateMultiples() {
            tenantsRentN.monthlyValue = tenantCount * oneTenantsRent;
            agentsFee = tenantsRentN.monthlyValue * 0.15f;
            wearAndTear = (tenantCount) / 2 * 90;
            buildingsInsurance = (tenantCount) / 2 * 12;
            accountantsFee = (tenantCount) / 2 * 9;
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
