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

    public partial class UsersFunction : EntityBase<UsersFunction> {

        /// <summary>
        /// Brisi zapisi vo tabelata UserFunctions
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <param name="functions">Lis(int) functions</param>
        /// Se povikuva vo UserManagementControllers\UserManagementController.cs
        public static void DeleteUserFunctions(int userId, List<int> functions) {
            foreach(int fId in functions) {
                UsersFunction delFunction=new UsersFunction();
                delFunction.FunctionID= fId;
                delFunction.UserID = userId;
                delFunction.Delete();
            }
        }

        /// <summary>
        /// Vnesuva zapisi vo tabelata UserFunctions
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <param name="functions">List(int) functions</param>
        /// Se povikuva vo UserManagementControllers\UserManagementController.cs
        public static void InsertUserFunctions(int userId, List<int> functions) {
            foreach(int fId in functions) {
                UsersFunction delFunction=new UsersFunction();
                delFunction.FunctionID= fId;
                delFunction.UserID = userId;
                delFunction.Insert();
            }
        }

        /// <summary>
        /// Vnesuva zapisi vo tabelata UserFunctions
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <param name="functions">List(functions) functions</param>
        /// Ne se povikuva 
        public static void InsertUserFunctions(int userId, List<Function> functions) {
            foreach(Function f in functions) {
                UsersFunction delFunction=new UsersFunction();
                delFunction.FunctionID= f.ID;
                delFunction.UserID = userId;
                delFunction.Insert();
            }
        }

        public static List<UsersFunction> getUsersWithFunction(int functionID) {
            List<UsersFunction> returnList = Table.Where(c => c.FunctionID == functionID).ToList();
            return returnList;
        }
        public static UsersFunction GetByFunctionAndUser(int functionID, int userID) {
            return Table.Where(f => f.FunctionID == functionID && f.UserID == userID).SingleOrDefault();
        }
    }
}