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

/// <summary>
/// Summary description for BrokerageOperator
/// </summary>
namespace Broker.DataAccess{
public partial class BrokerageOperator:EntityBase<BrokerageOperator>
{
  

    public static decimal GetValueForOperatorAndValues(decimal firstParameter, decimal secondParameter, BrokerageOperator bo) {
        if (bo.Code == "01") {
            return Decimal.Multiply(firstParameter, secondParameter);
        }
        if (bo.Code == "02") {
            return Decimal.Divide(firstParameter, secondParameter);
        }
        if (bo.Code == "03") {
            return Decimal.Add(firstParameter, secondParameter);
        }
        if (bo.Code == "04") {
            return Decimal.Subtract(firstParameter, secondParameter);
        }
        return 0;
    }
    
   


	}
}
