using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AccountFactureItemPolicyItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class AccountFactureItemPolicyItem : EntityBase<AccountFactureItemPolicyItem> {
        public static List<AccountFactureItemPolicyItem> GetByAccountFactureItem(int accountFactureItemID) {
            return Table.Where(c => c.FactureItemID == accountFactureItemID).ToList();
        }

        public static List<AccountFactureItemPolicyItem> GetByAccountFacture(int accountFactureID) {
            return Table.Where(c => c.AccountFactureItem.FactureID == accountFactureID).ToList();
        }

        public static void InsertForFacture(int factureItemID, List<int> listPolicyItems) {
            foreach (int piID in listPolicyItems) {
                AccountFactureItemPolicyItem afipi = new AccountFactureItemPolicyItem();
                afipi.FactureItemID = factureItemID;
                afipi.PolicyItemID = piID;
                afipi.Insert();
            }
        }
    }
}