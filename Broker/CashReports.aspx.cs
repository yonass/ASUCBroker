using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.FacturesControllers;
using Broker.Controllers.ReportControllers;

public partial class Broker_CashReports : AuthenticationPage {

    public int GXGridView1SelectedValue {
        get {
            if (ViewState["GXGridView1SelectedValue"] != null) {
                return int.Parse(ViewState["GXGridView1SelectedValue"].ToString());
            } else {
                return 0;
            }
        }
        set {
            ViewState["GXGridView1SelectedValue"] = value;
        }
    }
    public List<PaymentInfo> listPaymentInfos {
        get {
            if (ViewState["ListPaymentInfos"] == null) {
                return new List<PaymentInfo>();
            } else {
                return (List<PaymentInfo>)ViewState["ListPaymentInfos"];
            }
        }
        set {
            ViewState["ListPaymentInfos"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewCashReport.SelectByFKCountCached("BranchID", this.PageUser.BranchID);
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewNew);
        btnNew.CssClass = "novZapis_Active";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        GXGridView1.DataBind();
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewCashReport.SelectSearchByFKCountCached("BranchID", this.PageUser.BranchID, SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            //btnFactureItems.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        }
        if (e.CommandName == "DoubleClick") {
            //btnDelete.Enabled = true;
            //btnFactureItems.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        reportControl.ForeignKeyID = this.PageUser.BranchID;
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewCashReport.SelectFilterByFKCountCached("BranchID", this.PageUser.BranchID, FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            //mvMain.SetActiveView(viewFactureItems);
            btnNew.CssClass = "novZapis";
            //btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
        }


    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewCashReport.SelectByFKCountCached("BranchID", this.PageUser.BranchID);
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewCashReport.SelectByFKCountCached("BranchID", this.PageUser.BranchID);
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewCashReport.SelectByFKCountCached("BranchID", this.PageUser.BranchID);
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        //e.Values["UserID"] = this.PageUser.ID;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
    }

    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }

    protected void odsSearch_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("sArgument", SearchControl1.SearchArguments);
        e.InputParameters.Add("foreignKeyName", "BranchID");
        e.InputParameters.Add("id", this.PageUser.BranchID);
    }

    protected void odsFilterGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("fArgument", FilterControl1.FCFilterArgument);
        e.InputParameters.Add("foreignKeyName", "BranchID");
        e.InputParameters.Add("id", this.PageUser.BranchID);
    }

    protected void odsGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("foreignKeyName", "BranchID");
        e.InputParameters.Add("id", this.PageUser.BranchID);
    }


    protected void btnCheck_Click(object sender, EventArgs e) {
        DateTime reportDate = DateTime.Parse(tbReportDate.Text);
        List<Payment> lstPayments = Payment.GetForCashReport(reportDate, this.PageUser.BranchID);
        List<PaymentInfo> lstPayInfos = new List<PaymentInfo>();
        foreach (Payment pay in lstPayments) {
            PaymentInfo payI = new PaymentInfo();
            payI.ID = pay.ID;
            payI.IsForCashReporting = true;
            payI.PaymentTypeID = pay.PaymentTypeID;
            payI.PaymentTypeName = pay.PaymentType.Name;
            payI.PolicyNumber = pay.Rate.PolicyItem.PolicyNumber;
            payI.Value = pay.Value;
            lstPayInfos.Add(payI);
        }
        listPaymentInfos = lstPayInfos;
        gvNewPolicies.DataSource = lstPayInfos;
        gvNewPolicies.DataBind();
    }

    protected void gvNewPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        List<PaymentInfo> lastPaymentInfos = listPaymentInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForCashReporting = gvRow.FindControl("cbIsForCashReporting") as CheckBox;
            int paymentInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            PaymentInfo payI = lastPaymentInfos.Where(f => f.ID == paymentInfoID).SingleOrDefault();
            payI.IsForCashReporting = cbIsForCashReporting.Checked;
        }
        listPaymentInfos = lastPaymentInfos;
        gvNewPolicies.PageIndex = e.NewPageIndex;
        gvNewPolicies.DataSource = lastPaymentInfos;
        gvNewPolicies.DataBind();
    }

    protected void btnCreate_Click(object sender, EventArgs e) {

        List<PaymentInfo> lastPaymentInfos = listPaymentInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForCashReporting = gvRow.FindControl("cbIsForCashReporting") as CheckBox;
            int paymentInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            PaymentInfo payI = lastPaymentInfos.Where(f => f.ID == paymentInfoID).SingleOrDefault();
            payI.IsForCashReporting = cbIsForCashReporting.Checked;
        }
        listPaymentInfos = lastPaymentInfos;
        List<PaymentInfo> listPaymentInfo = listPaymentInfos.Where(c => c.IsForCashReporting == true).ToList();
        if (listPaymentInfo.Count > 0) {
            DateTime reportDate = DateTime.Parse(tbReportDate.Text);
            CashReport cr = new CashReport();
            cr.Number = CashReport.GetNextNumber(this.PageUser.Branch);
            cr.BranchID = this.PageUser.BranchID;
            cr.CashReportDate = reportDate;
            cr.CashReportStatusID = CashReportStatuse.GetByCode(CashReportStatuse.ZA_ODOBRUVANjE).ID;
            cr.Discard = false;
            cr.IsAccountBooked = false;
            cr.TotalValue = 0;
            cr.Insert();
            decimal totValue = 0;
            foreach (PaymentInfo payI in listPaymentInfo) {
                totValue += payI.Value;
                CashReportItem cri = new CashReportItem();
                cri.PaymentID = payI.ID;
                cri.CashReportID = cr.ID;
                cri.IsActive = true;
                cri.Insert();
                Payment uPayment = Payment.Get(payI.ID);
                uPayment.IsCashReported = true;
                Payment.Table.Context.SubmitChanges();
            }
            CashReport crTmp = CashReport.Get(cr.ID);
            crTmp.TotalValue = totValue;
            CashReport.Table.Context.SubmitChanges();
            btnPrintCashReport.Enabled = true;
            ViewState["CashReportID"] = cr.ID;
        }
    }

    protected void btnDiscardCashReport_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedValue != null) {
                id = Convert.ToInt32(GXGridView1.SelectedValue);
            } else {
                id = GXGridView1SelectedValue;
            }
            CashReport cr = CashReport.Get(id);
            if (cr.CashReportStatuse.Code == CashReportStatuse.ODOBRENO) {
                RegisterStartupScript("myAlert", "<script>alert('НЕ МОЖЕ ДА СЕ СТОРНИРА ОДОБРЕН КАСОВ ИЗВЕШТАЈ!')</script>");
                return;
            } else {
                CashReport.DiscardCashReport(id);
                GXGridView1.TotalRecords = ViewCashReport.SelectByFKCountCached("BranchID", this.PageUser.BranchID);
                GXGridView1.DataBind();
            }

        }
    }
    protected void btnPrintCashReport_Click(object sender, EventArgs e) {
        if (ViewState["CashReportID"] != null) {
            CashReport cr = CashReport.Get(Convert.ToInt32(ViewState["CashReportID"]));
            PrintCashReports.PrintCashReport(cr);
        }
    }

    protected void btnPintCashReport_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (id > 0) {
            CashReport cr = CashReport.Get(id);
            PrintCashReports.PrintCashReport(cr);
        }
    }
}

    
