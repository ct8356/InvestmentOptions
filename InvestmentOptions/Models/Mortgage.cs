using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cjt.Interfaces;

namespace InvestmentOptions {
    public class Mortgage : BranchNode {
        public enum BuyType { repayment, interestOnly };
        public BuyType type;
        public Property Property { get; set; }
        //MORTGAGE PAYMENTS
        public float SingleDeposit { get; set; } = 25000;
        public float moneyBorrowed;
        public IMoney repayment;
        public LeafNode moneyOwed;
        public LeafNode interest;
        public LeafNode debtReduced;
        public float interestRate = 0.065f / 12; //apparently, 6.5% annual is NOT a cumulative rate.
        //0.00408f; //monthly interest rate (number works well for 5% annual).
        //try 487 for 6% annual.
        public LeafNode payment;

        public Mortgage(InvestmentOption option) : base("mortgage") {
            Option = option;
            Property = option.Property;
            Nodes.Add(repayment = new LeafNode("mortgageRepayment"));
            Nodes.Add(interest = new LeafNode("mortgageInterest"));
            interest.ShowInChartList[2] = true;
            Nodes.Add(payment = new LeafNode("mortgagePayment"));
            Nodes.Add(moneyOwed = new LeafNode("moneyOwed"));
            moneyOwed.ShowInChartList[1] = true;
            Nodes.Add(debtReduced = new LeafNode("debtReduced"));
            
        } //YOU KNOW WHAT? I just realised, that don't actually need to give
        //references to these objects.
        //Because strictly, they would be created by user.
        //WOuld anonymous... Only would have Strings for names.
        //Only a rigid program, would have references.

        public void CalculateMortgagePayments(int intervals) {
            switch (type) {
                case BuyType.repayment: //repaymentMortgage.
                    if (moneyBorrowed >= 0)
                        payment.mv = moneyBorrowed * interestRate *
                        ((float)Math.Pow(1 + interestRate, intervals)) /
                        ((float)Math.Pow(1 + interestRate, intervals) - 1);
                    else payment.mv = 0;
                    interest.mv = moneyOwed.mv * interestRate;
                    repayment.mv = payment.mv - interest.mv;
                    if (moneyOwed.mv >= repayment.mv)
                        moneyOwed.mv = moneyOwed.mv - repayment.mv;
                    else moneyOwed.mv = repayment.mv;
                    //SEEMS to be a circular reference here.
                    //moneyOwed depends on repayment.
                    //repayment depends on interest.
                    //interest  depends on moneyOwed.
                    //THIS IS A PROBLEM! 
                    ////THING IS! Money Owed is SORT OF independent.
                    //OK to make moneyOwed depend on PREVIOUS repayment...
                    break;
                case BuyType.interestOnly: //interestOnlyMortgage
                    repayment.mv = 0;
                    interest.mv = moneyOwed.mv * interestRate;
                    payment.mv = interest.mv;
                    break;
            }
            debtReduced.mv = repayment.mv;
        }

        public void ResetVariables() {
            if (Property.OriginalHousePrice >= SingleDeposit)
                moneyBorrowed = Property.OriginalPropertyCount * (Property.OriginalHousePrice - SingleDeposit);
            else
                moneyBorrowed = 0;
            //TODO: could be issues here, if singleDeposit (rather than money paid) is considered for savings or something?
            if (Option.noMortgageNeeded) {
                Property.moneyInvested = Property.OriginalHousePrice;
                moneyBorrowed = 0;
            }
            moneyOwed.mv = moneyBorrowed;
        }

    }
}
