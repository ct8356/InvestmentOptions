using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvestmentOptions {
    public class Mortgage : BranchNode {
        public enum Type { repayment, interestOnly };
        public Type type;
        //MORTGAGE PAYMENTS
        public float housePrice = 100000;
        public float deposit = 25000;
        public float moneyBorrowed;
        public LeafNode repayment;
        public float moneyOwed;
        public LeafNode interest;
        public float interestRate = 0.065f / 12; //apparently, 6.5% annual is NOT a cumulative rate...
        //0.00408f; //monthly interest rate (number works well for 5% annual).
        //try 487 for 6% annual.
        public LeafNode payment;

        public Mortgage() {
            Nodes.Add(repayment = new LeafNode("repayment", intervals));
            Nodes.Add(interest = new LeafNode("interest", intervals));
            Nodes.Add(payment = new LeafNode("payment", intervals));
        } //YOU KNOW WHAT? I just realised, that don't actually need to give
        //references to these objects.
        //Because strictly, they would be created by user.
        //WOuld anonymous... Only would have Strings for names...
        //Only a rigid program, would have references...

        public void calculateMortgagePayments(int interval) {
            switch (type) {
                case Mortgage.Type.repayment: //repaymentMortgage.
                    payment.monthlyValue = moneyBorrowed * interestRate *
                        ((float)Math.Pow(1 + interestRate, intervals)) /
                        ((float)Math.Pow(1 + interestRate, intervals) - 1);
                    interest.monthlyValue = moneyOwed * interestRate;
                    repayment.monthlyValue = payment.monthlyValue - interest.monthlyValue;
                    moneyOwed = moneyOwed - repayment.monthlyValue;
                    break;
                case Mortgage.Type.interestOnly: //interestOnlyMortgage
                    repayment.monthlyValue = 0;
                    interest.monthlyValue = moneyOwed * interestRate;
                    payment.monthlyValue = interest.monthlyValue;
                    break;
            }
        }

        //public void calculateMortgagePayments(int interval) {
        //    switch (mortgage.type) {
        //        case Mortgage.Type.repayment: //repaymentMortgage.
        //            mortgage.payment.monthlyValue = mortgage.moneyBorrowed * mortgage.interestRate *
        //                ((float)Math.Pow(1 + mortgage.interestRate, intervals)) /
        //                ((float)Math.Pow(1 + mortgage.interestRate, intervals) - 1);
        //            mortgage.interest.monthlyValue = mortgage.moneyOwed * mortgage.interestRate;
        //            mortgage.repayment.monthlyValue = mortgage.payment.monthlyValue - mortgage.interest.monthlyValue;
        //            mortgage.moneyOwed = mortgage.moneyOwed - mortgage.repayment.monthlyValue;
        //            break;
        //        case Mortgage.Type.interestOnly: //interestOnlyMortgage
        //            mortgage.repayment.monthlyValue = 0;
        //            mortgage.interest.monthlyValue = mortgage.moneyOwed * mortgage.interestRate;
        //            mortgage.payment.monthlyValue = mortgage.interest.monthlyValue;
        //            break;
        //    }
        //}
    }
}
