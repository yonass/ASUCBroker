using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;
using System.IO;

public partial class Broker_FacturesPerBranch : AuthenticationPage {

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







    public static string FactureAttachmens_Directory {
        get {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\Factures";
            return Path;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewFacture.SelectByFKCountCached("UserID", this.PageUser.ID);
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }



    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnFactureItems.CssClass = "prikaz";
        btnPreview.CssClass = "osvezi";
        btnAttachments.CssClass = "dokumenti";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnPintFacture.CssClass = "pecati";
        btnChangeStatus.CssClass = "promeniStatus";
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        GXGridView1.DataBind();
        btnFactureItems.CssClass = "prikaz";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnAttachments.CssClass = "dokumenti";
        btnChangeStatus.CssClass = "promeniStatus";
        btnSearch.CssClass = "prebaraj";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewFacture.SelectSearchByFKCountCached("UserID", this.PageUser.ID, SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnFactureItems.Enabled = true;
            btnAttachments.Enabled = true;
            btnDiscardFacture.Enabled = true;
            btnChangeStatus.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsViewFacturePreview.DataBind();
            GridViewFactureItems.DataBind();
            dvFacturePreviewForAttachments.DataBind();
        }
        if (e.CommandName == "DoubleClick") {
            //btnDelete.Enabled = true;
            btnFactureItems.Enabled = true;
            btnAttachments.Enabled = true;
            btnDiscardFacture.Enabled = true;
            btnChangeStatus.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            mvMain.SetActiveView(viewFactureItems);
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsViewFacturePreview.DataBind();
            GridViewFactureItems.DataBind();
            dvFacturePreviewForAttachments.DataBind();
            BrokerageInFactureItemsPreview();
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnFactureItems.CssClass = "prikaz";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnFactureItems.CssClass = "prikaz";
        btnPintFacture.CssClass = "pecati";
        btnChangeStatus.CssClass = "promeniStatus";
        reportControl.ForeignKeyID = this.PageUser.ID;
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewFacture.SelectFilterByFKCountCached("UserID", this.PageUser.ID, FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            mvMain.SetActiveView(viewFactureItems);
            btnFactureItems.CssClass = "prikaz";
            //btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnFactureItems.CssClass = "prikaz";
            btnPintFacture.CssClass = "pecati";
        }


    }


    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        e.Values["UserID"] = this.PageUser.ID;
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
    protected void btnFactureItems_Click(object sender, EventArgs e) {

        if (GXGridView1.SelectedIndex != -1) {

            if (GXGridView1.SelectedValue != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsFacturePreview.SelectParameters.Clear();
                odsFacturePreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewFacturePreview.DataBind();
                odsFactureItems.SelectParameters.Clear();
                odsFactureItems.SelectParameters.Add("factureID", GXgvSelectedValue.ToString());
                GridViewFactureItems.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsFacturePreview.SelectParameters.Clear();
                odsFacturePreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewFacturePreview.DataBind();
                odsFactureItems.SelectParameters.Clear();
                odsFactureItems.SelectParameters.Add("factureID", GXgvSelectedValue.ToString());
                GridViewFactureItems.DataBind();
            }

            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnFactureItems.CssClass = "prikaz_Active";
            btnPintFacture.CssClass = "pecati";
            btnChangeStatus.CssClass = "promeniStatus";
            mvMain.SetActiveView(viewFactureItems);
            BrokerageInFactureItemsPreview();
        }
    }

    protected void GridViewFactureItems_SelectedIndexChanged(object sender, EventArgs e) {

    }

    protected void GridViewFactureItems_RowCommand(object sender, GridViewCommandEventArgs e) {

    }

    void BrokerageInFactureItemsPreview() {
        if (GXGridView1SelectedValue > 0) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(GXGridView1SelectedValue);
            DataControlField dcf = DetailsViewFacturePreview.Fields[3];
            DataControlField dcfGV = GridViewFactureItems.Columns[4];
            if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA) {
                dcf.Visible = true;
                dcfGV.Visible = true;
            } else if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT || f.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                dcf.Visible = false;
                dcfGV.Visible = false;
            }
        }
    }

    protected void btnPintFacture_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            Broker.DataAccess.Facture f;
            if (GXGridView1.SelectedValue != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                f = Broker.DataAccess.Facture.Get(GXgvSelectedValue);
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                f = Broker.DataAccess.Facture.Get(GXgvSelectedValue);
            }
            if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                List<PolicyItemFactureItem> pifi = PolicyItemFactureItem.GetByFactureID(f.ID);
                if (pifi[0].PolicyItem.Rates.Count > 0) {
                    btnFactureItems.CssClass = "prikaz";
                    btnPreview.CssClass = "osvezi";
                    btnReport.CssClass = "izvestaj";
                    btnSearch.CssClass = "prebaraj";
                    btnFactureItems.CssClass = "prikaz";
                    btnPintFacture.CssClass = "pecati_Active";
                    btnChangeStatus.CssClass = "promeniStatus";
                    btnPrintAnex.CssClass = "dogovor";
                    PrintFactures.PrintFacture(f);
                } else {
                    RegisterStartupScript("myAlert", "<script>alert('Фактурата не е договорена за плаќање на рати!')</script>");
                }
            } else {
                RegisterStartupScript("myAlert", "<script>alert('Фактурата не е договорена за плаќање на рати!')</script>");
            }
        }
    }

    protected void btnPrintAnexDeal_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            Broker.DataAccess.Facture f;
            if (GXGridView1.SelectedValue != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                f = Broker.DataAccess.Facture.Get(GXgvSelectedValue);
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                f = Broker.DataAccess.Facture.Get(GXgvSelectedValue);
            }
            if (f.DocumentSubType.Code == Broker.DataAccess.DocumentSubType.FAKTURA_KLIENT)
            {
                btnFactureItems.CssClass = "prikaz";
                btnPreview.CssClass = "osvezi";
                btnReport.CssClass = "izvestaj";
                btnSearch.CssClass = "prebaraj";
                btnFactureItems.CssClass = "prikaz";
                btnPintFacture.CssClass = "pecati";
                btnChangeStatus.CssClass = "promeniStatus";
                btnPrintAnex.CssClass = "dogovor_Active";
                PrintFactures.PrintAnex(f);
            }
        }
    }

    protected void btnAttachments_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {

            if (GXGridView1.SelectedValue != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsFacturePreviewForAttachments.SelectParameters.Clear();
                odsFacturePreviewForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvFacturePreviewForAttachments.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsFacturePreviewForAttachments.SelectParameters.Clear();
                odsFacturePreviewForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvFacturePreviewForAttachments.DataBind();
            }

            mvMain.SetActiveView(viewAttachments);
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnFactureItems.CssClass = "prikaz";
            btnChangeStatus.CssClass = "promeniStatus";
            btnAttachments.CssClass = "dokumenti_Active";
            GenerateAllFiles();
        }
    }

    void GenerateAllFiles() {
        string factureID = string.Empty;
        if (GXGridView1.SelectedValue != null) {
            factureID = GXGridView1.SelectedDataKey.Value.ToString();
        } else {
            factureID = GXGridView1SelectedValue.ToString();
        }
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.FAKTURA).ID && a.DocumentID == int.Parse(factureID)).ToList();
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
        string factureID = string.Empty;
        if (GXGridView1.SelectedValue != null) {
            factureID = GXGridView1.SelectedDataKey.Value.ToString();
        } else {
            factureID = GXGridView1SelectedValue.ToString();
        }
        string destinationPath = FactureAttachmens_Directory + @"\" + factureID;
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

    protected void dataGridFiles_SelectedIndexChanged(object sender, EventArgs e) {
        GridViewRow gvr = dataGridFiles.SelectedRow;
        if (gvr != null) {
            int documentID = 0;
            if (GXGridView1.SelectedValue != null) {
                documentID = Convert.ToInt32(GXGridView1.SelectedValue);
            } else {
                documentID = GXGridView1SelectedValue;
            }
            int documentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.FAKTURA).ID;
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

    protected void btnAddAttachment_Click(object sender, EventArgs e) {
        if (FileUpload1.HasFile) {
            try {
                string factureID = string.Empty;
                if (GXGridView1.SelectedValue != null) {
                    factureID = GXGridView1.SelectedDataKey.Value.ToString();
                } else {
                    factureID = GXGridView1SelectedValue.ToString();
                }
                string destinationPath = FactureAttachmens_Directory + @"\" + factureID;
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
                a.DocumentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.FAKTURA).ID;
                a.DocumentID = Convert.ToInt32(factureID);
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

    protected void dvFacturePreviewForAttachments_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void dvFacturePreviewForAttachments_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void odsFacturePreviewForAttachments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }





    protected void btnDiscardFacture_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            mvMain.SetActiveView(viewDiscardFacture);
        }
    }


    protected void btnDiscardFac_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedValue != null) {
                id = Convert.ToInt32(GXGridView1.SelectedValue);
            } else {
                id = GXGridView1SelectedValue;
            }
            Broker.DataAccess.Facture.DiscardFacture(id);
            mvMain.SetActiveView(viewGrid);
        }
    }


    void BrokerageInFactureChangeStatusPreview() {
        if (GXGridView1SelectedValue > 0) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(GXGridView1SelectedValue);
            DataControlField dcf = dvChangeStatus.Fields[3];
            if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA) {
                dcf.Visible = true;
            } else if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT || f.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                dcf.Visible = false;
            }
        }
    }

    protected void btnChangeStatus_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewChangeStatus);
        dvChangeStatus.DataBind();
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnFactureItems.CssClass = "prikaz";
        btnChangeStatus.CssClass = "promeniStatus_Active";
        btnAttachments.CssClass = "dokumenti";
        BrokerageInFactureChangeStatusPreview();
    }

    protected void btnUpdateFactureStatus_Click(object sender, EventArgs e) {
        if (dvChangeStatus.SelectedValue != null) {
            int factureID = Convert.ToInt32(dvChangeStatus.SelectedValue);
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(factureID);
            DropDownList ddlStatuses = (DropDownList)dvChangeStatus.FindControl("ddlStatuses");
            f.StatusID = Convert.ToInt32(ddlStatuses.SelectedValue);
            Broker.DataAccess.Facture.Table.Context.SubmitChanges();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void dvChangeStatus_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void odsFactureChangeStatus_Updating(object sender, ObjectDataSourceMethodEventArgs e) {

    }
    protected void dvChangeStatus_ItemUpdating(object sender, DetailsViewUpdateEventArgs e) {

    }
    protected void odsSearch_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("sArgument", SearchControl1.SearchArguments);
        e.InputParameters.Add("foreignKeyName", "UserID");
        e.InputParameters.Add("id", this.PageUser.ID);
    }

    protected void odsFilterGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("fArgument", FilterControl1.FCFilterArgument);
        e.InputParameters.Add("foreignKeyName", "UserID");
        e.InputParameters.Add("id", this.PageUser.ID);
    }

    protected void odsGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("foreignKeyName", "UserID");
        e.InputParameters.Add("id", this.PageUser.ID);
    }
}