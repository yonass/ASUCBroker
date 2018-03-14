using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RestrictionsRestrictionRollBack
/// </summary>
namespace Broker.DataAccess{
public partial class RestrictionsRestrictionRollBack: EntityBase<RestrictionsRestrictionRollBack>{

    public static void GetForRollBack(RestrictionsRollBackDistribution rbd, List<RightRestriction> restrictionsList) {
        List<RightRestriction> rbdiList = new List<RightRestriction>();
        foreach (RightRestriction rr in restrictionsList) {
            rr.IsRollBacked = true;
        }
        Table.Context.SubmitChanges();
    }
    

    }
}
