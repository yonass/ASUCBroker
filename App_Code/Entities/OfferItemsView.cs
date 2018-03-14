using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OfferItemsView
/// </summary>
namespace Broker.DataAccess {
    public partial class OfferItemsView : EntityBase<OfferItemsView> {

        public static List<OfferItemsView> GetByOfferNumber(string offerNumber) {
            return Table.Where(o => o.OfferNumber == offerNumber).ToList();
        }
        public static OfferItemsView GetByOfferItemID(int offerItemID) {
            return Table.Where(o => o.OfferItemID==offerItemID).SingleOrDefault();
        }
    }
}
