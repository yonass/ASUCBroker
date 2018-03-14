using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers.ReportControllers;

/// <summary>
/// Summary description for Rate
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Rate : EntityBase<Rate> {

        public static void UpdateExtend(Rate oldEntity, Rate newEntity) {
            decimal polValue = PolicyItem.Get(newEntity.PolicyItemID).PremiumValue;
            DataClassesDataContext dc = new DataClassesDataContext();
            string value = newEntity.Value.ToString();
            value = value.Replace(',', '.');
            string query = "Update Rates SET Value = " + value + " , Date = '" + newEntity.Date.Year + -+newEntity.Date.Month + -+newEntity.Date.Day + "' WHERE ID = " + newEntity.ID;
            dc.ExecuteCommand(query);
            //Cascade update rest rates
            string selQuery = @"Select r.* FROM Rates r WHERE r.PolicyItemID = " + newEntity.PolicyItemID + " ORDER BY r.Number";
            List<Rate> lstAllRates = dc.ExecuteQuery<Rate>(selQuery).ToList();
            if (lstAllRates != null) {
                if (lstAllRates.Count > 0) {
                    List<Rate> lstPreviousRates = lstAllRates.Where(c=>c.Number<=oldEntity.Number).ToList();
                    decimal totValue = 0;
                    foreach (Rate rate in lstPreviousRates) {
                        totValue += rate.Value;
                    }
                    List<Rate> lstRateForUpdates = lstAllRates.Where(c => c.Number > oldEntity.Number).ToList();
                    if (lstRateForUpdates.Count > 0) {
                        decimal rightValue = (polValue - totValue) / lstRateForUpdates.Count;
                        foreach (Rate rateU in lstRateForUpdates) {
                            string valueU = rightValue.ToString();
                            valueU = valueU.Replace(',', '.');
                            string queryU = "Update Rates SET Value = " + valueU + " WHERE ID = " + rateU.ID;
                            dc.ExecuteCommand(queryU);
                        }
                    }
                }
            }
        }

        public static List<Rate> GetByPolicyNumber(string policyNumber) {
            return Table.Where(r => r.PolicyItem.PolicyNumber == policyNumber).OrderBy(r => r.Number).ToList();
        }

        public static List<Rate> GetByNumberAndInsuranceCompany(string policyNumber, int insuranceCompanyID) {
            return Table.Where(r => r.PolicyItem.PolicyNumber == policyNumber && r.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID).OrderBy(r => r.Number).ToList();
        }

        public static List<Rate> GetByPolicyNumberAndInsuranceSubTypeIDAndInsuranceCompanyID(string policyNumber, int insuranceSubTypeID, int insuranceCompanyID) {
            return Table.Where(r => r.PolicyItem.PolicyNumber == policyNumber && r.PolicyItem.InsuranceSubTypeID == insuranceSubTypeID && r.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID).OrderBy(r => r.Number).ToList();
        }

        public static List<Rate> GetInPeriod(DateTime fromDate, DateTime toDate) {
            return Table.Where(c => c.Date.Date >= fromDate.Date && c.Date.Date <= toDate.Date && c.PolicyItem.Policy.Discard == false).ToList();
        }

        public static List<Rate> GetInPeriod(DateTime fromDate, DateTime toDate, List<int> lstBranhces,
            List<int> lstInsuranceCompanies, List<int> lstInsuranceSubTypes, List<int> lstUsers, List<int> lstMarketingAgents) {
            DateTime dt1 = fromDate;
            DateTime dt2 = toDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"SELECT r.* from rates r, policyitems pi, policies p
                        where r.policyitemid=pi.id and pi.policyid=p.id and p.branchid in (" + ReportFinPolicies.GetFromIDs(lstBranhces) + ") " +
                            @" and p.userid in (" + ReportFinPolicies.GetFromIDs(lstUsers) + ") " +
                            @" and p.insurancecompanyid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceCompanies) + ") " +
                            @" and pi.insurancesubtypeid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceSubTypes) + ") " +
                            @" and r.date>='" + y1 + -+m1 + -+d1 + "'" +
                            @" and r.date<='" + y2 + -+m2 + -+d2 + "'" +
                            @" and p.discard = 0 ";
            List<Rate> lst = dc.ExecuteQuery<Rate>(query).ToList();
            return lst;
        }

        public static List<Rate> GetByPolicyItemID(int policyItemID) {
            return Table.Where(r => r.PolicyItemID == policyItemID).OrderBy(r => r.Number).ToList();
        }

        public static List<Rate> GetByPolicyItemIDExtend(int policyItemID) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = "SELECT r.* from Rates r WHERE r.PolicyItemID = " + policyItemID + " ORDER BY r.ID";
            List<Rate> lstRates = dc.ExecuteQuery<Rate>(query).ToList();
            if (lstRates != null) {
                return lstRates;
            } else {
                return new List<Rate>();
            }
        }

        public static Rate GetCurrentRateForPayment(int policyItemID) {
            List<Rate> listRate = GetByPolicyItemID(policyItemID);
            foreach (Rate r in listRate) {
                decimal paidValuePerRate = Payment.GetPaidValueForRate(r.ID);
                if (paidValuePerRate < r.Value) {
                    return r;
                } else {
                    continue;
                }
            }
            return null;
        }

        public static Rate GetCurrentRateForPaymentExtend(int policyItemID) {
            List<Rate> listRate = GetByPolicyItemIDExtend(policyItemID);
            if (listRate != null) {
                foreach (Rate r in listRate) {
                    decimal paidValuePerRate = Payment.GetPaidValueForRateExtend(r.ID);
                    if (paidValuePerRate < r.Value) {
                        return r;
                    } else {
                        continue;
                    }
                }
            }
            return null;
        }

        public static decimal PaidRatesValue(int policyItemID) {
            List<Rate> listRates = GetByPolicyItemID(policyItemID);
            decimal paidValue = 0;
            foreach (Rate r in listRates) {
                paidValue += r.PaidValue;
            }
            return paidValue;
        }

        public void ValidateRate() {
            List<Payment> lstPayment = Payment.GetByRateID(this.ID);
            if (lstPayment.Count > 0) {
                ValidationErrors.Add("RateValue", "Не може да се менува вредност на рата за која се внесени плаќања!");
                return;
            }
            List<Rate> lstRate = Rate.GetByPolicyItemIDExtend(this.PolicyItemID);
            if (lstRate.Count == this.Number) {
                decimal prevValue = 0;
                List<Rate>  lstPrevRates = lstRate.Where(c=>c.Number<this.Number).ToList();
                foreach (Rate rateR in lstPrevRates) {
                    prevValue += rateR.Value;
                }
                PolicyItem pi = PolicyItem.Get(this.PolicyItemID);
                if (this.Value != (pi.PremiumValue - prevValue)) {
                    ValidationErrors.Add("RateValue", "Не може да се внесе износ на последна рата со кој вкупниот износ од рати ќе се разликува од премијата за наплата на полисата!");
                    return;
                }
            }

        }

    }
}
