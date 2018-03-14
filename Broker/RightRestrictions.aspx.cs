using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
public partial class Broker_RightRestrictions : System.Web.UI.Page {

    protected void Page_Load(object sender, EventArgs e) {
        
    }
    public int PolicyItemID {
        get {
            return int.Parse(ViewState["PolicyItemID"].ToString());
        }
        set {
            ViewState["PolicyItemID"] = value;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e) {
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
        int insuranceTypeID = Convert.ToInt32(ddlInsuranceType.SelectedValue);
        int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue);
        string policyNumber = tbPolicyNumber.Text;
        PolicyItem pi = PolicyItem.GetByNumberAndInsuranceSubType(policyNumber, insuranceSubTypeID, insuranceCompanyID);
        if (pi != null) {
            tbStartDate.Text = pi.Policy.StartDate.ToShortDateString();
            tbEndDate.Text = pi.Policy.EndDate.ToShortDateString();
            tbCoverage.Text = pi.PremiumValue.ToString();
            tbOwnerEMBG.Text = pi.Policy.Client1.EMBG;
            tbOwnerName.Text = pi.Policy.Client1.Name;
            PolicyItemID = pi.ID;

        }
        lblError.Visible = false;
        lblSuccess.Visible = false;


    }

    protected void ddlInsuranceTypeSelectedIndexChanged(object sender, EventArgs e) {
        ddlInsuranceSubTypes.DataBind();
        ddlInsuranceCompany.DataBind();
        lblError.Visible = false;
        lblSuccess.Visible = false;
    }

    protected void ddlInsuranceSubType_selecteIndexChanged(object sender, EventArgs e) {
        ddlInsuranceCompany.DataBind();
        lblError.Visible = false;
        lblSuccess.Visible = false;
        
    }

    protected void ddlInsuranceCompanyIndexChanged(object sender, EventArgs e) {
        lblError.Visible = false;
        lblSuccess.Visible = false;
    }
    protected void btnInsert_Click(object sender, EventArgs e) {
        RightRestrictionDistribution rrd = (RightRestrictionDistribution.ExistForCompany(int.Parse(ddlInsuranceCompany.SelectedValue),tbRestrictionNumber.Text.Trim()));
        if(rrd!=null){
            if (rrd.IsUsed) {
                lblError.Text = "Винкулацијата со број " + tbRestrictionNumber.Text + " е искористена";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            } else {
                RightRestriction rr = new RightRestriction();
                rr.PolicyItemID = PolicyItemID;
                rr.Number = tbRestrictionNumber.Text;
                rr.BankID = int.Parse(ddlBanks.SelectedValue);
                rr.ApplicationDate = DateTime.Today;
                rr.Description = tbDescription.Text;
                rr.Discard = false;
                rr.Value = decimal.Parse(tbCoverage.Text);
                rr.IsRollBacked = false;
                rr.InsuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
                rr.Insert();
                RightRestrictionDistribution.UpdateUsed(int.Parse(ddlInsuranceCompany.SelectedValue), rr.Number);
                lblError.Visible = false;
                lblSuccess.Visible = true;
            }
        }
        else {
            lblError.Text="Винкулацијата со број "+tbRestrictionNumber.Text+" не е задолжена";
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }


    }
}
