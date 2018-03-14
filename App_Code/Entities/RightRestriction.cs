using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RightRestriction
/// </summary>
namespace Broker.DataAccess{
public partial class RightRestriction:EntityBase<RightRestriction>{

    public static List<RightRestriction> GetForRollBack(DateTime startDate, DateTime endDate, int insuranceCompanyID) {
        return Table.Where(c => c.ApplicationDate >= startDate && c.ApplicationDate <= endDate && c.InsuranceCompanyID == insuranceCompanyID && c.IsRollBacked == false).ToList(); 
    }



    }
}
