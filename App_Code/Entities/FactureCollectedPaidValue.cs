using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FactureCollectedPaidValue
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class FactureCollectedPaidValue:EntityBase<FactureCollectedPaidValue> {
        public static decimal GetPaidValueForFacture(int factureID) {
            decimal retValue = 0;
            List<FactureItem> lstFactureItems = FactureItem.GetByFacture(factureID);
            foreach (FactureItem fi in lstFactureItems) {
                List<FactureCollectedPaidValue> list = GetByFactureItemID(fi.ID);
                foreach (FactureCollectedPaidValue fcpv in list) {
                    retValue += fcpv.PaidValue;
                }
            }
            return retValue;
        }

        public static decimal GetPaidValueForFactureItem(int factureItemID) {
            decimal retValue = 0;
            List<FactureCollectedPaidValue> list = GetByFactureItemID(factureItemID);
            foreach (FactureCollectedPaidValue fcpv in list) {
                retValue += fcpv.PaidValue;
            }
            return retValue;
        }

        public static List<FactureCollectedPaidValue> GetByFactureItemID(int factureItemID) {
            List<FactureCollectedPaidValue> tmpList = new List<FactureCollectedPaidValue>();
            List<FactureCollectedPaidValue> retList = new List<FactureCollectedPaidValue>();
            tmpList = Table.Where(c => c.FactureItemID == factureItemID).OrderBy(c=>c.PaidDate).ToList();
            foreach (FactureCollectedPaidValue fc in tmpList) {
                retList.Add(Table.GetOriginalEntityState(fc));
            }
            return retList;
        }

        public static List<FactureCollectedPaidValue> GetByFactureID(int factureID) {
            return Table.Where(c => c.FactureItem.FactureID == factureID).OrderBy(c => c.PaidDate).ToList();
        }

        public static List<FactureCollectedPaidValue> GetGroupedByFactureID(int factureID) {
            List<FactureCollectedPaidValue> listAll = GetByFactureID(factureID);
            List<FactureCollectedPaidValue> retList = new List<FactureCollectedPaidValue>();
            foreach (FactureCollectedPaidValue fcpv in listAll) {
                if (retList.Select(c => c.FactureItemID).ToList().Contains(fcpv.FactureItemID)) {
                    if (fcpv.PaidDate > retList.Where(c => c.FactureItemID == fcpv.FactureItemID).SingleOrDefault().PaidDate) {
                        retList.Where(c => c.FactureItemID == fcpv.FactureItemID).SingleOrDefault().PaidDate = fcpv.PaidDate;
                    }
                    retList.Where(c => c.FactureItemID == fcpv.FactureItemID).SingleOrDefault().PaidValue += fcpv.PaidValue;
                } else {
                    retList.Add(fcpv);
                }
            }
            return retList;
        }
    }
}
