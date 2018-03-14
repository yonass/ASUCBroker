using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;

public partial class Broker_LifePolicy : AuthenticationPage {

    private int policyDurationYears;
    public int PolicyDurationYears {
        get {
            try {
                if (ViewState["PolicyDurationYears"] != null) {
                    return Convert.ToInt32(ViewState["PolicyDurationYears"]);
                } else {
                    TextBox tbStartDate = (TextBox)PoliciesDetailsView.FindControl("tbStartDate");
                    TextBox tbEndDate = (TextBox)PoliciesDetailsView.FindControl("tbEndDate");
                    DateTime startDate = Convert.ToDateTime(tbStartDate.Text);
                    DateTime endDate = Convert.ToDateTime(tbEndDate.Text);
                    TimeSpan ts = endDate - startDate;
                    return Convert.ToInt32((ts.Days / 365.25));
                }
            } catch {
                return 0;
            }
        }
        set {
            ViewState["PolicyDurationYears"] = value;
        }
    }

    private int _insuranceSubtypeID;
    public int InsuranceSubtypeID {
        get {
            if (ViewState["InsuranceSubTypeID"] == null) {
                if (PoliciesDetailsView != null) {
                    DropDownList ddlInsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
                    if (ddlInsuranceSubType != null) {
                        if (ddlInsuranceSubType.SelectedIndex != -1) {
                            return int.Parse(ddlInsuranceSubType.SelectedValue);
                        }
                    }
                }
                return InsuranceSubType.Table.First().ID;
            }
            return int.Parse(ViewState["InsuranceSubTypeID"].ToString());
        }
        set {
            ViewState["InsuranceSubTypeID"] = value;
        }
    }

    private int _dealID;
    public int DealID {
        get {
            if (ViewState["DealID"] == null) {
                // return Deal.Table.First().ID;
                List<LifeDeal> dList = LifeDeal.GetBySubType(InsuranceSubtypeID);
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

    public decimal EuroValue {
        get {
            if (ViewState["euroValue"] != null) {
                decimal value = Convert.ToDecimal(ViewState["euroValue"]);
                if (value > 0) {
                    return value;
                }
            }
            try {
                Broker.DataAccess.Parameter par = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.VREDNOST_NA_EVRO);
                if (par != null) {
                    return Convert.ToDecimal(par.Value);
                }
            } catch {
                return (decimal)62;
            }
            return (decimal)62;
        }

        set {
            ViewState["euroValue"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            PolicyMultiView.SetActiveView(MainView);
            Button btnDiscard = (Button)PoliciesDetailsView.FindControl("btnDiscard");
            btnDiscard.Attributes.Add("onclick", "return confirm_delete();");
            TextBox tbEuroValue = (TextBox)PoliciesDetailsView.FindControl("tbEuroValue");
            tbEuroValue.Text = EuroValue.ToString();
        }
    }

    protected void PolicyDetailesView_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {

        }
    }

    protected void PolicyDetailesView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        if ((ViewState["ClientID"] == null) || (ViewState["OwnerID"] == null)) {
            RegisterStartupScript("myAlert", "<script>alert('НЕ Е ИЗБРАН ДОГОВОРУВАЧ ИЛИ ОСИГУРЕНИК!')</script>");
            e.Cancel = true;
            return;
        }
        

    }

    protected void ddlInsuranceCompanyIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
        DropDownList ddlDeals = PoliciesDetailsView.FindControl("ddlDeals") as DropDownList;
        ddlInsuranceSubType.DataBind();
        ddlDeals.DataBind();

        //DealID = int.Parse(ddlDeals.SelectedValue);
        if (ddlDeals.SelectedValue != null) {
            int dealID = DealID;
            int.TryParse(ddlDeals.SelectedValue, out dealID);
            DealID = dealID;
        }
        InsuranceSubtypeID = int.Parse(ddlInsuranceSubType.SelectedValue);
    }

