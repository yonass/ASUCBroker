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

    public partial class RolesFunction :EntityBase<RolesFunction>{

        /// <summary>
        /// Brisi zapis od tabelata 
        /// </summary>
        /// <param name="roleId">int roleID</param>
        /// <param name="functions">List(int) functions</param>
        /// Se povikuva vo UserManagementControllers\RolesManagementController.cs
        public static void DeleteRoleFunctions(int roleId, List<int> functions) {
            foreach(int fId in functions) {
                RolesFunction delFunction=new RolesFunction();
                delFunction.FunctionID= fId;
                delFunction.RoleID= roleId;
                delFunction.Delete();
            }
        }
        /// <summary>
        /// Vnesuva zapis vo tabelata
        /// </summary>
        /// <param name="roleId">int roleID</param>
        /// <param name="functions">List(int) functions</param>
        /// Se povikuva vo UserManagementControllers\RolesManagementController.cs
        public static void InsertRoleFunctions(int roleId, List<int> functions) {
            foreach(int fId in functions) {
                RolesFunction delFunction=new RolesFunction();
                delFunction.FunctionID= fId;
                delFunction.RoleID = roleId;
                delFunction.Insert();
            }
        }

    }
}
