using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DistributionDocType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class DistributionDocType : EntityBase<DistributionDocType> {

        public static string ISPRATNICA = "01";
        public static string POVRATNICA = "02";
        public static string PRIEM = "03";

        public static DistributionDocType GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }

    }
}