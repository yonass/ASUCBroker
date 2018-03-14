using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RateDeal
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class RateDeal : EntityBase<RateDeal> {
        public static List<RateDeal> GetAllActive() {
            return Table.Where(c => c.IsActive == true).OrderBy(c => c.Client.Name).ToList();
        }

        public static bool ExistDealNumberInRateDeals(string dealNumber, int clientID) {
            return (Table.Where(d => d.DealNumber == dealNumber && d.ClientID == clientID && d.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherDealNumber(int dealNumberID, string dealNumber, int clientID) {
            return (Table.Where(d => d.DealNumber == dealNumber && d.ClientID == clientID && d.ID != dealNumberID && d.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (RateDeal.ExistDealNumberInRateDeals(this.DealNumber, this.ClientID)) {
                ValidationErrors.Add("DEAL_NUMBER_INSERT_EXISTS", this.DealNumber + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (RateDeal.ExistOtherDealNumber(this.ID, this.DealNumber, this.ClientID)) {
                ValidationErrors.Add("DEAL_NUMBER_UPDATE_EXISTS", this.DealNumber + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        

    }
}

