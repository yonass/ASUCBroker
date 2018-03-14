using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for BrokerageController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class BrokerageController {
        public static void ValidateInsertCode(int dealID, int insuranceSubTypeID) {
            Brokerage brokerage = new Brokerage();
            brokerage.DealID = dealID;
            brokerage.InsuranceSubTypeID = insuranceSubTypeID;
            brokerage.TestBeforeInsert();
        }
    }
}
