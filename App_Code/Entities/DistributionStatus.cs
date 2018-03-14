using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DistributionStatus
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class DistributionStatuse : EntityBase<DistributionStatuse> {

        public static string NEPOTVRDENA = "01";
        public static string POTVRDENA = "02";
        public static string VRATENA = "03";
        public static string STORNO = "04";

        public static DistributionStatuse GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }

    }
}