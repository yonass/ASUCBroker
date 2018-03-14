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

using Broker.Controllers.Tree;

namespace Broker.DataAccess {

    public class SessionTree {

        public Dictionary<WebPage, bool> HasChildrenMap { get; set; }
        public Dictionary<WebPage, IEnumerable<INBOTreeNode>> GetChildrenMap { get; set; }
        public Dictionary<WebPage, IEnumerable<INBOTreeNode>> GetRootsMap { get; set; }
        public Dictionary<WebPage, INBOTreeNode> GetParentMap { get; set; }
        public Dictionary<int, INBOTreeNode> GetByIdMap { get; set; }

        public SessionTree() {
            HasChildrenMap = new Dictionary<WebPage, bool>();
            GetChildrenMap = new Dictionary<WebPage, IEnumerable<INBOTreeNode>>();
            GetRootsMap = new Dictionary<WebPage, IEnumerable<INBOTreeNode>>();
            GetParentMap = new Dictionary<WebPage, INBOTreeNode>();
            GetByIdMap = new Dictionary<int, INBOTreeNode>();
        }

        public bool HasChildren(WebPage page) {
            return HasChildrenMap[page];
        }

        public IEnumerable<INBOTreeNode> GetChildren(WebPage page) {
            return GetChildrenMap[page];
        }

        public IEnumerable<INBOTreeNode> GetRoots(WebPage page) {
            return GetRootsMap[page];
        }

        public INBOTreeNode GetParent(WebPage page) {
            return GetParentMap[page];
        }

        public INBOTreeNode GetById(int nodeId) {
            return GetByIdMap[nodeId];
        }


        public static bool IsInitialized() {
            return (HttpContext.Current.Session["TreeInitialized"] != null);
        }

        public static SessionTree GetSessionInstance() {
            SessionTree tree = (SessionTree)HttpContext.Current.Session["SessionTree"];
            if (tree == null) {
                tree = new SessionTree();
                HttpContext.Current.Session["SessionTree"] = tree;
            }
            return tree;
        }

        public static void UpdateSessionInstance(SessionTree newInstance) {
            HttpContext.Current.Session["SessionTree"] = newInstance;
        }

    }
}
