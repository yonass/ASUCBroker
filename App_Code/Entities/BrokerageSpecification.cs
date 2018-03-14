using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageSpecification
/// </summary>
namespace Broker.DataAccess {
    public partial class BrokerageSpecification : EntityBase<BrokerageSpecification> {

        public static List<BrokerageSpecification> GetForBrokerage(int brokerageBrokerageTypeID) {
            List<BrokerageSpecification> specificationsList = Table.Where(c => c.BrokerageBrokerageTypeID == brokerageBrokerageTypeID).ToList();
            return specificationsList;
        }
        public static BrokerageSpecification GetByValueAndType(int bbtID, decimal value) {
            List<BrokerageSpecification> bsList = Table.Where(c => c.BrokerageBrokerageTypeID == bbtID).ToList();
            bsList = bsList.Where(c => c.DownValue <= value).ToList();
            List<BrokerageSpecification> bsSubList = bsList.Where(c => c.UpValue >= value).ToList();
            if (bsSubList.Count == 0) {
                bsSubList = bsList.Where(c => c.UpValue == 0).ToList();
                
            }
            return bsSubList.First();
        }


    }
}
