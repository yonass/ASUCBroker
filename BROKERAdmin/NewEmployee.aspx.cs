using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Utility;
using Broker.Controllers.EmployeeManagement;
using Broker.DataAccess;

public partial class BROKERAdmin_NewEmployee : AuthenticationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewBrokeragesForMarketingAgent.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }

    }
    protected void btnNext_Click(object sender, EventArgs e) {
        Broker.DataAccess.Role r = Broker.DataAccess.Role.GetRoleByName(RolesInfo.MarketingAgent);
        int userID = EmployeeController.InsertMarketingAgent(EMBGTextBox.Text.Trim(), FullNameTextBox.Text.Trim(), int.Parse(BranchesList.SelectedValue), AddressTextBox.Text.Trim(), PhoneNumberTextBox.Text.Trim(), r.ID);
        tbUserID.Text = userID.ToString();
        tbName.Text = Broker.DataAccess.User.GetUserById(userID).Name;
        mvMain.SetActiveView(ViewInsertBrokerages);
        mvBrokerages.SetActiveView(viewItemsGrid);
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        GXGridView1.SelectedIndex = -1;
        mvMain.SetActiveView(ViewInsertNewEmployee);
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        
    }
    protected void btnEdit_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            int id = Convert.ToInt32(GXGridView1.SelectedValue);
            BrokeragesForMarketingAgent bfma = BrokeragesForMarketingAgent.Get(id);
            tbUserID.Text = bfma.UserID.ToString();
            tbName.Text = Broker.DataAccess.User.GetUserById(bfma.UserID).Name;
            odsGridViewItems.SelectParameters.Clear();
            odsGridViewItems.SelectParameters.Add("UserID", bfma.UserID.ToString());
            GridViewItems.DataBind();
            mvMain.SetActiveView(ViewInsertBrokerages);
            mvBrokerages.SetActiveView(viewItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        GXGridView1.DataSourceID = odsGridView.ID;
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }

    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewBrokeragesForMarketingAgent.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnEdit.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
          //  GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
           // DetailsView1.DataBind();
            //GridViewOfferItems.DataBind();
           // dvOfferPreview.DataBind();
           // dvOfferPreviewForAttachments.DataBind();
        }
        if (e.CommandName == "DoubleClick") {
            btnEdit.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            //mvMain.SetActiveView(viewOfferItems);
            //mvOfferItems.SetActiveView(viewOfferItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            //GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            //DetailsView1.DataBind();
            //GridViewOfferItems.DataBind();
            //dvOfferPreview.DataBind();
            //dvOfferPreviewForAttachments.DataBind();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }

    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewBrokeragesForMarketingAgent.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }

    protected void GridViewItems_RowUpdated(object sender, GridViewUpdatedEventArgs e) {
        
    }

    protected void GridViewItems_RowUpdating(object sender, GridViewUpdateEventArgs e) {
        FakeBind(true, true);
    }

    protected void FakeBind(bool isddlInsuranceTypesForChange, bool isddlInsuranceSubTypesForChange) {
        if (isddlInsuranceTypesForChange) {
            DropDownList ddlInsuranceTypes = (DropDownList)DetailsViewItems.FindControl("ddlInsuranceType");
            //FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceSubTypes);
            ddlInsuranceTypes.DataBind();
        }
        if (isddlInsuranceSubTypesForChange) {
            DropDownList ddlInsuranceSubTypes = (DropDownList)DetailsViewItems.FindControl("ddlInsuranceSubType");
            FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceSubTypes);
            ddlInsuranceSubTypes.DataBind();
        }
    }
    protected void GridViewItems_RowDeleted(object sender, GridViewDeletedEventArgs e) {
        
    }

    protected void odsGridView_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {

    }
    protected void btnNewItem_Click(object sender, EventArgs e) {
        int offerID = int.Parse(tbUserID.Text.Trim());

        mvBrokerages.SetActiveView(viewItemsEdit);
        //mvOfferItems.SetActiveView(viewOfferItemsEdit);
        DetailsViewItems.ChangeMode(DetailsViewMode.Insert);
        //    if (oItemList.Count > 0) {

        //        // ControlCollection c = DetailsViewOfferItems.Controls;
        //        ObjectDataSource odsInusranceType = DetailsViewOfferItems.FindControl("odsInsuranceTypes") as ObjectDataSource;
        //        odsInusranceType.SelectParameters.Clear();
        //        odsInusranceType.SelectParameters.Add("orderItemID", orderItem.ID.ToString());

        //        ObjectDataSource odsInusranceSubType = DetailsViewOfferItems.FindControl("odsInsuranceSubTypes") as ObjectDataSource;
        //        odsInusranceSubType.SelectParameters.Clear();
        //        odsInusranceSubType.SelectParameters.Add("orderItemID", orderItem.ID.ToString());
        //        odsInusranceSubType.SelectParameters.Add("insuranceTypeID", orderItem.InsuranceSubType.InsuranceTypeID.ToString());

        //        FakeBind(true, false, false, false);
        //        FakeBind(false, true, false, false);
        //        DropDownList ddlInsuranceCompanies = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceCompanies");
        //        FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompanies);
        //        ddlInsuranceCompanies.DataBind();
        //        mvOfferItems.SetActiveView(viewOfferItemsEdit);
        //        DetailsViewOfferItems.ChangeMode(DetailsViewMode.Insert);
        //    }
        //} else {
        //    mvOfferItems.SetActiveView(viewOfferItemsEdit);
        //    DetailsViewOfferItems.ChangeMode(DetailsViewMode.Insert);
        //    //  TextBox tbOfferNumber = (TextBox)DetailsViewOfferItems.FindControl("tbOfferNumber");
        //    //   tbOfferNumber.Text = Offer.Get(Convert.ToInt32(GXGridView1.SelectedDataKey.Value)).OfferNumber;
        //}
    }
    protected void DetailsViewDataSourceItems_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void DetailsViewDataSourceItems_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        
    }

    protected void DetailsViewItems_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
       
        e.Values["UserID"] = int.Parse(tbUserID.Text.Trim());
        e.Values["IsActive"] = true;
    }
    protected void DetailsViewItems_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsViewItems_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            //mvOfferItems.SetActiveView(viewOfferItemsGrid);
        }
    }
    protected void DetailsViewItems_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            int offerID = Convert.ToInt32(e.Values["UserID"]);
            //decimal offerItemCost = Convert.ToDecimal(e.Values["Cost"]);
            //Offer o = Offer.Get(offerID);
            //o.Cost += offerItemCost;
            //Offer.Table.Context.SubmitChanges();
            //odsOfferPreview.SelectParameters.Clear();
            //odsOfferPreview.SelectParameters.Add("id", offerID.ToString());
            //dvOfferPreview.DataBind();
            odsGridViewItems.SelectParameters.Clear();
            odsGridViewItems.SelectParameters.Add("userID", offerID.ToString());
            GridViewItems.DataBind();
            mvBrokerages.SetActiveView(viewItemsGrid);
        }
    }
    protected void ddlCompanies_SelectedIndexChanged(object sender, EventArgs e) {
        FakeBind(true, true);
    }
    protected void ddlInsuranceSubType_SelectedIndexChanged(object sender, EventArgs e) {
      
    }

    protected void ddlInsuranceType_SelectedIndexChanged(object sender, EventArgs e) {
        FakeBind(false, true);
    }
}
