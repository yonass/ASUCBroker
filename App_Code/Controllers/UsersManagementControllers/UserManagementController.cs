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
using Broker.DataAccess;
using System.Collections.Generic;

namespace Broker.Controllers.UserManagement {

    /// <summary>
    /// Klasa za menadziranje na korisnik
    /// </summary>
    public class UserManagementController {

        /// <summary>
        /// Default-en konstruktor
        /// </summary>
        public UserManagementController() {
        }

        /// <summary>
        /// Metod koj gi vraka osnovnite(preddefinirani) funkcii za specificiran korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(Function)</returns>
        public static IEnumerable<Broker.DataAccess.Function> GetRoleFucntionsByUser(int userId)
        {
            Broker.DataAccess.Role r = Broker.DataAccess.User.GetRoleByUserId(userId);
            return Broker.DataAccess.Role.GetFucntionsByRole(r.ID);
        }

        /// <summary>
        /// Metod koj vraka dopolnitelni funkcii za specificiran korisnik
        /// sto gi nema vo preddefiniranite funkcii za negovata uloga
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(Function)</returns>
        public static IEnumerable<Broker.DataAccess.Function> GetCustomFunctionsByUser(int userId)
        {
            return Broker.DataAccess.User.GetCustomFunctions(userId);
        }

        /// <summary>
        /// Metod koj gi spojuva vo lista preddefiniranite funkcii za ulogata na
        /// specificiraniot korisnik i dopolnitelnite funkcii za istiot, taka sto
        /// vraka lista so site dostapni funkcii za nego
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(Function)</returns>
        public static IEnumerable<Broker.DataAccess.Function> GetAvaliableFunctionsByUser(int userId)
        {
            List<Broker.DataAccess.Function> total = GetRoleFucntionsByUser(userId).ToList();
            total.AddRange(GetCustomFunctionsByUser(userId));
            return total; 
        }

        /// <summary>
        /// Metod koj vraka lista so objekti od klasata FunctionInfo za site
        /// dostapni funkcii za specificiran korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(FunctionInfo)</returns>
        public static IEnumerable<FunctionInfo> GetAvaliableFunctionInfosByUser(int userId) {
            List<Broker.DataAccess.Function> total = GetRoleFucntionsByUser(userId).ToList();
            
            List<FunctionInfo> list=new List<FunctionInfo>();
            foreach (Broker.DataAccess.Function roleFunction in total)
            {
                FunctionInfo info=new FunctionInfo(roleFunction, false);
                list.Add(info);
            }
            foreach (Broker.DataAccess.Function customFunction in GetCustomFunctionsByUser(userId))
            {
                FunctionInfo info=new FunctionInfo(customFunction, true);
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// Metod koj gi vraka funkciite koi gi poseduva roditelot na daden korisnik,
        /// a ne gi ima samiot korisnik
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="childId"></param>
        /// <returns>numerirana lista - IEnumerable(Function)</returns>
        public static IEnumerable<Broker.DataAccess.Function> GetNotGivenFunctions(int parentId, int childId)
        {
            //IEnumerable<Function> parentRoleFunctions = GetRoleFucntionsByUser(parentId);
            IEnumerable<Broker.DataAccess.Function> parentRoleFunctions = GetAvaibleFunctionsForUser(parentId);
            IEnumerable<Broker.DataAccess.Function> childAvaliableFunctions = GetAvaliableFunctionsByUser(childId);

            List<Broker.DataAccess.Function> result = new List<Broker.DataAccess.Function>();

            foreach (Broker.DataAccess.Function roleFunction in parentRoleFunctions)
            {
                bool used = false;
                if (childAvaliableFunctions.Count() > 0) {
                    foreach (Broker.DataAccess.Function availableFunction in childAvaliableFunctions)
                    {
                        if (availableFunction.ID == roleFunction.ID) {
                            used = true;
                            break;
                        }
                    }
                }
                if(!used) {
                    result.Add(roleFunction);
                }
            }
            return result;
        }

        /// <summary>
        /// Metod koj gi vraka decata na specificiran korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista - IEnumerable(User)</returns>
        public IEnumerable<Broker.DataAccess.User> GetChildUsers(int userId)
        {
            return Broker.DataAccess.User.GetChildUsers(userId);
        }

        public List<Broker.DataAccess.User> GetManageChildUsers(int userId)
        {
            List<Broker.DataAccess.User> lst = Broker.DataAccess.User.GetChildUsers(userId).ToList();
            return lst.Where(c => c.Role.Name == Broker.DataAccess.Role.Broker || c.Role.Name == Broker.DataAccess.Role.BROKERAdmin || c.Role.Name == Broker.DataAccess.Role.SEAdmin || c.Role.Name == Broker.DataAccess.Role.SEBranch).ToList();
        }

        /// <summary>
        /// Metod koj vrsi azuriranje na funkciite za daden korisnik
        /// so novo definirani funkcii
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="functions"></param>
        public static void updateUser(int userId, List<int> functions) {
            List<Broker.DataAccess.Function> oldFunctions = GetCustomFunctionsByUser(userId).ToList();
            List<int> forDelete= new List<int>();
            List<int> forInsert= new List<int>();
            foreach (Broker.DataAccess.Function function in oldFunctions)
            {
                bool found=false;
                foreach(int newFunction in functions) {
                    if(newFunction == function.ID) {
                        found = true;
                        break;
                    }
                }
                if(!found) {
                    forDelete.Add(function.ID);
                }
            }

            foreach(int newFunction in functions) {
                bool insert=true;
                foreach (Broker.DataAccess.Function function in oldFunctions)
                {
                    if(newFunction == function.ID) {
                        insert = false;
                        break;
                    }
                }
                if(insert) {
                    forInsert.Add(newFunction);
                }
            }

            UsersFunction.DeleteUserFunctions(userId, forDelete);
            UsersFunction.InsertUserFunctions(userId, forInsert);
        }

        /// <summary>
        /// Metod so koj se vrsi popolnuvanje na pole so funkcii koi se
        /// preddefinirani za konkretniot korisnik od mnozestvoto na site funkcii
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <param name="list"></param>
        /// se povikuva kaj UserManagement\UserFunctions.aspx.cs za da se
        /// ispolni CheckBoxList so funkcii za daden korisnik
        public static void fillEnabled(int userId, bool[] enabled, List<int> list) {
            List<Broker.DataAccess.Function> functions = GetCustomFunctionsByUser(userId).ToList();
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
                enabled[c++]=found;
            }
        }


        public static IEnumerable<Broker.DataAccess.Function> GetAvaibleFunctionsForUser(int userId)
        {
            Broker.DataAccess.Role r = Broker.DataAccess.User.GetRoleByUserId(userId);
            List<Broker.DataAccess.Function> customFunctions = GetCustomFunctionsByUser(userId).Where(v => v.CanBeGiven == true).ToList();
            List<Broker.DataAccess.Function> fIEnum = Broker.DataAccess.Role.GetFucntionsByRole(r.ID).ToList();
            fIEnum.AddRange(customFunctions.AsEnumerable());
            return fIEnum.AsEnumerable();
                       
        }
    }
}