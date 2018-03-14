using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Validators;

/// <summary>
/// Summary description for Policy
/// </summary>
/// 
namespace Broker.DataAccess {

    public partial class Policy : EntityBase<Policy> {

        public static List<Policy> GetByClient(int clientID) {
            List<Policy> pList = Table.Where(p => p.ClientID == clientID && p.Discard == false).ToList();
            return pList;
        }

        public static List<Policy> GetByCompanyAndPeriod(int companyID, DateTime startDate, DateTime endDate) {
            List<Policy> pList = Table.Where(p => p.InsuranceCompanyID == companyID && p.ApplicationDate >= startDate && p.ApplicationDate <= endDate).ToList();
            return pList;

        }
        public static List<Policy> GetByCompanyAndPeriodForRollBack(int companyID, DateTime startDate, DateTime endDate) {
            List<PolicyItem> piList = PolicyItem.GetForRollBack(companyID, startDate, endDate);
            List<Policy> pList = new List<Policy>();
            foreach (PolicyItem pi in piList) {
                if (!pList.Contains(pi.Policy)) {
                    pList.Add(pi.Policy);
                }
            }
            return pList;
        }

        public void ValidateSinglePolicy(int dealID, int insuranceSubTypeID) {
            Deal deal = Deal.Get(dealID);
            if (deal == null) {
                ValidationErrors.Add("DealValidator", "Немате избрано валиден договор");
            }
            InsuranceSubType ist = InsuranceSubType.Get(insuranceSubTypeID);
            if (ist == null) {
                ValidationErrors.Add("InsuranceSubType", "Немате избрано подкласа на осигурување");
            }
        }

        //public static Policy GetFromOffer() {
        //    return null;
        //}
        public static Policy GetFromOffer(int oiID) {
            if (oiID != 0) {
                Policy p = new Policy();
                OfferItem oi = OfferItem.Get(oiID);
                p.ClientID = oi.Offer.ClientID;
                p.OwnerID = oi.Offer.OwnerID;
                p.Client = oi.Offer.Client;
                p.Client1 = oi.Offer.Client1;
                p.InsuranceCompany = oi.InsuranceCompany;
                p.InsuranceCompanyID = oi.InsuranceCompanyID;
                return p;
            }
            return null;
        }
        public void ValidateSinglePolicyNumber(int companyID, int insuranceSubTypeID, string number, int branchID) {
            //Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(insuranceSubTypeID, number, companyID);
            Distribution d = Distribution.GetByNumberAndSubTypeAndCompanyIsAccepted(insuranceSubTypeID, number, companyID, branchID);
            if (d == null) {
                // del za proverka na delot za blanko polisi
                RightRestrictionDistribution rrd = RightRestrictionDistribution.ExistForCompany(companyID, number);
                if (rrd != null) {
                    if (!rrd.IsUsed && rrd.DocumentTypeID == DistributionDocumentType.GetByCode(DistributionDocumentType.BLANKO).ID) {
                    } else {
                        this.ValidationErrors.Add("PolicyNumberDistributionValidator", "Бројот на полиса " + number + " не е задолжен");

                    }
                } else {
                    this.ValidationErrors.Add("PolicyNumberDistributionValidator", "Бројот на полиса " + number + " не е задолжен");
                }
            } else {
                if (d.IsUsed) {
                    this.ValidationErrors.Add("PolicyNumberDistributionValidator", "Бројот на полиса " + number + " е искористен");
                }
            }
            // this.PerformCustomValidation();
        }
        public static Policy Insert2(Policy p) {
            return p;
        }

        public static string ValidatePolicyNumberForPacket(string policyNumber, int insuranceCompanyID, int insuranceSubTypeID) {
            Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(insuranceSubTypeID, policyNumber, insuranceCompanyID);
            if (d == null) {
                return "Бројот на полиса " + policyNumber + " не е задолжен";
            }
            if (d.IsUsed) {
                return "Бројот на полиса " + policyNumber + " е искористен";
            }
            return string.Empty;
        }

