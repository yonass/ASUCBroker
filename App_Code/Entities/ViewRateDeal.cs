using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ViewRateDeal
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class ViewRateDeal : EntityBase<ViewRateDeal> {

        public static List<ViewRateDeal> GetRateDealsForInsuranceSubType(int insuranceSubTypeID, decimal premiumValue, DateTime applicationDate) {
            List<ViewRateDeal> retList = new List<ViewRateDeal>();
            ViewRateDeal dummyRateDeal = new ViewRateDeal();
            dummyRateDeal.ID = 0;
            dummyRateDeal.Name = "";
            retList.Add(dummyRateDeal);
            List<RateDealInsuranceSubType> lst = RateDealInsuranceSubType.Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID &&
                c.FromValue <= premiumValue && c.ToValue >= premiumValue && c.RateDeal.IsActive == true && c.RateDeal.FromDate.Date <= applicationDate.Date && c.RateDeal.ToDate.Date >= applicationDate.Date).ToList();
            foreach(RateDealInsuranceSubType rdist in lst){
                ViewRateDeal vrd = ViewRateDeal.Table.Where(c => c.ID == rdist.RateDealID).SingleOrDefault();
                if(!retList.Contains(vrd)){
                    retList.Add(vrd);
                }
            }
            return retList;

        }


    }
}
