using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using System.Web.UI.HtmlControls;
using Broker.Controllers.RateController;

public partial class UserControls_ExtendContols_ExtendControl : System.Web.UI.UserControl
{
    //public event EventHandler ExtendControlBound;

    //protected void OnBound() {
    //    if (ExtendControlBound != null) {
    //        ExtendControlBound(this, null);
    //    }
    //}
    private object _f;
    public object F {
        get {
            return _f;
        }
        set {
            _f = value;
        }
    }

    public List<Rate> RatesInGridView {
        get {
            List<Rate> ActualRates = new List<Rate>();
            GridView GridViewRates;
            if (IsPacket) {
                MultiView mv = (MultiView)this.FindControl("mvMain");
                View activeView = mv.GetActiveView();
                GridViewRates = (GridView)this.FindControl("GridViewRates" + activeView.ID.Substring(activeView.ID.Length - 1));
            } else {
                GridViewRates = (GridView)this.FindControl("GridViewRates" + "1");
            }
            
            foreach (GridViewRow row in GridViewRates.Rows) {
                Rate r = new Rate();
                TextBox tbNumber = (TextBox)row.Cells[1].FindControl("tbNumber");
                r.Number = Convert.ToInt32(tbNumber.Text);
                TextBox tbDate = (TextBox)row.Cells[2].FindControl("tbDate");
                r.Date = Convert.ToDateTime(tbDate.Text);
                TextBox tbvalue = (TextBox)row.Cells[3].FindControl("tbValue");
                r.Value = Convert.ToDecimal(tbvalue.Text);
                DropDownList ddlPaymentType = (DropDownList)row.Cells[4].FindControl("ddlPaymentTypes");
                //r.PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue);
                r.PolicyItemID = 1;
                ActualRates.Add(r);

            }

            return ActualRates;
        }
    }

    //public int selIndex {
    //    get {
    //        if (ViewState["selIndex"] != null) {
    //            return (int)ViewState["selIndex"];
    //        } else {
    //            return 0;
    //        }
    //    }
    //    set {
    //        ViewState["selIndex"] = value;
    //    }
    //}


    public bool IsPacket {
        get {
            //if (Session["isPacket"] != null) {
            //    return (bool)Session["isPacket"];
            //} else {
            //    return false;
            //}
            if (!String.IsNullOrEmpty(Request.QueryString["packet"])) {
                string s = Request.QueryString["packet"];
                return bool.Parse(Request.QueryString["packet"]);
            }
            return false;
        }
        set {
           // ViewState["isPacket"] = value;
            //Session["isPacket"] = value;
        }
    }

    public int DealID {
        get {
            if (ViewState["dealID"] != null) {
                return (int)ViewState["dealID"];
            } else {
                return 1;
            }
        }
        set {
            ViewState["dealID"] = value;
        }
    }

    public int InsuranceSubtypeID {
        get {
            if (ViewState["insuranceSubtypeID"] != null) {
                return (int)ViewState["insuranceSubtypeID"];
            } else {
                return 2;
            }
        }
        set {
            ViewState["insuranceSubtypeID"] = value;
        }
    }

    public int PolicyID {
        get {
            if (ViewState["policyID"] != null) {
                return (int)ViewState["policyID"];
            } else {
                return 0;
            }
        }
        set {
            ViewState["policyID"] = value;
        }
    }

    public int PacketID {
        get {
            if (ViewState["packetID"] != null) {
                return (int)ViewState["packetID"];
            } else {
                return 3;
            }
        }
        set {
            ViewState["packetID"] = value;
        }
    }

