using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RightRestrictionDistribution
/// </summary>
namespace Broker.DataAccess {
    public partial class RightRestrictionDistribution : EntityBase<RightRestrictionDistribution> {

        public static RightRestrictionDistribution ExistForCompany(int insuranceCompanyID, string number) {
            RightRestrictionDistribution rrd = Table.Where(c => c.InsuranceCompanyID == insuranceCompanyID && c.Number == number).SingleOrDefault();
            return rrd;

        }

        public static void UpdateUsed(int insuranceCompanyID, string number) {
            RightRestrictionDistribution rrd = Table.Where(c => c.InsuranceCompanyID == insuranceCompanyID && c.Number == number).SingleOrDefault();
            rrd.IsUsed = true;
            rrd.Update2();
            //Context.SubmitChanges();
        }
        public static RightRestrictionDistribution GetForCompanyAndType(int insuranceCompanyID, string number, int documentTypeID) {
            RightRestrictionDistribution rrd = Table.Where(c => c.InsuranceCompanyID == insuranceCompanyID && c.Number == number && c.DocumentTypeID == documentTypeID).SingleOrDefault();
            return rrd;

        }

        public void ValidateNumber() {
            if (GetForCompanyAndType(this.InsuranceCompanyID, this.Number, this.DocumentTypeID) != null) {
                this.ValidationErrors.Add("Number", "Бројот е задолжен");
            }
        }

    }
}
