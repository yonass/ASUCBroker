using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RestrictionsRollBackDistribution
/// </summary>
namespace Broker.DataAccess {
    public partial class RestrictionsRollBackDistribution : EntityBase<RestrictionsRollBackDistribution> {

        public static int InsertRollBackDistribution(DateTime startDate, DateTime endDate, int insuranceCompanyID) {
            RestrictionsRollBackDistribution rbd = GetRollBack(startDate, endDate, insuranceCompanyID);
            List<RightRestriction> restrictionsList = RightRestriction.GetForRollBack(rbd.StartDate, rbd.EndDate, rbd.InsuranceCompanyID);
            if (restrictionsList.Count > 0) {
                rbd.Insert();
                RestrictionsRestrictionRollBack.GetForRollBack(rbd, restrictionsList);
                return restrictionsList.Count;
            }
            return 0;
        }

        public static RestrictionsRollBackDistribution GetRollBack(DateTime startDate, DateTime endDate, int insuranceCompanyID) {
            RestrictionsRollBackDistribution rbd = new RestrictionsRollBackDistribution();
            rbd.StartDate = startDate;
            rbd.EndDate = endDate;
            rbd.Date = DateTime.Today;
            rbd.InsuranceCompanyID = insuranceCompanyID;
            rbd.Number = GetNextNumber();
            return rbd;
        }

        public static int GetNextNumber() {
            IEnumerable<RestrictionsRollBackDistribution> rbdEnum = Table.OrderByDescending(c => c.Number);
            if (rbdEnum.FirstOrDefault() == null) {
                return 1;
            } else
                return rbdEnum.First().Number + 1;
        }

    }
}
