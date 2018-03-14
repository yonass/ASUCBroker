using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ManagementControllers;
using System.IO;

public partial class Broker_Damages : AuthenticationPage
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


    public static string DamageAttachmens_Directory
    {
        get
        {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\Damages";
            return Path;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GXGridView1.TotalRecords = ViewDamage.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            mvMain.SetActiveView(viewEdit);
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            //btnDelete.CssClass = "izbrisi";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnAttachments.CssClass = "dokumenti";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewEdit);
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
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
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnAttachments.CssClass = "dokumenti";
        btnSearch.CssClass = "prebaraj";
    }
    protected void SearchControl1_Search(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = ViewDamage.SelectSearchCountCached(SearchControl1.SearchArguments);
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
            dvDamageAttachemtsPreview.DataBind();
        }
        if (e.CommandName == "DoubleClick")
        {
            btnEdit.Enabled = true;
            btnAttachments.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            mvMain.SetActiveView(viewEdit);
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            dvDamageAttachemtsPreview.DataBind();
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnAttachments.CssClass = "dokumenti";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e)
    {
        GXGridView1.TotalRecords = ViewDamage.SelectFilterCountCached(FilterControl1.FCFilterArgument);
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
            //btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
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
            GXGridView1.TotalRecords = ViewDamage.SelectCountCached();
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
            GXGridView1.TotalRecords = ViewDamage.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GXGridView1.TotalRecords = ViewDamage.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
    {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        e.Values["EntryByUserID"] = this.PageUser.ID;
        TextBox tbPolicyNumber = DetailsView1.FindControl("tbPolicyNumber") as TextBox;
        string policyNumber = tbPolicyNumber.Text;
        DropDownList ddlInsuranceSubTypes = DetailsView1.FindControl("ddlInsuranceSubTypes") as DropDownList;
        int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue);
        DropDownList ddlInsuranceCompanies = DetailsView1.FindControl("ddlInsuranceCompanies") as DropDownList;
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        PolicyItem pi = PolicyItem.Table.Where(pol => pol.PolicyNumber == policyNumber && pol.InsuranceSubTypeID == insuranceSubTypeID && pol.Policy.InsuranceCompanyID == insuranceCompanyID).SingleOrDefault();
        if (pi != null)
        {
            e.Values["PolicyItemID"] = pi.ID;
        }

    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Damage d = e.InputParameters["newEntity"] as Damage;
        int policyItemID = Convert.ToInt32(e.InputParameters["PolicyItemID"]);
        TextBox tbPolicyNumber = DetailsView1.FindControl("tbPolicyNumber") as TextBox;
        string policyNumber = tbPolicyNumber.Text;
        DropDownList ddlInsuranceSubTypes = DetailsView1.FindControl("ddlInsuranceSubTypes") as DropDownList;
        int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue);
        DropDownList ddlInsuranceCompanies = DetailsView1.FindControl("ddlInsuranceCompanies") as DropDownList;
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        DamageController.ValidateUpdateDamageNumber(d.ID, d.DamageNumber, policyNumber, insuranceSubTypeID, insuranceCompanyID, policyItemID);
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Damage d = e.InputParameters["entityToInsert"] as Damage;
        TextBox tbPolicyNumber = DetailsView1.FindControl("tbPolicyNumber") as TextBox;
        string policyNumber = tbPolicyNumber.Text;
        DropDownList ddlInsuranceSubTypes = DetailsView1.FindControl("ddlInsuranceSubTypes") as DropDownList;
        int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue);
        DropDownList ddlInsuranceCompanies = DetailsView1.FindControl("ddlInsuranceCompanies") as DropDownList;
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        DamageController.ValidateInsertDamageNumber(d.DamageNumber, policyNumber, insuranceSubTypeID, insuranceCompanyID);
    }

    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

    }
    protected void btnAttachments_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            if (GXGridView1.SelectedValue != null)
            {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsDamageAttachemtsPreview.SelectParameters.Clear();
                odsDamageAttachemtsPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvDamageAttachemtsPreview.DataBind();
            } else
            {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsDamageAttachemtsPreview.SelectParameters.Clear();
                odsDamageAttachemtsPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvDamageAttachemtsPreview.DataBind();
            }
            mvMain.SetActiveView(viewAttachments);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAttachments.CssClass = "dokumenti_Active";
            dvDamageAttachemtsPreview.DataBind();
            GenerateAllFiles();
        }
    }



    protected void btnAddAttachment_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            try
            {
                string damageID = string.Empty;
                if (GXGridView1.SelectedValue != null)
                {
                    damageID = GXGridView1.SelectedDataKey.Value.ToString();
                } else
                {
                    damageID = GXGridView1SelectedValue.ToString();
                }
                string destinationPath = DamageAttachmens_Directory + @"\" + damageID;
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
                a.DocumentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.STETA).ID;
                a.DocumentID = Convert.ToInt32(damageID);
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
        string damageID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            damageID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            damageID = GXGridView1SelectedValue.ToString();
        }
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.STETA).ID && a.DocumentID == int.Parse(damageID)).ToList();
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
        string damageID = string.Empty;
        if (GXGridView1.SelectedValue != null)
        {
            damageID = GXGridView1.SelectedDataKey.Value.ToString();
        } else
        {
            damageID = GXGridView1SelectedValue.ToString();
        }
        string destinationPath = DamageAttachmens_Directory + @"\" + damageID;
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
    protected void ddlInsuranceTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlInsuranceSubTypes = DetailsView1.FindControl("ddlInsuranceSubTypes") as DropDownList;
        ddlInsuranceSubTypes.DataBind();
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        e.OldValues["EntryByUserID"] = this.PageUser.ID;
        e.NewValues["EntryByUserID"] = this.PageUser.ID;
        TextBox tbPolicyNumber = DetailsView1.FindControl("tbPolicyNumber") as TextBox;
        string policyNumber = tbPolicyNumber.Text;
        DropDownList ddlInsuranceSubTypes = DetailsView1.FindControl("ddlInsuranceSubTypes") as DropDownList;
        int insuranceSubTypeID = Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue);
        DropDownList ddlInsuranceCompanies = DetailsView1.FindControl("ddlInsuranceCompanies") as DropDownList;
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        PolicyItem pi = PolicyItem.Table.Where(pol => pol.PolicyNumber == policyNumber && pol.InsuranceSubTypeID == insuranceSubTypeID && pol.Policy.InsuranceCompanyID == insuranceCompanyID).SingleOrDefault();
        if (pi != null)
        {
            e.OldValues["PolicyItemID"] = pi.ID;
            e.NewValues["PolicyItemID"] = pi.ID;
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
            int documentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.STETA).ID;
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