    protected void Page_Init(object sender, EventArgs e) {

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MultiView mv = (MultiView)this.FindControl("mvMain");
        if (!Page.IsPostBack) {
            if (IsPacket) {
                mv.ActiveViewIndex = 0;
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e) {
       ControlCollection cc = this.Controls;
    }


    protected override object SaveViewState() {
        object[] newViewState = new object[2];

        List<string> txtValues = new List<string>();
        string txtValue = string.Empty;

        ControlCollection cc = this.Controls;
 
        txtValues.Add(((DropDownList)this.FindControl("ddlNumberOfRates1")).SelectedIndex.ToString());
        txtValue = ((DropDownList)this.FindControl("ddlNumberOfRates1")).SelectedIndex.ToString();

        newViewState[0] = txtValue;
        newViewState[1] = base.SaveViewState();
        return newViewState;
    }

    protected override void LoadViewState(object savedState) {
        ControlCollection cc = this.Controls;

        //if we can identify the custom view state as defined in the override for SaveViewState  
        if (savedState is object[] && ((object[])savedState).Length == 2 && ((object[])savedState)[0] is string) {
            object[] newViewState = (object[])savedState;
            //string[] txtValues = (string[])(newViewState[0]);
            string txtValue = (string)(newViewState[0]);
            if (txtValue.Length > 0) {
                //re-load ddl
                DropDownList ddlToRemove = (DropDownList)this.FindControl("ddlNumberOfRates1");
                this.Controls.Remove(ddlToRemove);

                Button buttonInsert = (Button)this.Controls[2];
                this.Controls.Remove(buttonInsert);

                DropDownList ddlNumberOfRates = new DropDownList();
                ddlNumberOfRates.ID = "ddlNumberOfRates" + (0 + 1).ToString();
                ddlNumberOfRates.AutoPostBack = true;
                for (int k = 1; k <= 12; k++) {
                    ListItem li = new ListItem();
                    li.Text = k.ToString();
                    li.Value = k.ToString();
                    ddlNumberOfRates.Items.Add(li);
                }
                ddlNumberOfRates.SelectedIndexChanged += new EventHandler(ddlNumberOfRates_SelectedIndexChanged);
                this.Controls.Add(ddlNumberOfRates);
                ddlNumberOfRates.SelectedIndex = Convert.ToInt32(txtValue);
                this.Controls.Add(buttonInsert);
            }
            //load the ViewState normally  
            base.LoadViewState(newViewState[1]);
        } else {
            base.LoadViewState(savedState);
        }
    }

    private List<Label> lblFieldNames;
    private List<TextBox> tbValues;

    protected override void LoadControlState(object savedState) {
        object[] list = savedState as object[];
        base.LoadControlState(list[0]);
        if (list[1] != null)
            F = list[1];
    }


    protected override object SaveControlState() {
        object[] list = new object[2];
        list[0] = base.SaveControlState();
        list[1] = F;
        return list[1];
    }

    //protected override void CreateChildControls() {

    //   // bool b = test.IsPacket;
            

             
    //        if (IsPacket) {

    //            List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();

    //            int j = 0;
    //            foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
    //                List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(pist.InsuranceSubTypeID);

    //                lblFieldNames = new List<Label>();
    //                tbValues = new List<TextBox>();

    //                MultiView mvMain = (MultiView)this.FindControl("mvMain");

    //                HtmlTable defaultTable = new HtmlTable();
    //                HtmlTableRow firstDefaultTableRow = new HtmlTableRow();
    //                defaultTable.Rows.Add(firstDefaultTableRow);
    //                HtmlTableCell firstCellFirstRowInDefaultTable = new HtmlTableCell();
    //                firstDefaultTableRow.Cells.Add(firstCellFirstRowInDefaultTable);
    //                Label lblPolicyNumber = new Label();
    //                lblPolicyNumber.ID = "lblPolicyNumber" + (j + 1).ToString();
    //                lblPolicyNumber.Text = "Број на полиса";
    //                firstCellFirstRowInDefaultTable.Controls.Add(lblPolicyNumber);
    //                HtmlTableCell secondCellFirstRowInDefaultTable = new HtmlTableCell();
    //                firstDefaultTableRow.Cells.Add(secondCellFirstRowInDefaultTable);
    //                TextBox tbPolicyNumber = new TextBox();
    //                tbPolicyNumber.ID = "tbPolicyNumber" + (j + 1).ToString();
    //                tbPolicyNumber.MaxLength = 30;
    //                RequiredFieldValidator rfvPolicyNumber = new RequiredFieldValidator();
    //                rfvPolicyNumber.ID = "rfvPolicyNumber" + (j + 1).ToString();
    //                rfvPolicyNumber.ErrorMessage = "*";
    //                rfvPolicyNumber.Display = ValidatorDisplay.Dynamic;
    //                rfvPolicyNumber.ControlToValidate = tbPolicyNumber.ID;
    //                secondCellFirstRowInDefaultTable.Controls.Add(tbPolicyNumber);
    //                secondCellFirstRowInDefaultTable.Controls.Add(rfvPolicyNumber);

    //                HtmlTableRow secondDefaultTableRow = new HtmlTableRow();
    //                defaultTable.Rows.Add(secondDefaultTableRow);
    //                HtmlTableCell firstCellSecondRowInDefaultTable = new HtmlTableCell();
    //                secondDefaultTableRow.Cells.Add(firstCellSecondRowInDefaultTable);
    //                Label lblInsuranceType = new Label();
    //                lblInsuranceType.ID = "lblInsuranceType" + (j + 1).ToString();
    //                lblInsuranceType.Text = "Класа на осигурување";
    //                firstCellSecondRowInDefaultTable.Controls.Add(lblInsuranceType);
    //                HtmlTableCell secondCellSecondRowInDefaultTable = new HtmlTableCell();
    //                secondDefaultTableRow.Cells.Add(secondCellSecondRowInDefaultTable);
    //                TextBox tbInsuranceType = new TextBox();
    //                tbInsuranceType.ID = "tbInsuranceType" + (j + 1).ToString();
    //                tbInsuranceType.Text = pist.InsuranceSubType.InsuranceType.Name;
    //                tbInsuranceType.Width = 400;
    //                tbInsuranceType.ReadOnly = true;
    //                secondCellSecondRowInDefaultTable.Controls.Add(tbInsuranceType);

    //                HtmlTableRow thirdDefaultTableRow = new HtmlTableRow();
    //                defaultTable.Rows.Add(thirdDefaultTableRow);
    //                HtmlTableCell firstCellThirdRowInDefaultTable = new HtmlTableCell();
    //                thirdDefaultTableRow.Cells.Add(firstCellThirdRowInDefaultTable);
    //                Label lblInsuranceSubType = new Label();
    //                lblInsuranceSubType.ID = "lblInsuranceSubType" + (j + 1).ToString();
    //                lblInsuranceSubType.Text = "Подкласа на осигурување";
    //                firstCellThirdRowInDefaultTable.Controls.Add(lblInsuranceSubType);
    //                HtmlTableCell secondCellThirdRowInDefaultTable = new HtmlTableCell();
    //                thirdDefaultTableRow.Cells.Add(secondCellThirdRowInDefaultTable);
    //                TextBox tbInsuranceSubType = new TextBox();
    //                tbInsuranceSubType.ID = "tbInsuranceSubType" + (j + 1).ToString();
    //                tbInsuranceSubType.Text = pist.InsuranceSubType.Description;
    //                tbInsuranceSubType.Width = 400;
    //                tbInsuranceSubType.ReadOnly = true;
    //                secondCellThirdRowInDefaultTable.Controls.Add(tbInsuranceSubType);

    //                HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
    //                defaultTable.Rows.Add(fourthDefaultTableRow);
    //                HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
    //                fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
    //                Label lblPolicyValue = new Label();
    //                lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
    //                lblPolicyValue.Text = "Премија";
    //                firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
    //                HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
    //                fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
    //                TextBox tbPolicyValue = new TextBox();
    //                tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
    //                tbPolicyValue.AutoPostBack = true;
    //                tbPolicyValue.TextChanged += new EventHandler(tbPolicyValue_TextChanged);
    //                RequiredFieldValidator rfvPolicyValue = new RequiredFieldValidator();
    //                rfvPolicyValue.ID = "rfvPolicyValue" + (j + 1).ToString();
    //                rfvPolicyValue.ErrorMessage = "*";
    //                rfvPolicyValue.Display = ValidatorDisplay.Dynamic;
    //                rfvPolicyValue.ControlToValidate = tbPolicyValue.ID;
    //                CompareValidator cvPolicyValue = new CompareValidator();
    //                cvPolicyValue.ID = "cvPolicyValue" + (j + 1).ToString();
    //                cvPolicyValue.ErrorMessage = "*";
    //                cvPolicyValue.Display = ValidatorDisplay.Dynamic;
    //                cvPolicyValue.ControlToValidate = tbPolicyValue.ID;
    //                cvPolicyValue.Operator = ValidationCompareOperator.DataTypeCheck;
    //                cvPolicyValue.Type = ValidationDataType.Double;
    //                secondCellFourthRowInDefaultTable.Controls.Add(tbPolicyValue);
    //                secondCellFourthRowInDefaultTable.Controls.Add(rfvPolicyValue);
    //                secondCellFourthRowInDefaultTable.Controls.Add(cvPolicyValue);

    //                HtmlTableRow fifthDefaultTableRow = new HtmlTableRow();
    //                defaultTable.Rows.Add(fifthDefaultTableRow);
    //                HtmlTableCell firstCellFifthRowInDefaultTable = new HtmlTableCell();
    //                fifthDefaultTableRow.Cells.Add(firstCellFifthRowInDefaultTable);
    //                Label lblBrokeragePercentage = new Label();
    //                lblBrokeragePercentage.ID = "lblBrokeragePercentage" + (j + 1).ToString();
    //                lblBrokeragePercentage.Text = "Процент на брокеража";
    //                firstCellFifthRowInDefaultTable.Controls.Add(lblBrokeragePercentage);
    //                HtmlTableCell secondCellFifthRowInDefaultTable = new HtmlTableCell();
    //                fifthDefaultTableRow.Cells.Add(secondCellFifthRowInDefaultTable);
    //                TextBox tbBrokeragePercentage = new TextBox();
    //                tbBrokeragePercentage.ID = "tbBrokeragePercentage" + (j + 1).ToString();
    //                tbBrokeragePercentage.Text = pist.BrokeragePecentage.ToString();
    //                tbBrokeragePercentage.ReadOnly = true;
    //                secondCellFifthRowInDefaultTable.Controls.Add(tbBrokeragePercentage);

    //                HtmlTableRow sixthDefaultTableRow = new HtmlTableRow();
    //                defaultTable.Rows.Add(sixthDefaultTableRow);
    //                HtmlTableCell firstCellSixthRowInDefaultTable = new HtmlTableCell();
    //                sixthDefaultTableRow.Cells.Add(firstCellSixthRowInDefaultTable);
    //                Label lblBrokerageValue = new Label();
    //                lblBrokerageValue.ID = "lblBrokerageValue" + (j + 1).ToString();
    //                lblBrokerageValue.Text = "Брокеража";
    //                firstCellSixthRowInDefaultTable.Controls.Add(lblBrokerageValue);
    //                HtmlTableCell secondCellSixthRowInDefaultTable = new HtmlTableCell();
    //                sixthDefaultTableRow.Cells.Add(secondCellSixthRowInDefaultTable);
    //                TextBox tbBrokerageValue = new TextBox();
    //                tbBrokerageValue.ID = "tbBrokerageValue" + (j + 1).ToString();
    //                tbBrokerageValue.ReadOnly = true;
    //                secondCellSixthRowInDefaultTable.Controls.Add(tbBrokerageValue);

    //                mvMain.Views[j].Controls.Add(defaultTable);


    //                HtmlTable table = new HtmlTable();

    //                foreach (Broker.DataAccess.Control c in listControls) {
    //                    if (c.IsActive) {
    //                        HtmlTableRow tableRow = new HtmlTableRow();
    //                        table.Rows.Add(tableRow);
    //                        HtmlTableCell tableCell = new HtmlTableCell();
    //                        tableRow.Cells.Add(tableCell);
    //                        Label label = new Label();
    //                        label.ID = c.LabelID;
    //                        label.Text = c.LabelName;
    //                        lblFieldNames.Add(label);
    //                        tableCell.Controls.Add(label);
    //                        HtmlTableCell tableCellSecond = new HtmlTableCell();
    //                        tableRow.Cells.Add(tableCellSecond);
    //                        TextBox textbox = new TextBox();
    //                        textbox.ID = c.TextBoxID;
    //                        tbValues.Add(textbox);
    //                        tableCellSecond.Controls.Add(textbox);
    //                        if (c.HasRequredFieldValidator) {
    //                            RequiredFieldValidator rfv = new RequiredFieldValidator();
    //                            rfv.ID = "rfv" + textbox.ID;
    //                            rfv.ErrorMessage = "*";
    //                            rfv.Display = ValidatorDisplay.Dynamic;
    //                            rfv.ControlToValidate = textbox.ID;
    //                            tableCellSecond.Controls.Add(rfv);
    //                        }
    //                    }
    //                }
    //                mvMain.Views[j].Controls.Add(table);

    //                //DropDownList ddlNumberOfRates = new DropDownList();
    //                //ddlNumberOfRates.ID = "ddlNumberOfRates" + (j + 1).ToString();
    //                //ddlNumberOfRates.AutoPostBack = true;
    //                //for (int k = 1; k <= 12; k++) {
    //                //    ListItem li = new ListItem();
    //                //    li.Text = k.ToString();
    //                //    li.Value = k.ToString();
    //                //    ddlNumberOfRates.Items.Add(li);
    //                //}
    //                //ddlNumberOfRates.SelectedIndexChanged += new EventHandler(ddlNumberOfRates_SelectedIndexChanged);
    //                //mvMain.Views[j].Controls.Add(ddlNumberOfRates);


    //                //GridView GridViewRates = new GridView();
    //                //GridViewRates.ID = "GridViewRates" + (j + 1).ToString();
    //                //GridViewRates.AutoGenerateColumns = false;
    //                //GridViewRates.AutoGenerateSelectButton = true;
    //                //GridViewRates.SelectedIndexChanged += new EventHandler(GridViewRates_SelectedIndexChanged);
    //                //GridViewRates.RowCommand += new GridViewCommandEventHandler(GridViewRates_RowCommand);
    //                //TemplateField tfNumber = new TemplateField();
    //                //tfNumber.ItemTemplate = new GridViewTemplate(ListItemType.Item, "Number", true, false);
    //                //tfNumber.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Реден број", true, false);
    //                //GridViewRates.Columns.Add(tfNumber);
    //                //TemplateField tfDate = new TemplateField();
    //                //tfDate.ItemTemplate = new GridViewTemplate(ListItemType.Item, "Date", true, false);
    //                //tfDate.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Датум", true, false);
    //                //GridViewRates.Columns.Add(tfDate);
    //                //TemplateField tfValue = new TemplateField();
    //                //tfValue.ItemTemplate = new GridViewTemplate(ListItemType.Item, "Value", false, false);
    //                //tfValue.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Износ", false, false);
    //                //GridViewRates.Columns.Add(tfValue);
    //                //TemplateField tfPaymentType = new TemplateField();
    //                //tfPaymentType.ItemTemplate = new GridViewTemplate(ListItemType.Item, "PaymentTypeID", false, true);
    //                //tfPaymentType.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Тип на плаќање", false, true);
    //                //GridViewRates.Columns.Add(tfPaymentType);
    //                //mvMain.Views[j].Controls.Add(GridViewRates);

    //                j++;
    //            }
    //        } else {
    //            int j = 0;
    //            List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(InsuranceSubtypeID);

    //            lblFieldNames = new List<Label>();
    //            tbValues = new List<TextBox>();

    //            HtmlTable defaultTable = new HtmlTable();
    //            HtmlTableRow firstDefaultTableRow = new HtmlTableRow();
    //            defaultTable.Rows.Add(firstDefaultTableRow);
    //            HtmlTableCell firstCellFirstRowInDefaultTable = new HtmlTableCell();
    //            firstDefaultTableRow.Cells.Add(firstCellFirstRowInDefaultTable);
    //            Label lblPolicyNumber = new Label();
    //            lblPolicyNumber.ID = "lblPolicyNumber" + (j + 1).ToString();
    //            lblPolicyNumber.Text = "Број на полиса";
    //            firstCellFirstRowInDefaultTable.Controls.Add(lblPolicyNumber);
    //            HtmlTableCell secondCellFirstRowInDefaultTable = new HtmlTableCell();
    //            firstDefaultTableRow.Cells.Add(secondCellFirstRowInDefaultTable);
    //            TextBox tbPolicyNumber = new TextBox();
    //            tbPolicyNumber.ID = "tbPolicyNumber" + (j + 1).ToString();
    //            tbPolicyNumber.MaxLength = 30;
    //            RequiredFieldValidator rfvPolicyNumber = new RequiredFieldValidator();
    //            rfvPolicyNumber.ID = "rfvPolicyNumber" + (j + 1).ToString();
    //            rfvPolicyNumber.ErrorMessage = "*";
    //            rfvPolicyNumber.Display = ValidatorDisplay.Dynamic;
    //            rfvPolicyNumber.ControlToValidate = tbPolicyNumber.ID;
    //            secondCellFirstRowInDefaultTable.Controls.Add(tbPolicyNumber);
    //            secondCellFirstRowInDefaultTable.Controls.Add(rfvPolicyNumber);

    //            HtmlTableRow secondDefaultTableRow = new HtmlTableRow();
    //            defaultTable.Rows.Add(secondDefaultTableRow);
    //            HtmlTableCell firstCellSecondRowInDefaultTable = new HtmlTableCell();
    //            secondDefaultTableRow.Cells.Add(firstCellSecondRowInDefaultTable);
    //            Label lblInsuranceType = new Label();
    //            lblInsuranceType.ID = "lblInsuranceType" + (j + 1).ToString();
    //            lblInsuranceType.Text = "Класа на осигурување";
    //            firstCellSecondRowInDefaultTable.Controls.Add(lblInsuranceType);
    //            HtmlTableCell secondCellSecondRowInDefaultTable = new HtmlTableCell();
    //            secondDefaultTableRow.Cells.Add(secondCellSecondRowInDefaultTable);
    //            TextBox tbInsuranceType = new TextBox();
    //            tbInsuranceType.ID = "tbInsuranceType" + (j + 1).ToString();
    //            InsuranceSubType ist = InsuranceSubType.Get(InsuranceSubtypeID);
    //            tbInsuranceType.Text = ist.InsuranceType.Name;
    //            tbInsuranceType.Width = 400;
    //            tbInsuranceType.ReadOnly = true;
    //            secondCellSecondRowInDefaultTable.Controls.Add(tbInsuranceType);

    //            HtmlTableRow thirdDefaultTableRow = new HtmlTableRow();
    //            defaultTable.Rows.Add(thirdDefaultTableRow);
    //            HtmlTableCell firstCellThirdRowInDefaultTable = new HtmlTableCell();
    //            thirdDefaultTableRow.Cells.Add(firstCellThirdRowInDefaultTable);
    //            Label lblInsuranceSubType = new Label();
    //            lblInsuranceSubType.ID = "lblInsuranceSubType" + (j + 1).ToString();
    //            lblInsuranceSubType.Text = "Подкласа на осигурување";
    //            firstCellThirdRowInDefaultTable.Controls.Add(lblInsuranceSubType);
    //            HtmlTableCell secondCellThirdRowInDefaultTable = new HtmlTableCell();
    //            thirdDefaultTableRow.Cells.Add(secondCellThirdRowInDefaultTable);
    //            TextBox tbInsuranceSubType = new TextBox();
    //            tbInsuranceSubType.ID = "tbInsuranceSubType" + (j + 1).ToString();
    //            tbInsuranceSubType.Text = ist.Description;
    //            tbInsuranceSubType.Width = 400;
    //            tbInsuranceSubType.ReadOnly = true;
    //            secondCellThirdRowInDefaultTable.Controls.Add(tbInsuranceSubType);

    //            HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
    //            defaultTable.Rows.Add(fourthDefaultTableRow);
    //            HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
    //            fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
    //            Label lblPolicyValue = new Label();
    //            lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
    //            lblPolicyValue.Text = "Премија";
    //            firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
    //            HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
    //            fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
    //            TextBox tbPolicyValue = new TextBox();
    //            tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
    //            tbPolicyValue.AutoPostBack = true;
    //            tbPolicyValue.TextChanged += new EventHandler(tbPolicyValue_TextChanged);
    //            RequiredFieldValidator rfvPolicyValue = new RequiredFieldValidator();
    //            rfvPolicyValue.ID = "rfvPolicyValue" + (j + 1).ToString();
    //            rfvPolicyValue.ErrorMessage = "*";
    //            rfvPolicyValue.Display = ValidatorDisplay.Dynamic;
    //            rfvPolicyValue.ControlToValidate = tbPolicyValue.ID;
    //            CompareValidator cvPolicyValue = new CompareValidator();
    //            cvPolicyValue.ID = "cvPolicyValue" + (j + 1).ToString();
    //            cvPolicyValue.ErrorMessage = "*";
    //            cvPolicyValue.Display = ValidatorDisplay.Dynamic;
    //            cvPolicyValue.ControlToValidate = tbPolicyValue.ID;
    //            cvPolicyValue.Operator = ValidationCompareOperator.DataTypeCheck;
    //            cvPolicyValue.Type = ValidationDataType.Double;
    //            secondCellFourthRowInDefaultTable.Controls.Add(tbPolicyValue);
    //            secondCellFourthRowInDefaultTable.Controls.Add(rfvPolicyValue);
    //            secondCellFourthRowInDefaultTable.Controls.Add(cvPolicyValue);

    //            HtmlTableRow fifthDefaultTableRow = new HtmlTableRow();
    //            defaultTable.Rows.Add(fifthDefaultTableRow);
    //            HtmlTableCell firstCellFifthRowInDefaultTable = new HtmlTableCell();
    //            fifthDefaultTableRow.Cells.Add(firstCellFifthRowInDefaultTable);
    //            Label lblBrokeragePercentage = new Label();
    //            lblBrokeragePercentage.ID = "lblBrokeragePercentage" + (j + 1).ToString();
    //            lblBrokeragePercentage.Text = "Процент на брокеража";
    //            firstCellFifthRowInDefaultTable.Controls.Add(lblBrokeragePercentage);
    //            HtmlTableCell secondCellFifthRowInDefaultTable = new HtmlTableCell();
    //            fifthDefaultTableRow.Cells.Add(secondCellFifthRowInDefaultTable);
    //            TextBox tbBrokeragePercentage = new TextBox();
    //            tbBrokeragePercentage.ID = "tbBrokeragePercentage" + (j + 1).ToString();
    //            tbBrokeragePercentage.Text = Brokerage.GetByInsuranceSubTypeAndDeal(InsuranceSubtypeID, DealID).Percentage.ToString();
    //            tbBrokeragePercentage.ReadOnly = true;
    //            secondCellFifthRowInDefaultTable.Controls.Add(tbBrokeragePercentage);

    //            HtmlTableRow sixthDefaultTableRow = new HtmlTableRow();
    //            defaultTable.Rows.Add(sixthDefaultTableRow);
    //            HtmlTableCell firstCellSixthRowInDefaultTable = new HtmlTableCell();
    //            sixthDefaultTableRow.Cells.Add(firstCellSixthRowInDefaultTable);
    //            Label lblBrokerageValue = new Label();
    //            lblBrokerageValue.ID = "lblBrokerageValue" + (j + 1).ToString();
    //            lblBrokerageValue.Text = "Брокеража";
    //            firstCellSixthRowInDefaultTable.Controls.Add(lblBrokerageValue);
    //            HtmlTableCell secondCellSixthRowInDefaultTable = new HtmlTableCell();
    //            sixthDefaultTableRow.Cells.Add(secondCellSixthRowInDefaultTable);
    //            TextBox tbBrokerageValue = new TextBox();
    //            tbBrokerageValue.ID = "tbBrokerageValue" + (j + 1).ToString();
    //            tbBrokerageValue.ReadOnly = true;
    //            secondCellSixthRowInDefaultTable.Controls.Add(tbBrokerageValue);

    //            this.Controls.Add(defaultTable);

               
    //            HtmlTable table = new HtmlTable();

    //            foreach (Broker.DataAccess.Control c in listControls) {
    //                if (c.IsActive) {
    //                    HtmlTableRow tableRow = new HtmlTableRow();
    //                    table.Rows.Add(tableRow);
    //                    HtmlTableCell tableCell = new HtmlTableCell();
    //                    tableRow.Cells.Add(tableCell);
    //                    Label label = new Label();
    //                    label.ID = c.LabelID;
    //                    label.Text = c.LabelName;
    //                    lblFieldNames.Add(label);
    //                    tableCell.Controls.Add(label);
    //                    HtmlTableCell tableCellSecond = new HtmlTableCell();
    //                    tableRow.Cells.Add(tableCellSecond);
    //                    TextBox textbox = new TextBox();
    //                    textbox.ID = c.TextBoxID;
    //                    tbValues.Add(textbox);
    //                    tableCellSecond.Controls.Add(textbox);
    //                    if (c.HasRequredFieldValidator) {
    //                        RequiredFieldValidator rfv = new RequiredFieldValidator();
    //                        rfv.ID = "rfv" + textbox.ID;
    //                        rfv.ErrorMessage = "*";
    //                        rfv.Display = ValidatorDisplay.Dynamic;
    //                        rfv.ControlToValidate = textbox.ID;
    //                        tableCellSecond.Controls.Add(rfv);
    //                    }
    //                }
    //            }
    //            this.Controls.Add(table);

    //            //DropDownList ddlNumberOfRates = new DropDownList();
    //            //ddlNumberOfRates.ID = "ddlNumberOfRates" + (j + 1).ToString();
    //            //ddlNumberOfRates.AutoPostBack = true;
    //            //for (int k = 1; k <= 12; k++) {
    //            //    ListItem li = new ListItem();
    //            //    li.Text = k.ToString();
    //            //    li.Value = k.ToString();
    //            //    ddlNumberOfRates.Items.Add(li);
    //            //}
    //            //ddlNumberOfRates.SelectedIndexChanged += new EventHandler(ddlNumberOfRates_SelectedIndexChanged);

    //            //this.Controls.Add(ddlNumberOfRates);

    //            //GridView GridViewRates = new GridView();
    //            //GridViewRates.ID = "GridViewRates" + (j + 1).ToString();
    //            //GridViewRates.AutoGenerateColumns = false;
    //            //GridViewRates.AutoGenerateSelectButton = true;
    //            //GridViewRates.SelectedIndexChanged += new EventHandler(GridViewRates_SelectedIndexChanged);
    //            //GridViewRates.RowCommand += new GridViewCommandEventHandler(GridViewRates_RowCommand);
    //            //TemplateField tfNumber = new TemplateField();
    //            //tfNumber.ItemTemplate = new GridViewTemplate(ListItemType.Item, "Number", true, false);
    //            //tfNumber.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Реден број", true, false);
    //            //GridViewRates.Columns.Add(tfNumber);
    //            //TemplateField tfDate = new TemplateField();
    //            //tfDate.ItemTemplate = new GridViewTemplate(ListItemType.Item, "Date", true, false);
    //            //tfDate.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Датум", true, false);
    //            //GridViewRates.Columns.Add(tfDate);
    //            //TemplateField tfValue = new TemplateField();
    //            //tfValue.ItemTemplate = new GridViewTemplate(ListItemType.Item, "Value", false, false);
    //            //tfValue.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Износ", false, false);
    //            //GridViewRates.Columns.Add(tfValue);
    //            //TemplateField tfPaymentType = new TemplateField();
    //            //tfPaymentType.ItemTemplate = new GridViewTemplate(ListItemType.Item, "PaymentTypeID", false, true);
    //            //tfPaymentType.HeaderTemplate = new GridViewTemplate(ListItemType.Header, "Тип на плаќање", false, true);
    //            //GridViewRates.Columns.Add(tfPaymentType);
    //            //this.Controls.Add(GridViewRates);

    //        }

    //        Button btnInsert = new Button();
    //        btnInsert.ID = "btnInsert";
    //        btnInsert.Text = "Внеси";
    //        btnInsert.CausesValidation = true;
    //        btnInsert.Click += new EventHandler(btnInsert_Click);
    //        this.Controls.Add(btnInsert);

    //        base.CreateChildControls();
        
    //}

    void buttonForView_Click(object sender, EventArgs e) {
        MultiView mv = (MultiView)this.FindControl("mvMain");
        Button btnView = (Button)sender;
        string viewName = "view" + btnView.ID.Substring(btnView.ID.Length - 1);
        View activeView = (View)this.FindControl(viewName);
        mv.SetActiveView(activeView);
    }


    void ddlNumberOfRates_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlNumberOfRates = (DropDownList)sender;
        List<Rate> lRates = new List<Rate>();
        TextBox tbPolicyNumber = (TextBox)this.FindControl("tbPolicyNumber" + ddlNumberOfRates.ID.Substring(ddlNumberOfRates.ID.Length-1));
        string policyNumber = tbPolicyNumber.Text;
        
        TextBox tbPolicyValue = (TextBox)this.FindControl("tbPolicyValue" + ddlNumberOfRates.ID.Substring(ddlNumberOfRates.ID.Length - 1));
        decimal policyValue = Convert.ToDecimal(tbPolicyValue.Text);

        Policy policy = Policy.Get(PolicyID);

        if(policy!=null){
            DateTime startDate = policy.ApplicationDate;
            decimal totalPolicyCost = policyValue;

            int ratesNumber = Convert.ToInt32(ddlNumberOfRates.SelectedValue);
            for (int i = 0; i < ratesNumber; i++) {
                Rate r = new Rate();
                r.Number = i + 1;
                r.Date = startDate.AddMonths(i);
                r.Value = RateController.Scale5(totalPolicyCost / ratesNumber);
                ///Ova da se proveri!!!
                r.PolicyItemID = 0;
                //r.PaymentTypeID = 1;
                lRates.Add(r);
            }
        }

        GridView GridViewRates = (GridView)this.FindControl("GridViewRates" + ddlNumberOfRates.ID.Substring(ddlNumberOfRates.ID.Length - 1));

        GridViewRates.AutoGenerateColumns = false;
        GridViewRates.DataSource = lRates;
        GridViewRates.DataBind();

    }

    void tbPolicyValue_TextChanged(object sender, EventArgs e) {
        TextBox tbPolicyValue = (TextBox)sender;
        int index = Convert.ToInt32(tbPolicyValue.ID.Substring(tbPolicyValue.ID.Length - 1));
        TextBox tbBrokeragePercentage = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokeragePercentage" + index.ToString());
        TextBox tbBrokerageValue = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokerageValue" + index.ToString());

        tbBrokerageValue.Text = (Convert.ToDecimal(tbPolicyValue.Text) * Convert.ToDecimal(tbBrokeragePercentage.Text) / 100).ToString();

    }

    void btnInsert_Click(object sender, EventArgs e) {
        if (IsPacket) {
            List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();
            int i = 0;
            foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
                PolicyItem policyItem = new PolicyItem();
                TextBox tbBrokeragePercentage = (TextBox)this.FindControl("tbBrokeragePercentage" + (i + 1).ToString());
                policyItem.BrokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
                TextBox tbPolicyNumber = (TextBox)this.FindControl("tbPolicyNumber" + (i + 1).ToString());
                policyItem.PolicyNumber = tbPolicyNumber.Text;
                TextBox tbBrokerageValue = (TextBox)this.FindControl("tbBrokerageValue" + (i + 1).ToString());
                policyItem.BrokerageValue = Convert.ToDecimal(tbBrokerageValue.Text);
                policyItem.InsuranceSubTypeID = pist.InsuranceSubTypeID;
                //Da se zema  od kaj so treba!!!!!!!
                policyItem.PolicyID = PolicyID;
                TextBox tbPolicyValue = (TextBox)this.FindControl("tbPolicyValue" + (i + 1).ToString());
                policyItem.PremiumValue = Convert.ToDecimal(tbPolicyValue.Text);
                //Da se zema  od kaj so treba!!!!!!!
                policyItem.StatusID = 5;
                policyItem.Insert();

                List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(pist.InsuranceSubTypeID);
                foreach (Broker.DataAccess.Control c in listControls) {
                    PolicyExtendInformation pei = new PolicyExtendInformation();
                    pei.ControlID = c.ID;
                    pei.PolicyItemID = policyItem.ID;
                    TextBox tbControl = (TextBox)this.FindControl(c.TextBoxID);
                    pei.Value = tbControl.Text;
                    pei.Insert();
                }
                i++;
            }
        } else {
            int i = 0;
            PolicyItem policyItem = new PolicyItem();
            TextBox tbBrokeragePercentage = (TextBox)this.FindControl("tbBrokeragePercentage" + (i + 1).ToString());
            policyItem.BrokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
            TextBox tbPolicyNumber = (TextBox)this.FindControl("tbPolicyNumber" + (i + 1).ToString());
            policyItem.PolicyNumber = tbPolicyNumber.Text;
            TextBox tbBrokerageValue = (TextBox)this.FindControl("tbBrokerageValue" + (i + 1).ToString());
            policyItem.BrokerageValue = Convert.ToDecimal(tbBrokerageValue.Text);
            policyItem.InsuranceSubTypeID = InsuranceSubtypeID;
            //Da se zema  od kaj so treba!!!!!!!
            policyItem.PolicyID = PolicyID;
            TextBox tbPolicyValue = (TextBox)this.FindControl("tbPolicyValue" + (i + 1).ToString());
            policyItem.PremiumValue = Convert.ToDecimal(tbPolicyValue.Text);
            //Da se zema  od kaj so treba!!!!!!!
            policyItem.StatusID = 5;
            policyItem.Insert();

            List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(InsuranceSubtypeID);
            foreach (Broker.DataAccess.Control c in listControls) {
                PolicyExtendInformation pei = new PolicyExtendInformation();
                pei.ControlID = c.ID;
                pei.PolicyItemID = policyItem.ID;
                TextBox tbControl = (TextBox)this.FindControl(c.TextBoxID);
                pei.Value = tbControl.Text;
                pei.Insert();
            }



        }
        
    }

