using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DistributionDocumentItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class DistributionDocumentItem : EntityBase<DistributionDocumentItem> {
        public static List<DistributionDocumentItem> GetByDistributionDocument(int distributionDocumentID) {
            return Table.Where(c => c.DistributionDocumentID == distributionDocumentID).ToList();
        }
    }
}
