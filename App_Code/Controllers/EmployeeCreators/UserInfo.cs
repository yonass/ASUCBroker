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
using System.Collections.Generic;
using Broker.DataAccess;

namespace Broker.Controllers.EmployeeManagement {

    /// <summary>
    /// Klasa koja dava opredeleni informacii za daden korisnik (user)
    /// </summary>
    public class UserInfo {

        private Broker.DataAccess.User user;

        /// <summary>
        /// Default-en konstruktor
        /// </summary>
        public UserInfo() {
        }

        /// <summary>
        /// Preoptovaren konstruktor so koj se gradi UserInfo
        /// </summary>
        /// <param name="user"></param>
        public UserInfo(Broker.DataAccess.User user)
        {
            this.user = user;
        }

        /// <summary>
        /// Interfejs za korisnicko ime (UserName)
        /// </summary>
        public string Name {
            get {
                return user.UserName;
            }
            set {
                user.UserName = value;
            }
        }

        /// <summary>
        /// Interfejs koj go vraka ID-to za opredelen korisnik
        /// </summary>
        public int ID {
            get {
                return this.user.ID;
            }
        }
    }
}