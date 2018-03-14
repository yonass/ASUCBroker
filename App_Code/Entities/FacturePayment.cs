using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FacturePayment
/// </summary>
namespace Broker.DataAccess {
    public partial class FacturePayment : EntityBase<FacturePayment> {

        public static IEnumerable<FacturePayment> GetByFacture(int factureID) {

            return Table.Where(c => c.FactureID == factureID);


        }


    }
}
