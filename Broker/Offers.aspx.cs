using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Utility;
using System.IO;
using Broker.Controllers;
using Broker.Controllers.ReportControllers;

public partial class Broker_Offers : AuthenticationPage {

    public int GXGridView1SelectedValue
    {
        get
        {
            if (ViewState["GXGridView1SelectedValue"] != null)
            {
                return int.Parse(ViewState["GXGridView1SelectedValue"].ToString());
            } else
            {
                return 0;
            }
        }
        set
        {
            ViewState["GXGridView1SelectedValue"] = value;
        }
    }


    public static string OfferAttachmens_Directory {
        get {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\Offers";
            return Path;
        }
    }


    public int ClientID {
        get {
            return (ViewState["ClientID"] != null) ? (int)ViewState["ClientID"] : 0;
        }
        set {
            ViewState["ClientID"] = value;
        }
    }
    public int OwnerID {
        get {
            return (ViewState["OwnerID"] != null) ? (int)ViewState["OwnerID"] : 0;
        }
        set {
            ViewState["OwnerID"] = value;
        }
    }
    public int OrderID {
        get {
            return (ViewState["OrderID"] != null) ? (int)ViewState["OrderID"] : 0;
        }
        set {
            ViewState["OrderID"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewOffer.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            int offerID = 0;
            if (GXGridView1.SelectedDataKey != null)
            {
                offerID = Convert.ToInt32(GXGridView1.SelectedValue);
            } else
            {
                offerID = GXGridView1SelectedValue;
            }
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeniActive";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOfferItems.CssClass = "stavki";
            PrintOffers.PrintOffer(Offer.Get(offerID));
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {

            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }

            mvMain.SetActiveView(viewEdit);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOfferItems.CssClass = "stavki";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnAttachments.CssClass = "dokumenti";
        btnOfferItems.CssClass = "stavki";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        GXGridView1.SelectedIndex = -1;
        mvMain.SetActiveView(viewEdit);
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        DetailsView1.DataBind();
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnOfferItems.CssClass = "stavki";
        TextBox tbOfferNumber = (TextBox)DetailsView1.FindControl("tbOfferNumber");
        tbOfferNumber.Text = CodeGenerator.OfferCodeGenerator();
    }
    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnOfferItems.CssClass = "stavki";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewOffer.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnEdit.Enabled = true;
            btnOfferItems.Enabled = true;
            btnAttachments.Enabled = true;
            btnPrint.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
            GridViewOfferItems.DataBind();
            dvOfferPreview.DataBind();
            dvOfferPreviewForAttachments.DataBind();
        }
        if (e.CommandName == "DoubleClick") {
            btnEdit.Enabled = true;
            btnOfferItems.Enabled = true;
            btnAttachments.Enabled = true;
            btnPrint.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            mvMain.SetActiveView(viewOfferItems);
            mvOfferItems.SetActiveView(viewOfferItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOfferItems.CssClass = "stavki_Active";
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
            GridViewOfferItems.DataBind();
            dvOfferPreview.DataBind();
            dvOfferPreviewForAttachments.DataBind();
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnOfferItems.CssClass = "stavki";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewOffer.SelectFilterCountCached(FilterControl1.FCFilterArgument);
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
            //btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOfferItems.CssClass = "stavki";
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewOffer.SelectCountCached();
            GXGridView1.DataBind();

            string offerNumber = e.Values["OfferNumber"] as string;
            Offer o = Offer.GetByNumber(offerNumber);

            string DataKeyValue = o.ID.ToString();

            int intSelectedIndex = 0;
            int intPageIndex = 0;
            int intGridViewPages = GXGridView1.PageCount;

            // Loop thru each page in the GridView
            for (int intPage = 0; intPage < intGridViewPages; intPage++) {
                // Set the current GridView page
                GXGridView1.PageIndex = intPage;
                // Bind the GridView to the current page
                GXGridView1.DataBind();
                // Loop thru each DataKey in the GridView
                for (int i = 0; i < GXGridView1.DataKeys.Count; i++) {
                    if (Convert.ToString(GXGridView1.DataKeys[i].Value) == DataKeyValue) {
                        // If it is a match set the variables and exit
                        intSelectedIndex = i;
                        intPageIndex = intPage;
                        break;
                    }
                }
            }

            // Set the GridView to the values found
            GXGridView1.PageIndex = intPageIndex;
            GXGridView1.SelectedIndex = intSelectedIndex;
            GXGridView1.DataBind();

            int GXgvSelectedValue = o.ID;
            GXGridView1SelectedValue = GXgvSelectedValue;
            odsOfferPreview.SelectParameters.Clear();
            odsOfferPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
            dvOfferPreview.DataBind();
            odsGridViewOfferItems.SelectParameters.Clear();
            odsGridViewOfferItems.SelectParameters.Add("offerID", GXgvSelectedValue.ToString());
            GridViewOfferItems.DataBind();

            mvMain.SetActiveView(viewOfferItems);
            mvOfferItems.SetActiveView(viewOfferItemsGrid);
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewOffer.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewOffer.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        if (OrderID > 0) {
            e.Values["OrderID"] = OrderID;
        }
        e.Values["Cost"] = (decimal)0;
        e.Values["ClientID"] = ClientID;
        e.Values["OwnerID"] = OwnerID;
        e.Values["UserID"] = this.PageUser.ID;
        e.Values["OfferDate"] = DateTime.Today;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Offer o = e.InputParameters["entityToInsert"] as Offer;
        //TextBox tbOrderNumber = DetailsView1.FindControl("tbOrderNumber") as TextBox;
        // o.TestBeforeInsert(tbOrderNumber.Text);
        o.TestBeforeInsert();

    }

    protected void btnDelete_Click1(object sender, EventArgs e) {

        GXGridView1.TotalRecords = ViewOffer.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void btnSearchOrderNumber_Click(object sender, EventArgs e) {
        TextBox tbOrderNumber = DetailsView1.FindControl("tbOrderNumber") as TextBox;
        Broker.DataAccess.Order o = Broker.DataAccess.Order.GetByOrderNumber(tbOrderNumber.Text);

        TextBox tbClientEMBG = DetailsView1.FindControl("tbClientEMBG") as TextBox;
        TextBox tbClientName = DetailsView1.FindControl("tbClientName") as TextBox;
        TextBox tbOwnerEMBG = DetailsView1.FindControl("tbOwnerEMBG") as TextBox;
        TextBox tbOwnerName = DetailsView1.FindControl("tbOwnerName") as TextBox;

        if (o != null) {
            OrderID = o.ID;
            ClientID = o.ClientID;
            OwnerID = o.OwnerID;
            tbClientEMBG.Text = o.Client.EMBG;
            tbClientEMBG.ReadOnly = true;
            tbClientName.Text = o.Client.Name;
            tbClientName.ReadOnly = true;
            tbOwnerEMBG.Text = o.Client1.EMBG;
            tbOwnerEMBG.ReadOnly = true;
            tbOwnerName.Text = o.Client1.Name;
            tbOwnerName.ReadOnly = true;
        } else {
            tbClientEMBG.Text = string.Empty;
            tbClientName.Text = string.Empty;
            tbOwnerEMBG.Text = string.Empty;
            tbOwnerName.Text = string.Empty;
        }
    }

    protected void btnSearchOrdernumber_Click(object sender, EventArgs e) {
        DropDownList ddlOrderItems = DetailsViewOfferItems.FindControl("ddlOrderItems") as DropDownList;
        ddlOrderItems.DataBind();
        FakeBind(false, true, true, true);

    }

    protected void btnSearchClientEMBG_Click(object sender, EventArgs e) {
        TextBox tbEmbg = DetailsView1.FindControl("tbClientEMBG") as TextBox;
        Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(tbEmbg.Text);
        if (c != null) {
            ViewState["ClientID"] = c.ID;
            TextBox tbName = DetailsView1.FindControl("tbClientName") as TextBox;
            tbName.Text = c.Name;
            Panel clientPanel = DetailsView1.FindControl("pnlClient") as Panel;
            clientPanel.Visible = false;

        } else {
            Panel clientPanel = DetailsView1.FindControl("pnlClient") as Panel;
            clientPanel.Visible = true;
            DetailsView dvClient = DetailsView1.FindControl("ClientDetailsView") as DetailsView;
            TextBox tbClientEMBG = dvClient.FindControl("tbEMBG") as TextBox;
            tbClientEMBG.Text = tbEmbg.Text;
        }
    }
    protected void btnSearchOwnerEMBG_Click(object sender, EventArgs e) {
        TextBox tbEmbg = DetailsView1.FindControl("tbOwnerEMBG") as TextBox;
        Broker.DataAccess.Client o = Broker.DataAccess.Client.GetByEmbg(tbEmbg.Text);
        if (o != null) {
            ViewState["OwnerID"] = o.ID;
            TextBox tbName = DetailsView1.FindControl("tbOwnerName") as TextBox;
            tbName.Text = o.Name;
            Panel ownerPanel = DetailsView1.FindControl("pnlOwner") as Panel;
            ownerPanel.Visible = false;
        } else {
            Panel ownerPanel = DetailsView1.FindControl("pnlOwner") as Panel;
            ownerPanel.Visible = true;
            DetailsView dvClient = DetailsView1.FindControl("OwnerDetailsView") as DetailsView;
            TextBox tbOwnerEMBG = dvClient.FindControl("tbEMBG") as TextBox;
            tbOwnerEMBG.Text = tbEmbg.Text;
        }
    }
    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        //e.InputParameters.Add("ClientID", int.Parse(GXGridView1.SelectedDataKey["ClientID"].ToString()));
        //e.InputParameters.Add("BankID", int.Parse(GXGridView1.SelectedDataKey["BankID"].ToString()));
    }

    protected void ClientDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            TextBox tbClientName = DetailsView1.FindControl("ClientDetailsView").FindControl("tbName") as TextBox;
            TextBox tbName = DetailsView1.FindControl("tbClientName") as TextBox;
            tbName.Text = tbClientName.Text;
            Panel pnl = DetailsView1.FindControl("pnlClient") as Panel;
            pnl.Visible = false;
            string embg = e.Values["EMBG"].ToString();
            Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(embg);
            ViewState["ClientID"] = c.ID;
        }
    }