    public void Test(EventArgs e) {
        OnInit(e);
    }

    protected override void OnLoad(EventArgs e) {

        base.OnLoad(e);
    }

    protected override void OnInit(EventArgs e) {

        if (!String.IsNullOrEmpty(Request.QueryString["packet"])) {
            string s = Request.QueryString["packet"];

            IsPacket = bool.Parse(Request.QueryString["packet"]);
        }
        // RadioButtonList rbl = test.FindControl("IsPacketRadioButtonList") as RadioButtonList;
        if (IsPacket) {

            List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();

            int n = 1;
            foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
                Button buttonForView = new Button();
                buttonForView.ID = "btnView" + n.ToString();
                buttonForView.Text = pist.InsuranceSubType.ShortDescription;
                buttonForView.CausesValidation = true;
                buttonForView.Click += new EventHandler(buttonForView_Click);
                this.Controls.Add(buttonForView);
                n++;
            }

            MultiView mvMain = new MultiView();
            mvMain.ID = "mvMain";
            this.Controls.Add(mvMain);

            int i = 1;
            foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
                View view = new View();
                view.ID = "view" + i.ToString();
                mvMain.Controls.Add(view);
                i++;
            }

            int j = 0;
            foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
                List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(pist.InsuranceSubTypeID);

