using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.EmployeeManagement;
using System.IO;

public partial class Broker_LifePolicies : AuthenticationPage {
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


    public static string PolicyAttachmens_Directory {
        get {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\LifePolicies";
            return Path;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = LifePoliciesView.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
            if (this.PageUser.Role.Name == RolesInfo.BROKERAdmin) {
                GXGridView1.TotalRecords = LifePoliciesView.SelectCountCached();
                odsGridView.SelectCountMethod = "SelectCountCached";
                odsGridView.SelectMethod = "Select";
            } else {
                odsGridView.SelectCountMethod = "SelectByFKCountCached";
                odsGridView.SelectMethod = "SelectByFK";
                GXGridView1.TotalRecords = LifePoliciesView.SelectByFKCountCached("UserID", this.PageUser.ID);
            }
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            mvMain.SetActiveView(viewEdit);
            //DetailsView1.DataBind();
            //DetailsView1.ChangeMode(DetailsViewMode.Edit);
            //btnNew.CssClass = "novZapis";
            //btnEdit.CssClass = "izmeni_Active";
            //btnDelete.CssClass = "izbrisi";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnPreviewPolicy.CssClass = "prikaz";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        //btnNew.CssClass = "novZapis";
        //btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnAttachments.CssClass = "dokumenti";
        btnPreviewPolicy.CssClass = "prikaz";
    }

    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        //btnNew.CssClass = "novZapis";
        //btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnPreviewPolicy.CssClass = "prikaz";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        if (this.PageUser.Role.Name == RolesInfo.BROKERAdmin) {
            GXGridView1.TotalRecords = PoliciesView.SelectSearchCountCached(SearchControl1.SearchArguments);
        } else {
            GXGridView1.TotalRecords = PoliciesView.SelectSearchByFKCountCached("UserID", this.PageUser.ID, SearchControl1.SearchArguments);
        }
        mvMain.SetActiveView(viewGrid);
    }


    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnPreviewPolicy.Enabled = true;
            btnDiscard.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            PoliciesDetailsView.DataBind();
            DetailsViewPolicyForAttachments.DataBind();
            SetInsuranceDurationInYears();
        }
        if (e.CommandName == "DoubleClick") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            mvMain.SetActiveView(viewEdit);
            btnDiscard.Enabled = true;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            //PoliciesDetailsView.DataBind();
            //DetailsViewPolicyForAttachments.DataBind();
            //OnInit(e);
            //CreateChildControls();
            //mvPolicyItem.ActiveViewIndex = 0;
            //btnPreviewPolicy.CssClass = "prikaz_Active";
            //btnPreview.CssClass = "osvezi";
            //btnReport.CssClass = "izvestaj";
            //btnSearch.CssClass = "prebaraj";
            //btnAttachments.CssClass = "dokumenti";
            btnPreviewPolicy_Click(null, null);
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        //btnNew.CssClass = "novZapis";
        //btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnPreviewPolicy.CssClass = "prikaz";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        if (this.PageUser.Role.Name == RolesInfo.BROKERAdmin) {
            GXGridView1.TotalRecords = PoliciesView.SelectFilterCountCached(FilterControl1.FCFilterArgument);
        } else {
            GXGridView1.TotalRecords = PoliciesView.SelectFilterByFKCountCached("UserID", this.PageUser.ID, FilterControl1.FCFilterArgument);
        }
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            //DetailsView1.DataBind();
            //DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            mvMain.SetActiveView(viewEdit);
            //btnNew.CssClass = "novZapis";
            //btnEdit.CssClass = "izmeni";
            //btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
        }
    }

    protected void btnAttachments_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsPolicyForAttachments.SelectParameters.Clear();
                odsPolicyForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewPolicyForAttachments.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsPolicyForAttachments.SelectParameters.Clear();
                odsPolicyForAttachments.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsViewPolicyForAttachments.DataBind();
            }

            btnAttachments.CssClass = "dokumenti_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnPreviewPolicy.CssClass = "prikaz";
            mvMain.SetActiveView(viewAttachments);
            GenerateAllFiles();
        }
    }

    protected void btnAddAttachment_Click(object sender, EventArgs e) {
        if (FileUpload1.HasFile) {
            try {
                //pateka za dodavanje na datotekata.
                string policyID = string.Empty;
                if (GXGridView1.SelectedValue != null) {
                    policyID = GXGridView1.SelectedDataKey.Value.ToString();
                } else {
                    policyID = GXGridView1SelectedValue.ToString();
                }
                string destinationPath = PolicyAttachmens_Directory + @"\" + policyID;
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
                a.DocumentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.POLISA).ID;
                a.DocumentID = Convert.ToInt32(policyID);
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
        string policyID = string.Empty;
        if (GXGridView1.SelectedValue != null) {
            policyID = GXGridView1.SelectedDataKey.Value.ToString();
        } else {
            policyID = GXGridView1SelectedValue.ToString();
        }
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.POLISA_ZA_ZIV_OSI).ID && a.DocumentID == int.Parse(policyID)).ToList();
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
        string policyID = string.Empty;
        if (GXGridView1.SelectedValue != null) {
            policyID = GXGridView1.SelectedDataKey.Value.ToString();
        } else {
            policyID = GXGridView1SelectedValue.ToString();
        }
        string destinationPath = PolicyAttachmens_Directory + @"\" + policyID;
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
    void SetInsuranceDurationInYears() {
        if (this.PageUser.Role.Name == RolesInfo.BROKERAdmin) {
            pnlBrokerageInformationsPerYears.Visible = true;
        } else {
            pnlBrokerageInformationsPerYears.Visible = false;
        }
        TextBox tbPolicyDurationYears = (TextBox)PoliciesDetailsView.FindControl("tbPolicyDurationYears");
        if (GXGridView1SelectedValue > 0) {
            LifePolicy p = LifePolicy.Get(GXGridView1SelectedValue);
            tbPolicyDurationYears.Text = ((p.EndDate - p.StartDate).Days / 365.15).ToString();
            gvLifePolicyPremiumValue.DataBind();
            gvLifePolicyBrokerage.DataBind();
        }
    }
    protected void DetailsViewPolicyForAttachments_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void DetailsViewPolicyForAttachments_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void odsPolicyForAttachments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }
    protected void btnPreviewPolicy_Click(object sender, EventArgs e) {

        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSourcePolicyDetails.SelectParameters.Clear();
                dvDataSourcePolicyDetails.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                PoliciesDetailsView.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSourcePolicyDetails.SelectParameters.Clear();
                dvDataSourcePolicyDetails.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                PoliciesDetailsView.DataBind();
            }

            btnAttachments.CssClass = "dokumenti";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnPreviewPolicy.CssClass = "prikaz_Active";
            mvMain.SetActiveView(viewEdit);
            SetInsuranceDurationInYears();
        }
    }
    protected void btnMainInformations_Click(object sender, EventArgs e) {
        mvPacket.SetActiveView(viewMainInformations);
        btnMainInformations.CssClass = "PacketButton_Active";
        btnAdditionalInformations.CssClass = "PacketButton";
    }
    protected void btnAdditionalInformations_Click(object sender, EventArgs e) {
        mvPacket.SetActiveView(viewAdditionalInformations);
        btnMainInformations.CssClass = "PacketButton";
        btnAdditionalInformations.CssClass = "PacketButton_Active";
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
            int documentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.POLISA_ZA_ZIV_OSI).ID;
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

    protected void btnDiscard_Click(object sender, EventArgs e) {
        if (GXGridView1SelectedValue > 0) {
            mvMain.SetActiveView(viewDiscardPolicy);
            LifePolicy p = LifePolicy.Get(GXGridView1SelectedValue);
            tbDiscardPolicyNumber.Text = p.PolicyNumber;
            tbDiscardInsuranceCompany.Text = p.InsuranceCompany.ShortName;
            tbDiscardInsuranceSubType.Text = p.InsuranceSubType.ShortDescription;
        }
    }

    protected void dvDataSourcePolicyDetails_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }


    protected void btnDiscardPol_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            LifePolicy p = LifePolicy.Get(GXGridView1SelectedValue);
            if (p != null) {
                p.Discard = true;
                LifePolicy.Table.Context.SubmitChanges();
            }
            GXGridView1.DataBind();
        }
    }


    protected void odsLifePolicyPremiumValue_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("lifePolicyID", GXGridView1SelectedValue);
    }

    protected void odsLifePolicyBrokerage_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("lifePolicyID", GXGridView1SelectedValue);
    }
    protected void odsGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        if (this.PageUser.Role.Name == RolesInfo.BROKERAdmin) {

        } else {
            e.InputParameters.Clear();
            e.InputParameters.Add("foreignKeyName", "UserID");
            e.InputParameters.Add("id", this.PageUser.ID);
        }
    }
    protected void odsFilterGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        if (this.PageUser.Role.Name == RolesInfo.BROKERAdmin) {

        } else {
            e.InputParameters.Clear();
            e.InputParameters.Add("fArgument", FilterControl1.FCFilterArgument);
            e.InputParameters.Add("foreignKeyName", "UserID");
            e.InputParameters.Add("id", this.PageUser.ID);
        }
    }
    protected void odsSearch_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        if (this.PageUser.Role.Name == RolesInfo.BROKERAdmin) {

        } else {
            e.InputParameters.Clear();
            e.InputParameters.Add("sArgument", SearchControl1.SearchArguments);
            e.InputParameters.Add("foreignKeyName", "UserID");
            e.InputParameters.Add("id", this.PageUser.ID);
        }
    }
}
