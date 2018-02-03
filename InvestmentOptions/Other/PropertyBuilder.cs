using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentOptions
{
    public class PropertyBuilder
    {

        InvestmentOption Option { get; set; }

        public PropertyBuilder(InvestmentOption option)
        {
            Option = option;
        }

        public Property Build()
        {
            Property property = new Property(Option);
            property.Nodes.Add(property.AgentsFee = new LeafNode("AgentsFee"));
            property.AgentsFee.ShowInChartList[2] = true;
            property.Nodes.Add(property.Ingoings = new LeafNode("Ingoings"));
            property.Nodes.Add(property.TenantCount = new LeafNode("TenantCount")); //Somewhat pointless passing this reference now,
            //As it gets reinstantiated each time its accessed?
            //WELL that is fine! each time nodes accesses it,
            //it gets a fresh up-to-date object, nice!
            property.TenantCount.ShowInChartList[3] = true;
            property.Nodes.Add(property.TenantsRent = new LeafNode("TenantsRent"));
            property.TenantsRent.ShowInChartList[1] = true;
            property.Nodes.Add(property.TaxableTenantsRent = new LeafNode("TaxableTenantsRent"));
            property.TaxableTenantsRent.ShowInChartList[1] = true;
            property.Nodes.Add(property.WearAndTear = new LeafNode("WearAndTear"));
            property.WearAndTear.ShowInChartList[2] = true;
            property.Nodes.Add(property.AccountantsFee = new LeafNode("AccountantsFee"));
            property.AccountantsFee.ShowInChartList[2] = true;
            property.Nodes.Add(property.Outgoings = new LeafNode("Outgoings"));
            property.Nodes.Add(property.TaxableProfit = new LeafNode("TaxableProfit"));
            property.TaxableProfit.ShowInChartList[1] = true;
            property.Nodes.Add(property.IncomeTax = new LeafNode("IncomeTax"));
            property.IncomeTax.ShowInChartList[1] = true;
            property.Nodes.Add(property.RentSavings = new LeafNode("RentSavings"));
            property.Nodes.Add(property.ProfitAndSavings = new LeafNode("ProfitAndSavings"));
            property.ProfitAndSavings.ShowInChartList[1] = true;
            property.Nodes.Add(property.ReturnOnInvestment = new LeafNode("ReturnOnInvestment"));
            property.ReturnOnInvestment.ShowInChartList[3] = true;
            property.Nodes.Add(property.CapitalGains = new LeafNode("CapitalGains"));
            property.CapitalGains.ShowInChartList[0] = true;
            property.Nodes.Add(property.TaxableCapitalGains = new LeafNode("TaxableCapitalGains"));
            property.TaxableCapitalGains.ShowInChartList[1] = true;
            property.Nodes.Add(property.BasicAllowableCapitalGains = new LeafNode("BasicAllowableCapitalGains")); //LeafNode needed?
            property.Nodes.Add(property.BasicTaxableCapitalGains = new LeafNode("BasicTaxableCapitalGains"));
            property.BasicTaxableCapitalGains.ShowInChartList[0] = true;
            property.BasicTaxableCapitalGains.ShowInChartList[1] = true;
            property.Nodes.Add(property.HigherTaxableCapitalGains = new LeafNode("HigherTaxableCapitalGains"));
            property.HigherTaxableCapitalGains.ShowInChartList[0] = true;
            property.HigherTaxableCapitalGains.ShowInChartList[1] = true;
            property.Nodes.Add(property.CapitalGainsTax = new LeafNode("CapitalGainsTax"));
            property.Nodes.Add(property.CapitalGainsProfit = new LeafNode("CapitalGainsProfit"));
            property.CapitalGainsProfit.ShowInChartList[0] = true;
            property.Nodes.Add(property.Costs = new LeafNode("Costs"));
            property.Costs.ShowInChartList[1] = true;
            return property;
        }

    }
}
