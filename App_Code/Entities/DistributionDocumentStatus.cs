using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DistributionDocumentStatus
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class DistributionDocumentStatuse : EntityBase<DistributionDocumentStatuse> {

        public static string ZA_PRIMANjE = "01";
        public static string PRIMEN = "02";
        public static string STORNIRAN = "03";
        public static string NEPRIFATEN = "04";

        public static DistributionDocumentStatuse GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }

    }
}