using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel; //yeah, because this is now the model for a component! A viewModel!
using System.Threading;

namespace InvestmentOptions {

    public class InvestmentOption : BranchNode {
        //NOTE: Whole reason you moved TreeView logic OUT of this class,
        //was because this class was becoming to complicated!
        //So keep them separate.
        //BUT MAYBE, but treeView above this one?
        //NEED TO SHRINK THIS FILE!
        //NEEDS TIDYING UP OF FIELDS! REVISIT
        //TREENODES
        public Job job;
        public Life life;
        public Mortgage mortgage;
        public Property property;
        public Shelter shelter;
        //OTHER NODES (WITH SERIES)
        public LeafNode bankAccount;
        public LeafNode netWorth;
        public LeafNode ingoings;
        public LeafNode outgoings;
        public bool showInPanel = false;
        public bool zeroInvestment = false;
        public bool noMortgageNeeded = false;
        public List<Boolean> booleans;
        public static Boolean countRentSavingsAsIncome = new Boolean("countRentSavingsAsIncome");
        public Boolean autoInvest = new Boolean("autoInvest");
        //NOTE: must be a better way of doing this... Not sure I even like Bindings. Not much simpler.
        public int years = 10;
        public int intervals; //(interval is one month).
        //List<String> keyList = new List<String>();
        //NOTE, since will need to reset all the below,
        //and since that is pain in bum, better to put them all in a list... maybe.
        //INGOINGS
        //Floats and ints fine into millions. Be careful when reach billions.
        public float bankInterest;
        public float bankInterestRate = 0.000f; //0, since bIR only covers inflation, not accounted for here.
        //a struct or union may be better for grouping living costs, but for now, we'll just use a class
        public float incomeTaxRate = 0.20f;
        //public Dictionary<String, LeafNode> nodeDictionary = new Dictionary<String, LeafNode>();
        //public List<LeafNode> nodeList = new List<LeafNode>();
        //public MyTreeView treeView;
        public MyTreeView RealWorldTree { get; set; }
        //interesting alternative to above, might be one class, with a load of nested-classes...
        //BUT I think the above is best way... want the ONLY relationship defined, to be defined by the TREE.

        public InvestmentOption(String name) : base(name) {
            Name = name;
            InitialiseVariables();
            //From now on, initialise specifically means, has to be done at start of program!
            //Reset means, has to be done at start of some other internal process...
            //Or a bind to tree method, OR use the tree itself as main variables.
            //OR bind the tree to these variables, OR any of above, just try one, then will see strngths and wknss.
            //initialiseTree(); //BIZARRELY! if call this, won't show up in other tree!
            //addChildren();
        }

        public InvestmentOption(String name, ProjectionForm form) : this(name) {
            RealWorldTree = new MyTreeView(form.controlPanel, this);
        }

        public void AddChildren()
        {
            Nodes.Add(job = new Job(this));
            Nodes.Add(life = new Life(this));
            Nodes.Add(mortgage = new Mortgage(this));
            Nodes.Add(property = new Property(this));
            Nodes.Add(shelter = new Shelter(this));
            Nodes.Add(bankAccount = new LeafNode("bankAccount"));
            Nodes.Add(netWorth = new LeafNode("netWorth"));
            Nodes.Add(ingoings = new LeafNode("ingoings"));
            Nodes.Add(outgoings = new LeafNode("outgoings"));
        }

        //public void initialiseTree() {
        //    //bizarrely, if call  this, won't show up in realWorldTree!!!
        //    //Can a node only exist in one tree at a time???
        //    treeView.Nodes.Add(ingoings);
        //        ingoings.Nodes.Add(jobIngoings);
        //        ingoings.Nodes.Add(realWorldTree.property.ingoings);
        //    treeView.Nodes.Add(outgoings);
        //        outgoings.Nodes.Add(cumShelterOutgoings);
        //        outgoings.Nodes.Add(realWorldTree.life.outgoings);
        //        outgoings.Nodes.Add(realWorldTree.property.outgoings);
        //        realWorldTree.property.outgoings.Nodes.Add(realWorldTree.mortgage.interest);
        //        realWorldTree.property.outgoings.Nodes.Add(realWorldTree.property.agentsFee);
        //        realWorldTree.property.outgoings.Nodes.Add(realWorldTree.property.wearAndTear);
        //        realWorldTree.property.outgoings.Nodes.Add(realWorldTree.mortgage.repayment);
        //        realWorldTree.property.outgoings.Nodes.Add(realWorldTree.property.accountantsFee);
        //        treeView.Nodes.Add(realWorldTree.property.incomeTax);
        //    treeView.Nodes.Add(cumStudentLoanRepayments);
        //    treeView.Nodes.Add(realWorldTree.mortgage.payment);
        //}

        public void Invest() {
            if (bankAccount.mv > mortgage.deposit) {
                //invest
                mortgage.moneyBorrowed += property.price - mortgage.deposit; // not that important.
                mortgage.moneyOwed += property.housePrice - mortgage.deposit;
                bankAccount.mv -= mortgage.deposit;
                property.moneyInvested += mortgage.deposit;
                //Benefits
                property.buyNewProperty();
            } //something you forgetting to update. what is it? YES the extra features that make you money!
        }

