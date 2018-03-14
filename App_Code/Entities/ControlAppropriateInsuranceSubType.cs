using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ControlAppropriateInsuranceSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class ControlAppropriateInsuranceSubType:EntityBase<ControlAppropriateInsuranceSubType> {

        public static List<Control> GetByInsuranceSubType(int insuranceSubTypeID) {
            return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID).Select(c => c.Control).ToList();
        }

        public static ControlAppropriateInsuranceSubType GetByControl(Control c) {
            return Table.Where(t => t.ControlID == c.ID && t.IsActive == true).SingleOrDefault();
        }


        public static List<Control> GetAllWithDiferentInsuranceSubType(int insuranceSubTypeID) {
            return Table.Where(c => c.InsuranceSubTypeID != insuranceSubTypeID).Select(c => c.Control).ToList();
        }

    }
}
