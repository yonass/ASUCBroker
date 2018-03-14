using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Utility;
using Broker.DataAccess;
using System.Web.UI.HtmlControls;
using Broker.Controllers.RateController;
using Broker.Controllers.ReportControllers;

public partial class Broker_Packet : AuthenticationPage {
    public string Message {
        get {
            return _message;
        }
        set {
            _message = value;
        }
    }
    private string _message;
    private bool _isValidPolicyNumber;

    public bool IsValidPolicyNumber {
        get {
            return true;
        }
        set {
        }
    }





    public int PacketID {
        get {
            if (ViewState["PacketID"] != null) {
                return (int)ViewState["PacketID"];
            } else {
                if (Packet.Table.Count() == 0) {
                    return 0;
                }
                int max = 0;
                Packet maxPacket = Packet.Table.First();
                foreach (Packet p in Packet.Table) {
                    int current = p.PacketsInsuranceSubTypes.Count;
                    if (current > max) {
                        max = current;
                        maxPacket = p;
                    }
                }
                return maxPacket.ID;
            }
        }
        set {
            ViewState["PacketID"] = value;
        }
    }

    public int PolicyID {
        get {
            if (ViewState["PolicyID"] != null) {
                return (int)ViewState["PolicyID"];
            } else {
                return 0;
            }
        }
        set {
            ViewState["PolicyID"] = value;
        }
    }

    public int FactureID {
        get {
            if (ViewState["FactureID"] != null) {
                return (int)ViewState["FactureID"];
            } else {
                return 0;
            }
        }
        set {
            ViewState["FactureID"] = value;
        }
    }

    public Policy Policy {
        get {
            if (Session["Policy"] != null) {
                return (Policy)Session["Policy"];
            } else {
                return null;
            }
        }
        set {
            Session["Policy"] = value;
        }
    }

    public List<Rate> RatesInGridView {
        get {
            List<Rate> ActualRates = new List<Rate>();
            GridView GridViewRates;
            MultiView mv = mvMain;
            View activeView = mv.GetActiveView();
            GridViewRates = (GridView)activeView.FindControl("GridViewRates");
            foreach (GridViewRow row in GridViewRates.Rows) {
                Rate r = new Rate();
                TextBox tbNumber = (TextBox)row.Cells[0].FindControl("tbNumber");
                r.Number = Convert.ToInt32(tbNumber.Text);
                TextBox tbDate = (TextBox)row.Cells[1].FindControl("tbDate");
                r.Date = Convert.ToDateTime(tbDate.Text);
                TextBox tbvalue = (TextBox)row.Cells[2].FindControl("tbValue");
                r.Value = Convert.ToDecimal(tbvalue.Text);
                //TextBox tbBrokeragevalue = (TextBox)row.Cells[3].FindControl("tbBrokerageValue");
                //r.BrokerageValue = Convert.ToDecimal(tbBrokeragevalue.Text);
                r.BrokerageValue = 0;
                DropDownList ddlPaymentType = (DropDownList)row.Cells[3].FindControl("ddlPaymentTypes");
                //r.PaymentTypeID = Convert.ToInt32(ddlPaymentType.SelectedValue);
                r.PolicyItemID = 1;
                ActualRates.Add(r);
            }
            return ActualRates;
        }
    }

    public List<TextBox> tbValues;
    public List<Label> lblFieldNames;

    protected void Page_Load(object sender, EventArgs e) {
        if (!Page.IsPostBack) {
            mvMain.ActiveViewIndex = 0;
        }
        //ObjectDataSource odsStatus = PoliciesDetailsView.FindControl("odsStatus") as ObjectDataSource;
        //odsStatus.SelectParameters.Clear();
        //odsStatus.SelectParameters.Add("documentSubTypeID", DocumentSubType.GetByCode(DocumentSubType.POLISA).ID.ToString());

    }

    //public void SetBrokerage() {
    //    DropDownList ddlPacket = PoliciesDetailsView.FindControl("ddlPackets") as DropDownList;
    //    Packet p= Packet.Get(int.Parse(ddlPacket.SelectedValue));
    //    decimal brokeragePercentage  = p.
    //}

    void tbPolicyValue_TextChanged(object sender, EventArgs e) {
        TextBox tbPolicyValue = (TextBox)sender;
        int index = Convert.ToInt32(tbPolicyValue.ID.Substring(tbPolicyValue.ID.Length - 1));
        TextBox tbBrokeragePercentage = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokeragePercentage" + index.ToString());
        TextBox tbBrokerageValue = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokerageValue" + index.ToString());

        //tbBrokerageValue.Text = (Convert.ToDecimal(tbPolicyValue.Text) * Convert.ToDecimal(tbBrokeragePercentage.Text) / 100).ToString();

    }

    protected void Page_PreRender(object sender, EventArgs e) {
        //mvMain.ActiveViewIndex = 1;
    }

    protected override void OnPreInit(EventArgs e) {

        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e) {

        pnlEverything.Controls.Clear();

        List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();

        //int n = 1;
        //foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
        //    Button buttonForView = new Button();
        //    buttonForView.ID = "btnView" + n.ToString();
        //    buttonForView.Text = pist.InsuranceSubType.ShortDescription;
        //    buttonForView.CausesValidation = true;
        //    buttonForView.Click += new EventHandler(buttonForView_Click);
        //    pnlEverything.Controls.Add(buttonForView);
        //    n++;
        //}

        // CheckBox cb = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        pnlEverything.Controls.Add(pnlViewButtons);
        pnlViewButtons.Controls.Clear();
        pnlEverything.Controls.Add(pnlMainInformation);
        pnlMainInformation.Controls.Add(mvMain);
        mvMain.Views.Clear();

        int i = 1;
        foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
            View view = new View();
            view.ID = "view" + i.ToString();
            mvMain.Controls.Add(view);
            i++;
        }

