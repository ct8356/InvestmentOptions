using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class Job : BranchNode {
        public LeafNode ingoings;
        public LeafNode studentLoanRepayments;
        public LeafNode income;

        public Job(InvestmentOption option): base("job") {
            Nodes.Add(ingoings = new LeafNode("ingoings"));
            Nodes.Add(studentLoanRepayments = new LeafNode("studentLoanRepayments"));
            Nodes.Add(income = new LeafNode("jobIncome"));
            income.mv = 1500;
            this.option = option;
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
