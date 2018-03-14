using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RollBackDistributionStatuse
/// </summary>
namespace Broker.DataAccess {
    public partial class RollBackDistributionStatuse : EntityBase<RollBackDistributionStatuse> {
        public static string NEPOTVRDENA_OD_FILIJALA = "01";
        public static string POTVRDENA_OD_FILIJALA = "02";
        public static string STORNIRANA_OD_FILIJALA = "03";
        public static string NEPOTVRDENA_OD_KOMPANIJA = "04";
        public static string POTVRDENA_OD_KOMPANIJA = "05";
        public static string STORNIRANA_OD_KOMPANIJA = "06";
        public static string ZAVRSENA_OD_FILIJALA= "07";

        public static RollBackDistributionStatuse GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }


    }
}
