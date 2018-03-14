using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LifeDealBrokerage
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class LifeDealBrokerage:EntityBase<LifeDealBrokerage> {
        public static List<LifeDealBrokerage> GetByLifeDeal(int lifeDealID) {
            return Table.OrderBy(c => c.LifeDealID == lifeDealID).OrderBy(c => c.OrdinalNumber).ToList();
        }
    }
}