                lblFieldNames = new List<Label>();
                tbValues = new List<TextBox>();

                HtmlTable defaultTable = new HtmlTable();
                HtmlTableRow firstDefaultTableRow = new HtmlTableRow();
                defaultTable.Rows.Add(firstDefaultTableRow);
                HtmlTableCell firstCellFirstRowInDefaultTable = new HtmlTableCell();
                firstDefaultTableRow.Cells.Add(firstCellFirstRowInDefaultTable);
                Label lblPolicyNumber = new Label();
                lblPolicyNumber.ID = "lblPolicyNumber" + (j + 1).ToString();
                lblPolicyNumber.Text = "Број на полиса";
                firstCellFirstRowInDefaultTable.Controls.Add(lblPolicyNumber);
                HtmlTableCell secondCellFirstRowInDefaultTable = new HtmlTableCell();
                firstDefaultTableRow.Cells.Add(secondCellFirstRowInDefaultTable);
                TextBox tbPolicyNumber = new TextBox();
                tbPolicyNumber.ID = "tbPolicyNumber" + (j + 1).ToString();
                tbPolicyNumber.MaxLength = 30;
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
                secondDefaultTableRow.Cells.Add(firstCellSecondRowInDefaultTable);
                Label lblInsuranceType = new Label();
                lblInsuranceType.ID = "lblInsuranceType" + (j + 1).ToString();
                lblInsuranceType.Text = "Класа на осигурување";
                firstCellSecondRowInDefaultTable.Controls.Add(lblInsuranceType);
                HtmlTableCell secondCellSecondRowInDefaultTable = new HtmlTableCell();
                secondDefaultTableRow.Cells.Add(secondCellSecondRowInDefaultTable);
                TextBox tbInsuranceType = new TextBox();
                tbInsuranceType.ID = "tbInsuranceType" + (j + 1).ToString();
                tbInsuranceType.Text = pist.InsuranceSubType.InsuranceType.Name;
                tbInsuranceType.Width = 400;
                tbInsuranceType.ReadOnly = true;
                secondCellSecondRowInDefaultTable.Controls.Add(tbInsuranceType);

