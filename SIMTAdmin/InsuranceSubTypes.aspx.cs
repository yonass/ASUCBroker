using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ManagementControllers;
using Broker.Controllers;

public partial class SIMTAdmin_InsuranceSubTypes : AuthenticationPage
{
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewInsuranceSubType.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
        
        
    }

    protected void btnEdit_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            mvMain.SetActiveView(viewEdit);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnDelete.CssClass = "izbrisi";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
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
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewEdit);
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        DropDownList ddlInsuranceType = DetailsView1.FindControl("ddlInsuranceTypes") as DropDownList;
        string code = CodeGenerator.InsuranceSubTypeCodeGenerator(int.Parse(ddlInsuranceType.SelectedValue));
        TextBox tbCode = DetailsView1.FindControl("tbCode") as TextBox;
        tbCode.Text = code;
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
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
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewInsuranceSubType.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
        }
        if (e.CommandName == "DoubleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
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
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewInsuranceSubType.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            mvMain.SetActiveView(viewEdit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnDelete.CssClass = "izbrisi_Active";
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
            GXGridView1.TotalRecords = ViewInsuranceSubType.SelectCountCached();
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
            GXGridView1.TotalRecords = ViewInsuranceSubType.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewInsuranceSubType.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        InsuranceSubType ist = e.InputParameters["newEntity"] as InsuranceSubType;
        InsuranceSubTypeController.ValidateUpdateCode(ist.ID, ist.Code);
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        InsuranceSubType ist = e.InputParameters["entityToInsert"] as InsuranceSubType;
        InsuranceSubTypeController.ValidateInsertCode(ist.Code);
    }

    protected void ddlInsuranceTypes_changed(object sender, EventArgs e) {
        DropDownList ddlInsuranceType = (DropDownList)sender;
        string code = CodeGenerator.InsuranceSubTypeCodeGenerator(int.Parse(ddlInsuranceType.SelectedValue));
        TextBox tbCode = DetailsView1.FindControl("tbCode") as TextBox;
        tbCode.Text = code;
    }
   
}