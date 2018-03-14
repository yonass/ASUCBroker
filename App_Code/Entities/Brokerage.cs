using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Brokerage
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Brokerage : EntityBase<Brokerage> {


        public static List<Brokerage> GetByInsuranceSubType(int InsuranceSubTypeID) {
            List<Brokerage> bList = GetActiveBrokerages();
            return bList.Where(b => b.InsuranceSubTypeID == InsuranceSubTypeID).ToList();
        }

        public static List<Brokerage> GetByDeals(List<Deal> dList) {
            List<Brokerage> bList = new List<Brokerage>();
            foreach (Deal d in dList) {
                List<Brokerage> brokerageList = Table.Where(b => b.DealID == d.ID).ToList();
                foreach (Brokerage b in brokerageList) {
                    bList.Add(b);
                }
            }
            return bList;
        }

        public static List<Brokerage> GetActiveByInsuranceSubType(int InsuranceSubTypeID) {
            List<Brokerage> bList = GetActiveBrokerages();
            return bList.Where(b => b.InsuranceSubTypeID == InsuranceSubTypeID).ToList();
        }

        public static List<Brokerage> GetActiveBrokerages() {
            return Table.Where(b => b.IsActive == true).ToList();
        }

        public static Brokerage GetByInsuranceSubTypeAndDeal(int insuranceSubtypeID, int dealID) {
            List<Brokerage> lBrokerage = GetActiveBrokerages();
            List<Brokerage> returnList = lBrokerage.Where(b => b.InsuranceSubTypeID == insuranceSubtypeID && b.DealID == dealID).ToList();
            if (returnList.Count == 0) {
                return null;
            } else {
                return returnList.First();
            }

            //return lBrokerage.Where(b => b.InsuranceSubTypeID == insuranceSubtypeID && b.DealID == dealID).SingleOrDefault();
        }
        public static List<Brokerage> GetActiveBrokeragesForDealAndInsuranceSubType(int dealID, int insuranceSubTypeID) {
            return Table.Where(b => b.DealID == dealID && b.InsuranceSubTypeID == insuranceSubTypeID).ToList();

        }

        public static List<Brokerage> GetByDeal(int dealID) {
            return Table.Where(b => b.DealID == dealID).ToList();
        }

        public static bool ExistBrokerage(int dealID, int insuranceSubTypeID) {
            return (Table.Where(b => b.DealID == dealID && b.InsuranceSubTypeID == insuranceSubTypeID && b.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Brokerage.ExistBrokerage(this.DealID, this.InsuranceSubTypeID)) {
                ValidationErrors.Add("BROKERAGE_EXISTS", "Веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        //public static decimal GetBrokerageForPeriodAndCompany(int insuranceCompanyID, DateTime startDate, DateTime endDate) {
        //    BrokerageSpecification bSpec = BrokerageSpecification.GetForBrokerage(10);
        //}

        public static decimal GetBrokerageForBrokerage(int brokerageID) {
            List<BrokerageBrokerageType> bbtList = BrokerageBrokerageType.GetForBrokerage(22);
            List<decimal> additionalValues = new List<decimal>();
            foreach (BrokerageBrokerageType bbt in bbtList) {
                
                List<BrokerageParameter> brokerageParametersList = BrokerageParameter.GetForBrokerageType(bbt.BrokerageTypeID);
                List<decimal> values = new List<decimal>();
                for (int i = 0; i < brokerageParametersList.Count; i++) {
                    values.Add(BrokerageParameter.GetValueForParameter(brokerageParametersList[i]));
                }
                decimal value = BrokerageType.GetValue(bbt.BrokerageTypeID,values);
                BrokerageSpecification bs = BrokerageSpecification.GetByValueAndType(bbt.ID, value);
                additionalValues.Add(bs.AdditionalValue);

            }
            decimal returnValue =0;
            foreach(decimal d in additionalValues){
                returnValue+=d;
            }
            return returnValue;
        }


    }
        }


 

