using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageParametersBrokerageType
/// </summary>
namespace Broker.DataAccess{
public partial class BrokerageParametersBrokerageType:EntityBase<BrokerageParametersBrokerageType> {

    public static List<BrokerageParametersBrokerageType> GetForBrokerageType(int brokTypeID) {
        List<BrokerageParametersBrokerageType> bpbtList = Table.Where(c => c.BrokerageTypeID== brokTypeID).ToList();
        return bpbtList;
    }


    }
}
