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

namespace Broker.Controllers.UserManagement {
    /// <summary>
    /// Klasa za mendziranje so vidlivi/nevidlivi web-strani za korisnici
    /// </summary>
    public class WebPagesVisibilityContoller {

        /// <summary>
        /// Metod koj gi spojuva vo lista preddefiniranite funkcii za ulogata na
        /// specificiraniot korisnik i dopolnitelnite funkcii za istiot, taka sto
        /// vraka lista so site dostapni funkcii za nego
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(Function)</returns>
        public static IEnumerable<Broker.DataAccess.Function> GetAvaliableFunctionsByUser(int userId)
        {
            return UserManagementController.GetAvaliableFunctionsByUser(userId);
        }

        /// <summary>
        /// Metod koj gi vraka vidlivite web-strani za specificiran korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(UsersWebPage)</returns>
        public static IEnumerable<UsersWebPage> GetVisibleWebPagesByUser(int userId) {
            return UsersWebPage.GetVisibleWebPagesByUser(userId);
        }

        /// <summary>
        /// Metod koj gi vraka vidlivite funkcii(kako stringovi) za specificiran korisnik
        /// spored soodvetnite vidlivi web-strani
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>mnozestvo od string-ovi - HashSet(string)</returns>
        public static HashSet<string> GetVisibleFunctionsByUser(int userId) {
            HashSet<string> set= new HashSet<string>();
            foreach(UsersWebPage userPage in GetVisibleWebPagesByUser(userId)) {
                Broker.DataAccess.Function f = WebPage.GetFunctionByWebPage(userPage.WebPageID);
                if(f!=null) {
                    set.Add(f.Name);
                }
            }
            return set;
        }

        /// <summary>
        /// Metod koj vraka objekti od tip FunctionInfo za vidlivite funkcii
        /// za specificiran korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(FunctionInfo)</returns>
        public IEnumerable<FunctionInfo> GetFunctionVisibilityInfo(int userId) {
            HashSet<string> visible = GetVisibleFunctionsByUser(userId);
            List<FunctionInfo> result = new List<FunctionInfo>();
            if (userId == 0) {
                return result;
            }
            foreach (Broker.DataAccess.Function f in GetAvaliableFunctionsByUser(userId))
            {
                FunctionInfo info=new FunctionInfo();
                info.Function=f;
                info.Checked = visible.Contains(f.Name);
                result.Add(info);
            }
            return result;
        }

        /// <summary>
        /// Metod koj vrzi azuriranje na vidlivite funkcii za specificiran korisnik
        /// so novo definirani funkcii dadeni kako string-ovi
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newFunctionsNames"></param>
        public static void UpdateVisibleFunctions(int userId, HashSet<string> newFunctionsNames) {
            HashSet<string> oldFunctions = GetVisibleFunctionsByUser(userId);
            List<UsersWebPage> forInsert = new List<UsersWebPage>();
            List<UsersWebPage> forDelete = new List<UsersWebPage>();

            foreach(string f in newFunctionsNames) {
                if(!oldFunctions.Contains(f)) {
                    UsersWebPage uPage=new UsersWebPage();
                    uPage.UserID=userId;
                    uPage.WebPageID = WebPage.GetWebPageByFunctionName(f).ID;
                    forInsert.Add(uPage);
                }
            }

            foreach(string f in oldFunctions) {
                if(!newFunctionsNames.Contains(f)) {
                    UsersWebPage uPage=new UsersWebPage();
                    uPage.UserID=userId;
                    uPage.WebPageID = WebPage.GetWebPageByFunctionName(f).ID;
                    forDelete.Add(uPage);
                }
            }
            UsersWebPage.DeleteUserWebPages(forDelete);
            UsersWebPage.InsertUserWebPages(forInsert);
        }
    }
}