using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OfferItemInfo
/// </summary>
/// 
namespace Broker.Controllers.OfferItemsController {
    public class OfferItemInfo {

        public string OfferNumber { get; set; }
        public string OfferID { get; set; }
        public string OrderNumber { get; set; }
        public string InsuranceCompanyID { get; set; }
        public string InsuranceTypeID { get; set; }
        public string InsuranceSubTypeID { get; set; }
        public string BrokerageID { get; set; }
        public string Percentage { get; set; }
        public string Cost { get; set; }


        public OfferItemInfo() {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
