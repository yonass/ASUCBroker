using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;
using Broker.Controllers.DistributionControllers;

public partial class SEAdmin_RollBackDistributionByInsuranceSubType : AuthenticationPage {

    public List<RollBackDistributionInfo> listOldRollBackInfos {
        get {
            if (ViewState["ListRollBackInfos"] == null) {
                return new List<RollBackDistributionInfo>();
            } else {
                return (List<RollBackDistributionInfo>)ViewState["ListRollBackInfos"];
            }
        }
        set {
            ViewState["ListRollBackInfos"] = value;
        }
    }

    public List<RollBackDistributionInfo> selectedOldRollBackInfos {
        get {
            if (ViewState["selectedListRollBackInfos"] == null) {
                return new List<RollBackDistributionInfo>();
            } else {
                return (List<RollBackDistributionInfo>)ViewState["selectedListRollBackInfos"];
            }
        }
        set {
            ViewState["selectedListRollBackInfos"] = value;
        }
    }

    public List<RollBackDistributionInfo> listNewRollBackInfos {
        get {
            if (ViewState["ListNewRollBackInfos"] == null) {
                return new List<RollBackDistributionInfo>();
            } else {
                return (List<RollBackDistributionInfo>)ViewState["ListNewRollBackInfos"];
            }
        }
        set {
            ViewState["ListNewRollBackInfos"] = value;
        }
    }

