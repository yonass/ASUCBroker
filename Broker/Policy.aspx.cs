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
using Broker.Controllers.ReportControllers;
using System.Web.Services.Description;

public partial class Broker_Policy : AuthenticationPage {

    public static string PolicyAttachmens_Directory {
        get {
            string physicalPath = AttachmentController.ApplicationPath();
            string Path = physicalPath + @"\Broker\Attachments\Policies";
            return Path;
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

    // private bool _isSuggestedNumber = true;
    public bool IsSuggestedNumber {
        get {
            return (ViewState["IsSuggested"] != null) ? bool.Parse(ViewState["IsSuggested"].ToString()) : true;
            //return _isSuggestedNumber;
        }
        set {
            ViewState["IsSuggested"] = value;
        }
    }

    void SetSearchByRegNumberFromTechnicalChek() {
        Panel pnlRegistrationNumberSearch = (Panel)PoliciesDetailsView.FindControl("pnlRegistrationNumberSearch");
        Broker.DataAccess.Parameter parHasSearchFromTechnical = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_PREBARUVA_OD_TEHNICKI_PREGLED);
        if (parHasSearchFromTechnical != null) {
            bool hasSearchFromTechnical = Convert.ToBoolean(parHasSearchFromTechnical.Value);
            if (hasSearchFromTechnical == true) {
                pnlRegistrationNumberSearch.Visible = true;
            } else {
                pnlRegistrationNumberSearch.Visible = false;
            }
        }
    }

    void SetSearchFromNBO() {
        Button btnNBOSearch = (Button)PoliciesDetailsView.FindControl("btnNBOSearch");
        Broker.DataAccess.Parameter parHasSearchFromNBO = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_PREBARUVA_OD_NBO);
        if (parHasSearchFromNBO != null) {
            bool hasHasSearchFromNBO = Convert.ToBoolean(parHasSearchFromNBO.Value);
            if (hasHasSearchFromNBO == true) {
                btnNBOSearch.Visible = true;
            } else {
                btnNBOSearch.Visible = false;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            if (IsSuggestedNumber) {
                SetPolicyNumber();
            }
            PolicyMultiView.SetActiveView(MainView);
            SetSearchByRegNumberFromTechnicalChek();
            SetSearchFromNBO();

            Button btnDiscard = (Button)PoliciesDetailsView.FindControl("btnDiscard");
            btnDiscard.Attributes.Add("onclick", "return confirm_delete();");

            DefaultSetIndexForSearch();
            //PoliciesDetailsView.Enabled = false;
        }
        CreateGridView(this.Page, null);
    }

    void SetPoliciesDetaildViewEnabled(bool isEnabled) {

    }

    void DefaultSetIndexForSearch() {
        Panel pnlSearchPreviosPolicy = PoliciesDetailsView.FindControl("pnlSearcPreviousPolicy") as Panel;
        RadioButtonList rblSearchParameter = pnlSearchPreviosPolicy.FindControl("rblSeachParameter") as RadioButtonList;
        rblSearchParameter.SelectedIndex = 0;
    }

    public List<Rate> RatesInGridView {
        get {
            List<Rate> ActualRates = new List<Rate>();
            GridView GridViewRates;
            Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
            GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
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
    protected void ddlInsuranceTypeSelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
        DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;
        //FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompany);
        DropDownList ddlDeals = PoliciesDetailsView.FindControl("ddlDeals") as DropDownList;
        ddlInsuranceSubType.DataBind();
        //ddlInsuranceCompany.DataBind();
        ddlDeals.DataBind();
        //DealID = int.Parse(ddlDeals.SelectedValue);
        InsuranceSubtypeID = int.Parse(ddlInsuranceSubType.SelectedValue);

        Panel pnlSearchPreviosPolicy = PoliciesDetailsView.FindControl("pnlSearcPreviousPolicy") as Panel;
        RadioButtonList rblSearchParameter = pnlSearchPreviosPolicy.FindControl("rblSeachParameter") as RadioButtonList;
        rblSearchParameter.DataBind();
        rblSearchParameter.SelectedIndex = 0;

        CreateChildControls();
        if (IsSuggestedNumber) {
            SetPolicyNumber();
        }
    }

    void SetInsuranceCompanyForPrefix(Distribution d) {
        List<Deal> deal = Deal.GetActiveDealsForCompanyAndInsuranceSubType(d.InsuranceCompanyID, d.InsuranceSubTypeID);
        if (deal.Count > 0) {
            DropDownList ddlInsuranceType = (DropDownList)PoliciesDetailsView.FindControl("ddlInsuranceType");
            DropDownList ddlInsuranceSubTypes = (DropDownList)PoliciesDetailsView.FindControl("ddlInsuranceSubTypes");
            DropDownList ddlInsuranceCompany = (DropDownList)PoliciesDetailsView.FindControl("ddlInsuranceCompany");
            DropDownList ddlDeals = (DropDownList)PoliciesDetailsView.FindControl("ddlDeals");
            ddlInsuranceType.SelectedValue = d.InsuranceSubType.InsuranceTypeID.ToString();
            ddlInsuranceSubTypes.DataBind();
            ddlInsuranceSubTypes.SelectedValue = d.InsuranceSubTypeID.ToString();
            ddlInsuranceSubTypes.DataBind();
            FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompany);
            ddlInsuranceCompany.DataBind();
            ddlInsuranceCompany.SelectedValue = d.InsuranceCompanyID.ToString();
            ddlInsuranceCompany.DataBind();
            ddlDeals.DataBind();
            CreateChildControls();
        } else {
            RegisterStartupScript("myAlert", "<script>alert('НЕМА АКТИВЕН ДОГОВОР СО ИЗБРАНАТА КОМПАНИЈА!')</script>");
        }
    }

    protected void tbPolicyNumber_Changed(object sender, EventArgs e) {
        IsSuggestedNumber = false;
        //TextBox tbPolicyNumber = (TextBox)sender;
        TextBox tbPolicyNumber = (TextBox)PoliciesDetailsView.FindControl("tbPolicyNumber");
        string policyNumber = tbPolicyNumber.Text;
        Broker.DataAccess.Parameter parHasPrefixes = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_KORISTAT_PREFIKSI);
        if (parHasPrefixes != null) {
            bool hasPrefixes = Convert.ToBoolean(parHasPrefixes.Value);
            if (hasPrefixes == true) {
                Broker.DataAccess.Parameter parPrefixLenght = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.DOLZINA_NA_PREFIKS);
                if (parPrefixLenght != null) {
                    int prefixLengh = 0;
                    int.TryParse(parPrefixLenght.Value, out prefixLengh);
                    string prefixPolicyNumber = "";
                    if (policyNumber.Length >= prefixLengh) {
                        prefixPolicyNumber = policyNumber.Substring(0, prefixLengh);
                        InsuranceCompany ic = InsuranceCompany.GetByPrefix(prefixPolicyNumber);
                        if (ic != null) {
                            //List<Distribution> listDistributions = Distribution.GetByNumberAndCompany(policyNumber, ic.ID);
                            List<Distribution> listDistributions = Distribution.GetByNumberAndCompanyAndBranch(policyNumber, ic.ID, this.PageUser.BranchID);
                            listDistributions = listDistributions.Where(l => l.IsUsed == false).ToList();
                            if (listDistributions.Count > 0) {
                                InsuranceSubType AOinsuranceSubType = InsuranceSubType.GetByCode(InsuranceSubType.AO);
                                Distribution d;
                                if (AOinsuranceSubType != null) {
                                    d = listDistributions.Where(l => l.InsuranceSubTypeID == AOinsuranceSubType.ID).SingleOrDefault();
                                    if (d != null) {
                                        SetInsuranceCompanyForPrefix(d);
                                        //PoliciesDetailsView.Enabled = true;
                                    } else {
                                        d = listDistributions[0];
                                        SetInsuranceCompanyForPrefix(d);
                                        //PoliciesDetailsView.Enabled = true;
                                    }
                                }
                            } else {
                                RegisterStartupScript("myAlert", "<script>alert('НЕМА ЗАДОЛЖУВАЊЕ СО ОВОЈ БРОЈ НА ПОЛИСА!')</script>");
                                //PoliciesDetailsView.Enabled = false;
                            }
                        } else {
                            RegisterStartupScript("myAlert", "<script>alert('НЕМА КОМПАНИЈА СО ИЗБРАНИОТ ПРЕФИКС!')</script>");
                            //PoliciesDetailsView.Enabled = false;
                        }
                    }
                }
            } else {
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
        }
    }

    public void SetPolicyNumber() {

        TextBox tbPolicyNumber = PoliciesDetailsView.FindControl("tbPolicyNumber") as TextBox;
        DropDownList ddlIsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
        DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;
        if (ddlInsuranceCompany.SelectedIndex != -1) {
            Distribution d = Distribution.GetSuggestedNumber(int.Parse(ddlInsuranceCompany.SelectedValue), this.PageUser.ID, int.Parse(ddlIsuranceSubType.SelectedValue));

            if (d != null) {
                tbPolicyNumber.Text = d.PolicyNumber;
            } else {
                tbPolicyNumber.Text = string.Empty;
            }
        } else {
            tbPolicyNumber.Text = string.Empty;
        }

    }