        public void CalculateCumulativeValues() {
            bankAccount.cumulativeValue = bankAccount.mv;
            netWorth.cumulativeValue = netWorth.mv;
            ingoings.cumulativeValue += ingoings.mv;
            job.ingoings.cumulativeValue += job.ingoings.mv;
            property.ingoings.cumulativeValue += property.tenantsRent.mv -
                property.incomeTax.mv;
            outgoings.cumulativeValue += outgoings.mv;
            shelter.outgoings.cumulativeValue += shelter.houseCosts.mv;
            life.outgoings.cumulativeValue += life.costs.mv;
            property.outgoings.cumulativeValue += property.outgoings.mv;
            mortgage.repayment.cumulativeValue += mortgage.repayment.mv;
            mortgage.interest.cumulativeValue += mortgage.interest.mv;
            mortgage.payment.cumulativeValue = mortgage.payment.mv;
            property.wearAndTear.cumulativeValue += property.wearAndTear.mv;
            property.agentsFee.cumulativeValue += property.agentsFee.mv;
            property.accountantsFee.cumulativeValue += property.accountantsFee.mv;
            property.incomeTax.cumulativeValue += property.incomeTax.mv;
            job.studentLoanRepayments.cumulativeValue += job.studentLoanRepayments.mv;
            //realWorldTree.property.capitalGains.cumulativeValue += realWorldTree.job.student
        }//Note, this can easily be done in own nodes... easier with events? or method waterfall?
        //either way, the method needs to be called by something... (by the delegate, or by the upper method).
        //NOTE: I might suggest, avoid using events unless really have to? i.e. registering with event of unmodifiable class?
        //AHAH! real question is, is it easier for you, to define the link in object, or in the parent???
        //WHATEVER! NEED to do something about this!!!

        public void calculateIngoings() {
            //just have to make sure all equations are well defined here...
            bankInterest = bankAccount.mv * bankInterestRate; //Monthly interest...     
            ingoings.mv = job.ingoings.mv + bankInterest + property.tenantsRent.mv -
                property.incomeTax.mv;
        }

        public void calculateOutgoings() {
            shelter.houseCosts.mv = shelter.rent.mv + shelter.houseBills.mv + shelter.councilTax.mv;
            outgoings.mv = shelter.houseCosts.mv + life.costs.mv + property.outgoings.mv;
        }

        //public void initialiseKeyList() {
        //    keyList.Add("bankAccount");
        //    keyList.Add("netWorth");
        //    keyList.Add("propertyProfit");
        //    keyList.Add("tenantsRent");
        //    keyList.Add("taxableTenantsRent");
        //    keyList.Add("propertyCosts");
        //}

        //public void initialiseNodeDictionary() {
        //    //actually, MIGHT want to have these as member variables,
        //    //somewhere, otherwise, have no develop/compile-time name suggestion/checking...
        //    //Try and leave both options open for now...
        //    foreach (String str in keyList) {
        //        nodeDictionary.Add(str, new LeafNode(str, this));
        //    }
        //}

        //public void initialiseNodeList() {
        //    nodeList.Add(bankAccountN = new LeafNode("bankAccount", this));
        //    nodeList.Add(netWorthN = new LeafNode("netWorthN", this));
        //    nodeList.Add(property.tenantsRent = new LeafNode("tenantsRentN", this));
        //    nodeList.Add(property.taxableTenantsRent = new LeafNode("taxableTenantsRentN", this));
        //    nodeList.Add(property.costs = new LeafNode("propertyCostsN", this));
        //    nodeList.Add(rentN = new LeafNode("rentN", this));
        //    nodeList.Add(mortgage.interest = new LeafNode("mortgageInterest", this));
        //    nodeList.Add(property.returnOnInvestment = new LeafNode("returnOnInvestment", this));
        //}

        public void InitialiseVariables() {
            intervals = years * 12;
        }

        public void MakeProjection() {
            ResetVariables();
            ResetRealWorldTree();
            for (int interval = 0; interval < intervals; interval++) {
                //calculations:
                mortgage.calculateMortgagePayments(intervals);
                property.updateVariables();
                calculateIngoings();
                calculateOutgoings();
                bankAccount.mv += ingoings.mv - outgoings.mv;
                netWorth.mv += ingoings.mv - outgoings.mv +
                    mortgage.repayment.mv + property.capitalGainsProfit.mv;
                //STORE TOTALS
                CalculateCumulativeValues();
                //UPDATE SERIES RECURSIVELY
                RealWorldTree.UpdateSeries(RealWorldTree.Nodes);
                //LOGIC
                if (autoInvest.Value)
                    Invest();
            }
        }

        public void ResetRealWorldTree() {
            RealWorldTree.ResetTree(RealWorldTree.Nodes);
        }

        public void ResetVariables() {
            property.resetIndependentVariables();
            mortgage.resetVariables();
            job.resetVariables();
            shelter.resetVariables();
            //Option specific stuff:
            bankAccount.mv = 0;
            bankAccount.cumulativeValue = 0;
            netWorth.mv = bankAccount.mv + property.propertyCount * mortgage.deposit;
            netWorth.cumulativeValue = netWorth.mv;
        }

        public override String ToString() {
            String name = Name;
            return name;
        }

        public void WriteLine(String name, float value) {
            Console.WriteLine(name + ": " + String.Format("{0:n}", value));
        }
    }

}
