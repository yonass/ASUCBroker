using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using System.Web.UI.HtmlControls;
using Broker.Controllers.RateController;
using Broker.Controllers.ReportControllers;

public partial class Broker_PolicyFromOffer : AuthenticationPage
{
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            //PolicyMultiView.SetActiveView(MainView);
            multiView.SetActiveView(firstView);

        }
        CreateGridView(this.Page, null);
    }
    private int _insuranceSubTypeID;
    public int InsuranceSubtypeID {
        get {
            if (ddlOfferItemInsuranceSubType.SelectedIndex!=-1){
                OfferItemsView oiv = OfferItemsView.GetByOfferItemID(int.Parse(ddlOfferItemInsuranceSubType.SelectedValue));
                //return int.Parse(ddlOfferItemInsuranceSubType.SelectedValue);
                return oiv.InsuranceSubTypeID;
            }
            return InsuranceSubType.Table.First().ID;
        }
        set {
            _insuranceSubTypeID = value;
        }
    }
    private int _dealID;
    public int DealID {
        get {
            if (ViewState["DealID"] == null) {
                // return Deal.Table.First().ID;
                List<Deal> dList = Deal.GetBySubType(InsuranceSubtypeID);
                if (dList != null) {
                    return dList.First().ID;
                } else {
                    return 0;
                }

            }
            return int.Parse(ViewState["DealID"].ToString());
        }
        set {
            ViewState["DealID"] = value;
        }
    }
    public int PolicyID {
        get {
            if (ViewState["PolicyID"] == null) {
                return 0;
            } else {
                return int.Parse(ViewState["PolicyID"].ToString());
            }
        }
        set {
            ViewState["PolicyID"] = value;
        }
    }


    protected void btnSearchOffer_Click(object sender, EventArgs e) {
        if (tbOfferNumber.Text.Trim() != string.Empty) {
            Offer offer = Offer.GetByNumber(tbOfferNumber.Text.Trim());
            if (offer != null) {
                odsOfferItemInsuranceSubType.SelectParameters.Clear();
                odsOfferItemInsuranceSubType.SelectParameters.Add("offerNumber",tbOfferNumber.Text.Trim());
                //odsOfferItemInsuranceSubType.DataBind();
                ddlOfferItemInsuranceSubType.DataBind();
                if (ddlOfferItemInsuranceSubType.SelectedIndex != -1) {
                    int i = int.Parse(ddlOfferItemInsuranceSubType.SelectedValue);
                    SetInsuranceCompany(i);
                }
                tbClientName.Text = offer.Client.Name;
                tbClientEMBG.Text = offer.Client.EMBG;
                tbOwnerName.Text = offer.Client1.Name;
                tbOwnerEMBG.Text = offer.Client1.EMBG;
                CreateChildControls();
            }
        }
    }

    protected void insuranceSubTypeChanged(object sender, EventArgs e) {
        if (ddlOfferItemInsuranceSubType.SelectedIndex != -1) {
            int i = int.Parse(ddlOfferItemInsuranceSubType.SelectedValue);
            SetInsuranceCompany(i);
            CreateChildControls();
        }
        
    }

    void SetInsuranceCompany(int offerItemID) {
        tbInsuranceCompany.Text = OfferItem.Get(offerItemID).InsuranceCompany.Name;
    }

    private List<Label> lblFieldNames;
    private List<TextBox> tbValues;
    protected override void CreateChildControls() {
       // Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        int j = 0;
        List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(InsuranceSubtypeID);

        pnlExtendControls.Controls.Clear();
        lblFieldNames = new List<Label>();
        tbValues = new List<TextBox>();

        HtmlTable titleTable = new HtmlTable();
        titleTable.Width = "695px";
        HtmlTableRow firstTitleTableRow = new HtmlTableRow();
        titleTable.Rows.Add(firstTitleTableRow);
        HtmlTableCell firstCellFirstRowInTitleTable = new HtmlTableCell();
        firstTitleTableRow.Cells.Add(firstCellFirstRowInTitleTable);
        Label lblPremiumBrokerageTitle = new Label();
        lblPremiumBrokerageTitle.ID = "lblPremiumBrokerageTitle";
        lblPremiumBrokerageTitle.Font.Bold = true;
        lblPremiumBrokerageTitle.Text = "Премија и брокеража";
        firstCellFirstRowInTitleTable.Controls.Add(lblPremiumBrokerageTitle);
        pnlExtendControls.Controls.Add(titleTable);

        HtmlTable defaultTable = new HtmlTable();
        defaultTable.Width = "695px";
        //decimal value = 0;
        //if (ddlOfferItemInsuranceSubType != null) {
        //    if (ddlOfferItemInsuranceSubType.SelectedIndex != -1) {
        //        int i = int.Parse(ddlOfferItemInsuranceSubType.SelectedValue);
        //        OfferItem oi = OfferItem.Get(i);
        //        value= oi.Cost;
        //    }
        //}

        HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
        fourthDefaultTableRow.BgColor = "#FAFAF8";
        defaultTable.Rows.Add(fourthDefaultTableRow);
        HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
        firstCellFourthRowInDefaultTable.Width = "160px";
        fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
        Label lblPolicyValue = new Label();
        lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
        lblPolicyValue.Text = "Премија";
        firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
        HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
        secondCellFourthRowInDefaultTable.Width = "187px";
        fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
        TextBox tbPolicyValue = new TextBox();
        tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
       // tbPolicyValue.Text = value.ToString();
        tbPolicyValue.AutoPostBack = true;
        tbPolicyValue.TextChanged += new EventHandler(tbPolicyValue_TextChanged);
        RequiredFieldValidator rfvPolicyValue = new RequiredFieldValidator();
        rfvPolicyValue.ID = "rfvPolicyValue" + (j + 1).ToString();
        rfvPolicyValue.ErrorMessage = "*";
        rfvPolicyValue.Display = ValidatorDisplay.Dynamic;
        rfvPolicyValue.ControlToValidate = tbPolicyValue.ID;
        CompareValidator cvPolicyValue = new CompareValidator();
        cvPolicyValue.ID = "cvPolicyValue" + (j + 1).ToString();
        cvPolicyValue.ErrorMessage = "*";
        cvPolicyValue.Display = ValidatorDisplay.Dynamic;
        cvPolicyValue.ControlToValidate = tbPolicyValue.ID;
        cvPolicyValue.Operator = ValidationCompareOperator.DataTypeCheck;
        cvPolicyValue.Type = ValidationDataType.Double;
        secondCellFourthRowInDefaultTable.Controls.Add(tbPolicyValue);
        secondCellFourthRowInDefaultTable.Controls.Add(rfvPolicyValue);
        secondCellFourthRowInDefaultTable.Controls.Add(cvPolicyValue);
        HtmlTableCell thirdCellFourthRowInDefaultTable = new HtmlTableCell();
        thirdCellFourthRowInDefaultTable.Width = "160px";
        fourthDefaultTableRow.Cells.Add(thirdCellFourthRowInDefaultTable);
        HtmlTableCell fourthCellFourthRowInDefaultTable = new HtmlTableCell();
        fourthCellFourthRowInDefaultTable.Width = "187px";
        fourthDefaultTableRow.Cells.Add(fourthCellFourthRowInDefaultTable);

        //HtmlTableRow fifthDefaultTableRow = new HtmlTableRow();
        //defaultTable.Rows.Add(fifthDefaultTableRow);
        //HtmlTableCell firstCellFifthRowInDefaultTable = new HtmlTableCell();
        //firstCellFifthRowInDefaultTable.Width = "160px";
        //fifthDefaultTableRow.Cells.Add(firstCellFifthRowInDefaultTable);
        //Label lblBrokeragePercentage = new Label();
        //lblBrokeragePercentage.ID = "lblBrokeragePercentage" + (j + 1).ToString();
        //lblBrokeragePercentage.Text = "Процент на брокеража";
        //firstCellFifthRowInDefaultTable.Controls.Add(lblBrokeragePercentage);
        //HtmlTableCell secondCellFifthRowInDefaultTable = new HtmlTableCell();
        //secondCellFifthRowInDefaultTable.Width = "187px";
        //fifthDefaultTableRow.Cells.Add(secondCellFifthRowInDefaultTable);
        //TextBox tbBrokeragePercentage = new TextBox();
        //tbBrokeragePercentage.ID = "tbBrokeragePercentage" + (j + 1).ToString();
        //if (DealID == 0) {
        //    tbBrokeragePercentage.Text = "0";
        //} else {
        //    tbBrokeragePercentage.Text = Brokerage.GetByInsuranceSubTypeAndDeal(InsuranceSubtypeID, DealID).Percentage.ToString();
        //}
        //tbBrokeragePercentage.ReadOnly = true;
        //secondCellFifthRowInDefaultTable.Controls.Add(tbBrokeragePercentage);
        //HtmlTableCell thirdCellFifthRowInDefaultTable = new HtmlTableCell();
        //thirdCellFifthRowInDefaultTable.Width = "160px";
        //fifthDefaultTableRow.Cells.Add(thirdCellFifthRowInDefaultTable);
        //HtmlTableCell fourthCellFifthRowInDefaultTable = new HtmlTableCell();
        //fourthCellFifthRowInDefaultTable.Width = "187px";
        //fifthDefaultTableRow.Cells.Add(fourthCellFifthRowInDefaultTable);

        //HtmlTableRow sixthDefaultTableRow = new HtmlTableRow();
        //sixthDefaultTableRow.BgColor = "#FAFAF8";
        //defaultTable.Rows.Add(sixthDefaultTableRow);
        //HtmlTableCell firstCellSixthRowInDefaultTable = new HtmlTableCell();
        //firstCellSixthRowInDefaultTable.Width = "160px";
        //sixthDefaultTableRow.Cells.Add(firstCellSixthRowInDefaultTable);
        //Label lblBrokerageValue = new Label();
        //lblBrokerageValue.ID = "lblBrokerageValue" + (j + 1).ToString();
        //lblBrokerageValue.Text = "Брокеража";
        //firstCellSixthRowInDefaultTable.Controls.Add(lblBrokerageValue);
        //HtmlTableCell secondCellSixthRowInDefaultTable = new HtmlTableCell();
        //secondCellSixthRowInDefaultTable.Width = "187px";
        //sixthDefaultTableRow.Cells.Add(secondCellSixthRowInDefaultTable);
        //TextBox tbBrokerageValue = new TextBox();
        //tbBrokerageValue.ID = "tbBrokerageValue" + (j + 1).ToString();
        //tbBrokerageValue.ReadOnly = true;
        ////tbBrokerageValue.Text = (Convert.ToDecimal(tbPolicyValue.Text) * Convert.ToDecimal(tbBrokeragePercentage.Text) / 100).ToString();
        //secondCellSixthRowInDefaultTable.Controls.Add(tbBrokerageValue);
        //HtmlTableCell thirdCellSixthRowInDefaultTable = new HtmlTableCell();
        //thirdCellSixthRowInDefaultTable.Width = "160px";
        //sixthDefaultTableRow.Cells.Add(thirdCellSixthRowInDefaultTable);
        //HtmlTableCell fourthCellSixthRowInDefaultTable = new HtmlTableCell();
        //fourthCellSixthRowInDefaultTable.Width = "187px";
        //sixthDefaultTableRow.Cells.Add(fourthCellSixthRowInDefaultTable);

        pnlExtendControls.Controls.Add(defaultTable);

        HtmlTable titlePolicyTable = new HtmlTable();
        titlePolicyTable.Width = "695px";
        HtmlTableRow firstTitlePolicyTableRow = new HtmlTableRow();
        titlePolicyTable.Rows.Add(firstTitlePolicyTableRow);
        HtmlTableCell firstCellFirstRowInTitlePolicyTable = new HtmlTableCell();
        firstTitlePolicyTableRow.Cells.Add(firstCellFirstRowInTitlePolicyTable);
        Label lblPolicyDetailsTitle = new Label();
        lblPolicyDetailsTitle.ID = "lblPolicyDetailsTitle";
        lblPolicyDetailsTitle.Font.Bold = true;
        lblPolicyDetailsTitle.Text = "Детални информации за полиса";
        firstCellFirstRowInTitlePolicyTable.Controls.Add(lblPolicyDetailsTitle);
        pnlExtendControls.Controls.Add(titlePolicyTable);

        HtmlTable table = new HtmlTable();
        table.Width = "695px";
        int counter = 0;
        foreach (Broker.DataAccess.Control c in listControls)
        {
            if (c.IsActive)
            {
                if (listControls.Count == 1)
                {
                    HtmlTableRow tableRow = new HtmlTableRow();
                    tableRow.BgColor = "#FAFAF8";
                    table.Rows.Add(tableRow);
                    HtmlTableCell tableCell = new HtmlTableCell();
                    tableCell.Width = "160px";
                    tableRow.Cells.Add(tableCell);
                    Label label = new Label();
                    label.ID = c.LabelID;
                    label.Text = c.LabelName;
                    lblFieldNames.Add(label);
                    tableCell.Controls.Add(label);
                    HtmlTableCell tableCellSecond = new HtmlTableCell();
                    tableCellSecond.Width = "187px";
                    tableRow.Cells.Add(tableCellSecond);
                    if (c.FieldType.Name == FieldType.TEXTBOX)
                    {
                        TextBox textbox = new TextBox();
                        textbox.ID = c.TextBoxID;
                        if (c.IsLatin != null)
                        {
                            if ((bool)c.IsLatin == true)
                            {
                                textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                            }
                        }
                        tbValues.Add(textbox);
                        tableCellSecond.Controls.Add(textbox);
                        if (c.HasRequredFieldValidator)
                        {
                            RequiredFieldValidator rfv = new RequiredFieldValidator();
                            rfv.ID = "rfv" + textbox.ID;
                            rfv.ErrorMessage = "*";
                            rfv.Display = ValidatorDisplay.Dynamic;
                            rfv.ControlToValidate = textbox.ID;
                            tableCellSecond.Controls.Add(rfv);
                        }
                        if (c.HasCompareValidator)
                        {
                            CompareValidator cv = new CompareValidator();
                            cv.ID = "cv" + textbox.ID;
                            cv.ErrorMessage = "*";
                            cv.Display = ValidatorDisplay.Dynamic;
                            cv.ControlToValidate = textbox.ID;
                            cv.Operator = ValidationCompareOperator.DataTypeCheck;
                            cv.Type = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                            tableCellSecond.Controls.Add(cv);
                        }
                        if (c.FieldType.Name == FieldType.DROPDOWNLIST)
                        {
                            DropDownList dropdownlist = new DropDownList();
                            dropdownlist.ID = c.TextBoxID;
                            dropdownlist.DataTextField = "Value";
                            dropdownlist.DataValueField = "ID";
                            List<ControlBindableValue> cbvList = ControlBindableValue.GetActiveByControlID(c.ID);
                            dropdownlist.DataSource = cbvList;
                            tableCellSecond.Controls.Add(dropdownlist);
                        }
                    }
                    HtmlTableCell tableCellThird = new HtmlTableCell();
                    tableCellThird.Width = "160px";
                    tableRow.Cells.Add(tableCellThird);
                    HtmlTableCell tableCellForth = new HtmlTableCell();
                    tableCellForth.Width = "187px";
                    tableRow.Cells.Add(tableCellForth);
                } else
                {
                    if (counter % 2 == 0)
                    {
                        HtmlTableRow tableRow = new HtmlTableRow();
                        if ((counter % 4 == 0) || (counter % 4 == 1))
                        {
                            tableRow.BgColor = "#FAFAF8";
                        }
                        table.Rows.Add(tableRow);
                        HtmlTableCell tableCell = new HtmlTableCell();
                        tableCell.Width = "160px";
                        tableRow.Cells.Add(tableCell);
                        Label label = new Label();
                        label.ID = c.LabelID;
                        label.Text = c.LabelName;
                        lblFieldNames.Add(label);
                        tableCell.Controls.Add(label);
                        HtmlTableCell tableCellSecond = new HtmlTableCell();
                        tableCellSecond.Width = "187px";
                        tableRow.Cells.Add(tableCellSecond);
                        if (c.FieldType.Name == FieldType.TEXTBOX)
                        {
                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID;
                            if (c.IsLatin != null)
                            {
                                if ((bool)c.IsLatin == true)
                                {
                                    textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                                }
                            }
                            tbValues.Add(textbox);
                            tableCellSecond.Controls.Add(textbox);
                            if (c.HasRequredFieldValidator)
                            {
                                RequiredFieldValidator rfv = new RequiredFieldValidator();
                                rfv.ID = "rfv" + textbox.ID;
                                rfv.ErrorMessage = "*";
                                rfv.Display = ValidatorDisplay.Dynamic;
                                rfv.ControlToValidate = textbox.ID;
                                tableCellSecond.Controls.Add(rfv);
                            }
                            if (c.HasCompareValidator)
                            {
                                CompareValidator cv = new CompareValidator();
                                cv.ID = "cv" + textbox.ID;
                                cv.ErrorMessage = "*";
                                cv.Display = ValidatorDisplay.Dynamic;
                                cv.ControlToValidate = textbox.ID;
                                cv.Operator = ValidationCompareOperator.DataTypeCheck;
                                cv.Type = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                tableCellSecond.Controls.Add(cv);
                            }
                        }
                        if (c.FieldType.Name == FieldType.DROPDOWNLIST)
                        {
                            DropDownList dropdownlist = new DropDownList();
                            dropdownlist.ID = c.TextBoxID;
                            dropdownlist.DataTextField = "Value";
                            dropdownlist.DataValueField = "ID";
                            List<ControlBindableValue> cbvList = ControlBindableValue.GetActiveByControlID(c.ID);
                            dropdownlist.DataSource = cbvList;
                            dropdownlist.DataBind();
                            tableCellSecond.Controls.Add(dropdownlist);
                        }

                    } else if (counter % 2 == 1)
                    {
                        HtmlTableRow tableRow = table.Rows[counter / 2];
                        if ((counter % 4 == 0) || (counter % 4 == 1))
                        {
                            tableRow.BgColor = "#FAFAF8";
                        }
                        HtmlTableCell tableCellThird = new HtmlTableCell();
                        tableCellThird.Width = "160px";
                        tableRow.Cells.Add(tableCellThird);
                        Label label = new Label();
                        label.ID = c.LabelID;
                        label.Text = c.LabelName;
                        lblFieldNames.Add(label);
                        tableCellThird.Controls.Add(label);
                        HtmlTableCell tableCellForth = new HtmlTableCell();
                        tableCellForth.Width = "187px";
                        tableRow.Cells.Add(tableCellForth);
                        if (c.FieldType.Name == FieldType.TEXTBOX)
                        {
                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID;
                            if (c.IsLatin != null)
                            {
                                if ((bool)c.IsLatin == true)
                                {
                                    textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                                }
                            }
                            tbValues.Add(textbox);
                            tableCellForth.Controls.Add(textbox);
                            if (c.HasRequredFieldValidator)
                            {
                                RequiredFieldValidator rfv = new RequiredFieldValidator();
                                rfv.ID = "rfv" + textbox.ID;
                                rfv.ErrorMessage = "*";
                                rfv.Display = ValidatorDisplay.Dynamic;
                                rfv.ControlToValidate = textbox.ID;
                                tableCellForth.Controls.Add(rfv);
                            }
                            if (c.HasCompareValidator)
                            {
                                CompareValidator cv = new CompareValidator();
                                cv.ID = "cv" + textbox.ID;
                                cv.ErrorMessage = "*";
                                cv.Display = ValidatorDisplay.Dynamic;
                                cv.ControlToValidate = textbox.ID;
                                cv.Operator = ValidationCompareOperator.DataTypeCheck;
                                cv.Type = Broker.DataAccess.VariableType.GetForVariableType(c.VariableTypeID);
                                tableCellForth.Controls.Add(cv);
                            }
                        }
                        if (c.FieldType.Name == FieldType.DROPDOWNLIST)
                        {
                            DropDownList dropdownlist = new DropDownList();
                            dropdownlist.ID = c.TextBoxID;
                            dropdownlist.DataTextField = "Value";
                            dropdownlist.DataValueField = "ID";
                            List<ControlBindableValue> cbvList = ControlBindableValue.GetActiveByControlID(c.ID);
                            dropdownlist.DataSource = cbvList;
                            dropdownlist.DataBind();
                            tableCellForth.Controls.Add(dropdownlist);
                        }

                    }
                }
                counter++;
            }
        }
        pnlExtendControls.Controls.Add(table);

       // CheckBox cbIsPaidInCash = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        if (!cbIsPaidInCash.Checked) {

            HtmlTable tableTitleRates = new HtmlTable();
            HtmlTableRow firstRowTableTitle = new HtmlTableRow();
            tableTitleRates.Rows.Add(firstRowTableTitle);
            HtmlTableCell firstCellInFirstRow = new HtmlTableCell();
            firstRowTableTitle.Cells.Add(firstCellInFirstRow);
            Label lblTitleRates = new Label();
            lblTitleRates.ID = "lblTitleRates";
            lblTitleRates.Text = "Рати";
            lblTitleRates.Font.Bold = true;
            firstCellInFirstRow.Controls.Add(lblTitleRates);
            pnlExtendControls.Controls.Add(tableTitleRates);

            HtmlTable tableRates = new HtmlTable();
            HtmlTableRow firstRowTableRates = new HtmlTableRow();
            firstRowTableRates.BgColor = "#FAFAF8";
            tableRates.Rows.Add(firstRowTableRates);
            HtmlTableCell firstCellInFirstRowInTableRates = new HtmlTableCell();
            firstCellInFirstRowInTableRates.Width = "80px";
            firstRowTableRates.Cells.Add(firstCellInFirstRowInTableRates);
            Label lblNumberOfRates = new Label();
            lblNumberOfRates.ID = "lblNumberOfRates";
            lblNumberOfRates.Text = "Број на рати";
            firstCellInFirstRowInTableRates.Controls.Add(lblNumberOfRates);

            HtmlTableCell secondCellInFirstRowInTableRates = new HtmlTableCell();
            secondCellInFirstRowInTableRates.Width = "79px";
            firstRowTableRates.Cells.Add(secondCellInFirstRowInTableRates);
            DropDownList ddlNumberOfRates = new DropDownList();
            ddlNumberOfRates.ID = "ddlNumberOfRates";
            ddlNumberOfRates.AutoPostBack = true;
            for (int k = 1; k <= 12; k++) {
                ListItem li = new ListItem();
                li.Text = k.ToString();
                li.Value = k.ToString();
                ddlNumberOfRates.Items.Add(li);
            }
            ddlNumberOfRates.SelectedIndexChanged += new EventHandler(ddlNumberOfRates_SelectedIndexChanged);
            secondCellInFirstRowInTableRates.Controls.Add(ddlNumberOfRates);

            HtmlTableCell thirdCellInFirstRowInTableRates = new HtmlTableCell();
            thirdCellInFirstRowInTableRates.Width = "140px";
            firstRowTableRates.Cells.Add(thirdCellInFirstRowInTableRates);
            Label lblStartDateForRates = new Label();
            lblStartDateForRates.ID = "lblStartDateForRates";
            lblStartDateForRates.Text = "Почетна дата на рати";
            thirdCellInFirstRowInTableRates.Controls.Add(lblStartDateForRates);

            HtmlTableCell forthCellInFirstRowInTableRates = new HtmlTableCell();
            forthCellInFirstRowInTableRates.Width = "80px";
            firstRowTableRates.Cells.Add(forthCellInFirstRowInTableRates);
            TextBox tbStartDateForRates = new TextBox();
            tbStartDateForRates.ID = "tbStartDateForRates";
            tbStartDateForRates.AutoPostBack = true;
            tbStartDateForRates.Width = 135;
            tbStartDateForRates.TextChanged += new EventHandler(DateChanged);
            forthCellInFirstRowInTableRates.Controls.Add(tbStartDateForRates);

            pnlExtendControls.Controls.Add(tableRates);

        }

    }
    void tbPolicyValue_TextChanged(object sender, EventArgs e) {

        TextBox tbPolicyValue = (TextBox)sender;
        int index = Convert.ToInt32(tbPolicyValue.ID.Substring(tbPolicyValue.ID.Length - 1));
        TextBox tbBrokeragePercentage = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokeragePercentage" + index.ToString());
        TextBox tbBrokerageValue = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokerageValue" + index.ToString());
        tbBrokerageValue.Text = (Convert.ToDecimal(tbPolicyValue.Text) * Convert.ToDecimal(tbBrokeragePercentage.Text) / 100).ToString();
       // CheckBox cbIsPaidInBrokersHouse = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        if (!cbIsPaidInCash.Checked) {
          //  Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
            DropDownList ddlRates = pnlExtendControls.FindControl("ddlNumberOfRates") as DropDownList;
            ddlNumberOfRates_SelectedIndexChanged(ddlRates, null);
        }

    }

    void ddlNumberOfRates_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlNumberOfRates = (DropDownList)sender;
       // Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        List<Rate> lRates = new List<Rate>();
        //TextBox tbPolicyNumber = (TextBox)pnlExtendControls.FindControl("tbPolicyNumber");
        //string policyNumber = tbPolicyNumber.Text;

        TextBox tbPolicyValue = (TextBox)pnlExtendControls.FindControl("tbPolicyValue1");
        decimal policyValue = 0;
        if (tbPolicyValue.Text != string.Empty) {
            policyValue = Convert.ToDecimal(tbPolicyValue.Text);
        }

        //DateTime startDate = policy.ApplicationDate;
        TextBox tbStartDateForRates = pnlExtendControls.FindControl("tbStartDateForRates") as TextBox;
        DateTime startDate = new DateTime();
        if (tbStartDateForRates == null) {
            startDate = DateTime.MinValue;
        } else {
            DateTime.TryParse(tbStartDateForRates.Text, out startDate);
        }

        if (startDate == DateTime.MinValue) {
            startDate = DateTime.Today;
        }
        decimal totalPolicyCost = policyValue;
        int ratesNumber = 1;
        if (ddlNumberOfRates != null) {
            ratesNumber = Convert.ToInt32(ddlNumberOfRates.SelectedValue);
        }
        for (int i = 0; i < ratesNumber; i++) {
            Rate r = new Rate();
            r.Number = i + 1;
            r.Date = startDate.AddMonths(i);
            r.Value = RateController.Scale5(totalPolicyCost / ratesNumber);
            int index = Convert.ToInt32(tbPolicyValue.ID.Substring(tbPolicyValue.ID.Length - 1));
            TextBox tbBrokeragePercentage = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokeragePercentage" + index.ToString());
            //decimal brokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
            decimal brokeragePercentage = 0;
            r.BrokerageValue = RateController.Scale5((brokeragePercentage / 100) * r.Value);

            ///Ova da se proveri!!!
            r.PolicyItemID = 0;
            //r.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
            lRates.Add(r);
        }

        GridView GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
        if (GridViewRates != null) {
            GridViewRates.AutoGenerateColumns = false;
            GridViewRates.DataSource = lRates;
            GridViewRates.DataBind();
        }

        Dictionary<int, decimal> myDictionary = new Dictionary<int, decimal>();
        foreach (Rate r in lRates) {
            myDictionary.Add(r.Number, r.Value);
        }
        ViewState["MyDictionary"] = myDictionary;
    }

    protected void DateChanged(object sender, EventArgs e) {

        //Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        DropDownList ddlNumberOfRates = pnlExtendControls.FindControl("ddlNumberOfRates") as DropDownList;
        ddlNumberOfRates_SelectedIndexChanged(ddlNumberOfRates, null);
    }

    protected void cbIsPaidInCash_CheckedChanged(object sender, EventArgs e) {
        if (cbIsPaidInCash.Checked) {
            pnlCashPayment.Visible = true;
        } else {
            pnlCashPayment.Visible = false;
        }
        CreateChildControls();
    }

    void CreateGridView(object sender, EventArgs e) {
       // Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        if (!cbIsPaidInCash.Checked) {
            GridView GridViewRates = new GridView();
            GridViewRates.ID = "GridViewRates";
            GridViewRates.AutoGenerateColumns = false;
            GridViewRates.GridLines = GridLines.None;
            GridViewRates.SelectedIndexChanged += new EventHandler(GridViewRates_SelectedIndexChanged);
            GridViewRates.RowCommand += new GridViewCommandEventHandler(GridViewRates_RowCommand);
            //CommandField cfSelect = new CommandField();
            //cfSelect.ButtonType = ButtonType.Link;
            //cfSelect.SelectText = "Пресметај";
            //cfSelect.ShowSelectButton = true;
            //GridViewRates.Columns.Add(cfSelect);
            TemplateField tfNumber = new TemplateField();
            tfNumber.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "Number", true, false);
            tfNumber.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Реден број", true, false);
            GridViewRates.Columns.Add(tfNumber);
            TemplateField tfDate = new TemplateField();
            tfDate.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "Date", true, false);
            tfDate.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Датум", true, false);
            GridViewRates.Columns.Add(tfDate);
            TemplateField tfValue = new TemplateField();
            tfValue.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "Value", false, false);
            tfValue.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Износ", false, false);
            GridViewRates.Columns.Add(tfValue);
            TemplateField tfBrokerageValue = new TemplateField();
            tfBrokerageValue.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "BrokerageValue", true, false);
            tfBrokerageValue.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Брокеража", true, false);
            GridViewRates.Columns.Add(tfBrokerageValue);
            TemplateField tfPaymentType = new TemplateField();
            tfPaymentType.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "PaymentTypeID", true, true);
            tfPaymentType.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Тип на плаќање", true, true);
            GridViewRates.Columns.Add(tfPaymentType);
            pnlExtendControls.Controls.Add(GridViewRates);

            Button buttonCalculate = new Button();
            buttonCalculate.ID = "btnCalculate";
            buttonCalculate.Text = "Пресметај";
            buttonCalculate.Click += new EventHandler(buttonCalculate_Click);
            pnlExtendControls.Controls.Add(buttonCalculate);
        }

    }
    void GridViewRates_RowCommand(object sender, GridViewCommandEventArgs e) {
        GridView GridViewRates = (GridView)sender;
        //selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        //  int gridViewIDIndex = int.Parse(GridViewRates.ID.Substring(GridViewRates.ID.Length - 1));
        //int gridViewSelectedIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        int gridViewSelectedIndex = Convert.ToInt32(e.CommandArgument);
        //TextBox tbValue = (TextBox)GridViewRates.SelectedRow.Cells[3].FindControl("tbValue");
        TextBox tbValue = (TextBox)GridViewRates.Rows[gridViewSelectedIndex].Cells[3].FindControl("tbValue");
        decimal value = Convert.ToDecimal(tbValue.Text);
        UpdateGridView(gridViewSelectedIndex, value);
    }

    void UpdateGridView(int gridViewSelectedIndex, decimal value) {
        List<Rate> lRates = new List<Rate>();
        //Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        TextBox tbPolicyValue = (TextBox)pnlExtendControls.FindControl("tbPolicyValue1");
        decimal totalPolicyCost = Convert.ToDecimal(tbPolicyValue.Text);
        if (totalPolicyCost > 0) {

            decimal currentValue = value;
            //int curentNumber = GridViewRates.EditIndex;
            int curentNumber = gridViewSelectedIndex + 1;

            List<Rate> ActualRates = new List<Rate>();
            ActualRates = RatesInGridView;

            decimal currentSum = 0;
            for (int i = 0; i < curentNumber - 1; i++) {
                lRates.Add(ActualRates[i]);
                currentSum += ActualRates[i].Value;
            }

            if (currentValue > totalPolicyCost - currentSum) {
                currentValue = totalPolicyCost - currentSum;
            }

            Rate currentRate = new Rate();
            currentRate.Number = curentNumber;
            currentRate.Date = ActualRates[curentNumber - 1].Date;
            currentRate.Value = currentValue;
            TextBox tbBrokeragePercentage = pnlExtendControls.FindControl("tbBrokeragePercentage1") as TextBox;
            //decimal brokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
            decimal brokeragePercentage = 0;
            currentRate.BrokerageValue = (brokeragePercentage / 100) * currentValue;
            currentSum += currentValue;
            //currentRate.PaymentTypeID = ActualRates[curentNumber - 1].PaymentTypeID;
            ///Da se proveri!
            currentRate.PolicyItemID = 0;
            lRates.Add(currentRate);

            decimal restSum = totalPolicyCost - currentSum;
            decimal restValuePerRate = RateController.Scale5(restSum / (ActualRates.Count - curentNumber));
            for (int j = curentNumber; j < ActualRates.Count; j++) {
                Rate rate = new Rate();
                rate.Number = j + 1;
                rate.Date = ActualRates[j].Date;
                rate.Value = restValuePerRate;
                rate.BrokerageValue = (brokeragePercentage / 100) * restValuePerRate;
                //rate.PaymentTypeID = ActualRates[j].PaymentTypeID;
                ///Da se proveri
                rate.PolicyItemID = 0;
                lRates.Add(rate);
            }
        }
        GridView GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
        GridViewRates.AutoGenerateColumns = false;
        GridViewRates.DataSource = lRates;
        GridViewRates.DataBind();
    }

    void GridViewRates_SelectedIndexChanged(object sender, EventArgs e) {
        GridView GridViewRates = (GridView)sender;
        //selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        //  int gridViewIDIndex = int.Parse(GridViewRates.ID.Substring(GridViewRates.ID.Length - 1));
        int gridViewSelectedIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        TextBox tbValue = (TextBox)GridViewRates.SelectedRow.Cells[3].FindControl("tbValue");
        decimal value = Convert.ToDecimal(tbValue.Text);
        UpdateGridView(gridViewSelectedIndex, value);
    }

    public List<Rate> RatesInGridView {
        get {
            List<Rate> ActualRates = new List<Rate>();
            GridView GridViewRates;
           // Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
            GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
            foreach (GridViewRow row in GridViewRates.Rows) {
                Rate r = new Rate();
                TextBox tbNumber = (TextBox)row.Cells[0].FindControl("tbNumber");
                r.Number = Convert.ToInt32(tbNumber.Text);
                TextBox tbDate = (TextBox)row.Cells[1].FindControl("tbDate");
                r.Date = Convert.ToDateTime(tbDate.Text);
                TextBox tbvalue = (TextBox)row.Cells[2].FindControl("tbValue");
                r.Value = Convert.ToDecimal(tbvalue.Text);
                TextBox tbBrokeragevalue = (TextBox)row.Cells[3].FindControl("tbBrokerageValue");
                r.BrokerageValue = Convert.ToDecimal(tbBrokeragevalue.Text);
                DropDownList ddlPaymentType = (DropDownList)row.Cells[4].FindControl("ddlPaymentTypes");
                //r.PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue);
                r.PolicyItemID = 1;
                ActualRates.Add(r);
            }
            return ActualRates;
        }
    }
    void buttonCalculate_Click(object sender, EventArgs e) {
        List<Rate> lRates = new List<Rate>();
        decimal totalPolicyCost = 0;
        List<decimal> brokeragePercentages = new List<decimal>();
        List<decimal> policyItemValues = new List<decimal>();
        List<decimal> policyItemRatios = new List<decimal>();
        decimal brokerageValue = 0;
        int j = 1;
        //Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        TextBox tbPolicyValue = (TextBox)pnlExtendControls.FindControl("tbPolicyValue" + j.ToString());
        totalPolicyCost += Convert.ToDecimal(tbPolicyValue.Text);
        policyItemValues.Add(Convert.ToDecimal(tbPolicyValue.Text));
        TextBox tbBrokeragePercentage = (TextBox)pnlExtendControls.FindControl("tbBrokeragePercentage" + j.ToString());
        brokeragePercentages.Add(Convert.ToDecimal(tbBrokeragePercentage.Text));
        TextBox tbBrokerageValue = (TextBox)pnlExtendControls.FindControl("tbBrokerageValue" + j.ToString());
        //brokerageValue += Convert.ToDecimal(tbBrokerageValue.Text);
        brokerageValue += 0;
        policyItemRatios.Add(policyItemValues[0] / totalPolicyCost);

        Dictionary<int, decimal> oldDictionary = new Dictionary<int, decimal>();
        oldDictionary = (Dictionary<int, decimal>)ViewState["MyDictionary"];
        Dictionary<int, decimal> newDictionary = new Dictionary<int, decimal>();
        foreach (Rate r in RatesInGridView) {
            newDictionary.Add(r.Number, r.Value);
        }
        decimal changedPolicyItemValueSum = 0;
        int changedNumber = 0;
        Dictionary<int, decimal> changedValues = new Dictionary<int, decimal>();
        for (int i = 1; i <= oldDictionary.Count; i++) {
            if (oldDictionary[i] != newDictionary[i]) {
                changedValues.Add(i, newDictionary[i]);
                changedPolicyItemValueSum += newDictionary[i];
                changedNumber++;
            }
        }
        decimal restValue = totalPolicyCost - changedPolicyItemValueSum;

        decimal valuePerRateForNonChanged = RateController.Scale5(restValue / (oldDictionary.Count - changedNumber));

        int t = 0;
        foreach (Rate r in RatesInGridView) {
            Rate newRate = new Rate();
            newRate.Date = r.Date;
            newRate.Number = r.Number;
            newRate.PaidValue = r.PaidValue;
            //newRate.PaymentTypeID = r.PaymentTypeID;
            newRate.PolicyItemID = r.PolicyItemID;
            if (changedValues.ContainsKey(r.Number)) {
                newRate.Value = changedValues[r.Number];
            } else {
                newRate.Value = valuePerRateForNonChanged;
            }
            decimal brokerageValuePerRate = 0;
            for (int k = 0; k <= brokeragePercentages.Count - 1; k++) {
                brokerageValuePerRate += RateController.Scale5(policyItemRatios[k] * (brokeragePercentages[k] / 100) * newRate.Value);
            }
            newRate.BrokerageValue = brokerageValuePerRate;
            lRates.Add(newRate);
            t++;
        }

        GridView GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
        GridViewRates.AutoGenerateColumns = false;
        GridViewRates.DataSource = lRates;
        GridViewRates.DataBind();

        Dictionary<int, decimal> myDictionary = new Dictionary<int, decimal>();
        foreach (Rate r in lRates) {
            myDictionary.Add(r.Number, r.Value);
        }
        //ViewState.Remove("MyDictionary");
        ViewState["MyDictionary"] = myDictionary;

    }
    protected void btnInsert_Click(object sender, EventArgs e) {
        string policyNumber = tbPolicyNumber.Text.Trim();
        OfferItem oi = OfferItem.Get(int.Parse(ddlOfferItemInsuranceSubType.SelectedValue));
        if (oi != null) {
            string validate = Policy.ValidateSinglePolicyNumberFromOffer(oi.InsuranceCompanyID, oi.InsuranceSubTypeID, tbPolicyNumber.Text.Trim());
            if (validate == string.Empty) {
                Policy p = new Policy();
                p.ApplicationDate = DateTime.Today;
                p.BranchID = this.PageUser.BranchID;
                TextBox tbBrokeragePercentage = pnlExtendControls.FindControl("tbBrokeragePercentage1") as TextBox;
                p.BrokeragePercentage = decimal.Parse(tbBrokeragePercentage.Text);
                //OfferItem oi = OfferItem.Get(int.Parse(ddlOfferItemInsuranceSubType.SelectedValue));
                p.ClientID = oi.Offer.ClientID;
                p.Discard = false;
                p.EndDate = DateTime.Parse(tbEndDate.Text);
                p.InsuranceCompanyID = oi.InsuranceCompanyID;
                p.OfferID = oi.OfferID;
                p.OwnerID = oi.Offer.OwnerID;
                p.InsertDate = DateTime.Today;
                p.PolicyNumber = tbPolicyNumber.Text;
                p.StartDate = DateTime.Parse(tbStartDate.Text);
                p.Statuse = Statuse.GetByCode(Statuse.ZAVRSENA_POLISA);
                TextBox tbValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
                p.TotalSum = decimal.Parse(tbValue.Text);
                p.UserID = this.PageUser.ID;
                p.Insert();



                PolicyItem pi = new PolicyItem();
                pi.BrokeragePercentage = decimal.Parse(tbBrokeragePercentage.Text);
                TextBox tbBrokerageValue = pnlExtendControls.FindControl("tbBrokerageValue1") as TextBox;
                pi.BrokerageValue = decimal.Parse(tbBrokerageValue.Text);

                pi.InsuranceSubTypeID = oi.InsuranceSubTypeID;
                pi.PolicyNumber = p.PolicyNumber;
                TextBox tbPolicyValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
                pi.PremiumValue = decimal.Parse(tbPolicyValue.Text);
                pi.StatusID = p.StatusID;
                pi.PolicyID = p.ID;
                pi.IsPaidInBrokerHouse = cbIsPaidInCash.Checked;
                p.TotalSum = pi.PremiumValue;
                pi.Insert();

                Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(pi.InsuranceSubTypeID, pi.PolicyNumber, pi.Policy.InsuranceCompanyID);
                d.IsUsed = true;
                Policy.Table.Context.SubmitChanges();

                List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(pi.InsuranceSubTypeID);
                foreach (Broker.DataAccess.Control c in listControls) {
                    PolicyExtendInformation pei = new PolicyExtendInformation();
                    pei.ControlID = c.ID;
                    pei.PolicyItemID = pi.ID;
                    if (c.FieldType.Name == FieldType.TEXTBOX)
                    {
                        TextBox tbControl = (TextBox)pnlExtendControls.FindControl(c.TextBoxID);
                        pei.Value = tbControl.Text;
                        if (c.HasCompareValidator)
                        {
                            CompareValidator cv = (CompareValidator)pnlExtendControls.FindControl("cv" + c.TextBoxID);
                            if (cv.Type == ValidationDataType.Double)
                            {
                                if (tbControl.Text == string.Empty)
                                {
                                    pei.Value = "0";
                                }
                            }
                        }
                    }
                    if (c.FieldType.Name == FieldType.DROPDOWNLIST)
                    {
                        DropDownList ddlControl = (DropDownList)pnlExtendControls.FindControl(c.TextBoxID);
                        pei.Value = ddlControl.SelectedItem.Text;
                        pei.ValueID = int.Parse(ddlControl.SelectedValue);
                    }
                    pei.Insert();
                }

                List<Rate> currentRates = new List<Rate>();
                List<Rate> RatesForInsert = new List<Rate>();

                GridView GridViewRates;
                GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
                if (GridViewRates != null) {
                    foreach (GridViewRow row in GridViewRates.Rows) {
                        Rate r = new Rate();
                        TextBox tbNumber = (TextBox)row.Cells[0].FindControl("tbNumber");
                        r.Number = Convert.ToInt32(tbNumber.Text);
                        TextBox tbDate = (TextBox)row.Cells[1].FindControl("tbDate");
                        r.Date = Convert.ToDateTime(tbDate.Text);
                        TextBox tbvalue = (TextBox)row.Cells[2].FindControl("tbValue");
                        r.Value = Convert.ToDecimal(tbvalue.Text);
                        TextBox tbBrokeragevalue = (TextBox)row.Cells[3].FindControl("tbBrokerageValue");
                        r.BrokerageValue = Convert.ToDecimal(tbBrokeragevalue.Text);
                        DropDownList ddlPaymentType = (DropDownList)row.Cells[4].FindControl("ddlPaymentTypes");
                        //r.PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue);
                        r.PolicyItemID = 1;
                        currentRates.Add(r);
                    }
                }
                if (!pi.IsPaidInBrokerHouse) {
                    if (currentRates.Count == 0) {
                        Rate r = new Rate();
                        r.Date = DateTime.Today;
                        r.Number = 1;
                        //r.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
                        r.PolicyItemID = pi.ID;
                        r.Value = pi.PremiumValue;
                        r.BrokerageValue = pi.BrokerageValue;
                        r.PaidValue = 0;
                        RatesForInsert.Add(r);
                        r.Insert();
                    } else {
                        foreach (Rate rate in currentRates) {
                            Rate r = new Rate();
                            r.Date = rate.Date;
                            r.Number = rate.Number;
                            //r.PaymentTypeID = rate.PaymentTypeID;
                            r.PolicyItemID = pi.ID;
                            r.Value = rate.Value;
                            r.BrokerageValue = rate.BrokerageValue;
                            if (pi.IsPaidInBrokerHouse) {
                                r.PaidValue = rate.Value;
                            } else {
                                r.PaidValue = 0;
                            }
                            RatesForInsert.Add(r);
                            r.Insert();
                        }
                    }
                    decimal brokerageSum = 0;
                    brokerageSum += pi.BrokerageValue;
                    List<PolicyItem> listPolicyItems = new List<PolicyItem>();
                    listPolicyItems.Add(pi);
                    Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GenerateForSinglePolicy(p);
                    f.BrokerageValue = brokerageSum;
                    Broker.DataAccess.Facture.Insert(f);
                    List<FactureItem> lFI = Broker.DataAccess.Facture.GenerateFactureItemsForSinglePolicy(listPolicyItems, f);
                    Broker.DataAccess.Facture.InsertFactureItems(lFI);
                   // FactureID = f.ID;
                    PolicyItemFactureItem.InsertForFacture(f.FactureItems.ToList(), p.PolicyItems.ToList());
                }
                if (pi.IsPaidInBrokerHouse) {
                  //  CashPayment cp = new CashPayment();
                  //  cp.Discarded = false;
                  //  cp.PolicyID = p.ID;
                  //  cp.SerialNumber = CashPayment.GenerateNextNumber(this.PageUser);
                  //  cp.UserID = this.PageUser.ID;
                  //  cp.Insert();
                  ////  Panel pnlPayments = PoliciesDetailsView.FindControl("PaymentTypePanel") as Panel;

                  //  TextBox tbCash = pnlCashPayment.FindControl("tbCash") as TextBox;
                  //  if (tbCash.Text.Trim() != string.Empty) {
                  //      decimal cashValue = decimal.Parse(tbCash.Text);
                  //      CashPaymentItem cpi = new CashPaymentItem();
                  //      cpi.CashPaymentID = cp.ID;
                  //      cpi.PaymentTypeID = PaymentType.GetByCode(PaymentType.CASH).ID;
                  //      cpi.Value = cashValue;
                  //      cpi.Insert();
                  //  }
                  //  TextBox tbCard = pnlCashPayment.FindControl("tbCreditCard") as TextBox;
                  //  if (tbCard.Text.Trim() != string.Empty) {
                  //      decimal cardValue = decimal.Parse(tbCard.Text);
                  //      CashPaymentItem cpi = new CashPaymentItem();
                  //      cpi.CashPaymentID = cp.ID;
                  //      cpi.PaymentTypeID = PaymentType.GetByCode(PaymentType.CREDITCARD).ID;
                  //      cpi.Value = cardValue;
                  //      DropDownList ddlCardTypes = pnlCashPayment.FindControl("ddlCardTypes") as DropDownList;
                  //      DropDownList ddlBanks = pnlCashPayment.FindControl("ddlBanks") as DropDownList;
                  //      if (ddlBanks.SelectedIndex != -1) {
                  //          if (ddlCardTypes.SelectedIndex != -1) {
                  //              cpi.BankCreditCardID = BankCreditCard.GetByBankAndCard(int.Parse(ddlBanks.SelectedValue), int.Parse(ddlCardTypes.SelectedValue)).ID;
                  //          }
                  //      }
                  //      cpi.Insert();
                  //  }

                }
                PolicyID = p.ID;
                p.PolicyItems.Add(pi);
                if (PolicyID != 0) {
                    
                    if (p.PolicyItems[0].IsPaidInBrokerHouse) {
                        lblFeedBack.Text = "Снимена е полиса со број" + p.PolicyItems[0].PolicyNumber + "платена во готово";
                        multiView.SetActiveView(BillView);
                    } else {
                        lblFeedbackRates.Text = "Снимена е полиса со број" + p.PolicyItems[0].PolicyNumber + "платена на рати";
                        multiView.SetActiveView(FactureRatesView);
                    }
                }


            } else {
                lblError.Text = validate;
            }
        }
    }
    protected void btnPrintBill_Click(object sender, EventArgs e) {
        btnDiscardBill.Enabled = true;
        btnPrintBill.Enabled = false;
    }
    protected void btnDiscardBill_Click(object sender, EventArgs e) {
        btnInsertNewPayment.Enabled = true;
    }
    protected void btnDiscardPolicy_Click(object sender, EventArgs e) {
        Broker.DataAccess.Policy p = Broker.DataAccess.Policy.Get(PolicyID);
        if (p != null) {
            p.Discard = true;
            Broker.DataAccess.Policy.Table.Context.SubmitChanges();
        }
    }
    protected void btnInsertNewPayment_Click(object sender, EventArgs e) {
        decimal cashValue = 0;
        decimal cardValue = 0;
        decimal totalValue = 0;
        if (tbCash.Text.Trim() != string.Empty) {
            cashValue = decimal.Parse(tbCash.Text);
        }
        if (tbCreditCard.Text.Trim() != string.Empty) {
            cardValue = decimal.Parse(tbCreditCard.Text);
        }

        //CashPayment cpay = CashPayment.GetDiscardedPayment(PolicyID);
        //foreach (CashPaymentItem cpItem in cpay.CashPaymentItems) {
        //    totalValue += cpItem.Value;
        //}
        //if (cpay != null) {
        //    if (totalValue != cashValue + cardValue) {

        //    } else {
        //        CashPayment cp = new CashPayment();
        //        cp.Discarded = false;
        //        cp.PolicyID = PolicyID;
        //        cp.SerialNumber = CashPayment.GenerateNextNumber(this.PageUser);
        //        cp.UserID = this.PageUser.ID;
        //        cp.Insert();
        //        if (tbCash.Text.Trim() != string.Empty) {
        //            CashPaymentItem cpi = new CashPaymentItem();
        //            cpi.CashPaymentID = cp.ID;
        //            cpi.PaymentTypeID = PaymentType.GetByCode(PaymentType.CASH).ID;
        //            cpi.Value = cashValue;
        //            cpi.Insert();
        //        }
        //        if (tbCreditCard.Text.Trim() != string.Empty) {
        //            CashPaymentItem cpi = new CashPaymentItem();
        //            cpi.CashPaymentID = cp.ID;
        //            cpi.PaymentTypeID = PaymentType.GetByCode(PaymentType.CREDITCARD).ID;
        //            cpi.Value = cardValue;
        //            if (ddlBank.SelectedIndex != -1) {
        //                if (ddlCardTypes.SelectedIndex != -1) {
        //                    cpi.BankCreditCardID = BankCreditCard.GetByBankAndCard(int.Parse(ddlBank.SelectedValue), int.Parse(ddlCardTypes.SelectedValue)).ID;
        //                }
        //            }
        //            cpi.Insert();
        //        }
        //    }
        //}
        btnInsertNewPayment.Enabled = false;

    }

    protected void btnPrintFacture_Click(object sender, EventArgs e) {
        if (ViewState["FactureID"] != null) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(Convert.ToInt32(ViewState["FactureID"]));
            PrintFactures.PrintFacture(f);
        }
    }
    protected void btnPrintFactureForRates_Click(object sender, EventArgs e) {
        if (FactureID > 0) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(FactureID);
            PrintFactures.PrintFacture(f);
        }
    }
    public int FactureID {
        get {
            if (ViewState["FactureID"] == null) {
                return 0;
            } else {
                return Convert.ToInt32(ViewState["FactureID"]);
            }
        }
        set {
            ViewState["FactureID"] = value;
        }
    }
}
