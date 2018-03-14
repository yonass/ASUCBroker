using System;
using System.Data;
using System.Data.Linq;
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

namespace Broker.Controllers.UserManagement {
    
    /// <summary>
    /// Klasa za menadment na ulogite
    /// </summary>
    public class RolesManagementController {

        /// <summary>
        /// Default-en konstruktor
        /// </summary>
        public RolesManagementController() {
        }

        /// <summary>
        /// Metod koj gi vraka site ulogi
        /// </summary>
        /// <returns>numerirana lista (IEnumerable(RoleInfo))</returns>
        public static IEnumerable<RoleInfo> GerAllRoles() {
            List<Broker.DataAccess.Role> roles = Broker.DataAccess.Role.GetRoles().Where(v => v.IsVisible == true).ToList();
            List<RoleInfo> result = new List<RoleInfo>();
            foreach (Broker.DataAccess.Role r in roles)
            {
                result.Add(new RoleInfo(r));
            }
            return result;
        }

        /// <summary>
        /// Metod koj gi vraka funkciite za specificirana uloga
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>numerirana lista (IEnumerable(FunctionInfo))</returns>
        public static IEnumerable<FunctionInfo> GetFucntionsByRole(int roleId) {
            List<Broker.DataAccess.Function> roleFunctionsList = Broker.DataAccess.Role.GetFucntionsByRole(roleId).ToList();
            HashSet<Broker.DataAccess.Function> roleFunctionsSet = new HashSet<Broker.DataAccess.Function>();
            foreach (Broker.DataAccess.Function f in roleFunctionsList)
            {
                roleFunctionsSet.Add(f);
            }
            List<FunctionInfo> infos  = new List<FunctionInfo>();
            List<Broker.DataAccess.Function> allFunctions = Broker.DataAccess.Function.GetFunctions().ToList();
            foreach (Broker.DataAccess.Function f in allFunctions)
            {
                if(roleFunctionsSet.Contains(f)) {
                    infos.Add(new FunctionInfo(f, FunctionInfo.CHECKED));
                } else {
                    infos.Add(new FunctionInfo(f, FunctionInfo.NOT_CHECKED));
                }
            }
            return infos;
        }

        /// <summary>
        /// Metod so koj se vrsi popolnuvanje na pole so funkcii koi se
        /// preddefinirani za konkretnata uloga od mnozestvoto na site funkcii
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="check"></param>
        /// <param name="list"></param>
        /// se koristi kaj UserManagement\RolesFunctions.aspx.cs za da se
        /// ispolni CheckBoxList so funkcii za dadena uloga
        public static void fillChecked(int roleId, bool[] check, List<int> list) {
            List<Broker.DataAccess.Function> functions = Broker.DataAccess.Role.GetFucntionsByRole(roleId).ToList();
            int c=0;
            foreach(int fId in list) {
                bool found = false;
                foreach (Broker.DataAccess.Function function in functions)
                {
                    if(function.ID == fId) {
                        found=true;
                        break;
                    }
                }
                check[c++]=found;
            }
        }

        /// <summary>
        /// Metod koj vrsi azuriranje na funkciite za dadena uloga
        /// so novo definirani funkcii
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="newFunctions"></param>
        public static void updateRole(int roleId, HashSet<int> newFunctions) {
            List<int> forDelete = new List<int>();
            List<int> forInsert = new List<int>();
            HashSet<int> oldFunctions  = new HashSet<int>();
            foreach (Broker.DataAccess.Function f in Broker.DataAccess.Role.GetFucntionsByRole(roleId))
            {
                oldFunctions.Add(f.ID);
            }
            foreach(int fId in newFunctions) {
                if(!oldFunctions.Contains(fId)) {
                    forInsert.Add(fId);
                }
            }
            foreach(int fId in oldFunctions) {
                if(!newFunctions.Contains(fId)) {
                    forDelete.Add(fId);
                }
            }
            Broker.DataAccess.RolesFunction.DeleteRoleFunctions(roleId, forDelete);
            Broker.DataAccess.RolesFunction.InsertRoleFunctions(roleId, forInsert);

            List<Broker.DataAccess.Function> allFunctions = Broker.DataAccess.Role.GetFucntionsByRole(roleId).ToList();
            HashSet<string> functionsSet = new HashSet<string>();
            foreach (Broker.DataAccess.Function f in allFunctions)
            {
                functionsSet.Add(f.Name);
            }
            List<Broker.DataAccess.User> roleUsers = Broker.DataAccess.User.GetUsersInRole(roleId).ToList();
            foreach (Broker.DataAccess.User u in roleUsers)
            {
                WebPagesVisibilityContoller.UpdateVisibleFunctions(u.ID, functionsSet);
            }
        }
    }
}