using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.UserManagement;

public partial class UserManagement_WebPagesFunctions : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {

    }

    protected void ButtonSave_Click(object sender, EventArgs e) {
        if (FunctionsList.SelectedItem == null || WebPagesList.SelectedItem == null) {
            return;
        }
        int functionId = Convert.ToInt32(FunctionsList.SelectedItem.Value);
        string page = WebPagesList.SelectedItem.Text;
        //funkcija za zacuvuvanje na promenite.
        WebPagesController.SaveFunctionWebPage(functionId, page);
        FunctionsList.Items.Remove(FunctionsList.SelectedItem);
        WebPagesList.Items.Remove(WebPagesList.SelectedItem);
    }
}
