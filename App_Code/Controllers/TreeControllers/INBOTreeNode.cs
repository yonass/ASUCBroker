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

namespace Broker.Controllers.Tree {

    /// <summary>
    /// Interfejs za jazel od dinamickoto drvo
    /// </summary>
    public interface INBOTreeNode {

        bool HasChildren(int userId);
        IEnumerable<INBOTreeNode> GetChildren(int userId);
        IEnumerable<INBOTreeNode> GetRoots(int userId);
        INBOTreeNode GetParent();

        object GetPropertyValue(string property);

        string Title { get;  }
        int ID { get; set; }
     //6   int OrderNumber { get; set; }
        INBOTreeNode GetById(int nodeId);
        
        
    }
}