using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.FacturesControllers;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;
using System.IO;

public partial class BROKERAdmin_AccountFacturesPreview : AuthenticationPage {

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

    public List<FactureInfo> listFactureInfos {
        get {
            if (ViewState["ListFactureInfos"] == null) {
                return new List<FactureInfo>();
            } else {
                return (List<FactureInfo>)ViewState["ListFactureInfos"];
            }
        }
        set {
            ViewState["ListFactureInfos"] = value;
        }
    }



    public List<FactureInfo> listNewFactureInfos {
        get {
            if (ViewState["ListNewFactureInfos"] == null) {
                return new List<FactureInfo>();
            } else {
                return (List<FactureInfo>)ViewState["ListNewFactureInfos"];
            }
        }
        set {
            ViewState["ListNewFactureInfos"] = value;
        }
    }

    public static string FactureAttachmens_Directory {
        get {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\AccountFactures";
            return Path;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewAccountFacture.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e) {

        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        DateTime dateOfPayment = DateTime.Now.AddDays(15);
        if (tbDateOfPayment.Text.Trim() != string.Empty) {
            DateTime.TryParse(tbDateOfPayment.Text, out dateOfPayment);
        }
        InsuranceCompany company = InsuranceCompany.Get(int.Parse(ddlInsuranceCompanies.SelectedValue));
        Broker.DataAccess.User u = this.PageUser;

        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        //List<PolicyItem> piList = PolicyItem.GetFromPreivousMonths(fromDate, company.ID);
        List<FactureInfo> lastFactureInfos = listNewFactureInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(facInfo => facInfo.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listNewFactureInfos = lastFactureInfos;
        lastFactureInfos = listFactureInfos;
        displayedRows = gvOldPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvOldPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(facInfo => facInfo.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listFactureInfos = lastFactureInfos;


        List<PolicyItem> policiesToInclude = new List<PolicyItem>();
        List<FactureInfo> listOldPolicies = (List<FactureInfo>)listFactureInfos;
        foreach (FactureInfo fi in listOldPolicies) {
            if (fi.IsForFacturing) {
                policiesToInclude.Add(PolicyItem.Get(fi.ID));
            }
        }
        List<FactureInfo> listNewPolicies = (List<FactureInfo>)listNewFactureInfos;
        foreach (FactureInfo fi in listNewPolicies) {
            if (fi.IsForFacturing) {
                PolicyItem pi = PolicyItem.Get(fi.ID);
                if (!policiesToInclude.Contains(pi)) {
                    policiesToInclude.Add(pi);
                }
            }
        }

        AccountFacture f = AccountFacture.Generate(tbAccountFactureNumber.Text, startDate, endDate, company, u, dateOfPayment, policiesToInclude, this.PageBranch);
        if (f != null) {
            lblError.Visible = false;
            odsFacturePreview.SelectParameters.Clear();
            odsFacturePreview.SelectParameters.Add("id", f.ID.ToString());
            odsFactureItems.SelectParameters.Clear();
            odsFactureItems.SelectParameters.Add("factureID", f.ID.ToString());
            mvMain.SetActiveView(viewFactureItems);
            GXGridView1SelectedValue = f.ID;
        } else {
            lblError.Visible = true;
            lblError.Font.Bold = true;
            lblError.Text = "Не постојат нефактурирани износи";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnFactureItems.CssClass = "prikaz";
        btnPreview.CssClass = "osvezi";
        btnAttachments.CssClass = "dokumenti";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnPintFacture.CssClass = "pecati";
        btnChangeStatus.CssClass = "izmeni";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewNew);
        btnNew.CssClass = "novZapis_Active";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnFactureItems.CssClass = "prikaz";
        btnPintFacture.CssClass = "pecati";
        btnAttachments.CssClass = "dokumenti";
        btnChangeStatus.CssClass = "izmeni";
    }
    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        GXGridView1.DataBind();
        btnNew.CssClass = "novZapis";
        btnFactureItems.CssClass = "prikaz";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnAttachments.CssClass = "dokumenti";
        btnChangeStatus.CssClass = "izmeni";
        btnSearch.CssClass = "prebaraj";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewAccountFacture.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnFactureItems.Enabled = true;
            btnAttachments.Enabled = true;
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
            btnChangeStatus.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            mvMain.SetActiveView(viewFactureItems);
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsViewFacturePreview.DataBind();
            GridViewFactureItems.DataBind();
            dvFacturePreviewForAttachments.DataBind();
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnFactureItems.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnFactureItems.CssClass = "prikaz";
        btnPintFacture.CssClass = "pecati";
        btnChangeStatus.CssClass = "izmeni";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewAccountFacture.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            mvMain.SetActiveView(viewFactureItems);
            btnNew.CssClass = "novZapis";
            btnFactureItems.CssClass = "izmeni";
            //btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnFactureItems.CssClass = "prikaz";
            btnPintFacture.CssClass = "pecati";
        }


    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewAccountFacture.SelectCountCached();
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
            GXGridView1.TotalRecords = ViewAccountFacture.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewAccountFacture.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
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

            btnNew.CssClass = "novZapis";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            btnFactureItems.CssClass = "prikaz_Active";
            btnPintFacture.CssClass = "pecati";
            btnChangeStatus.CssClass = "izmeni";
            mvMain.SetActiveView(viewFactureItems);
        }
    }

