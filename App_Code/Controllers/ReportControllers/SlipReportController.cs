using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
/// <summary>
/// Summary description for SlipReportController
/// </summary>
namespace Broker.Controllers.ReportControllers {
public class SlipReportController {

    public static void CreateFileForPeriodAndBank(int bankID, DateTime startDate, DateTime endDate) {
        DataClassesDataContext dcdc = new DataClassesDataContext();
        DateTime dt1 = startDate;
        DateTime dt2 = endDate;
        int d1 = dt1.Day;
        int m1 = dt1.Month;
        int y1 = dt1.Year;
        int d2 = dt2.Day;
        int m2 = dt2.Month;
        int y2 = dt2.Year;

        PDFCreators pdf = new PDFCreators(false, 25, 25, 25, 25);
        pdf.OpenPDF();
        pdf.SetTitle("  ");
        pdf.SetTitle("  ");

        pdf.SetTitle("Слипови во период  " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString());

        List<Slip> slipList = Slip.GetForPeriodAndBank(startDate, endDate, bankID);
        slipList = slipList.OrderBy(c => c.Date).ThenBy(c => c.SlipNumber).ToList();
        decimal totalValue = 0;
        decimal partialValue = 0;
        decimal dayTotalValue = 0;
        decimal dayPartialValue = 0;
        int orderNumber = 0;
        bool differentDays = false;
        // int dayOrderNumber = 0;
       // int monthOrderNumber = 0;
        string[] headersP = { "РБ", "Број на слип","Број на полиса", "Вредност", "Провизија", "Вредност без провизија", "Дата" };
        float[] policyColumnsWidths = { 5, 20,20, 15, 10, 15, 15 };
        TypeCode[] codes = { TypeCode.String, TypeCode.String,TypeCode.String, TypeCode.Decimal, TypeCode.Decimal, TypeCode.Decimal, TypeCode.DateTime };
        pdf.CreateTable(7, false, headersP, "", policyColumnsWidths);
        DateTime currentDate;
        if (slipList.Count > 0) {
            currentDate = slipList[0].Date;
        } else {
            currentDate = DateTime.Today;
        }
        foreach (Slip s in slipList) {
            if (s.Date != currentDate) {
                differentDays = true;
                    object[] vals = new object[7];
                    vals[0] = "";
                    vals[1] = "Вкупно";
                    vals[2] = "";
                    vals[3] = String.Format("{0:#,0.00}", dayTotalValue);
                    vals[4] = "";
                    vals[5] = String.Format("{0:#,0.00}", dayPartialValue);
                    vals[6] = "";
                    pdf.AddDataRow1(vals, 7, codes);
                    dayPartialValue = 0;
                    dayTotalValue = 0;
                    currentDate = s.Date;
                    orderNumber = 0;
                    totalValue += s.TotalValue;
                    partialValue += s.PartialValue;
                
            } else {
                orderNumber++;
                object[] vals2 = new object[7];
                vals2[0] = orderNumber;
                vals2[1] = s.SlipNumber;
                vals2[2] = s.PolicyNumber;
                vals2[3] = String.Format("{0:#,0.00}", s.TotalValue);
                vals2[4] = String.Format("{0:#,0.00}", s.ProvisionRate.Provision);
                vals2[5] = String.Format("{0:#,0.00}", s.PartialValue);
                vals2[6] = s.Date.ToShortDateString();
                pdf.AddDataRow1(vals2,7 , codes);
                totalValue += s.TotalValue;
                partialValue += s.PartialValue;
                dayTotalValue += s.TotalValue;
                dayPartialValue += s.PartialValue;
               
            }
        }
        //if (differentDays) {
        //    object[] vals5 = new object[7];
        //    vals5[0] = "";
        //    vals5[1] = "Вкупно";
        //    vals5[2] = "";
        //    vals5[3] = String.Format("{0:#,0.00}", dayTotalValue);
        //    vals5[4] = "";
        //    vals5[5] = String.Format("{0:#,0.00}", dayPartialValue);
        //    vals5[6] = "";
        //    pdf.AddDataRow1(vals5, 7, codes);

        //}

        object[] vals1 = new object[7];
        vals1[0] = "";
        vals1[1] = "Вкупно";
        vals1[2] = "";
        vals1[3] = String.Format("{0:#,0.00}", totalValue);
        vals1[4] = "";
        vals1[5] = String.Format("{0:#,0.00}", partialValue);
        vals1[6] = "";
        pdf.AddDataRow1(vals1, 7, codes);

        pdf.AddTable();
        pdf.FinishPDF();

    }
    }
}
