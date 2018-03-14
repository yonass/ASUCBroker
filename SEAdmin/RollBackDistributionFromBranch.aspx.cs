using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;

public partial class SEAdmin_RollBackDistributionFromBranch : AuthenticationPage {

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
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
            GXGridView1.TotalRecords = ViewRollBackDistributionBranch.SelectCountCached();
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
        btnConfirm.CssClass = "potvrdi";
    }

    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
        btnConfirm.CssClass = "potvrdi";
    }

    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
        btnConfirm.CssClass = "potvrdi";

    }

    protected void btnPintRollBack_Click(object sender, EventArgs e) {
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnDiscard.CssClass = "storniraj";
        btnPintRollBack.CssClass = "pecati";
        btnConfirm.CssClass = "potvrdi";
        if (GXGridView1SelectedValue > 0) {
            RollBackDistribution rbd = RollBackDistribution.Get(GXGridView1SelectedValue);
            RollBackDistributionController.CreateFileForExistingWithoutPaymentTypeRecap(rbd);
        }
    }

    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewRollBackDistributionBranch.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }

    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnPintRollBack.Enabled = true;
            btnConfirm.Enabled = true;
            btnDiscard.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        }
        if (e.CommandName == "DoubleClick") {
            btnPintRollBack.Enabled = true;
            btnConfirm.Enabled = true;
            btnDiscard.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        }
    }

    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewRollBackDistributionBranch.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }

    protected void btnConfirm_Click(object sender, EventArgs e) {

        if (GXGridView1SelectedValue > 0) {
            RollBackDistribution rbd = RollBackDistribution.Get(GXGridView1SelectedValue);
            RollBackDistribution.SetConfirmedFromBranch(rbd);
            GXGridView1SelectedValue = 0;
            GXGridView1.DataBind();
        }

    }
    protected void btnDiscard_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            RollBackDistribution rbd = RollBackDistribution.Get(GXGridView1SelectedValue);
            RollBackDistribution.SetDiscardedFromBranch(rbd);
            GXGridView1SelectedValue = 0;
            GXGridView1.DataBind();
        }
    }

}
