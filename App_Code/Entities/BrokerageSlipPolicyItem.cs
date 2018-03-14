using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageSlipPolicyItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class BrokerageSlipPolicyItem : EntityBase<BrokerageSlipPolicyItem> {
        public static List<BrokerageSlipPolicyItem> GetByBrokerageSlip(int brokerageSlipID) {
            return Table.Where(c => c.BrokerageSlipID == brokerageSlipID).ToList();
        }
    }
}