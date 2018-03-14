using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OfferItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class OfferItem : EntityBase<OfferItem> {

        public static List<OfferItem> GetOfferItems(int OfferID) {
            return Table.Where(oi => oi.OfferID == OfferID).ToList();
        }

        public static List<OfferItem> GetOfferItems() {
            return null;
        }

        public static List<OfferItem> GetByOfferNumber(string offerNumber) {
            return Table.Where(oi => oi.Offer.OfferNumber == offerNumber).ToList();
        }

        public static List<OfferItem> GetByOfferID(int offerID) {
            return Table.Where(oi => oi.OfferID == offerID).ToList();
        }
        public static void UpdateCurrentCost(OfferItem oldEntity, OfferItem newEntity)
        {
            OfferItem oi = OfferItem.Get(newEntity.ID);
            oi.Cost = newEntity.Cost;
            Table.Context.SubmitChanges();

            Offer o = Offer.Get(newEntity.OfferID);
            o.Cost -= oldEntity.Cost;
            o.Cost += newEntity.Cost;
            Offer.Table.Context.SubmitChanges();
        }
    }
}
	
