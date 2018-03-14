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


namespace Broker.DataAccess {
    /// <summary>
    /// Klasa so interfejsi i informacii za ulogite
    /// </summary>
    public class RoleInfo {

        public Role Role { get; set; }

        /// <summary>
        /// Interfejs za promenliva ID koja go vraka
        /// ID-to na ulogata (Role.ID)
        /// </summary>
        public int ID {
            get {
                return Role.ID;
            }
        }

        /// <summary>
        /// Interfejs za promenliva Name koja go vraka
        /// imeto na ulogata (Role.Name)
        /// </summary>
        public string Name {
            get {
                return Role.Name;
            }
        }

        /// <summary>
        /// Konstruktor na klasata
        /// </summary>
        /// <param name="role"></param>
        public RoleInfo(Role role) {
            this.Role = role;
        }
    }
}