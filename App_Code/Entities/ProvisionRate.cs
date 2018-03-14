using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProvisionRate
/// </summary>
namespace Broker.DataAccess {
    public partial class ProvisionRate : EntityBase<ProvisionRate> {

        public static List<ProvisionRate> GetActiveForBank(int bankID) {
            return Table.Where(c => c.BankID == bankID && c.IsActive == true).ToList();
        }

    }
}
