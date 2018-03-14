using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LifePolicy
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class LifePolicy : EntityBase<LifePolicy> {
        public void ValidateSinglePolicyNumber(int companyID, int insuranceSubTypeID, string number, int branchID) {
            //Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(insuranceSubTypeID, number, companyID);
            Distribution d = Distribution.GetByNumberAndSubTypeAndCompanyIsAccepted(insuranceSubTypeID, number, companyID, branchID);
            if (d == null) {
                // del za proverka na delot za blanko polisi
                RightRestrictionDistribution rrd = RightRestrictionDistribution.ExistForCompany(companyID, number);
                if (rrd != null) {
                    if (!rrd.IsUsed && rrd.DocumentTypeID == DistributionDocumentType.GetByCode(DistributionDocumentType.BLANKO).ID) {
                    } else {
                        this.ValidationErrors.Add("PolicyNumberDistributionValidator", "Бројот на полиса " + number + " не е задолжен");

                    }
                } else {
                    this.ValidationErrors.Add("PolicyNumberDistributionValidator", "Бројот на полиса " + number + " не е задолжен");
                }
            } else {
                if (d.IsUsed) {
                    this.ValidationErrors.Add("PolicyNumberDistributionValidator", "Бројот на полиса " + number + " е искористен");
                }
            }
            // this.PerformCustomValidation();
        }


        public static List<LifePolicy> GetUndiscardedInPeriod(DateTime fromDate, DateTime toDate) {
            return Table.Where(c => c.Discard == false && c.ApplicationDate.Date >= fromDate.Date && c.ApplicationDate.Date <= toDate.Date).ToList();
        }

        public static List<LifePolicy> GetFromPreviousMonths(DateTime fromDate, int insuranceCompanyID) {
            return LifePolicyBrokerage.Table.Where(c => c.FromDate.Date < fromDate.Date && c.IsFactured == false && c.LifePolicy.Discard == false
                && c.LifePolicy.InsuranceCompanyID == insuranceCompanyID).Select(c => c.LifePolicy).ToList();
        }

        public static List<LifePolicy> GetForFacturingFromCurrentMonth(DateTime fromDate, DateTime toDate, int insuranceCompanyID) {
            return LifePolicyBrokerage.Table.Where(c => c.FromDate.Date >= fromDate.Date && c.FromDate <= toDate.Date && c.IsFactured == false && c.LifePolicy.Discard == false
             && c.LifePolicy.InsuranceCompanyID == insuranceCompanyID).Select(c => c.LifePolicy).ToList();
        }
    }
}
