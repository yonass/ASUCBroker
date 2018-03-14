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

using Broker.DataAccess;
using Broker.Controllers.Tree;
using Broker.Controllers.EmployeeManagement;

namespace Broker.DataAccess {

    /// <summary>
    /// Klasa za gradenje na web-stranite kako elementi na dinamickoto drvo
    /// (nasleduva od klasata INBOTreeNode)
    /// </summary>
    public partial class WebPage : INBOTreeNode {

        /// <summary>
        /// Interfejs za promenlivata AbsoluteUrl koj ja vraka
        /// relativnata pateka na url-to kon web-stranata
        /// </summary>
        public string AbsoluteUrl {
            get {
                string serverHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
                string absoluteUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"]; ;
                if (this.RelativeUrl == "#") {
                    return "http://" + serverHost + absoluteUrl + "#";
                }

                return "http://" + serverHost + "/" + this.RelativeUrl;
            }
        }

        // vo methodite nadolu se gledat samo WebPages za daden User so userId

        // TODO: da se prepravat queries so LINQ Join ( vo slednite 3 metodi)
        // Ili prvo da se izvadat vo HashSet pa potoa da se koristi HashSet::Contains()
        
        /// <summary>
        /// Metod koj vraka vrednost vistina
        /// dokolku specificiraniot korisnik ima deca
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        public bool HasChildren(int userId) {
            SessionTree tree = SessionTree.GetSessionInstance();
            if (tree.HasChildrenMap.ContainsKey(this)) {
                return tree.HasChildren(this);
            }

            bool hasChildren = false;
            foreach (WebPage page in this.WebPages) {
                if (page.RelativeUrl != "#" && UsersWebPage.EntityExists(userId, page.ID)) {
                    hasChildren = true;
                    break;
                }
                if (page.RelativeUrl == "#" && page.HasChildren(userId)) {
                    hasChildren = true;
                    break;
                }
            }
            tree.HasChildrenMap.Add(this, hasChildren);
            SessionTree.UpdateSessionInstance(tree);
            return hasChildren;
        }

        /// <summary>
        /// Metod vraka lista od jazli na dinamickoto drvo
        /// so elementi - deca na konkreten korisnik (userID)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista od jazli (IEnumerable(INBOTreeNode))</returns>
        public IEnumerable<INBOTreeNode> GetChildren(int userId) {
            SessionTree tree = SessionTree.GetSessionInstance();
            if (tree.GetChildrenMap.ContainsKey(this)) {
                return tree.GetChildren(this);
            }

            List<INBOTreeNode> children = new List<INBOTreeNode>();
            IEnumerable<WebPage> WebPages = this.WebPages;
            WebPages = WebPages.OrderBy(s => s.OrderNumber);
            foreach (WebPage page in WebPages) {
                if (page.RelativeUrl != "#" && UsersWebPage.EntityExists(userId, page.ID)) {
                    children.Add(page);
                }
                if (page.RelativeUrl == "#") {
                    if (page.HasChildren(userId)) {
                        children.Add(page);
                    }
                }
            }
            
            tree.GetChildrenMap.Add(this, children);
            SessionTree.UpdateSessionInstance(tree);
            return children;
        }

        /// <summary>
        /// Metod vraka lista od jazli na dinamickoto drvo
        /// so elementi - koreni na konkreten korisnik (userID)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>numerirana lista od jazli (IEnumerable(INBOTreeNode))</returns>
        public IEnumerable<INBOTreeNode> GetRoots(int userId) {
            SessionTree tree = SessionTree.GetSessionInstance();
            if (tree.GetRootsMap.ContainsKey(this)) {
                return tree.GetRoots(this);
            }

            List<WebPage> _roots = (Select().Where(w => w.ParentID == null).ToList());
            List<INBOTreeNode> roots = new List<INBOTreeNode>();
            foreach (WebPage page in _roots) {
                roots.Add((INBOTreeNode)page);
            }

            List<INBOTreeNode> result = new List<INBOTreeNode>();
            foreach (WebPage root in roots) {
                foreach (WebPage page in root.WebPages) {
                    if (page.RelativeUrl == "#" && page.HasChildren(userId)) {
                        result.Add(root);
                        break;
                    }
                    if (page.RelativeUrl != "#" && UsersWebPage.EntityExists(userId, page.ID)) {
                        result.Add(root);
                        break;
                    }
                }
            }
            tree.GetRootsMap.Add(this, result);
            SessionTree.UpdateSessionInstance(tree);
            return result;
        }

        /// <summary>
        /// Metod koj go vraka roditelot na dinamickoto drvo
        /// </summary>
        /// <returns>jazel - INBOTreeNode</returns>
        public INBOTreeNode GetParent() {
            SessionTree tree = SessionTree.GetSessionInstance();
            if (tree.GetParentMap.ContainsKey(this)) {
                return tree.GetParent(this);
            }
            tree.GetParentMap.Add(this, this.WebPage1);
            SessionTree.UpdateSessionInstance(tree);
            return this.WebPage1;
        }

        /// <summary>
        /// Metod koj vraka vrednost za specificirano svojstvo
        /// </summary>
        /// <param name="property"></param>
        /// <returns>object</returns>
        public object GetPropertyValue(string property) {
            foreach (PropertyInfo prop in this.GetType().GetProperties()) {
                if (prop.Name == property) {
                    return prop.GetValue(this, null);
                }
            }
            return null;
        }

        /// <summary>
        /// Metod so koj se vraka nova instanca od klasata WebPage
        /// za specificiran jazel od drvoto i polnenje na
        /// soodvetniot jazel
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public INBOTreeNode GetById(int nodeId) {
            SessionTree tree = SessionTree.GetSessionInstance();
            if (tree.GetByIdMap.ContainsKey(nodeId)) {
                return tree.GetById(nodeId);
            }
            WebPage page = WebPage.Get(nodeId);
            tree.GetByIdMap.Add(nodeId, page);
            SessionTree.UpdateSessionInstance(tree);

            return page;
        }

        /// <summary>
        /// Metod koj vrsi override na funkcijata GetHashCode, 
        /// taka sto ID-to na objektot od ovaa klasa stanuva
        /// nov hash-code - go identifikuva objektot
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode() {
            return this.ID;
        }

        /// <summary>
        /// Metod koj vrsi override na funkcijata Equals, 
        /// taka sto izednacuvanjeto na instanci na objekti od
        /// ovaa klasa se identifikuvaat preku ID-a
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bool</returns>
        public override bool Equals(object obj) {
            return (this.ID == ((WebPage)obj).ID);
        }
    }
}
