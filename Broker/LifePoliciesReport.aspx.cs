using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.ReportControllers;

public partial class Broker_LifePoliciesReport : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void btnPrintReportForBrokerage_Click(object sender, EventArgs e) {
        DateTime fromDate = Convert.ToDateTime(tbStartDate.Text);
        DateTime toDate = Convert.ToDateTime(tbEndDate.Text);
        LifePolicyReportController.PrintLifePolicyReportController(fromDate, toDate);
    }
}
