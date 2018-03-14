using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageType
/// </summary>
namespace Broker.DataAccess {
    public partial class BrokerageType : EntityBase<BrokerageType> {


        public static decimal GetValue(int brokerageTypeID, List<decimal> values) {
            List<BrokerageTypeBrokerageOperator> btboList = BrokerageTypeBrokerageOperator.GetForBrokerageType(brokerageTypeID);
            btboList = btboList.OrderBy(c => c.OrderNumber).ToList();
            List<BrokerageOperator> boList = new List<BrokerageOperator>();
            foreach (BrokerageTypeBrokerageOperator btbo in btboList) {
                boList.Add(btbo.BrokerageOperator);
            }
            decimal result = values[0];
            if (boList.Count == 0) {
                return result;
            } else {
                for (int i = 0; i < boList.Count; i++) {
                    result = BrokerageOperator.GetValueForOperatorAndValues(result, values[i + 1], boList[i]);
                }
            }
            return result;
        }
    }
}
