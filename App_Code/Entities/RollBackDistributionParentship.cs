using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RollBackDistributionParentship
/// </summary>
namespace Broker.DataAccess {
    public partial class RollBackDistributionParentship : EntityBase<RollBackDistributionParentship> {

        public static void InsertParentShip(int parentID, int childID) {
            RollBackDistributionParentship r = new RollBackDistributionParentship();
            r.MainRollBackID = parentID;
            r.BranchRollBackID = childID;
            r.Insert();
        }
    }
}
