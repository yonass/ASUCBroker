using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BankCreditCard
/// </summary>
namespace Broker.DataAccess {
    public partial class BankCreditCard : EntityBase<BankCreditCard> {

        public static IEnumerable<BankCreditCard> GetByBank(int bankID) {
            return Table.Where(c => c.BankID == bankID);
        }

        public static List<BankCreditCard> GetByBankID(int bankID) {
            return Table.Where(c => c.BankID == bankID).ToList();
        }

        public static BankCreditCard GetByBankAndCard(int bankID, int cardID) {
            return Table.Where(c => c.BankID == bankID && c.CreditCardID == cardID).SingleOrDefault();
        }

        public static List<BankCreditCardInfo> GetAll() {
            List<BankCreditCardInfo> retList = new List<BankCreditCardInfo>();
            List<Bank> lstBanks = Bank.GetBanksWithGreditCards();
            foreach (Bank b in lstBanks) {
                List<BankCreditCard> lstBankCC = GetByBankID(b.ID);
                foreach (BankCreditCard bcc in lstBankCC) {
                    BankCreditCardInfo bcci = new BankCreditCardInfo();
                    bcci.ID = bcc.ID;
                    bcci.Name = b.Name + " - " + bcc.CreditCard.Name;
                    retList.Add(bcci);
                }
            }
            return retList;
        }


    }


    public class BankCreditCardInfo{
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