    protected void ClientDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewOffer.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void ClientDetailsView_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void ClientDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void ClientDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        // e.Values["IsActive"] = true;
    }

    protected void ClientdvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void ClientdvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void ClientdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void ClientdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        if (Validator.GetByName("EmbgInsertValidator").IsActive) {
            c.ValidateClient();
        }
    }
    protected void ClientDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void OwnerDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            TextBox tbClientName = DetailsView1.FindControl("OwnerDetailsView").FindControl("tbName") as TextBox;
            TextBox tbName = DetailsView1.FindControl("tbOwnerName") as TextBox;
            tbName.Text = tbClientName.Text;
            Panel pnl = DetailsView1.FindControl("pnlOwner") as Panel;
            pnl.Visible = false;
            string embg = e.Values["EMBG"].ToString();
            Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(embg);
            ViewState["OwnerID"] = c.ID;
        }
    }

    protected void OwnerDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void OwnerDetailsView_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void OwnerDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void OwnerDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        // e.Values["IsActive"] = true;
    }

    protected void OwnerdvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["newEntity"] as Broker.DataAccess.Client;
    }

    protected void OwnerdvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void OwnerdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void OwnerdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        if (Validator.GetByName("EmbgInsertValidator").IsActive) {
            c.ValidateOwner();
        }
    }
    protected void OwnerDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnOfferItems_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsOfferPreview.SelectParameters.Clear();
                odsOfferPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvOfferPreview.DataBind();
                odsGridViewOfferItems.SelectParameters.Clear();
                odsGridViewOfferItems.SelectParameters.Add("offerID", GXgvSelectedValue.ToString());
                GridViewOfferItems.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsOfferPreview.SelectParameters.Clear();
                odsOfferPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvOfferPreview.DataBind();
                odsGridViewOfferItems.SelectParameters.Clear();
                odsGridViewOfferItems.SelectParameters.Add("offerID", GXgvSelectedValue.ToString());
                GridViewOfferItems.DataBind();
            }
            mvMain.SetActiveView(viewOfferItems);
            mvOfferItems.SetActiveView(viewOfferItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOfferItems.CssClass = "stavki_Active";
        }
    }
    protected void btnNewOfferItem_Click(object sender, EventArgs e) {
        int offerID = 0;
        if (GXGridView1.SelectedValue != null)
        {
            offerID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        } else
        {
            offerID = GXGridView1SelectedValue;
        }
        Offer o = Offer.Get(offerID);
        
        if (o.OrderID != null) {
            TextBox tbOrderNumber = DetailsViewOfferItems.FindControl("tbOrderNumber") as TextBox;
            tbOrderNumber.Text = o.Order.OrderNumber;
            DropDownList ddlOrderItems = DetailsViewOfferItems.FindControl("ddlOrderItems") as DropDownList;
            

            ddlOrderItems.DataBind();

            Broker.DataAccess.OrderItem orderItem = Broker.DataAccess.OrderItem.Get(int.Parse(ddlOrderItems.SelectedValue));

            List<Broker.DataAccess.OrderItem> oItemList = Broker.DataAccess.OrderItem.GetByOrderID((int)o.OrderID);
            if (oItemList.Count > 0) {

               // ControlCollection c = DetailsViewOfferItems.Controls;
                ObjectDataSource odsInusranceType = DetailsViewOfferItems.FindControl("odsInsuranceTypes") as ObjectDataSource;
                odsInusranceType.SelectParameters.Clear();
                odsInusranceType.SelectParameters.Add("orderItemID", orderItem.ID.ToString());

                ObjectDataSource odsInusranceSubType = DetailsViewOfferItems.FindControl("odsInsuranceSubTypes") as ObjectDataSource;
                odsInusranceSubType.SelectParameters.Clear();
                odsInusranceSubType.SelectParameters.Add("orderItemID", orderItem.ID.ToString());
                odsInusranceSubType.SelectParameters.Add("insuranceTypeID", orderItem.InsuranceSubType.InsuranceTypeID.ToString());

                FakeBind(true, false, false, false);
                FakeBind(false, true, false, false);
                DropDownList ddlInsuranceCompanies = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceCompanies");
                FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompanies);
                ddlInsuranceCompanies.DataBind();
                mvOfferItems.SetActiveView(viewOfferItemsEdit);
                DetailsViewOfferItems.ChangeMode(DetailsViewMode.Insert);
            }
        } else {
            mvOfferItems.SetActiveView(viewOfferItemsEdit);
            DetailsViewOfferItems.ChangeMode(DetailsViewMode.Insert);
          //  TextBox tbOfferNumber = (TextBox)DetailsViewOfferItems.FindControl("tbOfferNumber");
         //   tbOfferNumber.Text = Offer.Get(Convert.ToInt32(GXGridView1.SelectedDataKey.Value)).OfferNumber;
        }
    }
    protected void GridViewOfferItems_RowUpdating(object sender, GridViewUpdateEventArgs e) {
        //string oldCostString = e.OldValues["Cost"].ToString();
        //string newCostString = e.NewValues["Cost"].ToString();
        //newCostString = newCostString.Replace(',', '.');
        //oldCostString = oldCostString.Replace(',', '.');
        //e.OldValues["Cost"] = Convert.ToDecimal(oldCostString);
        //e.NewValues["Cost"] = Convert.ToDecimal(newCostString);
        FakeBind(true, true, false, true);
    }

    protected void FakeBind(bool isddlInsuranceTypesForChange, bool isddlInsuranceSubTypesForChange, bool isddlDealsForChange, bool isddlBrokeragesForChange) {
        if (isddlInsuranceTypesForChange) {
            DropDownList ddlInsuranceTypes = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceTypes");
            //FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceSubTypes);
            ddlInsuranceTypes.DataBind();
        }
        if (isddlInsuranceSubTypesForChange) {
            DropDownList ddlInsuranceSubTypes = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceSubTypes");
            FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceSubTypes);
            ddlInsuranceSubTypes.DataBind();
        }
        if (isddlDealsForChange) {
            DropDownList ddlDeals = (DropDownList)DetailsViewOfferItems.FindControl("ddlDeals");
            FakeBinder.SimulateBindingContext(this.Page, ddlDeals);
            ddlDeals.DataBind();
        }
        if (isddlBrokeragesForChange) {
            //DropDownList ddlBrokerages = (DropDownList)DetailsViewOfferItems.FindControl("ddlBrokerages");
            //FakeBinder.SimulateBindingContext(this.Page, ddlBrokerages);
            //ddlBrokerages.DataBind();
        }
    }
    protected void odsGridView_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {

    }
    protected void DetailsViewOfferItems_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvOfferItems.SetActiveView(viewOfferItemsGrid);
        }
    }
    protected void DetailsViewOfferItems_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            int offerID = Convert.ToInt32(e.Values["OfferID"]);
            decimal offerItemCost = Convert.ToDecimal(e.Values["Cost"]);
            Offer o = Offer.Get(offerID);
            o.Cost += offerItemCost;
            Offer.Table.Context.SubmitChanges();
            odsOfferPreview.SelectParameters.Clear();
            odsOfferPreview.SelectParameters.Add("id", offerID.ToString());
            dvOfferPreview.DataBind();
            odsGridViewOfferItems.SelectParameters.Clear();
            odsGridViewOfferItems.SelectParameters.Add("offerID", offerID.ToString());
            GridViewOfferItems.DataBind();
            mvOfferItems.SetActiveView(viewOfferItemsGrid);
        }
    }
    protected void DetailsViewOfferItems_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        int offerID = 0;
        if (GXGridView1.SelectedValue != null)
        {
            offerID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value); ;
        } else
        {
            offerID = GXGridView1SelectedValue;
        }
        e.Values["OfferID"] = offerID;
    }
    protected void DetailsViewOfferItems_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }


    protected void ddlInsuranceTypes_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList insuranceType = (DropDownList)sender;
        DropDownList ddlInsuranceSubTypes = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceSubTypes");
        FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceSubTypes);
        ddlInsuranceSubTypes.DataBind();
        DropDownList ddlInsuranceCompanies = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceCompanies");
        FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompanies);
        ddlInsuranceCompanies.DataBind();
       // FakeBind(false, true, true, false);

    }

    protected void ddlOrderItems_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlOrderItems = (DropDownList)sender;
        Broker.DataAccess.OrderItem oi = Broker.DataAccess.OrderItem.Get(Convert.ToInt32(ddlOrderItems.SelectedValue));
        if (oi != null) {
            ObjectDataSource odsInusranceType = DetailsViewOfferItems.FindControl("odsInsuranceTypes") as ObjectDataSource;
            odsInusranceType.SelectParameters.Clear();
            odsInusranceType.SelectParameters.Add("orderItemID", ddlOrderItems.SelectedValue);

            ObjectDataSource odsInusranceSubType = DetailsViewOfferItems.FindControl("odsInsuranceSubTypes") as ObjectDataSource;
            odsInusranceSubType.SelectParameters.Clear();
            odsInusranceSubType.SelectParameters.Add("orderItemID", oi.ID.ToString());
            odsInusranceSubType.SelectParameters.Add("insuranceTypeID", oi.InsuranceSubType.InsuranceTypeID.ToString());
            
            //FakeBind(false, false, true, false);
            //ddlInsuranceSubTypes.SelectedValue = oi.InsuranceSubTypeID.ToString();
            //FakeBind(false, true, true, false);
            //DropDownList ddlInsuranceSubTypes = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceSubTypes");
            //FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceSubTypes);
            //ddlInsuranceSubTypes.DataBind();
            FakeBind(true, true, false, false);
            DropDownList ddlInsuranceCompanies = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceCompanies");
            FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompanies);
            ddlInsuranceCompanies.DataBind();
        }
    }

    protected void ddlInsuranceCompanies_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceCompanies = (DropDownList)sender;
      //  FakeBind(true, true, true, true);

    }
    protected void ddlInsuranceSubTypes_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceSubTypes = (DropDownList)sender;
        //FakeBind(false, false, true, false);
        DropDownList ddlInsuranceCompanies = (DropDownList)DetailsViewOfferItems.FindControl("ddlInsuranceCompanies");
        FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompanies);
        ddlInsuranceCompanies.DataBind();
    }
    protected void ddlDeals_SelectedIndexChanged(object sender, EventArgs e) {
        //DropDownList ddlDeals = (DropDownList)sender;
        //FakeBind(false, false, false, true);

    }

    protected void DetailsViewDataSourceOfferItems_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void DetailsViewDataSourceOfferItems_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {

    }
    protected void btnAttachments_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsOfferPreviewForAttachments.SelectParameters.Clear();
                odsOfferPreviewForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvOfferPreviewForAttachments.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsOfferPreviewForAttachments.SelectParameters.Clear();
                odsOfferPreviewForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvOfferPreviewForAttachments.DataBind();
            }
            mvMain.SetActiveView(viewAttachments);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti_Active";
            btnOfferItems.CssClass = "stavki";
            GenerateAllFiles();
        }
    }

    protected void btnAddAttachment_Click(object sender, EventArgs e) {
        if (FileUpload1.HasFile) {
            try {
                string offerID = string.Empty;
                if (GXGridView1.SelectedValue != null)
                {
                    offerID = GXGridView1.SelectedDataKey.Value.ToString();
                } else
                {
                    offerID = GXGridView1SelectedValue.ToString();
                }
                string destinationPath = OfferAttachmens_Directory + @"\" + offerID;
                if (!Directory.Exists(destinationPath)) {
                    Directory.CreateDirectory(destinationPath);
                }
                string path = destinationPath + @"\" + FileUpload1.FileName;
                FileUpload1.SaveAs(path);
                Label1.Text = "Датотека: " +
                     FileUpload1.PostedFile.FileName + "<br>" +
                     FileUpload1.PostedFile.ContentLength + " B<br>" +
                     "Тип на датотека: " +
                     FileUpload1.PostedFile.ContentType;

                //int len = FileUpload1.PostedFile.ContentLength;

                Attachment a = new Attachment();
                a.DocumentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.PONUDA).ID;
                a.DocumentID = Convert.ToInt32(offerID);
                a.RelativePath = path;
                a.Insert();


            } catch (Exception ex) {
                Label1.Text = ex.Message;
            }
        } else {
            Label1.Text = "Немате избрано валидна датотека.";
        }
        GenerateAllFiles();
    }
    
    void GenerateAllFiles() {
        string offerID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            offerID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            offerID = GXGridView1SelectedValue.ToString();
        }
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.PONUDA).ID && a.DocumentID == int.Parse(offerID)).ToList();
        if (listAttachments.Count > 0) {
            FileInfo[] fileInfo = new FileInfo[listAttachments.Count];
            int i = 0;
            foreach (Attachment at in listAttachments) {
                fileInfo[i] = new FileInfo(at.RelativePath);
                i++;
            }
            dataGridFiles.DataSource = fileInfo;
           
        } else {
            Label1.Text = "Нема прикачено документи.";
        }
        dataGridFiles.DataBind();
    }

    protected void btnFile_Click(object sender, EventArgs e) {
        LinkButton button = (LinkButton)sender;
        string offerID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            offerID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            offerID = GXGridView1SelectedValue.ToString();
        }
        string destinationPath = OfferAttachmens_Directory + @"\" + offerID;
        string file_path = destinationPath + @"\" + button.Text;
        FileInfo fi = new FileInfo(file_path);
        FileStream fs = fi.OpenRead();
        byte[] byte_get = new byte[fs.Length];
        fs.Read(byte_get, 0, byte_get.Length);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ContentType = AttachmentController.getMimeType(file_path);
        HttpContext.Current.Response.Charset = string.Empty;
        HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
        HttpContext.Current.Response.AddHeader("Content-Disposition:", "attachment; filename=" + button.Text);
        HttpContext.Current.Response.OutputStream.Write(byte_get, 0, byte_get.Length);
        HttpContext.Current.Response.OutputStream.Flush();
        HttpContext.Current.Response.OutputStream.Close();
        HttpContext.Current.Response.End();
    }


    protected void dvOfferPreview_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void dvOfferPreview_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void odsOfferPreview_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }
    protected void dvOfferPreviewForAttachments_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void dvOfferPreviewForAttachments_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void odsOfferPreviewForAttachments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }
    protected void GridViewOfferItems_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        int offerID = Convert.ToInt32(e.Values["OfferID"]);
        decimal offerItemCost = Convert.ToDecimal(e.Values["Cost"]);
        Offer o = Offer.Get(offerID);
        o.Cost -= offerItemCost;
        Offer.Table.Context.SubmitChanges();
        dvOfferPreview.DataBind();
    }
    protected void GridViewOfferItems_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        //int offerID = Convert.ToInt32(e.NewValues["OfferID"]);
        //decimal offerItemOldCost = Convert.ToDecimal(e.OldValues["Cost"]);
        //decimal offerItemNewCost = Convert.ToDecimal(e.NewValues["Cost"]);
        //Offer.UpdateCurrentCost(offerItemNewCost, offerItemOldCost, offerID);
        ////Offer o = Offer.Get(offerID);
        ////o.Cost -= offerItemOldCost;
        ////o.Cost += offerItemNewCost;
        ////Offer.Table.Context.SubmitChanges();
        dvOfferPreview.DataBind();
    }

    protected void dataGridFiles_SelectedIndexChanged(object sender, EventArgs e) {
        GridViewRow gvr = dataGridFiles.SelectedRow;
        if(gvr!=null){
            int documentID = 0;
            if (GXGridView1.SelectedValue != null)
            {
                documentID = Convert.ToInt32(GXGridView1.SelectedValue);
            } else
            {
                documentID = GXGridView1SelectedValue;
            }
            int documentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.PONUDA).ID;
            List<Attachment> listAttachments = Attachment.GetByDocumentTypeIDAndDocumentID(documentTypeID, documentID);
            TableCell tc = gvr.Cells[0];
            LinkButton linkButton = tc.FindControl("btnFile") as LinkButton;
            string attachmentName = linkButton.Text;
            foreach (Attachment a in listAttachments) {
                string[] splitString = a.RelativePath.Split('\\');
                if (splitString[splitString.Length - 1] == attachmentName) {
                    File.Delete(a.RelativePath);
                    Attachment.Table.DeleteOnSubmit(a);
                    Attachment.Table.Context.SubmitChanges();
                }
            }
        }
        GenerateAllFiles();
    }
}
