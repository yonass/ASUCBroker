using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LifeDeal
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class LifeDeal : EntityBase<LifeDeal> {
        public static List<LifeDeal> GetActiveDeals() {
            return Table.Where(d => d.IsActive == true).ToList();
        }

        public static List<LifeDeal> GetActiveDealsForCompanyAndInsuranceSubType(int InsuranceCompanyID, int InsuranceSubTypeID) {
            List<LifeDeal> dList = new List<LifeDeal>();
            dList = Table.Where(c => c.IsActive == true && c.InsuranceCompanyID == InsuranceCompanyID && c.InsuranceSubTypeID == InsuranceSubTypeID).ToList();
            return dList;
        }

        public static List<LifeDeal> GetByInsuranceCompany(int companyID) {
            return Table.Where(c => c.IsActive == true && c.InsuranceCompanyID == companyID).ToList();
        }

        public static List<LifeDeal> GetBySubType(int subTypeID) {
            List<LifeDeal> dList = new List<LifeDeal>();
            return Table.Where(c => c.InsuranceSubTypeID == subTypeID).ToList();
        }

    }
}