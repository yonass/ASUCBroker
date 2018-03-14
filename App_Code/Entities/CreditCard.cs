using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CreditCard
/// </summary>
namespace Broker.DataAccess {
    public partial class CreditCard : EntityBase<CreditCard> {
        public static List<CreditCard> GetByBank(int BankID) {
            IEnumerable<BankCreditCard> bCCIenum = BankCreditCard.GetByBank(BankID);
            List<CreditCard> ccList = new List<CreditCard>();
            foreach (BankCreditCard bCC in bCCIenum) {
                if (!ccList.Contains(bCC.CreditCard)) {
                    ccList.Add(bCC.CreditCard);
                }
            }
            return ccList;
        }

    }
}
