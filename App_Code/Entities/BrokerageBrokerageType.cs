using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageBrokerageType
/// </summary>
namespace Broker.DataAccess{
public partial class BrokerageBrokerageType:EntityBase<BrokerageBrokerageType>
{

    public static List<BrokerageBrokerageType> GetForBrokerage(int brokerageID) {
        return Table.Where(c => c.BrokerageID == brokerageID && c.IsActive == true).ToList();

    }



    }
}
