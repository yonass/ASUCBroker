using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for FinanceCardController
/// </summary>
/// 
namespace Broker.Controllers.FinanceControllers {
    public class FinanceCardController {
        public static List<FinanceCardInfo> GetByClient(Broker.DataAccess.Client c)
        {
            List<FinanceCardInfo> retList = new List<FinanceCardInfo>();
            List<Policy> pList = Policy.GetByClient(c.ID);
            decimal saldoValue = 0;
            for (int i = 0; i < pList.Count; i++) {
                List<FinanceCardInfo> finList = GetByPolicy(pList[i], saldoValue);
                retList.AddRange(finList);
                if (finList.Count > 0) {
                    saldoValue = finList.Last().SaldoValue;
                }
            }
            return retList;
        }

        public static List<FinanceCardInfo> GetByClientOpenItems(Broker.DataAccess.Client c)
        {
            List<FinanceCardInfo> retList = new List<FinanceCardInfo>();
            List<Policy> pList = Policy.GetByClient(c.ID);
            decimal saldoValue = 0;
            for (int i = 0; i < pList.Count; i++) {
                List<FinanceCardInfo> finListTmp = GetByPolicy(pList[i], 0);
                if (finListTmp.Count > 0) {
                    if (finListTmp.Last().SaldoValue == (decimal)0) {
                        string s = "sss";
                    } else {
                        List<FinanceCardInfo> finList = GetByPolicy(pList[i], saldoValue);
                        if (finList.Count > 0) {
                            retList.AddRange(finList);
                            saldoValue = finList.Last().SaldoValue;
                        }
                    }
                }
            }
            return retList;
        }

        public static List<FinanceCardInfo> GetByPaymentInPeriod(DateTime fromDate, DateTime toDate) {
            decimal totSaldo = 0;
            List<FinanceCardInfo> retList = new List<FinanceCardInfo>();
            List<Payment> lstPayments = Payment.GetInPeriod(fromDate, toDate);
            foreach (Payment payment in lstPayments) {
                FinanceCardInfo fciPayment = new FinanceCardInfo();
                fciPayment.PolicyNumber = payment.Rate.PolicyItem.PolicyNumber;
                fciPayment.PolicyItemID = payment.Rate.PolicyItemID;
                fciPayment.DocumentDate = payment.Date;
                fciPayment.PaidDate = payment.Rate.Date;
                if (lstPayments.Count == 1) {
                    fciPayment.Description = "Уплата по полиса " + payment.Rate.PolicyItem.PolicyNumber;
                } else {
                    fciPayment.Description = "Уплата за " + payment.Rate.Number + "-ва рата по полиса " + payment.Rate.PolicyItem.PolicyNumber;
                }
                fciPayment.DebtValue = 0;
                fciPayment.DemandValue = payment.Value;
                fciPayment.SaldoValue = totSaldo + (fciPayment.DebtValue - fciPayment.DemandValue);
                totSaldo = fciPayment.SaldoValue;
                retList.Add(fciPayment);
            }
            return retList;
        }

        public static List<FinanceCardInfo> GetByPaymentInPeriod(DateTime fromDate, DateTime toDate, List<int> lstBranhces,
            List<int> lstInsuranceCompanies, List<int> lstInsuranceSubTypes, List<int> lstUsers, List<int> lstMarketingAgents) {
            decimal totSaldo = 0;
            List<FinanceCardInfo> retList = new List<FinanceCardInfo>();
            List<Payment> lstPayments = Payment.GetInPeriod(fromDate, toDate, lstBranhces, lstInsuranceCompanies, lstInsuranceSubTypes, lstUsers, lstMarketingAgents);
            foreach (Payment payment in lstPayments) {
                FinanceCardInfo fciPayment = new FinanceCardInfo();
                fciPayment.PolicyNumber = payment.Rate.PolicyItem.PolicyNumber;
                fciPayment.PolicyItemID = payment.Rate.PolicyItemID;
                fciPayment.DocumentDate = payment.Date;
                fciPayment.PaidDate = payment.Rate.Date;
                if (lstPayments.Count == 1) {
                    fciPayment.Description = "Уплата по полиса " + payment.Rate.PolicyItem.PolicyNumber;
                } else {
                    fciPayment.Description = "Уплата за " + payment.Rate.Number + "-ва рата по полиса " + payment.Rate.PolicyItem.PolicyNumber;
                }
                fciPayment.DebtValue = 0;
                fciPayment.DemandValue = payment.Value;
                fciPayment.SaldoValue = totSaldo + (fciPayment.DebtValue - fciPayment.DemandValue);
                totSaldo = fciPayment.SaldoValue;
                retList.Add(fciPayment);
            }
            return retList;
        }

