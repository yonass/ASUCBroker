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


namespace Broker.DataAccess {

    public partial class Function : EntityBase<Function> {

        public static string RATES_APPROVER = "(БРОКЕРАдмин) Одобрување на рати";


        /// <summary>
        /// Vrakja lista od site funkcii
        /// </summary>
        /// <returns>Vrakja numerirana lista (IEnumerable)</returns>
        /// Se povikuva vo UsersManagementControllers\RolesManagementController.cs i 
        /// UsersManagementControllers\WebPagesController.cs
        public static IEnumerable<Function> GetFunctions() {
            return Table;
        }

        /// <summary>
        /// Vrakja fukcija spored imeto
        /// </summary>
        /// <param name="name">string name</param>
        /// <returns>Vrakja object Function</returns>
        /// Se povikuva vo entitetite WebPage.cs i vo UsersWebPage.cs
        public static Function GetFunctionByName(string name) {
            return Table.Where(v => v.Name == name).SingleOrDefault();
        }

        public override bool Equals(object obj) {
            Function f= (Function)obj;
            return (this.ID == f.ID);
        }

        public override int GetHashCode() {
            return this.ID;
        }
    }
}
