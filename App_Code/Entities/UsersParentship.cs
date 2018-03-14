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

    public partial class UsersParentship : EntityBase<UsersParentship> {

        /// <summary>
        /// Vnesuva nov zapis vo tabelata
        /// </summary>
        /// <param name="parentId">int parentID</param>
        /// <param name="childId">int childID</param>
        /// Se povikuva vo UsersParentships vo metodite:
        /// SetParents i dvata SetChildren 
        public static void InsertUsersParentship(int parentId, int childId) {
            UsersParentship parentship = new UsersParentship();
            parentship.ParentID = parentId;
            parentship.ChildID = childId;
            UsersParentship.Insert(parentship);
        }

        /// <summary>
        /// Vnesuva "roditeli" za opredelen user
        /// </summary>
        /// <param name="childId">int childID</param>
        /// <param name="parents">List(user) parents</param>
        /// se povikuva vo EmployeeCreators\EmpoyeeController.cs
        public static void SetParents(int childId, List<User> parents) {
            foreach(User parent in parents) {
                InsertUsersParentship(parent.ID, childId);
            }
        }

        /// <summary>
        /// Vnesuva "deca" za opredelen user
        /// </summary>
        /// <param name="childId">int childID</param>
        /// <param name="parents">List(user) chidren</param>
        /// se povikuva vo EmployeeCreators\EmpoyeeController.cs
        public static void SetChildren(int parentId, List<User> children) {
            foreach(User child in children) {
                InsertUsersParentship(parentId, child.ID);
            }
        }

        /// <summary>
        /// Vnesuva "deca" za opredelen user
        /// </summary>
        /// <param name="childId">int childID</param>
        /// <param name="parents">List(int) chidren</param>
        /// Se povikuva vo EmployeeCreators\EmpoyeeController.cs
        public static void SetChildren(int parentId, List<int> children) {
            foreach(int childId in children) {
                InsertUsersParentship(parentId, childId);
            }
        }


        /// <summary>
        /// Proverka dali ima "deca" user-ot
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <returns>Vrakja logicka promenliva (bool)</returns>
        /// Se povikuva vo TreeControllers\User.cs
        public static bool HasChildren(int userId) {
            return (Table.Where(v => v.ParentID==userId).Count()>0);
        }

        // TODO: Da se vrakja IEnumerable<User> so LINQ Join
        /// <summary>
        /// Vrakja lista od UserParentship za opredelen User
        /// </summary>
        /// <param name="userId">int userID</param>
        /// <returns>Vrakja numerirana lista (IEnumerable)</returns>
        /// Se povikuva vo TreeControllers\OrdinaryUser.cs, TreeControllers\SEUser.cs
        /// i TreeControllers\User.cs
        public static IEnumerable<UsersParentship> GetChildren(int userId) {
            return Table.Where(v => v.ParentID == userId);
        }
        /// <summary>
        /// Vrakja lista od UsersParentships so SE "roditeli"
        /// </summary>
        /// <param name="userID">int UserID</param>
        /// <returns>Vrakja lista (List)</returns>
        /// Ne se povikuva
        public static List<UsersParentship> GetSEParents(int userID) {
           return Table.Where(n => n.ChildID == userID).ToList();
        
        }
    }
}