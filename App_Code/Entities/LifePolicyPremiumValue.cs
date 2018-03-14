using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LifePolicyPremiumValue
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class LifePolicyPremiumValue : EntityBase<LifePolicyPremiumValue> {
        public static List<LifePolicyPremiumValue> GetByLifePolicy(int lifePolicyID) {
            return Table.Where(c => c.LifePolicyID == lifePolicyID).OrderBy(c => c.OrdinalNumberYear).ToList();
        }
    }
}