    protected void ddlDealsSelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
        DropDownList ddlDeals = (DropDownList)sender;
        DealID = int.Parse(ddlDeals.SelectedValue);
        InsuranceSubtypeID = int.Parse(ddlInsuranceSubType.SelectedValue);
    }

    protected void ddlInsuranceSubType_selecteIndexChanged(object sender, EventArgs e) {
        DropDownList ddlDeals = PoliciesDetailsView.FindControl("ddlDeals") as DropDownList;
        DropDownList ddlInsuranceSubType = (DropDownList)sender;
        DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;

        List<LifeDeal> dealList = LifeDeal.GetActiveDealsForCompanyAndInsuranceSubType(int.Parse(ddlInsuranceCompany.SelectedValue), int.Parse(ddlInsuranceSubType.SelectedValue));
        InsuranceSubtypeID = int.Parse(ddlInsuranceSubType.SelectedValue);

        if (dealList.Count != 0) {
            DealID = dealList[0].ID;
        }
        //else {
        //dealList = Deal.GetBySubType(int.Parse(ddlInsuranceSubType.SelectedValue));
        //DealID = dealList[0].ID;
        //}
        //FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompany);
        //ddlInsuranceCompany.DataBind();
        ddlDeals.DataBind();
    }

    protected void tbPolicyNumber_Changed(object sender, EventArgs e) {
        //TextBox tbPolicyNumber = (TextBox)sender;
        TextBox tbPolicyNumber = (TextBox)PoliciesDetailsView.FindControl("tbPolicyNumber");
        string policyNumber = tbPolicyNumber.Text;
        DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;
        //Distribution distribution = Distribution.GetByNumberAndSubTypeAndCompany(InsuranceSubtypeID, policyNumber, Convert.ToInt32(ddlInsuranceCompany.SelectedValue));
        Distribution distribution = Distribution.GetByNumberAndSubTypeAndCompanyIsAccepted(InsuranceSubtypeID, policyNumber, Convert.ToInt32(ddlInsuranceCompany.SelectedValue), this.PageUser.BranchID);
        if (distribution != null) {
            if (distribution.IsUsed) {
                RegisterStartupScript("myAlert", "<script>alert('ПОЛИСАТА Е ИСКОРИСТЕНА!')</script>");
                //PoliciesDetailsView.Enabled = false;
            } else {
                //PoliciesDetailsView.Enabled = true;
            }
        } else {
            RegisterStartupScript("myAlert", "<script>alert('НЕМА ЗАДОЛЖУВАЊЕ СО ОВОЈ БРОЈ НА ПОЛИСА!')</script>");
            //PoliciesDetailsView.Enabled = false;
        }
    }

    protected void btnDiscard_Click(object sender, EventArgs e) {

    }

    protected void btnClientToOwner_Click(object sender, EventArgs e) {
        if (ViewState["ClientID"] != null) {
            TextBox tbEmbg = PoliciesDetailsView.FindControl("tbOwnerEMBG") as TextBox;
            TextBox tbName = PoliciesDetailsView.FindControl("tbOwnerName") as TextBox;
            int clientID = int.Parse(ViewState["ClientID"].ToString());
            Broker.DataAccess.Client o = Broker.DataAccess.Client.Get(clientID);
            tbEmbg.Text = o.EMBG;
            tbName.Text = o.Name;
            ViewState["OwnerID"] = clientID;
        }
    }

    protected void btnOwnerToClient_Click(object sender, EventArgs e) {

        if (ViewState["OwnerID"] != null) {
            TextBox tbEmbg = PoliciesDetailsView.FindControl("tbClientEMBG") as TextBox;
            TextBox tbName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
            int ownerID = int.Parse(ViewState["OwnerID"].ToString());
            Broker.DataAccess.Client c = Broker.DataAccess.Client.Get(ownerID);
            tbEmbg.Text = c.EMBG;
            tbName.Text = c.Name;
            ViewState["ClientID"] = ownerID;
        }
    }

    protected void btnOwnerEMBGSearch_Click(object sender, EventArgs e) {
        TextBox tbEmbg = PoliciesDetailsView.FindControl("tbOwnerEMBG") as TextBox;
        Broker.DataAccess.Client o = Broker.DataAccess.Client.GetByEmbg(tbEmbg.Text);
        Panel ownerPanel = PoliciesDetailsView.FindControl("pnlOwner") as Panel;
        if (o != null) {
            ViewState["OwnerID"] = o.ID;
            TextBox tbName = PoliciesDetailsView.FindControl("tbOwnerName") as TextBox;
            tbName.Text = o.Name;
            ownerPanel.Visible = false;
        } else {
            ownerPanel.Visible = true;
            DetailsView OwnerDetailsView = ownerPanel.FindControl("OwnerDetailsView") as DetailsView;
            TextBox tbOwnerEMBG = OwnerDetailsView.FindControl("tbEMBG") as TextBox;
            tbOwnerEMBG.Text = tbEmbg.Text;
            tbOwnerEMBG.ReadOnly = true;
        }
    }

    protected void btnClientEMBGSearch_Click(object sender, EventArgs e) {
        TextBox tbClientEmbg = PoliciesDetailsView.FindControl("tbClientEMBG") as TextBox;
        string embg = tbClientEmbg.Text;
        Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(embg);
        Panel pnlClient = PoliciesDetailsView.FindControl("pnlClient") as Panel;
        if (c != null) {
            TextBox tbClientName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
            ViewState["ClientID"] = c.ID;
            tbClientName.Text = c.Name;
            pnlClient.Visible = false;
        } else {

            pnlClient.Visible = true;
            DetailsView ClientDetailsView = pnlClient.FindControl("ClientDetailsView") as DetailsView;
            TextBox tbClientEMBG = ClientDetailsView.FindControl("tbEMBG") as TextBox;
            tbClientEMBG.Text = embg;
            tbClientEMBG.ReadOnly = true;

        }

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
    }

    protected void ClientDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
        }
    }

    protected void ClientDetailsView_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {

    }

    protected void ClientDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void ClientDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        DetailsView ClientDetailsView = (DetailsView)sender;
        DropDownList ddlClientTypes = (DropDownList)ClientDetailsView.FindControl("ddlClientTypes");
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

    protected void ClientdvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void ClientdvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void ClientdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void ClientdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        // if (Validator.GetByName("EmbgInsertValidator").IsActive) {
        c.ValidateClient();
        //  }


    }
    protected void ClientDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            //  mvMain.SetActiveView(viewGrid);
            Panel pnl = PoliciesDetailsView.FindControl("pnlClient") as Panel;
            pnl.Visible = false;
        }
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
    }

    protected void OwnerDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
        }
    }

    protected void OwnerDetailsView_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        //GXGridView1.TotalRecords = Client.SelectCountCached();
        //GXGridView1.DataBind();
        //mvMain.SetActiveView(viewGrid);
    }

    protected void OwnerDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void OwnerDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        DetailsView ClientDetailsView = (DetailsView)sender;
        DropDownList ddlClientTypes = (DropDownList)ClientDetailsView.FindControl("ddlClientTypes");
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

    protected void OwnerdvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void OwnerdvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void OwnerdvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void OwnerdvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        Broker.DataAccess.Client c = e.InputParameters["entityToInsert"] as Broker.DataAccess.Client;
        //if (Validator.GetByName("EmbgInsertValidator").IsActive) {
        c.ValidateOwner();
        // }
    }
    protected void OwnerDetailsView_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            //  mvMain.SetActiveView(viewGrid);
            Panel pnl = PoliciesDetailsView.FindControl("pnlOwner") as Panel;
            pnl.Visible = false;
        }
    }

    protected void tbStartDate_TextChanged(object sender, EventArgs e) {
        TextBox tbStartDate = (TextBox)sender;
        TextBox tbEndDate = (TextBox)PoliciesDetailsView.FindControl("tbEndDate");
        try {
            DateTime startDate = Convert.ToDateTime(tbStartDate.Text);
            //tbEndDate.Text = startDate.AddYears(1).ToShortDateString();
            CalculateDurationYears();
            CalculateTotalValues();
            tbEndDate.Focus();
            return;
        } catch {
        }
        try {
            DateTime startDate = new DateTime(Convert.ToInt32(tbStartDate.Text.Substring(4, 4)), Convert.ToInt32(tbStartDate.Text.Substring(2, 2)), Convert.ToInt32(tbStartDate.Text.Substring(0, 2)));
            tbStartDate.Text = startDate.ToShortDateString();
            //tbEndDate.Text = startDate.AddYears(1).ToShortDateString();
            CalculateDurationYears();
            CalculateTotalValues();
            tbEndDate.Focus();
        } catch {
        }
    }

    void CalculateDurationYears() {
        try {
            TextBox tbStartDate = (TextBox)PoliciesDetailsView.FindControl("tbStartDate");
            TextBox tbEndDate = (TextBox)PoliciesDetailsView.FindControl("tbEndDate");
            DateTime startDate = Convert.ToDateTime(tbStartDate.Text);
            DateTime endDate = Convert.ToDateTime(tbEndDate.Text);
            TimeSpan ts = endDate - startDate;
            int years = Convert.ToInt32(ts.Days / 365.25);
            PolicyDurationYears = years;
        } catch {
            PolicyDurationYears = 0;
        }
        TextBox tbPolicyDurationYears = (TextBox)PoliciesDetailsView.FindControl("tbPolicyDurationYears");
        tbPolicyDurationYears.Text = PolicyDurationYears.ToString();
    }

    protected void tbEndDate_TextChanged(object sender, EventArgs e) {
        TextBox tbEndDate = (TextBox)sender;
        TextBox tbApplicationDate = (TextBox)PoliciesDetailsView.FindControl("tbApplicationDate");
        try {
            DateTime endDate = Convert.ToDateTime(tbEndDate.Text);
            CalculateDurationYears();
            CalculateTotalValues();
            tbApplicationDate.Focus();
            return;
        } catch {
        }
        try {
            DateTime endDate = new DateTime(Convert.ToInt32(tbEndDate.Text.Substring(4, 4)), Convert.ToInt32(tbEndDate.Text.Substring(2, 2)), Convert.ToInt32(tbEndDate.Text.Substring(0, 2)));
            tbEndDate.Text = endDate.ToShortDateString();
            CalculateDurationYears();
            CalculateTotalValues();
            tbApplicationDate.Focus();
            return;
        } catch {
        }
    }

    protected void tbApplicationDate_TextChanged(object sender, EventArgs e) {
        TextBox tbApplicationDate = (TextBox)sender;
        try {
            DateTime applicationDate = new DateTime(Convert.ToInt32(tbApplicationDate.Text.Substring(4, 4)), Convert.ToInt32(tbApplicationDate.Text.Substring(2, 2)), Convert.ToInt32(tbApplicationDate.Text.Substring(0, 2)));
            tbApplicationDate.Text = applicationDate.ToShortDateString();
        } catch {
        }
    }

     protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
         LifePolicy p = (LifePolicy)e.ReturnValue;
         if (p != null) {
             LifeDeal lifeDeal = LifeDeal.Get(DealID);
             decimal totalBrokerageValue = (p.InsuranceCoverageOne * (lifeDeal.PercentageFromSumForRestLiving / 100)) + (p.YearlyPremiumValueForAccident * (lifeDeal.PercentageFromPremiumForAccident / 100));
             List<LifeDealBrokerage> lstLPBrokerage = LifeDealBrokerage.GetByLifeDeal(lifeDeal.ID);
             int counter = 1;
             foreach (LifeDealBrokerage ldb in lstLPBrokerage) {
                 LifePolicyBrokerage lpb = new LifePolicyBrokerage();
                 lpb.FromDate = p.ApplicationDate.AddYears(counter-1);
                 lpb.IsFactured = false;
                 lpb.LifePolicyID = p.ID;
                 lpb.OrdinalNumber = counter;
                 lpb.ToDate = p.ApplicationDate.AddYears(counter);
                 lpb.Value = (ldb.BrokeragePecentForYear / 100) * totalBrokerageValue;
                 lpb.Insert();
                 counter++;
             }
             for (int i = 1; i <= PolicyDurationYears; i++) {
                 LifePolicyPremiumValue lppv = new LifePolicyPremiumValue();
                 lppv.FromDate = p.StartDate.AddYears(i - 1);
                 lppv.LifePolicyID = p.ID;
                 lppv.OrdinalNumberYear = i;
                 lppv.PremiumValueForAccident = p.YearlyPremiumValueForAccident;
                 lppv.PremiumValueForAccidentEuro = p.YearlyPremiumValueForAccidentEuro;
                 lppv.PremiumValueForLife = p.YearlyPremiumValueForLife;
                 lppv.PremiumValueForLifeEuro = p.YearlyPremiumValueForLifeEuro;
                 lppv.ToDate = p.StartDate.AddYears(i);
                 lppv.Insert();
             }
             Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(p.InsuranceSubTypeID, p.PolicyNumber, p.InsuranceCompanyID);
             if (d != null) {
                 d.IsUsed = true;
             } else {
                 RightRestrictionDistribution rrd = RightRestrictionDistribution.GetForCompanyAndType(p.InsuranceCompanyID, p.PolicyNumber, DistributionDocumentType.GetByCode(DistributionDocumentType.BLANKO).ID);
                 rrd.IsUsed = true;
             }
             Policy.Table.Context.SubmitChanges();
         }
    }

    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        if ((ViewState["ClientID"] == null) || (ViewState["OwnerID"] == null)) {
            RegisterStartupScript("myAlert", "<script>alert('НЕ Е ИЗБРАН ДОГОВОРУВАЧ ИЛИ ОСИГУРЕНИК!')</script>");
            return;
        }
        LifePolicy p = e.InputParameters["entityToInsert"] as LifePolicy;
        p.InsertDate = DateTime.Now;
        p.BranchID = this.PageUser.BranchID;
        p.ClientID = (int)ViewState["ClientID"];
        p.OwnerID = (int)ViewState["OwnerID"];
        p.InsuranceSubTypeID = InsuranceSubtypeID;
        p.UserID = this.PageUser.ID;
        p.LifeDealID = DealID;
        TextBox tbPolicyNumber = PoliciesDetailsView.FindControl("tbPolicyNumber") as TextBox;
        p.PolicyNumber = tbPolicyNumber.Text;
        p.ValidateSinglePolicyNumber(p.InsuranceCompanyID, InsuranceSubtypeID, p.PolicyNumber, p.BranchID);
    }

    private void CalculateTotalValues() {
        CalculatePremiumTotalValues();
        CalculateInsuranceCoverageTotalValues();
    }

    private void CalculatePremiumTotalValues() {
        TextBox tbYearlyPremiumValueForLifeEuro = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForLifeEuro");
        TextBox tbYearlyPremiumValueForAccidentEuro = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForAccidentEuro");
        TextBox tbTotalPremumValueEuro = (TextBox)PoliciesDetailsView.FindControl("tbTotalPremumValueEuro");
        decimal totalYearlyPremiumValueEuro = 0;
        decimal yearlyPremiumValueForLifeEuro = 0;
        decimal yearlyPremiumValueForAccidentEuro = 0;
        decimal.TryParse(tbYearlyPremiumValueForLifeEuro.Text, out yearlyPremiumValueForLifeEuro);
        decimal.TryParse(tbYearlyPremiumValueForAccidentEuro.Text, out yearlyPremiumValueForAccidentEuro);
        totalYearlyPremiumValueEuro = PolicyDurationYears * (yearlyPremiumValueForLifeEuro + yearlyPremiumValueForAccidentEuro);
        tbTotalPremumValueEuro.Text = totalYearlyPremiumValueEuro.ToString();

        TextBox tbYearlyPremiumValueForLife = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForLife");
        TextBox tbYearlyPremiumValueForAccident = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForAccident");
        TextBox tbTotalPremumValue = (TextBox)PoliciesDetailsView.FindControl("tbTotalPremumValue");
        decimal totalYearlyPremiumValue = 0;
        decimal yearlyPremiumValueForLife = 0;
        decimal yearlyPremiumValueForAccident = 0;
        decimal.TryParse(tbYearlyPremiumValueForLife.Text, out yearlyPremiumValueForLife);
        decimal.TryParse(tbYearlyPremiumValueForAccident.Text, out yearlyPremiumValueForAccident);
        totalYearlyPremiumValue = PolicyDurationYears * (yearlyPremiumValueForLife + yearlyPremiumValueForAccident);
        tbTotalPremumValue.Text = totalYearlyPremiumValue.ToString();
    }

    private void CalculateInsuranceCoverageTotalValues() {
        TextBox tbInsuranceCoverageOne = (TextBox)PoliciesDetailsView.FindControl("tbInsuranceCoverageOne");
        TextBox tbInsuranceCoverageTwo = (TextBox)PoliciesDetailsView.FindControl("tbInsuranceCoverageTwo");
        TextBox tbTotalInsuranceCoverageSum = (TextBox)PoliciesDetailsView.FindControl("tbTotalInsuranceCoverageSum");
        decimal totalInsuranceCoverage = 0;
        decimal insuranceCoverageOne = 0;
        decimal insuranceCoverageTwo = 0;
        decimal.TryParse(tbInsuranceCoverageOne.Text, out insuranceCoverageOne);
        decimal.TryParse(tbInsuranceCoverageTwo.Text, out insuranceCoverageTwo);
        totalInsuranceCoverage = PolicyDurationYears * (insuranceCoverageOne + insuranceCoverageTwo);
        tbTotalInsuranceCoverageSum.Text = totalInsuranceCoverage.ToString();

        TextBox tbInsuranceCoverageOneEuro = (TextBox)PoliciesDetailsView.FindControl("tbInsuranceCoverageOneEuro");
        TextBox tbInsuranceCoverageTwoEuro = (TextBox)PoliciesDetailsView.FindControl("tbInsuranceCoverageTwoEuro");
        TextBox tbTotalInsuranceCoverageSumEuro = (TextBox)PoliciesDetailsView.FindControl("tbTotalInsuranceCoverageSumEuro");
        decimal totalInsuranceCoverageEuro = 0;
        decimal insuranceCoverageOneEuro = 0;
        decimal insuranceCoverageTwoEuro = 0;
        decimal.TryParse(tbInsuranceCoverageOneEuro.Text, out insuranceCoverageOneEuro);
        decimal.TryParse(tbInsuranceCoverageTwoEuro.Text, out insuranceCoverageTwoEuro);
        totalInsuranceCoverageEuro = PolicyDurationYears * (insuranceCoverageOneEuro + insuranceCoverageTwoEuro);
        tbTotalInsuranceCoverageSumEuro.Text = totalInsuranceCoverageEuro.ToString();
    }


    protected void tbEuroValue_TextChanged(object sender, EventArgs e) {
        TextBox tbEuroValue = (TextBox)sender;
        try {
            decimal dec = Convert.ToDecimal(tbEuroValue.Text);
            if (dec > 0) {
                EuroValue = dec;
            } else {
                tbEuroValue.Text = EuroValue.ToString();
            }
        } catch {
            tbEuroValue.Text = EuroValue.ToString();
        }
    }
    protected void btnReCalculate_Click(object sender, EventArgs e) {
        CalculateTotalValues();
    }
    protected void tbInsuranceCoverageOneEuro_TextChanged(object sender, EventArgs e) {
        try {
            TextBox tbInsuranceCoverageOneEuro = (TextBox)sender;
            TextBox tbInsuranceCoverageOne = (TextBox)PoliciesDetailsView.FindControl("tbInsuranceCoverageOne");
            tbInsuranceCoverageOne.Text = (Convert.ToDecimal(tbInsuranceCoverageOneEuro.Text) * EuroValue).ToString();
            CalculateInsuranceCoverageTotalValues();
        } catch {

        }
    }
    protected void tbInsuranceCoverageTwoEuro_TextChanged(object sender, EventArgs e) {
        try {
            TextBox tbInsuranceCoverageTwoEuro = (TextBox)PoliciesDetailsView.FindControl("tbInsuranceCoverageTwoEuro");
            TextBox tbInsuranceCoverageTwo = (TextBox)PoliciesDetailsView.FindControl("tbInsuranceCoverageTwo");
            tbInsuranceCoverageTwo.Text = (Convert.ToDecimal(tbInsuranceCoverageTwoEuro.Text) * EuroValue).ToString();
            CalculateInsuranceCoverageTotalValues();
        } catch {

        }
    }
    protected void tbYearlyPremiumValueForLifeEuro_TextChanged(object sender, EventArgs e) {
        try {
            TextBox tbYearlyPremiumValueForLifeEuro = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForLifeEuro");
            TextBox tbYearlyPremiumValueForLife = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForLife");
            tbYearlyPremiumValueForLife.Text = (Convert.ToDecimal(tbYearlyPremiumValueForLifeEuro.Text) * EuroValue).ToString();
            CalculatePremiumTotalValues();
        } catch {

        }
    }
    protected void tbYearlyPremiumValueForAccidentEuro_TextChanged(object sender, EventArgs e) {
        try {
            TextBox tbYearlyPremiumValueForAccidentEuro = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForAccidentEuro");
            TextBox tbYearlyPremiumValueForAccident = (TextBox)PoliciesDetailsView.FindControl("tbYearlyPremiumValueForAccident");
            tbYearlyPremiumValueForAccident.Text = (Convert.ToDecimal(tbYearlyPremiumValueForAccidentEuro.Text) * EuroValue).ToString();
            CalculatePremiumTotalValues();
        } catch {

        }
    }
}
    


