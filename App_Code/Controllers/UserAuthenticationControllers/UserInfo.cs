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
using Broker.Controllers.EmployeeManagement;

namespace Broker.Controllers.UserAuthentication {
    public class UserInfo {
        /// <summary>
        /// Default-en konstruktor
        /// </summary>
        public UserInfo() {
        }

        /// <summary>
        /// Metod koj vraka cel entitet - korisnik dokolku se sovpagaat soodvetnoto korisnicko ime,
        /// loznikata i dokolku statusot na korisnikot e aktiven
        /// Se koristi za da se kontolira najavuvanjeto na korisnici na aplikacijata
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>entitet - korisnik (user)</returns>
        /// se povikuva kaj Authentication\AuthenticateUser.cs
        public static Broker.DataAccess.User GetUser(string userName, string password)
        {
            Broker.DataAccess.User user = Broker.DataAccess.User.GetUserByUsername(userName);
            if (user != null) {
                return Broker.DataAccess.User.Select().Where(u => u.UserName == userName && u.Password == password && u.IsActive == true).SingleOrDefault();
            } else return null;
        }

       
       
    }
}