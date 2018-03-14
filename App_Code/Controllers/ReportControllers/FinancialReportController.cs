using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for FinancialReportController
/// </summary>
namespace Broker.Controllers.ReportControllers {
    public class FinancialReportController {
        public static void CreateFileForPeriod(DateTime startDate, DateTime endDate, string reportType) {

            DataClassesDataContext dcdc = new DataClassesDataContext();
            PDFCreators pdf = new PDFCreators(true, 25, 25, 25, 25);
            pdf.OpenPDF();
            pdf.SetTitle("  ");
            pdf.SetTitle("  ");
            pdf.SetTitle("Финансиски извештај за извршена наплата");
            pdf.SetTitle(" за период  " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());

            string[] headersP = { "РБ", "Број на полиса", "Тип на плаќање", "Корисник", "Вредност", };
            float[] policyColumnsWidths = { 5, 35, 22, 14, 18 };
            TypeCode[] codes = { TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
            pdf.CreateTable(5, false, headersP, "", policyColumnsWidths);
            List<Payment> lstPayments = Payment.Table.Where(c => c.Date.Date >= startDate.Date && c.Date.Date <= endDate.Date && c.Rate.PolicyItem.Policy.Discard == false).ToList();
            decimal totalValue = 0;
            int counter = 0;

            if (reportType == "PolicyNumber") {
                lstPayments = lstPayments.OrderBy(c => c.Rate.PolicyItem.PolicyNumber).ToList();
            } else {
                lstPayments = lstPayments.OrderBy(c => c.PaymentTypeID).ToList();
            }
            Dictionary<int, decimal> dic = new Dictionary<int, decimal>();
            foreach (Payment pay in lstPayments) {
                counter++;
                object[] vals = new object[6];
                vals[0] = counter;
                vals[1] = pay.Rate.PolicyItem.PolicyNumber;
                vals[2] = pay.PaymentType.Name;
                vals[3] = pay.User.UserName;
                vals[4] = String.Format("{0:#,0.00}", pay.Value);
                totalValue += pay.Value;
                if (!dic.Keys.Contains(pay.PaymentTypeID)) {
                    dic.Add(pay.PaymentTypeID, pay.Value);
                } else {
                    dic[pay.PaymentTypeID] += pay.Value;
                }
                pdf.AddDataRow1(vals, 5, codes);
            }


            object[] valsTotal = new object[6];
            valsTotal[0] = "";
            valsTotal[1] = "";
            valsTotal[2] = "";
            valsTotal[3] = "Вкупно";
            valsTotal[4] = String.Format("{0:#,0.00}", totalValue);
            pdf.AddDataRow1(valsTotal, 5, codes);
            pdf.AddTable();

            pdf.NewPage();
            pdf.SetTitle("Рекапитулација:");
            string[] headersTotalP = new string[3];
            headersTotalP[0] = "РБ";
            headersTotalP[1] = "Тип на плаќање";
            headersTotalP[2] = "Сума";
            float[] policyColumnsTotalWidths = new float[3];
            policyColumnsTotalWidths[0] = 5;
            policyColumnsTotalWidths[1] = 50;
            policyColumnsTotalWidths[2] = 40;
            TypeCode[] codesTotal = new TypeCode[3];
            codesTotal[0] = TypeCode.String;
            codesTotal[1] = TypeCode.String;
            codesTotal[2] = TypeCode.Decimal;


            pdf.CreateTable(3, false, headersTotalP, "", policyColumnsTotalWidths);
            int recapCounter = 1;
            foreach (KeyValuePair<int, decimal> kvp in dic) {
                object[] valsRecap = new object[3];
                valsRecap[0] = recapCounter;
                valsRecap[1] = Broker.DataAccess.PaymentType.Get(kvp.Key).Name;
                valsRecap[2] = String.Format("{0:#,0.00}", kvp.Value);
                pdf.AddDataRow1(valsRecap, 3, codesTotal);
                recapCounter++;
            }

            pdf.AddTable();
            pdf.FinishPDF();
        }
    }
}

