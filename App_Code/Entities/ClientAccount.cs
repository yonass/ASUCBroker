using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClientAccount
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class ClientAccount : EntityBase<ClientAccount> {
        public static ClientAccount GetByClientAndBank(int ClientID, int BankID) {
            return Table.Where(ca => ca.ClientID == ClientID && ca.BankID == BankID).SingleOrDefault();
        }

        public static ClientAccount GetByClient(int clientID) {
            return Table.Where(ca => ca.ClientID == clientID).SingleOrDefault();
        }

        public static ClientAccount GetByAccountNumber(string accountNumber) {
            try {
                return Table.Where(ca => ca.AccountNumber == accountNumber).SingleOrDefault();
            } catch {
                return null;
            }
        }


        public static List<ClientAccount> GetByClientID(int clientID) {
            return Table.Where(ca => ca.ClientID == clientID).ToList();
        }

        public void ValidateAccount() {

            ClientAccount ca = ClientAccount.GetByClientAndBank(this.ClientID, this.BankID);
            if (ca != null && ca.IsActive) {
                ValidationErrors.Add("ClientAccountValidator", "Веќе постои сметката");
            }

        }
    }
}
