using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentOptions {
    public class BranchNode : TreeNode {
        public InvestmentOption Option { get; set; }

        public BranchNode()
        {
        }

        public BranchNode(String name) : base(name) {
            //do nothing
        }

    }
}