                HtmlTableRow thirdDefaultTableRow = new HtmlTableRow();
                defaultTable.Rows.Add(thirdDefaultTableRow);
                HtmlTableCell firstCellThirdRowInDefaultTable = new HtmlTableCell();
                thirdDefaultTableRow.Cells.Add(firstCellThirdRowInDefaultTable);
                Label lblInsuranceSubType = new Label();
                lblInsuranceSubType.ID = "lblInsuranceSubType" + (j + 1).ToString();
                lblInsuranceSubType.Text = "Подкласа на осигурување";
                firstCellThirdRowInDefaultTable.Controls.Add(lblInsuranceSubType);
                HtmlTableCell secondCellThirdRowInDefaultTable = new HtmlTableCell();
                thirdDefaultTableRow.Cells.Add(secondCellThirdRowInDefaultTable);
                TextBox tbInsuranceSubType = new TextBox();
                tbInsuranceSubType.ID = "tbInsuranceSubType" + (j + 1).ToString();
                tbInsuranceSubType.Text = pist.InsuranceSubType.Description;
                tbInsuranceSubType.Width = 400;
                tbInsuranceSubType.ReadOnly = true;
                secondCellThirdRowInDefaultTable.Controls.Add(tbInsuranceSubType);

                HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
                defaultTable.Rows.Add(fourthDefaultTableRow);
                HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
                fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
                Label lblPolicyValue = new Label();
                lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
                lblPolicyValue.Text = "Премија";
                firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
                HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
                fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
                TextBox tbPolicyValue = new TextBox();
                tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
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

