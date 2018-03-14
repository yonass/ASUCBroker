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
using System.Reflection;
using Broker.Controllers.EmployeeManagement;

using Broker.Controllers.Tree;

namespace Broker.DataAccess {

    /// <summary>
    /// Klasa za rabota so korisnik - User
    /// (nasleduva od klasata INBOTreeNode)
    /// </summary>
    public partial class User : INBOTreeNode {
        // TODO: Namesto celo vreme od baza na se prebaruva vo HashSet<INBOTree>
        HashSet<INBOTreeNode> children = null;
        HashSet<INBOTreeNode> parents = null;

        /// <summary>
        /// Interfejs za promenliva AbsoluteUrl koja 
        /// vraka "#"
        /// </summary>
        public string AbsoluteUrl {
            get {
                return "#";
            }
        }

        /// <summary>
        /// Metod koj vraka vrednost vistina
        /// dokolku specificiraniot korisnik ima deca
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        public bool HasChildren(int userId) {
            return UsersParentship.HasChildren(this.ID);
        }

        /// <summary>
        /// Virtuelen metod GetChildren preku koj
        /// se polni lista od jazli na dinamickoto drvo pri sto
        /// se zapazuvaat relaciite pomegu korisnicite
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista od jazli (IEnumerable(INBOTreeNode))</returns>
        public virtual IEnumerable<INBOTreeNode> GetChildren(int userId) {
            List<INBOTreeNode> result = new List<INBOTreeNode>();
            foreach(UsersParentship parentship in UsersParentship.GetChildren(this.ID)) {
                User u = User.Get(parentship.ChildID);
                if(this.Role.Name == RolesInfo.SIMTAdmin && u.Role.Name==RolesInfo.BROKERAdmin) {
                    result.Add(u);
                }
                if(this.Role.Name == RolesInfo.BROKERAdmin && u.Role.Name==RolesInfo.Broker) {
                    result.Add(u);
                }
                if(this.Role.Name == RolesInfo.SIMTAdmin && u.Role.Name==RolesInfo.Broker) {
                    result.Add(u);
                }
                if(this.Role.Name == RolesInfo.SIMTAdmin && u.Role.Name==RolesInfo.SEAdmin) {
                    result.Add(u);
                }
                if(this.Role.Name == RolesInfo.SEAdmin && u.Role.Name==RolesInfo.Broker) {
                    result.Add(u);
                }
                
            }
            return result;
        }

        /// <summary>
        /// Virtuelen metod GetRoots
        /// so cel da se dobijat korenite vo dinamickoto drvo za
        /// specificiran korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista od jazli (IEnumerable(INBOTreeNode))</returns>
        public virtual IEnumerable<INBOTreeNode> GetRoots(int userId) {
            User _root =  Table.Where(v => v.ID == userId).SingleOrDefault();
            List<INBOTreeNode> roots = new List<INBOTreeNode>();
            roots.Add(_root);
            return roots;
        }

        /// <summary>
        /// Ovoj metod sekogas vraka null
        /// </summary>
        /// <returns>INBOTreeNode</returns>
        public INBOTreeNode GetParent() {
            return null;
        }

        /// <summary>
        /// Metod koj vraka vrednost za specificirano svojstvo
        /// </summary>
        /// <param name="property"></param>
        /// <returns>object</returns>
        public object GetPropertyValue(string property) {
            foreach(PropertyInfo prop in this.GetType().GetProperties()) {
                if(prop.Name==property) {
                    return prop.GetValue(this, null);
                }
            }
            return null;
        }

        /// <summary>
        /// Virtuelen metod koj go vraka korisnikot (User)
        /// spored specificiran jazel od dinamickoto drvo
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns>INBOTreeNode</returns>
        public virtual INBOTreeNode GetById(int nodeId) {
            return User.Get(nodeId);
        }

        /// <summary>
        /// Interfejs za promenlivata Title
        /// sto se soodejstvuva na korisnickoto ime 
        /// na korisnikot - UserName
        /// </summary>
        public string Title {
            get {
                return this.UserName;
            }
            set {
                this.UserName = value;
            }
        }

    }
}