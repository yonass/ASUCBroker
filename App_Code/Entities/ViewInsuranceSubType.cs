using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ViewInsuranceSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class ViewInsuranceSubType : EntityBase<ViewInsuranceSubType> {
        public static IQueryable<ViewInsuranceSubType> SelectByInsuranceType(int insuranceTypeID) {
            return Table.Where(v => v.InsuranceTypeID == insuranceTypeID);
        }
    }
}
