using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Broker.DataAccess;
using Broker.Controllers.UserAuthentication;
using System.IO;
using Broker.Controllers.EmployeeManagement;
using System.Collections.Generic;

namespace Broker.Authentication {
    /// <summary>
    /// Klasa za avtentificiranje na korisnici, nivna najava i odjava od sistemot
    /// </summary>
    public class AuthenticateUser {
        /// <summary>
        /// Default-en konstruktor
        /// </summary>
        public AuthenticateUser() {
        }
        public static string WRONG_USERPASS = "Погрешно корисничко име или лозинка!";
        //public static string WRONG_CERTIFICATE = "Невалиден сертификат!";
        //public static string WRONG_COMPANY = "Погрешна компанија!";
        //public static string DUPLICATE_LOGIN = "Веќе има најавено корисник со такво корисничко име!";
        public static string OK_LOGIN = "";


        public static string Login(string userName, string password) {
            bool hasCookie = false;
            Broker.DataAccess.User user = Broker.Controllers.UserAuthentication.UserInfo.GetUser(userName, password);
            if (user != null) {
                        HttpContext.Current.Session["User"] = user;
                        HttpContext.Current.Session["Branch"] = user.Branch;
                        HttpContext.Current.Session["UserID"] = user.ID;
                        HttpContext.Current.Session["BranchID"] = user.BranchID;
                        HttpContext.Current.Session["SessionTree"] = new SessionTree();
                        return OK_LOGIN;
                    } else {
                return WRONG_USERPASS;
            }
        }


        public static bool Logout() {
            int userID = (int)HttpContext.Current.Session["UserID"];
            removeFromApp(userID);
            //removeCookie(userID);
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Redirect("~/Login.aspx");
            return true;
        }

        public static void PureLogout() {
            int userID = (int)HttpContext.Current.Session["UserID"];
            removeCookie(userID);
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

        }

        public static bool CheckCertificate(User user) {
            //User expectedUser = (User)HttpContext.Current.Session["ExpectedUser"];
            //if (expectedUser.ID != user.ID) {
            //    return false;
            //}
            return true;
        }

        public static bool GetExpectedUser(string certificate) {
            //User user = User.GetUserByCertificate(certificate);
            //if (user != null) {
            //    HttpContext.Current.Session["ExpectedUser"] = user;
            //    return true;
            //}
            //return false;
            return true;
        }

        private static void addCookie(int userID) {
            HttpCookie appCookie = new HttpCookie(userID.ToString(), userID.ToString());
            appCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(appCookie);
        }

        private static void removeCookie(int userID) {
            HttpCookie remCookie = HttpContext.Current.Request.Cookies[userID.ToString()];
            remCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(remCookie);
        }

        private static bool addInApp(int userID) {
            HttpContext.Current.Application.Lock();
            HashSet<int> usersSet = (HashSet<int>)HttpContext.Current.Application["usersSet"];
            if (usersSet == null) {
                usersSet = new HashSet<int>();
            }
            if (usersSet.Contains<int>(userID)) {
                return false;
            }
            usersSet.Add(userID);
            addCookie(userID);
            HttpContext.Current.Application["usersSet"] = usersSet;
            HttpContext.Current.Application.UnLock();
            return true;
        }

        public static void removeFromApp(int userID) {
            HttpContext.Current.Application.Lock();
            HashSet<int> usersSet = (HashSet<int>)HttpContext.Current.Application["usersSet"];
            if (usersSet != null) {
                usersSet.Remove(userID);
            }
            HttpContext.Current.Application["usersSet"] = usersSet;
            HttpContext.Current.Application.UnLock();
        }

      
        private static bool checkCookie(int userID) {
            HttpCookie c = HttpContext.Current.Request.Cookies[userID.ToString()];
            if (c != null && c.Value.Equals(userID.ToString())) {
                return true;
            } else {
                return false;
            }
        }
    }
}
