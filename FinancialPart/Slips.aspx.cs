using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;

public partial class FinancialPart_Slips : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void btnInsert_Click(object sender, EventArgs e) {
        
        int bankID = int.Parse(ddlBank.SelectedValue);
        int provisionRateID = int.Parse(ddlProvision.SelectedValue);
        DateTime date = DateTime.Parse(tbDate.Text);
        decimal value = decimal.Parse(tbValue.Text);
        decimal totalValue = decimal.Parse(tbTotalValue.Text);
        Slip s = new Slip();
        s.BankID = bankID;
        s.PartialValue = value;
        s.ProvisionRateID = provisionRateID;
        s.SlipNumber = tbSlipNumber.Text;
        s.TotalValue = totalValue;
        s.Date = date;
        s.PolicyNumber = tbPolicyNumber.Text;
        s.Insert();
        tbPolicyNumber.Text = string.Empty;
        tbValue.Text= string.Empty;
        tbTotalValue.Text = string.Empty;
        tbSlipNumber.Text = string.Empty;
    }

    protected void tbValue_Chaged(object sender, EventArgs e) {
        decimal totalValue = decimal.Parse(tbTotalValue.Text);
        if (ddlProvision.SelectedIndex != -1) {
            ProvisionRate pr = ProvisionRate.Get(int.Parse(ddlProvision.SelectedValue));
            decimal value = totalValue * pr.Provision / 100;
            tbValue.Text =decimal.Round((totalValue-value),2).ToString();
        } else {
            tbValue.Text = string.Empty;
        }

    }


}
