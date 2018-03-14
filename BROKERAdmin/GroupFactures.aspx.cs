using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;
using Broker.Controllers.FacturesControllers;
using Broker.Controllers.PolicyControllers;

public partial class BROKERAdmin_GroupFactures : AuthenticationPage {

    public List<FactureInfo> listNewFactureInfos {
        get {
            if (ViewState["ListNewFactureInfos"] == null) {
                return new List<FactureInfo>();
            } else {
                return (List<FactureInfo>)ViewState["ListNewFactureInfos"];
            }
        }
        set {
            ViewState["ListNewFactureInfos"] = value;
        }
    }



    protected void Page_Load(object sender, EventArgs e) {

    }
    public int FactureID {
        get {
            if (ViewState["FactureID"] != null) {
                return int.Parse(ViewState["FactureID"].ToString());
            } else {
                return 0;
            }
        }
        set {
            ViewState["FactureID"] = value;
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e) {
      
        if (FactureID > 0) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(FactureID);
            PrintFactures.PrintFacture(f);
        }
    }
    protected void btnPrintAnex_Click(object sender, EventArgs e) {
        if (FactureID > 0) {
            Broker.DataAccess.Facture f = Broker.DataAccess.Facture.Get(FactureID);
            PrintFactures.PrintAnex(f);
        }
    }
    protected void btnInsert_Click(object sender, EventArgs e) {
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        List<FactureInfo> lastFactureInfos = listNewFactureInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(facInfo => facInfo.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listNewFactureInfos = lastFactureInfos;
        List<PolicyItem> items = new List<PolicyItem>();
        foreach (FactureInfo fi in lastFactureInfos) {
            if (fi.IsForFacturing) {
                PolicyItem pi = PolicyItem.Get(fi.ID);
                items.Add(pi);
            }
        }
        if (items.Count > 0) {
            if (ddlClients.SelectedIndex != -1) {
                int clientID = int.Parse(ddlClients.SelectedValue);
                int factureID = Broker.DataAccess.Facture.CreateGroupFacture(this.PageUser.ID, clientID, startDate, endDate, items);
                FactureID = factureID;
                btnPrint.Visible = true;
                btnInsert.Visible = false;
                //btnPrintAnex.Visible = true;
                InsertClientInFinansovo(clientID);
            } else {
                lblError.Visible = true;
                btnPrint.Visible = false;
            }
        } else {
            lblError.Visible = true;
            btnPrint.Visible = false;
        }
    }

    void InsertClientInFinansovo(int clientID)
    {
        PolicyController.InsertClientInFinansovo(clientID);
    }

    protected void gvNewPolicies_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        List<FactureInfo> lastFactureInfos = listNewFactureInfos;
        GridViewRowCollection displayedRows = gvNewPolicies.Rows;
        foreach (GridViewRow gvRow in displayedRows) {
            CheckBox cbIsForFacturing = gvRow.FindControl("cbIsForFacturing") as CheckBox;
            int factureInfoID = Convert.ToInt32(gvNewPolicies.DataKeys[gvRow.RowIndex].Value);
            FactureInfo fi = lastFactureInfos.Where(f => f.ID == factureInfoID).SingleOrDefault();
            fi.IsForFacturing = cbIsForFacturing.Checked;
        }
        listNewFactureInfos = lastFactureInfos;
        gvNewPolicies.PageIndex = e.NewPageIndex;
        gvNewPolicies.DataSource = lastFactureInfos;
        gvNewPolicies.DataBind();
    }


    protected void btnCheck_Click(object sender, EventArgs e) {

        List<Broker.DataAccess.Client> clientsList = Broker.DataAccess.Client.GetForGroupFactures(DateTime.Parse(tbStartDate.Text), DateTime.Parse(tbEndDate.Text));
        if (clientsList.Count > 0) {
            ddlClients.DataSource = clientsList;
            ddlClients.DataBind();
            ddlClients.Visible = true;
            lblClient.Visible = true;
            btnCheck.Visible = false;
            btnInsert.Visible = true;
            BindPolForClient(clientsList[0].ID);
        } else {
            ddlClients.DataSource = clientsList;
            ddlClients.DataBind();
            ddlClients.Visible = false;
            lblClient.Visible = false;
        }
    }

    void BindPolForClient(int clientID) {
        DateTime startDate = DateTime.Parse(tbStartDate.Text);
        DateTime endDate = DateTime.Parse(tbEndDate.Text);
        List<PolicyItem> lstPolPerClient = PolicyItem.GetForGroupFactures(startDate, endDate, clientID);
        List<FactureInfo> lstFacInfo = new List<FactureInfo>();
        foreach (PolicyItem pi in lstPolPerClient) {
            FactureInfo fi = new FactureInfo();
            fi.ID = pi.ID;
            fi.PolicyNumber = pi.PolicyNumber;
            fi.IsForFacturing = true;
            fi.PremiumValue = pi.PremiumValue;
            lstFacInfo.Add(fi);
        }
        listNewFactureInfos = lstFacInfo;
        gvNewPolicies.DataSource = lstFacInfo;
        gvNewPolicies.DataBind();
    }

    protected void ddlClients_SelectedIndexChanged(object sender, EventArgs e) {
        int clientID = Convert.ToInt32(ddlClients.SelectedValue);
        BindPolForClient(clientID);
        
    }
}

