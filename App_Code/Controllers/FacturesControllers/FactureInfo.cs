using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FactureInfo
/// </summary>
namespace Broker.Controllers.FacturesControllers {
    [Serializable]
    public class FactureInfo {
        public FactureInfo() {
            //
            // TODO: Add constructor logic here
            //
        }

        public int ID { get; set; }
        public string PolicyNumber { get; set; }
        public bool IsForFacturing { get; set; }
        public decimal PremiumValue { get; set; }
        public decimal PaidValue { get; set; }

    }


    [Serializable]
    public class PaymentInfo {
        public PaymentInfo() {
            //
            // TODO: Add constructor logic here
            //
        }

        public int ID { get; set; }
        public string PolicyNumber { get; set; }
        public decimal Value { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public bool IsForCashReporting { get; set; }

    }


    [Serializable]
    public class PolicyForSlipsInfo {
        public PolicyForSlipsInfo() {
            //
            // TODO: Add constructor logic here
            //
        }

        public int ID { get; set; }
        public string PolicyNumber { get; set; }
        public bool IsForFacturing { get; set; }
        public int InsuranceSubTypeID { get; set; }
        public int? BrokerageID { get; set; }
        public int? PacketBrokerageID { get; set; }

    }
}
