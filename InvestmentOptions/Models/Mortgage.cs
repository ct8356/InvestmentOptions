using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {
    public class Mortgage : BranchNode {
        public enum BuyType { repayment, interestOnly };
        public BuyType type;
        //MORTGAGE PAYMENTS
        public float deposit = 25000;
        public float moneyBorrowed;
        public LeafNode repayment;
        public float moneyOwed;
        public LeafNode interest;
        public LeafNode debtReduced;
        public float interestRate = 0.065f / 12; //apparently, 6.5% annual is NOT a cumulative rate...
        //0.00408f; //monthly interest rate (number works well for 5% annual).
        //try 487 for 6% annual.
        public LeafNode payment;

        public Mortgage(InvestmentOption option) : base("mortgage") {
            Nodes.Add(repayment = new LeafNode("mortgageRepayment"));
            Nodes.Add(interest = new LeafNode("mortgageinterest"));
            interest.ShowInChartList[2] = true;
            Nodes.Add(payment = new LeafNode("mortgagePayment"));
            Nodes.Add(debtReduced = new LeafNode("debtReduced"));
            this.Option = option;
        } //YOU KNOW WHAT? I just realised, that don't actually need to give
        //references to these objects.
        //Because strictly, they would be created by user.
        //WOuld anonymous... Only would have Strings for names...
        //Only a rigid program, would have references...

        public void CalculateMortgagePayments(int intervals) {
            switch (type) {
                case Mortgage.BuyType.repayment: //repaymentMortgage.
                    payment.mv = moneyBorrowed * interestRate *
                        ((float)Math.Pow(1 + interestRate, intervals)) /
                        ((float)Math.Pow(1 + interestRate, intervals) - 1);
                    interest.mv = moneyOwed * interestRate;
                    repayment.mv = payment.mv - interest.mv;
                    moneyOwed = moneyOwed - repayment.mv;
                    break;
                case Mortgage.BuyType.interestOnly: //interestOnlyMortgage
                    repayment.mv = 0;
                    interest.mv = moneyOwed * interestRate;
                    payment.mv = interest.mv;
                    break;
            }
            debtReduced.mv = repayment.mv;
        }

        public void ResetVariables() {
            moneyBorrowed = 
                Option.Property.OriginalPropertyCount * (Option.Property.housePrice - deposit);
            if (Option.noMortgageNeeded) {
                Option.Property.moneyInvested = Option.Property.OriginalHousePrice;
                moneyBorrowed = 0;
            }
            moneyOwed = moneyBorrowed;
        }

    }
}
