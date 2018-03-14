using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;
using Broker.Controllers.DistributionControllers;

public partial class SEAdmin_RollBackDistributionForCompany : AuthenticationPage {
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

    public List<RollBackDistributionCompanyInfo> list {
        get {
            if (ViewState["list"] != null) {
                return (List<RollBackDistributionCompanyInfo>)ViewState["list"];
            } else {
                return new List<RollBackDistributionCompanyInfo>();
            }
        }
        set {
            ViewState["list"] = value;
        }
    }

    public List<RollBackDistributionCompanyInfo> listSelected {
        get {
            if (ViewState["listSelected"] != null) {
                return (List<RollBackDistributionCompanyInfo>)ViewState["listSelected"];
            } else {
                return new List<RollBackDistributionCompanyInfo>();
            }
        }
        set {
            ViewState["listSelected"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
            GXGridView1.TotalRecords = RollBackDistributionCompanyInfo.SelectCountCached();
            list = RollBackDistributionCompanyInfo.Select().ToList();
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        GXGridView1.PageIndex = 0;
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        //btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
    }

    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        // btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
        //   btnConfirm.CssClass = "potvrdi";
    }

    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        // btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
        // btnConfirm.CssClass = "potvrdi";

    }

    protected void btnPintRollBack_Click(object sender, EventArgs e) {
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        // btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
        // btnConfirm.CssClass = "potvrdi";
        if (GXGridView1SelectedValue > 0) {
            RollBackDistribution rbd = RollBackDistribution.Get(GXGridView1SelectedValue);
            RollBackDistributionController.CreateFileForExistingWithoutPaymentTypeRecap(rbd);
        }
    }

    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewRollBackDistributionCompany.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }

    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnPintRollBack.Enabled = true;
            btnPrintAll.Enabled = true;
            btnConfirm.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        }
        if (e.CommandName == "DoubleClick") {
            btnPintRollBack.Enabled = true;
            btnPrintAll.Enabled = true;
            btnConfirm.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        }
    }

    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewRollBackDistributionCompany.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }

    protected void btnConfirm_Click(object sender, EventArgs e) {
        List<RollBackDistributionCompanyInfo> distList = list;
        GridViewRowCollection displayedRows = GXGridView1.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbSelected") as CheckBox;
            int rollBackID = Convert.ToInt32(GXGridView1.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionCompanyInfo fi = distList.Where(c => c.ID == rollBackID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
        }

        list = distList;
        List<RollBackDistributionCompanyInfo> infos = list.Where(c => c.IsForRollBack == true).ToList();
        listSelected = infos;
        gvSelectedRollBаcks.DataSource = infos;
        gvSelectedRollBаcks.DataBind();
        mvMain.SetActiveView(viewSelected);
        btnInsert.Enabled = true;


    }
    protected void GXGridView1_PageChanged(object sender, GridViewPageEventArgs e) {
        List<RollBackDistributionCompanyInfo> distList = list;
        GridViewRowCollection displayedRows = GXGridView1.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbSelected") as CheckBox;
            int rollBackID = Convert.ToInt32(GXGridView1.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionCompanyInfo fi = distList.Where(c => c.ID == rollBackID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
        }

        list = distList;
        GXGridView1.PageIndex = e.NewPageIndex;
        GXGridView1.DataSourceID = null;
        GXGridView1.DataSource = distList;
        GXGridView1.DataBind();
    }
    protected void gvSelectedRollBаcks_PageIndexChanging(object sender, GridViewPageEventArgs e) {

    }

    protected void btnPrintAll_Click(object sender, EventArgs e) {
        RollBackDistributionController.CreateFileForCompanyWithoutPaymentTypeRecap(listSelected);


    }
    protected void btnDiscard_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            RollBackDistribution rbd = RollBackDistribution.Get(GXGridView1SelectedValue);
            RollBackDistribution.SetDiscardedFromBranch(rbd);
            GXGridView1SelectedValue = 0;
            GXGridView1.DataBind();
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e) {
        List<RollBackDistributionCompanyInfo> finalList = listSelected;
        GridViewRowCollection displayedRows = gvSelectedRollBаcks.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbSelected") as CheckBox;
            int rollBackID = Convert.ToInt32(gvSelectedRollBаcks.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionCompanyInfo fi = finalList.Where(c => c.ID == rollBackID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
        }

        if (finalList.Where(c => c.IsForRollBack).ToList().Count > 0) {
            if (RollBackDistribution.CanMakeGroupRollBack(finalList.Where(c => c.IsForRollBack).ToList())) {
                lblError.Visible = false;
                RollBackDistribution.InsertForCompany(finalList.Where(c => c.IsForRollBack).ToList(), this.PageUser.ID);
            } else {
                lblError.Text = "Имате избрано раздолжници од повеќе компании";
                lblError.Visible = true;
            }
        } else {
            lblError.Text = "Немате селектирано ниту една раздолжница";
            lblError.Visible = true;
        }
        
    }
}
