using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Broker.Controllers.ReportControllers;

public partial class BROKERAdmin_Reports : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {

    }

    protected void btnPrintByInsuranceSubType_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        int insuranceSubTypeID = int.Parse(ddlInsuranceSubTypes.SelectedValue);
        ReportController.AllPolicies(fromDate, toDate, 0, insuranceSubTypeID, 0, 0, 0);


    }
    protected void btnPrintByInsuranceType_Click(object sender, EventArgs e) {

        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        int insuranceTypeID = int.Parse(ddlInsuranceTypes.SelectedValue);
        if (insuranceTypeID == 0) {
            ReportController.AllPolicies(fromDate, toDate, 0, 0, 0, 0, 0);
        } else {
            ReportController.AllPolicies(fromDate, toDate, insuranceTypeID, 0, 0, 0, 0);
        }
    }


    protected void btnPrintByUser_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        int userID = int.Parse(ddlUsers.SelectedValue);
        ReportController.AllPolicies(fromDate, toDate, 0, 0, userID, 0, 0);
    }


    protected void btnPrintByBranch_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        int branchID = int.Parse(ddlBranches.SelectedValue);
        ReportController.AllPolicies(fromDate, toDate, 0, 0, 0, branchID, 0);
    }
    protected void btnPrintByCompany_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        int companyID = int.Parse(ddlCompanies.SelectedValue);
        ReportController.AllPolicies(fromDate, toDate, 0, 0, 0, 0, companyID);
    }
}
