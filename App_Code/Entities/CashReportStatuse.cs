using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CashReportStatuse
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class CashReportStatuse:EntityBase<CashReportStatuse> {

        public static string ZA_ODOBRUVANjE = "1";
        public static string ODOBRENO = "2";
        public static string VRATENO = "3";

        public static CashReportStatuse GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }


    }
}