    public List<RollBackDistributionInfo> selectedNewRollBackInfos {
        get {
            if (ViewState["selectedListNewRollBackInfos"] == null) {
                return new List<RollBackDistributionInfo>();
            } else {
                return (List<RollBackDistributionInfo>)ViewState["selectedListNewRollBackInfos"];
            }
        }
        set {
            ViewState["selectedListNewRollBackInfos"] = value;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void btnCreateClick(object sender, EventArgs e) {
        lblFeedBack.Visible = false;
        DateTime fromDate = DateTime.Parse(tbStartDate.Text);
        DateTime toDate = DateTime.Parse(tbEndDate.Text);
        int branchID = this.PageUser.BranchID;
        List<PolicyItem> oldPiList = PolicyItem.GetForRollBackFromPreviousMonths(fromDate, int.Parse(ddlInsuranceCompany.SelectedValue),branchID);
        List<RollBackDistributionInfo> oldPolicyInfos = new List<RollBackDistributionInfo>();
        List<InsuranceSubType> oldSubTypes = new List<InsuranceSubType>();
        InsuranceSubType dummy = new InsuranceSubType();
        dummy.Description = string.Empty; 
        dummy.ID = 0;
        oldSubTypes.Add(dummy);
        foreach (PolicyItem pi in oldPiList) {
            RollBackDistributionInfo rdbi = new RollBackDistributionInfo();
            rdbi.ID = pi.ID;
            rdbi.IsForRollBack = true;
            rdbi.PolicyNumber = pi.PolicyNumber;
            rdbi.InsuranceSubTypeID = pi.InsuranceSubTypeID;
            rdbi.BrokerageValue = pi.BrokerageValue;
            rdbi.PremiumValue = pi.PremiumValue;
            oldPolicyInfos.Add(rdbi);
            if (!oldSubTypes.Contains(pi.InsuranceSubType)) {
                oldSubTypes.Add(pi.InsuranceSubType);
            }
        }
        listOldRollBackInfos = oldPolicyInfos;
        selectedOldRollBackInfos = oldPolicyInfos;
        
        gvOldPolicies.DataSource = oldPolicyInfos;
        gvOldPolicies.DataBind();
        if (oldSubTypes.Count > 1) {
            ddlPreviosSubTypes.Visible = true;
            lblPreviosSubTypes.Visible = true;
            ddlPreviosSubTypes.DataSource = oldSubTypes.OrderBy(c=>c.ID);
            ddlPreviosSubTypes.DataBind();
        }

        List<PolicyItem> newPiList = PolicyItem.GetForRollBackFromCurrentMonth(fromDate, toDate, int.Parse(ddlInsuranceCompany.SelectedValue),branchID);
        List<RollBackDistributionInfo> newRollBackInfos = new List<RollBackDistributionInfo>();
        List<InsuranceSubType> newSubTypes = new List<InsuranceSubType>();
        newSubTypes.Add(dummy);
        foreach (PolicyItem pi in newPiList) {
            RollBackDistributionInfo rbdi = new RollBackDistributionInfo();
            rbdi.ID = pi.ID;
            rbdi.IsForRollBack = true;
            rbdi.PolicyNumber = pi.PolicyNumber;
            rbdi.BrokerageValue = pi.BrokerageValue;
            rbdi.InsuranceSubTypeID = pi.InsuranceSubTypeID;
            rbdi.PremiumValue = pi.PremiumValue;
            newRollBackInfos.Add(rbdi);
            if (!newSubTypes.Contains(pi.InsuranceSubType)) {
                newSubTypes.Add(pi.InsuranceSubType);
            }
        }
        if (newSubTypes.Count > 1) {
            ddlCurrentSubTypes.Visible = true;
            lblCurentSubTypes.Visible = true;
            ddlCurrentSubTypes.DataSource = newSubTypes.OrderBy(c=>c.ID);
            ddlCurrentSubTypes.DataBind();
        }
        listNewRollBackInfos = newRollBackInfos;
        selectedNewRollBackInfos = newRollBackInfos;
        gvNewPolicies.DataSource = newRollBackInfos;
        gvNewPolicies.DataBind();
        btnCreate.Visible = false;
        btnPrintPolicies.Visible = true;
        btnInsert.Visible = true;
    }
    protected void btnInsert_Click(object sender, EventArgs e) {

        //Check last page changed old policies 
        List<RollBackDistributionInfo> lastRBInfos = selectedOldRollBackInfos;
        List<RollBackDistributionInfo> totalInfos = listOldRollBackInfos;
        GridViewRowCollection displayedRows = gvOldPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbIsForRollBack") as CheckBox;
            int rollBackInfoID = Convert.ToInt32(gvOldPolicies.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionInfo fi = lastRBInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
            RollBackDistributionInfo trdi = totalInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            trdi.IsForRollBack = cbIsForRollBack.Checked;
        }
        selectedOldRollBackInfos = lastRBInfos;
        listOldRollBackInfos = totalInfos;

        //Check last page changed new policies 
        List<RollBackDistributionInfo> lastRBNewInfos = selectedNewRollBackInfos;
        List<RollBackDistributionInfo> totalNewInfos = listNewRollBackInfos;
        GridViewRowCollection displayedNewRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedNewRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbIsForRollBack") as CheckBox;
            int rollBackInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionInfo fi = lastRBNewInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
            RollBackDistributionInfo trdi = totalNewInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            trdi.IsForRollBack = cbIsForRollBack.Checked;
        }
        selectedNewRollBackInfos = lastRBNewInfos;
        listNewRollBackInfos = totalNewInfos;

        RollBackDistribution.InsertByInsuranceSubTypeAndCompanyFromBranch(int.Parse(ddlInsuranceCompany.SelectedValue), this.PageBranch.ID, this.PageUser.ID, listOldRollBackInfos, listNewRollBackInfos, DateTime.Parse(tbStartDate.Text), DateTime.Parse(tbEndDate.Text));
      //  RollBackDistributionController.CreateFileForSelectedPolicies(listOldRollBackInfos, listNewRollBackInfos, DateTime.Parse(tbStartDate.Text), DateTime.Parse(tbEndDate.Text), int.Parse(ddlInsuranceCompany.SelectedValue), this.PageBranch.ID);


    }
    protected void btnCreateRightRestrictions_Click(object sender, EventArgs e) {
    }
    protected void btnInsertRightRestrictions_Click(object sender, EventArgs e) {
    }
    protected void gvOldPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        List<RollBackDistributionInfo> lastRBInfos = selectedOldRollBackInfos;
        List<RollBackDistributionInfo> totalInfos = listOldRollBackInfos;
        GridViewRowCollection displayedRows = gvOldPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbIsForRollBack") as CheckBox;
            int rollBackInfoID = Convert.ToInt32(gvOldPolicies.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionInfo fi = lastRBInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
            RollBackDistributionInfo trdi = totalInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            trdi.IsForRollBack=cbIsForRollBack.Checked;
        }
        selectedOldRollBackInfos = lastRBInfos;
        listOldRollBackInfos = totalInfos;
        gvOldPolicies.PageIndex = e.NewPageIndex;
        gvOldPolicies.DataSource = lastRBInfos;
        gvOldPolicies.DataBind();
    }
    protected void gvNewPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        List<RollBackDistributionInfo> lastRBInfos = selectedNewRollBackInfos;
        List<RollBackDistributionInfo> totalNewInfos = listNewRollBackInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbIsForRollBack") as CheckBox;
            int rollBackInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionInfo fi = lastRBInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
            RollBackDistributionInfo trdi = totalNewInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            trdi.IsForRollBack = cbIsForRollBack.Checked;

        }
        selectedNewRollBackInfos = lastRBInfos;
        listNewRollBackInfos = totalNewInfos;
        gvNewPolicies.PageIndex = e.NewPageIndex;
        gvNewPolicies.DataSource = lastRBInfos;
        gvNewPolicies.DataBind();
    }

    protected void ddlPreviousSubTypes_Changed(object sender, EventArgs e) {
        List<RollBackDistributionInfo> lastRBInfos = selectedOldRollBackInfos;
        List<RollBackDistributionInfo> totalInfos = listOldRollBackInfos;
        GridViewRowCollection displayedRows = gvOldPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbIsForRollBack") as CheckBox;
            int rollBackInfoID = Convert.ToInt32(gvOldPolicies.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionInfo fi = lastRBInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
            RollBackDistributionInfo trdi = totalInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            trdi.IsForRollBack = cbIsForRollBack.Checked;
        }
        selectedOldRollBackInfos = lastRBInfos;
        listOldRollBackInfos = totalInfos;
        List<RollBackDistributionInfo> selectedInfos = new List<RollBackDistributionInfo>();
        int selectedSubTypeID = int.Parse(ddlPreviosSubTypes.SelectedValue);
        if (selectedSubTypeID > 0) {
            selectedInfos = listOldRollBackInfos.Where(c => c.InsuranceSubTypeID == selectedSubTypeID).ToList();
            gvOldPolicies.DataSource = selectedInfos;
            gvOldPolicies.DataBind();
            
        } else {
            selectedInfos = listOldRollBackInfos;
            gvOldPolicies.DataSource = selectedInfos;
            gvOldPolicies.DataBind();
        }
        selectedOldRollBackInfos = selectedInfos;
    }

    protected void ddlCurrentSubTypes_Changed(object sender, EventArgs e) {
        List<RollBackDistributionInfo> selectedInfos = new List<RollBackDistributionInfo>();
        int selectedSubTypeID = int.Parse(ddlCurrentSubTypes.SelectedValue);
        if (selectedSubTypeID > 0) {
            selectedInfos = listNewRollBackInfos.Where(c => c.InsuranceSubTypeID == selectedSubTypeID).ToList();
            gvNewPolicies.DataSource = selectedInfos;
            gvNewPolicies.DataBind();

        } else {
            selectedInfos = listNewRollBackInfos;
            gvNewPolicies.DataSource = selectedInfos;
            gvNewPolicies.DataBind();
        }
        selectedNewRollBackInfos = selectedInfos;
    }

    protected void btnPrintPoliciesClick(object sender, EventArgs e) {

        //Check last page changed old policies 
        List<RollBackDistributionInfo> lastRBInfos = selectedOldRollBackInfos;
        List<RollBackDistributionInfo> totalInfos = listOldRollBackInfos;
        GridViewRowCollection displayedRows = gvOldPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbIsForRollBack") as CheckBox;
            int rollBackInfoID = Convert.ToInt32(gvOldPolicies.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionInfo fi = lastRBInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
            RollBackDistributionInfo trdi = totalInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            trdi.IsForRollBack = cbIsForRollBack.Checked;
        }
        selectedOldRollBackInfos = lastRBInfos;
        listOldRollBackInfos = totalInfos;

        
        //Check last page changed new policies 
        List<RollBackDistributionInfo> lastRBNewInfos = selectedNewRollBackInfos;
        List<RollBackDistributionInfo> totalNewInfos = listNewRollBackInfos;
        GridViewRowCollection displayedNewRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedNewRows) {
            CheckBox cbIsForRollBack = gvRow.FindControl("cbIsForRollBack") as CheckBox;
            int rollBackInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            RollBackDistributionInfo fi = lastRBNewInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            fi.IsForRollBack = cbIsForRollBack.Checked;
            RollBackDistributionInfo trdi = totalNewInfos.Where(f => f.ID == rollBackInfoID).SingleOrDefault();
            trdi.IsForRollBack = cbIsForRollBack.Checked;
        }
        selectedNewRollBackInfos = lastRBNewInfos;
        listNewRollBackInfos = totalNewInfos;
        btnInsert.Visible = true;


        RollBackDistributionController.CreateFileForSelectedPolicies(listOldRollBackInfos, listNewRollBackInfos, DateTime.Parse(tbStartDate.Text), DateTime.Parse(tbEndDate.Text), int.Parse(ddlInsuranceCompany.SelectedValue), this.PageBranch.ID);
    }

}

