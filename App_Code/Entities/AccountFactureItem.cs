using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AccountFactureItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class AccountFactureItem : EntityBase<AccountFactureItem> {

        public static List<AccountFactureItem> GetByAccountFacture(int factureID) {
            return Table.Where(c => c.FactureID == factureID).OrderBy(c => c.Number).ToList();
        }

    }
}
