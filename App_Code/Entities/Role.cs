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

    public partial class Role :EntityBase<Role>{

        public static string SIMTAdmin = "SIMTAdmin";
        public static string BROKERAdmin = "BROKERAdmin";
        public static string SEAdmin = "SEAdmin";
        public static string Broker = "Broker";
        public static string Employee = "Employee";
        public static string ExternalAgent = "ExternalAgent";
        public static string SEBranch = "SEBranch";

        public static Role GetByName(string name) {
            return Table.Where(c => c.Name == name).SingleOrDefault();
        }

        /// <summary>
        /// Gi vrakja site funkcii za opredelena rolja
        /// </summary>
        /// <param name="roleID">int roleID</param>
        /// <returns>Vrakja numerirana lista (IEnumerable)</returns>
        /// Se povikuva vo EmployeeCreators\EmployeeController.cs i 
        /// UserManagementControllers\RolesManagementController.cs
        public static IEnumerable<Function> GetFucntionsByRole(int roleID) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"SELECT f.ID, f.Name, f.Description
                             FROM Roles r
                             INNER JOIN RolesFunctions rs ON r.ID = rs.RoleID
                             INNER JOIN Functions f ON rs.FunctionId = f.ID 
                             WHERE r.ID= " + roleID;
            return dc.ExecuteQuery<Function>(query);
        } 

        /// <summary>
        /// Gi vrakja site Rolji
        /// </summary>
        /// <returns>Vrakja numerirana lista (IEnumerable)</returns>
        /// Se povikuva vo EmployeeCreators\EmployeeController.cs i 
        /// UserManagementControllers\RolesManagementController.cs
        public static IEnumerable<Role> GetRoles() {
            return Table;
        }

        /// <summary>
        /// Vrakja rolja spored imeto
        /// </summary>
        /// <param name="name">string name</param>
        /// <returns>Vrakja Role object</returns>
        /// Se povikuva vo EmployeeCreators\EmployeeController.cs,
        /// EmployeeCreators\EmployRoleController.cs i vo entitetite
        /// Company i User
        public static Role GetRoleByName(string name) {
            Role r=  Table.Where(v => v.Name==name).SingleOrDefault();
            if(r!=null) {
                return r;
            }
            return null;
        }

    }
}