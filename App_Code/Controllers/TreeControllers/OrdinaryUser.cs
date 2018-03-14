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
    /// Klasa za rabota so obicen korisnik - OrdinaryUser
    /// (nasleduva od klasata User)
    /// </summary>
    public class OrdinaryUser : User {

        /// <summary>
        /// Default-en konstruktor na klasata
        /// </summary>
        public OrdinaryUser() {
        }

        /// <summary>
        /// Konstruktor na klasata koj se povikuva so vlezen parametar 
        /// jazel na dinamickoto drvo - INBOTreeNode
        /// </summary>
        /// <param name="node"></param>
        public OrdinaryUser(INBOTreeNode node) {
            this.ID = node.ID;
            this.Title = node.Title;
        }

        /// <summary>
        /// Konstruktor na klasata koj se povikuva so vlezen parametar
        /// korisnik - User
        /// </summary>
        /// <param name="u"></param>
        public OrdinaryUser(User u) {
            this.ID = u.ID;
            this.Title = u.Title;
            this.Role = u.Role;
            //this.AbsoluteUrl = u.AbsoluteUrl;
        }

        /// <summary>
        /// Metod so koj se vrsi override na funkcijata GetChildren
        /// i se polni lista od jazli na dinamickoto drvo pri sto
        /// se zapazuvaat relaciite pomegu korisnicite
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista od jazli (IEnumerable(INBOTreeNode))</returns>
        public override IEnumerable<INBOTreeNode> GetChildren(int userId) {
            List<INBOTreeNode> result = new List<INBOTreeNode>();
            foreach(UsersParentship parentship in UsersParentship.GetChildren(this.ID)) {
                User user = (User)base.GetById(parentship.ChildID);
                OrdinaryUser u = new OrdinaryUser(user);

                if(this.Role.Name == RolesInfo.SIMTAdmin && u.Role.Name==RolesInfo.BROKERAdmin) {
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

                if(this.Role.Name == RolesInfo.BROKERAdmin && u.Role.Name==RolesInfo.Broker) {
                    result.Add(u);
                }

            }
            return result;
        }

        /// <summary>
        /// Metod so koj se vrsi override na funkcijata GetRoots
        /// so cel da se dobijat korenite vo dinamickoto drvo za
        /// specificiran korisnik
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista od jazli (IEnumerable(INBOTreeNode))</returns>
        public override IEnumerable<INBOTreeNode> GetRoots(int userId) {
            User _root =  User.Get(userId);
            List<INBOTreeNode> roots = new List<INBOTreeNode>();
            roots.Add(new OrdinaryUser(_root));
            return roots;
        }

        /// <summary>
        /// Metod so koj se vrsi override na funkcijata GetById,
        /// pri sto se vraka nova instanca od klasata obicen korisnik
        /// - OrdinaryUser za specificiran jazel od drvoto i polnenje na
        /// soodvetniot jazel
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns>jazel - INBOTreeNode</returns>
        public override INBOTreeNode GetById(int nodeId) {
            User user = (User)base.GetById(nodeId);
            return new OrdinaryUser(user);
        }
    }
}