        public static List<FinanceCardInfo> GetByRateInPeriod(DateTime fromDate, DateTime toDate) {
            decimal totSaldo = 0;
            List<FinanceCardInfo> retList = new List<FinanceCardInfo>();
            List<Rate> lstRates = Rate.GetInPeriod(fromDate, toDate);
            foreach (Rate r in lstRates) {
                FinanceCardInfo fciRate = new FinanceCardInfo();
                fciRate.PolicyNumber = r.PolicyItem.PolicyNumber;
                fciRate.PolicyItemID = r.PolicyItemID;
                fciRate.DocumentDate = r.PolicyItem.Policy.ApplicationDate;
                fciRate.PaidDate = r.Date;
                if (lstRates.Count == 1) {
                    fciRate.Description = "Продадена полиса " + r.PolicyItem.PolicyNumber;
                } else {
                    fciRate.Description = r.Number.ToString() + "-ва рата по полиса " + r.PolicyItem.PolicyNumber;
                }
                fciRate.DebtValue = r.Value;
                fciRate.DemandValue = 0;
                fciRate.SaldoValue = totSaldo + (fciRate.DebtValue - fciRate.DemandValue);
                totSaldo = fciRate.SaldoValue;
                retList.Add(fciRate);
            }
            return retList;
        }

        public static List<FinanceCardInfo> GetByRateInPeriod(DateTime fromDate, DateTime toDate, List<int> lstBranhces,
            List<int> lstInsuranceCompanies, List<int> lstInsuranceSubTypes, List<int> lstUsers, List<int> lstMarketingAgents) {
            decimal totSaldo = 0;
            List<FinanceCardInfo> retList = new List<FinanceCardInfo>();
            List<Rate> lstRates = Rate.GetInPeriod(fromDate, toDate, lstBranhces, lstInsuranceCompanies, lstInsuranceSubTypes, lstUsers, lstMarketingAgents);
            foreach (Rate r in lstRates) {
                FinanceCardInfo fciRate = new FinanceCardInfo();
                fciRate.PolicyNumber = r.PolicyItem.PolicyNumber;
                fciRate.PolicyItemID = r.PolicyItemID;
                fciRate.DocumentDate = r.PolicyItem.Policy.ApplicationDate;
                fciRate.PaidDate = r.Date;
                if (lstRates.Count == 1) {
                    fciRate.Description = "Продадена полиса " + r.PolicyItem.PolicyNumber;
                } else {
                    fciRate.Description = r.Number.ToString() + "-ва рата по полиса " + r.PolicyItem.PolicyNumber;
                }
                fciRate.DebtValue = r.Value;
                fciRate.DemandValue = 0;
                fciRate.SaldoValue = totSaldo + (fciRate.DebtValue - fciRate.DemandValue);
                totSaldo = fciRate.SaldoValue;
                retList.Add(fciRate);
            }
            return retList;
        }

