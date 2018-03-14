using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokeragesForMarketingAgent
/// </summary>
namespace Broker.DataAccess {
    public partial class BrokeragesForMarketingAgent : EntityBase<BrokeragesForMarketingAgent> {

        public static List<BrokeragesForMarketingAgent> GetByUserID(int userID) {
            return Table.Where(c => c.UserID == userID).ToList();
        }
        public static List<BrokeragesForMarketingAgent> GetByUserID() {
            return null;
        }
        public static void UpdateCurrentPercentages(BrokeragesForMarketingAgent oldEntity, BrokeragesForMarketingAgent newEntity) {
            BrokeragesForMarketingAgent bfma = BrokeragesForMarketingAgent.Get(newEntity.ID);
            bfma.PercentageForLaws = newEntity.PercentageForLaws;
            bfma.PercentageForPrivates = newEntity.PercentageForPrivates;
            Table.Context.SubmitChanges();
        }

        public static BrokeragesForMarketingAgent GetByUserAndInsuranceCompanyForSubType(int userID, int insuranceCompanyID, int inusranceSubTypeID) {
            return Table.Where(c => c.IsActive == true && c.UserID == userID && c.InsuranceCompanyID == insuranceCompanyID && c.InsuranceSubTypeID == inusranceSubTypeID).SingleOrDefault();
        }
    }
}