                //HtmlTableRow fifthDefaultTableRow = new HtmlTableRow();
                //defaultTable.Rows.Add(fifthDefaultTableRow);
                //HtmlTableCell firstCellFifthRowInDefaultTable = new HtmlTableCell();
                //fifthDefaultTableRow.Cells.Add(firstCellFifthRowInDefaultTable);
                //Label lblBrokeragePercentage = new Label();
                //lblBrokeragePercentage.ID = "lblBrokeragePercentage" + (j + 1).ToString();
                //lblBrokeragePercentage.Text = "Процент на брокеража";
                //firstCellFifthRowInDefaultTable.Controls.Add(lblBrokeragePercentage);
                //HtmlTableCell secondCellFifthRowInDefaultTable = new HtmlTableCell();
                //fifthDefaultTableRow.Cells.Add(secondCellFifthRowInDefaultTable);
                //TextBox tbBrokeragePercentage = new TextBox();
                //tbBrokeragePercentage.ID = "tbBrokeragePercentage" + (j + 1).ToString();
                //tbBrokeragePercentage.Text = pist.BrokeragePercentageForPrivates.ToString();
                //tbBrokeragePercentage.ReadOnly = true;
                //secondCellFifthRowInDefaultTable.Controls.Add(tbBrokeragePercentage);

                //HtmlTableRow sixthDefaultTableRow = new HtmlTableRow();
                //defaultTable.Rows.Add(sixthDefaultTableRow);
                //HtmlTableCell firstCellSixthRowInDefaultTable = new HtmlTableCell();
                //sixthDefaultTableRow.Cells.Add(firstCellSixthRowInDefaultTable);
                //Label lblBrokerageValue = new Label();
                //lblBrokerageValue.ID = "lblBrokerageValue" + (j + 1).ToString();
                //lblBrokerageValue.Text = "Брокеража";
                //firstCellSixthRowInDefaultTable.Controls.Add(lblBrokerageValue);
                //HtmlTableCell secondCellSixthRowInDefaultTable = new HtmlTableCell();
                //sixthDefaultTableRow.Cells.Add(secondCellSixthRowInDefaultTable);
                //TextBox tbBrokerageValue = new TextBox();
                //tbBrokerageValue.ID = "tbBrokerageValue" + (j + 1).ToString();
                //tbBrokerageValue.ReadOnly = true;
                //secondCellSixthRowInDefaultTable.Controls.Add(tbBrokerageValue);

                mvMain.Views[j].Controls.Add(defaultTable);


                HtmlTable table = new HtmlTable();