        public static List<FinanceCardInfo> GetByPolicy(Policy p, decimal totSaldo) {
            List<FinanceCardInfo> retList = new List<FinanceCardInfo>();
            List<PolicyItem> lstPI = PolicyItem.GetByPolicy(p.ID);
            foreach (PolicyItem pi in lstPI) {
                List<Rate> lstRates = Rate.GetByPolicyItemID(pi.ID);
                List<Payment> lstPayment = Payment.GetByPolicyItemID(pi.ID);

                foreach (Rate r in lstRates) {
                    FinanceCardInfo fciRate = new FinanceCardInfo();
                    fciRate.DocumentDate = pi.Policy.ApplicationDate;
                    fciRate.PaidDate = r.Date;
                    if (lstRates.Count == 1) {
                        fciRate.Description = "Продадена полиса " + pi.PolicyNumber;
                    } else {
                        fciRate.Description = r.Number.ToString() + "-ва рата по полиса " + pi.PolicyNumber;
                    }
                    fciRate.DebtValue = r.Value;
                    fciRate.DemandValue = 0;
                    fciRate.SaldoValue = totSaldo + (fciRate.DebtValue - fciRate.DemandValue);
                    totSaldo = fciRate.SaldoValue;
                    retList.Add(fciRate);
                }

                foreach (Payment payment in lstPayment) {
                    FinanceCardInfo fciPayment = new FinanceCardInfo();
                    fciPayment.DocumentDate = payment.Date;
                    fciPayment.PaidDate = payment.Rate.Date;
                    if (lstRates.Count == 1) {
                        fciPayment.Description = "Уплата по полиса " + pi.PolicyNumber;
                    } else {
                        fciPayment.Description = "Уплата за " + payment.Rate.Number + "-ва рата по полиса " + pi.PolicyNumber;
                    }
                    fciPayment.DebtValue = 0;
                    fciPayment.DemandValue = payment.Value;
                    fciPayment.SaldoValue = totSaldo + (fciPayment.DebtValue - fciPayment.DemandValue);
                    totSaldo = fciPayment.SaldoValue;
                    retList.Add(fciPayment);
                }
            }
            return retList;
        }
    }


    public class FinanceCardInfo {
        public DateTime DocumentDate { get; set; }
        public DateTime PaidDate { get; set; }
        public string Description { get; set; }
        public decimal DebtValue { get; set; }
        public decimal DemandValue { get; set; }
        public decimal SaldoValue { get; set; }
        public string PolicyNumber { get; set; }
        public int PolicyItemID { get; set; }
    }

    [Serializable]
    public class BankslipInfo {
        public int ID { get; set; }
        public string BankslipNumber { get; set; }
        public int BankID { get; set; }
        public decimal DebtValue { get; set; }
        public decimal DemandValue {get;set;}
        public DateTime Date { get; set; }

        public static BankslipInfo GetFromBankslip(Bankslip b) {
            BankslipInfo bi = new BankslipInfo();
            bi.BankID = b.BankID;
            bi.BankslipNumber = b.BankslipNumber;
            bi.Date = b.Date;
            bi.DebtValue = b.DebtValue;
            bi.DemandValue = b.DemandValue;
            bi.ID = b.ID;
            return bi;
        }
    }

    [Serializable]
    public class BankslipItemInfo {
        public int ID { get; set; }
        public int BankslipID { get; set; }
        public string ClientName { get; set; }
        public string ClientAccountNumber { get; set; }
        public decimal DemandValue { get; set; }
        public decimal DebtValue { get; set; }
        public decimal ProvisionValue { get; set; }
        public string Code { get; set; }
        public string PaymentDescription { get; set; }
        public string CallOnPaymentNumber { get; set; }

        public static BankslipItemInfo GetFromBankslipItem(BankslipItem bi) {
            BankslipItemInfo bii = new BankslipItemInfo();
            bii.BankslipID = bi.BankslipID;
            bii.CallOnPaymentNumber = bi.CallOnPaymentNumber;
            bii.ClientAccountNumber = bi.ClientAccountNumber;
            bii.ClientName = bi.ClientName;
            bii.Code = bi.Code;
            bii.DebtValue = bi.DebtValue;
            bii.DemandValue = bi.DemandValue;
            bii.ID = bi.ID;
            bii.PaymentDescription = bi.PaymentDescription;
            bii.ProvisionValue = bi.ProvisionValue;
            return bii;
        }
    }

}
