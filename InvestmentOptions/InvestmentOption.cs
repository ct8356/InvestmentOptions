using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel; //yeah, because this is now the model for a component! A viewModel!
using System.Threading;

namespace InvestmentOptions {

    public class InvestmentOption {
        //NEED TO SHRINK THIS FILE!
        public ProjectionPanel projectionPanel;
        public String Name;
        public int years = 10;
        public int intervals; //(interval is one month)...
        //Floats and ints fine into millions. Be careful when reach billions.
        List<String> keyList = new List<String>();
        //NOTE, since will need to reset all the below,
        //and since that is pain in bum, better to put them all in a list... maybe...
        //INGOINGS
        public float jobIncome = 1500;
        public float bankInterest;
        public float bankInterestRate = 0.000f; //0, since bIR only covers inflation, not accounted for here..
        //public float tenantsRent; //Sure to always be fairly HIGH amount, coz would buy in SouthWest...
        //OUTGOINGS
        //NOTE I THINK YOU SHOULD MAKE A PROPERTY CLASS???? OR STRUCT???
        public float houseCosts;
        //public float rent = 330; //Could actually be less. But nah. I should have bit of luxury...
        public float houseBills = 45;
        public float councilTax = 100;
        public float studentLoanPayment;
        //PROPERTY
        public Property property;
        //LIVING COSTS
        public Life life = new Life();
        //a struct or union may be better for grouping living costs, but for now, we'll just use a class
        //MORTGAGE
        public Mortgage mortgage = new Mortgage();
        //INCOME TAX
        public float incomeTaxRate = 0.20f;
        //NOTE: If wanted to get rid of all the floats above, could turn some of them into methods...
        //OR EVEN BETTER, PROPERTIES!
        //INDICES
        public float moneyInvested;
        //These perhaps should be stored somewhere else, but for now, just store them here...
        public Dictionary<String, LeafNode> nodeDictionary = new Dictionary<String, LeafNode>();
        public List<LeafNode> nodeList = new List<LeafNode>();
        public MyTreeView treeView = new MyTreeView();
        public MyTreeView realWorldTree = new MyTreeView();
        //For now, I'm gonna say, don't use it for the tree nodes... just the list...
        public LeafNode ingoings = new LeafNode() {Name = "ingoings"};
            public LeafNode jobIngoings = new LeafNode() {Name = "jobIngoings"};
        public LeafNode outgoings = new LeafNode() { Name = "outgoings"};
            public LeafNode cumShelterOutgoings = new LeafNode() { Name = "cumShelterOutgoings" };
        public LeafNode cumStudentLoanRepayments = new LeafNode() { Name = "cumStudentLoanRepayments" };
        //interesting alternative to above, might be one class, with a load of nested-classes...
        //BUT I think the above is best way... want the ONLY relationship defined, to be defined by the TREE.

        //OTHER NODES (WITH SERIES)
        public LeafNode bankAccountN;
        public LeafNode netWorthN;
        public LeafNode rentN;

        public InvestmentOption() {
            property = new Property(this);
            initialiseVariables();
            //From now on, init specifically means, has to be done at start of program!
            //Reset means, has to be done at start of some other internal process...
            initialiseTree();
            //Or a bind to tree method...
            //OR use the tree itself as main variables.
            //OR bind the tree to these variables...
            //OR any of above, just try one... then will see strengths and weaknesses.
            initialiseRealWorldTree();
            //Just make the realWorldTree add its children, and nodes, add theirs, on construction! ???
            initialiseKeyList();
            initialiseNodeDictionary();
            initialiseNodeList();
        }

        public void initialiseTree() {
            treeView.Nodes.Add(ingoings);
                ingoings.Nodes.Add(jobIngoings);
                ingoings.Nodes.Add(property.ingoings);
            treeView.Nodes.Add(outgoings);
                outgoings.Nodes.Add(cumShelterOutgoings);
                outgoings.Nodes.Add(life.outgoings);
                outgoings.Nodes.Add(property.outgoings);
                    property.outgoings.Nodes.Add(mortgage.interest);
                    property.outgoings.Nodes.Add(property.agentsFee);
                    property.outgoings.Nodes.Add(property.wearAndTear);
                    property.outgoings.Nodes.Add(mortgage.repayment);
                    property.outgoings.Nodes.Add(property.accountantsFee);
            treeView.Nodes.Add(property.incomeTax);
            treeView.Nodes.Add(cumStudentLoanRepayments);
            treeView.Nodes.Add(mortgage.payment);
        }

        public void autoInvest() {
            if (bankAccountN.monthlyValue > mortgage.deposit) {
                //invest
                mortgage.moneyBorrowed += property.price - mortgage.deposit; // not that important.
                mortgage.moneyOwed += mortgage.housePrice - mortgage.deposit;
                bankAccountN.monthlyValue -= mortgage.deposit;
                moneyInvested += mortgage.deposit;
                //Benefits
                property.tenantCount += 2;
                updateMultiples();
            } //something you forgetting to update. what is it? YES the extra features that make you money!
        }

        public void calculateIngoings() {
            //just have to make sure all equations are well defined here...
            bankInterest = nodeDictionary["bankAccount"].monthlyValue * bankInterestRate; //Monthly interest...
            property.calculatePropertyIncomeTax();
            ingoings.monthlyValue = jobIngoings.monthlyValue + bankInterest + property.tenantsRent.monthlyValue - 
                property.incomeTax.monthlyValue;
        }