        View view1 = new View();
        view1.ID = "view" + i.ToString();
        mvMain.Controls.Add(view1);

        base.OnInit(e);
    }

    void btnInsert_Click(object sender, EventArgs e) {
        List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();
        int i = 0;
        int j = 0;
        bool IsValid = true;
        string message = string.Empty;
        List<string> errorMessages = new List<string>();
        List<PolicyItem> listPolicyItems = new List<PolicyItem>();

        foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
            TextBox tbPolicyNumber = (TextBox)mvMain.FindControl("tbPolicyNumber" + (j + 1).ToString());
            message = Broker.DataAccess.Policy.ValidatePolicyNumberForPacket(tbPolicyNumber.Text, pist.Packet.InsuranceCompanyID, pist.InsuranceSubTypeID);
            ++j;
            if (message != string.Empty) {
                errorMessages.Add(message);
                IsValid = false;
            }
        }
        CheckBox cbIsPaidInBrokersHouse = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        decimal cashValue = 0;
        decimal cardValue = 0;

        // da se proveri dali e vo red sumata
        if (cbIsPaidInBrokersHouse.Checked) {
            decimal total = 0;
            for (int y = 0; y < listPacketInsuranceSubTypes.Count; y++) {
                TextBox tbValue = (TextBox)mvMain.FindControl("tbPolicyValue" + (y + 1).ToString());
                total += decimal.Parse(tbValue.Text);
            }
            TextBox tbCashValue = mvMain.FindControl("tbCash") as TextBox;
            TextBox tbCardValue = mvMain.FindControl("tbCreditCard") as TextBox;

            if (tbCardValue.Text.Trim() != string.Empty) {
                cardValue = decimal.Parse(tbCardValue.Text);
            }
            if (tbCashValue.Text.Trim() != string.Empty) {
                cashValue = decimal.Parse(tbCashValue.Text);
            }
            if (total != cashValue + cardValue) {
                errorMessages.Add("Внесените вредности за плаќањето не одговараат на вредностите на премиите на полисите!");
                IsValid = false;
            }
        }

        // Treba da bidi (valid)
        if (IsValid) {

            Policy p = (Policy)Policy;
            p.InsertDate = DateTime.Now;
            p.Insert();
            PolicyID = p.ID;

            foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
                PolicyItem policyItem = new PolicyItem();
                //TextBox tbBrokeragePercentage = (TextBox)mvMain.FindControl("tbBrokeragePercentage" + (i + 1).ToString());
                //policyItem.BrokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
                policyItem.BrokeragePercentage = 0;
                TextBox tbPolicyNumber = (TextBox)mvMain.FindControl("tbPolicyNumber" + (i + 1).ToString());
                policyItem.PolicyNumber = tbPolicyNumber.Text;
                //TextBox tbBrokerageValue = (TextBox)mvMain.FindControl("tbBrokerageValue" + (i + 1).ToString());
                //policyItem.BrokerageValue = Convert.ToDecimal(tbBrokerageValue.Text);
                policyItem.BrokerageValue = 0;
                policyItem.InsuranceSubTypeID = pist.InsuranceSubTypeID;
                policyItem.PolicyID = p.ID;
                TextBox tbPolicyValue = (TextBox)mvMain.FindControl("tbPolicyValue" + (i + 1).ToString());
                policyItem.PremiumValue = Convert.ToDecimal(tbPolicyValue.Text);
                DropDownList ddlStatuses = (DropDownList)mvMain.FindControl("ddlStatuses" + (i + 1).ToString());
                policyItem.StatusID = Convert.ToInt32(ddlStatuses.SelectedValue);
                policyItem.IsPaidInBrokerHouse = cbIsPaidInBrokersHouse.Checked;
                policyItem.Insert();
                p.TotalSum += policyItem.PremiumValue;
                listPolicyItems.Add(policyItem);

                Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(pist.InsuranceSubTypeID, tbPolicyNumber.Text, pist.Packet.InsuranceCompanyID);
                d.IsUsed = true;
                Distribution.Table.Context.SubmitChanges();

                List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetActiveByInsuranceSubType(pist.InsuranceSubTypeID);
                foreach (Broker.DataAccess.Control c in listControls) {
                    PolicyExtendInformation pei = new PolicyExtendInformation();
                    pei.ControlID = c.ID;
                    pei.PolicyItemID = policyItem.ID;
                    if (c.FieldType.Name == FieldType.TEXTBOX) {
                        TextBox tbControl = (TextBox)mvMain.FindControl(c.TextBoxID + i.ToString());
                        pei.Value = tbControl.Text;
                    }
                    if (c.FieldType.Name == FieldType.DROPDOWNLIST) {
                        DropDownList tbControl = (DropDownList)mvMain.FindControl(c.TextBoxID + i.ToString());
                        pei.Value = tbControl.SelectedItem.Text;
                        pei.ValueID = int.Parse(tbControl.SelectedValue);
                    }
                    pei.Insert();
                }
                i++;
            }

            List<Rate> currentRates = new List<Rate>();
            List<Rate> RatesForInsert = new List<Rate>();


            if (!cbIsPaidInBrokersHouse.Checked) {
                currentRates = RatesInGridView;
                decimal totalPolicyCost = 0;
                List<decimal> brokeragePercentages = new List<decimal>();
                List<decimal> policyItemValues = new List<decimal>();
                List<decimal> policyItemRatios = new List<decimal>();
                decimal brokerageValue = 0;
                for (int g = 1; g <= mvMain.Views.Count - 1; g++) {
                    TextBox tbPolicyValueG = (TextBox)mvMain.FindControl("tbPolicyValue" + g.ToString());
                    totalPolicyCost += Convert.ToDecimal(tbPolicyValueG.Text);
                    policyItemValues.Add(Convert.ToDecimal(tbPolicyValueG.Text));
                    //TextBox tbBrokeragePercentageG = (TextBox)mvMain.FindControl("tbBrokeragePercentage" + g.ToString());
                    //brokeragePercentages.Add(Convert.ToDecimal(tbBrokeragePercentageG.Text));
                    brokeragePercentages.Add(0);
                    //TextBox tbBrokerageValueG = (TextBox)mvMain.FindControl("tbBrokerageValue" + g.ToString());
                    //brokerageValue += Convert.ToDecimal(tbBrokerageValueG.Text);
                    brokerageValue += 0;
                }
                for (int h = 1; h <= mvMain.Views.Count - 1; h++) {
                    policyItemRatios.Add(policyItemValues[h - 1] / totalPolicyCost);
                }

                int w = 0;
                foreach (PolicyItem pi in listPolicyItems) {
                    foreach (Rate rate in currentRates) {
                        Rate r = new Rate();
                        r.Date = rate.Date;
                        r.Number = rate.Number;
                        //r.PaymentTypeID = rate.PaymentTypeID;
                        r.PolicyItemID = pi.ID;
                        r.Value = rate.Value * policyItemRatios[w];
                        r.BrokerageValue = rate.BrokerageValue * policyItemRatios[w];
                        RatesForInsert.Add(r);
                        r.Insert();
                    }
                    w++;
                }
                decimal brokerageSum = 0;
                foreach (PolicyItem pi in listPolicyItems) {
                    //p.PolicyItems.Add(pi);
                    brokerageSum += pi.BrokerageValue;
                }
                Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GenerateForSinglePolicy(p);
                f.BrokerageValue = brokerageSum;
                Broker.DataAccess.Facture.Insert(f);
                List<FactureItem> lFI = Broker.DataAccess.Facture.GenerateFactureItemsForSinglePolicy(listPolicyItems, f);
                //Facture.InsertFactureAndFactureItems(f, lFI);
                Broker.DataAccess.Facture.InsertFactureItems(lFI);
                FactureID = f.ID;
                PolicyItemFactureItem.InsertForFacture(f.FactureItems.ToList(), p.PolicyItems.ToList());
                mvPacket.SetActiveView(FacturePrintingView);
            } else {
                mvPacket.SetActiveView(BillView);
            }
        } else {
            Label lblPolicyNumberValidator = mvMain.Views[mvMain.Views.Count - 1].FindControl("lblPolicyNumberValidator") as Label;
            //  lblPolicyNumber.ID = "lblPolicyNumberValidator";
            lblPolicyNumberValidator.Visible = true;
            string errorMessage = string.Empty;
            foreach (string err in errorMessages) {
                errorMessage += err + " ";
            }
            lblPolicyNumberValidator.Text = errorMessage;
        }
    }


    void GridViewRates_RowCommand(object sender, GridViewCommandEventArgs e) {
        GridView GridViewRates = (GridView)sender;
        //selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        //int gridViewIDIndex = int.Parse(GridViewRates.ID.Substring(GridViewRates.ID.Length - 1));
        //int gridViewSelectedIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        int gridViewSelectedIndex = Convert.ToInt32(e.CommandArgument);
        //TextBox tbValue = (TextBox)GridViewRates.SelectedRow.Cells[3].FindControl("tbValue");
        TextBox tbValue = (TextBox)GridViewRates.Rows[gridViewSelectedIndex].Cells[2].FindControl("tbValue");
        decimal value = Convert.ToDecimal(tbValue.Text);
        //UpdateGridView(gridViewIDIndex, gridViewSelectedIndex, value);
        UpdateGridView(gridViewSelectedIndex, value);
    }

    void UpdateGridView(int gridViewSelectedIndex, decimal value) {
        List<Rate> lRates = new List<Rate>();
        decimal totalPolicyCost = 0;
        List<decimal> brokeragePercentages = new List<decimal>();
        List<decimal> policyItemValues = new List<decimal>();
        decimal brokerageValue = 0;
        for (int i = 1; i <= mvMain.Views.Count - 1; i++) {
            TextBox tbPolicyValue = (TextBox)mvMain.FindControl("tbPolicyValue" + i.ToString());
            totalPolicyCost += Convert.ToDecimal(tbPolicyValue.Text);
            policyItemValues.Add(totalPolicyCost);
            //TextBox tbBrokeragePercentage = (TextBox)mvMain.FindControl("tbBrokeragePercentage" + i.ToString());
            //brokeragePercentages.Add(Convert.ToDecimal(tbBrokeragePercentage.Text));
            brokeragePercentages.Add(0);
            //TextBox tbBrokerageValue = (TextBox)mvMain.FindControl("tbBrokerageValue" + i.ToString());
            //brokerageValue += Convert.ToDecimal(tbBrokerageValue.Text);
            brokerageValue += 0;
        }
        if (totalPolicyCost > 0) {

            decimal currentValue = value;
            //int curentNumber = GridViewRates.EditIndex;
            int curentNumber = gridViewSelectedIndex + 1;

            List<Rate> ActualRates = new List<Rate>();
            ActualRates = RatesInGridView;

            decimal currentSum = 0;
            decimal currentBrokerageValueSum = 0;
            for (int i = 0; i < curentNumber - 1; i++) {
                lRates.Add(ActualRates[i]);
                currentSum += ActualRates[i].Value;
                currentBrokerageValueSum += ActualRates[i].BrokerageValue;
            }

            if (currentValue > totalPolicyCost - currentSum) {
                currentValue = totalPolicyCost - currentSum;
            }

            Rate currentRate = new Rate();
            currentRate.Number = curentNumber;
            currentRate.Date = ActualRates[curentNumber - 1].Date;
            currentRate.Value = currentValue;
            decimal averageBrokeragePercentage = 0;
            foreach (decimal d in brokeragePercentages) {
                averageBrokeragePercentage += d;
            }
            averageBrokeragePercentage = averageBrokeragePercentage / (decimal)brokeragePercentages.Count;
            decimal ratio = currentValue / (totalPolicyCost - currentSum);
            decimal newBrokerageValue = 0;
            for (int u = 0; u < brokeragePercentages.Count; u++) {
                newBrokerageValue += ((brokeragePercentages[u] / 100) * ratio * policyItemValues[u]);
            }
            //currentRate.BrokerageValue = RateController.Scale5(brokerageValue - currentBrokerageValueSum) / RatesInGridView.Count;
            //currentRate.BrokerageValue = RateController.Scale5(currentValue / averageBrokeragePercentage);
            currentRate.BrokerageValue = newBrokerageValue;
            currentSum += currentValue;
            currentBrokerageValueSum += currentRate.BrokerageValue;
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
                rate.BrokerageValue = (brokerageValue - currentBrokerageValueSum) / (ActualRates.Count - curentNumber);
                //rate.PaymentTypeID = ActualRates[j].PaymentTypeID;
                ///Da se proveri
                rate.PolicyItemID = 0;
                lRates.Add(rate);
            }
        }
        GridView GridViewRates = (GridView)mvMain.FindControl("GridViewRates");
        GridViewRates.AutoGenerateColumns = false;
        GridViewRates.DataSource = lRates;
        GridViewRates.DataBind();
    }

    void GridViewRates_SelectedIndexChanged(object sender, EventArgs e) {
        GridView GridViewRates = (GridView)sender;
        //selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        //int gridViewIDIndex = int.Parse(GridViewRates.ID.Substring(GridViewRates.ID.Length - 1));
        int gridViewSelectedIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
        TextBox tbValue = (TextBox)GridViewRates.SelectedRow.Cells[2].FindControl("tbValue");
        decimal value = Convert.ToDecimal(tbValue.Text);
        UpdateGridView(gridViewSelectedIndex, value);
    }

    void buttonForView_Click(object sender, EventArgs e) {
        MultiView mv = mvMain;
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
        //OnInit(null);
        //pnlEverything.Controls.Clear();
        if (PacketID == 0) {
            return;
        }

        List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();

        int n = 1;
        foreach (PacketsInsuranceSubType pist in listPacketInsuranceSubTypes) {
            Button buttonForView = new Button();
            buttonForView.ID = "btnView" + n.ToString();
            buttonForView.Text = pist.InsuranceSubType.ShortDescription;
            buttonForView.CausesValidation = true;
            buttonForView.CssClass = "PacketButton";
            buttonForView.Click += new EventHandler(buttonForView_Click);
            //pnlEverything.Controls.Add(buttonForView);
            pnlViewButtons.Controls.Add(buttonForView);
            n++;
        }
        Button buttonForViewRates = new Button();
        buttonForViewRates.ID = "btnView" + n.ToString();
        buttonForViewRates.Text = "Рати";
        buttonForViewRates.CausesValidation = true;
        buttonForViewRates.CssClass = "PacketButton";
        buttonForViewRates.Click += new EventHandler(buttonForView_Click);
        //pnlEverything.Controls.Add(buttonForView);
        CheckBox cbIsPaidInBrokersHouse = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        if (cbIsPaidInBrokersHouse.Checked) {
            buttonForViewRates.Visible = false;
        }
        pnlViewButtons.Controls.Add(buttonForViewRates);

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

            HtmlTableRow seventhDefaultTableRow = new HtmlTableRow();
            defaultTable.Rows.Add(seventhDefaultTableRow);
            HtmlTableCell firstCellSeventhRowInDefaultTable = new HtmlTableCell();
            seventhDefaultTableRow.Cells.Add(firstCellSeventhRowInDefaultTable);
            Label lblStatus = new Label();
            lblStatus.ID = "lblStatus" + (j + 1).ToString();
            lblStatus.Text = "Статус";
            firstCellSeventhRowInDefaultTable.Controls.Add(lblStatus);
            HtmlTableCell secondCellSeventhRowInDefaultTable = new HtmlTableCell();
            seventhDefaultTableRow.Cells.Add(secondCellSeventhRowInDefaultTable);
            DropDownList ddlStatuses = new DropDownList();
            ddlStatuses.ID = "ddlStatuses" + (j + 1).ToString();
            ObjectDataSource odsStatuses = new ObjectDataSource();
            odsStatuses.ID = "odsStatuses" + (j + 1).ToString();
            ddlStatuses.DataTextField = "Description";
            ddlStatuses.DataValueField = "ID";
            odsStatuses.DataObjectTypeName = "Broker.DataAccess.Statuse";
            odsStatuses.TypeName = "Broker.DataAccess.Statuse";
            odsStatuses.SelectMethod = "GetActiveStatuses";
            ddlStatuses.DataSourceID = "odsStatuses" + (j + 1).ToString();
            secondCellSeventhRowInDefaultTable.Controls.Add(ddlStatuses);
            secondCellSeventhRowInDefaultTable.Controls.Add(odsStatuses);

            mvMain.Views[j].Controls.Clear();

            mvMain.Views[j].Controls.Add(defaultTable);


            HtmlTable table = new HtmlTable();

            foreach (Broker.DataAccess.Control c in listControls) {
                if (c.IsActive) {
                    HtmlTableRow tableRow = new HtmlTableRow();
                    table.Rows.Add(tableRow);
                    HtmlTableCell tableCell = new HtmlTableCell();
                    tableRow.Cells.Add(tableCell);
                    Label label = new Label();
                    label.ID = c.LabelID + j.ToString();
                    label.Text = c.LabelName;
                    lblFieldNames.Add(label);
                    tableCell.Controls.Add(label);
                    HtmlTableCell tableCellSecond = new HtmlTableCell();
                    tableRow.Cells.Add(tableCellSecond);
                    if (c.FieldType.Name == FieldType.TEXTBOX) {
                        TextBox textbox = new TextBox();
                        textbox.ID = c.TextBoxID + j.ToString();
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
                    if (c.FieldType.Name == FieldType.DROPDOWNLIST) {
                        DropDownList dropdownlist = new DropDownList();
                        dropdownlist.ID = c.TextBoxID + j.ToString();
                        dropdownlist.DataTextField = "Value";
                        dropdownlist.DataValueField = "ID";
                        List<ControlBindableValue> cbvList = ControlBindableValue.GetActiveByControlID(c.ID);
                        dropdownlist.DataSource = cbvList;
                        dropdownlist.DataBind();
                        tableCellSecond.Controls.Add(dropdownlist);
                    }
                }
            }
            mvMain.Views[j].Controls.Add(table);
            j++;
        }

        int m = listPacketInsuranceSubTypes.Count - 1;

        //    CheckBox cbIsPaidInBrokersHouse = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;

        if (!cbIsPaidInBrokersHouse.Checked) {
            m++;
            Label lblNumberOfRates = new Label();
            lblNumberOfRates.ID = "lblNumberOfRates";
            lblNumberOfRates.Text = "Број на рати";
            mvMain.Views[m].Controls.Add(lblNumberOfRates);

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
            mvMain.Views[m].Controls.Add(ddlNumberOfRates);

            Label lblStartDateForRates = new Label();
            lblStartDateForRates.ID = "lblStartDateForRates";
            lblStartDateForRates.Text = "Почетна дата за рати";
            mvMain.Views[m].Controls.Add(lblStartDateForRates);

            TextBox tbStartDateForRates = new TextBox();
            tbStartDateForRates.ID = "tbStartDateForRates";
            mvMain.Views[m].Controls.Add(tbStartDateForRates);

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
            //TemplateField tfBrokerageValue = new TemplateField();
            //tfBrokerageValue.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "BrokerageValue", true, false);
            //tfBrokerageValue.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Брокеража", true, false);
            //GridViewRates.Columns.Add(tfBrokerageValue);
            TemplateField tfPaymentType = new TemplateField();
            tfPaymentType.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "PaymentTypeID", false, true);
            tfPaymentType.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Тип на плаќање", false, true);
            GridViewRates.Columns.Add(tfPaymentType);
            mvMain.Views[m].Controls.Add(GridViewRates);

            Button buttonCalculate = new Button();
            buttonCalculate.ID = "btnCalculate";
            buttonCalculate.Text = "Пресметај";
            buttonCalculate.CausesValidation = false;
            buttonCalculate.Click += new EventHandler(buttonCalculate_Click);
            mvMain.Views[m].Controls.Add(buttonCalculate);

        }
        int e = mvMain.ActiveViewIndex;

        if (cbIsPaidInBrokersHouse.Checked) {
            HtmlTable tablePayment = new HtmlTable();
            HtmlTableRow tableRowPayment = new HtmlTableRow();
            tablePayment.Rows.Add(tableRowPayment);
            HtmlTableCell tableCellPayment = new HtmlTableCell();
            tableRowPayment.Cells.Add(tableCellPayment);
            Label labelCash = new Label();
            labelCash.ID = "lblCash";
            labelCash.Text = "Готовина";
            lblFieldNames.Add(labelCash);
            tableCellPayment.Controls.Add(labelCash);
            HtmlTableCell tableCellSecondPayment = new HtmlTableCell();
            tableRowPayment.Cells.Add(tableCellSecondPayment);
            TextBox textboxCash = new TextBox();
            textboxCash.ID = "tbCash";
            tbValues.Add(textboxCash);
            tableCellSecondPayment.Controls.Add(textboxCash);
            mvMain.Views[m].Controls.Add(tablePayment);

            HtmlTableRow tableRowCardPayment = new HtmlTableRow();
            tablePayment.Rows.Add(tableRowCardPayment);
            HtmlTableCell tableCellCardPayment = new HtmlTableCell();
            tableRowCardPayment.Cells.Add(tableCellCardPayment);
            Label labelCard = new Label();
            labelCard.ID = "lblCard";
            labelCard.Text = "Картичка";
            lblFieldNames.Add(labelCard);
            tableCellCardPayment.Controls.Add(labelCard);
            HtmlTableCell tableCellSecondCardPayment = new HtmlTableCell();
            tableRowCardPayment.Cells.Add(tableCellSecondCardPayment);
            TextBox textboxCard = new TextBox();
            textboxCard.ID = "tbCreditCard";
            tbValues.Add(textboxCard);
            tableCellSecondCardPayment.Controls.Add(textboxCard);

            HtmlTableRow tableRowCardType = new HtmlTableRow();
            tablePayment.Rows.Add(tableRowCardType);
            HtmlTableCell tableCellBank = new HtmlTableCell();
            tableRowCardType.Cells.Add(tableCellBank);
            Label labelBank = new Label();
            labelBank.ID = "lblBank";
            labelBank.Text = "Банка";
            lblFieldNames.Add(labelBank);
            tableCellBank.Controls.Add(labelBank);

            HtmlTableCell tableCellSecondBank = new HtmlTableCell();
            tableRowCardType.Cells.Add(tableCellSecondBank);
            DropDownList ddlBanks = new DropDownList();
            ddlBanks.ID = "ddlBanks";
            ddlBanks.DataValueField = "ID";
            ddlBanks.DataTextField = "Name";
            ddlBanks.DataSourceID = "odsBanks";

            ObjectDataSource odsBanks = new ObjectDataSource();
            odsBanks.ID = "odsBanks";
            odsBanks.SelectMethod = "GetBanksWithGreditCards";
            odsBanks.TypeName = "Broker.DataAccess.Bank";
            ddlBanks.AutoPostBack = true;
            ddlBanks.SelectedIndexChanged += new EventHandler(ddlBanks_SelectedIndexChanged);

            tableCellSecondBank.Controls.Add(odsBanks);
            tableCellSecondBank.Controls.Add(ddlBanks);
            ddlBanks.DataBind();


            HtmlTableRow tableSecondRowCardType = new HtmlTableRow();
            tablePayment.Rows.Add(tableSecondRowCardType);
            HtmlTableCell tableCellCardType = new HtmlTableCell();
            tableSecondRowCardType.Cells.Add(tableCellCardType);
            Label labelCardType = new Label();
            labelCardType.ID = "lblCardType";
            labelCardType.Text = "Тип на картичка";
            lblFieldNames.Add(labelCardType);
            tableCellCardType.Controls.Add(labelCardType);

            HtmlTableCell tableCellSecondCardType = new HtmlTableCell();
            tableSecondRowCardType.Cells.Add(tableCellSecondCardType);
            DropDownList ddlCreditCardTypes = new DropDownList();
            ddlCreditCardTypes.ID = "ddlCreditCardTypes";
            ddlCreditCardTypes.DataValueField = "ID";
            ddlCreditCardTypes.DataTextField = "Name";
            ddlCreditCardTypes.DataSourceID = "odsCreditCardTypes";

            ObjectDataSource odsCreditCardTypes = new ObjectDataSource();
            odsCreditCardTypes.ID = "odsCreditCardTypes";
            odsCreditCardTypes.SelectMethod = "GetByBank";
            odsCreditCardTypes.TypeName = "Broker.DataAccess.CreditCard";
            odsCreditCardTypes.SelectParameters.Clear();
            odsCreditCardTypes.SelectParameters.Add("BankID", ddlBanks.SelectedValue);

            tableCellSecondCardType.Controls.Add(odsCreditCardTypes);
            tableCellSecondCardType.Controls.Add(ddlCreditCardTypes);
            mvMain.Views[m].Controls.Add(tablePayment);
            ddlCreditCardTypes.DataBind();
        }


        Button btnInsert = new Button();
        btnInsert.ID = "btnInsert";
        btnInsert.Text = "Внеси";
        btnInsert.CausesValidation = true;
        btnInsert.Click += new EventHandler(btnInsert_Click);
        mvMain.Views[m].Controls.Add(btnInsert);







        Label lblPolicyNumberValidator = new Label();
        lblPolicyNumberValidator.ID = "lblPolicyNumberValidator";
        lblPolicyNumberValidator.Visible = false;
        mvMain.Views[m].Controls.Add(lblPolicyNumberValidator);


        base.CreateChildControls();

        for (int w = j; w > 0; w--) {
            DropDownList ddlStatuses = (DropDownList)mvMain.FindControl("ddlStatuses" + w.ToString());
            ddlStatuses.DataBind();
        }

    }

    void buttonCalculate_Click(object sender, EventArgs e) {

        List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();
        int count = listPacketInsuranceSubTypes.Count; 

        List<Rate> lRates = new List<Rate>();
        decimal totalPolicyCost = 0;
        List<decimal> brokeragePercentages = new List<decimal>();
        List<decimal> policyItemValues = new List<decimal>();
        List<decimal> policyItemRatios = new List<decimal>();
        decimal brokerageValue = 0;
        //for (int i = 1; i <= mvMain.Views.Count - 2; i++) {
        for (int i=1; i<=count; i++){
            TextBox tbPolicyValue = (TextBox)mvMain.FindControl("tbPolicyValue" + i.ToString());
            totalPolicyCost += Convert.ToDecimal(tbPolicyValue.Text);
            policyItemValues.Add(Convert.ToDecimal(tbPolicyValue.Text));
            TextBox tbBrokeragePercentage = (TextBox)mvMain.FindControl("tbBrokeragePercentage" + i.ToString());
            //brokeragePercentages.Add(Convert.ToDecimal(tbBrokeragePercentage.Text));
            brokeragePercentages.Add(0);
            TextBox tbBrokerageValue = (TextBox)mvMain.FindControl("tbBrokerageValue" + i.ToString());
            //brokerageValue += Convert.ToDecimal(tbBrokerageValue.Text);
            brokerageValue += 0;
        }
        //for (int j = 1; j <= mvMain.Views.Count - 2; j++) {
        for (int j = 1; j <= count; j++) {
            policyItemRatios.Add(policyItemValues[j - 1] / totalPolicyCost);
        }

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

        GridView GridViewRates = (GridView)mvMain.FindControl("GridViewRates");
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

    void ddlNumberOfRates_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlNumberOfRates = (DropDownList)sender;
        List<Rate> lRates = new List<Rate>();
        //TextBox tbPolicyNumber = (TextBox)mvMain.FindControl("tbPolicyNumber" + ddlNumberOfRates.ID.Substring(ddlNumberOfRates.ID.Length - 1));
        //string policyNumber = tbPolicyNumber.Text;
        decimal policyValue = 0;
        decimal brokerageValue = 0;
        List<PacketsInsuranceSubType> listPacketInsuranceSubTypes = PacketsInsuranceSubType.Table.Where(pi => pi.PacketID == PacketID).ToList();
        for (int k = 0; k < listPacketInsuranceSubTypes.Count; k++) {
            //  ControlCollection cc = mvMain.Controls;
            View view = (View)mvMain.FindControl("view" + (k + 1).ToString());
            TextBox tbPolicyValue = (TextBox)view.FindControl("tbPolicyValue" + (k + 1).ToString());
            policyValue += decimal.Parse(tbPolicyValue.Text);
            //TextBox tbBrokerageValue = (TextBox)view.FindControl("tbBrokerageValue" + (k + 1).ToString());
            //brokerageValue += Convert.ToDecimal(tbBrokerageValue.Text);
            brokerageValue += 0;
        }

        Policy pol = (Policy)Policy;

        //DateTime startDate = policy.ApplicationDate;
        TextBox tbStartDateForRates = mvMain.Views[mvMain.Views.Count - 1].FindControl("tbStartDateForRates") as TextBox;
        DateTime startDate = new DateTime();
        DateTime.TryParse(tbStartDateForRates.Text, out startDate);


        if (startDate == DateTime.MinValue) {
            startDate = DateTime.Today;
        }
        decimal totalPolicyCost = policyValue;

        int ratesNumber = Convert.ToInt32(ddlNumberOfRates.SelectedValue);
        for (int i = 0; i < ratesNumber; i++) {
            Rate r = new Rate();
            r.Number = i + 1;
            r.Date = startDate.AddMonths(i);
            r.Value = RateController.Scale5(totalPolicyCost / ratesNumber);
            //TextBox tbBrokeragePercentage = (TextBox)mvMain.FindControl("tbBrokeragePercentage" + ddlNumberOfRates.ID.Substring(ddlNumberOfRates.ID.Length - 1).ToString());
            //decimal brokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
            r.BrokerageValue = RateController.Scale5(brokerageValue / ratesNumber);
            ///Ova da se proveri!!!
            r.PolicyItemID = 0;
            //r.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
            lRates.Add(r);
        }

        GridView GridViewRates = (GridView)pnlEverything.FindControl("GridViewRates");

        GridViewRates.AutoGenerateColumns = false;
        GridViewRates.DataSource = lRates;
        GridViewRates.DataBind();

        Dictionary<int, decimal> myDictionary = new Dictionary<int, decimal>();
        foreach (Rate r in lRates) {
            myDictionary.Add(r.Number, r.Value);
        }
        ViewState["MyDictionary"] = myDictionary;

    }

    protected void btnClientEMBGSearch_Click(object sender, EventArgs e) {
        TextBox tbClientEmbg = PoliciesDetailsView.FindControl("tbClientEMBG") as TextBox;
        string embg = tbClientEmbg.Text;
        Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(embg);
        Panel pnlClient = PoliciesDetailsView.FindControl("pnlClient") as Panel;
        if (c != null) {
            pnlClient.Visible = false;
            TextBox tbClientName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
            ViewState["ClientID"] = c.ID;
            tbClientName.Text = c.Name;
        } else {
            pnlClient.Visible = true;
            DetailsView ClientDetailsView = pnlClient.FindControl("ClientDetailsView") as DetailsView;
            TextBox tbEBMG = ClientDetailsView.FindControl("tbEMBG") as TextBox;
            tbEBMG.ReadOnly = true;
            tbEBMG.Text = embg;
        }
        OnInit(e);
        CreateChildControls();
    }
    protected void ddlInsuranceCompanyIndexChanged(object sender, EventArgs e) {
        DropDownList ddlPackets = PoliciesDetailsView.FindControl("ddlPackets") as DropDownList;
        FakeBinder.SimulateBindingContext(this.Page, ddlPackets);
        ddlPackets.DataBind();
    }
    protected void btnOwnerEMBGSearch_Click(object sender, EventArgs e) {
        TextBox tbEmbg = PoliciesDetailsView.FindControl("tbOwnerEMBG") as TextBox;
        Broker.DataAccess.Client o = Broker.DataAccess.Client.GetByEmbg(tbEmbg.Text);
        Panel ownerPanel = PoliciesDetailsView.FindControl("pnlOwner") as Panel;
        if (o != null) {
            ownerPanel.Visible = false;
            ViewState["OwnerID"] = o.ID;
            TextBox tbName = PoliciesDetailsView.FindControl("tbOwnerName") as TextBox;
            tbName.Text = o.Name;
        } else {
            ownerPanel.Visible = true;
            DetailsView OwnerDetailsView = ownerPanel.FindControl("OwnerDetailsView") as DetailsView;
            TextBox tbOwnerEMBG = OwnerDetailsView.FindControl("tbEMBG") as TextBox;
            tbOwnerEMBG.Text = tbEmbg.Text;
            tbOwnerEMBG.ReadOnly = true;
        }
        OnInit(e);
        CreateChildControls();
    }
    protected void ClientDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvPacket.SetActiveView(viewMainInformations);
        }
    }

    protected void ClientDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {

    }
    protected void ClientDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {

            string embg = e.Values["EMBG"].ToString();
            Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(embg);
            ViewState["ClientID"] = c.ID;
            TextBox tbName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
            tbName.Text = c.Name;
            Panel pnl = PoliciesDetailsView.FindControl("pnlClient") as Panel;
            pnl.Visible = false;

        }
        OnInit(e);
        CreateChildControls();
    }
    protected void ClientdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void ClientdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        c.ValidateClient();
        

    }
    protected void OwnerDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvPacket.SetActiveView(viewMainInformations);
        }
    }
    protected void OwnerDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {

    }
    protected void OwnerDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            string embg = e.Values["EMBG"].ToString();
            Broker.DataAccess.Client o = Broker.DataAccess.Client.GetByEmbg(embg);
            ViewState["OwnerID"] = o.ID;
            TextBox tbName = PoliciesDetailsView.FindControl("tbOwnerName") as TextBox;
            tbName.Text = o.Name;
            Panel pnl = PoliciesDetailsView.FindControl("pnlOwner") as Panel;
            pnl.Visible = false;
        }
        OnInit(e);
        CreateChildControls();
    }
    protected void OwnerDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void ClientDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void OwnerdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
        

    }
    protected void OwnerdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        c.ValidateOwner();
       
    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Policy p = e.InputParameters["p"] as Policy;
        p.StatusID = Statuse.GetByCode(Statuse.ZAVRSENA_POLISA).ID;
       // p.ApplicationDate = DateTime.Today;
        p.BranchID = this.PageUser.BranchID;
        p.ClientID = (int)ViewState["ClientID"];
        p.OwnerID = (int)ViewState["OwnerID"];
        p.UserID = this.PageUser.ID;
        int i = p.ID;
        DropDownList ddlPackets = PoliciesDetailsView.FindControl("ddlPackets") as DropDownList;
        if (ddlPackets.SelectedIndex != -1) {
            PacketID = Convert.ToInt32(ddlPackets.SelectedValue);
        } else {
            return;
        }
    }

    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
        Policy p = (Policy)e.ReturnValue;
        Policy = p;
        //PolicyID = polID;
        DropDownList ddlPackets = PoliciesDetailsView.FindControl("ddlPackets") as DropDownList;
        if (ddlPackets.SelectedIndex != -1) {
            PacketID = Convert.ToInt32(ddlPackets.SelectedValue);
            OnInit(e);
            CreateChildControls();
            mvPacket.SetActiveView(viewAdditionalInformations);
        }
    }
    protected void ddlPackets_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlPackets = (DropDownList)sender;
        PacketID = Convert.ToInt32(ddlPackets.SelectedValue);
        OnInit(e);
        CreateChildControls();
    }
    protected void Checked_Changed(object sender, EventArgs e) {
        OnInit(e);
        CreateChildControls();
    }
    //protected void NextStep_Click(object sender, EventArgs e) {
    //    ObjectDataSource odc = dvDataSource;
    //    ObjectDataSourceMethodEventArgs ee = odc.o

    //}
    protected void btnMainInformations_Click(object sender, EventArgs e) {
        mvPacket.SetActiveView(viewMainInformations);
        btnAdditionalInformations.CssClass = "PacketButton";
        btnMainInformations.CssClass = "PacketButton_Active";
    }
    protected void btnAdditionalInformations_Click(object sender, EventArgs e) {
        DropDownList ddlPackets = PoliciesDetailsView.FindControl("ddlPackets") as DropDownList;
        if (ddlPackets.SelectedIndex != -1) {
            mvPacket.SetActiveView(viewAdditionalInformations);
            btnAdditionalInformations.CssClass = "PacketButton_Active";
            btnMainInformations.CssClass = "PacketButton";
            Policy newPolicy = new Policy();
            TextBox tbPolicyNumber = PoliciesDetailsView.FindControl("tbPolicyNumber") as TextBox;
            newPolicy.PolicyNumber = tbPolicyNumber.Text;
            DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;
            newPolicy.InsuranceCompanyID = Convert.ToInt32(ddlInsuranceCompany.SelectedValue);
            // DropDownList ddlPackets = PoliciesDetailsView.FindControl("ddlPackets") as DropDownList;
            newPolicy.PacketID = Convert.ToInt32(ddlPackets.SelectedValue);
            DropDownList ddlStatus = PoliciesDetailsView.FindControl("ddlStatus") as DropDownList;
            newPolicy.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            newPolicy.ClientID = (int)ViewState["ClientID"];
            newPolicy.OwnerID = (int)ViewState["OwnerID"];
            newPolicy.BranchID = this.PageBranch.ID;
            TextBox tbApplicationDate = PoliciesDetailsView.FindControl("tbApplicationDate") as TextBox; ;
            newPolicy.ApplicationDate = Convert.ToDateTime(tbApplicationDate.Text);
            TextBox tbStartDate = PoliciesDetailsView.FindControl("tbStartDate") as TextBox; ;
            newPolicy.StartDate = Convert.ToDateTime(tbStartDate.Text);
            TextBox tbEndDate = PoliciesDetailsView.FindControl("tbEndDate") as TextBox;
            newPolicy.EndDate = Convert.ToDateTime(tbEndDate.Text);
            newPolicy.UserID = this.PageUser.ID;
            Policy = newPolicy;
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

    protected void btnPrintFacture_Click(object sender, EventArgs e) {
        if (FactureID > 0) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(FactureID);
            PrintFactures.PrintFacture(f);
        }
    }

    protected void btnInsertNewPayment_Click(object sender, EventArgs e) {
        decimal cashValue = 0;
        decimal cardValue = 0;
        decimal totalValue = 0;
        if (tbCashPayment.Text.Trim() != string.Empty) {
            cashValue = decimal.Parse(tbCashPayment.Text);
        }
        if (tbCreditCardPayment.Text.Trim() != string.Empty) {
            cardValue = decimal.Parse(tbCreditCardPayment.Text);
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
        //        if (tbCashPayment.Text.Trim() != string.Empty) {
        //            CashPaymentItem cpi = new CashPaymentItem();
        //            cpi.CashPaymentID = cp.ID;
        //            cpi.PaymentTypeID = PaymentType.GetByCode(PaymentType.CASH).ID;
        //            cpi.Value = cashValue;
        //            cpi.Insert();
        //        }
        //        if (tbCreditCardPayment.Text.Trim() != string.Empty) {
        //            DropDownList ddlCreditCards = mvMain.FindControl("ddlCreditCardTypes") as DropDownList;
        //            DropDownList ddlBanks = mvMain.FindControl("ddlBanks") as DropDownList;



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

    void ddlBanks_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlCreditCards = mvMain.FindControl("ddlCreditCardTypes") as DropDownList;
        DropDownList ddlBank = (DropDownList)sender;
        ObjectDataSource odsCreditCardTypes = mvMain.FindControl("odsCreditCardTypes") as ObjectDataSource;
        odsCreditCardTypes.SelectParameters.Clear();
        odsCreditCardTypes.SelectParameters.Add("BankID", ddlBank.SelectedValue);


    }


    
}