    protected void ddlInsuranceSubType_selecteIndexChanged(object sender, EventArgs e) {
        DropDownList ddlDeals = PoliciesDetailsView.FindControl("ddlDeals") as DropDownList;
        DropDownList ddlInsuranceSubType = (DropDownList)sender;
        DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;

        List<Deal> dealList = Deal.GetActiveDealsForCompanyAndInsuranceSubType(int.Parse(ddlInsuranceCompany.SelectedValue), int.Parse(ddlInsuranceSubType.SelectedValue));
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

        Panel pnlSearchPreviosPolicy = PoliciesDetailsView.FindControl("pnlSearcPreviousPolicy") as Panel;
        RadioButtonList rblSearchParameter = pnlSearchPreviosPolicy.FindControl("rblSeachParameter") as RadioButtonList;
        rblSearchParameter.DataBind();
        rblSearchParameter.SelectedIndex = 0;

        CreateChildControls();
        if (IsSuggestedNumber) {
            SetPolicyNumber();
        }
    }
    protected void PolicyFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e) {
    }

    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
    }


    protected void btnDelete_Click1(object sender, EventArgs e) {
    }

    protected void ddlOfferItems_SelectedIndexChanged(object sender, EventArgs e) {


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
    protected void fvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        e.InputParameters.Add("OfferItemID", offerItemID);
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


    protected void ddlInsuranceCompanyIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceType = PoliciesDetailsView.FindControl("ddlInsuranceType") as DropDownList;
        DropDownList ddlInsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
        DropDownList ddlDeals = PoliciesDetailsView.FindControl("ddlDeals") as DropDownList;
        ddlInsuranceType.DataBind();
        ddlInsuranceSubType.DataBind();
        ddlDeals.DataBind();

        //DealID = int.Parse(ddlDeals.SelectedValue);
        if (ddlDeals.SelectedValue != null) {
            int dealID = DealID;
            int.TryParse(ddlDeals.SelectedValue, out dealID);
            DealID = dealID;
        }
        InsuranceSubtypeID = int.Parse(ddlInsuranceSubType.SelectedValue);
        CreateChildControls();
        if (IsSuggestedNumber) {
            SetPolicyNumber();
        }
    }

    protected void ddlDealsSelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlInsuranceType = PoliciesDetailsView.FindControl("ddlInsuranceType") as DropDownList;
        DropDownList ddlInsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
        DropDownList ddlDeals = (DropDownList)sender;
        DealID = int.Parse(ddlDeals.SelectedValue);
        InsuranceSubtypeID = int.Parse(ddlInsuranceSubType.SelectedValue);
        CreateChildControls();
    }

    protected void btnInsert_Click(object sender, EventArgs e) {


    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {

    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
        Client c = e.InputParameters["newEntity"] as Client;
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
        // Ovde ako se insertirala polisata se insertira prvo policyItem, pa se brisat zadolzuvanja
        // pa se vnesuvat vrednostite od generiranite polinja, pa ima proverka dali e na rati i se vnesuvatr ratite
        Policy p = (Policy)e.ReturnValue;
        if (p != null) {
            PolicyItem pi = new PolicyItem();

            Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
            TextBox tbBrokeragePercentage = pnlExtendControls.FindControl("tbBrokeragePercentage1") as TextBox;
            pi.BrokeragePercentage = 0;// decimal.Parse(tbBrokeragePercentage.Text);
            TextBox tbBrokerageValue = pnlExtendControls.FindControl("tbBrokerageValue1") as TextBox;
            pi.BrokerageValue = 0;// decimal.Parse(tbBrokerageValue.Text);
            DropDownList ddlDeals = PoliciesDetailsView.FindControl("ddlDeals") as DropDownList;
            int dealID = int.Parse(ddlDeals.SelectedValue);
            pi.BrokerageID = Brokerage.GetActiveBrokeragesForDealAndInsuranceSubType(dealID, InsuranceSubtypeID)[0].ID;
            DropDownList dllInsuranceSubType = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
            pi.InsuranceSubTypeID = int.Parse(dllInsuranceSubType.SelectedValue);
            pi.PolicyNumber = p.PolicyNumber;
            TextBox tbPolicyValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
            TextBox tbRealPolicyValue = pnlExtendControls.FindControl("tbRealPolicyValue1") as TextBox;
            pi.PremiumValue = decimal.Parse(tbPolicyValue.Text);
            pi.RealPremiumValue = decimal.Parse(tbRealPolicyValue.Text);
            pi.DiscountValue = pi.RealPremiumValue - pi.PremiumValue;
            pi.StatusID = p.StatusID;
            pi.PolicyID = p.ID;
            //CheckBox cbIsPaidInBrokerHouse = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
            //pi.IsPaidInBrokerHouse = cbIsPaidInBrokerHouse.Checked;
            RadioButtonList rblPaymentTypes = (RadioButtonList)PoliciesDetailsView.FindControl("rblPaymentTypes");
            if (rblPaymentTypes.SelectedValue == "PaidOnce" || rblPaymentTypes.SelectedValue == "PaidRates") {
                pi.IsPaidInBrokerHouse = true;
            } else {
                pi.IsPaidInBrokerHouse = false;
            }
            p.TotalSum = pi.PremiumValue;
            Brokerage brok = Brokerage.Get(pi.BrokerageID);
            if (p.Client.IsLaw) {
                pi.BrokeragePercentage = brok.PercentageForLaws;
                pi.BrokerageValue = (pi.PremiumValue * brok.PercentageForLaws / 100);
            } else {
                pi.BrokeragePercentage = brok.PercentageForPrivates;
                pi.BrokerageValue = (pi.PremiumValue * brok.PercentageForPrivates / 100);
            }
            pi.PolicyIncomeFactureStatusID = PolicyIncomeFactureStatuse.GetByCode(PolicyIncomeFactureStatuse.NE_E_FAKTURIRANA).ID;
            pi.IsAccountFactured = false;
            pi.IsBrokerageSliped = false;
            if (p.MarketingAgentID != null) {
                BrokeragesForMarketingAgent bfma = BrokeragesForMarketingAgent.GetByUserAndInsuranceCompanyForSubType((int)p.MarketingAgentID, p.InsuranceCompanyID, pi.InsuranceSubTypeID);
                if (bfma != null) {
                    pi.MarketingAgentBrokerageID = bfma.ID;
                }
            }
            if (rblPaymentTypes.SelectedValue == "PaidRates" || rblPaymentTypes.SelectedValue == "PaidInInsuranceCompany") {
                Panel pnlRates = (Panel)PoliciesDetailsView.FindControl("pnlRates");
                DropDownList ddlRateDeals = (DropDownList)pnlRates.FindControl("ddlRateDeals");
                int ddlRateDealsSelValue = 0;
                if (ddlRateDeals.SelectedValue != null) {
                    int.TryParse(ddlRateDeals.SelectedValue, out ddlRateDealsSelValue);
                    if (ddlRateDealsSelValue > 0) {
                        pi.RateDealInsuranceSubTypeID = RateDealInsuranceSubType.GetByRateDealInsuranceSubTypePremiumValue(ddlRateDealsSelValue, pi.InsuranceSubTypeID, pi.PremiumValue).ID;
                    }
                }
                CheckBox cbIsForFacturing = pnlRates.FindControl("cbIsForFacturing") as CheckBox;
                pi.IsFactureCreated = cbIsForFacturing.Checked;
            }
            if (rblPaymentTypes.SelectedValue == "PaidOnce") {
                Panel PaymentTypePanel = (Panel)PoliciesDetailsView.FindControl("PaymentTypePanel");
                RadioButtonList rblPaymentTypeForOncePayment = PaymentTypePanel.FindControl("rblPaymentTypeForOncePayment") as RadioButtonList;
                if (rblPaymentTypeForOncePayment.SelectedValue == "FacturePayment") {
                    CheckBox cvIsForFacturingOncePaid = PaymentTypePanel.FindControl("cvIsForFacturingOncePaid") as CheckBox;
                    pi.IsFactureCreated = cvIsForFacturingOncePaid.Checked;
                }
            }
            pi.Insert();

            Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(pi.InsuranceSubTypeID, pi.PolicyNumber, pi.Policy.InsuranceCompanyID);
            if (d != null) {
                d.IsUsed = true;
            } else {
                RightRestrictionDistribution rrd = RightRestrictionDistribution.GetForCompanyAndType(pi.Policy.InsuranceCompanyID, pi.PolicyNumber, DistributionDocumentType.GetByCode(DistributionDocumentType.BLANKO).ID);
                rrd.IsUsed = true;
            }

            Policy.Table.Context.SubmitChanges();
            //TemporaryDistribution td = TemporaryDistribution.GetByDistribution(d.ID);
            //if (td != null) {
            //    td.Delete();
            //}


            List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetActiveByInsuranceSubType(pi.InsuranceSubTypeID);
            foreach (Broker.DataAccess.Control c in listControls) {
                PolicyExtendInformation pei = new PolicyExtendInformation();
                pei.ControlID = c.ID;
                pei.PolicyItemID = pi.ID;

                if (c.FieldType.Name == FieldType.TEXTBOX) {
                    TextBox tbControl = (TextBox)pnlExtendControls.FindControl(c.TextBoxID);
                    pei.Value = tbControl.Text;
                    if (c.HasCompareValidator) {
                        CompareValidator cv = (CompareValidator)pnlExtendControls.FindControl("cv" + c.TextBoxID);
                        if (cv.Type == ValidationDataType.Double) {
                            if (tbControl.Text == string.Empty) {
                                pei.Value = "0";
                            }
                        }
                    }
                }
                if (c.FieldType.Name == FieldType.DROPDOWNLIST) {
                    DropDownList ddlControl = (DropDownList)pnlExtendControls.FindControl(c.TextBoxID);
                    pei.Value = ddlControl.SelectedItem.Text;
                    pei.ValueID = int.Parse(ddlControl.SelectedValue);
                }
                if (c.FieldType.Name == FieldType.CHECKBOX) {
                    CheckBox cb = (CheckBox)pnlExtendControls.FindControl(c.TextBoxID);
                    pei.Value = cb.Checked.ToString();
                    pei.ValueID = null;
                }


                pei.Insert();
            }
            List<Rate> currentRates = new List<Rate>();
            List<Rate> RatesForInsert = new List<Rate>();

            GridView GridViewRates;
            GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
            int paymentTypeForFirstPayment = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.FACTURE).ID;
            int? bankCreditCardForFirstPayment = null;
            if (GridViewRates != null) {
                foreach (GridViewRow row in GridViewRates.Rows) {
                    Rate r = new Rate();
                    TextBox tbNumber = (TextBox)row.Cells[0].FindControl("tbNumber");
                    r.Number = Convert.ToInt32(tbNumber.Text);
                    TextBox tbDate = (TextBox)row.Cells[1].FindControl("tbDate");
                    r.Date = Convert.ToDateTime(tbDate.Text);
                    TextBox tbvalue = (TextBox)row.Cells[2].FindControl("tbValue");
                    r.Value = 0;
                    decimal rValue = 0;
                    decimal.TryParse(tbvalue.Text, out rValue);
                    r.Value = rValue;
                    r.BrokerageValue = 0;
                    DropDownList ddlPaymentType = (DropDownList)row.Cells[3].FindControl("ddlPaymentTypes");
                    r.PolicyItemID = 1;
                    currentRates.Add(r);
                    if (r.Number == 1) {
                        paymentTypeForFirstPayment = Convert.ToInt32(ddlPaymentType.SelectedValue);
                        if (Broker.DataAccess.PaymentType.Get(paymentTypeForFirstPayment).Code == Broker.DataAccess.PaymentType.CREDITCARD)
                        {
                            DropDownList ddlBankCreditCards = (DropDownList)row.Cells[4].FindControl("ddlBankCreditCards");
                            bankCreditCardForFirstPayment = Convert.ToInt32(ddlBankCreditCards.SelectedValue);
                        }
                    }
                }
            }
            if ((rblPaymentTypes.SelectedValue == "PaidRates") || (rblPaymentTypes.SelectedValue == "PaidInInsuranceCompany")) {
                if (currentRates.Count == 0) {
                    Rate r = new Rate();
                    //r.Date = DateTime.Today;
                    r.Date = p.ApplicationDate;
                    r.Number = 1;
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
                        r.PolicyItemID = pi.ID;
                        r.Value = rate.Value;
                        r.BrokerageValue = rate.BrokerageValue;
                        r.PaidValue = 0;
                        if (rblPaymentTypes.SelectedValue == "PaidRates") {
                            if (rate.Number == 1) {
                                r.PaidValue = rate.Value;
                            } else {
                                r.PaidValue = 0;
                            }
                        }
                        RatesForInsert.Add(r);
                        r.Insert();
                        if (rate.Number == 1) {
                            Payment paidFirstRate = new Payment();
                            paidFirstRate.Date = r.Date;
                            paidFirstRate.IsFactured = false;
                            paidFirstRate.RateID = r.ID;
                            paidFirstRate.Value = r.Value;
                            paidFirstRate.PaymentTypeID = paymentTypeForFirstPayment;
                            paidFirstRate.BankCreditCardID = bankCreditCardForFirstPayment;
                            paidFirstRate.BranchID = this.PageUser.BranchID;
                            paidFirstRate.UserID = this.PageUser.ID;
                            paidFirstRate.IsCashReported = false;
                            paidFirstRate.Insert();
                            decimal basicValue = paidFirstRate.Value;
                            decimal k = basicValue / paidFirstRate.Rate.PolicyItem.PremiumValue;
                            List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                            foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                                PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                                if (pei != null) {
                                    decimal peiValue = 0;
                                    decimal.TryParse(pei.Value, out peiValue);
                                    basicValue -= k * peiValue;
                                    if (peiValue > 0) {
                                        PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                        ppist.PaymentID = paidFirstRate.ID;
                                        ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                        ppist.PaidValue = k * peiValue;
                                        ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                        ppist.Insert();
                                    }
                                }
                            }
                            if (basicValue > 0) {
                                PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                ppist.PaymentID = paidFirstRate.ID;
                                ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                                ppist.PaidValue = basicValue;
                                ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                ppist.Insert();
                            }
                        }
                    }
                }
                if (rblPaymentTypes.SelectedValue == "PaidRates") {
                    if ((bool)pi.IsFactureCreated) {
                        decimal brokerageSum = 0;
                        brokerageSum += pi.BrokerageValue;
                        List<PolicyItem> listPolicyItems = new List<PolicyItem>();
                        listPolicyItems.Add(pi);
                        Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GenerateForSinglePolicy(p);
                        f.BrokerageValue = brokerageSum;
                        Broker.DataAccess.Facture.Insert(f);
                        List<FactureItem> lFI = Broker.DataAccess.Facture.GenerateFactureItemsForSinglePolicy(listPolicyItems, f);
                        Broker.DataAccess.Facture.InsertFactureItems(lFI);
                        FactureID = f.ID;
                        PolicyItemFactureItem.InsertForSingleFacture(lFI[0], pi);
                        InsertClientInFinansovo(p.ClientID);
                    }
                }
            }
            if (rblPaymentTypes.SelectedValue == "PaidOnce") {

                Panel pnlPayments = PoliciesDetailsView.FindControl("PaymentTypePanel") as Panel;
                RadioButtonList rblPaymentTypeForOncePayment = (RadioButtonList)pnlPayments.FindControl("rblPaymentTypeForOncePayment");

                Rate r = new Rate();
                //r.Date = DateTime.Today;
                r.Date = p.ApplicationDate;
                r.Number = 1;
                r.PolicyItemID = pi.ID;
                r.Value = pi.PremiumValue;
                r.BrokerageValue = pi.BrokerageValue;
                if (rblPaymentTypeForOncePayment.SelectedValue == "CashPayment") {
                    r.PaidValue = pi.PremiumValue;
                } else {
                    r.PaidValue = 0;
                }
                RatesForInsert.Add(r);
                r.Insert();

                if (rblPaymentTypeForOncePayment.SelectedValue == "CashPayment") {
                    decimal cashValue = 0;
                    TextBox tbCash = pnlPayments.FindControl("tbCash") as TextBox;
                    decimal cardValue = 0;
                    TextBox tbCard = pnlPayments.FindControl("tbCreditCard") as TextBox;
                    DropDownList ddlBank = pnlPayments.FindControl("ddlBank") as DropDownList;
                    DropDownList ddlCardTypes = pnlPayments.FindControl("ddlCardTypes") as DropDownList;

                    decimal.TryParse(tbCash.Text, out cashValue);
                    decimal.TryParse(tbCard.Text, out cardValue);
                    decimal k = 1;
                    if (cashValue > 0) {
                        Payment paidFirstRate = new Payment();
                        paidFirstRate.Date = r.Date;
                        paidFirstRate.IsFactured = false;
                        paidFirstRate.RateID = r.ID;
                        paidFirstRate.Value = cashValue;
                        paidFirstRate.PaymentTypeID = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.CASH).ID;
                        paidFirstRate.BranchID = this.PageUser.BranchID;
                        paidFirstRate.UserID = this.PageUser.ID;
                        paidFirstRate.IsCashReported = false;
                        paidFirstRate.Insert();
                        decimal basicValue = paidFirstRate.Value;
                        k = cashValue / (cashValue + cardValue);
                        List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                        foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                            if (pei != null) {
                                decimal peiValue = 0;
                                decimal.TryParse(pei.Value, out peiValue);
                                basicValue -= k * peiValue;
                                if (peiValue > 0) {
                                    PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                    ppist.PaymentID = paidFirstRate.ID;
                                    ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                    ppist.PaidValue = k * peiValue;
                                    ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                    ppist.Insert();
                                }
                            }
                        }
                        if (basicValue > 0) {
                            PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                            ppist.PaymentID = paidFirstRate.ID;
                            ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                            ppist.PaidValue = basicValue;
                            ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                            ppist.Insert();
                        }
                    }
                    if (cardValue > 0) {
                        int bankCreditCardID = BankCreditCard.GetByBankAndCard(Convert.ToInt32(ddlBank.SelectedValue), Convert.ToInt32(ddlCardTypes.SelectedValue)).ID;
                        Payment paidFirstRate = new Payment();
                        paidFirstRate.Date = r.Date;
                        paidFirstRate.IsFactured = false;
                        paidFirstRate.RateID = r.ID;
                        paidFirstRate.Value = cardValue;
                        paidFirstRate.PaymentTypeID = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.CREDITCARD).ID;
                        paidFirstRate.BankCreditCardID = bankCreditCardID;
                        paidFirstRate.BranchID = this.PageUser.BranchID;
                        paidFirstRate.UserID = this.PageUser.ID;
                        paidFirstRate.IsCashReported = false;
                        paidFirstRate.Insert();
                        decimal basicValue = paidFirstRate.Value;
                        k = cashValue / (cashValue + cardValue);
                        List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                        foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                            if (pei != null) {
                                decimal peiValue = 0;
                                decimal.TryParse(pei.Value, out peiValue);
                                basicValue -= k * peiValue;
                                if (peiValue > 0) {
                                    PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                                    ppist.PaymentID = paidFirstRate.ID;
                                    ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                                    ppist.PaidValue = k * peiValue;
                                    ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                                    ppist.Insert();
                                }
                            }
                        }
                        if (basicValue > 0) {
                            PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                            ppist.PaymentID = paidFirstRate.ID;
                            ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                            ppist.PaidValue = basicValue;
                            ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                            ppist.Insert();
                        }
                    }
                } else if (rblPaymentTypeForOncePayment.SelectedValue == "FacturePayment") {
                    //Payment paidFirstRate = new Payment();
                    //paidFirstRate.Date = r.Date;
                    //paidFirstRate.IsFactured = false;
                    //paidFirstRate.RateID = r.ID;
                    //paidFirstRate.Value = r.Value;
                    //paidFirstRate.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
                    //paidFirstRate.Insert();
                    //decimal basicValue = paidFirstRate.Value;
                    //decimal k = basicValue / paidFirstRate.Rate.PolicyItem.PremiumValue;
                    //List<ControlAppropriateInsuranceSubType> listAppropriateIST = Broker.DataAccess.ControlAppropriateInsuranceSubType.Table.ToList();
                    //foreach (ControlAppropriateInsuranceSubType c in listAppropriateIST) {
                    //    PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ControlID);
                    //    if (pei != null) {
                    //        decimal peiValue = 0;
                    //        decimal.TryParse(pei.Value, out peiValue);
                    //        basicValue -= k * peiValue;
                    //        if (peiValue > 0) {
                    //            PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                    //            ppist.PaymentID = paidFirstRate.ID;
                    //            ppist.InsuranceSubTypeID = c.InsuranceSubTypeID;
                    //            ppist.PaidValue = k * peiValue;
                    //            ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                    //            ppist.Insert();
                    //        }
                    //    }
                    //}
                    //if (basicValue > 0) {
                    //    PaymentsPerInsSubType ppist = new PaymentsPerInsSubType();
                    //    ppist.PaymentID = paidFirstRate.ID;
                    //    ppist.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                    //    ppist.PaidValue = basicValue;
                    //    ppist.BrokerageValue = ppist.PaidValue * pi.BrokeragePercentage / 100;
                    //    ppist.Insert();
                    //}
                    if ((bool)pi.IsFactureCreated) {
                        decimal brokerageSum = 0;
                        brokerageSum += pi.BrokerageValue;
                        List<PolicyItem> listPolicyItems = new List<PolicyItem>();
                        listPolicyItems.Add(pi);
                        Broker.DataAccess.Facture f = Broker.DataAccess.Facture.GenerateForSinglePolicy(p);
                        f.BrokerageValue = brokerageSum;
                        Broker.DataAccess.Facture.Insert(f);
                        List<FactureItem> lFI = Broker.DataAccess.Facture.GenerateFactureItemsForSinglePolicy(listPolicyItems, f);
                        Broker.DataAccess.Facture.InsertFactureItems(lFI);
                        FactureID = f.ID;
                        PolicyItemFactureItem.InsertForSingleFacture(lFI[0], pi);
                        InsertClientInFinansovo(p.ClientID);
                    }
                }

            }
            PolicyID = p.ID;
            ViewState["vsPolicyItemID"] = pi.ID;
            p.PolicyItems.Add(pi);
        }
    }

    void  InsertClientInFinansovo(int clientID){
        PolicyController.InsertClientInFinansovo(clientID);
    }

    protected void rblPaymentTypeForOncePayment_SelectedIndexChanged(object sender, EventArgs e) {
        Panel PaymentTypePanel = (Panel)PoliciesDetailsView.FindControl("PaymentTypePanel");
        RadioButtonList rblPaymentTypeForOncePayment = (RadioButtonList)sender;
        TextBox tbCash = (TextBox)PaymentTypePanel.FindControl("tbCash");
        TextBox tbCreditCard = (TextBox)PaymentTypePanel.FindControl("tbCreditCard");
        DropDownList ddlBank = (DropDownList)PaymentTypePanel.FindControl("ddlBank");
        DropDownList ddlCardTypes = (DropDownList)PaymentTypePanel.FindControl("ddlCardTypes");
        CheckBox cvIsForFacturingOncePaid = PaymentTypePanel.FindControl("cvIsForFacturingOncePaid") as CheckBox;
        if (rblPaymentTypeForOncePayment.SelectedValue == "CashPayment") {
            tbCash.Enabled = true;
            tbCreditCard.Enabled = true;
            ddlBank.Enabled = true;
            ddlCardTypes.Enabled = true;
            cvIsForFacturingOncePaid.Visible = false;
        } else if (rblPaymentTypeForOncePayment.SelectedValue == "FacturePayment") {
            tbCash.Enabled = false;
            tbCreditCard.Enabled = false;
            ddlBank.Enabled = false;
            ddlCardTypes.Enabled = false;
            cvIsForFacturingOncePaid.Visible = true;
        }
    }

    protected void btnPrintAnexDealFactureForRates_Click(object sender, EventArgs e) {
        if (ViewState["FactureID"] != null) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(Convert.ToInt32(ViewState["FactureID"]));
            PrintFactures.PrintAnex(f);
        }
    }

    protected void btnPrintAnexDealPaidInInsuranceCompany_Click(object sender, EventArgs e) {
        if (ViewState["vsPolicyItemID"] != null) {
            PolicyItem pi = PolicyItem.Get(Convert.ToInt32(ViewState["vsPolicyItemID"]));
            PrintFactures.PrintAnex(pi.Policy);
        }
    }

    protected void btnPrintFacture_Click(object sender, EventArgs e) {
        if (ViewState["FactureID"] != null) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(Convert.ToInt32(ViewState["FactureID"]));
            PrintFactures.PrintFacture(f);
        }
    }

    protected void btnPrintBillInfoForPolicy_Click(object sender, EventArgs e) {
        if (ViewState["vsPolicyItemID"] != null) {
            PolicyItem pi = PolicyItem.Get(Convert.ToInt32(ViewState["vsPolicyItemID"]));
            PrintBillInfo.PrintBillInfoForPolicy(pi);
        }
    }

    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
        if ((ViewState["ClientID"] == null) || (ViewState["OwnerID"] == null)) {
            RegisterStartupScript("myAlert", "<script>alert('НЕ Е ИЗБРАН ДОГОВОРУВАЧ ИЛИ ОСИГУРЕНИК!')</script>");
            return;
        }
        
        Policy p = e.InputParameters["entityToInsert"] as Policy;
        //CheckBox cb = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        bool isCashPaid = true;
        RadioButtonList rblPaymentTypes = (RadioButtonList)PoliciesDetailsView.FindControl("rblPaymentTypes");
        if (rblPaymentTypes.SelectedValue == "PaidOnce") {
            Panel PaymentTypePanel = (Panel)PoliciesDetailsView.FindControl("PaymentTypePanel");
            RadioButtonList rblPaymentTypeForOncePayment = (RadioButtonList)PaymentTypePanel.FindControl("rblPaymentTypeForOncePayment");
            if (rblPaymentTypeForOncePayment.SelectedValue == "CashPayment") {
                isCashPaid = true;
            } else {
                isCashPaid = false;
            }
        } else {
            isCashPaid = false;
        }
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        if (rblPaymentTypes.SelectedValue == "PaidRates") {
            Panel pnlRates = (Panel)PoliciesDetailsView.FindControl("pnlRates");
            DropDownList ddlNumberOfRates = (DropDownList)pnlRates.FindControl("ddlNumberOfRates");
            if (ddlNumberOfRates.SelectedValue == "1") {
                //RegisterStartupScript("myAlert", "<script>alert('МОРА ДА СЕ ИЗБЕРАТ БАРЕМ 2 РАТИ!')</script>");
                //return;
                p.ValidationErrors.Add("RateNumberValidator", "Мора да се изберат барем 2 рати!");
            }
            decimal totalRateValues = 0;
            foreach (Rate r in RatesInGridView) {
                totalRateValues += r.Value;
            }
            TextBox tbValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
            decimal totalValue = decimal.Parse(tbValue.Text);
            if (totalRateValues != totalValue) {
                p.ValidationErrors.Add("RateTotValuesValidator", "Погрешни вредности на ратите - вкупниот збир не одговара на внесената премија за наплата!");
            }
        }
        bool isEqualValue = true;
        if (isCashPaid) {
            TextBox tbCash = PoliciesDetailsView.FindControl("tbCash") as TextBox;
            TextBox tbCreditCard = PoliciesDetailsView.FindControl("tbCreditCard") as TextBox;
            decimal cashValue = 0;
            decimal cardValue = 0;
            if (tbCash.Text.Trim() != string.Empty) {
                cashValue = decimal.Parse(tbCash.Text);
            }
            if (tbCreditCard.Text.Trim() != string.Empty) {
                cardValue = decimal.Parse(tbCreditCard.Text);
            }

            TextBox tbValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
            decimal totalValue = decimal.Parse(tbValue.Text);
            if (totalValue != cardValue + cashValue) {
                isEqualValue = false;
            }
            p.RatesApproverID = null;
        } else {
            DropDownList ddlApproverOfRates = pnlExtendControls.FindControl("ddlApproverOfRates") as DropDownList;
            p.RatesApproverID = int.Parse(ddlApproverOfRates.SelectedValue);
        }
        if (!isEqualValue) {
            p.ValidationErrors.Add("ValuesValidator", "Погрешни вредности за сумите");
        }
        DropDownList ddlMarketingAgent = PoliciesDetailsView.FindControl("ddlMarketingAgents") as DropDownList;
        DropDownList ddlInsuranceSubTypes = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
        if (int.Parse(ddlMarketingAgent.SelectedValue) == 0) {
            p.MarketingAgentID = null;

        } else {
            p.MarketingAgentID = int.Parse(ddlMarketingAgent.SelectedValue);

        }
        p.StatusID = Statuse.GetByCode(Statuse.ZAVRSENA_POLISA).ID; ;
        //p.ApplicationDate = DateTime.Today;
        p.InsertDate = DateTime.Now;
        p.BranchID = this.PageUser.BranchID;
        p.ClientID = (int)ViewState["ClientID"];
        p.OwnerID = (int)ViewState["OwnerID"];
        p.UserID = this.PageUser.ID;
        TextBox tbPolicyNumber = PoliciesDetailsView.FindControl("tbPolicyNumber") as TextBox;
        p.PolicyNumber = tbPolicyNumber.Text;
        p.ValidateSinglePolicyNumber(p.InsuranceCompanyID, InsuranceSubtypeID, p.PolicyNumber, p.BranchID);
    }

    protected void btnAttachments_Click(object sender, EventArgs e) {

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
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
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
            decimal brokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
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

    void SetPaidAndToPaidValues() {
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        TextBox tbPolicyValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
        TextBox tbRealPolicyValue = pnlExtendControls.FindControl("tbRealPolicyValue1") as TextBox;
        TextBox tbFinDiscountValue = pnlExtendControls.FindControl("tbFinDiscountValue1") as TextBox;
        TextBox tbPaidValue = pnlExtendControls.FindControl("tbPaidValue1") as TextBox;
        TextBox tbToPaidValue = pnlExtendControls.FindControl("tbToPaidValue1") as TextBox;
        RadioButtonList rblPaymentTypes = (RadioButtonList)PoliciesDetailsView.FindControl("rblPaymentTypes");
        if (rblPaymentTypes.SelectedValue == "PaidOnce") {
            tbPaidValue.Text = tbPolicyValue.Text;
            tbToPaidValue.Text = "0";
        } else {
            tbPaidValue.Text = "0";
            tbToPaidValue.Text = "0";
        }
    }

    protected void tbRealPolicyValue_TextChanged(object sender, EventArgs e) {
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        TextBox tbPolicyValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
        TextBox tbRealPolicyValue = pnlExtendControls.FindControl("tbRealPolicyValue1") as TextBox;
        TextBox tbFinDiscountValue = pnlExtendControls.FindControl("tbFinDiscountValue1") as TextBox;
        decimal realPolicyValue = 0;
        decimal discount = 0;
        decimal.TryParse(tbFinDiscountValue.Text, out discount);
        decimal.TryParse(tbRealPolicyValue.Text, out realPolicyValue);
        tbPolicyValue.Text = RateController.Scale5(realPolicyValue - (discount / 100) * realPolicyValue).ToString();
        SetPaidAndToPaidValues();
        tbFinDiscountValue.Focus();
    }

    protected void tbFinDiscountValue_TextChanged(object sender, EventArgs e) {
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        TextBox tbPolicyValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
        TextBox tbRealPolicyValue = pnlExtendControls.FindControl("tbRealPolicyValue1") as TextBox;
        TextBox tbFinDiscountValue = pnlExtendControls.FindControl("tbFinDiscountValue1") as TextBox;
        decimal realPolicyValue = 0;
        decimal discount = 0;
        decimal.TryParse(tbFinDiscountValue.Text, out discount);
        decimal.TryParse(tbRealPolicyValue.Text, out realPolicyValue);
        tbPolicyValue.Text = RateController.Scale5(realPolicyValue - (discount / 100) * realPolicyValue).ToString();
        SetPaidAndToPaidValues();
        tbPolicyValue.Focus();
    }

    private List<Label> lblFieldNames;
    private List<TextBox> tbValues;
    protected override void CreateChildControls() {
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        int j = 0;
        List<Broker.DataAccess.Control> listControls = Broker.DataAccess.Control.GetByInsuranceSubType(InsuranceSubtypeID).OrderBy(c => c.OrderNumber).ToList();

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
        lblPremiumBrokerageTitle.Text = "Премија";
        firstCellFirstRowInTitleTable.Controls.Add(lblPremiumBrokerageTitle);


        HtmlTable defaultTable = new HtmlTable();
        defaultTable.Width = "348px";

        TextBox tbPolicyValue = new TextBox();
        tbPolicyValue.ID = "tbPolicyValue" + (j + 1).ToString();
        tbPolicyValue.CssClass = "tekstPole";

        HtmlTableRow realPolicyDefaultTableRow = new HtmlTableRow();
        HtmlTableCell fifthCellFourthRowInDefaultTable = new HtmlTableCell();
        fifthCellFourthRowInDefaultTable.Width = "200px";
        Label lblRealPolicyValue = new Label();
        lblRealPolicyValue.ID = "lblRealPolicyValue" + (j + 1).ToString();
        lblRealPolicyValue.Text = "Полисирана премија";
        fifthCellFourthRowInDefaultTable.Controls.Add(lblRealPolicyValue);
        HtmlTableCell sixthCellFourthRowInDefaultTable = new HtmlTableCell();
        sixthCellFourthRowInDefaultTable.Width = "148px";
        TextBox tbRealPolicyValue = new TextBox();
        tbRealPolicyValue.ID = "tbRealPolicyValue" + (j + 1).ToString();
        tbRealPolicyValue.AutoPostBack = true;
        tbRealPolicyValue.TextChanged += new EventHandler(tbRealPolicyValue_TextChanged);
        tbRealPolicyValue.CssClass = "tekstPole";
        RequiredFieldValidator rfvRealPolicyValue = new RequiredFieldValidator();
        rfvRealPolicyValue.ID = "rfvRealPolicyValue" + (j + 1).ToString();
        rfvRealPolicyValue.ErrorMessage = "*";
        rfvRealPolicyValue.Display = ValidatorDisplay.Dynamic;
        rfvRealPolicyValue.ControlToValidate = tbRealPolicyValue.ID;
        CompareValidator cvRealPolicyValue = new CompareValidator();
        cvRealPolicyValue.ID = "cvRealPolicyValue" + (j + 1).ToString();
        cvRealPolicyValue.ErrorMessage = "*";
        cvRealPolicyValue.Display = ValidatorDisplay.Dynamic;
        cvRealPolicyValue.ControlToValidate = tbRealPolicyValue.ID;
        cvRealPolicyValue.Operator = ValidationCompareOperator.DataTypeCheck;
        cvRealPolicyValue.Type = ValidationDataType.Double;
        CompareValidator cvGERealPolicyValue = new CompareValidator();
        cvGERealPolicyValue.ID = "cvGERealPolicyValue" + (j + 1).ToString();
        cvGERealPolicyValue.ErrorMessage = "*";
        cvGERealPolicyValue.Display = ValidatorDisplay.Dynamic;
        cvGERealPolicyValue.ControlToValidate = tbRealPolicyValue.ID;
        cvGERealPolicyValue.ControlToCompare = tbPolicyValue.ID;
        cvGERealPolicyValue.Operator = ValidationCompareOperator.GreaterThanEqual;
        cvGERealPolicyValue.Type = ValidationDataType.Double;
        sixthCellFourthRowInDefaultTable.Controls.Add(tbRealPolicyValue);
        sixthCellFourthRowInDefaultTable.Controls.Add(rfvRealPolicyValue);
        sixthCellFourthRowInDefaultTable.Controls.Add(cvRealPolicyValue);
        sixthCellFourthRowInDefaultTable.Controls.Add(cvGERealPolicyValue);
        realPolicyDefaultTableRow.Cells.Add(fifthCellFourthRowInDefaultTable);
        realPolicyDefaultTableRow.Cells.Add(sixthCellFourthRowInDefaultTable);
        defaultTable.Rows.Add(realPolicyDefaultTableRow);

        HtmlTableRow finDiscountPolicyDefaultTableRow = new HtmlTableRow();
        HtmlTableCell fifthCellfinDiscountInDefaultTable = new HtmlTableCell();
        Label lblFinDiscount = new Label();
        lblFinDiscount.ID = "lblFinDiscount" + (j + 1).ToString();
        lblFinDiscount.Text = "Финансиски попуст (%)";
        fifthCellfinDiscountInDefaultTable.Controls.Add(lblFinDiscount);
        HtmlTableCell sixthCellfinDiscountInDefaultTable = new HtmlTableCell();
        TextBox tbFinDiscountValue = new TextBox();
        tbFinDiscountValue.ID = "tbFinDiscountValue" + (j + 1).ToString();
        tbFinDiscountValue.CssClass = "tekstPole";
        tbFinDiscountValue.AutoPostBack = true;
        tbFinDiscountValue.TextChanged += new EventHandler(tbFinDiscountValue_TextChanged);
        sixthCellfinDiscountInDefaultTable.Controls.Add(tbFinDiscountValue);
        finDiscountPolicyDefaultTableRow.Cells.Add(fifthCellfinDiscountInDefaultTable);
        finDiscountPolicyDefaultTableRow.Cells.Add(sixthCellfinDiscountInDefaultTable);
        defaultTable.Rows.Add(finDiscountPolicyDefaultTableRow);

        HtmlTableRow fourthDefaultTableRow = new HtmlTableRow();
        fourthDefaultTableRow.BgColor = "#FAFAF8";
        defaultTable.Rows.Add(fourthDefaultTableRow);
        HtmlTableCell firstCellFourthRowInDefaultTable = new HtmlTableCell();
        firstCellFourthRowInDefaultTable.Width = "160px";
        fourthDefaultTableRow.Cells.Add(firstCellFourthRowInDefaultTable);
        Label lblPolicyValue = new Label();
        lblPolicyValue.ID = "lblPolicyValue" + (j + 1).ToString();
        lblPolicyValue.Text = "Премија за наплата";
        firstCellFourthRowInDefaultTable.Controls.Add(lblPolicyValue);
        HtmlTableCell secondCellFourthRowInDefaultTable = new HtmlTableCell();
        secondCellFourthRowInDefaultTable.Width = "187px";
        fourthDefaultTableRow.Cells.Add(secondCellFourthRowInDefaultTable);
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

        HtmlTableRow paidValuePolicyDefaultTableRow = new HtmlTableRow();
        HtmlTableCell fifthCellpaidValueInDefaultTable = new HtmlTableCell();
        Label lblPaidValue = new Label();
        lblPaidValue.ID = "lblPaidValue" + (j + 1).ToString();
        lblPaidValue.Text = "Уплатено";
        fifthCellpaidValueInDefaultTable.Controls.Add(lblPaidValue);
        HtmlTableCell sixthCellpaidValueInDefaultTable = new HtmlTableCell();
        TextBox tbPaidValue = new TextBox();
        tbPaidValue.ID = "tbPaidValue" + (j + 1).ToString();
        tbPaidValue.CssClass = "tekstPole";
        tbPaidValue.ReadOnly = true;
        sixthCellpaidValueInDefaultTable.Controls.Add(tbPaidValue);
        paidValuePolicyDefaultTableRow.Cells.Add(fifthCellpaidValueInDefaultTable);
        paidValuePolicyDefaultTableRow.Cells.Add(sixthCellpaidValueInDefaultTable);
        defaultTable.Rows.Add(paidValuePolicyDefaultTableRow);

        HtmlTableRow toPaidValuePolicyDefaultTableRow = new HtmlTableRow();
        HtmlTableCell fifthCelltoPaidValueInDefaultTable = new HtmlTableCell();
        Label lblToPaidValue = new Label();
        lblToPaidValue.ID = "lblToPaidValue" + (j + 1).ToString();
        lblToPaidValue.Text = "Должна премија";
        fifthCelltoPaidValueInDefaultTable.Controls.Add(lblToPaidValue);
        HtmlTableCell sixthCelltoPaidValueInDefaultTable = new HtmlTableCell();
        TextBox tbToPaidValue = new TextBox();
        tbToPaidValue.ID = "tbToPaidValue" + (j + 1).ToString();
        tbToPaidValue.CssClass = "tekstPole";
        tbToPaidValue.ReadOnly = true;
        sixthCelltoPaidValueInDefaultTable.Controls.Add(tbToPaidValue);
        toPaidValuePolicyDefaultTableRow.Cells.Add(fifthCelltoPaidValueInDefaultTable);
        toPaidValuePolicyDefaultTableRow.Cells.Add(sixthCelltoPaidValueInDefaultTable);
        defaultTable.Rows.Add(toPaidValuePolicyDefaultTableRow);

        Panel pnlPolicyDetailedInformations = new Panel();
        pnlPolicyDetailedInformations.ID = "pnlPolicyDetailedInformations";
        //pnlPolicyDetailedInformations.CssClass = "sec_policy";

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
        pnlPolicyDetailedInformations.Controls.Add(titlePolicyTable);



        HtmlTable table = new HtmlTable();
        if (listControls.Where(c => c.ColumnNumber != 1).ToList().Count > 0) {
            table.Width = "695px";
        } else {
            table.Width = "347px";
        }
        int counter = 0;
        foreach (Broker.DataAccess.Control c in listControls) {
            if (c.IsActive) {
                if (listControls.Count == 1) {
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
                    if (c.FieldType.Name == FieldType.TEXTBOX) {
                        TextBox textbox = new TextBox();
                        textbox.CssClass = "tekstPole";
                        textbox.ID = c.TextBoxID;
                        if (c.IsLatin != null) {
                            if ((bool)c.IsLatin == true) {
                                textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                            }
                        }
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
                        if (c.HasCompareValidator) {
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
                    if (c.FieldType.Name == FieldType.DROPDOWNLIST) {
                        DropDownList dropdownlist = new DropDownList();
                        dropdownlist.ID = c.TextBoxID;
                        dropdownlist.DataTextField = "Value";
                        dropdownlist.DataValueField = "ID";
                        dropdownlist.CssClass = "select";
                        List<ControlBindableValue> cbvList = ControlBindableValue.GetActiveByControlID(c.ID);
                        dropdownlist.DataSource = cbvList;
                        tableCellSecond.Controls.Add(dropdownlist);
                    }
                    if (c.FieldType.Name == FieldType.CHECKBOX) {
                        CheckBox checkBox = new CheckBox();
                        checkBox.ID = c.TextBoxID;
                        tableCellSecond.Controls.Add(checkBox);
                    }
                    HtmlTableCell tableCellThird = new HtmlTableCell();
                    tableCellThird.Width = "160px";
                    tableRow.Cells.Add(tableCellThird);
                    HtmlTableCell tableCellForth = new HtmlTableCell();
                    tableCellForth.Width = "187px";
                    tableRow.Cells.Add(tableCellForth);
                } else {
                    if (c.ColumnNumber == 1) {
                        HtmlTableRow tableRow = new HtmlTableRow();
                        if ((counter % 4 == 0) || (counter % 4 == 1)) {
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
                        if (c.FieldType.Name == FieldType.TEXTBOX) {

                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID;
                            textbox.CssClass = "tekstPole";
                            if (c.IsLatin != null) {
                                if ((bool)c.IsLatin == true) {
                                    textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                                }
                            }
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
                            if (c.HasCompareValidator) {
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
                        if (c.FieldType.Name == FieldType.DROPDOWNLIST) {
                            DropDownList dropdownlist = new DropDownList();
                            dropdownlist.ID = c.TextBoxID;
                            dropdownlist.DataTextField = "Value";
                            dropdownlist.DataValueField = "ID";
                            dropdownlist.CssClass = "select";
                            List<ControlBindableValue> cbvList = ControlBindableValue.GetActiveByControlID(c.ID);
                            dropdownlist.DataSource = cbvList;
                            dropdownlist.DataBind();
                            tableCellSecond.Controls.Add(dropdownlist);
                        }
                        if (c.FieldType.Name == FieldType.CHECKBOX) {
                            CheckBox checkBox = new CheckBox();
                            checkBox.ID = c.TextBoxID;
                            tableCellSecond.Controls.Add(checkBox);
                        }
                    } else if (c.ColumnNumber == 2) {
                        HtmlTableRow tableRow = table.Rows[counter / 2];
                        if ((counter % 4 == 0) || (counter % 4 == 1)) {
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
                        if (c.FieldType.Name == FieldType.TEXTBOX) {
                            TextBox textbox = new TextBox();
                            textbox.ID = c.TextBoxID;
                            textbox.CssClass = "tekstPole";
                            tbValues.Add(textbox);
                            if (c.IsLatin != null) {
                                if ((bool)c.IsLatin == true) {
                                    textbox.Attributes.Add("onkeyup", "toEnglish(this.id)");
                                }
                            }
                            tableCellForth.Controls.Add(textbox);
                            if (c.HasRequredFieldValidator) {
                                RequiredFieldValidator rfv = new RequiredFieldValidator();
                                rfv.ID = "rfv" + textbox.ID;
                                rfv.ErrorMessage = "*";
                                rfv.Display = ValidatorDisplay.Dynamic;
                                rfv.ControlToValidate = textbox.ID;
                                tableCellForth.Controls.Add(rfv);
                            }
                            if (c.HasCompareValidator) {
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

                        if (c.FieldType.Name == FieldType.DROPDOWNLIST) {
                            DropDownList dropdownlist = new DropDownList();
                            dropdownlist.ID = c.TextBoxID;
                            dropdownlist.DataTextField = "Value";
                            dropdownlist.DataValueField = "ID";
                            List<ControlBindableValue> cbvList = ControlBindableValue.GetActiveByControlID(c.ID);
                            dropdownlist.DataSource = cbvList;
                            dropdownlist.DataBind();
                            tableCellForth.Controls.Add(dropdownlist);
                        }
                        if (c.FieldType.Name == FieldType.CHECKBOX) {
                            CheckBox checkBox = new CheckBox();
                            checkBox.ID = c.TextBoxID;
                            tableCellForth.Controls.Add(checkBox);
                        }
                    }
                }
                counter++;
            }
        }
        pnlExtendControls.Controls.Add(pnlPolicyDetailedInformations);
        pnlPolicyDetailedInformations.Controls.Add(table);
        pnlExtendControls.Controls.Add(titleTable);
        pnlExtendControls.Controls.Add(defaultTable);
        //CheckBox cbIsPaidInCash = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        //if (!cbIsPaidInCash.Checked) {

        //}

    }

    protected void tbStartDate_TextChanged(object sender, EventArgs e) {
        TextBox tbStartDate = (TextBox)sender;
        TextBox tbEndDate = (TextBox)PoliciesDetailsView.FindControl("tbEndDate");
        try {
            DateTime startDate = Convert.ToDateTime(tbStartDate.Text);
            tbEndDate.Text = startDate.AddYears(1).ToShortDateString();
            tbEndDate.Focus();
            return;
        } catch {
        }
        try {
            DateTime startDate = new DateTime(Convert.ToInt32(tbStartDate.Text.Substring(4, 4)), Convert.ToInt32(tbStartDate.Text.Substring(2, 2)), Convert.ToInt32(tbStartDate.Text.Substring(0, 2)));
            tbStartDate.Text = startDate.ToShortDateString();
            tbEndDate.Text = startDate.AddYears(1).ToShortDateString();
            tbEndDate.Focus();
        } catch {
        }
    }

    protected void tbEndDate_TextChanged(object sender, EventArgs e) {
        TextBox tbEndDate = (TextBox)sender;
        TextBox tbApplicationDate = (TextBox)PoliciesDetailsView.FindControl("tbApplicationDate");
        try {
            DateTime endDate = new DateTime(Convert.ToInt32(tbEndDate.Text.Substring(4, 4)), Convert.ToInt32(tbEndDate.Text.Substring(2, 2)), Convert.ToInt32(tbEndDate.Text.Substring(0, 2)));
            tbEndDate.Text = endDate.ToShortDateString();
            tbApplicationDate.Focus();
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

    void buttonCalculate_Click(object sender, EventArgs e) {
        List<Rate> lRates = new List<Rate>();
        decimal totalPolicyCost = 0;
        List<decimal> brokeragePercentages = new List<decimal>();
        List<decimal> policyItemValues = new List<decimal>();
        List<decimal> policyItemRatios = new List<decimal>();
        decimal brokerageValue = 0;
        int j = 1;
        Broker.DataAccess.Parameter minPercetageForFirstRatePar = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZADOLZITELEN_PROCENT_ZA_PRVA_RATA);
        decimal minPercetageForFirstRate = Convert.ToDecimal(minPercetageForFirstRatePar.Value);
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        Panel pnlRates = PoliciesDetailsView.FindControl("pnlRates") as Panel;
        TextBox tbPolicyValue = (TextBox)pnlExtendControls.FindControl("tbPolicyValue" + j.ToString());
        decimal policyValue = 0;
        decimal.TryParse(tbPolicyValue.Text, out policyValue);
        decimal minValueForFirstRate = RateController.Scale5(policyValue * minPercetageForFirstRate / 100);
        totalPolicyCost += policyValue;
        policyItemValues.Add(policyValue);
        TextBox tbBrokeragePercentage = (TextBox)pnlExtendControls.FindControl("tbBrokeragePercentage" + j.ToString());
        //brokeragePercentages.Add(Convert.ToDecimal(tbBrokeragePercentage.Text));
        decimal brPercentage = 1;
        Brokerage b = Brokerage.GetByInsuranceSubTypeAndDeal(InsuranceSubtypeID, DealID);
        if (b != null) {
            brPercentage = b.PercentageForPrivates;
        }
        brokeragePercentages.Add(brPercentage);
        TextBox tbBrokerageValue = (TextBox)pnlExtendControls.FindControl("tbBrokerageValue" + j.ToString());
        //brokerageValue += Convert.ToDecimal(tbBrokerageValue.Text);
        brokerageValue += (totalPolicyCost / brPercentage);
        //da se izbegne delenje so nula!!!
        if (totalPolicyCost > 0) {
            policyItemRatios.Add(policyItemValues[0] / totalPolicyCost);
        } else {
            policyItemRatios.Add(policyItemValues[0] / 1);
        }

        Dictionary<int, decimal> oldDictionary = new Dictionary<int, decimal>();
        oldDictionary = (Dictionary<int, decimal>)ViewState["MyDictionary"];
        Dictionary<int, decimal> newDictionary = new Dictionary<int, decimal>();
        foreach (Rate r in RatesInGridView) {
            newDictionary.Add(r.Number, r.Value);
        }
        if (newDictionary.Count > 0) {
            if (newDictionary[1] < minValueForFirstRate) {
                RegisterStartupScript("myAlert", "<script>alert('Првата рата мора да биде најмалку " + minPercetageForFirstRate + " проценти од премијата!')</script>");
                return;
            }
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
        decimal currValue = 0;
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
                if (newRate.Number == RatesInGridView.Count) {
                    newRate.Value = totalPolicyCost - currValue;
                } else {
                    newRate.Value = valuePerRateForNonChanged;
                }
            }
            currValue += newRate.Value;
            decimal brokerageValuePerRate = 0;
            for (int k = 0; k <= brokeragePercentages.Count - 1; k++) {
                brokerageValuePerRate += RateController.Scale5(policyItemRatios[k] * (brokeragePercentages[k] / 100) * newRate.Value);
            }
            newRate.BrokerageValue = brokerageValuePerRate;
            lRates.Add(newRate);
            t++;
        }

        TextBox tbPaidValue = (TextBox)pnlExtendControls.FindControl("tbPaidValue1");
        TextBox tbToPaidValue = (TextBox)pnlExtendControls.FindControl("tbToPaidValue1");
        if (lRates.Count == 1) {
            tbPaidValue.Text = "0";
            tbToPaidValue.Text = policyValue.ToString();
        } else if (lRates.Count > 1) {
            tbPaidValue.Text = lRates[0].Value.ToString();
            tbToPaidValue.Text = (policyValue - lRates[0].Value).ToString();
        }

        //GridView GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
        GridView GridViewRates = (GridView)pnlRates.FindControl("GridViewRates");
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



    void ddlRateDealInsuranceSubTypes_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlRateDealInsuranceSubTypes = (DropDownList)sender;
    }

    protected void ddlRateDeals_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlRateDeals = (DropDownList)sender;
        DropDownList ddlInsuranceSubTypes = (DropDownList)PoliciesDetailsView.FindControl("ddlInsuranceSubTypes");
        Panel pnlExtendControls = (Panel)PoliciesDetailsView.FindControl("pnlExtendControls");
        TextBox tbPolicyValue = (TextBox)pnlExtendControls.FindControl("tbPolicyValue1");
        decimal policyValue = 0;
        if (tbPolicyValue != null) {
            decimal.TryParse(tbPolicyValue.Text, out policyValue);
        }
        Panel pnlRates = PoliciesDetailsView.FindControl("pnlRates") as Panel;
        DropDownList ddlNumberOfRates = (DropDownList)pnlRates.FindControl("ddlNumberOfRates");
        if (ddlRateDeals.SelectedValue != null) {
            int ddlRateDealSelValue = 0;
            int.TryParse(ddlRateDeals.SelectedValue, out ddlRateDealSelValue);
            if (ddlRateDealSelValue > 0) {
                ddlNumberOfRates.SelectedValue = RateDealInsuranceSubType.GetByRateDealInsuranceSubTypePremiumValue(ddlRateDealSelValue, Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue), policyValue).NumberOfRates.ToString();
                ddlNumberOfRates.Enabled = false;
                ddlNumberOfRates_SelectedIndexChanged(ddlNumberOfRates, null);
            } else {
                ddlNumberOfRates.Enabled = true;
            }
        }
    }

    protected void ddlNumberOfRates_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlNumberOfRates = (DropDownList)sender;
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        Panel pnlRates = PoliciesDetailsView.FindControl("pnlRates") as Panel;
        Broker.DataAccess.Parameter minPercetageForFirstRatePar = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZADOLZITELEN_PROCENT_ZA_PRVA_RATA);
        decimal minPercetageForFirstRate = Convert.ToDecimal(minPercetageForFirstRatePar.Value);
        List<Rate> lRates = new List<Rate>();
        TextBox tbPolicyNumber = (TextBox)pnlExtendControls.FindControl("tbPolicyNumber");
        string policyNumber = tbPolicyNumber.Text;

        TextBox tbPolicyValue = (TextBox)pnlExtendControls.FindControl("tbPolicyValue1");
        decimal policyValue = 0;
        if (tbPolicyValue.Text != string.Empty) {
            policyValue = Convert.ToDecimal(tbPolicyValue.Text);
        }
        decimal minValueForFirstRate = RateController.Scale5(policyValue * minPercetageForFirstRate / 100);
        //DateTime startDate = policy.ApplicationDate;
        TextBox tbStartDateForRates = pnlExtendControls.FindControl("tbStartDateForRates") as TextBox;
        DateTime startDate = new DateTime();
        if (tbStartDateForRates == null) {
            startDate = DateTime.MinValue;
        } else {
            try {
                startDate = new DateTime(Convert.ToInt32(tbStartDateForRates.Text.Substring(4, 4)), Convert.ToInt32(tbStartDateForRates.Text.Substring(2, 2)), Convert.ToInt32(tbStartDateForRates.Text.Substring(0, 2)));
            } catch {
                DateTime.TryParse(tbStartDateForRates.Text, out startDate);
            }
        }

        if (startDate == DateTime.MinValue) {
            startDate = DateTime.Today;
        }
        decimal totalPolicyCost = policyValue;
        int ratesNumber = 1;
        if (ddlNumberOfRates != null) {
            ratesNumber = Convert.ToInt32(ddlNumberOfRates.SelectedValue);
        }

        decimal defaultValueForRate = RateController.Scale5(totalPolicyCost / ratesNumber);
        decimal valueForCorrection = 0;
        for (int i = 0; i < ratesNumber; i++) {
            Rate r = new Rate();
            r.Number = i + 1;
            r.Date = startDate.AddMonths(i);
            if (r.Number == 1) {
                if (minValueForFirstRate > defaultValueForRate) {
                    r.Value = minValueForFirstRate;
                } else {
                    r.Value = defaultValueForRate;
                }
                valueForCorrection += r.Value;
            } else if (r.Number >= 2 && r.Number < ratesNumber) {
                if (minValueForFirstRate > defaultValueForRate) {
                    r.Value = RateController.Scale5((totalPolicyCost - minValueForFirstRate) / (ratesNumber - 1));
                } else {
                    r.Value = defaultValueForRate;
                }
                valueForCorrection += r.Value;
            } else if (r.Number == ratesNumber) {
                r.Value = totalPolicyCost - valueForCorrection;
            }

            int index = Convert.ToInt32(tbPolicyValue.ID.Substring(tbPolicyValue.ID.Length - 1));

            //TextBox tbBrokeragePercentage = (TextBox)tbPolicyValue.Parent.FindControl("tbBrokeragePercentage" + index.ToString());
            //decimal brokeragePercentage = Convert.ToDecimal(tbBrokeragePercentage.Text);
            decimal brokeragePercentage = 0;
            Deal d = Deal.Get(DealID);
            Brokerage b = Brokerage.GetByInsuranceSubTypeAndDeal(InsuranceSubtypeID, DealID);
            if (b != null) {
                brokeragePercentage = b.PercentageForPrivates;
            }

            r.BrokerageValue = RateController.Scale5((brokeragePercentage / 100) * r.Value);

            ///Ova da se proveri!!!
            r.PolicyItemID = 0;
            //r.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
            lRates.Add(r);
        }

        TextBox tbPaidValue = (TextBox)pnlExtendControls.FindControl("tbPaidValue1");
        TextBox tbToPaidValue = (TextBox)pnlExtendControls.FindControl("tbToPaidValue1");
        if (lRates.Count == 1) {
            tbPaidValue.Text = "0";
            tbToPaidValue.Text = policyValue.ToString();
        } else if (lRates.Count > 1) {
            tbPaidValue.Text = lRates[0].Value.ToString();
            tbToPaidValue.Text = (policyValue - lRates[0].Value).ToString();
        }

        //GridView GridViewRates = (GridView)pnlExtendControls.FindControl("GridViewRates");
        GridView GridViewRates = (GridView)pnlRates.FindControl("GridViewRates");
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


    protected void btnOfferNumber_Click(object sender, EventArgs e) {

    }
    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
    }
    protected void dvDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e) {

    }

    protected void odsRateDeals_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
        DropDownList ddlInsuranceSubTypes = (DropDownList)PoliciesDetailsView.FindControl("ddlInsuranceSubTypes");
        Panel pnlExtendControls = (Panel)PoliciesDetailsView.FindControl("pnlExtendControls");
        TextBox tbPolicyValue = (TextBox)pnlExtendControls.FindControl("tbPolicyValue1");
        TextBox tbApplicationDate = (TextBox)PoliciesDetailsView.FindControl("tbApplicationDate");
        DateTime applicationDate = DateTime.Today;
        DateTime.TryParse(tbApplicationDate.Text, out applicationDate);
        decimal policyValue = 0;
        if (tbPolicyValue != null) {
            decimal.TryParse(tbPolicyValue.Text, out policyValue);
        }
        e.InputParameters.Clear();
        e.InputParameters.Add("insuranceSubTypeID", Convert.ToInt32(ddlInsuranceSubTypes.SelectedValue));
        e.InputParameters.Add("premiumValue", policyValue);
        if (applicationDate >= DateTime.Today.AddYears(-1)) {
            e.InputParameters.Add("applicationDate", applicationDate);
        } else {
            e.InputParameters.Add("applicationDate", DateTime.Today);
        }
    }

    protected void DateChanged(object sender, EventArgs e) {

        //Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        Panel pnlRates = PoliciesDetailsView.FindControl("pnlRates") as Panel;
        DropDownList ddlNumberOfRates = pnlRates.FindControl("ddlNumberOfRates") as DropDownList;
        ddlNumberOfRates_SelectedIndexChanged(ddlNumberOfRates, null);
    }

    protected void Checked_Changed(object sender, EventArgs e) {
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        TextBox tbPolicyValue = pnlExtendControls.FindControl("tbPolicyValue1") as TextBox;
        TextBox tbRealPolicyValue = pnlExtendControls.FindControl("tbRealPolicyValue1") as TextBox;
        TextBox tbFinDiscountValue = pnlExtendControls.FindControl("tbFinDiscountValue1") as TextBox;
        RadioButtonList rblPaymentTypes = (RadioButtonList)sender;
        Panel panel = PoliciesDetailsView.FindControl("PaymentTypePanel") as Panel;
        Panel pnlRates = PoliciesDetailsView.FindControl("pnlRates") as Panel;
        if (rblPaymentTypes.SelectedValue == "PaidOnce") {
            panel.Visible = true;
            pnlRates.Visible = false;
            tbFinDiscountValue.ReadOnly = false;
        } else if (rblPaymentTypes.SelectedValue == "PaidRates") {
            panel.Visible = false;
            pnlRates.Visible = true;
            DropDownList ddlRateDeals = (DropDownList)pnlRates.FindControl("ddlRateDeals");
            ddlRateDeals.DataBind();
            DropDownList ddlNumberOfRates = pnlRates.FindControl("ddlNumberOfRates") as DropDownList;
            ddlNumberOfRates_SelectedIndexChanged(ddlNumberOfRates, null);
            tbPolicyValue.Text = tbRealPolicyValue.Text;
            tbFinDiscountValue.ReadOnly = true;
            tbFinDiscountValue.Text = "0";
        } else if (rblPaymentTypes.SelectedValue == "PaidInInsuranceCompany") {
            panel.Visible = false;
            pnlRates.Visible = true;
            DropDownList ddlRateDeals = (DropDownList)pnlRates.FindControl("ddlRateDeals");
            ddlRateDeals.DataBind();
            DropDownList ddlNumberOfRates = pnlRates.FindControl("ddlNumberOfRates") as DropDownList;
            ddlNumberOfRates_SelectedIndexChanged(ddlNumberOfRates, null);
            tbFinDiscountValue.ReadOnly = false;
        }

        //CreateChildControls();
        try {
            btnSearcPreviousPolicy_Click(null, null);
        } catch {

        }
    }
    protected void PolicyDetailesView_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {

            if (PolicyID != 0) {
                Policy p = Policy.Get(PolicyID);
                if (p.PolicyItems[0].IsPaidInBrokerHouse) {
                    lblFeedBack.Text = "Снимена е полиса со број " + p.PolicyItems[0].PolicyNumber + " со наплата во брокерско друшво";
                } else {
                    lblFeedbackRates.Text = "Снимена е полиса со број " + p.PolicyItems[0].PolicyNumber + " со наплата директно во о.компанија";
                }
                RadioButtonList rblPaymentTypes = (RadioButtonList)PoliciesDetailsView.FindControl("rblPaymentTypes");
                if (rblPaymentTypes.SelectedValue == "PaidOnce") {
                    PolicyMultiView.SetActiveView(BillView);
                    Panel PaymentTypePanel = (Panel)PoliciesDetailsView.FindControl("PaymentTypePanel");
                    RadioButtonList rblPaymentTypeForOncePayment = (RadioButtonList)PaymentTypePanel.FindControl("rblPaymentTypeForOncePayment");
                    if (rblPaymentTypeForOncePayment.SelectedValue == "CashPayment") {
                        pnlBillCash.Visible = true;
                        pnlBillFacture.Visible = false;
                    } else {
                        pnlBillCash.Visible = false;
                        pnlBillFacture.Visible = true;
                    }
                } else if (rblPaymentTypes.SelectedValue == "PaidRates") {
                    PolicyMultiView.SetActiveView(FactureRatesView);
                } else if (rblPaymentTypes.SelectedValue == "PaidInInsuranceCompany") {
                    PolicyMultiView.SetActiveView(viewPaidInInsuranceCompany);
                }
            } else {
                //Response.Redirect(Request.Path);
            }
        }

    }

    protected void btnPrintFactureForRates_Click(object sender, EventArgs e) {
        if (FactureID > 0) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(FactureID);
            PrintFactures.PrintFacture(f);
        }
    }

    protected void PolicyDetailesView_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        if ((ViewState["ClientID"] == null) || (ViewState["OwnerID"] == null)) {
            RegisterStartupScript("myAlert", "<script>alert('НЕ Е ИЗБРАН ДОГОВОРУВАЧ ИЛИ ОСИГУРЕНИК!')</script>");
            e.Cancel = true;
            return;
        }
    }
    protected void btnPrintBill_Click(object sender, EventArgs e) {
        btnDiscardBill.Enabled = true;
        btnPrintBill.Enabled = false;
    }

    protected void btnDiscardBill_Click(object sender, EventArgs e) {
        if (PolicyID != 0) {

        }
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

    void CreateGridView(object sender, EventArgs e) {
        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
        Panel pnlRates = PoliciesDetailsView.FindControl("pnlRates") as Panel;
        //CheckBox cbIsPaidInCash = PoliciesDetailsView.FindControl("cbPaymentPlace") as CheckBox;
        RadioButtonList rblPaymentTypes = (RadioButtonList)PoliciesDetailsView.FindControl("rblPaymentTypes");
        bool isCashPaid = true;
        if (rblPaymentTypes.SelectedValue == "PaidOnce") {
            Panel PaymentTypePanel = (Panel)PoliciesDetailsView.FindControl("PaymentTypePanel");
            RadioButtonList rblPaymentTypeForOncePayment = (RadioButtonList)PaymentTypePanel.FindControl("rblPaymentTypeForOncePayment");
            if (rblPaymentTypeForOncePayment.SelectedValue == "CashPayment") {
                isCashPaid = true;
            } else {
                isCashPaid = false;
            }
        } else {
            isCashPaid = false;
        }
        if (isCashPaid == false) {
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
            tfPaymentType.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "PaymentTypeID", true, true);
            tfPaymentType.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Тип на плаќање", true, true);
            GridViewRates.Columns.Add(tfPaymentType);
            TemplateField tfBankCreditCard = new TemplateField();
            tfBankCreditCard.ItemTemplate = new DynamicGridViewTemplate(ListItemType.Item, "BankCreditCardID", true, true);
            tfBankCreditCard.HeaderTemplate = new DynamicGridViewTemplate(ListItemType.Header, "Тип на картица", true, true);
            GridViewRates.Columns.Add(tfBankCreditCard);
            //pnlExtendControls.Controls.Add(GridViewRates);
            pnlRates.Controls.Add(GridViewRates);

            Button buttonCalculate = new Button();
            buttonCalculate.ID = "btnCalculate";
            buttonCalculate.Text = "Пресметај";
            buttonCalculate.CssClass = "gray_button";
            buttonCalculate.CausesValidation = false;
            buttonCalculate.Click += new EventHandler(buttonCalculate_Click);
            //pnlExtendControls.Controls.Add(buttonCalculate);
            pnlRates.Controls.Add(buttonCalculate);
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

    protected void btnDiscard_Click(object sender, EventArgs e) {
        TextBox tbPolicyNumber = PoliciesDetailsView.FindControl("tbPolicyNumber") as TextBox;
        DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;
        Policy p = new Policy();
        p.ValidateSinglePolicyNumber(int.Parse(ddlInsuranceCompany.SelectedValue), InsuranceSubtypeID, tbPolicyNumber.Text.Trim(), this.PageUser.BranchID);
        if (p.ValidationErrors.Count != 0) {
            string message = p.ValidationErrors["PolicyNumberDistributionValidator"].ErrorMessage;
            RegisterStartupScript("myAlert", "<script>alert('" + message + "')</script>");

        } else {
            p.PolicyNumber = tbPolicyNumber.Text.Trim();
            Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(Broker.DataAccess.Client.DISCARD_EMBG);
            p.ApplicationDate = DateTime.Today;
            p.BranchID = this.PageBranch.ID;
            p.BrokeragePercentage = 0;
            p.ClientID = c.ID;
            p.Discard = true;
            p.EndDate = DateTime.Today;
            p.InsuranceCompanyID = int.Parse(ddlInsuranceCompany.SelectedValue);
            p.StartDate = DateTime.Today;
            p.InsertDate = DateTime.Today;
            p.StatusID = Statuse.GetByCode(Statuse.ZAVRSENA_POLISA).ID;
            p.TotalSum = 0;
            p.UserID = this.PageUser.ID;
            p.OwnerID = c.ID;
            p.Insert();

            PolicyItem pi = new PolicyItem();
            pi.BrokeragePercentage = 0;
            pi.BrokerageValue = 0;
            pi.InsuranceSubTypeID = InsuranceSubtypeID;
            pi.IsPaidInBrokerHouse = true;
            pi.IsRollBacked = false;
            pi.PolicyID = p.ID;
            pi.PolicyNumber = p.PolicyNumber;
            pi.PremiumValue = 0;
            pi.StatusID = Statuse.GetByCode(Statuse.ZAVRSENA_POLISA).ID;
            pi.Insert();

            //CashPayment cp = new CashPayment();
            //cp.IsFactured = false;
            //cp.PolicyID = pi.ID;
            //cp.Discarded = true;
            //cp.SerialNumber = CashPayment.GenerateNextNumber(this.PageUser);
            //cp.UserID = this.PageUser.ID;
            //cp.IsCashReported = false;
            //cp.Insert();

            //CashPaymentItem cpi = new CashPaymentItem();
            //cpi.BankCreditCardID = null;
            //cpi.CashPaymentID = cp.ID;
            //cpi.PaymentTypeID = PaymentType.GetByCode(PaymentType.CASH).ID;
            //cpi.Value = 0;
            //cpi.Insert();

            List<Broker.DataAccess.Control> controlsList = Broker.DataAccess.Control.GetActiveByInsuranceSubType(InsuranceSubtypeID);
            foreach (Broker.DataAccess.Control control in controlsList) {
                PolicyExtendInformation pei = new PolicyExtendInformation();
                ValidationDataType vdt = Broker.DataAccess.VariableType.GetForVariableType(control.VariableTypeID);
                if (vdt == ValidationDataType.Double || vdt == ValidationDataType.Integer || vdt == ValidationDataType.Currency) {
                    pei.ControlID = control.ID;
                    pei.PolicyItemID = pi.ID;
                    pei.Value = "0";
                    pei.Insert();
                }
                if (vdt == ValidationDataType.Date) {
                    pei.ControlID = control.ID;
                    pei.PolicyItemID = pi.ID;
                    pei.Value = DateTime.Today.ToShortDateString();
                    pei.Insert();

                }
                if (vdt == ValidationDataType.String) {
                    pei.ControlID = control.ID;
                    pei.PolicyItemID = pi.ID;
                    pei.Value = string.Empty;
                    pei.Insert();
                }

            }
            Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(InsuranceSubtypeID, pi.PolicyNumber, int.Parse(ddlInsuranceCompany.SelectedValue));
            d.IsUsed = true;
            Distribution.Table.Context.SubmitChanges();
            PolicyMultiView.SetActiveView(BillView);
            lblFeedBack.Text = "Снимена е сторно полиса со број " + p.PolicyNumber;
            btnDiscardPolicy.Visible = false;
        }
    }

    protected void btnNBOSearch_Click(object sender, EventArgs e) {
        TextBox tbPolicyNumber = (TextBox)PoliciesDetailsView.FindControl("tbPolicyNumber");
        string policyNumber = tbPolicyNumber.Text;
        TemporaryPolicy tp = TemporaryPolicy.GetByPolicyNumber(policyNumber);
        if (tp != null) {
            InsuranceSubType insuranceSubTypeAO = InsuranceSubType.GetByCode(InsuranceSubType.AO);

            DropDownList ddlInsuranceType = PoliciesDetailsView.FindControl("ddlInsuranceType") as DropDownList;
            ddlInsuranceType.SelectedValue = insuranceSubTypeAO.InsuranceTypeID.ToString();
            ddlInsuranceType.DataBind();

            DropDownList ddlInsuranceSubTypes = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
            ddlInsuranceSubTypes.DataBind();
            ddlInsuranceSubTypes.SelectedValue = insuranceSubTypeAO.ID.ToString();
            ddlInsuranceSubTypes.DataBind();

            DropDownList ddlDeals = PoliciesDetailsView.FindControl("ddlDeals") as DropDownList;
            DropDownList ddlInsuranceCompany = PoliciesDetailsView.FindControl("ddlInsuranceCompany") as DropDownList;

            FakeBinder.SimulateBindingContext(this.Page, ddlInsuranceCompany);
            ddlInsuranceCompany.DataBind();
            ddlDeals.DataBind();

            ddlInsuranceType.Enabled = false;
            ddlInsuranceSubTypes.Enabled = false;


            TextBox tbClientName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
            tbClientName.Text = tp.ClientName;
            Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
            TextBox tbEndDate = PoliciesDetailsView.FindControl("tbEndDate") as TextBox;
            TextBox tbStartDate = PoliciesDetailsView.FindControl("tbStartDate") as TextBox;

            tbStartDate.Text = tp.StartDate.ToShortDateString();
            tbEndDate.Text = tp.EndDate.ToShortDateString();
        }

    }


    protected void btnGetByRegNumber_Click(object sender, EventArgs e) {

        if (InsuranceSubtypeID == InsuranceSubType.GetByCode(InsuranceSubType.AO).ID) {
            TextBox tbRegistrationNumber = PoliciesDetailsView.FindControl("tbRegNumberForSearch") as TextBox;
            try {
                GetByRegistrationNumber.WebServiceGetVehicleInformation wsgvi = new GetByRegistrationNumber.WebServiceGetVehicleInformation();
                GetByRegistrationNumber.VehicleInformations vi = wsgvi.GetByRegistrationNumber(tbRegistrationNumber.Text.Trim());
                if (vi != null) {
                    Broker.DataAccess.Municipality m = Broker.DataAccess.Municipality.GetByCode(vi.ClientMunicipalityCode);
                    if (m == null) {
                        m = new Broker.DataAccess.Municipality();
                        m.Code = vi.ClientMunicipalityCode;
                        m.IsActive = true;
			m.Name = vi.ClientMunicipalityName;
            m.StateID = Broker.DataAccess.State.GetByCode(Broker.DataAccess.State.MACEDONIA).ID;
                        m.Insert();
                    }
                    Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(vi.ClientEMBG);
                    if (c == null) {
                        c = new Broker.DataAccess.Client();
                        c.EMBG = vi.ClientEMBG;
                        c.Address = vi.ClientAddress;
                        c.Name = vi.ClientName;
                        c.PlaceID = Broker.DataAccess.Place.GetFirstyMunicipality(m.ID).ID;
                        c.Insert();
                    }
                    ViewState["OwnerID"] = c.ID;
                    ViewState["ClientID"] = c.ID;
                    TextBox tbOwnerEmbg = PoliciesDetailsView.FindControl("tbOwnerEMBG") as TextBox;
                    TextBox tbOwnerName = PoliciesDetailsView.FindControl("tbOwnerName") as TextBox;
                    tbOwnerEmbg.Text = c.EMBG;
                    tbOwnerName.Text = c.Name;

                    DropDownList ddlInsuranceSubTypes = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
                    ddlInsuranceSubTypes.Enabled = false;
                    DropDownList ddlInsuranceType = PoliciesDetailsView.FindControl("ddlInsuranceType") as DropDownList;
                    ddlInsuranceType.Enabled = false;
                    //int insuranceSubTypeID = int.Parse(ddlInsuranceSubTypes.SelectedValue);
                    //List<Broker.DataAccess.Control> cList = Broker.DataAccess.Control.GetActiveByInsuranceSubType(insuranceSubTypeID);
                    TextBox tbClientEmbg = PoliciesDetailsView.FindControl("tbClientEmbg") as TextBox;
                    TextBox tbClientName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
                    tbClientEmbg.Text = c.EMBG;
                    tbClientName.Text = c.Name;
                    Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                    TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                    tbRegNumber.Text = vi.VehicleRegistrationNumber;
                    TextBox tbChassisNumber = pnlExtendControls.FindControl("tbChassisNumber") as TextBox;
                    tbChassisNumber.Text = vi.VehicleChassisNumber;
                    //TextBox tbVehicleType = pnlExtendControls.FindControl("tbVehicleType") as TextBox;
                    //tbVehicleType.Text = vi.VehicleTypeDescription;
                    TextBox tbModel = pnlExtendControls.FindControl("tbModel") as TextBox;
                    tbModel.Text = vi.VehicleModel;
                    TextBox tbBrand = pnlExtendControls.FindControl("tbBrand") as TextBox;
                    tbBrand.Text = vi.VehicleBrand;
                    TextBox tbPower = pnlExtendControls.FindControl("tbPower") as TextBox;
                    tbPower.Text = vi.VehiclePower;
                    TextBox tbCarring = pnlExtendControls.FindControl("tbCarring") as TextBox;
                    tbCarring.Text = vi.VehicleCarrying;
                    TextBox tbVolume = pnlExtendControls.FindControl("tbVolume") as TextBox;
                    tbVolume.Text = vi.VehicleVolume;
                    TextBox tbPassingers = pnlExtendControls.FindControl("tbPassingers") as TextBox;
                    tbPassingers.Text = vi.VehiclePassengers;

                    TextBox tbEndDate = PoliciesDetailsView.FindControl("tbEndDate") as TextBox;
                    TextBox tbStartDate = PoliciesDetailsView.FindControl("tbStartDate") as TextBox;

                    tbStartDate.Text = DateTime.Parse(vi.VehicleExpirationDate).AddYears(-1).ToShortDateString();
                    tbEndDate.Text = DateTime.Parse(vi.VehicleExpirationDate).ToShortDateString();
                } else {
                    Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                    TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                    tbRegNumber.Text = tbRegistrationNumber.Text;
                }

            } catch {
                RegisterStartupScript("myAlert", "<script>alert('НЕМА КОМУНИКАЦИЈА СО СЕРВЕРОТ!')</script>");
                Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                tbRegNumber.Text = tbRegistrationNumber.Text;
            }

        }

        if (InsuranceSubtypeID == InsuranceSubType.GetByCode(InsuranceSubType.ZK).ID) {
            TextBox tbRegistrationNumber = PoliciesDetailsView.FindControl("tbRegNumberForSearch") as TextBox;
            try {
                PolicyExtendInformation pei = PolicyExtendInformation.GetByValue(tbRegistrationNumber.Text);
                if (pei != null) {
                    Broker.DataAccess.Client client = pei.PolicyItem.Policy.Client;
                    Broker.DataAccess.Client owner = pei.PolicyItem.Policy.Client1;
                    ViewState["ClientID"] = client.ID;
                    ViewState["OwnerID"] = owner.ID;
                    TextBox tbClientEmbg = PoliciesDetailsView.FindControl("tbClientEmbg") as TextBox;
                    TextBox tbClientName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
                    tbClientEmbg.Text = client.EMBG;
                    tbClientName.Text = client.Name;
                    TextBox tbOwnerEmbg = PoliciesDetailsView.FindControl("tbOwnerEMBG") as TextBox;
                    TextBox tbOwnerName = PoliciesDetailsView.FindControl("tbOwnerName") as TextBox;
                    tbOwnerEmbg.Text = owner.EMBG;
                    tbOwnerName.Text = owner.Name;
                    DropDownList ddlInsuranceSubTypes = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
                    ddlInsuranceSubTypes.Enabled = false;
                    DropDownList ddlInsuranceType = PoliciesDetailsView.FindControl("ddlInsuranceType") as DropDownList;
                    ddlInsuranceType.Enabled = false;
                    Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                    TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                    tbRegNumber.Text = PolicyExtendInformation.GetByPolicyItemAndControl(pei.PolicyItemID, Broker.DataAccess.Control.GetByInsuranceSubTypeAndTexbBoxID(pei.PolicyItem.InsuranceSubTypeID, Broker.DataAccess.Control.REGISTRATION_NUMBER).ID).Value;
                    TextBox tbChassisNumber = pnlExtendControls.FindControl("tbChassisNumber") as TextBox;
                    tbChassisNumber.Text = PolicyExtendInformation.GetByPolicyItemAndControl(pei.PolicyItemID, Broker.DataAccess.Control.GetByInsuranceSubTypeAndTexbBoxID(pei.PolicyItem.InsuranceSubTypeID, Broker.DataAccess.Control.CHASSIS_NUMBER).ID).Value;
                    //TextBox tbVehicleType = pnlExtendControls.FindControl("tbVehicleType") as TextBox;
                    //tbVehicleType.Text = vi.VehicleTypeDescription;
                    TextBox tbModel = pnlExtendControls.FindControl("tbModel") as TextBox;
                    tbModel.Text = PolicyExtendInformation.GetByPolicyItemAndControl(pei.PolicyItemID, Broker.DataAccess.Control.GetByInsuranceSubTypeAndTexbBoxID(pei.PolicyItem.InsuranceSubTypeID, Broker.DataAccess.Control.MODEL).ID).Value;
                    TextBox tbBrand = pnlExtendControls.FindControl("tbBrand") as TextBox;
                    tbBrand.Text = PolicyExtendInformation.GetByPolicyItemAndControl(pei.PolicyItemID, Broker.DataAccess.Control.GetByInsuranceSubTypeAndTexbBoxID(pei.PolicyItem.InsuranceSubTypeID, Broker.DataAccess.Control.BRAND).ID).Value;
                } else {
                    try {
                        GetByRegistrationNumber.WebServiceGetVehicleInformation wsgvi = new GetByRegistrationNumber.WebServiceGetVehicleInformation();
                        GetByRegistrationNumber.VehicleInformations vi = wsgvi.GetByRegistrationNumber(tbRegistrationNumber.Text.Trim());
                        if (vi != null) {
                            Broker.DataAccess.Municipality m = Broker.DataAccess.Municipality.GetByCode(vi.ClientMunicipalityCode);
                            if (m == null) {
                                m = new Broker.DataAccess.Municipality();
                                m.Code = vi.ClientMunicipalityCode;
				m.Name = vi.ClientMunicipalityName;
                                m.IsActive = true;
                                m.StateID = Broker.DataAccess.State.GetByCode(Broker.DataAccess.State.MACEDONIA).ID;
                                m.Insert();
                            }
                            Broker.DataAccess.Client c = Broker.DataAccess.Client.GetByEmbg(vi.ClientEMBG);
                            if (c == null) {
                                c = new Broker.DataAccess.Client();
                                c.EMBG = vi.ClientEMBG;
                                c.Address = vi.ClientAddress;
                                c.Name = vi.ClientName;
                                c.PlaceID = Broker.DataAccess.Place.GetFirstyMunicipality(m.ID).ID;
                                c.Insert();
                            }
                            ViewState["OwnerID"] = c.ID;
                            ViewState["ClientID"] = c.ID;
                            TextBox tbOwnerEmbg = PoliciesDetailsView.FindControl("tbOwnerEMBG") as TextBox;
                            TextBox tbOwnerName = PoliciesDetailsView.FindControl("tbOwnerName") as TextBox;
                            tbOwnerEmbg.Text = c.EMBG;
                            tbOwnerName.Text = c.Name;

                            DropDownList ddlInsuranceSubTypes = PoliciesDetailsView.FindControl("ddlInsuranceSubTypes") as DropDownList;
                            ddlInsuranceSubTypes.Enabled = false;
                            DropDownList ddlInsuranceType = PoliciesDetailsView.FindControl("ddlInsuranceType") as DropDownList;
                            ddlInsuranceType.Enabled = false;
                            //int insuranceSubTypeID = int.Parse(ddlInsuranceSubTypes.SelectedValue);
                            //List<Broker.DataAccess.Control> cList = Broker.DataAccess.Control.GetActiveByInsuranceSubType(insuranceSubTypeID);
                            TextBox tbClientEmbg = PoliciesDetailsView.FindControl("tbClientEmbg") as TextBox;
                            TextBox tbClientName = PoliciesDetailsView.FindControl("tbClientName") as TextBox;
                            tbClientEmbg.Text = c.EMBG;
                            tbClientName.Text = c.Name;
                            Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                            TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                            tbRegNumber.Text = vi.VehicleRegistrationNumber;
                            TextBox tbChassisNumber = pnlExtendControls.FindControl("tbChassisNumber") as TextBox;
                            tbChassisNumber.Text = vi.VehicleChassisNumber;
                            //TextBox tbVehicleType = pnlExtendControls.FindControl("tbVehicleType") as TextBox;
                            //tbVehicleType.Text = vi.VehicleTypeDescription;
                            TextBox tbModel = pnlExtendControls.FindControl("tbModel") as TextBox;
                            tbModel.Text = vi.VehicleModel;
                            TextBox tbBrand = pnlExtendControls.FindControl("tbBrand") as TextBox;
                            tbBrand.Text = vi.VehicleBrand;

                            TextBox tbEndDate = PoliciesDetailsView.FindControl("tbEndDate") as TextBox;
                            TextBox tbStartDate = PoliciesDetailsView.FindControl("tbStartDate") as TextBox;

                            tbStartDate.Text = DateTime.Parse(vi.VehicleExpirationDate).AddYears(-1).ToShortDateString();
                            tbEndDate.Text = DateTime.Parse(vi.VehicleExpirationDate).ToShortDateString();
                        } else {
                            Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                            TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                            tbRegNumber.Text = tbRegistrationNumber.Text;
                        }
                    } catch {
                        RegisterStartupScript("myAlert", "<script>alert('НЕМА КОМУНИКАЦИЈА СО СЕРВЕРОТ!')</script>");
                        Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                        TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                        tbRegNumber.Text = tbRegistrationNumber.Text;
                    }

                }
            } catch {
                Panel pnlExtendControls = PoliciesDetailsView.FindControl("pnlExtendControls") as Panel;
                TextBox tbRegNumber = pnlExtendControls.FindControl("tbRegistrationNumber") as TextBox;
                tbRegNumber.Text = tbRegistrationNumber.Text;
            }
        }
    }

    protected void btnSearcPreviousPolicy_Click(object sender, EventArgs e) {
        Panel pnlSearchPreviosPolicy = PoliciesDetailsView.FindControl("pnlSearcPreviousPolicy") as Panel;
        TextBox tbSearchParameter = pnlSearchPreviosPolicy.FindControl("tbSearchParameters") as TextBox;
        RadioButtonList rblSearchParameter = pnlSearchPreviosPolicy.FindControl("rblSeachParameter") as RadioButtonList;
        DropDownList ddlFoundPolicyItems = pnlSearchPreviosPolicy.FindControl("ddlFoundPolicyItems") as DropDownList;
        int controlID = int.Parse(rblSearchParameter.SelectedValue);
        List<PolicyItem> piList = PolicyItem.SearchPolicyItem(controlID, tbSearchParameter.Text.Trim(), InsuranceSubtypeID);
        if (piList.Count == 1) {
            ddlFoundPolicyItems.Visible = false;
            FillPolicyExtendInfoermations(piList[0]);
        } else if (piList.Count > 0) {
            ddlFoundPolicyItems.Visible = true;
            ddlFoundPolicyItems.DataSource = piList;
            ddlFoundPolicyItems.DataBind();
            FillPolicyExtendInfoermations(piList[0]);
        } else if (piList.Count == 0) {
            ddlFoundPolicyItems.Visible = false;
            ddlFoundPolicyItems.DataSource = piList;
            ddlFoundPolicyItems.DataBind();
        }
    }

    protected void ddlFoundPolicyItems_SelectedIndexChanged(object sender, EventArgs e) {
        DropDownList ddlFoundPolicyItems = (DropDownList)sender;
        if (ddlFoundPolicyItems.SelectedValue != null) {
            if (ddlFoundPolicyItems.SelectedValue != string.Empty) {
                PolicyItem pi = PolicyItem.Get(int.Parse(ddlFoundPolicyItems.SelectedValue));
                FillPolicyExtendInfoermations(pi);
            }
        }
    }

    void FillPolicyExtendInfoermations(PolicyItem pi) {
        TextBox tbClientEMBG = (TextBox)PoliciesDetailsView.FindControl("tbClientEMBG");
        TextBox tbOwnerEMBG = (TextBox)PoliciesDetailsView.FindControl("tbOwnerEMBG");
        TextBox tbClientName = (TextBox)PoliciesDetailsView.FindControl("tbClientName");
        TextBox tbOwnerName = (TextBox)PoliciesDetailsView.FindControl("tbOwnerName");
        TextBox tbStartDate = (TextBox)PoliciesDetailsView.FindControl("tbStartDate");
        TextBox tbEndDate = (TextBox)PoliciesDetailsView.FindControl("tbEndDate");
        TextBox tbApplicationDate = (TextBox)PoliciesDetailsView.FindControl("tbApplicationDate");
        ViewState["ClientID"] = pi.Policy.ClientID;
        ViewState["OwnerID"] = pi.Policy.OwnerID;
        tbClientEMBG.Text = pi.Policy.Client.EMBG;
        tbOwnerEMBG.Text = pi.Policy.Client1.EMBG;
        tbClientName.Text = pi.Policy.Client.Name;
        tbOwnerName.Text = pi.Policy.Client1.Name;
        if (pi.Policy.EndDate >= DateTime.Today) {
            tbStartDate.Text = pi.Policy.EndDate.ToShortDateString();
            tbEndDate.Text = pi.Policy.EndDate.AddYears(1).ToShortDateString();
        } else {
            tbStartDate.Text = DateTime.Today.ToShortDateString();
            tbEndDate.Text = DateTime.Today.AddYears(1).ToShortDateString();
        }
        tbApplicationDate.Text = DateTime.Today.ToShortDateString();
        List<Broker.DataAccess.Control> controls = Broker.DataAccess.Control.GetByInsuranceSubType(InsuranceSubtypeID);
        Panel pnlExtendControls = (Panel)PoliciesDetailsView.FindControl("pnlExtendControls");
        foreach (Broker.DataAccess.Control c in controls) {
            PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ID);
            if (pei != null) {
                System.Web.UI.Control con = pnlExtendControls.FindControl(c.TextBoxID);
                if (con != null) {
                    if (c.FieldType.Name == FieldType.DROPDOWNLIST) {
                        DropDownList ddl = (DropDownList)con;
                        if (pei.ValueID != null) {
                            ddl.SelectedValue = ((int)pei.ValueID).ToString();
                        }
                    } else if (c.FieldType.Name == FieldType.TEXTBOX) {
                        TextBox tb = (TextBox)con;
                        tb.Text = pei.Value;
                    } else if (c.FieldType.Name == FieldType.CHECKBOX) {
                        CheckBox checkBox = (CheckBox)con;
                        checkBox.Checked = Convert.ToBoolean(pei.Value);
                    }
                }
            }

        }
    }
}