    protected void GridViewFactureItems_SelectedIndexChanged(object sender, EventArgs e) {

    }

    protected void GridViewFactureItems_RowCommand(object sender, GridViewCommandEventArgs e) {

    }

    protected void btnPintFacture_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            AccountFacture f;
            f = AccountFacture.Get(GXGridView1SelectedValue);
            btnNew.CssClass = "novZapis";
            btnFactureItems.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnFactureItems.CssClass = "prikaz";
            btnPintFacture.CssClass = "pecati_Active";
            btnChangeStatus.CssClass = "izmeni";
            PrintAccountFactures.PrintAccountFacture(f);
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
            btnNew.CssClass = "novZapis";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnFactureItems.CssClass = "prikaz";
            btnChangeStatus.CssClass = "izmeni";
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



    protected void btnCheck_Click(object sender, EventArgs e) {

        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        List<PolicyItem> oldPiList = PolicyItem.GetFromPreivousMonthsForAccountFactures(fromDate, int.Parse(ddlInsuranceCompanies.SelectedValue));
        List<FactureInfo> oldFactureInfos = new List<FactureInfo>();
        foreach (PolicyItem pi in oldPiList) {
            FactureInfo fi = new FactureInfo();
            fi.ID = pi.ID;
            fi.IsForFacturing = true;
            fi.PolicyNumber = pi.PolicyNumber;
            oldFactureInfos.Add(fi);
        }
        listFactureInfos = oldFactureInfos;
        gvOldPolicies.DataSource = oldFactureInfos;
        gvOldPolicies.Caption = "Полиси од претходни месеци (" + oldFactureInfos.Count.ToString() + ")";
        gvOldPolicies.DataBind();

        List<PolicyItem> newPiList = PolicyItem.GetForAccountFacturingFromCurrentMonth(fromDate, toDate, int.Parse(ddlInsuranceCompanies.SelectedValue));
        List<FactureInfo> newFactureInfos = new List<FactureInfo>();
        foreach (PolicyItem pi in newPiList) {
            FactureInfo fi = new FactureInfo();
            fi.ID = pi.ID;
            fi.IsForFacturing = true;
            fi.PolicyNumber = pi.PolicyNumber;
            newFactureInfos.Add(fi);
        }
        listNewFactureInfos = newFactureInfos;
        gvNewPolicies.DataSource = newFactureInfos;
        gvNewPolicies.Caption = "Полиси од селектираниот период (" + newFactureInfos.Count.ToString() + ")";
        gvNewPolicies.DataBind();

    }

