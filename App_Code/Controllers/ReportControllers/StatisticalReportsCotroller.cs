using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for StatisticalReportsCotroller
/// </summary>
namespace Broker.Controllers.ReportControllers {
    public class StatisticalReportsCotroller {
        public StatisticalReportsCotroller() {

        }

        public static void GetPremiumStatisticsForTimePeriod(DateTime startDate, DateTime endDate) {
            DataClassesDataContext dcdc = new DataClassesDataContext();
            DateTime dt1 = startDate;
            DateTime dt2 = endDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;
            string s = @"select it.ID as TypeID,ic.ID as CompanyID,sum(pitems.premiumvalue) as TotalSum,sum(pitems.id) as TotalCount, it.IsForLive as IsForLive from policyitems 
                    pitems INNER JOIN policies p ON pitems.policyid =p.id 
                    INNER JOIN insurancesubtypes ist ON pitems.InsuranceSubTypeID = ist.ID
                    INNER JOIN insurancetypes it ON it.ID = ist.insurancetypeID
                    INNER JOIN insuranceCompanies ic ON ic.ID = p.insuranceCompanyID
                    where p.applicationdate>='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 +
                        @"'and p.discard = 0
                    group by it.ID, ic.ID,it.IsForLive 
                    order by it.ID";


            List<ReportClass> summaryList = dcdc.ExecuteQuery<ReportClass>(s).ToList();
            List<InsuranceCompany> icList = new List<InsuranceCompany>();
         //   List<InsuranceType> itList = new List<InsuranceType>();
            foreach (ReportClass rc in summaryList) {
                InsuranceCompany ic = icList.Where(company => company.ID == rc.CompanyID).SingleOrDefault();
                if (ic == null) {
                    icList.Add(InsuranceCompany.Get(rc.CompanyID));
                }
                //InsuranceType it = itList.Where(type => type.ID == rc.TypeID).SingleOrDefault();
                //if (it == null) {
                //    itList.Add(InsuranceType.Get(rc.TypeID));
                //}
            }

            
            icList = icList.OrderBy(c => c.ID).ToList();
          //  itList = itList.OrderBy(t => t.ID).ToList();
            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(660, 480);
            pdf.SetTitle("ЗБИРНА СТАТИСТКА ЗА БРУТО ПОЛИСИРАНА ПРЕМИЈА ПО КЛАСИ");
            pdf.SetTitle("НА ОСИГУРУВАЊЕ ЗА ПЕРИОД " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());
           
            pdf.SetTitleLeft("");

            //pdf.SetTitleRight("во 000 ден.");

            string[] headers = new string[icList.Count + 3];
            headers[0] = "";
            headers[1] = "Класи на осигурување";
            for (int i = 0; i < icList.Count; i++) {
                headers[2+i] = icList[i].ShortName;
            }

            headers[icList.Count + 2] = "Вкупно";
            TypeCode[] codes = new TypeCode[headers.Count()];

            codes[0] = TypeCode.Int32;
            codes[1] = TypeCode.String;
            for (int i = 0; i <= icList.Count; i++) {
                codes[2 + i] = TypeCode.Decimal;
            }
            float[] policyColumnsWidths = new float[headers.Count()];
            policyColumnsWidths[0] = 2;
            policyColumnsWidths[1] = 18;
            for (int i = 0; i <= icList.Count; i++) {
                policyColumnsWidths[2 + i] = 80 / (icList.Count+1);
            }
            int count = 0;
        
           List<InsuranceType> itList = InsuranceType.Select().OrderBy(c=>c.ID).ToList();
            pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
            foreach(InsuranceType it in itList){
                decimal totalTypeSum = 0;
                List<ReportClass> subList = summaryList.Where(sl => sl.TypeID == it.ID).ToList();
                object[] vals = new object[headers.Count()];
                vals[0] = ++count;
                vals[1] = it.ShortName;
                for (int i = 0; i < icList.Count; i++) {
                    ReportClass rcItem = subList.Where(cl => cl.CompanyID == icList[i].ID).SingleOrDefault();
                    if (rcItem == null) {
                        vals[i + 2] = 0;
                    } else {
                        vals[i + 2] = String.Format("{0:#,0.00}", rcItem.TotalSum);
                        totalTypeSum+=rcItem.TotalSum;
                    }
                }
                vals[headers.Count() - 1] = String.Format("{0:#,0.00}", totalTypeSum); 
                
                pdf.AddDataRowWithBorder(vals,headers.Count(),codes);
            }
            // za kreiranje na vkupno

            object[] totalVals = new object[headers.Count()];
            totalVals[0] = "";
            totalVals[1] = "Вкупно неживот";
            decimal total=0;
            for (int i = 0; i < icList.Count; i++) {

                List<ReportClass> rcSubList = summaryList.Where(c => c.CompanyID == icList[i].ID && c.IsForLive == false).ToList();
                decimal totalForCompany = 0;
                foreach (ReportClass rc in rcSubList) {
                    totalForCompany += rc.TotalSum;
                }

                totalVals[i + 2] = totalForCompany;
                total += totalForCompany;
            }
            totalVals[headers.Count() - 1] = String.Format("{0:#,0.00}", total); ;
            pdf.AddDataRowWithBorder(totalVals, headers.Count(), codes);

            object[] totalValsLive = new object[headers.Count()];
            totalValsLive[0] = "";
            totalValsLive[1] = "Вкупно живот";
            decimal totalLive = 0;

            for (int i = 0; i < icList.Count; i++) {

                List<ReportClass> rcSubList = summaryList.Where(c => c.CompanyID == icList[i].ID && c.IsForLive == true).ToList();
                decimal totalForCompany = 0;
                foreach (ReportClass rc in rcSubList) {
                    totalForCompany += rc.TotalSum;
                }

                totalValsLive[i + 2] = String.Format("{0:#,0.00}", totalForCompany);
                totalLive += totalForCompany;
            }
            totalValsLive[headers.Count() - 1] = String.Format("{0:#,0.00}", totalLive); 
            pdf.AddDataRowWithBorder(totalValsLive, headers.Count(), codes);

            pdf.AddTable();
            pdf.FinishPDF();
        }


