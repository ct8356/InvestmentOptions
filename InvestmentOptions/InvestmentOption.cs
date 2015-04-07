using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel; //yeah, because this is now the model for a component! A viewModel!
using System.Threading;

namespace InvestmentOptions {

    public class InvestmentOption : BranchNode {
        public event PropertyChangedEventHandler PropertyChanged;
        //NEED TO SHRINK THIS FILE!
        //NEEDS TIDYING UP OF FIELDS! CBTL
        public ProjectionForm form;
        public ProjectionPanel projectionPanel;
        ////TREENODES
        //public Job job;
        //public Life life;
        //public Mortgage mortgage;
        //public Property property;
        //public Shelter shelter;
        ////OTHER NODES (WITH SERIES)
        //public LeafNode bankAccount;
        //public LeafNode netWorth;
        //public LeafNode ingoings;
        //public LeafNode outgoings;
        public bool showInPanel = false;
        public bool zeroInvestment = false;
        public bool noMortgageNeeded = false;
        public List<MyBoolean> booleans;
        public static MyBoolean countRentSavingsAsIncome = new MyBoolean("countRentSavingsAsIncome");
        public MyBoolean autoInvest = new MyBoolean("autoInvest");
        //NOTE: must be a better way of doing this... Not sure I even like Bindings. Not much simpler...
        public String Name;
        public int years = 10;
        public int intervals; //(interval is one month)...
        //List<String> keyList = new List<String>();
        //NOTE, since will need to reset all the below,
        //and since that is pain in bum, better to put them all in a list... maybe...
        //INGOINGS
        //Floats and ints fine into millions. Be careful when reach billions.
        public float bankInterest;
        public float bankInterestRate = 0.000f; //0, since bIR only covers inflation, not accounted for here..
        //a struct or union may be better for grouping living costs, but for now, we'll just use a class
        public float incomeTaxRate = 0.20f;
        //public Dictionary<String, LeafNode> nodeDictionary = new Dictionary<String, LeafNode>();
        //public List<LeafNode> nodeList = new List<LeafNode>();
        //public MyTreeView treeView;
        public MyTreeView realWorldTree;
        //interesting alternative to above, might be one class, with a load of nested-classes...
        //BUT I think the above is best way... want the ONLY relationship defined, to be defined by the TREE.

        public InvestmentOption(String name) : base(name) {
            this.Name = name;
            initialiseVariables();
            //From now on, initialise specifically means, has to be done at start of program!
            //Reset means, has to be done at start of some other internal process...
            //Or a bind to tree method, OR use the tree itself as main variables.
            //OR bind the tree to these variables, OR any of above, just try one, then will see strngths and wknss.
            //initialiseTree(); //BIZARRELY! if call this, won;t show up in other tree!
            //addChildren();
        }

        public InvestmentOption(String name, ProjectionForm form) : this(name) {
            this.form = form;
            //treeView = new MyTreeView(form.controlPanel, this);
            realWorldTree = new MyTreeView(form.controlPanel, this);
        }

        //public void addChildren() {
        //    //Nodes.Add(option.Nodes);
        //    Nodes.Add(job = new Job());
        //    Nodes.Add(life = new Life());
        //    Nodes.Add(mortgage = new Mortgage());
        //    Nodes.Add(property = new Property());
        //    Nodes.Add(shelter = new Shelter());
        //    Nodes.Add(bankAccount = new LeafNode("bankAccount"));
        //    Nodes.Add(netWorth = new LeafNode("netWorth"));
        //    Nodes.Add(ingoings = new LeafNode("ingoings"));
        //    Nodes.Add(outgoings = new LeafNode("outgoings"));
        //    //NOTE: could add these children to investmentOption, THEN add them here!!!
        //    //BETTER!
        //    //Only issue is, WOULD I have to bind them together???
        //    //DO I call anything, here?
        //}

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

        public void invest() {
            if (realWorldTree.bankAccount.mv > realWorldTree.mortgage.deposit) {
                //invest
                realWorldTree.mortgage.moneyBorrowed += realWorldTree.property.price - realWorldTree.mortgage.deposit; // not that important.
                realWorldTree.mortgage.moneyOwed += realWorldTree.mortgage.housePrice - realWorldTree.mortgage.deposit;
                realWorldTree.bankAccount.mv -= realWorldTree.mortgage.deposit;
                realWorldTree.property.moneyInvested += realWorldTree.mortgage.deposit;
                //Benefits
                realWorldTree.property.buyNewProperty();
            } //something you forgetting to update. what is it? YES the extra features that make you money!
        }

