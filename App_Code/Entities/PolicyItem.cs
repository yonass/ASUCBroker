using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Validators;

/// <summary>
/// Summary description for PolicyItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class PolicyItem : EntityBase<PolicyItem> {

        public static PolicyItem GetByNumber(string policyNumber, int insuranceCompanyID) {
            return Table.Where(pi => pi.PolicyNumber == policyNumber && pi.Policy.InsuranceCompanyID == insuranceCompanyID && pi.Policy.Discard == false).SingleOrDefault();
        }

        public static PolicyItem GetByNumberAndInsuranceCompany(string policyNumber, int insuranceCompanyID) {
            return Table.Where(pi => pi.PolicyNumber == policyNumber && pi.Policy.InsuranceCompanyID == insuranceCompanyID && pi.Policy.Discard == false).SingleOrDefault();
        }

        public static PolicyItem GetByNumberAndInsuranceSubType(string policyNumber, int insuranceSubTypeID, int insuranceCompanyID) {
            return Table.Where(pi => pi.PolicyNumber == policyNumber && pi.InsuranceSubTypeID == insuranceSubTypeID && pi.Policy.InsuranceCompanyID == insuranceCompanyID).SingleOrDefault();
        }

        public static List<InsuranceSubType> GetByPolicyNumber(string policyNumber, int insuranceCompanyID) {
            return Table.Where(pi => pi.PolicyNumber == policyNumber && pi.Policy.InsuranceCompanyID == insuranceCompanyID && pi.IsPaidInBrokerHouse == false && pi.Policy.Discard == false).Select(pi => pi.InsuranceSubType).ToList();
        }

        public static List<PolicyItem> GetListByPolicyNumber(string policyNumber) {
            return Table.Where(pi => pi.PolicyNumber == policyNumber).ToList();
        }

        public void ValidatePolicyItem() {
            if (!PolicyNumberValidator.IsDistributed(this.PolicyNumber, this.InsuranceSubTypeID, this.Policy.InsuranceCompanyID)) {
                ValidationErrors.Add("PolicyItemNumber", "Бројот на полисата не е задолжен");
            }
        }

        public static int GetPolicyIDForPolicyItemsWithMaxCount() {
            string query = @"select policyid
                            from policyitems
                            group by policyid
                            order by count(1)";
            //List<int> myList = new List<int>();
            DataClassesDataContext db = new DataClassesDataContext();
            List<int> highCount = db.ExecuteQuery<int>(query).ToList();
            //IEnumerable<int> neso = db.ExecuteQuery<int>(query);
            if (highCount.Count == 0) {
                return 0;
            } else {
                return highCount.Last();
            }
        }

        public static List<PolicyItem> GetForFacture(Facture f, int insuranceCompanyID) {
            List<PolicyItem> pList = new List<PolicyItem>();

            List<Payment> paymentList = Payment.GetByDateAndInsuranceCompany(f.FromDate, f.ToDate, insuranceCompanyID);
            foreach (Payment pay in paymentList) {
                if (!pList.Contains(pay.Rate.PolicyItem)) {
                    pList.Add(pay.Rate.PolicyItem);
                }
            }
            return pList;
        }

        public static List<PolicyItem> GetForFactureCash(Facture f, int insuranceCompanyID) {

            List<PolicyItem> pList = Table.Where(p => p.Policy.ApplicationDate >= f.FromDate && p.Policy.ApplicationDate <= f.ToDate && p.Policy.InsuranceCompanyID == insuranceCompanyID).ToList();
            return pList;
        }

        public static List<PolicyItem> GetForRollBack(int insuranceCompanyID, DateTime startDate, DateTime endDate) {
            return Table.Where(c => c.IsRollBacked == false && c.Policy.ApplicationDate >= startDate && c.Policy.ApplicationDate <= endDate && c.Policy.InsuranceCompanyID == insuranceCompanyID).ToList();

        }

        public static List<PolicyItem> GetForDatePeriod(DateTime startDate, DateTime endDate) {
            return Table.Where(pi => pi.Policy.ApplicationDate >= startDate && pi.Policy.ApplicationDate <= endDate).OrderBy(pi => pi.IsPaidInBrokerHouse).ThenBy(pi => pi.PolicyNumber).ToList();
        }

        public static List<PolicyItem> GetForDatePeriod(DateTime startDate, DateTime endDate, bool isPaidInBrokerHouse) {
            return Table.Where(pi => pi.Policy.ApplicationDate >= startDate && pi.Policy.ApplicationDate <= endDate && pi.IsPaidInBrokerHouse == isPaidInBrokerHouse).OrderBy(pi => pi.PolicyNumber).ToList();
        }

        public static List<PolicyItem> GetByCompanyAndPeriodAndSubType(DateTime startDate, DateTime endDate, int insuranceCompanyID, int inusranceSubTypeID) {
            return Table.Where(pi => pi.InsuranceSubTypeID == inusranceSubTypeID && pi.Policy.ApplicationDate <= endDate && pi.Policy.ApplicationDate >= startDate && pi.Policy.InsuranceCompanyID == insuranceCompanyID && pi.Policy.Discard == false).ToList();
        }

        public static List<PolicyItem> GetByPeriodAndSubType(DateTime startDate, DateTime endDate, int inusranceSubTypeID) {
            return Table.Where(pi => pi.InsuranceSubTypeID == inusranceSubTypeID && pi.Policy.ApplicationDate <= endDate && pi.Policy.ApplicationDate >= startDate && pi.Policy.Discard == false).ToList();
        }


        public static decimal GetTotalSum(List<PolicyItem> piList) {
            decimal totalSum = 0;
            foreach (PolicyItem pi in piList) {
                if (pi.IsRollBacked) {
                    totalSum += pi.PremiumValue;
                }
            }
            return totalSum;
        }

        //public static List<PolicyItem> GetNotFactured(DateTime fromDate,DateTime toDate, int companyID) {

        //    List<PolicyItem> previousPolicies = GetFromPreivousMonths(fromDate, companyID);


        //  List<int> pitemsList = new List<int>();



        //}

        public static List<PolicyItem> GetNonIncomeFactured(int insuranceCompanyID) {
            return Table.Where(c => c.Policy.InsuranceCompanyID == insuranceCompanyID && c.PolicyIncomeFactureStatuse.Code == PolicyIncomeFactureStatuse.NE_E_FAKTURIRANA
                 && c.Policy.Discard == false).ToList();
        }

        public static List<PolicyItem> GetFromPreivousMonthsForAccountFactures(DateTime startDate, int companyID) {
            return Table.Where(c => c.Policy.Discard == false && c.Policy.InsuranceCompanyID == companyID && c.IsAccountFactured == false && c.Policy.ApplicationDate.Date <= startDate.Date).ToList();
        }

        public static List<PolicyItem> GetNonBrokerageSliped(int insuranceCompanyID, DateTime startDate, DateTime endDate) {
            return Table.Where(c => c.Policy.InsuranceCompanyID == insuranceCompanyID && c.IsBrokerageSliped == false
                 && c.Policy.Discard == false && c.Policy.ApplicationDate.Date >= startDate.Date && c.Policy.ApplicationDate.Date <= endDate.Date).ToList();
        }


        public static List<PolicyItem> GetFromPreivousMonths(DateTime startDate, int companyID) {


            DataClassesDataContext dcdc = new DataClassesDataContext();

            List<PolicyItem> policyItemsList = new List<PolicyItem>();


            string queryPayments = @"select pitems.id from policies p 
                                     INNER JOIN policyitems pitems ON p.id = pitems.policyid
                                     INNER JOIN rates r ON r.policyitemid=pitems.id 
                                     INNER JOIN payments pay ON pay.rateid = r.id
                                     where p.discard = 0 and pay.date<'" + startDate.Year + -+startDate.Month + -+startDate.Day +
                                     @"'and pay.isfactured = 0 and p.insurancecompanyid=" + companyID + " group by pitems.id";

            IEnumerable<int> idPaymentsList = dcdc.ExecuteQuery<int>(queryPayments);
            foreach (int id in idPaymentsList) {
                policyItemsList.Add(PolicyItem.Get(id));
            }
            return policyItemsList;
        }

        public static List<PolicyItem> GetForAccountFacturingFromCurrentMonth(DateTime startDate, DateTime endDate, int ic) {
            return Table.Where(c => c.Policy.Discard == false && c.Policy.InsuranceCompanyID == ic && c.IsAccountFactured == false
                && c.Policy.ApplicationDate.Date >= startDate.Date && c.Policy.ApplicationDate.Date <= endDate.Date).ToList();

        }

        public static List<PolicyItem> GetByPolicy(int policyID) {
            return Table.Where(c => c.PolicyID == policyID).ToList();
        }

        public static List<PolicyItem> GetForFacturingFromCurrentMonth(DateTime startDate, DateTime endDate, int ic) {
            List<Payment> paymentList = Payment.GetByDateAndInsuranceCompany(startDate, endDate, ic);
            Dictionary<int, decimal[]> dictionary = new Dictionary<int, decimal[]>();
            List<PolicyItem> policyItemList = new List<PolicyItem>();
            foreach (Payment p in paymentList) {

                PolicyItem policyItem = p.Rate.PolicyItem;
                if (!policyItemList.Contains(policyItem)) {
                    policyItemList.Add(policyItem);
                }
            }
            return policyItemList;
        }

        public static decimal GetPremiumValueForSummuryRollBacks(bool isRollBack, DateTime fromDate, DateTime toDate, bool isPaidInBrokerHouse, int insuranceCompanyID, PaymentType pt, out int count) {
            decimal sum = 0;
            count = 0;
            List<PolicyItem> listPolicyItems = Table.Where(p => p.Policy.Discard == false && p.Policy.ApplicationDate.Date >= fromDate && p.Policy.ApplicationDate.Date <= toDate && p.IsRollBacked == isRollBack && p.Policy.InsuranceCompanyID == insuranceCompanyID).ToList();
            if (isPaidInBrokerHouse) {
                foreach (PolicyItem pi in listPolicyItems) {
                    if (pi.IsPaidInBrokerHouse) {
                        count++;
                        sum += pi.PremiumValue;
                    }
                }
            //} else {
            //    foreach (PolicyItem pi in listPolicyItems) {
            //        if (!pi.IsPaidInBrokerHouse) {
            //            List<Rate> ratesList = Rate.GetByPolicyItemID(pi.ID);
            //            int paymentTypeID = pt.ID;
            //            if (ratesList[0].PaymentTypeID == paymentTypeID) {
            //                count++;
            //                sum += pi.PremiumValue;
            //            }
            //        }
            //    }
            }
            return sum;
        }

        public static void SetIsAccountFactured(List<PolicyItem> piList) {
            foreach (PolicyItem pi in piList) {
                pi.IsAccountFactured = true;
                PolicyItem.Table.Context.SubmitChanges();
            }
        }

        public static decimal GetPremiumValueForSummury(DateTime fromDate, DateTime toDate, bool isPaidInBrokerHouse, int insuranceCompanyID, PaymentType pt, out int count) {
            decimal sum = 0;
            count = 0;
            List<PolicyItem> listPolicyItems = Table.Where(p => p.Policy.Discard == false && p.Policy.ApplicationDate.Date >= fromDate && p.Policy.ApplicationDate.Date <= toDate && p.Policy.InsuranceCompanyID == insuranceCompanyID).ToList();
            if (isPaidInBrokerHouse) {
                foreach (PolicyItem pi in listPolicyItems) {
                    if (pi.IsPaidInBrokerHouse) {
                        count++;
                        sum += pi.PremiumValue;
                    }
                }
            //} else {
            //    foreach (PolicyItem pi in listPolicyItems) {
            //        if (!pi.IsPaidInBrokerHouse) {
            //            List<Rate> ratesList = Rate.GetByPolicyItemID(pi.ID);
            //            int paymentTypeID = pt.ID;
            //            if (ratesList[0].PaymentTypeID == paymentTypeID) {
            //                count++;
            //                sum += pi.PremiumValue;
            //            }
            //        }
            //    }
            }
            return sum;
        }

        public static List<PolicyItem> SearchPolicyItem(int controlID, string controlValue, int insuranceSubTypeID) {
            List<PolicyItem> retList = new List<PolicyItem>();
            if (controlID > 0) {
                PolicyItem pi = PolicyExtendInformation.SearchPolicyItemByControl(controlID, controlValue);
                if (pi != null) {
                    retList.Add(pi);
                }
                return retList;
            } else {
                try {
                    if (controlID == 0) {
                        retList = Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.PolicyNumber == controlValue && c.Policy.Discard == false).ToList();
                        return retList;
                    } else if (controlID == -1) {
                        retList = Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.Policy.Discard == false && c.Policy.Client.EMBG == controlValue).ToList();
                        return retList;
                    } else if (controlID == -2) {
                        retList = Table.Where(c => c.InsuranceSubTypeID == insuranceSubTypeID && c.Policy.Discard == false && c.Policy.Client1.EMBG == controlValue).ToList();
                        return retList;
                    }
                    return retList;
                } catch (Exception ex) {
                    return retList;
                }
            }
        }

        public static decimal GetPremiumValueForExistingRollBack(int rollBackID, bool isPaidInBrokerHouse, PaymentType pt, out int count) {
            decimal sum = 0;
            count = 0;
            List<RollBackDistributionItem> rbdiList = RollBackDistributionItem.GetByRollBackDistibution(rollBackID);
            if (isPaidInBrokerHouse) {
                foreach (RollBackDistributionItem rbdi in rbdiList) {
                    if (rbdi.PolicyItem.IsPaidInBrokerHouse && !rbdi.PolicyItem.Policy.Discard) {
                        count++;
                        sum += rbdi.PolicyItem.PremiumValue;
                    }
                }
            //} else {
            //    foreach (RollBackDistributionItem rbdi in rbdiList) {
            //        if (!rbdi.PolicyItem.IsPaidInBrokerHouse && !rbdi.PolicyItem.Policy.Discard) {
            //            List<Rate> ratesList = Rate.GetByPolicyItemID(rbdi.PolicyItemID);
            //            int paymentTypeID = pt.ID;
            //            if (ratesList[0].PaymentTypeID == paymentTypeID) {
            //                count++;
            //                sum += rbdi.PolicyItem.PremiumValue;
            //            }
            //        }
            //    }
            }
            return sum;
        }

        public static List<PolicyItem> GetForRollBackFromPreviousMonths(DateTime startDate, int insuranceCompanyID, int branchID) {
            string query = @"select pitems.* from policies p 
                            INNER JOIN policyitems pitems ON p.id = pitems.policyid
                            where p.applicationdate<'" + startDate.Year + -+startDate.Month + -+startDate.Day +
                           @"'and pitems.IsRollBacked = 0 and p.insurancecompanyid=" + insuranceCompanyID +
                           @" and p.BranchID = " + branchID;

            DataClassesDataContext dcdc = new DataClassesDataContext();
            List<PolicyItem> policyNumbers = dcdc.ExecuteQuery<PolicyItem>(query).ToList();
            return policyNumbers;

        }

        public static List<PolicyItem> GetForRollBackFromCurrentMonth(DateTime startDate, DateTime endDate, int insuranceCompanyID, int branchID) {
            string query = @"select pitems.* from policies p 
                            INNER JOIN policyitems pitems ON p.id = pitems.policyid
                            where p.applicationdate>='" + startDate.Year + -+startDate.Month + -+startDate.Day +
                           @"' and p.applicationdate<='" + endDate.Year + -+endDate.Month + -+endDate.Day +
                           @"'and pitems.IsRollBacked = 0 and p.insurancecompanyid=" + insuranceCompanyID +
                           @" and branchID = " + branchID;

            DataClassesDataContext dcdc = new DataClassesDataContext();
            List<PolicyItem> policyNumbers = dcdc.ExecuteQuery<PolicyItem>(query).ToList();
            return policyNumbers;
        }

        public static List<PolicyItem> GetForGroupFactures(DateTime startDate, DateTime endDate, int clientID) {
            string query = @"select pi.* from clients c
                            INNER JOIN policies p ON c.id = p.clientID
                            INNER JOIN policyitems pi ON pi.policyID = p.ID
                            WHERE p.discard = 0 and pi.IsFactureCreated = 0 and
                            p.ApplicationDate >='" + startDate.Year + -+startDate.Month + -+startDate.Day +
                            @"' and p.applicationdate<='" + endDate.Year + -+endDate.Month + -+endDate.Day +
                            @"'and c.ID ="+clientID;
            DataClassesDataContext dcdc = new DataClassesDataContext();
            List<PolicyItem> policyItems = dcdc.ExecuteQuery<PolicyItem>(query).ToList();
            return policyItems;
        }

        public void UpdateFacturedStatus(bool isFactured) {
            this.IsFactureCreated = isFactured;
            Table.Context.SubmitChanges();
        }

    }
}
