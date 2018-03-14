using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FactureSpecification
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class FactureSpecification : EntityBase<FactureSpecification> {

        public static List<FactureSpecification> GetByFacture(int factureID) {
            return Table.Where(fs => fs.FactureID == factureID).ToList();
        }

    }
}
