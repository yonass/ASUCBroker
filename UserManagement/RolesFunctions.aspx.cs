using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.UserManagement;

public partial class UserManagement_RolesFunctions : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {

    }
    /// <summary>
    /// metod za dodavanje na novi funkcii na uloga.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CheckBoxList1_DataBound(object sender, EventArgs e) {
        List<int> functions = new List<int>();
        foreach (ListItem item in CheckBoxList1.Items) {
            functions.Add(Convert.ToInt32(item.Value));
        }
        bool[] check = new bool[functions.Count];
        String role = RadioButtonList1.SelectedValue;
        if (role != "") {
            int roleId = Convert.ToInt32(RadioButtonList1.SelectedValue);
            RolesManagementController.fillChecked(Convert.ToInt32(RadioButtonList1.SelectedValue), check, functions);
            int c = 0;
            foreach (ListItem item in CheckBoxList1.Items) {
                item.Selected = check[c++];
            }
        }
    }
    /// <summary>
    /// metod za zacuvuvanje na promenite.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SaveButton_Click(object sender, EventArgs e) {
        HashSet<int> list = new HashSet<int>();
        foreach (ListItem item in CheckBoxList1.Items) {
            if (item.Selected) {
                list.Add(Convert.ToInt32(item.Value));
            }
        }
        int roleId = Convert.ToInt32(RadioButtonList1.SelectedValue);
        //funkcija za azuriranje na listata na funkcii za uloga.
        RolesManagementController.updateRole(roleId, list);

    }
}
