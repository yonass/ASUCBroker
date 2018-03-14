using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.UserManagement;

public partial class UserManagement_UsersWebPages : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {

    }

    protected void SavaButton_Click(object sender, EventArgs e) {
        HashSet<string> list = new HashSet<string>();
        foreach (DataListItem item in DataList1.Items) {
            foreach (Control c in item.Controls) {
                if (c is CheckBox) {
                    CheckBox box = c as CheckBox;
                    if (box.Checked) {
                        list.Add(box.Text);
                    }
                }
            }
        }
        int userId = Convert.ToInt32(ddlUsers.SelectedValue);
        //funkcija za dodeluvanje na web-stran na korisnik.
        WebPagesVisibilityContoller.UpdateVisibleFunctions(userId, list);
        RegisterStartupScript("myAlert", "<script>alert('Успешно доделување/одземање на видливи функции.')</script>");
    }
}
