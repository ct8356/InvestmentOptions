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
        public Job Job { get; set; }
        public Life Life { get; set; }
        public Mortgage Mortgage { get; set; }
        public Property Property { get; set; }
        public Shelter Shelter { get; set; }
        //OTHER NODES (WITH SERIES)
        //NOTE: these other nodes need definitions of
        //how to behave.
        //behaviour for each is different,
        //SO its best to create a class for each one!
        public LeafNode BankAccount { get; set; }
        public LeafNode NetWorth { get; set; }
        public LeafNode Ingoings { get; set; }
        public LeafNode Outgoings { get; set; }
        public bool showInPanel = false;
        public bool zeroInvestment = false;
        public bool noMortgageNeeded = false;
        public List<Boolean> booleans;
        public static Boolean countRentSavingsAsIncome = new Boolean("countRentSavingsAsIncome");
        public Boolean autoInvest = new Boolean("autoInvest");
        //NOTE: must be a better way of doing this... 
        //Not sure I even like Bindings. Not much simpler.
        public int years = 10;
        public int intervals; //(interval is one month).
        //List<String> keyList = new List<String>();
        //NOTE, since will need to reset all the below,
        //and since that is pain in bum, better to put them all in a list... maybe.
        //INGOINGS
        //Floats and ints fine into millions. Be careful when reach billions.
        public float bankInterest;
        public float bankInterestRate = 0.000f;
        //0, since bIR only covers inflation, not accounted for here.
        //a struct or union may be better for grouping living costs, 
        //but for now, we'll just use a class
        public float incomeTaxRate = 0.20f;
        //public Dictionary<String, LeafNode> nodeDictionary;
        //public List<LeafNode> nodeList = new List<LeafNode>();
        //public MyTreeView treeView;
        public new TreeView TreeView { get; set; }
        //interesting alternative to above, 
        //might be one class, with a load of nested-classes...
        //BUT I think the above is best way... 
        //want the ONLY relationship defined, to be defined by the TREE.

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
            TreeView = new TreeView(form.controlPanel, this);
            // REVISIT not quite right.
        }

        public void AddChildren()
        {
            Nodes.Add(Job = new Job(this));
            Nodes.Add(Life = new Life(this));
            Nodes.Add(Mortgage = new Mortgage(this));
            Nodes.Add(Property = new Property(this));
            Nodes.Add(Shelter = new Shelter(this));
            //Shelter.rent.showInChartList[1] = true;
            Nodes.Add(BankAccount = new LeafNode("bankAccount"));
            BankAccount.ShowInChartList[0] = true;
            Nodes.Add(NetWorth = new LeafNode("netWorth"));
            NetWorth.ShowInChartList[0] = true;
            Nodes.Add(Ingoings = new LeafNode("ingoings"));
            Nodes.Add(Outgoings = new LeafNode("outgoings"));
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
            if (BankAccount.mv > Mortgage.deposit) {
                //invest
                Mortgage.moneyBorrowed += Property.price - Mortgage.deposit; // not that important.
                Mortgage.moneyOwed += Property.housePrice - Mortgage.deposit;
                BankAccount.mv -= Mortgage.deposit;
                Property.moneyInvested += Mortgage.deposit;
                //Benefits
                Property.buyNewProperty();
            } //something you forgetting to update. what is it? YES the extra features that make you money!
        }

        public void CalculateCumulativeValues() {
            BankAccount.cumulativeValue = BankAccount.mv;
            NetWorth.cumulativeValue = NetWorth.mv;
            Ingoings.cumulativeValue += Ingoings.mv;
            Job.ingoings.cumulativeValue += Job.ingoings.mv;
            Property.ingoings.cumulativeValue += Property.tenantsRent.mv -
                Property.incomeTax.mv;
            Outgoings.cumulativeValue += Outgoings.mv;
            Shelter.outgoings.cumulativeValue += Shelter.houseCosts.mv;
            Life.outgoings.cumulativeValue += Life.costs.mv;
            Property.outgoings.cumulativeValue += Property.outgoings.mv;
            Mortgage.repayment.cumulativeValue += Mortgage.repayment.mv;
            Mortgage.interest.cumulativeValue += Mortgage.interest.mv;
            Mortgage.payment.cumulativeValue = Mortgage.payment.mv;
            Property.wearAndTear.cumulativeValue += Property.wearAndTear.mv;
            Property.agentsFee.cumulativeValue += Property.agentsFee.mv;
            Property.accountantsFee.cumulativeValue += Property.accountantsFee.mv;
            Property.incomeTax.cumulativeValue += Property.incomeTax.mv;
            Job.studentLoanRepayments.cumulativeValue += Job.studentLoanRepayments.mv;
            //property.capitalGains.cumulativeValue += job.student
            //Note, this can easily be done in own nodes.
            //easier with events? or method waterfall?
            //either way, the method needs to be called by something.
            //(by the delegate, or by the upper method).
            //NOTE: I might suggest, avoid using events unless really have to? 
            //i.e. registering with event of unmodifiable class?
            //AHAH! real question is, is it easier for you, 
            //to define the link in object or in the parent???
            //WHATEVER! NEED to do something about this!!!
        }

        public void CalculateIngoings() {
            //just have to make sure all equations are well defined here...
            bankInterest = BankAccount.mv * bankInterestRate; //Monthly interest...     
            Ingoings.mv = Job.ingoings.mv + bankInterest + Property.tenantsRent.mv -
                Property.incomeTax.mv;
        }

        public void CalculateOutgoings() {
            Shelter.houseCosts.mv = Shelter.rent.mv + Shelter.houseBills.mv + Shelter.councilTax.mv;
            Outgoings.mv = Shelter.houseCosts.mv + Life.costs.mv + Property.outgoings.mv;
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
            ResetTreeView();
            for (int interval = 0; interval < intervals; interval++) {
                //Calculations:
                Mortgage.calculateMortgagePayments(intervals);
                Property.updateVariables();
                CalculateIngoings();
                CalculateOutgoings();
                BankAccount.mv += Ingoings.mv - Outgoings.mv;
                NetWorth.mv += Ingoings.mv - Outgoings.mv +
                    Mortgage.repayment.mv + Property.capitalGainsProfit.mv;
                //STORE TOTALS
                CalculateCumulativeValues();
                //UPDATE SERIES RECURSIVELY
                TreeView.UpdateSeries(TreeView.Nodes);
                //LOGIC
                if (autoInvest.Value)
                    Invest();
            }
        }

        public void ResetTreeView() {
            TreeView.ResetTree(TreeView.Nodes);
        }

        public void ResetVariables() {
            Property.ResetIndependentVariables();
            Mortgage.resetVariables();
            Job.resetVariables();
            Shelter.ResetVariables();
            //Option specific stuff
            BankAccount.mv = 0;
            BankAccount.cumulativeValue = 0;
            NetWorth.mv = BankAccount.mv + Property.propertyCount * Mortgage.deposit;
            NetWorth.cumulativeValue = NetWorth.mv;
        }

        public override String ToString() {
            return Name;
        }

        public void WriteLine(String name, float value) {
            Console.WriteLine(name + ": " + String.Format("{0:n}", value));
        }
    }

}
