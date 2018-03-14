using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InsuranceCompany
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class InsuranceCompany : EntityBase<InsuranceCompany> {

        public static List<InsuranceCompany> GetActiveInsuranceCompanies() {
            return Table.Where(ic => ic.IsActive == true).ToList();
        }

        public static List<InsuranceCompany> GetByDeals(List<Deal> dList) {
            List<InsuranceCompany> icList = new List<InsuranceCompany>();
            foreach (Deal d in dList) {
                if (d.InsuranceCompany.IsActive) {
                    if (!icList.Contains(d.InsuranceCompany)) {
                        icList.Add(d.InsuranceCompany);
                    }
                }
            }
            if (icList.Count > 0) {
                return icList;
            } else {
                return null;
            }

        }

        public static List<InsuranceCompany> GetByDeals() {
            return GetActiveInsuranceCompanies();
        }

        public static List<InsuranceCompany> GetByDealsAndPackets() {
            List<InsuranceCompany> returnList = new List<InsuranceCompany>();
            List<InsuranceCompany> dealsCompanies = GetByActiveDeals();
            List<InsuranceCompany> packetCompanies = GetWithPackets();
            returnList.AddRange(dealsCompanies);
            foreach (InsuranceCompany ic in packetCompanies) {
                if (!returnList.Contains(ic)) {
                    returnList.Add(ic);
                }
            }
            returnList = returnList.OrderBy(l => l.Code).ToList();
            return returnList;
        }

        public static List<InsuranceCompany> GetByDealsAndPacketsAndLifeDeals() {
            List<InsuranceCompany> returnList = new List<InsuranceCompany>();
            List<InsuranceCompany> dealsCompanies = GetByActiveDeals();
            List<InsuranceCompany> packetCompanies = GetWithPackets();
            returnList.AddRange(dealsCompanies);
            foreach (InsuranceCompany ic in packetCompanies) {
                if (!returnList.Contains(ic)) {
                    returnList.Add(ic);
                }
            }
            List<InsuranceCompany> lifeIC = GetByActiveDealsForLife();
            foreach (InsuranceCompany ic in lifeIC) {
                if (!returnList.Contains(ic)) {
                    returnList.Add(ic);
                }
            }
            returnList = returnList.OrderBy(l => l.Code).ToList();
            return returnList;
        }

        public static bool ExistCodeInInsuranceCompanies(string code) {
            return (Table.Where(ic => ic.Code == code && ic.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(ic => ic.Code == code && ic.ID != codeID && ic.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (InsuranceCompany.ExistCodeInInsuranceCompanies(this.Code)) {
                ValidationErrors.Add("INSURANCECOMPANY_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (InsuranceCompany.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("INSURANCECOMPANY_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public static List<InsuranceCompany> GetByActiveDealsForLife() {
            List<LifeDeal> dealsList = LifeDeal.GetActiveDeals();
            List<InsuranceCompany> companiesList = new List<InsuranceCompany>();
            foreach (LifeDeal d in dealsList) {
                if (!companiesList.Contains(d.InsuranceCompany) && d.InsuranceCompany.IsActive) {
                    companiesList.Add(d.InsuranceCompany);
                }
            }
            return companiesList.OrderBy(c => c.Code).ToList();
        }

        public static List<InsuranceCompany> GetByActiveDeals() {
            List<Deal> dealsList = Deal.GetActiveDeals();
            List<InsuranceCompany> companiesList = new List<InsuranceCompany>();
            foreach (Deal d in dealsList) {
                if (!companiesList.Contains(d.InsuranceCompany) && d.InsuranceCompany.IsActive) {
                    companiesList.Add(d.InsuranceCompany);
                }
            }
            return companiesList.OrderBy(c => c.Code).ToList();
        }
        public static List<InsuranceCompany> GetWithPackets() {
            List<InsuranceCompany> icList = new List<InsuranceCompany>();
            List<Packet> pList = Packet.GetActive();
            foreach (Packet p in pList) {
                if (!icList.Contains(p.InsuranceCompany) && p.InsuranceCompany.IsActive) {
                    icList.Add(p.InsuranceCompany);
                }
            }
            return icList;
        }

        public static List<InsuranceCompany> GetByInsuranceSubType(int insuranceSubTypeID) {
            int SubTypeID = InsuranceSubType.Table.First().ID;
            if (insuranceSubTypeID != 0) {
                SubTypeID = insuranceSubTypeID;
            }
            List<Brokerage> brokerageList = Brokerage.GetActiveByInsuranceSubType(SubTypeID);
            List<InsuranceCompany> insCompanyList = new List<InsuranceCompany>();
            foreach (Brokerage b in brokerageList) {
                if (b.Deal.IsActive && b.Deal.InsuranceCompany.IsActive) {
                    if (!insCompanyList.Contains(b.Deal.InsuranceCompany)) {
                        insCompanyList.Add(b.Deal.InsuranceCompany);
                    }
                }
            }
            return insCompanyList;
        }
        public static List<InsuranceCompany> GetByInsuranceSubType() {
            return GetActiveInsuranceCompanies();
        }

        public static List<InsuranceCompany> GetForFacturing(DateTime fromDate, DateTime toDate) {

            string query = @"select insuranceCompanyid from policies p 
                           INNER JOIN policyitems pitems ON p.id = pitems.policyid
                           INNER JOIN cashpayments cp ON cp.policyid = pitems.id
                           where p.applicationdate >='" + fromDate.Year + -+fromDate.Month + -+fromDate.Day +
                           @"' and p.applicationdate<='" + toDate.Year + -+toDate.Month + -+toDate.Day +
                           @"'group by insuranceCompanyid";
            DataClassesDataContext dcdc = new DataClassesDataContext();
            List<int> idList = dcdc.ExecuteQuery<int>(query).ToList();
            List<InsuranceCompany> icList = new List<InsuranceCompany>();
            foreach (int id in idList) {
                icList.Add(InsuranceCompany.Get(id));
            }
            return icList;
        }

        public static InsuranceCompany GetByPrefix(string prefix) {
            return Table.Where(i => i.Prefix == prefix && i.IsActive == true).SingleOrDefault();
        }

    }
}