        public static string ValidateSinglePolicyNumberFromOffer(int companyID, int insuranceSubTypeID, string policyNumber) {
            Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(insuranceSubTypeID, policyNumber, companyID);
            if (d == null) {
                return "Бројот на полиса " + policyNumber + " не е задолжен";
            } else {
                if (d.IsUsed) {
                    return "Бројот на полиса " + policyNumber + " е искористен";
                }
            }
            return string.Empty;
        }

        public static List<string> GetForFiscalBill(int policyID) {
            Policy p = Policy.Get(policyID);
            char[] separator = { ' ', '#', ';' };
            List<string> returnList = new List<string>();
            string startString = "01,0000,1";
            startString += separator[2];
            returnList.Add(startString);
            SpecialFieldType sft = SpecialFieldType.GetByCode(SpecialFieldType.FISCAL);
            int i = 0;
            foreach (PolicyItem pi in p.PolicyItems) {
                char currentSeparator = separator[i % 2];
                string s = currentSeparator.ToString() + pi.InsuranceSubType.ShortDescription;
                s += '\t';
                s += pi.PremiumValue;
                s += separator[2].ToString();
                returnList.Add(s);
                ++i;
                List<Control> controlsList = Control.GetForFiscalByInsuranceSubType(pi.InsuranceSubTypeID);
                foreach (Control c in controlsList) {
                    currentSeparator = separator[i % 2];
                    s = currentSeparator.ToString() + c.Description;
                    s += '\t';
                    PolicyExtendInformation pei = PolicyExtendInformation.GetByPolicyItemAndControl(pi.ID, c.ID);
                    s += String.Format("{0:#,0.00}", pei.Value);
                    s += separator[2].ToString();
                    returnList.Add(s);
                    i++;
                }
            }
            string lastString = "%8";
            returnList.Add(lastString);
            return returnList;
        }

        public static List<FinCardSaldo> GetForFinCard(int clientID) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"select p.id, pi.policynumber, p.applicationdate, pi.premiumvalue as dolzi,
                            isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as pobaruva,
                            isnull(pi.premiumvalue - (select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as saldo
                            from policies p, policyitems pi
                            where pi.policyid=p.id
                            and p.clientid = " + clientID +
                            @" order by p.id";
            List<FinCardSaldo> lst = dc.ExecuteQuery<FinCardSaldo>(query).ToList();
            return lst;
        }

        public static List<FinCardSaldo> GetForFacture(int factureID) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"select p.id, pi.policynumber, p.applicationdate, pi.premiumvalue as dolzi,
                            isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as pobaruva,
                            pi.premiumvalue - isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as saldo
                            from policies p, policyitems pi, policyitemfactureitems pifi, factureitems fi
                            where pi.policyid=p.id
                            and pifi.policyitemid=pi.id
                            and pifi.factureitemid=fi.id
                            and fi.factureid= " + factureID +
                            @" order by p.id";
            List<FinCardSaldo> lst = dc.ExecuteQuery<FinCardSaldo>(query).ToList();
            return lst;
        }

        public static List<FinCardSaldo> GetForFinCard(int clientID, string itemsType) {
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = string.Empty;
            query = @"select p.id, pi.policynumber, p.applicationdate, pi.premiumvalue as dolzi,
                            isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as pobaruva,
                            pi.premiumvalue - isnull((select sum(pay.value) from policies p1, policyitems pi1, rates r, payments pay
                            where pi1.policyid=p1.id and r.policyitemid=pi1.id
                            and pay.rateid=r.id
                            and p1.id=p.id),0) as saldo
                            from policies p, policyitems pi
                            where pi.policyid=p.id
                            and p.clientid = " + clientID +
                           @" order by p.applicationdate desc";

            List<FinCardSaldo> lst = dc.ExecuteQuery<FinCardSaldo>(query).ToList();
            if (itemsType == "OpenItems") {
                lst = lst.Where(c => c.dolzi != c.pobaruva).OrderBy(c => c.applicationdate).ToList();
            }
            return lst;
        }
    }

    public class FinCardSaldo {
        public int id { get; set; }
        public string policynumber { get; set; }
        public DateTime applicationdate { get; set; }
        public decimal dolzi { get; set; }
        public decimal pobaruva { get; set; }
        public decimal saldo { get; set; }
    }
}
