using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Authentication;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            if (Session["User"] != null) {
                Broker.DataAccess.User user = (Broker.DataAccess.User)Session["User"];
                lblUserInfo.Text = "Корисник " + user.UserName + " " + user.Name;
            }
            //lblBrokerHouseName.Text = Broker.DataAccess.BrokerHouseInformation.GetBrokerHouseName();
            //imgLogoOnMaster.ImageUrl = 
        }
        if (ContentPlaceHolder1.Page != null) {
            string s = ContentPlaceHolder1.Page.Title;
            lblPageTitle.Text = s;
        }
    }

    protected void NBOTree_SelectedNodeChanged(object sender, EventArgs e) {
        if (NBOTree.SelectedNode.ChildNodes.Count > 0) {
            if ((bool)NBOTree.SelectedNode.Expanded) {
                NBOTree.SelectedNode.Collapse();
            } else {
                NBOTree.SelectedNode.Expand();
            }
        }
        NBOTree.SelectedNode.Parent.Expand();
        
    }


    protected void NBOTree_TreeNodeExpanded(object sender, EventArgs e) {
    }

    protected void NBOTree_TreeNodeDataBound(object sender, TreeNodeEventArgs e) {
    }


    protected void NBOTree_DataBound(object sender, EventArgs e) {
    }

    void Page_PreRender(object sender, EventArgs e) {
        int c = NBOTree.Nodes.Count;
    }

    protected void btnLogout_Click(object sender, EventArgs e) {
        AuthenticateUser.Logout();
    }
    protected void NBOTree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e) {
    }
}
