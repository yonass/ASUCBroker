using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

/// <summary>
/// Summary description for TemporaryDistribution
/// </summary>
namespace Broker.DataAccess{
    public partial class TemporaryDistribution : EntityBase<TemporaryDistribution> {

        public static int Duration = 15;

        public static bool Exist(int DistributionID, int UserID) {

            List<TemporaryDistribution> tdList = Table.Where(t => t.DistributionID == DistributionID).ToList();

            if (tdList.Count==0) {
                return false;
            }
            foreach (TemporaryDistribution td in tdList) {
                if (td.UserID == UserID || td.Time.AddMinutes(Duration) < DateTime.Now) {
                    //                    return false;
                } else {

                    //                  return true;
                }
            }
            return true;
        }

        public static void Insert(int distributionID, int userID) {
            TemporaryDistribution td = new TemporaryDistribution();
            td.DistributionID = distributionID;
            td.UserID = userID;
            td.Time = DateTime.Now;
            td.Insert();
        }

        public static TemporaryDistribution GetByDistribution(int distributionID) {
            return Table.Where(d => d.DistributionID == distributionID).SingleOrDefault();

        }
    }
}
