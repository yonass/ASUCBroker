using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DistributionDocument
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class DistributionDocument : EntityBase<DistributionDocument> {
        public static string GetNextNumber() {
            List<DistributionDocument> list = Table.OrderBy(c => c.ID).ToList();
            if (list.Count > 0) {
                return (Convert.ToInt32(list.Last().DocumentNumber) + 1).ToString();
            } else {
                return "1";
            }
        }

        public static DistributionDocument GetByDocumentNumber(string documentNumber) {
            return Table.Where(c => c.DocumentNumber == documentNumber).SingleOrDefault();
        }
    }
}