using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using System.IO;
using System.Web.UI.HtmlControls;
using Broker.Controllers.RateController;

public partial class Broker_PoliciesPerBranch : AuthenticationPage {
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


    public int PolicyID {
        get {
            //if (GXGridView1.SelectedIndex != -1) {
            //    return Convert.ToInt32(GXGridView1.SelectedValue);
            //} else {
            //    return 0;
            //}
            if (ViewState["PolicyID"] != null) {
                return (int)ViewState["PolicyID"];
            } else {
                return PolicyItem.GetPolicyIDForPolicyItemsWithMaxCount();
            }
        }
        set {
            ViewState["PolicyID"] = value;
        }
    }

    public static string PolicyAttachmens_Directory {
        get {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\Policies";
            return Path;
        }
    }



    public int offerItemID {
        get {
            return (ViewState["OfferItemID"] != null) ? int.Parse(ViewState["OfferItemID"].ToString()) : 0;
        }
        set {
        }
    }
    private string _premiumValue;
    public decimal PremiumValue {
        get {
            return (_premiumValue == null) ? 0 : decimal.Parse(_premiumValue);
        }
        set {
            _premiumValue = value.ToString();
        }
    }

    //private int PolicyID {
    //    get {
    //        if (_policy == null) {
    //            return 0;
    //        }
    //        return _policy.ID;
    //    }
    //    set {
    //        _policy.ID = value;
    //    }
    //}


    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = PoliciesView.SelectByFKCountCached("BranchID", this.PageUser.BranchID);
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
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
        GXGridView1.TotalRecords = PoliciesView.SelectSearchByFKCountCached("BranchID", this.PageUser.BranchID, SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }



    protected void PolicyFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e) {

    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        PolicyID = Convert.ToInt32(GXGridView1.SelectedValue);
        if (e.CommandName == "SingleClick") {
            btnPreviewPolicy.Enabled = true;
            btnDiscard.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            PoliciesDetailsView.DataBind();
            DetailsViewPolicyForAttachments.DataBind();
        }
        if (e.CommandName == "DoubleClick") {
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            btnDiscard.Enabled = true;
            mvMain.SetActiveView(viewEdit);
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
        reportControl.ForeignKeyID = this.PageUser.BranchID;
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = PoliciesView.SelectFilterByFKCountCached("BranchID", this.PageUser.BranchID, FilterControl1.FCFilterArgument);
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

    protected void btnDelete_Click1(object sender, EventArgs e) {
        GXGridView1.TotalRecords = Broker.DataAccess.Client.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);

    }


    protected void ClientDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
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
        List<Attachment> listAttachments = Attachment.Table.Where(a => a.DocumentTypeID == Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.POLISA).ID && a.DocumentID == int.Parse(policyID)).ToList();
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

    protected override void OnInit(EventArgs e) {

        int policyID = PolicyID;
        pnlEverything.Controls.Clear();

        List<PolicyItem> listPolicyItems = PolicyItem.Table.Where(pi => pi.PolicyID == policyID).ToList();

        pnlEverything.Controls.Add(pnlViewButtons);
        pnlViewButtons.Controls.Clear();
        pnlEverything.Controls.Add(pnlMainInformation);
        pnlMainInformation.Controls.Add(mvPolicyItem);
        mvPolicyItem.Views.Clear();

        int i = 1;
        foreach (PolicyItem pItem in listPolicyItems) {
            View view = new View();
            view.ID = "view" + i.ToString();
            mvPolicyItem.Controls.Add(view);
            i++;
        }

        base.OnInit(e);
    }
    void buttonForView_Click(object sender, EventArgs e) {
        MultiView mv = mvPolicyItem;
        Button btnView = (Button)sender;
        string viewName = "view" + btnView.ID.Substring(btnView.ID.Length - 1);
        View activeView = (View)mv.Views[Convert.ToInt32(btnView.ID.Substring(btnView.ID.Length - 1)) - 1];

        ControlCollection cc = pnlViewButtons.Controls;
        foreach (System.Web.UI.Control control in cc) {
            try {
                Button button = (Button)control;
                button.CssClass = "PacketButton";
            } catch {
                continue;
            }
        }

        btnView.CssClass = "PacketButton_Active";

        //mv.SetActiveView(activeView);
        mv.ActiveViewIndex = Convert.ToInt32(btnView.ID.Substring(btnView.ID.Length - 1)) - 1;
    }

    protected override void CreateChildControls() {

        int policyID = PolicyID;

        List<PolicyItem> listPolicyItems = PolicyItem.Table.Where(pi => pi.PolicyID == policyID).ToList();

        int n = 1;
        foreach (PolicyItem pItem in listPolicyItems) {
            Button buttonForView = new Button();
            buttonForView.ID = "btnView" + n.ToString();
            buttonForView.Text = pItem.InsuranceSubType.ShortDescription;
            buttonForView.CausesValidation = true;
            buttonForView.CssClass = "PacketButton";
            buttonForView.Click += new EventHandler(buttonForView_Click);
            //pnlEverything.Controls.Add(buttonForView);
            pnlViewButtons.Controls.Add(buttonForView);
            n++;
        }

        int j = 0;
        foreach (PolicyItem pist in listPolicyItems) {
            List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(pist.InsuranceSubTypeID).OrderBy(c => c.OrderNumber).ToList();

            HtmlTable defaultTable = new HtmlTable();
            defaultTable.Width = "690px";
            defaultTable.Style.Add("padding-left", "4px");
            HtmlTableRow firstDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(firstDefaultTableRow);
            HtmlTableCell firstCellFirstRowInDefaultTable = new HtmlTableCell();
            firstCellFirstRowInDefaultTable.Width = "172px";
            firstDefaultTableRow.Cells.Add(firstCellFirstRowInDefaultTable);
            Label lblPolicyNumber = new Label();
            lblPolicyNumber.ID = "lblPolicyNumber" + (j + 1).ToString();
            lblPolicyNumber.Text = "Број на полиса";
            firstCellFirstRowInDefaultTable.Controls.Add(lblPolicyNumber);
            HtmlTableCell secondCellFirstRowInDefaultTable = new HtmlTableCell();
            firstDefaultTableRow.Cells.Add(secondCellFirstRowInDefaultTable);
            TextBox tbPolicyNumber = new TextBox();
            tbPolicyNumber.ID = "tbPolicyNumber" + (j + 1).ToString();
            tbPolicyNumber.ReadOnly = true;
            tbPolicyNumber.Font.Bold = true;
            tbPolicyNumber.Text = pist.PolicyNumber;
            RequiredFieldValidator rfvPolicyNumber = new RequiredFieldValidator();
            rfvPolicyNumber.ID = "rfvPolicyNumber" + (j + 1).ToString();
            rfvPolicyNumber.ErrorMessage = "*";
            rfvPolicyNumber.Display = ValidatorDisplay.Dynamic;
            rfvPolicyNumber.ControlToValidate = tbPolicyNumber.ID;
            secondCellFirstRowInDefaultTable.Controls.Add(tbPolicyNumber);
            secondCellFirstRowInDefaultTable.Controls.Add(rfvPolicyNumber);

            HtmlTableRow secondDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(secondDefaultTableRow);
            HtmlTableCell firstCellSecondRowInDefaultTable = new HtmlTableCell();
            firstCellSecondRowInDefaultTable.Width = "172px";
            secondDefaultTableRow.Cells.Add(firstCellSecondRowInDefaultTable);
            Label lblInsuranceType = new Label();
            lblInsuranceType.ID = "lblInsuranceType" + (j + 1).ToString();
            lblInsuranceType.Text = "Класа на осигурување";
            firstCellSecondRowInDefaultTable.Controls.Add(lblInsuranceType);
            HtmlTableCell secondCellSecondRowInDefaultTable = new HtmlTableCell();
            secondDefaultTableRow.Cells.Add(secondCellSecondRowInDefaultTable);
            TextBox tbInsuranceType = new TextBox();
            tbInsuranceType.ID = "tbInsuranceType" + (j + 1).ToString();
            tbInsuranceType.Text = pist.InsuranceSubType.InsuranceType.ShortName;
            tbInsuranceType.Width = 400;
            tbInsuranceType.ReadOnly = true;
            secondCellSecondRowInDefaultTable.Controls.Add(tbInsuranceType);

            HtmlTableRow thirdDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(thirdDefaultTableRow);
            HtmlTableCell firstCellThirdRowInDefaultTable = new HtmlTableCell();
            thirdDefaultTableRow.Cells.Add(firstCellThirdRowInDefaultTable);
            firstCellThirdRowInDefaultTable.Width = "172px";
            Label lblInsuranceSubType = new Label();
            lblInsuranceSubType.ID = "lblInsuranceSubType" + (j + 1).ToString();
            lblInsuranceSubType.Text = "Подкласа на осигурување";
            firstCellThirdRowInDefaultTable.Controls.Add(lblInsuranceSubType);
            HtmlTableCell secondCellThirdRowInDefaultTable = new HtmlTableCell();
            thirdDefaultTableRow.Cells.Add(secondCellThirdRowInDefaultTable);
            TextBox tbInsuranceSubType = new TextBox();
            tbInsuranceSubType.ID = "tbInsuranceSubType" + (j + 1).ToString();
            tbInsuranceSubType.Text = pist.InsuranceSubType.ShortDescription;
            tbInsuranceSubType.Width = 400;
            tbInsuranceSubType.ReadOnly = true;
            secondCellThirdRowInDefaultTable.Controls.Add(tbInsuranceSubType);

            HtmlTableRow realPolicyDefaultTableRow = new HtmlTableRow();
            HtmlTableCell fifthCellFourthRowInDefaultTable = new HtmlTableCell();
            fifthCellFourthRowInDefaultTable.Width = "172px";
            Label lblRealPolicyValue = new Label();
            lblRealPolicyValue.ID = "lblRealPolicyValue" + (j + 1).ToString();
            lblRealPolicyValue.Text = "Полисирана премија";
            fifthCellFourthRowInDefaultTable.Controls.Add(lblRealPolicyValue);
            HtmlTableCell sixthCellFourthRowInDefaultTable = new HtmlTableCell();
            TextBox tbRealPolicyValue = new TextBox();
            tbRealPolicyValue.ID = "tbRealPolicyValue" + (j + 1).ToString();
            //tbRealPolicyValue.CssClass = "tekstPole";
            tbRealPolicyValue.CssClass = "currencyClass";
            tbRealPolicyValue.ReadOnly = true;
            tbRealPolicyValue.Text = String.Format("{0:#,0.00}", pist.RealPremiumValue);
            sixthCellFourthRowInDefaultTable.Controls.Add(tbRealPolicyValue);
            realPolicyDefaultTableRow.Cells.Add(fifthCellFourthRowInDefaultTable);
            realPolicyDefaultTableRow.Cells.Add(sixthCellFourthRowInDefaultTable);
            defaultTable.Rows.Add(realPolicyDefaultTableRow);

            HtmlTableRow finDiscountPolicyDefaultTableRow = new HtmlTableRow();
            HtmlTableCell fifthCellfinDiscountInDefaultTable = new HtmlTableCell();
            fifthCellfinDiscountInDefaultTable.Width = "172px";
            Label lblFinDiscount = new Label();
            lblFinDiscount.ID = "lblFinDiscount" + (j + 1).ToString();
            lblFinDiscount.Text = "Финансиски попуст (%)";
            fifthCellfinDiscountInDefaultTable.Controls.Add(lblFinDiscount);
            HtmlTableCell sixthCellfinDiscountInDefaultTable = new HtmlTableCell();
            TextBox tbFinDiscountValue = new TextBox();
            tbFinDiscountValue.ID = "tbFinDiscountValue" + (j + 1).ToString();
            tbFinDiscountValue.CssClass = "currencyClass";
            tbFinDiscountValue.ReadOnly = true;
            decimal finDiscountValue = 0;
            if (pist.RealPremiumValue > 0) {
                finDiscountValue = RateController.Scale5((1 - pist.PremiumValue / pist.RealPremiumValue) * 100);
            } 
            tbFinDiscountValue.Text = String.Format("{0:#,0.00}", finDiscountValue);
            sixthCellfinDiscountInDefaultTable.Controls.Add(tbFinDiscountValue);
            finDiscountPolicyDefaultTableRow.Cells.Add(fifthCellfinDiscountInDefaultTable);
            finDiscountPolicyDefaultTableRow.Cells.Add(sixthCellfinDiscountInDefaultTable);
            defaultTable.Rows.Add(finDiscountPolicyDefaultTableRow);

            TextBox tbPolicyValue = new TextBox();
            tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
            tbPolicyValue.CssClass = "currencyClass";
            tbPolicyValue.Text = String.Format("{0:#,0.00}", pist.PremiumValue);
            tbPolicyValue.ReadOnly = true;
            HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
            fourthDefaultTableRow.BgColor = "#FAFAF8";
            defaultTable.Rows.Add(fourthDefaultTableRow);
            HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
            firstCellFourthRowInDefaultTable.Width = "172px";
            fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
            Label lblPolicyValue = new Label();
            lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
            lblPolicyValue.Text = "Премија за наплата";
            firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
            HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
            fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
            secondCellFourthRowInDefaultTable.Controls.Add(tbPolicyValue);

            HtmlTableRow paidValuePolicyDefaultTableRow = new HtmlTableRow();
            HtmlTableCell fifthCellpaidValueInDefaultTable = new HtmlTableCell();
            Label lblPaidValue = new Label();
            lblPaidValue.ID = "lblPaidValue" + (j + 1).ToString();
            lblPaidValue.Text = "Уплатено";
            fifthCellpaidValueInDefaultTable.Width = "172px";
            fifthCellpaidValueInDefaultTable.Controls.Add(lblPaidValue);
            HtmlTableCell sixthCellpaidValueInDefaultTable = new HtmlTableCell();
            TextBox tbPaidValue = new TextBox();
            tbPaidValue.ID = "tbPaidValue" + (j + 1).ToString();
            tbPaidValue.CssClass = "currencyClass";
            tbPaidValue.ReadOnly = true;
            decimal paidValue = 0;
            List<Payment> lst = Payment.GetByPolicyItemID(pist.ID);
            foreach (Payment payment in lst) {
                 paidValue += payment.Value;
            }
            tbPaidValue.Text = String.Format("{0:#,0.00}", paidValue);
            sixthCellpaidValueInDefaultTable.Controls.Add(tbPaidValue);
            paidValuePolicyDefaultTableRow.Cells.Add(fifthCellpaidValueInDefaultTable);
            paidValuePolicyDefaultTableRow.Cells.Add(sixthCellpaidValueInDefaultTable);
            defaultTable.Rows.Add(paidValuePolicyDefaultTableRow);

            HtmlTableRow toPaidValuePolicyDefaultTableRow = new HtmlTableRow();
            HtmlTableCell fifthCelltoPaidValueInDefaultTable = new HtmlTableCell();
            Label lblToPaidValue = new Label();
            lblToPaidValue.ID = "lblToPaidValue" + (j + 1).ToString();
            lblToPaidValue.Text = "Должна премија";
            fifthCelltoPaidValueInDefaultTable.Width = "172px";
            fifthCelltoPaidValueInDefaultTable.Controls.Add(lblToPaidValue);
            HtmlTableCell sixthCelltoPaidValueInDefaultTable = new HtmlTableCell();
            TextBox tbToPaidValue = new TextBox();
            tbToPaidValue.ID = "tbToPaidValue" + (j + 1).ToString();
            tbToPaidValue.CssClass = "currencyClass";
            tbToPaidValue.ReadOnly = true;
            tbToPaidValue.Text = String.Format("{0:#,0.00}", (pist.PremiumValue - paidValue));
            sixthCelltoPaidValueInDefaultTable.Controls.Add(tbToPaidValue);
            toPaidValuePolicyDefaultTableRow.Cells.Add(fifthCelltoPaidValueInDefaultTable);
            toPaidValuePolicyDefaultTableRow.Cells.Add(sixthCelltoPaidValueInDefaultTable);
            defaultTable.Rows.Add(toPaidValuePolicyDefaultTableRow);

            HtmlTableRow seventhDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(seventhDefaultTableRow);
            HtmlTableCell firstCellSeventhRowInDefaultTable = new HtmlTableCell();
            seventhDefaultTableRow.Cells.Add(firstCellSeventhRowInDefaultTable);
            Label lblStatus = new Label();
            lblStatus.ID = "lblStatus" + (j + 1).ToString();
            lblStatus.Text = "Статус";
            firstCellSeventhRowInDefaultTable.Width = "172px";
            firstCellSeventhRowInDefaultTable.Controls.Add(lblStatus);
            HtmlTableCell secondCellSeventhRowInDefaultTable = new HtmlTableCell();
            seventhDefaultTableRow.Cells.Add(secondCellSeventhRowInDefaultTable);
            TextBox tbStatus = new TextBox();
            tbStatus.ID = "tbStatus" + (j + 1).ToString();
            tbStatus.ReadOnly = true;
            tbStatus.Text = pist.Statuse.Description;
            secondCellSeventhRowInDefaultTable.Controls.Add(tbStatus);

            mvPolicyItem.Views[j].Controls.Clear();

            mvPolicyItem.Views[j].Controls.Add(defaultTable);


            HtmlTable table = new HtmlTable();
            table.Width = "690px";
            table.Style.Add("padding-left", "4px");
            int counter = 0;
            foreach (Broker.DataAccess.Control c in listControls) {
                if (c.IsActive) {
                    if (c.ColumnNumber == 1) {
                        HtmlTableRow tableRow = new HtmlTableRow();
                        table.Rows.Add(tableRow);
                        HtmlTableCell tableCell = new HtmlTableCell();
                        tableCell.Width = "172px";
                        tableRow.Cells.Add(tableCell);
                        Label label = new Label();
                        label.ID = c.LabelID + j.ToString();
                        label.Text = c.LabelName;
                        tableCell.Controls.Add(label);
                        HtmlTableCell tableCellSecond = new HtmlTableCell();
                        tableRow.Cells.Add(tableCellSecond);
                        if (c.FieldType.Name == FieldType.CHECKBOX) {
                            CheckBox checkBox = new CheckBox();
                            checkBox.ID = c.TextBoxID + j.ToString();
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pist.ID, c.ID);
                            if (pei != null) {
                                if (pei.Value != null) {
                                    try {
                                        checkBox.Checked = Boolean.Parse(pei.Value);
                                        checkBox.Enabled = false;
                                        tableCellSecond.Controls.Add(checkBox);
                                    } catch {
                                        continue;
                                    }
                                }
                            }
                        } else {
                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID + j.ToString();
                            textbox.ReadOnly = true;
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pist.ID, c.ID);
                            if (pei != null) {
                                textbox.Text = pei.Value;
                                if (pei.Value != string.Empty) {
                                    ValidationDataType vdt = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                    if (vdt == ValidationDataType.Double) {
                                        textbox.CssClass = "currencyClass";
                                        textbox.Text = String.Format("{0:#,0.00}", pei.Value);
                                    }
                                }
                            }
                            tableCellSecond.Controls.Add(textbox);
                        }
                    } else if (c.ColumnNumber == 2) {
                        HtmlTableRow tableRow = table.Rows[counter / 2];
                        if ((counter % 4 == 0) || (counter % 4 == 1)) {
                            tableRow.BgColor = "#FAFAF8";
                        }
                        HtmlTableCell tableCellThird = new HtmlTableCell();
                        tableCellThird.Width = "172px";
                        tableRow.Cells.Add(tableCellThird);
                        Label label = new Label();
                        label.ID = c.LabelID + j.ToString();
                        label.Text = c.LabelName;
                        tableCellThird.Controls.Add(label);
                        HtmlTableCell tableCellForth = new HtmlTableCell();
                        tableCellForth.Width = "172px";
                        tableRow.Cells.Add(tableCellForth);
                        if (c.FieldType.Name == FieldType.CHECKBOX) {
                            CheckBox checkBox = new CheckBox();
                            checkBox.ID = c.TextBoxID + j.ToString();
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pist.ID, c.ID);
                            if (pei != null) {
                                if (pei.Value != null) {
                                    try {
                                        checkBox.Checked = Boolean.Parse(pei.Value);
                                        checkBox.Enabled = false;
                                        tableCellForth.Controls.Add(checkBox);
                                    } catch {
                                        continue;
                                    }
                                }
                            }
                        } else {
                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID + j.ToString();
                            textbox.ReadOnly = true;
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pist.ID, c.ID);
                            if (pei != null) {
                                textbox.Text = pei.Value;
                                if (pei.Value != string.Empty) {
                                    ValidationDataType vdt = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                    if (vdt == ValidationDataType.Double) {
                                        textbox.CssClass = "currencyClass";
                                        textbox.Text = String.Format("{0:#,0.00}", pei.Value);
                                    }
                                }
                            }
                            tableCellForth.Controls.Add(textbox);
                        }
                    }
                }
                counter++;
            }
            mvPolicyItem.Views[j].Controls.Add(table);
            j++;
        }

        base.CreateChildControls();
    }

    protected void DetailsViewPolicyForAttachments_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void DetailsViewPolicyForAttachments_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void odsPolicyForAttachments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }
    protected void btnPreviewPolicy_Click(object sender, EventArgs e) {

        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedDataKey != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                PolicyID = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSourcePolicyDetails.SelectParameters.Clear();
                dvDataSourcePolicyDetails.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                PoliciesDetailsView.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                PolicyID = GXGridView1SelectedValue;
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
            OnInit(e);
            CreateChildControls();
            mvPolicyItem.ActiveViewIndex = 0;
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
            int documentTypeID = Broker.DataAccess.DocumentType.GetByCode(Broker.DataAccess.DocumentType.POLISA).ID;
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
            Policy p = Policy.Get(GXGridView1SelectedValue);
            tbDiscardPolicyNumber.Text = p.PolicyItems[0].PolicyNumber;
            tbDiscardInsuranceCompany.Text = p.InsuranceCompany.ShortName;
            tbDiscardInsuranceSubType.Text = p.PolicyItems[0].InsuranceSubType.ShortDescription;
        }
    }

    protected void btnDiscardPol_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            Policy p = Policy.Get(GXGridView1SelectedValue);
            if (p != null) {
                p.Discard = true;
                Policy.Table.Context.SubmitChanges();
            }
            GXGridView1.DataBind();
        }
    }

    protected void odsSearch_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("sArgument", SearchControl1.SearchArguments);
        e.InputParameters.Add("foreignKeyName", "BranchID");
        e.InputParameters.Add("id", this.PageUser.BranchID);
    }

    protected void odsFilterGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("fArgument", FilterControl1.FCFilterArgument);
        e.InputParameters.Add("foreignKeyName", "BranchID");
        e.InputParameters.Add("id", this.PageUser.BranchID);
    }

    protected void odsGridView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("foreignKeyName", "BranchID");
        e.InputParameters.Add("id", this.PageUser.BranchID);
    }
}