        public void calculateCumulativeValues() {
            realWorldTree.bankAccount.cumulativeValue = realWorldTree.bankAccount.mv;
            realWorldTree.netWorth.cumulativeValue = realWorldTree.netWorth.mv;
            realWorldTree.ingoings.cumulativeValue += realWorldTree.ingoings.mv;
            realWorldTree.job.ingoings.cumulativeValue += realWorldTree.job.ingoings.mv;
            realWorldTree.property.ingoings.cumulativeValue += realWorldTree.property.tenantsRent.mv -
                realWorldTree.property.incomeTax.mv;
            realWorldTree.outgoings.cumulativeValue += realWorldTree.outgoings.mv;
            realWorldTree.shelter.outgoings.cumulativeValue += realWorldTree.shelter.houseCosts.mv;
            realWorldTree.life.outgoings.cumulativeValue += realWorldTree.life.costs.mv;
            realWorldTree.property.outgoings.cumulativeValue += realWorldTree.property.outgoings.mv;
            realWorldTree.mortgage.repayment.cumulativeValue += realWorldTree.mortgage.repayment.mv;
            realWorldTree.mortgage.interest.cumulativeValue += realWorldTree.mortgage.interest.mv;
            realWorldTree.mortgage.payment.cumulativeValue = realWorldTree.mortgage.payment.mv;
            realWorldTree.property.wearAndTear.cumulativeValue += realWorldTree.property.wearAndTear.mv;
            realWorldTree.property.agentsFee.cumulativeValue += realWorldTree.property.agentsFee.mv;
            realWorldTree.property.accountantsFee.cumulativeValue += realWorldTree.property.accountantsFee.mv;
            realWorldTree.property.incomeTax.cumulativeValue += realWorldTree.property.incomeTax.mv;
            realWorldTree.job.studentLoanRepayments.cumulativeValue += realWorldTree.job.studentLoanRepayments.mv;
            //realWorldTree.property.capitalGains.cumulativeValue += realWorldTree.job.student
        }//Note, this can easily be done in own nodes... easier with events? or method waterfall?
        //either way, the method needs to be called by something... (by the delegate, or by the upper method).
        //NOTE: I might suggest, avoid using events unless really have to? i.e. registering with event of unmodifiable class?
        //AHAH! real question is, is it easier for you, to define the link in object, or in the parent???
        //WHATEVER! NEED to do something about this!!!

        public void calculateIngoings() {
            //just have to make sure all equations are well defined here...
            bankInterest = realWorldTree.bankAccount.mv * bankInterestRate; //Monthly interest...     
            realWorldTree.ingoings.mv = realWorldTree.job.ingoings.mv + bankInterest + realWorldTree.property.tenantsRent.mv -
                realWorldTree.property.incomeTax.mv;
        }

        public void calculateOutgoings() {
            realWorldTree.shelter.houseCosts.mv = realWorldTree.shelter.rent.mv + realWorldTree.shelter.houseBills.mv + realWorldTree.shelter.councilTax.mv;
            realWorldTree.outgoings.mv = realWorldTree.shelter.houseCosts.mv + realWorldTree.life.costs.mv + realWorldTree.property.outgoings.mv;
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

        public void initialiseVariables() {
            intervals = years * 12;
        }

        public void makeProjection(ProjectionPanel panel) {
            this.projectionPanel = panel;
            resetVariables(); //YES!!! glad you called this here... This should sort everything out...
            //resetTree(treeView.Nodes);
            resetRealWorldTree();
            //createNodeList();
            for (int interval = 0; interval < intervals; interval++) {
                //calculations:
                //NEED TO FIRE AN EVENT HERE!!! 
                //Or, just call one method, that waterfalls all child methods...
                //(NOTE! Order is very important! that is almost good reason for a few events!).
                //With events, register the delegate/method with the event.... and write a method...
                //Both are fairly similar...
                //FOR NOW, don't change anything... I don't see a massive improvement in clarity...
                realWorldTree.mortgage.calculateMortgagePayments(intervals);
                realWorldTree.property.updateVariables();
                calculateIngoings();
                calculateOutgoings();
                realWorldTree.bankAccount.mv += realWorldTree.ingoings.mv - realWorldTree.outgoings.mv;
                realWorldTree.netWorth.mv += realWorldTree.ingoings.mv - realWorldTree.outgoings.mv +
                    realWorldTree.mortgage.repayment.mv + realWorldTree.property.capitalGainsProfit.mv;
                //STORE TOTALS:
                calculateCumulativeValues();
                //UPDATE SERIES OF ALL NODES IN THE TREE!
                //I SUGGEST, next BIG task, should be, put these in OWN places... //CBTL
                realWorldTree.updateSeries(realWorldTree.Nodes);
                //STRANGELY, above is not getting called when I click a checkBox... should... 
                if (autoInvest.value)
                    invest();
            }
            //treeView.labelTree(treeView.Nodes);
        }

        public void resetRealWorldTree() {
            realWorldTree.resetTree(realWorldTree.Nodes);
        }

        //public void resetTree(TreeNodeCollection nodes) {
        //    foreach (LeafNode node in nodes) {
        //        node.cumulativeValue = 0;
        //        resetTree(node.Nodes);
        //    }
        //}

        public void resetVariables() {
            realWorldTree.property.resetIndependentVariables();
            realWorldTree.mortgage.resetVariables();
            realWorldTree.job.resetVariables();
            realWorldTree.shelter.resetVariables();
            //Option specific stuff:
            realWorldTree.bankAccount.mv = 0;
            realWorldTree.bankAccount.cumulativeValue = 0;
            realWorldTree.netWorth.mv = realWorldTree.bankAccount.mv + realWorldTree.property.propertyCount * realWorldTree.mortgage.deposit;
            realWorldTree.netWorth.cumulativeValue = realWorldTree.netWorth.mv;
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
