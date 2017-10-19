using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class BranchNode : TreeNode {
        public InvestmentOption option;

        public BranchNode(String name) : base(name) {
            //do nothing
        }

    }
}
