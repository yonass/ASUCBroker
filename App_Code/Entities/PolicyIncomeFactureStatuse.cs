using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PolicyIncomeFactureStatuse
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class PolicyIncomeFactureStatuse : EntityBase<PolicyIncomeFactureStatuse> {

        public static string NE_E_FAKTURIRANA = "1";
        public static string CELOSNO_FAKTURIRANA = "2";
        public static string DELUMNO_FAKTURIRANA = "3";

        public static PolicyIncomeFactureStatuse GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }

    }
}
