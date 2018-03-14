using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IncomingFacturePolicyItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class IncomingFacturePolicyItem : EntityBase<IncomingFacturePolicyItem> {
        public static List<IncomingFacturePolicyItem> GetByIncomingFactureID(int factureID) {
            return Table.Where(c => c.IncomingFactureID == factureID && c.IsActive == true).ToList();   
        }

        public static IncomingFacture GetByPolicyItem(int policyItemID) {
            try {
                return Table.Where(c => c.PolicyItemID == policyItemID && c.IsActive == true).Select(c=>c.IncomingFacture).SingleOrDefault();
            } catch {
                return null;
            }
        }
    }
}
