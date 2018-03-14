using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageTypeClasses
/// </summary>
namespace Broker.DataAccess{
public class BrokerageTypeClasses {
    
   


    }

public class RANGE {

    public decimal BrokerageValue;
    public static decimal GetForCompanyAndPeriod(int insuranceCompanyID, DateTime startDate, DateTime endDate) {
        List<Policy> pList = Policy.GetByCompanyAndPeriod(insuranceCompanyID, startDate, endDate);
        decimal value = 0;
        foreach (Policy p in pList) {
            value += p.TotalSum;
        }
    return value;
    }


}


}