                foreach (Broker.DataAccess.Control c in listControls) {
                    if (c.IsActive) {
                        HtmlTableRow tableRow = new HtmlTableRow();
                        table.Rows.Add(tableRow);
                        HtmlTableCell tableCell = new HtmlTableCell();
                        tableRow.Cells.Add(tableCell);
                        Label label = new Label();
                        label.ID = c.LabelID;
                        label.Text = c.LabelName;
                        lblFieldNames.Add(label);
                        tableCell.Controls.Add(label);
                        HtmlTableCell tableCellSecond = new HtmlTableCell();
                        tableRow.Cells.Add(tableCellSecond);
                        TextBox textbox = new TextBox();
                        textbox.ID = c.TextBoxID;
                        tbValues.Add(textbox);
                        tableCellSecond.Controls.Add(textbox);
                        if (c.HasRequredFieldValidator) {
                            RequiredFieldValidator rfv = new RequiredFieldValidator();
                            rfv.ID = "rfv" + textbox.ID;
                            rfv.ErrorMessage = "*";
                            rfv.Display = ValidatorDisplay.Dynamic;
                            rfv.ControlToValidate = textbox.ID;
                            tableCellSecond.Controls.Add(rfv);
                        }
                    }
                }
                mvMain.Views[j].Controls.Add(table);
                j++;
            }

            int m = 0;
            foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
                DropDownList ddlNumberOfRates = new DropDownList();
                ddlNumberOfRates.ID = "ddlNumberOfRates" + (m + 1).ToString();
                ddlNumberOfRates.AutoPostBack = true;
                for (int k = 1; k <= 12; k++) {
                    ListItem li = new ListItem();
                    li.Text = k.ToString();
                    li.Value = k.ToString();
                    ddlNumberOfRates.Items.Add(li);
                }
                ddlNumberOfRates.SelectedIndexChanged += new EventHandler(ddlNumberOfRates_SelectedIndexChanged);
                mvMain.Views[m].Controls.Add(ddlNumberOfRates);
                m++;
            }
        } else {

            int j = 0;
            List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(InsuranceSubtypeID);

            lblFieldNames = new List<Label>();
            tbValues = new List<TextBox>();

            HtmlTable defaultTable = new HtmlTable();
            HtmlTableRow firstDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(firstDefaultTableRow);
            HtmlTableCell firstCellFirstRowInDefaultTable = new HtmlTableCell();
            firstDefaultTableRow.Cells.Add(firstCellFirstRowInDefaultTable);
            Label lblPolicyNumber = new Label();
            lblPolicyNumber.ID = "lblPolicyNumber" + (j + 1).ToString();
            lblPolicyNumber.Text = "Број на полиса";
            firstCellFirstRowInDefaultTable.Controls.Add(lblPolicyNumber);
            HtmlTableCell secondCellFirstRowInDefaultTable = new HtmlTableCell();
            firstDefaultTableRow.Cells.Add(secondCellFirstRowInDefaultTable);
            TextBox tbPolicyNumber = new TextBox();
            tbPolicyNumber.ID = "tbPolicyNumber" + (j + 1).ToString();
            tbPolicyNumber.MaxLength = 30;
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
            secondDefaultTableRow.Cells.Add(firstCellSecondRowInDefaultTable);
            Label lblInsuranceType = new Label();
            lblInsuranceType.ID = "lblInsuranceType" + (j + 1).ToString();
            lblInsuranceType.Text = "Класа на осигурување";
            firstCellSecondRowInDefaultTable.Controls.Add(lblInsuranceType);
            HtmlTableCell secondCellSecondRowInDefaultTable = new HtmlTableCell();
            secondDefaultTableRow.Cells.Add(secondCellSecondRowInDefaultTable);
            TextBox tbInsuranceType = new TextBox();
            tbInsuranceType.ID = "tbInsuranceType" + (j + 1).ToString();
            InsuranceSubType ist = InsuranceSubType.Get(InsuranceSubtypeID);
            tbInsuranceType.Text = ist.InsuranceType.Name;
            tbInsuranceType.Width = 400;
            tbInsuranceType.ReadOnly = true;
            secondCellSecondRowInDefaultTable.Controls.Add(tbInsuranceType);

            HtmlTableRow thirdDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(thirdDefaultTableRow);
            HtmlTableCell firstCellThirdRowInDefaultTable = new HtmlTableCell();
            thirdDefaultTableRow.Cells.Add(firstCellThirdRowInDefaultTable);
            Label lblInsuranceSubType = new Label();
            lblInsuranceSubType.ID = "lblInsuranceSubType" + (j + 1).ToString();
            lblInsuranceSubType.Text = "Подкласа на осигурување";
            firstCellThirdRowInDefaultTable.Controls.Add(lblInsuranceSubType);
            HtmlTableCell secondCellThirdRowInDefaultTable = new HtmlTableCell();
            thirdDefaultTableRow.Cells.Add(secondCellThirdRowInDefaultTable);
            TextBox tbInsuranceSubType = new TextBox();
            tbInsuranceSubType.ID = "tbInsuranceSubType" + (j + 1).ToString();
            tbInsuranceSubType.Text = ist.Description;
            tbInsuranceSubType.Width = 400;
            tbInsuranceSubType.ReadOnly = true;
            secondCellThirdRowInDefaultTable.Controls.Add(tbInsuranceSubType);

            HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(fourthDefaultTableRow);
            HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
            fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
            Label lblPolicyValue = new Label();
            lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
            lblPolicyValue.Text = "Премија";
            firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
            HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
            fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
            TextBox tbPolicyValue = new TextBox();
            tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
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

            //HtmlTableRow fifthDefaultTableRow = new HtmlTableRow();
            //defaultTable.Rows.Add(fifthDefaultTableRow);
            //HtmlTableCell firstCellFifthRowInDefaultTable = new HtmlTableCell();
            //fifthDefaultTableRow.Cells.Add(firstCellFifthRowInDefaultTable);
            //Label lblBrokeragePercentage = new Label();
            //lblBrokeragePercentage.ID = "lblBrokeragePercentage" + (j + 1).ToString();
            //lblBrokeragePercentage.Text = "Процент на брокеража";
            //firstCellFifthRowInDefaultTable.Controls.Add(lblBrokeragePercentage);
            //HtmlTableCell secondCellFifthRowInDefaultTable = new HtmlTableCell();
            //fifthDefaultTableRow.Cells.Add(secondCellFifthRowInDefaultTable);
            //TextBox tbBrokeragePercentage = new TextBox();
            //tbBrokeragePercentage.ID = "tbBrokeragePercentage" + (j + 1).ToString();
            //tbBrokeragePercentage.Text = Brokerage.GetByInsuranceSubTypeAndDeal(InsuranceSubtypeID, DealID).Percentage.ToString();
            //tbBrokeragePercentage.ReadOnly = true;
            //secondCellFifthRowInDefaultTable.Controls.Add(tbBrokeragePercentage);

            //HtmlTableRow sixthDefaultTableRow = new HtmlTableRow();
            //defaultTable.Rows.Add(sixthDefaultTableRow);
            //HtmlTableCell firstCellSixthRowInDefaultTable = new HtmlTableCell();
            //sixthDefaultTableRow.Cells.Add(firstCellSixthRowInDefaultTable);
            //Label lblBrokerageValue = new Label();
            //lblBrokerageValue.ID = "lblBrokerageValue" + (j + 1).ToString();
            //lblBrokerageValue.Text = "Брокеража";
            //firstCellSixthRowInDefaultTable.Controls.Add(lblBrokerageValue);
            //HtmlTableCell secondCellSixthRowInDefaultTable = new HtmlTableCell();
            //sixthDefaultTableRow.Cells.Add(secondCellSixthRowInDefaultTable);
            //TextBox tbBrokerageValue = new TextBox();
            //tbBrokerageValue.ID = "tbBrokerageValue" + (j + 1).ToString();
            //tbBrokerageValue.ReadOnly = true;
            //secondCellSixthRowInDefaultTable.Controls.Add(tbBrokerageValue);

            this.Controls.Add(defaultTable);


            HtmlTable table = new HtmlTable();

            foreach (Broker.DataAccess.Control c in listControls) {
                if (c.IsActive) {
                    HtmlTableRow tableRow = new HtmlTableRow();
                    table.Rows.Add(tableRow);
                    HtmlTableCell tableCell = new HtmlTableCell();
                    tableRow.Cells.Add(tableCell);
                    Label label = new Label();
                    label.ID = c.LabelID;
                    label.Text = c.LabelName;
                    lblFieldNames.Add(label);
                    tableCell.Controls.Add(label);
                    HtmlTableCell tableCellSecond = new HtmlTableCell();
                    tableRow.Cells.Add(tableCellSecond);
                    TextBox textbox = new TextBox();
                    textbox.ID = c.TextBoxID;
                    tbValues.Add(textbox);
                    tableCellSecond.Controls.Add(textbox);
                    if (c.HasRequredFieldValidator) {
                        RequiredFieldValidator rfv = new RequiredFieldValidator();
                        rfv.ID = "rfv" + textbox.ID;
                        rfv.ErrorMessage = "*";
                        rfv.Display = ValidatorDisplay.Dynamic;
                        rfv.ControlToValidate = textbox.ID;
                        tableCellSecond.Controls.Add(rfv);
                    }
                }
            }
            this.Controls.Add(table);



            DropDownList ddlNumberOfRates = new DropDownList();
            ddlNumberOfRates.ID = "ddlNumberOfRates" + "1";
            ddlNumberOfRates.AutoPostBack = true;
            for (int k = 1; k <= 12; k++) {
                ListItem li = new ListItem();
                li.Text = k.ToString();
                li.Value = k.ToString();
                ddlNumberOfRates.Items.Add(li);
            }
            ddlNumberOfRates.SelectedIndexChanged += new EventHandler(ddlNumberOfRates_SelectedIndexChanged);
            this.Controls.Add(ddlNumberOfRates);


        }



        Button btnInsert = new Button();
        btnInsert.ID = "btnInsert";
        btnInsert.Text = "Внеси";
        btnInsert.CausesValidation = true;
        btnInsert.EnableViewState = false;
        btnInsert.Click += new EventHandler(btnInsert_Click);
        this.Controls.Add(btnInsert);
        base.OnInit(e);
        Page.RegisterRequiresControlState(this);

    }

    //public bool IsPacket {
    //    get {
    //        RadioButtonList rbl = PoliciesDetailsView.FindControl("IsPacketRadioButtonList") as RadioButtonList;
    //        if (rbl == null) {
    //            return false;
    //        }
    //        return bool.Parse(rbl.SelectedValue);
    //        //  return _isPacket;

    //    }
    //    set {
    //        _isPacket = value;
    //    }
    //}

    void GridViewRates_SelectedIndexChanged(object sender, EventArgs e) {
        GridView GridViewRates = (GridView)sender;
        //selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        int gridViewIDIndex = int.Parse(GridViewRates.ID.Substring(GridViewRates.ID.Length - 1));
        int gridViewSelectedIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        TextBox tbValue = (TextBox)GridViewRates.SelectedRow.Cells[3].FindControl("tbValue");
        decimal value = Convert.ToDecimal(tbValue.Text);
        UpdateGridView(gridViewIDIndex, gridViewSelectedIndex, value);
    }

    void GridViewRates_RowCommand(object sender, GridViewCommandEventArgs e) {
        GridView GridViewRates = (GridView)sender;
        //selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        int gridViewIDIndex = int.Parse(GridViewRates.ID.Substring(GridViewRates.ID.Length - 1));
        //int gridViewSelectedIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        int gridViewSelectedIndex = Convert.ToInt32(e.CommandArgument);
        //TextBox tbValue = (TextBox)GridViewRates.SelectedRow.Cells[3].FindControl("tbValue");
        TextBox tbValue = (TextBox)GridViewRates.Rows[gridViewSelectedIndex].Cells[3].FindControl("tbValue");
        decimal value = Convert.ToDecimal(tbValue.Text);
        UpdateGridView(gridViewIDIndex, gridViewSelectedIndex, value);
    }

    void UpdateGridView(int gridViewIDIndex, int gridViewSelectedIndex, decimal value) {
        List<Rate> lRates = new List<Rate>();
        TextBox tbPolicyValue = (TextBox)this.FindControl("tbPolicyValue" + gridViewIDIndex.ToString());
        decimal totalPolicyCost = Convert.ToDecimal(tbPolicyValue.Text);
        if (totalPolicyCost>0) {

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
                //rate.PaymentTypeID = ActualRates[j].PaymentTypeID;
                ///Da se proveri
                rate.PolicyItemID = 0;
                lRates.Add(rate);
            }
        }
        GridView GridViewRates = (GridView)this.FindControl("GridViewRates" + gridViewIDIndex.ToString());
        GridViewRates.AutoGenerateColumns = false;
        GridViewRates.DataSource = lRates;
        GridViewRates.DataBind();
    }
   
}