        public static void GetNumberStatisticsForTimePeriod(DateTime startDate, DateTime endDate) {
            DataClassesDataContext dcdc = new DataClassesDataContext();
            DateTime dt1 = startDate;
            DateTime dt2 = endDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;
            string s = @"select it.ID as TypeID,ic.ID as CompanyID,sum(pitems.premiumvalue) as TotalSum,count(pitems.id) as TotalCount, it.IsForLive as IsForLive from policyitems 
                    pitems INNER JOIN policies p ON pitems.policyid =p.id 
                    INNER JOIN insurancesubtypes ist ON pitems.InsuranceSubTypeID = ist.ID
                    INNER JOIN insurancetypes it ON it.ID = ist.insurancetypeID
                    INNER JOIN insuranceCompanies ic ON ic.ID = p.insuranceCompanyID
                    where p.applicationdate>='" + y1 + -+m1 + -+d1 + "' and p.applicationdate<='" + y2 + -+m2 + -+d2 +
                        @"'and p.discard = 0
                    group by it.ID, ic.ID,it.IsForLive 
                    order by it.ID";


            List<ReportClass> summaryList = dcdc.ExecuteQuery<ReportClass>(s).ToList();
            List<InsuranceCompany> icList = new List<InsuranceCompany>();
            foreach (ReportClass rc in summaryList) {
                InsuranceCompany ic = icList.Where(company => company.ID == rc.CompanyID).SingleOrDefault();
                if (ic == null) {
                    icList.Add(InsuranceCompany.Get(rc.CompanyID));
                }
            }


            icList = icList.OrderBy(c => c.ID).ToList();
            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.AddJDBLogo(660, 480);
            pdf.SetTitle("ЗБИРНА СТАТИСТКА ЗА БРОЈ НА ПОЛИСИ ПО КЛАСИ НА ОСИГУРУВАЊЕ");
            pdf.SetTitle("ЗА ПЕРИОД " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());

            pdf.SetTitleLeft("");
                     

            string[] headers = new string[icList.Count + 3];
            headers[0] = "";
            headers[1] = "Класи на осигурување";
            for (int i = 0; i < icList.Count; i++) {
                headers[2 + i] = icList[i].ShortName;
            }

            headers[icList.Count + 2] = "Вкупно";
            TypeCode[] codes = new TypeCode[headers.Count()];

            codes[0] = TypeCode.Int32;
            codes[1] = TypeCode.String;
            for (int i = 0; i <= icList.Count; i++) {
                codes[2 + i] = TypeCode.Int32;
            }
            float[] policyColumnsWidths = new float[headers.Count()];
            policyColumnsWidths[0] = 2;
            policyColumnsWidths[1] = 18;
            for (int i = 0; i <= icList.Count; i++) {
                policyColumnsWidths[2 + i] = 80 / (icList.Count + 1);
            }
            int count = 0;

            List<InsuranceType> itList = InsuranceType.Select().OrderBy(c => c.ID).ToList();
            pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
            foreach (InsuranceType it in itList) {
                decimal totalTypeCount = 0;
                List<ReportClass> subList = summaryList.Where(sl => sl.TypeID == it.ID).ToList();
                object[] vals = new object[headers.Count()];
                vals[0] = ++count;
                vals[1] = it.ShortName;
                for (int i = 0; i < icList.Count; i++) {
                    ReportClass rcItem = subList.Where(cl => cl.CompanyID == icList[i].ID).SingleOrDefault();
                    if (rcItem == null) {
                        vals[i + 2] = 0;
                    } else {
                        vals[i + 2] = String.Format("{0:#,0}", rcItem.TotalCount);
                        totalTypeCount += rcItem.TotalCount;
                    }
                }
                vals[headers.Count() - 1] = String.Format("{0:#,0}", totalTypeCount);

                pdf.AddDataRowWithBorder(vals, headers.Count(), codes);
            }
            // za kreiranje na vkupno

            object[] totalVals = new object[headers.Count()];
            totalVals[0] = "";
            totalVals[1] = "Вкупно неживот";
            decimal total = 0;
            for (int i = 0; i < icList.Count; i++) {

                List<ReportClass> rcSubList = summaryList.Where(c => c.CompanyID == icList[i].ID && c.IsForLive == false).ToList();
                decimal totalForCompany = 0;
                foreach (ReportClass rc in rcSubList) {
                    totalForCompany += rc.TotalCount;
                }

                totalVals[i + 2] = String.Format("{0:#,0}", totalForCompany);
                total += totalForCompany;
            }
            totalVals[headers.Count() - 1] = String.Format("{0:#,0}", total);
            pdf.AddDataRowWithBorder(totalVals, headers.Count(), codes);

            object[] totalValsLive = new object[headers.Count()];
            totalValsLive[0] = "";
            totalValsLive[1] = "Вкупно живот";
            decimal totalLive = 0;

            for (int i = 0; i < icList.Count; i++) {

                List<ReportClass> rcSubList = summaryList.Where(c => c.CompanyID == icList[i].ID && c.IsForLive == true).ToList();
                decimal totalForCompany = 0;
                foreach (ReportClass rc in rcSubList) {
                    totalForCompany += rc.TotalCount;
                }

                totalValsLive[i + 2] = String.Format("{0:#,0}", totalForCompany);
                totalLive += totalForCompany;
            }
            totalValsLive[headers.Count() - 1] = String.Format("{0:#,0}", totalLive);
            pdf.AddDataRowWithBorder(totalValsLive, headers.Count(), codes);

            pdf.AddTable();
            pdf.FinishPDF();
        }
        
    }
    public class ReportClass {
        public int TypeID { get; set; }
        public int CompanyID { get; set; }
        public decimal TotalSum { get; set; }
        public int TotalCount { get; set; }
        public bool IsForLive { get; set; }
    }
}