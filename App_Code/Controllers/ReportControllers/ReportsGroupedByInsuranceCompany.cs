using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;

/// <summary>
/// Summary description for ReportsGroupedByInsuranceCompany
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers
{
    public class ReportsGroupedByInsuranceCompany
    {
        public static void ReportsForPaymentsTypesGroupedByInsuranceCompany(DateTime fromDate, DateTime toDate)
        {
            List<InsuranceCompany> icList = new List<InsuranceCompany>();
            icList = InsuranceCompany.Select().ToList();
            PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("ЗБИРНА СТАТИСТИКА ЗА ПРОДАЖБА ПО ТИП НА ПЛАЌАЊЕ КАЈ ОСИГУРИТЕЛНА КОМПАНИЈА");
            pdf.SetTitle("ЗА ПЕРИОД " + fromDate.ToShortDateString() + " - " + toDate.ToShortDateString());
            pdf.SetTitleLeft("");
            string[] headers = new string[4];
            headers[0] = "Реден број";
            headers[1] = "Тип на плаќање";
            headers[2] = "Број на полиси";
            headers[3] = "Вкупен износ";
            
            
            TypeCode[] codes = new TypeCode[headers.Count()];
            codes[0] = TypeCode.Int32;
            codes[1] = TypeCode.String;
            codes[2] = TypeCode.Int32;
            codes[3] = TypeCode.Decimal;

            float[] policyColumnsWidths = new float[headers.Count()];
            policyColumnsWidths[0] = 10;
            policyColumnsWidths[1] = 50;
            policyColumnsWidths[2] = 20;
            policyColumnsWidths[3] = 20;
            List<Broker.DataAccess.PaymentType> lstPaymentTypes = Broker.DataAccess.PaymentType.Table.ToList();
            foreach (InsuranceCompany ic in icList)
            {
                int tot = 0;
                decimal total = 0;
                pdf.SetTitleLeft(ic.Name.ToUpper());
                pdf.CreateTableWithBorder(headers.Count(), false, headers, "", policyColumnsWidths);
                int counter = 1;
                foreach (Broker.DataAccess.PaymentType pt in lstPaymentTypes)
                {
                    int count = 0;
                    decimal sum = Broker.DataAccess.Payment.GetSumByPaymentTypeAndInsuranceCompanyInPeriod(fromDate, toDate, ic.ID, pt, out count);
                    object[] vals = new object[headers.Count()];
                    vals[0] = counter;
                    vals[1] = pt.Name;
                    vals[2] = count;
                    vals[3] = String.Format("{0:#,0.00}", sum);
                    tot += count;
                    total += sum;
                    pdf.AddDataRowWithBorder(vals, headers.Count(), codes);
                    counter++;
                }

                object[] valsTot = new object[headers.Count()];
                valsTot[0] = "";
                valsTot[1] = "ВКУПНО:";
                valsTot[2] = tot;
                valsTot[3] = String.Format("{0:#,0.00}", total);
                pdf.AddDataRowWithBorder(valsTot, headers.Count(), codes);

                pdf.AddTable();
                pdf.SetTitleLeft(" ");
            }

            pdf.FinishPDF();
        }
    }
}
