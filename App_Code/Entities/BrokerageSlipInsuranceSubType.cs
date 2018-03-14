using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageSlipInsuranceSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class BrokerageSlipInsuranceSubType:EntityBase<BrokerageSlipInsuranceSubType> {
        public static List<BrokerageSlipInsuranceSubType> GetByBrokerageSlip(int brokerageSlipID) {
            return Table.Where(c => c.BrokerageSlipID == brokerageSlipID).ToList();
        }
    }
}
