using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.ReportControllers;

public partial class FinancialPart_FinancialReports : AuthenticationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnPrint_Click(object sender, EventArgs e) {
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        string reportType = rblReportType.SelectedValue;
        FinancialReportController.CreateFileForPeriod(startDate, endDate, reportType);
    }
}
