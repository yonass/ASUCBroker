using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.UserManagement;

public partial class UserManagement_UsersFunctions : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {
        if (IsPostBack)
            return;
        ObjectDataSource1.SelectParameters.Clear();
        ObjectDataSource1.SelectParameters.Add(new System.Web.UI.WebControls.Parameter("userId", TypeCode.Int32, PageUser.ID.ToString()));
        ObjectDataSource3.SelectParameters.Clear();
        ObjectDataSource3.SelectParameters.Add(new System.Web.UI.WebControls.Parameter("parentId", TypeCode.Int32, PageUser.ID.ToString()));
        ObjectDataSource3.SelectParameters.Add(new System.Web.UI.WebControls.Parameter("childId", TypeCode.Int32, "1"));
    }
    /// <summary>
    /// metod za prikaz na site funkcii po korisnik.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RoleFunctionsList_DataBound(object sender, EventArgs e) {
        if (IsPostBack) {
            List<int> functions = new List<int>();
            foreach (ListItem item in RoleFunctionsList.Items) {
                item.Selected = true;
                functions.Add(Convert.ToInt32(item.Value));
            }
            bool[] enabled = new bool[functions.Count];
            int userId = Convert.ToInt32(ddlUsers.SelectedValue);
            UserManagementController.fillEnabled(userId, enabled, functions);
            int c = 0;
            foreach (ListItem item in RoleFunctionsList.Items) {
                item.Enabled = enabled[c++];
            }
        }
    }


    protected void Button2_Click(object sender, EventArgs e) {
        exchangeSelected(UserFunctionsList.Items, RoleFunctionsList.Items, true);
    }

    protected void Button1_Click(object sender, EventArgs e) {
        exchangeSelected(RoleFunctionsList.Items, UserFunctionsList.Items, false);
    }

    protected void Button3_Click(object sender, EventArgs e) {
        exchangeSelected(RoleFunctionsList.Items, UserFunctionsList.Items, false);
        exchangeSelected(RoleFunctionsList.Items, UserFunctionsList.Items, true);
    }

    public void exchangeSelected(ListItemCollection source, ListItemCollection destination, bool selected) {
        List<ListItem> list = new List<ListItem>();
        foreach (ListItem item in source) {
            if (item.Selected == selected && item.Enabled == true) {
                destination.Add(item);
                list.Add(item);
            }
        }
        int c = 0;
        foreach (ListItem item in list) {
            source.Remove(list[c++]);
        }
    }
    /// <summary>
    /// metod za dodavanje na nova fukcija na korisnik.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button4_Click(object sender, EventArgs e) {
        List<int> list = new List<int>();
        foreach (ListItem item in RoleFunctionsList.Items) {
            if (item.Enabled) {
                list.Add(Convert.ToInt32(item.Value));
            }
        }
        int userId = Convert.ToInt32(ddlUsers.SelectedValue);
        //funkcija za azuriranje na funkciite na korisnik.
        UserManagementController.updateUser(userId, list);
        RegisterStartupScript("myAlert", "<script>alert('Успешно доделување/одземање на функции.')</script>");
    }
    /// <summary>
    /// metod za promena na korisnikot za koj sakame da dodame novi funkcii.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnChildChanged(object sender, EventArgs e) {
        ObjectDataSource1.SelectParameters.Clear();
        ObjectDataSource1.SelectParameters.Add(new System.Web.UI.WebControls.Parameter("userId", TypeCode.Int32, ddlUsers.SelectedValue));

        ObjectDataSource3.SelectParameters.Clear();
        ObjectDataSource3.SelectParameters.Add(new System.Web.UI.WebControls.Parameter("parentId", TypeCode.Int32, PageUser.ID.ToString()));
        ObjectDataSource3.SelectParameters.Add(new System.Web.UI.WebControls.Parameter("childId", TypeCode.Int32, ddlUsers.SelectedValue));
        RoleFunctionsList.DataBind();
        UserFunctionsList.DataBind();

        RoleFunctionsList.Visible = true;
        UserFunctionsList.Visible = true;

    }
}