//A customized class for displaying the Template Column

public class GridViewTemplate : ITemplate {

    //A variable to hold the type of ListItemType.

    ListItemType _templateType;



    //A variable to hold the column name.

    string _columnName;


    bool _isReadOnly;

    bool _isDropDownList;
    


    //Constructor where we define the template type and column name.

    public GridViewTemplate(ListItemType type, string colname, bool isReadOnly, bool isDropDownList) {

        //Stores the template type.

        _templateType = type;



        //Stores the column name.

        _columnName = colname;

        _isReadOnly = isReadOnly;

        _isDropDownList = isDropDownList;

    }



    void ITemplate.InstantiateIn(System.Web.UI.Control container) {


        switch (_templateType) {

            case ListItemType.Header:

                //Creates a new label control and add it to the container.

                Label lbl = new Label();            //Allocates the new label object.

                lbl.Text = _columnName;             //Assigns the name of the column in the lable.

                container.Controls.Add(lbl);        //Adds the newly created label control to the container.

                break;



            case ListItemType.Item:

                //Creates a new text box control and add it to the container.

                if (!_isDropDownList) {

                    TextBox tb1 = new TextBox();                            //Allocates the new text box object.

                    tb1.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.

                    tb1.Columns = 8;                                        //Creates a column with size 8.

                    tb1.ID = "tb" + _columnName;

                    if (_isReadOnly) {
                        tb1.ReadOnly = true;
                    }

                    container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
                
                } else {

                    DropDownList ddl1 = new DropDownList();
                    ddl1.ID = "ddlPaymentTypes";

                    ObjectDataSource ods1 = new ObjectDataSource();
                    ods1.ID = "ods1";
                    ods1.SelectMethod = "Select";
                    ods1.TypeName = "Broker.DataAccess.PaymentType";

                    ddl1.DataSourceID = "ods1";
                    ddl1.DataTextField = "Name";
                    ddl1.DataValueField = "ID";

                    ddl1.DataBinding+=new EventHandler(ddl1_DataBinding);

                    container.Controls.Add(ods1);
                    container.Controls.Add(ddl1);

                }

                break;



            case ListItemType.EditItem:

                //As, I am not using any EditItem, I didnot added any code here.

                break;



            case ListItemType.Footer:

                CheckBox chkColumn = new CheckBox();

                chkColumn.ID = "Chk" + _columnName;

                container.Controls.Add(chkColumn);

                break;

        }

    }



    /// <summary>

    /// This is the event, which will be raised when the binding happens.

    /// </summary>

    /// <param name="sender"></param>

    /// <param name="e"></param>

    void tb1_DataBinding(object sender, EventArgs e) {

        TextBox txtdata = (TextBox)sender;

        GridViewRow container = (GridViewRow)txtdata.NamingContainer;

        object dataValue = DataBinder.Eval(container.DataItem, _columnName);

        if (dataValue != DBNull.Value) {

            if (_columnName == "Date") {
                txtdata.Text = String.Format("{0:d}", dataValue);
            } else {
                txtdata.Text = dataValue.ToString();
            }

        }

    }


    void ddl1_DataBinding(object sender, EventArgs e) {
        DropDownList ddldata = (DropDownList)sender;
        GridViewRow container = (GridViewRow)ddldata.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, _columnName);

        if (dataValue != DBNull.Value) {

            ddldata.SelectedValue = dataValue.ToString();

        }

    }

}