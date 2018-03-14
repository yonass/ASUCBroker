using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LifePolicyFactureItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class LifePolicyFactureItem : EntityBase<LifePolicyFactureItem> {
        public static void InsertForFacture(FactureItem fi, List<LifePolicy> piList) {
            foreach (LifePolicy pi in piList) {
                LifePolicyFactureItem pifi = new LifePolicyFactureItem();
                pifi.FactureItemID = fi.ID;
                pifi.LifePolicyD = pi.ID;
                pifi.Insert();
            }
        }

        public static List<LifePolicyFactureItem> GetByFactureItemID(int factureItemID){
            return Table.Where(c=>c.FactureItemID == factureItemID).ToList();
        }
    }
}
