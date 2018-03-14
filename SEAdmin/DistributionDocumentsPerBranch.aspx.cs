using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;

public partial class SEAdmin_DistributionDocumentsPerBranch : AuthenticationPage {
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

    protected void btnPrintDocument_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnDocumentItems.CssClass = "prikaz";
            btnPrintDocument.CssClass = "pecati_Active";
            btnChangeStatus.CssClass = "izmeni";
            DistributionDocument dd = DistributionDocument.Get(GXGridView1SelectedValue);
            PrintDistributionDocuments.PrintDistributionDocument(dd);
        }
    }

    protected void btnChangeStatus_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            DistributionDocument dd = DistributionDocument.Get(GXGridView1SelectedValue);
            if (dd.DistributionDocType.Code == DistributionDocType.ISPRATNICA) {
                mvMain.SetActiveView(viewChangeStatus);
                btnPreview.CssClass = "osvezi";
                btnReport.CssClass = "izvestaj";
                btnSearch.CssClass = "prebaraj";
                btnDocumentItems.CssClass = "prikaz";
                btnPrintDocument.CssClass = "pecati";
                btnChangeStatus.CssClass = "izmeni_Active";
                dvChangeStatusDistributionDocumentInfo.DataBind();
            } else {
                RegisterStartupScript("myAlert", "<script>alert('Документот не е испратница!')</script>");
            }
        }

    }

    protected void btnAnswer_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            DistributionDocument dd = DistributionDocument.Get(GXGridView1SelectedValue);

            List<DistributionDocumentItem> lstDDI = DistributionDocumentItem.GetByDistributionDocument(dd.ID);
            if (rblAnswer.SelectedValue == "Accept") {
                dd.DocumentStatusID = DistributionDocumentStatuse.GetByCode(DistributionDocumentStatuse.PRIMEN).ID;
                DistributionDocument.Table.Context.SubmitChanges();
                foreach (DistributionDocumentItem ddi in lstDDI) {
                    Distribution d = Distribution.Get(ddi.DistributionID);
                    d.DistributionStatusID = DistributionStatuse.GetByCode(DistributionStatuse.POTVRDENA).ID;
                    Distribution.Table.Context.SubmitChanges();
                }
            } else if (rblAnswer.SelectedValue == "Deny") {
                dd.DocumentStatusID = DistributionDocumentStatuse.GetByCode(DistributionDocumentStatuse.NEPRIFATEN).ID;
                DistributionDocument.Table.Context.SubmitChanges();
                foreach (DistributionDocumentItem ddi in lstDDI) {
                    Distribution d = Distribution.Get(ddi.DistributionID);
                    d.DistributionStatusID = DistributionStatuse.GetByCode(DistributionStatuse.VRATENA).ID;
                    d.BranchID = null;
                    d.UserID = Broker.DataAccess.User.GetFirstSEAdminUser().ID;
                    Distribution.Table.Context.SubmitChanges();
                }
            }
            viewGrid.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewDistributionDocument.SelectByFKCountCached("BranchID", this.PageUser.BranchID);
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnDocumentItems_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            mvMain.SetActiveView(viewDistributionDocumentItems);
            dvDistributionDocumentInfo.DataBind();
            GridViewDistributionDocumentItems.DataBind();
            dvDistributionDocumentInfo.DataBind();
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnDocumentItems.CssClass = "prikaz_Active";
            btnPrintDocument.CssClass = "pecati";
            btnChangeStatus.CssClass = "izmeni";
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnDocumentItems.CssClass = "prikaz";
        btnPrintDocument.CssClass = "pecati";
        btnChangeStatus.CssClass = "izmeni";
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnDocumentItems.CssClass = "prikaz";
        btnPrintDocument.CssClass = "pecati";
        btnChangeStatus.CssClass = "izmeni";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewDistributionDocument.SelectSearchByFKCountCached("BranchID", this.PageUser.BranchID, SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            btnPrintDocument.Enabled = true;
            btnDocumentItems.Enabled = true;
        }
        if (e.CommandName == "DoubleClick") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            btnPrintDocument.Enabled = true;
            btnDocumentItems.Enabled = true;
            btnDocumentItems_Click(null, null);
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnDocumentItems.CssClass = "prikaz";
        btnPrintDocument.CssClass = "pecati";
        btnChangeStatus.CssClass = "izmeni";
        reportControl.ForeignKeyID = this.PageUser.BranchID;
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewDistributionDocument.SelectFilterByFKCountCached("BranchID", this.PageUser.BranchID, FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
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


    protected void reportControl_Print(object sender, EventArgs e) {
        reportControl.ForeignKeyID = this.PageUser.BranchID;
    }
    protected void reportControl_DataBinding(object sender, EventArgs e) {
        reportControl.ForeignKeyID = this.PageUser.BranchID;
    }

    protected void odsDistributionDocument_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }
    protected void odsDistributionDocumentItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("distributionDocumentID", GXGridView1SelectedValue);
    }

}