        public void calculateOutgoings() {
            property.calculatePropertyIncomeTax();
            if (property.buyType == Property.BuyType.toLiveIn) {
                rentN.monthlyValue = 0;
            }
            else {
                rentN.monthlyValue = 330;
            }
            houseCosts = rentN.monthlyValue + houseBills + councilTax;
            outgoings.monthlyValue = houseCosts + life.costs.monthlyValue + property.outgoings.monthlyValue;
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
                nodeDictionary.Add(str, new LeafNode(str, intervals));
            }
        }

        public void initialiseNodeList() {
            nodeList.Add(bankAccountN = new LeafNode("bankAccount", intervals));
            nodeList.Add(netWorthN = new LeafNode("netWorthN", intervals));
            nodeList.Add(property.tenantsRent = new LeafNode("tenantsRentN", intervals));
            nodeList.Add(property.taxableTenantsRent = new LeafNode("taxableTenantsRentN", intervals));
            nodeList.Add(property.costs = new LeafNode("propertyCostsN", intervals));
            nodeList.Add(rentN = new LeafNode("rentN", intervals));
            nodeList.Add(mortgage.interest = new LeafNode("mortgageInterest", intervals));
            nodeList.Add(property.returnOnInvestment = new LeafNode("returnOnInvestment", intervals));
        }

        public void initialiseRealWorldTree() {
            realWorldTree.Nodes.Add(life);
            realWorldTree.Nodes.Add(mortgage);
            realWorldTree.Nodes.Add(property);
        }

        public void initialiseVariables() {
            intervals = years * 12;
        }

        public void makeProjection(ProjectionPanel panel) {
            this.projectionPanel = panel;
            resetVariables();
            resetTree(treeView.Nodes);
            resetRealWorldTree();
            //createNodeList();
            for (int interval = 0; interval < intervals; interval++) {
                //calculations:
                mortgage.calculateMortgagePayments(interval);
                calculateIngoings();
                calculateOutgoings();
                bankAccountN.monthlyValue += ingoings.monthlyValue - outgoings.monthlyValue;
                netWorthN.monthlyValue += ingoings.monthlyValue - outgoings.monthlyValue + 
                    mortgage.repayment.monthlyValue;
                property.costs.monthlyValue = property.outgoings.monthlyValue - mortgage.repayment.monthlyValue;
                moneyInvested += mortgage.repayment.monthlyValue;
                property.returnOnInvestment.monthlyValue = 12 * property.profit.monthlyValue / moneyInvested * 100;
                //STORE IN ARRAYS:
                //foreach (LeafNode node in nodeList) {
                //    node.projection[interval] = node.monthlyValue;
                //}
                realWorldTree.updateProjectionsAndSeries(realWorldTree.Nodes);
                //STORE TOTALS:
                ingoings.cumulativeValue += ingoings.monthlyValue;
                    jobIngoings.cumulativeValue += jobIngoings.monthlyValue;
                    property.ingoings.cumulativeValue += property.tenantsRent.monthlyValue - 
                        property.incomeTax.monthlyValue;
                outgoings.cumulativeValue += outgoings.monthlyValue;
                    cumShelterOutgoings.cumulativeValue += houseCosts;
                    life.outgoings.cumulativeValue += life.costs.monthlyValue; 
                    property.outgoings.cumulativeValue += property.outgoings.monthlyValue;
                        mortgage.repayment.cumulativeValue += mortgage.repayment.monthlyValue;
                        mortgage.interest.cumulativeValue += mortgage.interest.monthlyValue;
                        property.wearAndTear.cumulativeValue += property.wearAndTear.monthlyValue;
                        property.agentsFee.cumulativeValue += property.agentsFee.monthlyValue;
                        property.accountantsFee.cumulativeValue += property.accountantsFee.monthlyValue;
                property.incomeTax.cumulativeValue += property.incomeTax.monthlyValue;
                cumStudentLoanRepayments.cumulativeValue += studentLoanPayment;
                if (mortgage.type == Mortgage.Type.interestOnly) autoInvest();
            }
            mortgage.payment.cumulativeValue = mortgage.payment.monthlyValue;
            treeView.labelTree(treeView.Nodes);
        }

        public void resetRealWorldTree() {
            realWorldTree.resetTree(realWorldTree.Nodes);
        }

        public void resetTree(TreeNodeCollection nodes) {
            foreach (LeafNode node in nodes) {
                node.cumulativeValue = 0;
                resetTree(node.Nodes);
            }
        }

        public void resetVariables() {
            moneyInvested = mortgage.deposit;
            mortgage.moneyBorrowed = mortgage.housePrice - mortgage.deposit;
            mortgage.moneyOwed = mortgage.moneyBorrowed;
            calculateStudentLoanPayment();
            jobIngoings.monthlyValue = jobIncome - studentLoanPayment;
            bankAccountN.monthlyValue = 0;
            netWorthN.monthlyValue = bankAccountN.monthlyValue;
            property.tenantCount = property.originalTenantCount;
            updateMultiples();
        }

        public void updateMultiples() {
            property.tenantsRent.monthlyValue = property.tenantCount * property.oneTenantsRent;
            property.agentsFee.monthlyValue = property.tenantsRent.monthlyValue * 0.15f;
            property.wearAndTear.monthlyValue = (property.tenantCount) / 2 * 90;
            property.buildingsInsurance = (property.tenantCount) / 2 * 12;
            property.accountantsFee.monthlyValue = (property.tenantCount) / 2 * 9;
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
