using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers;

/// <summary>
/// Summary description for Offer
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Offer:EntityBase<Offer> {

        public static Offer GetByNumber(string offerNumber) {
            return Table.Where(o => o.OfferNumber == offerNumber).SingleOrDefault();
        }

        public static bool ExistOfferNumber(string offerNumber){
            return (Table.Where(o => o.OfferNumber == offerNumber).SingleOrDefault() != null); 
        }

        public static Offer GetOffer(int id)
        {
            if (id > 0)
            {
                return Offer.Get(id);
            } else
            {
                Offer o = new Offer();
                o.OfferNumber = CodeGenerator.OfferCodeGenerator();
                return o;
            }
        }

        public void TestBeforeInsert() {
           //if (Order.GetByOrderNumber(offerNumber)==null) {
           //     ValidationErrors.Add("ORDERNUMBER_DOESNOT_EXISTS", "Не постои налог со овој број!!!");
           // }
            if(ExistOfferNumber(this.OfferNumber)){
                ValidationErrors.Add("OFFERNUMBER_EXIST", "Постои понуда со овој број!!!");
            }
        //    this.PerformCustomValidation();
        }

        public static void UpdateCurrentCost(decimal newCost, decimal oldCost, int offerID)
        {
            Offer o = Offer.Get(offerID);
            o.Cost -= oldCost;
            o.Cost += newCost;
            Table.Context.SubmitChanges();
        }
        

    }
}
