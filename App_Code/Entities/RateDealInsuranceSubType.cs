using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RateDealInsuranceSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class RateDealInsuranceSubType:EntityBase<RateDealInsuranceSubType> {
        public static List<RateDealInsuranceSubType> GetByRateDeal(int rateDealID) {
            return Table.Where(c => c.RateDealID == rateDealID).ToList();
        }

        public static RateDealInsuranceSubType GetByRateDealInsuranceSubTypePremiumValue(int rateDealID, int insuranceSubTypeID, decimal premiumValue) {
            return Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.RateDealID == rateDealID &&
                c.FromValue <= premiumValue && c.ToValue >= premiumValue && c.RateDeal.IsActive == true).SingleOrDefault();

        }
    }
}