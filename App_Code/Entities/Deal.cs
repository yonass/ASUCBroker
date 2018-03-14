using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Deal
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Deal:EntityBase<Deal> {

        public static List<Deal> GetActiveDeals() {
            return Table.Where(d => d.IsActive == true).ToList();
        }


        public static List<Deal> GetBySubType(int subTypeID) {
            List<Brokerage> brokerageList = Brokerage.GetByInsuranceSubType(subTypeID);
            List<Deal> dList = new List<Deal>();
            foreach (Brokerage b in brokerageList) {
                if (b.Deal.IsActive) {
                    if (!dList.Contains(b.Deal)) {
                        dList.Add(b.Deal);
                    }
                }
            }
            if (dList.Count > 0) {
                return dList;
            } else
                return null;
        }


        public static List<Deal> GetByInsuranceCompany(int insuranceCompanyID) {
            List<Deal> lDeal = GetActiveDeals();
            return lDeal.Where(d => d.InsuranceCompanyID == insuranceCompanyID).ToList();
        }


        public static List<Deal> GetByCompanyID(int companyID) {
            return GetActiveDeals().Where(d => d.InsuranceCompanyID == companyID).ToList();
        }


        public static bool ExistCodeInDeals(string code) {
            return (Table.Where(d => d.Code == code && d.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(d => d.Code == code && d.ID != codeID && d.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Deal.ExistCodeInDeals(this.Code)) {
                ValidationErrors.Add("DEAL_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Deal.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("DEAL_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }
        public static List<Deal> GetActiveDealsForCompanyAndInsuranceSubType(int InsuranceCompanyID, int InsuranceSubTypeID) {
            List<Deal> dList = new List<Deal>();
            List<Brokerage> bList = Brokerage.GetByInsuranceSubType(InsuranceSubTypeID);
            foreach (Brokerage b in bList) {
                if (b.Deal.IsActive && b.Deal.InsuranceCompanyID == InsuranceCompanyID && !dList.Contains(b.Deal)) {
                    dList.Add(b.Deal);
                }
            }
            return dList;
        }

    }
}
