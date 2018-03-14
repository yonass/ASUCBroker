using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AccreditationInsuranceType
/// </summary>
/// 
namespace Broker.DataAccess
{
    public partial class AccreditationInsuranceType : EntityBase<AccreditationInsuranceType>
    {
        public static List<AccreditationInsuranceType> GetByAccreditation(int accreditationID)
        {
            return Table.Where(ait => ait.AccreditationID == accreditationID).ToList();
        }
    }
}
