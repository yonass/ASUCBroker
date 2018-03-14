using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.FacturesControllers;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;

public partial class BROKERAdmin_BrokerageSlips : AuthenticationPage {

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
    public List<PolicyForSlipsInfo> listPolicyForSlipsInfos {
        get {
            if (ViewState["ListFactureInfos"] == null) {
                return new List<PolicyForSlipsInfo>();
            } else {
                return (List<PolicyForSlipsInfo>)ViewState["ListFactureInfos"];
            }
        }
        set {
            ViewState["ListFactureInfos"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            GXGridView1.TotalRecords = ViewBrokerageSlip.SelectCountCached();
            mvMain.SetActiveView(viewGrid);
            reportControl.BranchName = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            reportControl.CompanyName = "Брокерско друштво";
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewSearch);
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj_Active";
    }
    protected void btnNew_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewNew);
        btnNew.CssClass = "novZapis_Active";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void btnPreview_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewGrid);
        SearchControl1.SearchArguments = null;
        GXGridView1.DataSourceID = odsGridView.ID;
        GXGridView1.DataBind();
        btnNew.CssClass = "novZapis";
        btnPreview.CssClass = "osvezi_Active";
        btnReport.CssClass = "izvestaj";
        btnSearch.CssClass = "prebaraj";
    }
    protected void SearchControl1_Search(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewBrokerageSlip.SelectSearchCountCached(SearchControl1.SearchArguments);
        mvMain.SetActiveView(viewGrid);
    }
    protected void GXGridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
        if (e.CommandName == "SingleClick") {
            //btnFactureItems.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
            GXGridView1SelectedValue = Convert.ToInt32(GXGridView1.SelectedValue);
        }
        if (e.CommandName == "DoubleClick") {
            //btnDelete.Enabled = true;
            //btnFactureItems.Enabled = true;
            int selIndex = Convert.ToInt32(e.CommandArgument);
            GXGridView1.SelectedIndex = selIndex;
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        mvMain.SetActiveView(viewReport);
        btnNew.CssClass = "novZapis";
        //btnDelete.CssClass = "izbrisi";
        btnPreview.CssClass = "osvezi";
        btnReport.CssClass = "izvestaj_Active";
        btnSearch.CssClass = "prebaraj";
    }
    protected void FilterControl1_Filter(object sender, EventArgs e) {
        GXGridView1.TotalRecords = ViewBrokerageSlip.SelectFilterCountCached(FilterControl1.FCFilterArgument);
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e) {
        if (e.CommandName == "Cancel") {
            mvMain.SetActiveView(viewGrid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e) {
        if (GXGridView1.SelectedIndex != -1) {
            //mvMain.SetActiveView(viewFactureItems);
            btnNew.CssClass = "novZapis";
            //btnDelete.CssClass = "izbrisi_Active";
            btnPreview.CssClass = "osvezi";
            btnReport.CssClass = "izvestaj";
            btnSearch.CssClass = "prebaraj";
        }


    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewBrokerageSlip.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
        if (e.Exception != null) {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            ValidationUtility.ShowValidationErrors(this, e.Exception);
        } else {
            GXGridView1.TotalRecords = ViewBrokerageSlip.SelectCountCached();
            GXGridView1.DataBind();
            mvMain.SetActiveView(viewGrid);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
        GXGridView1.TotalRecords = ViewBrokerageSlip.SelectCountCached();
        GXGridView1.DataBind();
        mvMain.SetActiveView(viewGrid);
    }

    protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e) {
        e.Cancel = true;
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e) {
        //e.Values["UserID"] = this.PageUser.ID;
    }

    protected void dvDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e) {
    }

    protected void dvDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {

    }
    protected void dvDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e) {
    }

    protected void dvDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {

    }


    protected void btnCheck_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbFromDate.Text);
        DateTime toDate = DateTime.Parse(tbToDate.Text);
        List<PolicyItem> pList = PolicyItem.GetNonBrokerageSliped(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), fromDate, toDate);
        List<PolicyForSlipsInfo> fInfos = new List<PolicyForSlipsInfo>();
        foreach (PolicyItem pi in pList) {
            PolicyForSlipsInfo pfsi = new PolicyForSlipsInfo();
            pfsi.ID = pi.ID;
            pfsi.BrokerageID = pi.BrokerageID;
            pfsi.PacketBrokerageID = pi.PacketBrokerageID;
            pfsi.InsuranceSubTypeID = pi.InsuranceSubTypeID;
            pfsi.IsForFacturing = true;
            pfsi.PolicyNumber = pi.PolicyNumber;
            fInfos.Add(pfsi);
        }
        listPolicyForSlipsInfos= fInfos;
        gvNewPolicies.DataSource = fInfos;
        gvNewPolicies.DataBind();
    }

    protected void gvNewPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        List<PolicyForSlipsInfo> lastPolicyForSlipsInfos = listPolicyForSlipsInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            PolicyForSlipsInfo pfsi = lastPolicyForSlipsInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
            pfsi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listPolicyForSlipsInfos = lastPolicyForSlipsInfos;
        gvNewPolicies.PageIndex = e.NewPageIndex;
        gvNewPolicies.DataSource = lastPolicyForSlipsInfos;
        gvNewPolicies.DataBind();
    }

    protected void btnCreate_Click(object sender, EventArgs e) {
        DateTime fromDate = DateTime.Parse(tbFromDate.Text);
        DateTime toDate = DateTime.Parse(tbToDate.Text);
        int insuranceCompanyID = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        List<PolicyForSlipsInfo> lastPolicyForSlipsInfos = listPolicyForSlipsInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            PolicyForSlipsInfo pfsi = lastPolicyForSlipsInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
            pfsi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listPolicyForSlipsInfos = lastPolicyForSlipsInfos;
        List<PolicyForSlipsInfo> listFactureInfo = listPolicyForSlipsInfos.Where(c => c.IsForFacturing == true).ToList();
        if (listFactureInfo.Count > 0) {
            BrokerageSlip bs = new BrokerageSlip();
            bs.BasisForCompensation = tbBasisForCompensation.Text;
            bs.Date = DateTime.Today;
            bs.Description = tbDescription.Text;
            bs.FromDate = fromDate;
            bs.InsuranceCompanyID = insuranceCompanyID;
            bs.InsuranceRisks = tbInsuranceRisks.Text;
            bs.InsuranceThings = tbInsuranceThings.Text;
            if (listFactureInfo.Count == 1) {
                PolicyItem tmpPol = PolicyItem.Get(listFactureInfo[0].ID);
                if (tmpPol != null) {
                    bs.Insurer = tmpPol.Policy.Client1.Name;
                } else {
                    bs.Insurer = "ПО ПРИЛОГ ПОЛИСИ";
                }
            } else {
                bs.Insurer = "ПО ПРИЛОГ ПОЛИСИ";
            }
            bs.Number = BrokerageSlip.GetNextNumber(insuranceCompanyID);
            bs.OtherConditions = tbOtherConditions.Text;
            bs.ToDate = toDate;
            bs.Insert();
            Dictionary<int, List<decimal>> dic = new Dictionary<int, List<decimal>>();
            foreach (PolicyForSlipsInfo pfsi in listFactureInfo) {
                if (!dic.Keys.Contains(pfsi.InsuranceSubTypeID)) {
                    List<decimal> brokPercentages = new List<decimal>();
                    if (pfsi.BrokerageID != null) {
                        Brokerage brokerage = Brokerage.Get(pfsi.BrokerageID);
                        brokPercentages.Add(brokerage.PercentageForPrivates);
                        brokPercentages.Add(brokerage.PercentageForLaws);
                    } else if (pfsi.PacketBrokerageID != null) {
                        PacketsInsuranceSubType pist = PacketsInsuranceSubType.Get(pfsi.PacketBrokerageID);
                        brokPercentages.Add(pist.BrokeragePecentageForPrivates);
                        brokPercentages.Add(pist.BrokeragePecentageForLaws);
                    }
                    dic.Add(pfsi.InsuranceSubTypeID, brokPercentages);
                }
                BrokerageSlipPolicyItem bspi = new BrokerageSlipPolicyItem();
                bspi.BrokerageSlipID = bs.ID;
                bspi.PolicyItemID = pfsi.ID;
                bspi.Insert();
                PolicyItem pItem = PolicyItem.Get(pfsi.ID);
                pItem.IsBrokerageSliped = true;
                PolicyItem.Table.Context.SubmitChanges();
            }
            foreach (KeyValuePair<int, List<decimal>> kvPair in dic) {
                BrokerageSlipInsuranceSubType bsist = new BrokerageSlipInsuranceSubType();
                bsist.BrokerageSlipID = bs.ID;
                bsist.BrokeragePercentagePrivates = kvPair.Value[0];
                bsist.BrokeragePercentageLaws = kvPair.Value[1];
                bsist.InsuranceSubTypeID = kvPair.Key;
                bsist.Insert();
            }

            btnPrBrokSlip.Enabled = true;
            ViewState["BrokerageSlipID"] = bs.ID;
            GXGridView1SelectedValue = bs.ID;
        }
    }


    protected void btnPrintBrokerageSlip_Click(object sender, EventArgs e) {
        int id = GXGridView1SelectedValue;
        if (id > 0) {
            BrokerageSlip bs = BrokerageSlip.Get(id);
            PrintBrokerageSlips.PrintBrokerageSlip(bs);
        }
    }

    protected void btnPrBrokSlip_Click(object sender, EventArgs e) {
        if (ViewState["BrokerageSlipID"] != null) {
            BrokerageSlip bs = BrokerageSlip.Get(Convert.ToInt32(ViewState["BrokerageSlipID"]));
            PrintBrokerageSlips.PrintBrokerageSlip(bs);
        }
    }
}



