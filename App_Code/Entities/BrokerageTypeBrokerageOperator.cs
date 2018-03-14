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

/// <summary>
/// Summary description for BrokerageTypeBrokerageOperator
/// </summary>
namespace Broker.DataAccess{
    public partial class BrokerageTypeBrokerageOperator : EntityBase<BrokerageTypeBrokerageOperator> {

        public static List<BrokerageTypeBrokerageOperator> GetForBrokerageType(int brokerageID) {
            List<BrokerageTypeBrokerageOperator> btboList = Table.Where(c => c.BrokerageTypeID == brokerageID).ToList();
            return btboList;
            
        }
    }
	
}
