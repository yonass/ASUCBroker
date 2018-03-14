using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ManagementControllers;
using System.IO;

public partial class BROKERAdmin_RateDeals : AuthenticationPage {
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


    public static string RateDealAttachmens_Directory {
        get {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\RateDeals";
            return Path;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewRateDeal.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void dvRateDealPreview_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void dvRateDealPreview_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
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
        GXGridView1.TotalRecords = ViewRateDeal.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }

    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }

    protected void GridViewRateDealItems_RowDeleting(object sender, GridViewDeleteEventArgs e) {

    }

    protected void GridViewRateDealItems_RowDeleted(object sender, GridViewDeletedEventArgs e) {

    }

    protected void DetailsViewRateDealItems_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsViewRateDealItems_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsGridViewRateDealItems.SelectParameters.Clear();
                odsGridViewRateDealItems.SelectParameters.Add("rateDealID", GXgvSelectedValue.ToString());
                GridViewRateDealItems.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsGridViewRateDealItems.SelectParameters.Clear();
                odsGridViewRateDealItems.SelectParameters.Add("rateDealID", GXgvSelectedValue.ToString());
                GridViewRateDealItems.DataBind();
            }
            mvMain.SetActiveView(viewRateDealItems);
            mvRateDealItems.SetActiveView(viewRateDealItemsGrid);
        }
    }
    protected void DetailsViewRateDealItems_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        int orderID = 0;
        if (GXGridView1.SelectedValue != null) {
            orderID = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        } else {
            orderID = GXGridView1SelectedValue;
        }
        e.Values["RateDealID"] = orderID;
    }

    protected void btnNewRateDealItem_Click(object sender, EventArgs e) {
        mvRateDealItems.SetActiveView(viewRateDealItemsEdit);
        DetailsViewRateDealItems.ChangeMode(DetailsViewMode.Insert);
    }


    protected void DetailsViewRateDealItems_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewRateDealItems);
            mvRateDealItems.SetActiveView(viewRateDealItemsGrid);
        }
    }

    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnRateDealItems.Enabled = true;
            btnAttachments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
        }
        if (e.CommandName == "DoubleClick") {
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnRateDealItems.Enabled = true;
            btnAttachments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            mvMain.SetActiveView(viewRateDealItems);
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
        GXGridView1.TotalRecords = ViewRateDeal.SelectFilterCountCached(FilterControl1.FCFilterArgument);
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
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewRateDeal.SelectCountCached();
            GXGridView1.DataBind();
            dvRateDealPreview.DataBind();
            mvRateDealItems.SetActiveView(viewGrid);
            mvMain.SetActiveView(viewRateDealItems);
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewRateDeal.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewRateDeal.SelectCountCached();
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
        RateDeal rd = e.InputParameters["newEntity"] as RateDeal;
        DealController.ValidateUpdateDealNumber(rd.ID, rd.DealNumber, rd.ClientID);
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
        object o = e.ReturnValue;
        RateDeal rd = (RateDeal)o;
        GXGridView1SelectedValue = rd.ID;
    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        RateDeal rd = e.InputParameters["entityToInsert"] as RateDeal;
        DealController.ValidateInsertDealNumber(rd.DealNumber, rd.ClientID);
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
            RateDeal d = RateDeal.Get(dealID);
            d.IsActive = false;
            RateDeal.Table.Context.SubmitChanges();

            GXGridView1.TotalRecords = ViewRateDeal.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void btnRateDealItems_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {

                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsRateDealPreview.SelectParameters.Clear();
                odsRateDealPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvRateDealPreview.DataBind();
                odsGridViewRateDealItems.SelectParameters.Clear();
                odsGridViewRateDealItems.SelectParameters.Add("rateDealID", GXgvSelectedValue.ToString());
                GridViewRateDealItems.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsRateDealPreview.SelectParameters.Clear();
                odsRateDealPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvRateDealPreview.DataBind();
                odsGridViewRateDealItems.SelectParameters.Clear();
                odsGridViewRateDealItems.SelectParameters.Add("rateDealID", GXgvSelectedValue.ToString());
                GridViewRateDealItems.DataBind();
            }

            mvMain.SetActiveView(viewRateDealItems);
            mvRateDealItems.SetActiveView(viewRateDealItemsGrid);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnRateDealItems.CssClass = "stavki_Active";
        }
    }

    protected void btnAttachments_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsRateDealsForAttachments.SelectParameters.Clear();
                odsRateDealsForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewRateDealsForAttachments.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsRateDealsForAttachments.SelectParameters.Clear();
                odsRateDealsForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewRateDealsForAttachments.DataBind();
            }
            mvMain.SetActiveView(viewAttachments);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti_Active";
            btnRateDealItems.CssClass = "stavki";
            GenerateAllFiles();
        }
    }

    protected void dataGridFiles_SelectedIndexChanged(object sender, EventArgs e) {
        GridViewRow gvr = dataGridFiles.SelectedRow;
        if (gvr != null) {
            int documentID = 0;
            if (GXGridView1.SelectedValue != null) {
                documentID = Convert.ToInt32(GXGridView1.SelectedValue);
            } else {
                documentID = GXGridView1SelectedValue;
            }
            int documentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.DOGOVOR_ZA_RATI).ID;
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

    protected void dataGridFiles_RowDeleted(object sender, GridViewDeletedEventArgs e) {

    }
    protected void dataGridFiles_RowCommand(object sender, GridViewCommandEventArgs e) {
    }

    protected void DetailsViewRateDealsForAttachments_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void DetailsViewRateDealsForAttachments_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void odsRateDealsForAttachments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }

    protected void btnAddAttachment_Click(object sender, EventArgs e) {
        string s = FileUpload1.AccessKey;

        if (FileUpload1.HasFile) {
            try {
                string orderID = string.Empty;
                if (GXGridView1.SelectedValue != null) {
                    orderID = GXGridView1.SelectedDataKey.Value.ToString();
                } else {
                    orderID = GXGridView1SelectedValue.ToString();
                }
                string destinationPath = RateDealAttachmens_Directory + @"\" + orderID;
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
                a.DocumentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.DOGOVOR_ZA_RATI).ID;
                a.DocumentID = Convert.ToInt32(orderID);
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
        string orderID = string.Empty;
        if (GXGridView1.SelectedValue != null) {
            orderID = GXGridView1.SelectedDataKey.Value.ToString();
        } else {
            orderID = GXGridView1SelectedValue.ToString();
        }
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.DOGOVOR_ZA_RATI).ID && a.DocumentID == int.Parse(orderID)).ToList();
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
        string orderID = string.Empty;
        if (GXGridView1.SelectedValue != null) {
            orderID = GXGridView1.SelectedDataKey.Value.ToString();
        } else {
            orderID = GXGridView1SelectedValue.ToString();
        }
        string destinationPath = RateDealAttachmens_Directory + @"\" + orderID;
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
    protected void odsGridViewRateDealItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("rateDealID", GXGridView1SelectedValue);
    }

    protected void odsRateDealPreview_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }

    protected void dvDataSourceRateDealItems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }
}