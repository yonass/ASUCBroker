using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PolicyExtendInformation
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class PolicyExtendInformation : EntityBase<PolicyExtendInformation> {

        public static PolicyExtendInformation GetByPolicyItemAndControl(int policyItemID, int controlID) {
            return Table.Where(pei => pei.PolicyItemID == policyItemID && pei.ControlID == controlID).SingleOrDefault();
        }

        public static PolicyExtendInformation GetByValue(string value) {
            List<PolicyExtendInformation> list = Table.Where(pei => pei.Value == value).ToList();
            if (list.Count > 0) {
                return list.Last();
            } else {
                return null;
            }
        }

        public static PolicyItem SearchPolicyItemByControl(int controlID, string controlValue) {
            List<PolicyExtendInformation> peiList = Table.Where(c => c.ControlID == controlID && c.Value == controlValue && c.PolicyItem.Policy.Discard == false).ToList();
            if (peiList.Count == 1) {
                return peiList.First().PolicyItem;
            } else {
                if (peiList.Count == 0) {
                    return null;
                }
                return peiList.OrderBy(c => c.PolicyItem.Policy.ApplicationDate).ToList().First().PolicyItem;
            }
        }

    }
}
