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

namespace Broker.Controllers.EmployeeManagement {
    /// <summary>
    /// Klasa vo koja se zakodirani iminjata na ulogite kako sto se vneseni vo bazata na podatoci
    /// </summary>
    public class RolesInfo {

     
        public static string SIMTAdmin = "SIMTAdmin";


        public static string BROKERAdmin = "BROKERAdmin";

        public static string Broker = "Broker";


        public static string SEAdmin = "SEAdmin";

        public static string Employee = "Employee";

        public static string MarketingAgent = "ExternalAgent";

    }
}