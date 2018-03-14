using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;

public partial class BROKERAdmin_LifeDeals : AuthenticationPage {
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
            GXGridView1.TotalRecords = ViewLifeDeal.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }
            mvMain.SetActiveView(viewEdit);
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnDelete.CssClass = "izbrisi";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnBrokerages.CssClass = "brokerazi";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnBrokerages.CssClass = "brokerazi";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewEdit);
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnBrokerages.CssClass = "brokerazi";
    }
    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnBrokerages.CssClass = "brokerazi";
    }

    protected void btnBrokerages_Click(object sender, EventArgs e) {

        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {

                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsLifeDealPreview.SelectParameters.Clear();
                odsLifeDealPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvLifeDealPreview.DataBind();
                odsGridViewLifeDealItems.SelectParameters.Clear();
                odsGridViewLifeDealItems.SelectParameters.Add("lifeDealID", GXgvSelectedValue.ToString());
                GridViewLifeDealItems.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsLifeDealPreview.SelectParameters.Clear();
                odsLifeDealPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvLifeDealPreview.DataBind();
                odsGridViewLifeDealItems.SelectParameters.Clear();
                odsGridViewLifeDealItems.SelectParameters.Add("lifeDealID", GXgvSelectedValue.ToString());
                GridViewLifeDealItems.DataBind();
            }

            mvMain.SetActiveView(viewBrokerages);
            mvLifeDealItems.SetActiveView(viewLifeDealItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnDelete.CssClass = "izbrisi";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnBrokerages.CssClass = "brokerazi_Active";
        }
    }

    protected void DetailsViewLifeDealItems_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewBrokerages);
            mvLifeDealItems.SetActiveView(viewLifeDealItemsGrid);
        }
    }

    protected void DetailsViewLifeDealItems_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsGridViewLifeDealItems.SelectParameters.Clear();
                odsGridViewLifeDealItems.SelectParameters.Add("lifeDealID", GXgvSelectedValue.ToString());
                GridViewLifeDealItems.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsGridViewLifeDealItems.SelectParameters.Clear();
                odsGridViewLifeDealItems.SelectParameters.Add("lifeDealID", GXgvSelectedValue.ToString());
                GridViewLifeDealItems.DataBind();
            }
            mvMain.SetActiveView(viewBrokerages);
            mvLifeDealItems.SetActiveView(viewLifeDealItemsGrid);
        }
    }

    protected void dvDataSourceLifeDealItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }

    protected void DetailsViewLifeDealItems_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsViewLifeDealItems_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        int orderID = 0;
        if (GXGridView1.SelectedValue != null) {
            orderID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        } else {
            orderID = GXGridView1SelectedValue;
        }
        e.Values["LifeDealID"] = orderID;
    }

    protected void btnNewLifeDealItem_Click(object sender, EventArgs e) {
        mvLifeDealItems.SetActiveView(viewLifeDealItemsEdit);
        DetailsViewLifeDealItems.ChangeMode(DetailsViewMode.Insert);
    }

    protected void odsGridViewLifeDealItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("lifeDealID", GXGridView1SelectedValue);
    }

    protected void GridViewLifeDealItems_RowDeleting(object sender, GridViewDeleteEventArgs e) {

    }

    protected void GridViewLifeDealItems_RowDeleted(object sender, GridViewDeletedEventArgs e) {

    }

    protected void odsLifeDealPreview_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }

    protected void dvLifeDealPreview_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void dvLifeDealPreview_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewLifeDeal.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnBrokerages.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
        }
        if (e.CommandName == "DoubleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnBrokerages.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            mvMain.SetActiveView(viewEdit);
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnBrokerages.CssClass = "brokerazi";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewLifeDeal.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            mvMain.SetActiveView(viewEdit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnBrokerages.CssClass = "brokerazi";
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewLifeDeal.SelectCountCached();
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
            GXGridView1.TotalRecords = ViewLifeDeal.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewLifeDeal.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        e.Values["IsActive"] = true;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        LifeDeal ld = e.InputParameters["newEntity"] as LifeDeal;
        //DealController.ValidateUpdateCode(d.ID, d.Code);
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        LifeDeal ld = e.InputParameters["entityToInsert"] as LifeDeal;
        //DealController.ValidateInsertCode(d.Code);
    }

    protected void btnDelete_Click1(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            int dealID = 0;
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dealID = GXgvSelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dealID = GXgvSelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }
            LifeDeal ld = LifeDeal.Get(dealID);
            ld.IsActive = false;
            LifeDeal.Table.Context.SubmitChanges();

            GXGridView1.TotalRecords = ViewLifeDeal.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }
}