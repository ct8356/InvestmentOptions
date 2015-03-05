using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class Job : BranchNode {
        public LeafNode ingoings;
        public LeafNode studentLoanRepayments;
        public LeafNode income;

        public Job(InvestmentOption option): base("job", option) {
            Nodes.Add(ingoings = new LeafNode("ingoings", option));
            Nodes.Add(studentLoanRepayments = new LeafNode("studentLoanRepayments", option));
            Nodes.Add(income = new LeafNode("jobIncome", option));
            income.mv = 1500;
        }

        public void calculateStudentLoanPayment() {
            studentLoanRepayments.mv = income.mv * 0.05f;
        }

        public void resetVariables() {
            calculateStudentLoanPayment();
            ingoings.mv = income.mv - studentLoanRepayments.mv;
        }
    }
}
