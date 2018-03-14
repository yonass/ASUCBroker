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

namespace Broker.Controllers.UserManagement {
    /// <summary>
    /// Klasa so interfejsi i informacii za web-stranite
    /// </summary>
    public class WebPageInfo {

        public string Name { get; set; }
        public int ID { get; set; }

        /// <summary>
        /// Konstruktor na klasata
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public WebPageInfo(string name, int id) {
            this.Name  = name;
            this.ID = id;
        }
    }
}