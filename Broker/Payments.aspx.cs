using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.RateController;

public partial class Broker_Payments : AuthenticationPage {
    public List<Rate> RatesInGridView {
        get {
            List<Rate> ActualRates = new List<Rate>();

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


    public int selIndex {
        get {
            if (ViewState["selIndex"] != null) {
                return (int)ViewState["selIndex"];
            } else {
                return 0;
            }
        }
        set {
            ViewState["selIndex"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e) {
        if (!Page.IsPostBack) {
            tbDateOfPayment.Text = DateTime.Today.ToShortDateString();
        }
    }
    void UpdateTextBoxes(PolicyItem pi) {
        tbPolicyPremiumCost.Text = pi.PremiumValue.ToString();
        tbPolicyTotalPaidValue.Text = Rate.PaidRatesValue(pi.ID).ToString();
        tbPolicyForPaidValue.Text = (Convert.ToDecimal(tbPolicyPremiumCost.Text) - Convert.ToDecimal(tbPolicyTotalPaidValue.Text)).ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e) {
        string policyNumber = tbPolicyNumber.Text;
        int insuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
        try {
            PolicyItem pi = PolicyItem.GetByNumberAndInsuranceCompany(policyNumber, insuranceCompanyID);

            if (pi != null) {
                UpdateTextBoxes(pi);
                lblFeedback.Text = string.Empty;
                GridViewRates.DataBind();
                btnGenerate.Enabled = true;
            } else {
                lblFeedback.Text = "Не е пронајдена полиса!";
            }
            odsPaidPayments.SelectParameters.Clear();
            odsPaidPayments.SelectParameters.Add("policyItemID", pi.ID.ToString());
            GridViewPayments.DataBind();
        } catch (Exception ex) {
            pnlInsurancecSubType.Visible = true;
            ddlInsuranceSubTypes.DataBind();
            lblFeedback.Text = "Изберете полиса!";
            btnGenerate.Enabled = false;
            btnInsert.Enabled = false;
            // GridViewRates.DataBind();
            // GridViewPayments.DataBind();
        }
    }

    protected void tbValue_TextChanged(object sender, EventArgs e) {

    }

    protected void GridViewRates_SelectedIndexChanged(object sender, EventArgs e) {
        selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
    }

    protected void GridViewRates_RowCommand(object sender, GridViewCommandEventArgs e) {
        selIndex = Convert.ToInt32(GridViewRates.SelectedIndex);
    }

    protected void btnInsert_Click(object sender, EventArgs e) {
        DateTime inputDate = Convert.ToDateTime(tbDateOfPayment.Text);
        int insuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
        PolicyItem pi;
        if (ddlInsuranceSubTypes.Items.Count > 0) {
            pi = PolicyItem.GetByNumberAndInsuranceSubType(tbPolicyNumber.Text, Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue), insuranceCompanyID);
        } else {
            pi = PolicyItem.GetByNumber(tbPolicyNumber.Text, insuranceCompanyID);
        }
        if (inputDate.Date > DateTime.Today) {
            lblFeedback.Text = "Не е можно внесување на датуми поголеми од денешниот";
        } else {
            List<Payment> listPayments = Payment.GetByPolicyItemID(pi.ID);
            decimal paymentTotalValue = 0;
            foreach (Payment payment in listPayments) {
                paymentTotalValue += payment.Value;
            }
            if (Convert.ToDecimal(tbValueOfPayment.Text) > (pi.PremiumValue - paymentTotalValue)) {
                lblFeedback.Text = "Поголем износ од преостанатиот износ за плаќање";
            } else {
                decimal valueFromClient = Convert.ToDecimal(tbValueOfPayment.Text);
                while (valueFromClient > 0) {
                    Rate currentRate = Rate.GetCurrentRateForPayment(pi.ID);
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
                        newPayment.Value = currentRate.Value - Payment.GetPaidValueForRate(currentRate.ID);
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
                        currentRate.PaidValue += newPayment.Value;
                        Rate.Table.Context.SubmitChanges();
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
                            currentRate.PaidValue += valueFromClient;
                            Rate.Table.Context.SubmitChanges();
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
                            currentRate.PaidValue += newPayment.Value;
                            Rate.Table.Context.SubmitChanges();
                            valueFromClient -= newPayment.Value;
                        }
                    }
                }
                Broker.DataAccess.Facture.UpdatePaidStatusForFacture(pi.ID);
            }
        }
        GridViewPayments.DataSource = null;
        GridViewPayments.DataSourceID = odsPaidPayments.ID;
        odsPaidPayments.SelectParameters.Clear();
        odsPaidPayments.SelectParameters.Add("policyItemID", pi.ID.ToString());
        GridViewPayments.DataBind();

        GridViewRates.DataSource = null;
        GridViewRates.DataSourceID = odsRates.ID;

        odsRates.SelectParameters.Clear();
        odsRates.TypeName = "Broker.DataAccess.Rate";
        odsRates.DataObjectTypeName = "Broker.DataAccess.Rate";
        odsRates.SelectMethod = "GetByPolicyNumberAndInsuranceSubTypeIDAndInsuranceCompanyID";
        odsRates.SelectParameters.Add("policyNumber", tbPolicyNumber.Text);
        odsRates.SelectParameters.Add("insuranceSubTypeID", pi.InsuranceSubType.ID.ToString());
        odsRates.SelectParameters.Add("insuranceCompanyID", ddlInsuranceCompany.SelectedValue);
        //odsRates.SelectMethod = "Broker.DataAccess.PolicyItem.GetByNumberAndInsuranceSubType";
        GridViewRates.DataBind();
        UpdateTextBoxes(pi);
    }
    protected void ddlInsuranceSubTypes_SelectedIndexChanged(object sender, EventArgs e) {
        odsRates.SelectParameters.Clear();
        odsRates.TypeName = "Broker.DataAccess.Rate";
        odsRates.DataObjectTypeName = "Broker.DataAccess.Rate";
        odsRates.SelectMethod = "GetByPolicyNumberAndInsuranceSubTypeIDAndInsuranceCompanyID";
        odsRates.SelectParameters.Add("policyNumber", tbPolicyNumber.Text);
        odsRates.SelectParameters.Add("insuranceSubTypeID", Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue).ToString());
        odsRates.SelectParameters.Add("insuranceCompanyID", Convert.ToInt32(ddlInsuranceCompany.SelectedValue).ToString());
        GridViewRates.DataBind();
        PolicyItem pi = PolicyItem.GetByNumberAndInsuranceSubType(tbPolicyNumber.Text, Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue), int.Parse(ddlInsuranceCompany.SelectedValue));
        odsPaidPayments.SelectParameters.Clear();
        odsPaidPayments.SelectParameters.Add("policyItemID", pi.ID.ToString());
        GridViewPayments.DataBind();
        UpdateTextBoxes(pi);
    }
    protected void btnGenerate_Click(object sender, EventArgs e) {
        DateTime inputDate = Convert.ToDateTime(tbDateOfPayment.Text);
        PolicyItem pi;
        List<Payment> newPayments = new List<Payment>();
        if (ddlInsuranceSubTypes.Items.Count > 0) {
            pi = PolicyItem.GetByNumberAndInsuranceSubType(tbPolicyNumber.Text, Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue), int.Parse(ddlInsuranceCompany.SelectedValue));
        } else {
            pi = PolicyItem.GetByNumber(tbPolicyNumber.Text, int.Parse(ddlInsuranceCompany.SelectedValue));
        }
        if (inputDate.Date > DateTime.Today) {
            lblFeedback.Text = "Не е можно внесување на датуми поголеми од денешниот";
        } else {
            lblFeedback.Text = string.Empty;
            List<Payment> listPayments = Payment.GetByPolicyItemID(pi.ID);
            newPayments.AddRange(listPayments);
            decimal paymentTotalValue = 0;
            foreach (Payment payment in listPayments) {
                paymentTotalValue += payment.Value;
            }
            if (Convert.ToDecimal(tbValueOfPayment.Text) > (pi.PremiumValue - paymentTotalValue)) {
                lblFeedback.Text = "Поголем износ од преостанатиот износ за плаќање";
            } else {
                lblFeedback.Text = string.Empty;
                decimal valueFromClient = Convert.ToDecimal(tbValueOfPayment.Text);
                Broker.DataAccess.PaymentType pt = Broker.DataAccess.PaymentType.Get(Convert.ToInt32(ddlPaymentTypes.SelectedValue));
                Rate currentRate = Rate.GetCurrentRateForPayment(pi.ID);
                while (valueFromClient > 0) {
                    Payment newPayment = new Payment();
                    newPayment.Date = Convert.ToDateTime(tbDateOfPayment.Text);
                    newPayment.RateID = currentRate.ID;
                    newPayment.Rate = currentRate;
                    newPayment.IsCashReported = false;
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
                        newPayment.Value = currentRate.Value - Payment.GetPaidValueForRate(currentRate.ID);
                        valueFromClient -= newPayment.Value;
                        currentRate.PaidValue += newPayment.Value;
                        newPayments.Add(newPayment);
                        int currentRateNumber = currentRate.Number;
                        int nextNumber;
                        int maxNumber = Rate.Table.Where(r => r.PolicyItemID == pi.ID).OrderBy(r => r.Number).Select(r => r.Number).ToList().Last();
                        if (currentRateNumber < maxNumber) {
                            nextNumber = ++currentRateNumber;
                        } else {
                            nextNumber = currentRateNumber;
                        }
                        currentRate = Rate.Table.Where(r => r.PolicyItemID == pi.ID && r.Number == nextNumber).SingleOrDefault();
                    } else {
                        if (valueFromClient <= (currentRate.Value - currentRate.PaidValue)) {
                            newPayment.Value = valueFromClient;
                            currentRate.PaidValue = valueFromClient;
                            newPayments.Add(newPayment);
                            break;
                        } else {
                            newPayment.Value = (currentRate.Value - currentRate.PaidValue);
                            currentRate.PaidValue = currentRate.PaidValue;
                            newPayments.Add(newPayment);
                            valueFromClient -= newPayment.Value;
                            int currentRateNumber = currentRate.Number;
                            int nextNumber;
                            int maxNumber = Rate.Table.Where(r => r.PolicyItemID == pi.ID).OrderBy(r => r.Number).Select(r => r.Number).ToList().Last();
                            if (currentRateNumber < maxNumber) {
                                nextNumber = ++currentRateNumber;
                            } else {
                                nextNumber = currentRateNumber;
                            }
                            currentRate = Rate.Table.Where(r => r.PolicyItemID == pi.ID && r.Number == nextNumber).SingleOrDefault();
                        }
                    }
                }
                btnInsert.Enabled = true;
            }
        }
        odsPaidPayments.SelectParameters.Clear();
        GridViewPayments.DataSourceID = null;
        GridViewPayments.DataSource = newPayments;
        GridViewPayments.DataBind();
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
}
