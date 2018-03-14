using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using System.IO;
using Broker.Controllers.FacturesControllers;

public partial class BROKERAdmin_IncomingFactures : AuthenticationPage
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

    public static string IncomingFactureAttachmens_Directory
    {
        get
        {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\BROKERAdmin\Attachments\IncomingFactures";
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
            GXGridView1.TotalRecords = ViewIncomingFacture.SelectCountCached();
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
                LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
                LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
            }
            mvMain.SetActiveView(viewEdit);
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
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
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        GXGridView1.SelectedIndex = -1;
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        DetailsView1.DataBind();
        mvMain.SetActiveView(viewEdit);
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        //  btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";

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
    }
    protected void SearchControl1_Search(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = ViewIncomingFacture.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SingleClick")
        {
            btnEdit.Enabled = true;
            btnAttachments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsViewIncomingFactureForAttachments.DataBind();
        }
        if (e.CommandName == "DoubleClick")
        {
            btnEdit.Enabled = true;
            btnAttachments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti";
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            DetailsViewIncomingFactureForAttachments.DataBind();
            mvMain.SetActiveView(viewEdit);
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
    }
    protected void FilterControl1_Filter(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = ViewIncomingFacture.SelectFilterCountCached(FilterControl1.FCFilterArgument);
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
            GXGridView1.TotalRecords = ViewIncomingFacture.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
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
            GXGridView1.TotalRecords = ViewIncomingFacture.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GXGridView1.TotalRecords = ViewIncomingFacture.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        if (e.Values["IncomingFactureTypeID"].ToString() != IncomingFactureType.GetByCode(IncomingFactureType.ZA_POLISI).ID.ToString()) {
            e.Values["InsuranceCompanyID"] = null;
        }
        e.Values["UserID"] = this.PageUser.ID;
        e.Values["BranchID"] = this.PageBranch.ID;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        object o = e.ReturnValue;
        IncomingFacture inFac = (IncomingFacture)o;
        if (inFac.IncomingFactureTypeID == IncomingFactureType.GetByCode(IncomingFactureType.ZA_POLISI).ID) {
            List<IncomingFacturePolicyItem> lstOld = IncomingFacturePolicyItem.GetByIncomingFactureID(inFac.ID);
            foreach (IncomingFacturePolicyItem ifpi in lstOld) {
                ifpi.IsActive = false;
                IncomingFacturePolicyItem.Table.Context.SubmitChanges();
            }
            GridView gvNewPolicies = (GridView)DetailsView1.FindControl("gvNewPolicies");
            List<FactureInfo> lastFactureInfos = listFactureInfos;
            GridViewRowCollection displayedRows = gvNewPolicies.Rows;
            foreach (GridViewRow gvRow in displayedRows) {
                CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
                int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
                FactureInfo fi = lastFactureInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
                fi.IsForFacturing = cbIsForFacturing.Checked;
            }
            listFactureInfos = lastFactureInfos;
            List<FactureInfo> listFactureInfo = listFactureInfos.Where(c => c.IsForFacturing == true).ToList();
            foreach (FactureInfo fi in listFactureInfo) {
                IncomingFacturePolicyItem ifpi = new IncomingFacturePolicyItem();
                ifpi.IncomingFactureID = inFac.ID;
                ifpi.PolicyItemID = fi.ID;
                ifpi.IsActive = true;
                ifpi.Insert();
                PolicyItem pi = PolicyItem.Get(fi.ID);
                pi.PolicyIncomeFactureStatusID = PolicyIncomeFactureStatuse.GetByCode(PolicyIncomeFactureStatuse.CELOSNO_FAKTURIRANA).ID;
                PolicyItem.Table.Context.SubmitChanges();
            }
        }
    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        object o = e.ReturnValue;
        IncomingFacture inFac = (IncomingFacture)o;
        if (inFac.IncomingFactureType.Code == IncomingFactureType.ZA_POLISI) {
            GridView gvNewPolicies = (GridView)DetailsView1.FindControl("gvNewPolicies");
            List<FactureInfo> lastFactureInfos = listFactureInfos;
            GridViewRowCollection displayedRows = gvNewPolicies.Rows;
            foreach (GridViewRow gvRow in displayedRows) {
                CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
                int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
                FactureInfo fi = lastFactureInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
                fi.IsForFacturing = cbIsForFacturing.Checked;
            }
            listFactureInfos = lastFactureInfos;
            List<FactureInfo> listFactureInfo = listFactureInfos.Where(c => c.IsForFacturing == true).ToList();
            foreach (FactureInfo fi in listFactureInfo) {
                IncomingFacturePolicyItem ifpi = new IncomingFacturePolicyItem();
                ifpi.IncomingFactureID = inFac.ID;
                ifpi.PolicyItemID = fi.ID;
                ifpi.IsActive = true;
                ifpi.Insert();
                PolicyItem pi = PolicyItem.Get(fi.ID);
                pi.PolicyIncomeFactureStatusID = PolicyIncomeFactureStatuse.GetByCode(PolicyIncomeFactureStatuse.CELOSNO_FAKTURIRANA).ID;
                PolicyItem.Table.Context.SubmitChanges();
            }
        }
    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        IncomingFacture incomingFacture = e.InputParameters["entityToInsert"] as IncomingFacture;
        //incomingFacture.ValidateNumber();

        //ca.ValidateAccount();
    }

    protected void btnDelete_Click1(object sender, EventArgs e)
    {
        ClientAccount ca = ClientAccount.GetByClientAndBank(int.Parse(GXGridView1.SelectedDataKey["ClientID"].ToString()), int.Parse(GXGridView1.SelectedDataKey["BankID"].ToString()));
        ca.IsActive = false;
        ClientAccount.Table.Context.SubmitChanges();
        GXGridView1.TotalRecords = ViewIncomingFacture.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

   
    
    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //e.InputParameters.Add("ClientID", int.Parse(GXGridView1.SelectedDataKey["ClientID"].ToString()));
        //e.InputParameters.Add("BankID", int.Parse(GXGridView1.SelectedDataKey["BankID"].ToString()));
        if (DetailsView1.CurrentMode == DetailsViewMode.Edit) {
            LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
        }
    }


   

   
    
    protected void btnAttachments_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedDataKey != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsIncomingFactureForAttachments.SelectParameters.Clear();
                odsIncomingFactureForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewIncomingFactureForAttachments.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsIncomingFactureForAttachments.SelectParameters.Clear();
                odsIncomingFactureForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewIncomingFactureForAttachments.DataBind();
            }
            mvMain.SetActiveView(viewAttachments);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti_Active";
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
                string IncomingFactureID = string.Empty;
                if (GXGridView1.SelectedValue != null)
                {
                    IncomingFactureID = GXGridView1.SelectedDataKey.Value.ToString();
                } else
                {
                    IncomingFactureID = GXGridView1SelectedValue.ToString();
                }
                string destinationPath = IncomingFactureAttachmens_Directory + @"\" + IncomingFactureID;
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
                a.DocumentID = Convert.ToInt32(IncomingFactureID);
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
        string IncomingFactureID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            IncomingFactureID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            IncomingFactureID = GXGridView1SelectedValue.ToString();
        }
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.NALOG).ID && a.DocumentID == int.Parse(IncomingFactureID)).ToList();
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
        string IncomingFactureID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            IncomingFactureID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            IncomingFactureID = GXGridView1SelectedValue.ToString();
        }
        string destinationPath = IncomingFactureAttachmens_Directory + @"\" + IncomingFactureID;
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

    protected void GridViewIncomingFactureItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int deletedOrdinalNumber = Convert.ToInt32(e.Values["OrdinalNumber"]);
        //int IncomingFactureID = Convert.ToInt32(e.Values["IncomingFactureID"]);
        //IncomingFactureItem.UpdateOrdinalNumbersForDeleteIncomingFactureItem(IncomingFactureID, deletedOrdinalNumber);
    }


    protected void dvIncomingFacturePreview_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {

    }
    protected void dvIncomingFacturePreview_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }
    protected void dvDataSourceIncomingFactureItems_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSourceIncomingFactureItems_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        
    }
    protected void DetailsViewIncomingFactureForAttachments_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {

    }
    protected void DetailsViewIncomingFactureForAttachments_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }
    protected void odsIncomingFactureForAttachments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
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
        if (DetailsView1.CurrentMode == DetailsViewMode.Edit) {
            LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
        }
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

    void LoadPoliciesForFacturing(int insuranceCompanyID) {
        GridView gvNewPolicies = (GridView)DetailsView1.FindControl("gvNewPolicies");
        List<PolicyItem> newPiList = PolicyItem.GetNonIncomeFactured(insuranceCompanyID);
        List<FactureInfo> newFactureInfos = new List<FactureInfo>();
        foreach (PolicyItem pi in newPiList) {
            FactureInfo fi = new FactureInfo();
            fi.ID = pi.ID;
            fi.IsForFacturing = true;
            fi.PolicyNumber = pi.PolicyNumber;
            newFactureInfos.Add(fi);
        }
        listFactureInfos = newFactureInfos;
        gvNewPolicies.DataSource = newFactureInfos;
        gvNewPolicies.DataBind();

    }

    void LoadPoliciesForEdit(int incomeFactureID) {
        GridView gvNewPolicies = (GridView)DetailsView1.FindControl("gvNewPolicies");
        List<PolicyItem> newPiList = IncomingFacturePolicyItem.GetByIncomingFactureID(incomeFactureID).Select(c => c.PolicyItem).ToList();
        List<FactureInfo> newFactureInfos = new List<FactureInfo>();
        foreach (PolicyItem pi in newPiList) {
            FactureInfo fi = new FactureInfo();
            fi.ID = pi.ID;
            fi.IsForFacturing = true;
            fi.PolicyNumber = pi.PolicyNumber;
            newFactureInfos.Add(fi);
        }
        listFactureInfos = newFactureInfos;
        gvNewPolicies.DataSource = newFactureInfos;
        gvNewPolicies.DataBind();

    }

    protected void ddlIncomingFactureTypes_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlIncomingFactureTypes = (DropDownList)sender;
        IncomingFactureType ifc = IncomingFactureType.Get(Convert.ToInt32(ddlIncomingFactureTypes.SelectedValue));
        DropDownList ddlInsuranceCompanies = (DropDownList)DetailsView1.FindControl("ddlInsuranceCompanies");
        TextBox tbEMBG = (TextBox)DetailsView1.FindControl("tbEMBG");
        TextBox tbName = (TextBox)DetailsView1.FindControl("tbName");
        TextBox tbAddress = (TextBox)DetailsView1.FindControl("tbAddress");
        TextBox tbTaxValue = (TextBox)DetailsView1.FindControl("tbTaxValue");
        GridView gvNewPolicies = (GridView)DetailsView1.FindControl("gvNewPolicies");
        TextBox tbPolicyNumberToList = (TextBox)DetailsView1.FindControl("tbPolicyNumberToList");
        Button btnAddPolicyToList = (Button)DetailsView1.FindControl("btnAddPolicyToList");
        if (ifc.Code == IncomingFactureType.ZA_POLISI) {
            ddlInsuranceCompanies.Visible = true;
            tbEMBG.Enabled = false;
            tbName.Enabled = false;
            tbAddress.Enabled = false;
            gvNewPolicies.Visible = true;
            tbPolicyNumberToList.Visible = true;
            btnAddPolicyToList.Visible = true;
            tbTaxValue.Text = "0";
        } else {
            ddlInsuranceCompanies.Visible = false;
            tbEMBG.Enabled = true;
            tbName.Enabled = true;
            tbAddress.Enabled = true;
            tbPolicyNumberToList.Visible = false;
            btnAddPolicyToList.Visible = false;
            gvNewPolicies.Visible = false;
            ddlInsuranceCompanies.SelectedIndex = -1;
        }

    }

    protected void gvNewPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        GridView gvNewPolicies = (GridView)DetailsView1.FindControl("gvNewPolicies");
        List<FactureInfo> lastFactureInfos = listFactureInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listFactureInfos = lastFactureInfos;
        gvNewPolicies.PageIndex = e.NewPageIndex;
        gvNewPolicies.DataSource = lastFactureInfos;
        gvNewPolicies.DataBind();
    }


    protected void ddlInsuranceCompanies_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceCompanies = (DropDownList)DetailsView1.FindControl("ddlInsuranceCompanies");
        int icID = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        InsuranceCompany ic = InsuranceCompany.Get(icID);
        TextBox tbEMBG = (TextBox)DetailsView1.FindControl("tbEMBG");
        TextBox tbName = (TextBox)DetailsView1.FindControl("tbName");
        TextBox tbAddress = (TextBox)DetailsView1.FindControl("tbAddress");
        tbEMBG.Text = ic.EMBG;
        tbName.Text = ic.Name;
        tbAddress.Text = ic.Address;
        if (DetailsView1.CurrentMode == DetailsViewMode.Insert) {
            LoadPoliciesForFacturing(icID);
        } else if(DetailsView1.CurrentMode == DetailsViewMode.Edit) {
            LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
        }
    }
    protected void btnAddPolicyToList_Click(object sender, EventArgs e) {
        try {
            TextBox tbPolicyNumberToList = (TextBox)DetailsView1.FindControl("tbPolicyNumberToList");
            DropDownList ddlInsuranceCompanies = (DropDownList)DetailsView1.FindControl("ddlInsuranceCompanies");
            int icID = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
            InsuranceCompany ic = InsuranceCompany.Get(icID);
            PolicyItem pi = PolicyItem.GetByNumberAndInsuranceCompany(tbPolicyNumberToList.Text, icID);
            if (pi != null) {
                GridView gvNewPolicies = (GridView)DetailsView1.FindControl("gvNewPolicies");
                List<FactureInfo> lastFactureInfos = listFactureInfos;
                FactureInfo fi = listFactureInfos.Where(c => c.PolicyNumber == tbPolicyNumberToList.Text).SingleOrDefault();
                if (fi != null) {
                    RegisterStartupScript("myAlert", "<script>alert('Полисата постои во листата!')</script>");
                    return;
                }
                FactureInfo newFactureInfo = new FactureInfo();
                newFactureInfo.PolicyNumber = tbPolicyNumberToList.Text;
                newFactureInfo.IsForFacturing = true;
                newFactureInfo.ID = pi.ID;
                lastFactureInfos.Add(newFactureInfo);
                listFactureInfos = lastFactureInfos;
                gvNewPolicies.DataSource = lastFactureInfos;
                gvNewPolicies.DataBind();
            } else {
                RegisterStartupScript("myAlert", "<script>alert('Не постои полиса со избраниот број!')</script>");
            }
        } catch {
            RegisterStartupScript("myAlert", "<script>alert('Грешка!')</script>");
        }
    }
    protected void dvDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e) {
        if (DetailsView1.CurrentMode == DetailsViewMode.Edit) {
            LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
        }
    }
    protected void dvDataSource_DataBinding(object sender, EventArgs e) {
        if (DetailsView1.CurrentMode == DetailsViewMode.Edit) {
            LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
        }
    }
    protected void DetailsView1_DataBound(object sender, EventArgs e) {
        if (DetailsView1.CurrentMode == DetailsViewMode.Edit) {
            LoadPoliciesForEdit(Convert.ToInt32(DetailsView1.SelectedValue));
        }
    }
}

