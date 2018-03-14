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
    /// Klasa koja definira hierarhisko mnozestvo za dinamickoto drvo
    /// (nasleduva od klasite ArrayList i IHierarchicalEnumerable)
    /// </summary>
    public class NBOTreeHierarchicalEnumerable : ArrayList, IHierarchicalEnumerable {

        /// <summary>
        /// Konstruktor na klasata
        /// </summary>
        public NBOTreeHierarchicalEnumerable()
            : base() {
        }

        /// <summary>
        /// Metod koj vrsi kastiranje na vlezniot parametar vo
        /// objekt od tip IHierarchyData
        /// </summary>
        /// <param name="enumeratedItem"></param>
        /// <returns>IHierarchyData</returns>
        public IHierarchyData GetHierarchyData(object enumeratedItem) {
            return enumeratedItem as IHierarchyData;
        }
    }
}