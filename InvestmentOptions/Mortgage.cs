using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class Mortgage {
        //MORTGAGE PAYMENTS
        public float housePrice = 100000;
        public float deposit = 25000;
        public float moneyBorrowed;
        public float repayment;
        public float moneyOwed;
        public float interestRate = 0.065f / 12; //apparently, 6.5% annual is NOT a cumulative rate...
        //0.00408f; //monthly interest rate (number works well for 5% annual).
        //try 487 for 6% annual.
        public float interest;
    }
}
