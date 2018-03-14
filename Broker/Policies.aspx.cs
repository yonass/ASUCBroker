using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Utility;
using Broker.Controllers.PolicyControllers;
using System.Collections.Specialized;
using System.Reflection;
using System.IO;
using System.Web.UI.HtmlControls;
using Broker.Controllers.RateController;
using Broker.Controllers.FinanceControllers;
using Broker.Controllers.ReportControllers;

public partial class Broker_Policies : AuthenticationPage {
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
            GXGridView1.TotalRecords = PoliciesView.SelectCountCached();
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
        GXGridView1.TotalRecords = PoliciesView.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }



    protected void PolicyFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e) {

    }

    void SetSaldoValues() {
        if (GXGridView1SelectedValue > 0) {
            Policy p = Policy.Get(GXGridView1SelectedValue);
            GridViewFinCard.Caption = "Полиса број - " + p.PolicyItems[0].PolicyNumber;
            tbDebtValue.Text = String.Format("{0:#,0.00}", p.PolicyItems[0].PremiumValue);
            decimal demandValue = Payment.GetPaidValueForPolicyItem(p.PolicyItems[0].ID);
            tbDemandValue.Text = String.Format("{0:#,0.00}", demandValue);
            tbSaldoValue.Text = String.Format("{0:#,0.00}", (p.PolicyItems[0].PremiumValue - demandValue));
        }
    }

    void BindPaymentPerPolicy() {
        if (gvRates.DataSourceID == null) {
            gvRates.DataSourceID = odsRates.ID;
        }
        if (gvRates.DataSourceID == "") {
            gvRates.DataSourceID = odsRates.ID;
        }
        if (gvPayments.DataSourceID == null) {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        if (gvPayments.DataSourceID == "") {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        Policy p = Policy.Get(GXGridView1SelectedValue);
        gvRates.Caption = "Полиса - " + p.PolicyItems[0].PolicyNumber;
        gvRates.DataBind();
        gvPayments.Caption = "Полиса - " + p.PolicyItems[0].PolicyNumber;
        gvPayments.DataBind();
        UpdateTextBoxes(p.PolicyItems[0]);
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
            GridViewFinCard.DataBind();
            SetSaldoValues();
            BindPaymentPerPolicy();
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
            GridViewFinCard.DataBind();
            SetSaldoValues();
            BindPaymentPerPolicy();
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
        GXGridView1.TotalRecords = PoliciesView.SelectFilterCountCached(FilterControl1.FCFilterArgument);
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
            firstCellThirdRowInDefaultTable.Width = "172";
            thirdDefaultTableRow.Cells.Add(firstCellThirdRowInDefaultTable);
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
            Label lblRealPolicyValue = new Label();
            lblRealPolicyValue.ID = "lblRealPolicyValue" + (j + 1).ToString();
            lblRealPolicyValue.Text = "Полисирана премија";
            fifthCellFourthRowInDefaultTable.Width = "172px";
            fifthCellFourthRowInDefaultTable.Controls.Add(lblRealPolicyValue);
            HtmlTableCell sixthCellFourthRowInDefaultTable = new HtmlTableCell();
            TextBox tbRealPolicyValue = new TextBox();
            tbRealPolicyValue.ID = "tbRealPolicyValue" + (j + 1).ToString();
            //tbRealPolicyValue.CssClass = "tekstPole";
            tbRealPolicyValue.ReadOnly = true;
            tbRealPolicyValue.Text = String.Format("{0:#,0.00}", pist.RealPremiumValue);
            tbRealPolicyValue.CssClass = "currencyClass";
            sixthCellFourthRowInDefaultTable.Controls.Add(tbRealPolicyValue);
            realPolicyDefaultTableRow.Cells.Add(fifthCellFourthRowInDefaultTable);
            realPolicyDefaultTableRow.Cells.Add(sixthCellFourthRowInDefaultTable);
            defaultTable.Rows.Add(realPolicyDefaultTableRow);

            HtmlTableRow finDiscountPolicyDefaultTableRow = new HtmlTableRow();
            HtmlTableCell fifthCellfinDiscountInDefaultTable = new HtmlTableCell();
            Label lblFinDiscount = new Label();
            lblFinDiscount.ID = "lblFinDiscount" + (j + 1).ToString();
            lblFinDiscount.Text = "Финансиски попуст (%)";
            fifthCellfinDiscountInDefaultTable.Width = "172px";
            fifthCellfinDiscountInDefaultTable.Controls.Add(lblFinDiscount);
            HtmlTableCell sixthCellfinDiscountInDefaultTable = new HtmlTableCell();
            TextBox tbFinDiscountValue = new TextBox();
            tbFinDiscountValue.ID = "tbFinDiscountValue" + (j + 1).ToString();
            //tbFinDiscountValue.CssClass = "tekstPole";
            tbFinDiscountValue.ReadOnly = true;
            decimal realPolValue = 0;
            if (pist.RealPremiumValue > 0) {
                realPolValue = RateController.Scale5((1 - pist.PremiumValue / pist.RealPremiumValue) * 100);
            }
            tbFinDiscountValue.Text = String.Format("{0:#,0.00}", realPolValue);
            tbFinDiscountValue.CssClass = "currencyClass";
            sixthCellfinDiscountInDefaultTable.Controls.Add(tbFinDiscountValue);
            finDiscountPolicyDefaultTableRow.Cells.Add(fifthCellfinDiscountInDefaultTable);
            finDiscountPolicyDefaultTableRow.Cells.Add(sixthCellfinDiscountInDefaultTable);
            defaultTable.Rows.Add(finDiscountPolicyDefaultTableRow);

            TextBox tbPolicyValue = new TextBox();
            tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
            //tbPolicyValue.CssClass = "tekstPole";
            tbPolicyValue.Text = String.Format("{0:#,0.00}", pist.PremiumValue);
            tbPolicyValue.CssClass = "currencyClass";
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
            //tbPaidValue.CssClass = "tekstPole";
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
            //tbToPaidValue.CssClass = "tekstPole";
            tbToPaidValue.CssClass = "currencyClass";
            tbToPaidValue.ReadOnly = true;
            tbToPaidValue.Text = String.Format("{0:#,0.00}", (pist.PremiumValue - paidValue));
            sixthCelltoPaidValueInDefaultTable.Controls.Add(tbToPaidValue);
            toPaidValuePolicyDefaultTableRow.Cells.Add(fifthCelltoPaidValueInDefaultTable);
            toPaidValuePolicyDefaultTableRow.Cells.Add(sixthCelltoPaidValueInDefaultTable);
            defaultTable.Rows.Add(toPaidValuePolicyDefaultTableRow);

            HtmlTableRow fifthDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(fifthDefaultTableRow);
            HtmlTableCell firstCellFifthRowInDefaultTable = new HtmlTableCell();
            fifthDefaultTableRow.Cells.Add(firstCellFifthRowInDefaultTable);
            Label lblBrokeragePercentage = new Label();
            lblBrokeragePercentage.ID = "lblBrokeragePercentage" + (j + 1).ToString();
            firstCellFifthRowInDefaultTable.Width = "172px";
            lblBrokeragePercentage.Text = "Процент на брокеража";
            firstCellFifthRowInDefaultTable.Controls.Add(lblBrokeragePercentage);
            HtmlTableCell secondCellFifthRowInDefaultTable = new HtmlTableCell();
            fifthDefaultTableRow.Cells.Add(secondCellFifthRowInDefaultTable);
            TextBox tbBrokeragePercentage = new TextBox();
            tbBrokeragePercentage.ID = "tbBrokeragePercentage" + (j + 1).ToString();
            tbBrokeragePercentage.Text = pist.BrokeragePercentage.ToString() + " %";
            tbBrokeragePercentage.CssClass = "currencyClass";
            tbBrokeragePercentage.ReadOnly = true;
            secondCellFifthRowInDefaultTable.Controls.Add(tbBrokeragePercentage);

            HtmlTableRow sixthDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(sixthDefaultTableRow);
            HtmlTableCell firstCellSixthRowInDefaultTable = new HtmlTableCell();
            sixthDefaultTableRow.Cells.Add(firstCellSixthRowInDefaultTable);
            Label lblBrokerageValue = new Label();
            lblBrokerageValue.ID = "lblBrokerageValue" + (j + 1).ToString();
            lblBrokerageValue.Text = "Брокеража";
            firstCellSixthRowInDefaultTable.Width = "172px";
            firstCellSixthRowInDefaultTable.Controls.Add(lblBrokerageValue);
            HtmlTableCell secondCellSixthRowInDefaultTable = new HtmlTableCell();
            sixthDefaultTableRow.Cells.Add(secondCellSixthRowInDefaultTable);
            TextBox tbBrokerageValue = new TextBox();
            tbBrokerageValue.ID = "tbBrokerageValue" + (j + 1).ToString();
            tbBrokerageValue.ReadOnly = true;
            tbBrokerageValue.CssClass = "currencyClass";
            tbBrokerageValue.Text = String.Format("{0:#,0.00}", pist.BrokerageValue);
            secondCellSixthRowInDefaultTable.Controls.Add(tbBrokerageValue);

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
                            if (pei != null)
                            {
                                textbox.Text = pei.Value;
                                if (pei.Value != string.Empty) {
                                    ValidationDataType vdt = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                    if (vdt == ValidationDataType.Double) {
                                        textbox.CssClass = "currencyClass";
                                        textbox.Text = String.Format("{0:#,0.00}", decimal.Parse(pei.Value));
                                    }

                                    if (vdt == ValidationDataType.Integer) {
                                        textbox.CssClass = "currencyClass";
                                        textbox.Text = String.Format("{0:#,0}", int.Parse(pei.Value));
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
                            if (pei != null)
                            {
                                textbox.Text = pei.Value;
                                if (pei.Value != string.Empty) {
                                    ValidationDataType vdt = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                    if (vdt == ValidationDataType.Double) {
                                        textbox.CssClass = "currencyClass";
                                        textbox.Text = String.Format("{0:#,0.00}", decimal.Parse(pei.Value));
                                    }

                                    if (vdt == ValidationDataType.Integer) {
                                        textbox.CssClass = "currencyClass";
                                        textbox.Text = String.Format("{0:#,0}", int.Parse(pei.Value));
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
            SetPaymentInformationForPolicy();
        }
    }
    protected void btnMainInformations_Click(object sender, EventArgs e) {
        mvPacket.SetActiveView(viewMainInformations);
        btnMainInformations.CssClass = "PacketButton_Active";
        btnAdditionalInformations.CssClass = "PacketButton";
        btnFinanceCardInformations.CssClass = "PacketButton";
        btnPaymentsPerPolicy.CssClass = "PacketButton";
    }

    protected void btnFinanceCardInformations_Click(object sender, EventArgs e) {
        GridViewFinCard.DataBind();
        SetSaldoValues();
        mvPacket.SetActiveView(viewFinancialCard);
        btnMainInformations.CssClass = "PacketButton";
        btnAdditionalInformations.CssClass = "PacketButton";
        btnFinanceCardInformations.CssClass = "PacketButton_Active";
        btnPaymentsPerPolicy.CssClass = "PacketButton";
    }

    protected void btnPaymentsPerPolicy_Click(object sender, EventArgs e) {
        mvPacket.SetActiveView(viewPaymentsPerPolicy);
        btnMainInformations.CssClass = "PacketButton";
        btnAdditionalInformations.CssClass = "PacketButton";
        btnFinanceCardInformations.CssClass = "PacketButton";
        btnPaymentsPerPolicy.CssClass = "PacketButton_Active";
    }

    protected void btnAdditionalInformations_Click(object sender, EventArgs e) {
        mvPacket.SetActiveView(viewAdditionalInformations);
        btnMainInformations.CssClass = "PacketButton";
        btnAdditionalInformations.CssClass = "PacketButton_Active";
        btnFinanceCardInformations.CssClass = "PacketButton";
        btnPaymentsPerPolicy.CssClass = "PacketButton";
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

    protected void btnPrintFinCard_Click(object sender, EventArgs e) {
        Policy p = Policy.Get(GXGridView1SelectedValue);
        PrintFinCardController.PrintFinCard(p, null, "");
    }

    protected void dvDataSourcePolicyDetails_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }
    protected void odsFinCard_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        Policy pol = Policy.Get(GXGridView1SelectedValue);
        e.InputParameters.Add("p", pol);
        e.InputParameters.Add("totSaldo", (decimal)0);
    }
    protected void btnAnexDeal_Click(object sender, EventArgs e) {
        Policy p = Policy.Get(GXGridView1SelectedValue);
        PrintFactures.PrintAnex(p);
    }

    protected void btnDeletePolicy_Click(object sender, EventArgs e)
    {
        Policy p = Policy.Get(GXGridView1SelectedValue);
        string retStr = PolicyController.DeletePolicy(p.PolicyItems[0].ID);
        RegisterStartupScript("myAlert", "<script>alert('" + retStr + "')</script>");
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAttachments.CssClass = "dokumenti";
        btnPreviewPolicy.CssClass = "prikaz";
        GXGridView1.TotalRecords = PoliciesView.SelectCountCached();
        mvMain.SetActiveView(viewGrid);
    }

    void SetPaymentInformationForPolicy() {
        Policy p = Policy.Get(GXGridView1SelectedValue);
        if (p.PolicyItems[0].IsPaidInBrokerHouse) {
            if (p.PolicyItems[0].Rates.Count > 1) {
                tbPolicyItemPaymentType.Text = "ПЛАЌАЊЕ НА РАТИ";
                btnAnexDeal.Enabled = true;
            } else {
                tbPolicyItemPaymentType.Text = "ЕДНОКРАТНО ПЛАЌАЊЕ";
                btnAnexDeal.Enabled = false;
            }
        } else {
            tbPolicyItemPaymentType.Text = "ДИРЕКТНО ВО О. КОМПАНИЈА";
            btnAnexDeal.Enabled = true;
        }
        Broker.DataAccess.Facture brokFacture = PolicyItemFactureItem.GetByPolicyItemIDForBrokerage(p.PolicyItems[0].ID);
        if (brokFacture != null) {
            tbFactureForBrokerage.Text = brokFacture.FactureNumber;
        } else {
            tbFactureForBrokerage.Text = string.Empty;
        }
        Broker.DataAccess.Facture clientFacture = PolicyItemFactureItem.GetByPolicyItemID(p.PolicyItems[0].ID);
        if (clientFacture != null) {
            tbFactureForClientNumber.Text = clientFacture.FactureNumber;
        } else {
            tbFactureForClientNumber.Text = string.Empty;
        }
        IncomingFacture incF = IncomingFacturePolicyItem.GetByPolicyItem(p.PolicyItems[0].ID);
        if (incF != null) {
            tbIncomeFactureNumber.Text = incF.FactureNumber;
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
    protected void odsRates_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        Policy p = Policy.Get(GXGridView1SelectedValue);
        e.InputParameters.Clear();
        e.InputParameters.Add("policyItemID", p.PolicyItems[0].ID);
    }
    protected void odsPayments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        Policy p = Policy.Get(GXGridView1SelectedValue);
        e.InputParameters.Clear();
        e.InputParameters.Add("policyItemID", p.PolicyItems[0].ID);
    }
    protected void odsPayments_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Payment payment = e.InputParameters["newEntity"] as Payment;
        payment.ValidatePayment();
        if (payment.ValidationErrors.Count > 0) {
            e.Cancel = true;
            RegisterStartupScript("myAlert", "<script>alert('" + payment.ValidationErrors.First().Value.ErrorMessage + "')</script>");
        }
    }
    protected void gvPayments_RowUpdating(object sender, GridViewUpdateEventArgs e) {
        //DropDownList ddlBankCreditCards = (DropDownList)gvPayments.FindControl("ddlBankCreditCards");
        //DropDownList ddlBanks = (DropDownList)gvPayments.FindControl("ddlBanks");
        DropDownList ddlBankCreditCards = (DropDownList)gvPayments.Rows[e.RowIndex].FindControl("ddlBankCreditCards");
        DropDownList ddlBanks = (DropDownList)gvPayments.Rows[e.RowIndex].FindControl("ddlBanks");
        if (ddlBankCreditCards.SelectedValue != null) {
            if (ddlBankCreditCards.SelectedValue != "None") {
                if (ddlBankCreditCards.SelectedValue != "") {
                    e.NewValues["BankCreditCardID"] = Convert.ToInt32(ddlBankCreditCards.SelectedValue);
                }
            }
        }
        if (ddlBanks.SelectedValue != null) {
            if (ddlBanks.SelectedValue != "None") {
                if (ddlBanks.SelectedValue != "") {
                    e.NewValues["BankslipBankID"] = Convert.ToInt32(ddlBanks.SelectedValue);
                }
            } else {
                e.NewValues["BankslipBankID"] = null;
            }
        }
    }

    protected void ddlPaymentTypesInpaymentsPerPolicy_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlPaymentTypes = (DropDownList)sender;
        GridViewRow gvr = (GridViewRow)ddlPaymentTypes.Parent.Parent;
        DropDownList ddlBankCreditCards = (DropDownList)gvr.FindControl("ddlBankCreditCards");
        Broker.DataAccess.PaymentType pt = Broker.DataAccess.PaymentType.Get(Convert.ToInt32(ddlPaymentTypes.SelectedValue));
        if (pt.Code != Broker.DataAccess.PaymentType.CREDITCARD)
        {
            ddlBankCreditCards.SelectedValue = "None";
            ddlBankCreditCards.Enabled = false;
        } else {
            ddlBankCreditCards.Enabled = true;
        }
    }

    protected void odsRates_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Rate rate = e.InputParameters["newEntity"] as Rate;
        rate.ValidateRate();
        if (rate.ValidationErrors.Count > 0) {
            e.Cancel = true;
            RegisterStartupScript("myAlert", "<script>alert('" + rate.ValidationErrors.First().Value.ErrorMessage + "')</script>");
        }
    }
    protected void gvRates_RowUpdating(object sender, GridViewUpdateEventArgs e) {

    }
    protected void gvRates_RowEditing(object sender, GridViewEditEventArgs e) {
        gvRates.SelectedIndex = e.NewEditIndex;
        gvRates.SelectedRow.RowState = DataControlRowState.Edit;
        if (gvRates.DataSourceID == null) {
            gvRates.DataSourceID = odsRates.ID;
        }
        if (gvRates.DataSourceID == string.Empty) {
            gvRates.DataSourceID = odsRates.ID;
        }
        gvRates.DataBind();
    }
    protected void odsPayments_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {

    }

    void UpdateTextBoxes(PolicyItem pi) {
        tbPolicyPremiumCost.Text = String.Format("{0:#,0.00}", pi.PremiumValue);
        decimal paidValue = Payment.GetPaidValueForPolicyItemExtend(pi.ID);
        tbPolicyTotalPaidValue.Text = String.Format("{0:#,0.00}", paidValue);
        tbPolicyForPaidValue.Text = String.Format("{0:#,0.00}", (pi.PremiumValue - paidValue));
        tbDateOfPayment.Text = DateTime.Today.ToShortDateString();
        tbValueOfPayment.Text = "0";
    }

    protected void btnInsert_Click(object sender, EventArgs e) {
        DateTime inputDate = Convert.ToDateTime(tbDateOfPayment.Text);
        Policy policy = Policy.Get(GXGridView1SelectedValue);
        PolicyItem pi = policy.PolicyItems[0];
        if (inputDate.Date > DateTime.Today) {
            RegisterStartupScript("myAlert", "<script>alert('Не е можно внесување на датуми поголеми од денешниот!')</script>");
            return;
        } else {
            List<Payment> listPayments = Payment.GetByPolicyItemIDExtend(pi.ID);
            decimal paymentTotalValue = 0;
            foreach (Payment payment in listPayments) {
                paymentTotalValue += payment.Value;
            }
            if (Convert.ToDecimal(tbValueOfPayment.Text) > (pi.PremiumValue - paymentTotalValue)) {
                RegisterStartupScript("myAlert", "<script>alert('Поголем износ од преостанатиот износ за плаќање!')</script>");
            } else {
                decimal valueFromClient = Convert.ToDecimal(tbValueOfPayment.Text);
                while (valueFromClient > 0) {
                    Rate currentRate = Rate.GetCurrentRateForPaymentExtend(pi.ID);
                    Payment newPayment = new Payment();
                    newPayment.Date = Convert.ToDateTime(tbDateOfPayment.Text);
                    newPayment.RateID = currentRate.ID;
                    newPayment.IsCashReported = false;
                    Broker.DataAccess.PaymentType pt = Broker.DataAccess.PaymentType.Get(Convert.ToInt32(ddlPaymentTypes.SelectedValue));
                    newPayment.PaymentTypeID = pt.ID;
                    if (pt.Code == Broker.DataAccess.PaymentType.CREDITCARD)
                    {
                        newPayment.BankCreditCardID = BankCreditCard.GetByBankAndCard(Convert.ToInt32(ddlBank.SelectedValue), Convert.ToInt32(ddlCardTypes.SelectedValue)).ID;
                    }
                    newPayment.UserID = this.PageUser.ID;
                    newPayment.BranchID = this.PageUser.BranchID;
                    if (tbBankslipNumber.Text.Trim() != string.Empty) {
                        newPayment.BankslipNumber = tbBankslipNumber.Text;
                        newPayment.BankslipBankID = Convert.ToInt32(ddlBankslipBanks.SelectedValue);
                    }
                    if (valueFromClient >= currentRate.Value) {
                        newPayment.Value = currentRate.Value - Payment.GetPaidValueForRateExtend(currentRate.ID);
                        newPayment.Insert();
                        decimal basicValue = newPayment.Value;
                        decimal k = basicValue / newPayment.Rate.PolicyItem.PremiumValue;
                        List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                        foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                            if (pei != null) {
                                decimal peiValue = 0;
                                decimal.TryParse(pei.Value, out peiValue);
                                basicValue -= k * peiValue;
                                if (peiValue > 0) {
                                    PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                    ppist.PaymentID = newPayment.ID;
                                    ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                    ppist.PaidValue = k * peiValue;
                                    ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                    ppist.Insert();
                                }
                            }
                        }
                        if (basicValue > 0) {
                            PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                            ppist.PaymentID = newPayment.ID;
                            ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                            ppist.PaidValue = basicValue;
                            ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                            ppist.Insert();
                        }
                        valueFromClient -= newPayment.Value;
                        //currentRate.PaidValue += newPayment.Value;
                        //Rate.Table.Context.SubmitChanges();
                        decimal crPaidValue = currentRate.PaidValue + newPayment.Value;
                        string tmpvalue = crPaidValue.ToString();
                        tmpvalue = tmpvalue.Replace(',', '.');
                        string query = @"UPDATE Rates SET PaidValue = " + tmpvalue + " WHERE ID = " + currentRate.ID;
                        DataClassesDataContext dc = new DataClassesDataContext();
                        dc.ExecuteCommand(query);
                    } else {
                        if (valueFromClient <= (currentRate.Value - currentRate.PaidValue)) {
                            newPayment.Value = valueFromClient;
                            newPayment.Insert();
                            decimal basicValue = newPayment.Value;
                            decimal k = basicValue / newPayment.Rate.PolicyItem.PremiumValue;
                            List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                            foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                                PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                                if (pei != null) {
                                    decimal peiValue = 0;
                                    decimal.TryParse(pei.Value, out peiValue);
                                    basicValue -= k * peiValue;
                                    if (peiValue > 0) {
                                        PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                        ppist.PaymentID = newPayment.ID;
                                        ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                        ppist.PaidValue = k * peiValue;
                                        ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                        ppist.Insert();
                                    }
                                }
                            }

                            if (basicValue > 0) {
                                PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                ppist.PaymentID = newPayment.ID;
                                ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                                ppist.PaidValue = basicValue;
                                ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                ppist.Insert();
                            }
                            //currentRate.PaidValue += valueFromClient;
                            //Rate.Table.Context.SubmitChanges();
                            decimal crPaidValue = currentRate.PaidValue + valueFromClient;
                            string tmpvalue = crPaidValue.ToString();
                            tmpvalue = tmpvalue.Replace(',', '.');
                            string query = @"UPDATE Rates SET PaidValue = " + tmpvalue + " WHERE ID = " + currentRate.ID;
                            DataClassesDataContext dc = new DataClassesDataContext();
                            dc.ExecuteCommand(query);
                            break;
                        } else {
                            newPayment.Value = (currentRate.Value - currentRate.PaidValue);
                            newPayment.Insert();
                            decimal basicValue = newPayment.Value;
                            decimal k = basicValue / newPayment.Rate.PolicyItem.PremiumValue;
                            List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                            foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                                PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                                if (pei != null) {
                                    decimal peiValue = 0;
                                    decimal.TryParse(pei.Value, out peiValue);
                                    basicValue -= k * peiValue;
                                    if (peiValue > 0) {
                                        PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                        ppist.PaymentID = newPayment.ID;
                                        ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                        ppist.PaidValue = k * peiValue;
                                        ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                        ppist.Insert();
                                    }
                                }
                            }

                            if (basicValue > 0) {
                                PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                ppist.PaymentID = newPayment.ID;
                                ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                                ppist.PaidValue = basicValue;
                                ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                ppist.Insert();
                            }
                            //currentRate.PaidValue += valueFromClient;
                            //Rate.Table.Context.SubmitChanges();
                            decimal crPaidValue = currentRate.PaidValue + newPayment.Value; 
                            string tmpvalue = crPaidValue.ToString();
                            tmpvalue = tmpvalue.Replace(',', '.');
                            string query = @"UPDATE Rates SET PaidValue = " + tmpvalue + " WHERE ID = " + currentRate.ID;
                            DataClassesDataContext dc = new DataClassesDataContext();
                            dc.ExecuteCommand(query);
                            valueFromClient -= newPayment.Value;
                        }
                    }
                }
                Broker.DataAccess.Facture.UpdatePaidStatusForFactureExtend(pi.ID);
            }
        }

        BindPaymentPerPolicy();
    }

    protected void ddlPaymentTypes_SelectedIndexChanged(object sender, EventArgs e) {
        Broker.DataAccess.PaymentType pt = Broker.DataAccess.PaymentType.Get(Convert.ToInt32(ddlPaymentTypes.SelectedValue));
        if (pt.Code == Broker.DataAccess.PaymentType.CREDITCARD)
        {
            ddlBank.Enabled = true;
            ddlCardTypes.Enabled = true;
        } else {
            ddlBank.Enabled = false;
            ddlCardTypes.Enabled = false;
        }

    }
    protected void odsBank_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        ddlCardTypes.DataBind();
    }
    protected void gvPayments_RowEditing(object sender, GridViewEditEventArgs e) {

        gvPayments.SelectedIndex = e.NewEditIndex;
        gvPayments.SelectedRow.RowState = DataControlRowState.Edit;
        if (gvPayments.DataSourceID == null) {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        if (gvPayments.DataSourceID == string.Empty) {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        gvPayments.DataBind();
    }
    protected void gvPayments_RowUpdated(object sender, GridViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            BindPaymentPerPolicy();
            Policy p = Policy.Get(GXGridView1SelectedValue);
            DataClassesDataContext dc = new DataClassesDataContext();
            string strQuery = "SELECT p.* from Payments p, Rates r Where p.RateID = r.ID AND PolicyItemID = " + p.PolicyItems[0].ID + " ORDER BY r.Number";
            List<Payment> lstPayments = dc.ExecuteQuery<Payment>(strQuery).ToList();
            gvPayments.DataSourceID = null;
            gvPayments.DataSource = lstPayments;
            gvPayments.DataBind();
        }
    }
    protected void gvRates_RowUpdated(object sender, GridViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            BindPaymentPerPolicy();
            Policy p = Policy.Get(GXGridView1SelectedValue);
            DataClassesDataContext dc = new DataClassesDataContext();
            string strQuery = "SELECT r.* from Rates r Where PolicyItemID = " + p.PolicyItems[0].ID + " ORDER BY Number";
            List<Rate> lstRates = dc.ExecuteQuery<Rate>(strQuery).ToList();
            gvRates.DataSourceID = null;
            gvRates.DataSource = lstRates;
            gvRates.DataBind();
        }
    }
    protected void gvPayments_DataBinding(object sender, EventArgs e) {

    }



    protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e) {

        object o = e.Row;
        if (e.Row.RowIndex >= 0) {
            DropDownList ddlBankCreditCards = (DropDownList)e.Row.FindControl("ddlBankCreditCards");
            DropDownList ddlBanks = (DropDownList)e.Row.FindControl("ddlBanks");
            Label lblCreditCardNameInPayment = (Label)e.Row.FindControl("lblCreditCardNameInPayment");
            Label lblBankslipBankName = (Label)e.Row.FindControl("lblBankslipBankName");
            Payment p = (Payment)e.Row.DataItem;
            if (p != null) {
                if (p.BankCreditCardID != null) {
                    if (ddlBankCreditCards != null) {
                        ddlBankCreditCards.SelectedValue = p.BankCreditCardID.Value.ToString();
                        ddlBankCreditCards.Enabled = true;
                    }
                    if (lblCreditCardNameInPayment != null) {
                        lblCreditCardNameInPayment.Text = p.BankCreditCard.Bank.Name + " - " + p.BankCreditCard.CreditCard.Name;
                    }
                } else {
                    if (ddlBankCreditCards != null) {
                        ddlBankCreditCards.Enabled = false;
                    }
                }
                if (p.BankslipBankID != null) {
                    if (ddlBank != null) {
                        ddlBank.SelectedValue = p.BankslipBankID.Value.ToString();
                    }
                    if (lblBankslipBankName != null) {
                        lblBankslipBankName.Text = p.Bank.Name;
                    }
                }
            }
        }
    }
    protected void gvPayments_RowCreated(object sender, GridViewRowEventArgs e) {
        object o = e.Row;
        if (e.Row.RowIndex >= 0) {

            DropDownList ddlBankCreditCards = (DropDownList)e.Row.FindControl("ddlBankCreditCards");
            DropDownList ddlBanks = (DropDownList)e.Row.FindControl("ddlBanks");
            Label lblCreditCardNameInPayment = (Label)e.Row.FindControl("lblCreditCardNameInPayment");
            Label lblBankslipBankName = (Label)e.Row.FindControl("lblBankslipBankName");
            Payment p = (Payment)e.Row.DataItem;
            if (p != null) {
                if (p.BankCreditCardID != null) {
                    if (ddlBankCreditCards != null) {
                        ddlBankCreditCards.SelectedValue = p.BankCreditCardID.Value.ToString();
                        ddlBankCreditCards.Enabled = true;
                    }
                    if (lblCreditCardNameInPayment != null) {
                        lblCreditCardNameInPayment.Text = p.BankCreditCard.Bank.Name + " - " + p.BankCreditCard.CreditCard.Name;
                    }
                } else {
                    if (ddlBankCreditCards != null) {
                        ddlBankCreditCards.Enabled = false;
                    }
                }
                if (p.BankslipBankID != null) {
                    if (ddlBank != null) {
                        ddlBank.SelectedValue = p.BankslipBankID.Value.ToString();
                    }
                    if (lblBankslipBankName != null) {
                        lblBankslipBankName.Text = p.Bank.Name;
                    }
                }
            }
        }
    }
    protected void gvPayments_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "Edit") {
            foreach (GridViewRow gvr in gvPayments.Rows) {
                object o = e.CommandArgument;
                DropDownList ddlBankCreditCards = (DropDownList)gvPayments.FindControl("ddlBankCreditCards");
                DropDownList ddlBanks = (DropDownList)gvPayments.FindControl("ddlBanks");
                Payment p = (Payment)gvr.DataItem;
                if (p != null) {
                    if (p.BankCreditCardID != null) {
                        if (ddlBankCreditCards != null) {
                            ddlBankCreditCards.SelectedValue = p.BankCreditCardID.Value.ToString();
                        }
                    }
                    if (p.BankslipBankID != null) {
                        if (ddlBank != null) {
                            ddlBank.SelectedValue = p.BankslipBankID.Value.ToString();
                        }
                    }
                }
            }
        }
    }

    protected void odsPayments_Deleting(object sender, ObjectDataSourceMethodEventArgs e) {
        Payment payment = (Payment)e.InputParameters["entityToDelete"];
        payment.ValidatePaymentOnDeleting();
        if (payment.ValidationErrors.Count > 0) {
            e.Cancel = true;
            RegisterStartupScript("myAlert", "<script>alert('" + payment.ValidationErrors.First().Value.ErrorMessage + "')</script>");
        }
    }
    protected void gvPayments_RowDeleted(object sender, GridViewDeletedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            BindPaymentPerPolicy();
            Policy p = Policy.Get(GXGridView1SelectedValue);
            DataClassesDataContext dc = new DataClassesDataContext();
            string strQuery = "SELECT p.* from Payments p, Rates r Where p.RateID = r.ID AND PolicyItemID = " + p.PolicyItems[0].ID + " ORDER BY r.Number";
            List<Payment> lstPayments = dc.ExecuteQuery<Payment>(strQuery).ToList();
            gvPayments.DataSourceID = null;
            gvPayments.DataSource = lstPayments;
            gvPayments.DataBind();
        }
    }
    protected void gvPayments_RowDeleting(object sender, GridViewDeleteEventArgs e) {
        gvPayments.SelectedIndex = e.RowIndex;
        gvPayments.SelectedRow.RowState = DataControlRowState.Normal;
        if (gvPayments.DataSourceID == null) {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        if (gvPayments.DataSourceID == string.Empty) {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        gvPayments.DataBind();
    }
}
