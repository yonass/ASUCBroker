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
using System.Collections;
using System.Collections.Generic;

namespace Broker.Controllers.Tree {
    /// <summary>
    /// Klasa za specificiranje na podatocniot izvor na dinamickoto drvo 
    /// (nasleduva od klasite HierarchicalDataSourceControl i IHierarchicalDataSource)
    /// </summary>
    public class NBOTreeDataSource : HierarchicalDataSourceControl, IHierarchicalDataSource {

        /// <summary>
        /// Konstruktor na klasata
        /// </summary>
        public NBOTreeDataSource() : base() { }

        private NBOTreeDataSourceView view = null;
        private int _userId;
        private string _typeName;

        /// <summary>
        /// Metod koj vrsi override na GetHierarchicalView za da se povikuva 
        /// osven so preddefiniraniot vlezen parametar i so dva drugi parametri
        /// - _userID i _typeName
        /// </summary>
        /// <param name="viewPath"></param>
        /// <returns>objekt od tip NBOTreeDataSourceView</returns>
        protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath) {
            view = new NBOTreeDataSourceView(viewPath,_userId, _typeName );
            return view;
        }

        /// <summary>
        /// Metod koj vrsi override na CreateControlCollection koj vraka
        /// nova instanca od klasata ControlCollection povikana so parametar
        /// - pokazuvac kon konkretnava klasa
        /// </summary>
        /// <returns>ControlCollection</returns>
        protected override ControlCollection CreateControlCollection() {
            return new ControlCollection(this);
        }

        /// <summary>
        /// Interfejs za specificiranje na ID na korisnik (UserID)
        /// - atribut od klasata
        /// </summary>
        public int UserID {
            get {
                return this._userId;
            }
            set {
                this._userId=value;
            }
        }

        /// <summary>
        /// Interfejs za specificiranje na imeto na tipot na atributot
        /// TypeName od klasava
        /// </summary>
        public string TypeName {
            get {
                return _typeName;
            }
            set {
                this._typeName = value;
            }
        }
        
    }


    
}

