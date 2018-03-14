using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorPage : System.Web.UI.Page {
    public static string DEFAULT_ERROR_MESSAGE = "Нарушување на процесот !!!";

    protected void Page_Load(object sender, EventArgs e) {
        string message = Request["errMessage"];
        if (message == null) {
            message = DEFAULT_ERROR_MESSAGE;
        }
        lblError.Text = message;
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
}
