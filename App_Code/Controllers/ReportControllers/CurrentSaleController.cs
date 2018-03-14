using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for CurrentSaleController
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers
{
    public class CurrentSaleController
    {
        
        public static List<Policy> GetFacturePaidPoliciesInPeriodForUser(DateTime currentDate, int userID)
        {
            List<Policy> policiesList = Rate.Table.Where(r => r.PolicyItem.Policy.InsertDate.Date == currentDate.Date && r.PolicyItem.Policy.UserID == userID).Select(r => r.PolicyItem.Policy).ToList();
            return policiesList;
        }

        
        public static decimal GetSubExpectedFacturePaymentsPerDay(DateTime currentDate, int userID)
        {
            decimal sum = 0;
            List<Policy> listPolicies = GetFacturePaidPoliciesInPeriodForUser(currentDate, userID);
            foreach(Policy p in listPolicies){
                if(p.Discard==false){
                    sum += p.TotalSum;
                }
            }
            return sum;
        }

    }
}