    protected void btnDiscardFacture_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedValue != null) {
                id = Convert.ToInt32(GXGridView1.SelectedValue);
            } else {
                id = GXGridView1SelectedValue;
            }
            AccountFacture.DiscardFacture(id);
        }
    }

    protected void gvOldPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        List<FactureInfo> lastFactureInfos = listFactureInfos;
        GridViewRowCollection displayedRows = gvOldPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvOldPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listFactureInfos = lastFactureInfos;
        gvOldPolicies.PageIndex = e.NewPageIndex;
        gvOldPolicies.DataSource = lastFactureInfos;
        gvOldPolicies.DataBind();
    }

    protected void gvNewPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        List<FactureInfo> lastFactureInfos = listNewFactureInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listNewFactureInfos = lastFactureInfos;
        gvNewPolicies.PageIndex = e.NewPageIndex;
        gvNewPolicies.DataSource = lastFactureInfos;
        gvNewPolicies.DataBind();
    }


    protected void btnChangeStatus_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewChangeStatus);
        dvChangeStatus.DataBind();
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnFactureItems.CssClass = "prikaz";
        btnChangeStatus.CssClass = "izmeni_Active";
        btnAttachments.CssClass = "dokumenti";
    }

    protected void btnUpdateFactureStatus_Click(object sender, EventArgs e) {
        if (dvChangeStatus.SelectedValue != null) {
            int factureID = Convert.ToInt32(dvChangeStatus.SelectedValue);
            AccountFacture f = AccountFacture.Get(factureID);
            DropDownList ddlStatuses = (DropDownList)dvChangeStatus.FindControl("ddlStatuses");
            f.StatusID = Convert.ToInt32(ddlStatuses.SelectedValue);
            AccountFacture.Table.Context.SubmitChanges();
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
    protected void btnCalculateCurrentState_Click(object sender, EventArgs e) {
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        InsuranceCompany company = InsuranceCompany.Get(int.Parse(ddlInsuranceCompanies.SelectedValue));
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        List<FactureInfo> lastFactureInfos = listNewFactureInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(facInfo => facInfo.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listNewFactureInfos = lastFactureInfos;
        lastFactureInfos = listFactureInfos;
        displayedRows = gvOldPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvOldPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(facInfo => facInfo.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listFactureInfos = lastFactureInfos;

        decimal brokerageValue = 0;
        decimal totalSum = 0;
        decimal currentBrokerageValue = 0;

        List<PolicyItem> policiesToInclude = new List<PolicyItem>();
        List<FactureInfo> listOldPolicies = (List<FactureInfo>)listFactureInfos;
        foreach (FactureInfo fi in listOldPolicies) {
            if (fi.IsForFacturing) {
                policiesToInclude.Add(PolicyItem.Get(fi.ID));
            }
        }
        List<FactureInfo> listNewPolicies = (List<FactureInfo>)listNewFactureInfos;
        foreach (FactureInfo fi in listNewPolicies) {
            if (fi.IsForFacturing) {
                PolicyItem pi = PolicyItem.Get(fi.ID);
                if (!policiesToInclude.Contains(pi)) {
                    policiesToInclude.Add(pi);
                }
            }
        }

        foreach (PolicyItem policyItem in policiesToInclude) {
            if (policyItem.Policy.PacketID != null) {
                if (policyItem.Policy.Client.IsLaw) {
                    currentBrokerageValue = policyItem.PremiumValue * policyItem.PacketsInsuranceSubType.BrokeragePecentageForLaws / 100;

                } else {
                    currentBrokerageValue = policyItem.PremiumValue * policyItem.PacketsInsuranceSubType.BrokeragePecentageForPrivates / 100;
                }
            } else {
                if (policyItem.Policy.Client.IsLaw) {
                    currentBrokerageValue = policyItem.PremiumValue * policyItem.Brokerage.PercentageForLaws / 100;
                } else {
                    currentBrokerageValue = policyItem.PremiumValue * policyItem.Brokerage.PercentageForPrivates / 100;
                }
            }
            brokerageValue += currentBrokerageValue;
            totalSum += policyItem.PremiumValue;
        }
        tbCurrentBrokerageValue.Text = String.Format("{0:#,0.00}", brokerageValue);
        tbCurrentPremiumValue.Text = String.Format("{0:#,0.00}", totalSum);
    }
}