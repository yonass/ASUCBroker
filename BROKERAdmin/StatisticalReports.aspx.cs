using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.ReportControllers;

public partial class BROKERAdmin_StatisticalReports : AuthenticationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            ddlInsuranceCompanies.DataBind();
        }
    }
    protected void btnPrintByPremium_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        StatisticalReportsCotroller.GetPremiumStatisticsForTimePeriod(fromDate, toDate);
    }
    protected void btnPrintByNumber_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        StatisticalReportsCotroller.GetNumberStatisticsForTimePeriod(fromDate, toDate);
    }

    protected void btnCompanyGroup_Click(object sender, EventArgs e)
    {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        ReportsGroupedByInsuranceCompany.ReportsForPaymentsTypesGroupedByInsuranceCompany(fromDate, toDate);
    }

    protected void btnReportForASO_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        int insuranceCompanyID = 0;
        int.TryParse(ddlInsuranceCompanies.SelectedValue, out insuranceCompanyID);
        ReportForASO.PrintReportForASO(insuranceCompanyID, fromDate, toDate);
    }
    //protected void btnFinCard_Click(object sender, EventArgs e) {
    //    DateTime fromDate = DateTime.Parse(tbStartDate.Text);
    //    DateTime toDate = DateTime.Parse(tbEndDate.Text);
    //    if (ddlFinCardType.SelectedValue == "FinCardByApplicationDate") {
    //        PrintFinCardController.PrintFinCardByApplicationDateReport(fromDate, toDate);
    //    } else if (ddlFinCardType.SelectedValue == "FinCardByPaidDates") {
    //        PrintFinCardController.PrintFinCardByPaidDates(fromDate, toDate);
    //    }
    //}
}
