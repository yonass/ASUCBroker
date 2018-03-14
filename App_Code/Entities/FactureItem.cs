using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FactureItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class FactureItem : EntityBase<FactureItem> {

        public static List<FactureItem> GetByFacture(int factureID) {
            return Table.Where(fi => fi.FactureID == factureID).ToList();
        }

        public static List<FactureItemGrouped> GetFactureItemsForCompany(int companyID, DateTime fromDate, DateTime toDate) {

           DataClassesDataContext dcdc = new DataClassesDataContext();

            DateTime dt1 = toDate;
            DateTime dt2 = fromDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;
            
            string query1 = @"SELECT count(p.ID)as PaymentsCount,sum(p.value) as TotalValue,ist.ID as InsuranceSubTypeID
                                FROM Payments p INNER JOIN rates r ON p.rateID = r.ID
                                INNER JOIN policyitems poli ON poli.ID = r.PolicyItemID
								INNER JOIN Policies pol ON poli.policyID = pol.ID
								INNER JOIN InsuranceSubTypes ist ON ist.ID = poli.InsuranceSubTypeID
								INNER JOIN InsuranceCompanies ic ON ic.ID =pol.InsuranceCompanyID
                                WHERE ic.ID= " + companyID +
                                @" AND p.Date <= '" + y1 + -+m1 + -+d1 +
                                    @"' AND p.Date >= '" + y2 + -+m2 + -+d2 +
                                    @"'GROUP BY ist.ID";
            List<FactureItemGrouped> fit = dcdc.ExecuteQuery<FactureItemGrouped>(query1).ToList();



            
            return fit;
        }


        public static List<FactureItemGrouped> GetForCashPayments(int companyID, DateTime fromDate, DateTime toDate) {

            DataClassesDataContext dcdc = new DataClassesDataContext();

            DateTime dt1 = toDate;
            DateTime dt2 = fromDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;
            
            string query2 = @" SELECT count(p.ID)as PaymentsCount,sum(cpi.value) as TotalValue,ist.ID as InsuranceSubTypeID
                                FROM CashPayments p 
								INNER JOIN Policies pol ON p.policyID = pol.ID
								INNER JOIN PolicyItems poli ON pol.ID = poli.policyID
								INNER JOIN CashPaymentItems cpi ON p.ID= cpi.CashPaymentID
								INNER JOIN InsuranceSubTypes ist ON ist.ID = poli.InsuranceSubTypeID
								INNER JOIN InsuranceCompanies ic ON ic.ID =pol.InsuranceCompanyID
                                WHERE p.Discarded = 0 and ic.ID= " + companyID +
                                @" AND pol.ApplicationDate <= '" + y1 + -+m1 + -+d1 +
                                    @"' AND pol.ApplicationDate >= '" + y2 + -+m2 + -+d2 +
                                    @"'GROUP BY ist.ID";

            List<FactureItemGrouped> fit = dcdc.ExecuteQuery<FactureItemGrouped>(query2).ToList();
            return fit;
        }

        public class FactureItemGrouped {
            public int PaymentsCount { get; set; }
            public decimal TotalValue { get; set; }
            public int InsuranceSubTypeID { get; set; }
        }
    }

}
