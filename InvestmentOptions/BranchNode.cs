using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class BranchNode : MyTreeNode {
        public InvestmentOption option;

        public BranchNode(String name) : base(name) {
            //do nothing
        }

    }
}
