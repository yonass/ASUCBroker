using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IncomingFactureType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class IncomingFactureType:EntityBase<IncomingFactureType> {
        public static string ZA_POLISI = "1";
        public static string ZA_BONUS = "2";
        public static string ZA_TROSOCI = "3";

        public static IncomingFactureType GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }
    }
}
