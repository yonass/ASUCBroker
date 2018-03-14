using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.FinanceControllers;
using Broker.Controllers.ReportControllers;

public partial class Broker_Clients : AuthenticationPage {
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



    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {

            GXGridView1.TotalRecords = ViewClient.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }

    }

    protected void btnPrintFinCard_Click(object sender, EventArgs e) {
        Broker.DataAccess.Client c = Broker.DataAccess.Client.Get(GXGridView1SelectedValue);
        PrintFinCardController.PrintFinCard(null, c, rblItemsTypes.SelectedValue);
    }

    protected void btnEdit_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            if (GXGridView1.SelectedValue != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                dvDataSource.SelectParameters.Clear();
                dvDataSource.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                DetailsView1.DataBind();
            }
            mvMain.SetActiveView(viewEdit);
            DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAccounts.CssClass = "smetki";
            btnRepresentationDeal.CssClass = "dogovor";
            btnFinCardForClient.CssClass = "fkartica";
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //   btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
        btnAccounts.CssClass = "smetki";
        btnRepresentationDeal.CssClass = "dogovor";
        btnFinCardForClient.CssClass = "fkartica";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewEdit);
        DetailsView1.ChangeMode(DetailsViewMode.Insert);
        btnNew.CssClass = "novZapis_Active";
        btnEdit.CssClass = "izmeni";
        //    btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAccounts.CssClass = "smetki";
        btnRepresentationDeal.CssClass = "dogovor";
        btnFinCardForClient.CssClass = "fkartica";
    }
    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.TotalRecords = ViewClient.SelectCountCached();
        GXGridView1.DataSourceID = odsGridView.ID;
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //    btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAccounts.CssClass = "smetki";
        btnRepresentationDeal.CssClass = "dogovor";
        btnFinCardForClient.CssClass = "fkartica";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewClient.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }

    void SetSaldoValues() {
        if (GXGridView1SelectedValue > 0) {
            Broker.DataAccess.Client c = Broker.DataAccess.Client.Get(GXGridView1SelectedValue);
            List<FinanceCardInfo> lst = new List<FinanceCardInfo>();
            if (rblItemsTypes.SelectedValue == "AllItems") {
                lst = FinanceCardController.GetByClient(c);
            } else if (rblItemsTypes.SelectedValue == "OpenItems") {
                lst = FinanceCardController.GetByClientOpenItems(c);
            }
            decimal debtValue = 0;
            decimal demandValue = 0;
            foreach (FinanceCardInfo fci in lst) {
                debtValue += fci.DebtValue;
                demandValue += fci.DemandValue;
            }
            tbDebtValue.Text = String.Format("{0:#,0.00}", debtValue);
            tbDemandValue.Text = String.Format("{0:#,0.00}", demandValue);
            tbSaldoValue.Text = String.Format("{0:#,0.00}", (debtValue - demandValue));
        }
    }


    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            btnEdit.Enabled = true;
            btnAccounts.Enabled = true;
            btnRepresentationDeal.Enabled = true;
            btnFinCardForClient.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            dvClientPreview.DataBind();
            dvClientForAccreditation.DataBind();
            GridViewClientAccounts.DataBind();
            GridViewAccreditations.DataBind();
            GridViewMainFinCard.DataBind();
            //GridViewFinCard.DataBind();
            SetSaldoValues();
        }
        if (e.CommandName == "DoubleClick") {
            btnEdit.Enabled = true;
            btnAccounts.Enabled = true;
            btnRepresentationDeal.Enabled = true;
            btnFinCardForClient.Enabled = true;
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAccounts.CssClass = "smetki_Active";
            btnRepresentationDeal.CssClass = "dogovor";
            btnFinCardForClient.CssClass = "fkartica";
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            mvMain.SetActiveView(viewClientAccounts);
            mvClientAccounts.SetActiveView(viewClientAccountsGrid);
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
            dvClientPreview.DataBind();
            dvClientForAccreditation.DataBind();
            //GridViewFinCard.DataBind();
            GridViewMainFinCard.DataBind();
            SetSaldoValues();
        }
    }
    protected void btnFinCardForClient_Click(object sender, EventArgs e) {
        GridViewMainFinCard.DataBind();
        SetSaldoValues();
        mvMain.SetActiveView(viewFinancialCard);
        btnFinCardForClient.CssClass = "fkartica_Active";
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
        btnAccounts.CssClass = "smetki";
        btnRepresentationDeal.CssClass = "dogovor";
    }

    protected void GridViewMainFinCard_RowDataBound(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            GridView gv = (GridView)e.Row.FindControl("GridViewFinCard");
            Policy p = Policy.Get(Convert.ToInt32(GridViewMainFinCard.DataKeys[e.Row.RowIndex].Value));
            List<FinanceCardInfo> lst = FinanceCardController.GetByPolicy(p, 0);
            gv.DataSource = lst;
            gv.DataBind();
        }
    }

    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        btnEdit.CssClass = "izmeni";
        //  btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
        btnAccounts.CssClass = "smetki";
        btnRepresentationDeal.CssClass = "dogovor";
        btnFinCardForClient.CssClass = "fkartica";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewClient.SelectFilterCountCached(FilterControl1.FCFilterArgument);

    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            DetailsView1.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            mvMain.SetActiveView(viewEdit);
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            //  btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAccounts.CssClass = "smetki";
            btnRepresentationDeal.CssClass = "dogovor";
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewClient.SelectCountCached();
            GXGridView1.DataBind();
            //mvMain.SetActiveView(viewGrid);

            string clientEMBG = e.Values["EMBG"] as string;
            Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(clientEMBG);

            string DataKeyValue = c.ID.ToString();
            GXGridView1SelectedValue = c.ID;

            // Set the GridView to the values found

            dvClientPreview.DataBind();
            GridViewClientAccounts.DataBind();
            mvMain.SetActiveView(viewClientAccounts);
            mvClientAccounts.SetActiveView(viewClientAccountsGrid);

        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewClient.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewClient.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        DropDownList ddlClientTypes = (DropDownList)DetailsView1.FindControl("ddlClientTypes");
        if (ddlClientTypes.SelectedValue == "Private") {
            e.Values["IsLaw"] = false;
            e.Values["IsForeigner"] = false;
        }
        if (ddlClientTypes.SelectedValue == "Law") {
            e.Values["IsLaw"] = true;
            e.Values["IsForeigner"] = false;
        }
        if (ddlClientTypes.SelectedValue == "ForeignPrivate") {
            e.Values["IsLaw"] = false;
            e.Values["IsForeigner"] = true;
        }
        if (ddlClientTypes.SelectedValue == "ForeignLaw") {
            e.Values["IsLaw"] = true;
            e.Values["IsForeigner"] = true;
        }
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        c.ValidateClient();
    }

    protected void btnDelete_Click1(object sender, EventArgs e) {
        //State s = State.Get(Convert.ToInt32(DetailsView1.SelectedValue));
        //s.IsActive = false;
        //State.Table.Context.SubmitChanges();
        GXGridView1.TotalRecords = ViewClient.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }
    protected void btnAccounts_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            mvMain.SetActiveView(viewClientAccounts);
            mvClientAccounts.SetActiveView(viewClientAccountsGrid);
            if (GXGridView1.SelectedValue != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsClientPreview.SelectParameters.Clear();
                odsClientPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvClientPreview.DataBind();
                odsGridViewClientAccounts.SelectParameters.Clear();
                odsGridViewClientAccounts.SelectParameters.Add("clientID", GXgvSelectedValue.ToString());
                GridViewClientAccounts.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsClientPreview.SelectParameters.Clear();
                odsClientPreview.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvClientPreview.DataBind();
                odsGridViewClientAccounts.SelectParameters.Clear();
                odsGridViewClientAccounts.SelectParameters.Add("clientID", GXgvSelectedValue.ToString());
                GridViewClientAccounts.DataBind();
            }
            btnRepresentationDeal.CssClass = "dogovor";
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAccounts.CssClass = "smetki_Active";
        }
    }

    protected void dvClientPreview_ItemCommand(object sender, DetailsViewCommandEventArgs e) {

    }
    protected void dvClientPreview_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void odsClientPreview_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("id", GXGridView1SelectedValue);
    }
    protected void btnNewClientAccount_Click(object sender, EventArgs e) {
        mvClientAccounts.SetActiveView(viewClientAccountsEdit);
    }
    protected void DetailsViewClientAccount_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewClientAccounts);
            mvClientAccounts.SetActiveView(viewClientAccountsGrid);
        }
    }
    protected void DetailsViewClientAccount_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GridViewClientAccounts.DataBind();
            mvMain.SetActiveView(viewClientAccounts);
            mvClientAccounts.SetActiveView(viewClientAccountsGrid);
        }
    }
    protected void DetailsViewClientAccount_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        if (GXGridView1.SelectedValue != null) {
            e.Values["ClientID"] = Convert.ToInt32(GXGridView1.SelectedDataKey.Value);
        } else {
            e.Values["ClientID"] = Convert.ToInt32(dvClientPreview.SelectedValue);
        }
        e.Values["IsActive"] = true;
    }
    protected void DetailsViewClientAccount_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }
    protected void dvDataSourceClientAccount_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSourceClientAccount_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        ClientAccount ca = e.InputParameters["entityToInsert"] as ClientAccount;
        ca.ValidateAccount();
    }
    protected void btnRepresentationDeal_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {

            if (GXGridView1.SelectedValue != null) {
                int GXgvSelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
                odsClientForAccreditation.SelectParameters.Clear();
                odsClientForAccreditation.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvClientForAccreditation.DataBind();
                odsGridViewAccreditations.SelectParameters.Clear();
                odsGridViewAccreditations.SelectParameters.Add("clientID", GXgvSelectedValue.ToString());
                GridViewAccreditations.DataBind();
            } else {
                int GXgvSelectedValue = GXGridView1SelectedValue;
                odsClientForAccreditation.SelectParameters.Clear();
                odsClientForAccreditation.SelectParameters.Add("id", GXgvSelectedValue.ToString());
                dvClientForAccreditation.DataBind();
                odsGridViewAccreditations.SelectParameters.Clear();
                odsGridViewAccreditations.SelectParameters.Add("clientID", GXgvSelectedValue.ToString());
                GridViewAccreditations.DataBind();
            }

            btnRepresentationDeal.CssClass = "dogovor_Active";
            btnNew.CssClass = "novZapis";
            btnEdit.CssClass = "izmeni";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
            btnAccounts.CssClass = "smetki";
            mvMain.SetActiveView(viewAccreditation);

        }
    }

    protected void odsClientForAccreditation_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
    }

    protected void dvClientForAccreditation_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }


    protected void btnNewAccreditation_Click(object sender, EventArgs e) {
        pnlNewAccreditation.Visible = true;
    }

    protected void GridViewAccreditations_SelectedIndexChanged(object sender, EventArgs e) {
        if (GridViewAccreditations.SelectedValue != null) {
            int accreditationID = Convert.ToInt32(GridViewAccreditations.SelectedValue);
            PrintRepresentationDeal.RepresentationDeal(accreditationID);

        }
    }
    protected void btnInsertAccreditation_Click(object sender, EventArgs e) {
        bool hasSelected = false;
        foreach (ListItem listItem in cblInsuranceTypes.Items) {
            if (listItem.Selected) {
                hasSelected = true;
                break;
            }
        }
        if (hasSelected) {

            int clientID = 0;
            if (GXGridView1.SelectedValue != null) {
                clientID = Convert.ToInt32(GXGridView1.SelectedValue);
            } else {
                clientID = Convert.ToInt32(dvClientForAccreditation.SelectedValue);
            }
            Accreditation a = new Accreditation();
            a.Title = tbTitle.Text;
            a.FromDate = Convert.ToDateTime(tbFromDate.Text);
            a.ToDate = Convert.ToDateTime(tbToDate.Text);
            a.ClientID = clientID;
            a.Insert();
            foreach (ListItem listItem in cblInsuranceTypes.Items) {
                if (listItem.Selected) {
                    AccreditationInsuranceType ait = new AccreditationInsuranceType();
                    ait.AccreditationID = a.ID;
                    ait.InsuranceTypeID = Convert.ToInt32(listItem.Value);
                    ait.Insert();
                }
            }
            pnlNewAccreditation.Visible = false;
            GridViewAccreditations.DataBind();
        } else {
            RegisterStartupScript("myAlert", "<script>alert('Мора да селектирате барем една класа на осигурување!')</script>");
        }
    }
    protected void btnCancelAccreditation_Click(object sender, EventArgs e) {
        pnlNewAccreditation.Visible = false;
        GridViewAccreditations.DataBind();
    }
    protected void cbInsuranceTypesAll_CheckedChanged(object sender, EventArgs e) {
        if (cbInsuranceTypesAll.Checked == true) {
            cblInsuranceTypes.Enabled = false;
            foreach (ListItem listItem in cblInsuranceTypes.Items) {
                listItem.Selected = true;
            }
        } else {
            cblInsuranceTypes.Enabled = true;
            foreach (ListItem listItem in cblInsuranceTypes.Items) {
                listItem.Selected = false;
            }
        }
    }
    protected void GXGridView1_SelectedIndexChanged(object sender, EventArgs e) {
        GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        dvClientForAccreditation.DataBind();
        dvClientPreview.DataBind();
        GridViewAccreditations.DataBind();
        GridViewClientAccounts.DataBind();
    }

    protected void odsFinCard_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        Broker.DataAccess.Client client = Broker.DataAccess.Client.Get(GXGridView1SelectedValue);
        e.InputParameters.Add("c", client);
    }
    protected void odsPolPerClient_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("clientID", GXGridView1SelectedValue);
        e.InputParameters.Add("itemsType", rblItemsTypes.SelectedValue);
    }
    protected void odsGridViewClientAccounts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Clear();
        e.InputParameters.Add("clientID", GXGridView1SelectedValue);
    }
    protected void rblItemsTypes_SelectedIndexChanged(object sender, EventArgs e) {
        GridViewMainFinCard.DataBind();
        SetSaldoValues();
    }
    protected void GridViewMainFinCard_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {
        int index = e.NewSelectedIndex;
        GridViewMainFinCard.SelectedIndex = index;
        ViewState["PolicyForPaymentID"] = GridViewMainFinCard.SelectedValue;
        mvMain.SetActiveView(viewPaymentsForPolicy);
        BindPaymentPerPolicy();
    }
    protected void GridViewMainFinCard_RowCommand(object sender, GridViewCommandEventArgs e) {
        int selIndex = Convert.ToInt32(e.CommandArgument);
        GridViewMainFinCard.SelectedIndex = selIndex;
    }
    protected void GridViewMainFinCard_RowEditing(object sender, GridViewEditEventArgs e) {

    }

    protected void odsRates_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
        Policy pol = Policy.Get(polID);
        e.InputParameters.Clear();
        e.InputParameters.Add("policyItemID", pol.PolicyItems[0].ID);
    }
    protected void odsPayments_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
        Policy pol = Policy.Get(polID);
        e.InputParameters.Clear();
        e.InputParameters.Add("policyItemID", pol.PolicyItems[0].ID);
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
    protected void odsPayments_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {

    }

    void UpdateTextBoxes() {
        int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
        Policy pol = Policy.Get(polID);
        PolicyItem pi = pol.PolicyItems[0];
        tbPolicyPremiumCost.Text = String.Format("{0:#,0.00}", pi.PremiumValue);
        decimal paidValue = Payment.GetPaidValueForPolicyItemExtend(pi.ID);
        tbPolicyTotalPaidValue.Text = String.Format("{0:#,0.00}", paidValue);
        tbPolicyForPaidValue.Text = String.Format("{0:#,0.00}", (pi.PremiumValue - paidValue));
        tbDateOfPayment.Text = DateTime.Today.ToShortDateString();
        tbValueOfPayment.Text = "0";
    }

    void BindPaymentPerPolicy() {
        int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
        Policy pol = Policy.Get(polID);
        gvRates.Caption = "Полиса - " + pol.PolicyItems[0].PolicyNumber;
        if (gvRates.DataSourceID == null) {
            gvRates.DataSourceID = odsRates.ID;
        }
        if (gvRates.DataSourceID == string.Empty) {
            gvRates.DataSourceID = odsRates.ID;
        }
        gvRates.DataBind();
        gvPayments.Caption = "Полиса - " + pol.PolicyItems[0].PolicyNumber;
        if (gvPayments.DataSourceID == null) {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        if (gvPayments.DataSourceID == string.Empty) {
            gvPayments.DataSourceID = odsPayments.ID;
        }
        gvPayments.DataBind();
        UpdateTextBoxes();
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

    protected void btnInsert_Click(object sender, EventArgs e) {
        DateTime inputDate = Convert.ToDateTime(tbDateOfPayment.Text);
        int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
        Policy policy = Policy.Get(polID);
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
    protected void btnBack_Click(object sender, EventArgs e) {
        GridViewMainFinCard.DataBind();
        mvMain.SetActiveView(viewFinancialCard);
        SetSaldoValues();
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
            int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
            Policy pol = Policy.Get(polID);
            DataClassesDataContext dc = new DataClassesDataContext();
            string strQuery = "SELECT p.* from Payments p, Rates r Where p.RateID = r.ID AND PolicyItemID = " + pol.PolicyItems[0].ID + " ORDER BY r.Number";
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
            int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
            Policy pol = Policy.Get(polID);
            DataClassesDataContext dc = new DataClassesDataContext();
            string strQuery = "SELECT r.* from Rates r Where PolicyItemID = " + pol.PolicyItems[0].ID + " ORDER BY Number";
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
                    if (ddlBanks != null) {
                        ddlBanks.SelectedValue = p.BankslipBankID.Value.ToString();
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
                    if (ddlBanks != null) {
                        ddlBanks.SelectedValue = p.BankslipBankID.Value.ToString();
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
            int polID = Convert.ToInt32(ViewState["PolicyForPaymentID"]);
            Policy pol = Policy.Get(polID);
            DataClassesDataContext dc = new DataClassesDataContext();
            string strQuery = "SELECT p.* from Payments p, Rates r Where p.RateID = r.ID AND PolicyItemID = " + pol.PolicyItems[0].ID + " ORDER BY r.Number";
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

    protected void btnLog_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            string id = Convert.ToInt32(GXGridView1.SelectedValue).ToString();
            using (Broker.LoggingDataAccess.LoggingDataContext dc = new Broker.LoggingDataAccess.LoggingDataContext())
            {
                string relativeUrl = this.WebPage.RelativeUrl;
                List<Broker.LoggingDataAccess.ViewDBLoggingByPageWithTableID> listLog = dc.ViewDBLoggingByPageWithTableIDs.Where(c => c.RelativeUrl.Contains(relativeUrl) && c.FieldName == "ID" && c.Value == id).OrderBy(x => x.ID).ToList();

                string script = string.Empty;
                foreach (Broker.LoggingDataAccess.ViewDBLoggingByPageWithTableID logItem in listLog)
                {
                    script += logItem.Action + ": " + logItem.Name + " на " + logItem.TimeOfUpdate.ToShortDateString() + " во " + logItem.TimeOfUpdate.ToShortTimeString() + "\\n";
                }


                string alert = "<script>alert('" + script + "')</script>";


                RegisterStartupScript("myAlert", alert);


            }
        }
    }

    protected void btnLogAccess_Click(object sender, EventArgs e)
    {
        if (GXGridView1.SelectedIndex != -1)
        {
            string id = Convert.ToInt32(GXGridView1.SelectedValue).ToString();
            using (Broker.LoggingDataAccess.LoggingDataContext dc = new Broker.LoggingDataAccess.LoggingDataContext())
            {
                string relativeUrl = this.WebPage.RelativeUrl;
                List<Broker.LoggingDataAccess.ViewDBOtvoreniEdit> listLog = dc.ViewDBOtvoreniEdits.Where(c => c.RelativeUrl.Contains(relativeUrl) && c.OpenedFormID == id).OrderBy(x => x.TimeAccessed).ToList();

                string script = string.Empty;
                foreach (Broker.LoggingDataAccess.ViewDBOtvoreniEdit logItem in listLog)
                {
                    script += logItem.Action + ": " + logItem.Name + " на " + logItem.TimeAccessed.ToShortDateString() + " во " + logItem.TimeAccessed.ToShortTimeString() + "\\n";
                }

                string alert = "<script>alert('" + script + "')</script>";


                RegisterStartupScript("myAlert", alert);


            }
        }
    }

}
