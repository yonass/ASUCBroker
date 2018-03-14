using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.ReportControllers;
using Broker.DataAccess;

public partial class SEAdmin_RollBackDistributions : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {
        lblFeedBack.Visible = false;
    }
    protected void btnCreateClick(object sender, EventArgs e) {
        lblFeedBack.Visible = false;
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        int insuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
        RollBackDistribution rbd = RollBackDistribution.GetRollBack(startDate, endDate, insuranceCompanyID);
        RollBackDistributionController.CreateFileForRollBack(rbd, false);
    }
    protected void btnInsert_Click(object sender, EventArgs e) {
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        int insuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
        int count = RollBackDistribution.InsertRollBackDistribution(startDate, endDate, insuranceCompanyID);
        if (count > 0) {
            lblFeedBack.Visible = true;
            lblFeedBack.Text = "Снимено е раздолжувањето";
        } else {
            lblFeedBack.Visible = true;
            lblFeedBack.Text = "Не постојат полиси за раздолжување во тој период";
        }
    }

    protected void btnCreateRightRestrictions_Click(object sender, EventArgs e) {
        lblFeedBack.Visible = false;
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        int insuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
        RollBackDistributionController.ForRightRestrictions(insuranceCompanyID, startDate, endDate);

    }

    protected void btnInsertRightRestrictions_Click(object sender, EventArgs e) {
        lblFeedBack.Visible = false;
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        int insuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
        RestrictionsRollBackDistribution rrbd = RestrictionsRollBackDistribution.GetRollBack(startDate, endDate, insuranceCompanyID);
        int count = RestrictionsRollBackDistribution.InsertRollBackDistribution(startDate, endDate, insuranceCompanyID);
        if (count > 0) {
            lblFeedBack.Visible = true;
            lblFeedBack.Text = "Внесено е раздолжувањето";
        } else {
            lblFeedBack.Visible = true;
            lblFeedBack.Text = "Не постојат винкулации за раздолжување во тој период";
        }

    }
}
