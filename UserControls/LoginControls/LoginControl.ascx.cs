using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Broker.DataAccess;
using Broker.Authentication;
using System.IO;
using System.Text;


public partial class UserControls_LoginControls_LoginControl : System.Web.UI.UserControl {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void btnLogin_Click(object sender, EventArgs e) {
        string IpAddress = HttpContext.Current.Request.UserHostAddress;
        string username = tbUserName.Text;
        Broker.DataAccess.User u = Broker.DataAccess.User.GetByUserName(username);
        if (u != null) {
            string password = tbPassword.Text;
            if (!Broker.DataAccess.User.IsValidPassword(u, password))
            {
                lblMessage.Text = "Погрешна лозинка";
                LoginLog.Insert(IpAddress, username, password, false);
                return;
            } else {
                //HttpContext.Current.Session["User"] = u;
                //HttpContext.Current.Session["UserID"] = u.ID;
                //Response.Redirect("~Default.aspx");
            }
        } else {
            lblMessage.Text = "Погрешно корисничко име";
        }

        string message;
        message = AuthenticateUser.Login(tbUserName.Text, tbPassword.Text);
        if (message != AuthenticateUser.OK_LOGIN) {
          //  Response.Redirect(WebPage.SUPER_ROOT);
            LoginLog.Insert(IpAddress, username, tbPassword.Text, false);
        } else {
            LoginLog.Insert(IpAddress, username, tbPassword.Text, true);
            Response.Redirect("~/Default.aspx");
        }
    }
}



