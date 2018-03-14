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


namespace Broker.Controllers.Tree {

    /// <summary>
    /// Klasa za specificiranje na pogledot na podatocniot izvor na dinamickoto drvo 
    /// (nasleduva od klasata HierarchicalDataSourceView)
    /// </summary>
    public class NBOTreeDataSourceView : HierarchicalDataSourceView {
        private string _viewPath;
        private int _userId;
        private string _typeName;

        /// <summary>
        /// Konstruktor na klasata koj se povikuva so eden parametar
        /// </summary>
        /// <param name="viewPath"></param>
        public NBOTreeDataSourceView(string viewPath) {
            _viewPath = viewPath;
        }

        /// <summary>
        /// Konstruktor na klasata koj se povikuva so dva parametri
        /// </summary>
        /// <param name="viewPath"></param>
        /// <param name="userId"></param>
        public NBOTreeDataSourceView(string viewPath, int userId) {
            _viewPath = viewPath;
            _userId = userId;
        }

        /// <summary>
        /// Konstruktor na klasata koj se povikuva so tri parametri
        /// </summary>
        /// <param name="viewPath"></param>
        /// <param name="userId"></param>
        /// <param name="typeName"></param>
        public NBOTreeDataSourceView(string viewPath, int userId, string typeName) {
            _viewPath = viewPath;
            _userId = userId;
            _typeName = typeName;
        }
        bool first=true;

        /// <summary>
        /// Metod koj vrsi override na Select od klasata IHierarchicalEnumerable so cel
        /// koj vo zavisnost od vrednosta na atributot _viewPath go polni
        /// numeriranoto mnozestvo IHierarchicalEnumerable so zaglavija (ako _viewPath e prazno)
        /// ili so decata od hierarhijata na konkretniot korisnik (ako _viewPath ima vrednost)
        /// </summary>
        /// <returns>NBOTreeHierarchicalEnumerable</returns>
        public override IHierarchicalEnumerable Select() {
            NBOTreeHierarchicalEnumerable result = new NBOTreeHierarchicalEnumerable();

            List<INBOTreeNode> roots=new List<INBOTreeNode>();

            if(_viewPath=="") {
                if(_typeName == "WebPage") {
                    roots.AddRange(new WebPage().GetRoots(_userId));
                }
                if(_typeName == "User") {
                    roots.AddRange(new Broker.DataAccess.User().GetRoots(_userId));
                }

                if(_typeName == "OrdinaryUser") {
                    roots.AddRange(new OrdinaryUser().GetRoots(_userId));
                }

                if(_typeName == "SEUser") {
                    roots.AddRange(new SEUser().GetRoots(_userId));
                }

            } else {
                if(_typeName == "WebPage") {
                    INBOTreeNode node = new WebPage().GetById(Convert.ToInt32(_viewPath));
                    roots.AddRange(node.GetChildren(_userId));
                }
                if(_typeName == "User") {
                    INBOTreeNode node = new Broker.DataAccess.User().GetById(Convert.ToInt32(_viewPath));
                    roots.AddRange(node.GetChildren(_userId));
                }

                if(_typeName == "OrdinaryUser") {
                    INBOTreeNode node = new OrdinaryUser().GetById(Convert.ToInt32(_viewPath));
                    roots.AddRange(node.GetChildren(_userId));
                }

                if(_typeName == "SEUser") {
                    INBOTreeNode node = new SEUser().GetById(Convert.ToInt32(_viewPath));
                    roots.AddRange(node.GetChildren(_userId));
                }

            }
            if(first) {
                foreach(INBOTreeNode node in roots) {
                    result.Add(new NBOTreeHierarchyData(node, _userId, _typeName));
                }
            }
            first=false;
            return result;
        }
    }
}