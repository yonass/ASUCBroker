using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using System.IO;
using System.Drawing;
using Broker.Controllers.UserManagement;
using Broker.Controllers;
using System.Reflection;
using System.Collections.Specialized;

public partial class Broker_Orders : AuthenticationPage
{
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

    public static string OrderAttachmens_Directory
    {
        get
        {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\Orders";
            return Path;
        }
    }


    private int _clientID;
    public int ClientID
    {
        get
        {
            return (ViewState["ClientID"] != null) ? (int)ViewState["ClientID"] : 0;
        }
        set
        {
            ViewState["ClientID"] = value;
        }
    }
    public int OwnerID
    {
        get
        {
            return (ViewState["OwnerID"] != null) ? (int)ViewState["OwnerID"] : 0;
        }
        set
        {
            ViewState["OwnerID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GXGridView1.TotalRecords = OrdersView.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
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
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOrderItems.CssClass = "stavki";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //  btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnAttachments.CssClass = "dokumenti";
        btnOrderItems.CssClass = "stavki";
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        GXGridView1.SelectedIndex = -1;
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        DetailsView1.DataBind();
        mvMain.SetActiveView(viewEdit);
        TextBox tbOrderNumber = (TextBox)DetailsView1.FindControl("tbOrderNumber");
        tbOrderNumber.Text = CodeGenerator.OrderCodeGenerator();
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        //  btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnOrderItems.CssClass = "stavki";

    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        GXGridView1.DataBind();
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //   btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnOrderItems.CssClass = "stavki";
    }
    protected void SearchControl1_Search(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = OrdersView.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SingleClick")
        {
            btnEdit.Enabled = true;
            btnOrderItems.Enabled = true;
            btnAttachments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsViewOrderForAttachments.DataBind();
            dvOrderPreview.DataBind();
            GridViewOrderItems.DataBind();
        }
        if (e.CommandName == "DoubleClick")
        {
            btnEdit.Enabled = true;
            btnOrderItems.Enabled = true;
            btnAttachments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            mvMain.SetActiveView(viewOrderItems);
            mvOrderItems.SetActiveView(viewOrderItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOrderItems.CssClass = "stavki_Active";
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsViewOrderForAttachments.DataBind();
            dvOrderPreview.DataBind();
            GridViewOrderItems.DataBind();
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //  btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnOrderItems.CssClass = "stavki";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = OrdersView.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            mvMain.SetActiveView(viewEdit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            //      btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOrderItems.CssClass = "stavki";
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
            GXGridView1.TotalRecords = OrdersView.SelectCountCached();
            GXGridView1.DataBind();
            //mvMain.SetActiveView(viewGrid);

            string orderNumber = e.Values["OrderNumber"] as string;
            Broker.DataAccess.Order o = Broker.DataAccess.Order.GetByOrderNumber(orderNumber);

            string DataKeyValue = o.ID.ToString();

            int intSelectedIndex = 0;
            int intPageIndex = 0;
            int intGridViewPages = GXGridView1.PageCount;

            // Loop thru each page in the GridView
            for (int intPage = 0; intPage < intGridViewPages; intPage++)
            {
                // Set the current GridView page
                GXGridView1.PageIndex = intPage;
                // Bind the GridView to the current page
                GXGridView1.DataBind();
                // Loop thru each DataKey in the GridView
                for (int i = 0; i < GXGridView1.DataKeys.Count; i++)
                {
                    if (Convert.ToString(GXGridView1.DataKeys[i].Value) == DataKeyValue)
                    {
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
            odsOrderPreview.SelectParameters.Clear();
            odsOrderPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
            dvOrderPreview.DataBind();
            odsGridViewOrderItems.SelectParameters.Clear();
            odsGridViewOrderItems.SelectParameters.Add("orderID", GXgvSelectedValue.ToString());
            GridViewOrderItems.DataBind();
            mvMain.SetActiveView(viewOrderItems);
            mvOrderItems.SetActiveView(viewOrderItemsGrid);

        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
            GXGridView1.TotalRecords = OrdersView.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GXGridView1.TotalRecords = OrdersView.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        //e.Values["IsActive"] = true;
        e.Values["ClientID"] = ClientID;
        e.Values["OrderDate"] = DateTime.Today;
        e.Values["OwnerID"] = OwnerID;
        e.Values["UserID"] = this.PageUser.ID;


    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Broker.DataAccess.Order o = e.InputParameters["entityToInsert"] as Broker.DataAccess.Order;
        o.ValidateNumber();

        //ca.ValidateAccount();
    }

    protected void btnDelete_Click1(object sender, EventArgs e)
    {
        ClientAccount ca = ClientAccount.GetByClientAndBank(int.Parse(GXGridView1.SelectedDataKey["ClientID"].ToString()), int.Parse(GXGridView1.SelectedDataKey["BankID"].ToString()));
        ca.IsActive = false;
        ClientAccount.Table.Context.SubmitChanges();
        GXGridView1.TotalRecords = OrdersView.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void btnSearchClientEMBG_Click(object sender, EventArgs e)
    {
        TextBox tbEmbg = DetailsView1.FindControl("tbClientEMBG") as TextBox;
        Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(tbEmbg.Text);
        if (c != null)
        {
            ViewState["ClientID"] = c.ID;
            TextBox tbName = DetailsView1.FindControl("tbClientName") as TextBox;
            tbName.Text = c.Name;
            Panel clientPanel = DetailsView1.FindControl("pnlClient") as Panel;
            clientPanel.Visible = false;

        } else
        {
            Panel clientPanel = DetailsView1.FindControl("pnlClient") as Panel;
            clientPanel.Visible = true;
            DetailsView dv1 = DetailsView1.FindControl("ClientDetailsView") as DetailsView;
            TextBox tbClientEmbg1 = dv1.FindControl("tbEMBG") as TextBox;
            tbClientEmbg1.Text = tbEmbg.Text;
        }
    }
    protected void btnSearchOwnerEMBG_Click(object sender, EventArgs e)
    {
        TextBox tbEmbg = DetailsView1.FindControl("tbOwnerEMBG") as TextBox;
        Broker.DataAccess.Client o = Broker.DataAccess.Client.GetByEmbg(tbEmbg.Text);
        if (o != null)
        {
            ViewState["OwnerID"] = o.ID;
            TextBox tbName = DetailsView1.FindControl("tbOwnerName") as TextBox;
            tbName.Text = o.Name;
            Panel ownerPanel = DetailsView1.FindControl("pnlOwner") as Panel;
            ownerPanel.Visible = false;
        } else
        {
            Panel ownerPanel = DetailsView1.FindControl("pnlOwner") as Panel;
            ownerPanel.Visible = true;
            DetailsView dv1 = DetailsView1.FindControl("OwnerDetailsView") as DetailsView;
            TextBox tbOwnerEMBG = dv1.FindControl("tbEMBG") as TextBox;
            tbOwnerEMBG.Text = tbEmbg.Text;
        }
    }
    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //e.InputParameters.Add("ClientID", int.Parse(GXGridView1.SelectedDataKey["ClientID"].ToString()));
        //e.InputParameters.Add("BankID", int.Parse(GXGridView1.SelectedDataKey["BankID"].ToString()));
    }

    protected void ClientDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
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

    protected void ClientDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
            GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void ClientDetailsView_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void ClientDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void ClientDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        // e.Values["IsActive"] = true;
    }

    protected void ClientdvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void ClientdvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void ClientdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void ClientdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        if (Validator.GetByName("EmbgInsertValidator").IsActive)
        {
            c.ValidateClient();
        }
    }
    protected void ClientDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void OwnerDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
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

    protected void OwnerDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
            GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void OwnerDetailsView_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void OwnerDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void OwnerDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        // e.Values["IsActive"] = true;
    }

    protected void OwnerdvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void OwnerdvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void OwnerdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void OwnerdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        if (Validator.GetByName("EmbgInsertValidator").IsActive)
        {
            c.ValidateOwner();
        }
    }
    protected void OwnerDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnOrderItems_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedDataKey != null)
            {

                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsOrderPreview.SelectParameters.Clear();
                odsOrderPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvOrderPreview.DataBind();
                odsGridViewOrderItems.SelectParameters.Clear();
                odsGridViewOrderItems.SelectParameters.Add("orderID", GXgvSelectedValue.ToString());
                GridViewOrderItems.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsOrderPreview.SelectParameters.Clear();
                odsOrderPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvOrderPreview.DataBind();
                odsGridViewOrderItems.SelectParameters.Clear();
                odsGridViewOrderItems.SelectParameters.Add("orderID", GXgvSelectedValue.ToString());
                GridViewOrderItems.DataBind();
            }

            mvMain.SetActiveView(viewOrderItems);
            mvOrderItems.SetActiveView(viewOrderItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnOrderItems.CssClass = "stavki_Active";
        }
    }

    protected void DetailsViewOrderItems_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            mvMain.SetActiveView(viewOrderItems);
            mvOrderItems.SetActiveView(viewOrderItemsGrid);
        }
    }
    protected void DetailsViewOrderItems_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else
        {
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsGridViewOrderItems.SelectParameters.Clear();
                odsGridViewOrderItems.SelectParameters.Add("orderID", GXgvSelectedValue.ToString());
                GridViewOrderItems.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsGridViewOrderItems.SelectParameters.Clear();
                odsGridViewOrderItems.SelectParameters.Add("orderID", GXgvSelectedValue.ToString());
                GridViewOrderItems.DataBind();
            }
            mvMain.SetActiveView(viewOrderItems);
            mvOrderItems.SetActiveView(viewOrderItemsGrid);
        }
    }
    protected void DetailsViewOrderItems_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        int orderID = 0;
        if (GXGridView1.SelectedValue != null)
        {
            orderID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        } else
        {
            orderID = GXGridView1SelectedValue;
        }
        e.Values["OrderID"] = orderID;
        int nextOrdinalNumber = Broker.DataAccess.OrderItem.GetNextOrdinalNumber(orderID);
        e.Values["OrdinalNumber"] = nextOrdinalNumber;
    }
    protected void DetailsViewOrderItems_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }
    protected void btnNewOrderItem_Click(object sender, EventArgs e)
    {
        mvOrderItems.SetActiveView(viewOrderItemsEdit);
        DetailsViewOrderItems.ChangeMode(DetailsViewMode.Insert);
    }
    protected void btnAttachments_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsOrderForAttachments.SelectParameters.Clear();
                odsOrderForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewOrderForAttachments.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsOrderForAttachments.SelectParameters.Clear();
                odsOrderForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewOrderForAttachments.DataBind();
            }
            mvMain.SetActiveView(viewAttachments);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti_Active";
            btnOrderItems.CssClass = "stavki";
            GenerateAllFiles();
        }
    }

    protected void btnAddAttachment_Click(object sender, EventArgs e)
    {
        string s = FileUpload1.AccessKey;

        if (FileUpload1.HasFile)
        {
            try
            {
                string orderID = string.Empty;
                if (GXGridView1.SelectedValue != null)
                {
                    orderID = GXGridView1.SelectedDataKey.Value.ToString();
                } else
                {
                    orderID = GXGridView1SelectedValue.ToString();
                }
                string destinationPath = OrderAttachmens_Directory + @"\" + orderID;
                if (!Directory.Exists(destinationPath))
                {
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
                a.DocumentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.NALOG).ID;
                a.DocumentID = Convert.ToInt32(orderID);
                a.RelativePath = path;
                a.Insert();


            } catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        } else
        {
            Label1.Text = "Немате избрано валидна датотека.";
        }
        GenerateAllFiles();
    }

    void GenerateAllFiles()
    {
        string orderID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            orderID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            orderID = GXGridView1SelectedValue.ToString();
        }
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.NALOG).ID && a.DocumentID == int.Parse(orderID)).ToList();
        if (listAttachments.Count > 0)
        {
            FileInfo[] fileInfo = new FileInfo[listAttachments.Count];
            int i = 0;
            foreach (Attachment at in listAttachments)
            {
                fileInfo[i] = new FileInfo(at.RelativePath);
                i++;
            }
            dataGridFiles.DataSource = fileInfo;

        } else
        {
            Label1.Text = "Нема прикачено документи.";
        }
        dataGridFiles.DataBind();
    }

    protected void btnFile_Click(object sender, EventArgs e)
    {
        LinkButton button = (LinkButton)sender;
        string orderID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            orderID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            orderID = GXGridView1SelectedValue.ToString();
        }
        string destinationPath = OrderAttachmens_Directory + @"\" + orderID;
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

    protected void GridViewOrderItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int deletedOrdinalNumber = Convert.ToInt32(e.Values["OrdinalNumber"]);
        //int orderID = Convert.ToInt32(e.Values["OrderID"]);
        //OrderItem.UpdateOrdinalNumbersForDeleteOrderItem(orderID, deletedOrdinalNumber);
    }

    protected void GridViewOrderItems_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        int deletedOrdinalNumber = Convert.ToInt32(e.Values["OrdinalNumber"]);
        int orderID = Convert.ToInt32(e.Values["OrderID"]);
        Broker.DataAccess.OrderItem.UpdateOrdinalNumbersForDeleteOrderItem(orderID, deletedOrdinalNumber);
    }

    protected void dvOrderPreview_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {

    }
    protected void dvOrderPreview_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }
    protected void dvDataSourceOrderItems_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSourceOrderItems_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {

    }
    protected void DetailsViewOrderForAttachments_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {

    }
    protected void DetailsViewOrderForAttachments_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }
    protected void odsOrderForAttachments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

    }

    protected void dataGridFiles_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {

    }
    protected void dataGridFiles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    protected void DetailsView1_DataBinding(object sender, EventArgs e)
    {

    }

    protected void dataGridFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = dataGridFiles.SelectedRow;
        if (gvr != null)
        {
            int documentID = 0;
            if (GXGridView1.SelectedValue != null)
            {
                documentID = Convert.ToInt32(GXGridView1.SelectedValue);
            } else
            {
                documentID = GXGridView1SelectedValue;
            }
            int documentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.NALOG).ID;
            List<Attachment> listAttachments = Attachment.GetByDocumentTypeIDAndDocumentID(documentTypeID, documentID);
            TableCell tc = gvr.Cells[0];
            LinkButton linkButton = tc.FindControl("btnFile") as LinkButton;
            string attachmentName = linkButton.Text;
            foreach (Attachment a in listAttachments)
            {
                string[] splitString = a.RelativePath.Split('\\');
                if (splitString[splitString.Length - 1] == attachmentName)
                {
                    File.Delete(a.RelativePath);
                    Attachment.Table.DeleteOnSubmit(a);
                    Attachment.Table.Context.SubmitChanges();
                }
            }
        }
        GenerateAllFiles();
    }
}
