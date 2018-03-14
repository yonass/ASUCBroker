using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers.ReportControllers;

/// <summary>
/// Summary description for Payment
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Payment : EntityBase<Payment> {

        public static List<Payment> GetByDateAndInsuranceCompany(DateTime fromDate, DateTime toDate, int insuranceCompanyID) {
            List<Payment> listPayments = new List<Payment>();
            listPayments = Table.Where(p => p.IsFactured == false && p.Date.Date >= fromDate.Date && p.Date.Date <= toDate.Date && p.Rate.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID && p.Rate.PolicyItem.Policy.Discard == false).ToList();
            return listPayments;
        }

        public static void DeleteExtend(Payment entityToDelete) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string firstQuery = @"DELETE FROM PaymentsPerInsSubTypes WHERE PaymentID = " + entityToDelete.ID;
            dc.ExecuteCommand(firstQuery);
            string query = @"DELETE FROM Payments WHERE ID = " + entityToDelete.ID;
            dc.ExecuteCommand(query);
            Rate r = Rate.Get(entityToDelete.RateID);
            decimal oldRatePaidValue = r.PaidValue;
            decimal newRatePaidValue = oldRatePaidValue - entityToDelete.Value;
            string valuePaid = newRatePaidValue.ToString();
            valuePaid = valuePaid.Replace(',', '.');
            string queryRate = "Update Rates Set PaidValue = " + valuePaid + " Where ID = " + r.ID;
            dc.ExecuteCommand(queryRate);
            Facture.UpdatePaidStatusForFactureExtend(r.PolicyItemID);
        }

        public static void UpdateExtend(Payment oldEntity, Payment newEntity) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string value = newEntity.Value.ToString();
            value = value.Replace(',', '.');
            decimal k = 1;
            if (oldEntity.Value != 0) {
                k = newEntity.Value / oldEntity.Value;
            }
            string Kvalue = k.ToString();
            Kvalue = Kvalue.Replace(',', '.');
            string firstQuery = @"UPDATE PaymentsPerInsSubTypes SET PaidValue = " + Kvalue + " * paidvalue, brokeragevalue = " + Kvalue + " * brokeragevalue WHERE PaymentID = " + newEntity.ID;
            dc.ExecuteCommand(firstQuery);
            string query = @"Update Payments SET Value = " + value + " , Date = '" + newEntity.Date.Year + -+newEntity.Date.Month + -+newEntity.Date.Day + "' , PaymentTypeID = " + newEntity.PaymentTypeID;
            if (newEntity.BankslipNumber != string.Empty) {
                if (newEntity.BankslipNumber != null) {
                    query += @", BankslipNumber = '" + newEntity.BankslipNumber + "'";
                }
                if (newEntity.BankslipBankID != null) {
                    query += @", BankslipBankID = " + newEntity.BankslipBankID;
                } else {
                    query += @", BankslipBankID = null";
                }
            } else {
                query += @", BankslipNumber = '" + newEntity.BankslipNumber + "'";
                if (newEntity.BankslipBankID != null) {
                    query += @", BankslipBankID = " + newEntity.BankslipBankID;
                } else {
                    query += @", BankslipBankID = null";
                }
            }
            if (newEntity.BankCreditCardID != null) {
                query += @", BankCreditCardID = " + newEntity.BankCreditCardID;
            } else {
                query += @", BankCreditCardID = null";
            }
            query += @" WHERE ID = " + newEntity.ID;
            Rate r = Rate.Get(newEntity.RateID);
            decimal oldRatePaidValue = r.PaidValue;
            decimal newRatePaidValue = oldRatePaidValue + (newEntity.Value - oldEntity.Value);
            string valuePaid = newRatePaidValue.ToString();
            valuePaid = valuePaid.Replace(',', '.');
            string queryRate = "Update Rates Set PaidValue = " + valuePaid + " Where ID = " + r.ID;
            dc.ExecuteCommand(query);
            dc.ExecuteCommand(queryRate);
            Facture.UpdatePaidStatusForFactureExtend(r.PolicyItemID);
        }

        public static List<Payment> GetForCashReport(DateTime cashReportDate, int branchID) {
            List<Payment> lstPayments = Table.Where(c => c.Date.Date == cashReportDate.Date && c.Rate.PolicyItem.Policy.Discard == false
                && c.BranchID == branchID && (c.PaymentType.Code == PaymentType.CASH || c.PaymentType.Code == PaymentType.CREDITCARD)).ToList();
            return lstPayments;
        }

        public static List<Payment> GetInPeriod(DateTime fromDate, DateTime toDate) {
            return Table.Where(c => c.Date.Date >= fromDate.Date && c.Date.Date <= toDate.Date && c.Rate.PolicyItem.Policy.Discard == false).ToList();
        }

        public static List<Payment> GetInPeriod(DateTime fromDate, DateTime toDate, List<int> lstBranhces,
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
            string query = @"SELECT pay.* from payments pay, rates r, policyitems pi, policies p
                             where r.policyitemid=pi.id and pi.policyid=p.id and pay.rateid=r.id and p.branchid in (" + ReportFinPolicies.GetFromIDs(lstBranhces) + ") " +
                            @" and p.userid in (" + ReportFinPolicies.GetFromIDs(lstUsers) + ") " +
                            @" and p.insurancecompanyid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceCompanies) + ") " +
                            @" and pi.insurancesubtypeid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceSubTypes) + ") " +
                            @" and pay.date>='" + y1 + -+m1 + -+d1 + "'" +
                            @" and pay.date<='" + y2 + -+m2 + -+d2 + "'" +
                            @" and p.discard = 0 ";
            List<Payment> lst = dc.ExecuteQuery<Payment>(query).ToList();
            return lst;
        }

        public static List<Payment> GetForFactureByDateAndInsuranceCompany(DateTime toDate, int insuranceCompanyID) {
            List<Payment> listPayments = new List<Payment>();
            listPayments = Table.Where(p => p.IsFactured == false && p.Date.Date <= toDate.Date && p.Rate.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID && p.Rate.PolicyItem.Policy.Discard == false).ToList();
            return listPayments;
        }

        public static List<Payment> GetByRateID(int rateID) {
            return Table.Where(p => p.RateID == rateID).ToList();
        }

        public static decimal GetPaidValueForRate(int rateID) {
            List<Payment> listPayments = GetByRateID(rateID);
            decimal paidValue = 0;
            foreach (Payment p in listPayments) {
                paidValue += p.Value;
            }
            return paidValue;
        }

        public static decimal GetPaidValueForRateExtend(int rateID) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"SELECT isnull(SUM(Value),0) AS paidValue FROM Payments p
                            WHERE p.RateID = " + rateID;
            decimal paidValue = 0;
            paidValue = dc.ExecuteQuery<decimal>(query).SingleOrDefault();
            return paidValue;
        }

        public static decimal GetPaidValueForPolicyItem(int policyItemID) {
            List<Payment> listPayments = GetByPolicyItemID(policyItemID);
            decimal paidValue = 0;
            foreach (Payment p in listPayments) {
                paidValue += p.Value;
            }
            return paidValue;
        }


        public static decimal GetPaidValueForPolicyItemExtend(int policyItemID) {
            decimal retValue = 0;
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"select sum(payments.value) as paidvalue
                        from payments, rates, policyitems
                        where payments.rateid=rates.id
                        and rates.policyitemid=policyitems.id
                        and policyitems.id = " + policyItemID;
            decimal? dQuery = dc.ExecuteQuery<decimal?>(query).SingleOrDefault();
            if (dQuery != null) {
                retValue = (decimal)dQuery;
            }
            return retValue;
        }


        public static List<Payment> GetByPolicyItemID(int policyItemID) {
            List<Rate> listRates = Rate.GetByPolicyItemID(policyItemID);
            List<Payment> listPayments = new List<Payment>();
            foreach (Rate r in listRates) {
                listPayments.AddRange(GetByRateID(r.ID));
            }
            return listPayments;
        }

        public static List<Payment> GetByPolicyItemIDExtend(int policyItemID) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = "SELECT p.* FROM Payments p, Rates r WHERE p.RateID = r.ID AND r.PolicyItemID = " + policyItemID + " ORDER BY p.ID";
            List<Payment> lst = dc.ExecuteQuery<Payment>(query).ToList();
            if (lst != null) {
                return lst;
            } else {
                return new List<Payment>();
            }
        }

        public static List<Payment> GetByPolicyItemID() {
            return null;
        }

        public static void SetFactured(List<Payment> paymentList, Facture f) {
            foreach (Payment p in paymentList) {
                p.IsFactured = true;
                FacturePayment fp = new FacturePayment();
                fp.PaymentID = p.ID;
                fp.FactureID = f.ID;
                fp.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
                fp.Insert();

            }
        }


        public static decimal GetSumByPaymentTypeAndInsuranceCompanyInPeriod(DateTime fromDate, DateTime toDate, int insuranceCompanyID, PaymentType pt, out int count) {
            decimal sum = 0;
            count = 0;
            HashSet<int> hs = new HashSet<int>();
            List<Payment> lst = Table.Where(c => c.Date.Date >= fromDate.Date && c.Date.Date <= toDate.Date && c.Rate.PolicyItem.Policy.Discard == false
                && c.Rate.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID && c.PaymentTypeID == pt.ID).ToList();
            foreach (Payment p in lst) {
                if (!hs.Contains(p.Rate.PolicyItemID)) {
                    hs.Add(p.Rate.PolicyItemID);
                }
                sum += p.Value;
            }
            count = hs.Count;
            return sum;
        }

        public static decimal GetCountByPaymentTypeAndInsuranceCompanyInPeriod(DateTime fromDate, DateTime toDate, int insuranceCompanyID, PaymentType pt) {
            int count = 0;
            List<PolicyItem> lst = Table.Where(c => c.Date.Date >= fromDate.Date && c.Date.Date <= toDate.Date && c.Rate.PolicyItem.Policy.Discard == false
                && c.Rate.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID && c.PaymentTypeID == pt.ID).Select(c => c.Rate.PolicyItem).Distinct().ToList();
            return lst.Count;
        }


        public static void SetFactured(List<Payment> paymentList, Facture f, List<PolicyItem> policiesToRemove) {
            List<Payment> notForSetting = new List<Payment>();
            foreach (PolicyItem pi in policiesToRemove) {
                notForSetting.AddRange(Payment.GetByPolicyItemID(pi.ID));
            }
            List<Payment> validPaymentList = new List<Payment>();
            foreach (Payment p in paymentList) {
                if (!notForSetting.Contains(p)) {
                    validPaymentList.Add(p);
                }
            }
            foreach (Payment p in validPaymentList) {
                p.IsFactured = true;
                p.Update2();
                //Context.SubmitChanges();
                FacturePayment fp = new FacturePayment();
                fp.PaymentID = p.ID;
                fp.FactureID = f.ID;
                fp.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
                fp.Insert();

            }
        }

        public void ValidatePaymentOnDeleting() {
            if (this.IsCashReported) {
                ValidationErrors.Add("PaymentValue", "Не може да се избрише плаќање кое е раздолжено со касов извештај!");
                return;
            }
            if (this.IsFactured) {
                ValidationErrors.Add("PaymentValue", "Не може да се избрише плаќање за кое е фактурирана брокеража!");
                return;
            }
            Rate currentRate = Rate.Get(this.RateID);
            string query = "SELECT r.Number FROM Payments p, Rates r WHERE p.RateID = r.ID AND r.PolicyItemID = " + currentRate.PolicyItemID + " ORDER BY r.Number";
            DataClassesDataContext dc = new DataClassesDataContext();
            List<int> lst = dc.ExecuteQuery<int>(query).ToList();
            if (lst != null) {
                if (lst.Count > 0) {
                    if (lst.Last() != currentRate.Number) {
                        ValidationErrors.Add("PaymentValue", "Не може да се избрише плаќање кое не е последно за полисата!");
                        return;
                    }
                }
            }

        }

        public void ValidatePayment() {
            if (this.Value <= 0) {
                ValidationErrors.Add("PaymentValue", "Мора да се внесе вредност поголема од 0!");
                return;
            }
            if (this.IsCashReported) {
                ValidationErrors.Add("PaymentValue", "Не може да се менува плаќање кое е раздолжено со касов извештај!");
                return;
            }
            if (this.IsFactured) {
                ValidationErrors.Add("PaymentValue", "Не може да се менува плаќање за кое е фактурирана брокеража!");
                return;
            }
            Rate rate = Rate.Get(this.RateID);
            if (this.Value > rate.Value) {
                ValidationErrors.Add("PaymentValue", "Не може да се внесе поголем износ од вредноста на ратата!");
                return;
            }
            decimal oldValuePerPayment = 0;
            decimal paidValueForRate = 0;
            string queryOldValue = @"SELECT Value FROM Payments WHERE ID = " + this.ID;
            DataClassesDataContext dc = new DataClassesDataContext();
            oldValuePerPayment = dc.ExecuteQuery<decimal>(queryOldValue).SingleOrDefault();
            string queryGetPaidValueForRate = @"SELECT PaidValue FROM Rates WHERE ID = " + this.RateID;
            paidValueForRate = dc.ExecuteQuery<decimal>(queryGetPaidValueForRate).SingleOrDefault();
            decimal minValue = paidValueForRate - oldValuePerPayment;
            decimal maxValuePerPayment = rate.Value - minValue;
            if (this.Value > maxValuePerPayment) {
                ValidationErrors.Add("PaymentValue", "Не може да се внесе поголем износ од преостаната вредност за плаќање на ратата!");
                return;
            }
        }
    }

}