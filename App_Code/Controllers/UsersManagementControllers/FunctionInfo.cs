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
    /// Klasa so interfejsi i informacii za funkciite
    /// </summary>
    public class FunctionInfo {

        public static bool CHECKED = true;
        public static bool NOT_CHECKED = true;

        public Function Function { get; set; }
        public WebPage WebPage { get; set; }

        public bool Enabled { get; set; }
        public bool Checked { get; set; }

        /// <summary>
        /// Default-en konstruktor
        /// </summary>
        public FunctionInfo() {
        }

        /// <summary>
        /// Konstruktor na klasata
        /// </summary>
        /// <param name="f"></param>
        /// <param name="check"></param>
        public FunctionInfo(Function f, bool check) {
            this.Function = f;
            this.Checked = check;
        }

        /// <summary>
        /// Interfejs za promenliva ID koja go vraka
        /// ID-to na funkcijata (Function.ID)
        /// </summary>
        public int ID {
            get {
                return Function.ID;
            }
        }

        /// <summary>
        /// Interfejs za promenliva Name koja go vraka
        /// imeto na funkcijata (Function.Name)
        /// </summary>
        public string Name {
            get {
                return Function.Name;
            }
        }
    }
}