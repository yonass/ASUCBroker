using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;

public partial class BROKERAdmin_CashReports : AuthenticationPage {

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

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewCashReport.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnChangeStatus.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        GXGridView1.DataBind();
        btnChangeStatus.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewCashReport.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            //btnFactureItems.Enabled = true;
            btnChangeStatus.Enabled = true;
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
        btnChangeStatus.CssClass = "novZapis";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewCashReport.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            //mvMain.SetActiveView(viewFactureItems);
            btnChangeStatus.CssClass = "novZapis";
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
            GXGridView1.TotalRecords = ViewCashReport.SelectCountCached();
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
            GXGridView1.TotalRecords = ViewCashReport.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewCashReport.SelectCountCached();
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



    protected void btnPintCashReport_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (id > 0) {
            CashReport cr = CashReport.Get(id);
            PrintCashReports.PrintCashReport(cr);
        }
    }

    protected void btnChangeStatus_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (id > 0) {
            mvMain.SetActiveView(viewChangeStatus);
            dvChangeStatus.DataBind();
        }
    }

    protected void btnUpdateStatus_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (id > 0) {
            RadioButtonList rblStatuses = (RadioButtonList)dvChangeStatus.FindControl("rblStatuses");
            CashReport cr = CashReport.Get(id);
            if (rblStatuses.SelectedValue == "ODOBRENO") {
                cr.CashReportStatusID = CashReportStatuse.GetByCode(CashReportStatuse.ODOBRENO).ID;
            } else if (rblStatuses.SelectedValue == "VRATENO") {
                cr.CashReportStatusID = CashReportStatuse.GetByCode(CashReportStatuse.VRATENO).ID;
            }
            cr.ApproverUserID = this.PageUser.ID;
            cr.ApproverDate = DateTime.Now;
            CashReport.Table.Context.SubmitChanges();
            GXGridView1.TotalRecords = ViewCashReport.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }

    }
}
