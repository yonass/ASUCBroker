using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for LifePolicyReportController
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class LifePolicyReportController {
        public static void PrintLifePolicyReportController(DateTime fromDate, DateTime toDate) {
            PDFCreators creator = new PDFCreators(true, 10, 10, 15, 15);
            creator.SetDocumentHeaderFooter();
            creator.OpenPDF();
            creator.GetContentByte();
            creator.SetTitleLeftWithFontSize10("ИЗВЕШТАЈ ЗА ЖИВОТНО ОСИГУРУВАЊЕ");

            List<LifePolicy> lstLifePolicies = LifePolicy.GetUndiscardedInPeriod(fromDate, toDate);
            //GetNumberOfBrokerageYears
            int numberOfYearBrok = 4;
            foreach (LifePolicy lp in lstLifePolicies) {
                List<LifePolicyBrokerage> lstLPB = LifePolicyBrokerage.GetByLifePolicy(lp.ID);
                if (lstLPB.Count > numberOfYearBrok) {
                    numberOfYearBrok = lstLPB.Count;
                }
            }
            int totHeadersCount = 9 + numberOfYearBrok;
            string[] headers = new string[totHeadersCount];
            headers[0] = "Р.б.";
            headers[1] = "Полиса бр.";
            headers[2] = "Понуда бр.";
            headers[3] = "Датум на продажба";
            headers[4] = "Годишна премија (живот)";
            headers[5] = "Годишна премија (незгода)";
            headers[6] = "Годишна премија";
            headers[7] = "Вкупна премија";
            for (int i = 0; i < numberOfYearBrok; i++) {
                headers[8 + i] = "Брокеража за " + (i + 1) + " година";
            }
            headers[totHeadersCount - 1] = "Вкупна брокеража";

            float[] widthPercentages = new float[totHeadersCount];
            widthPercentages[0] = 4;
            widthPercentages[1] = 8;
            widthPercentages[2] = 6;
            widthPercentages[3] = 10;
            widthPercentages[4] = 7;
            widthPercentages[5] = 7;
            widthPercentages[6] = 7;
            widthPercentages[7] = 7;
            float perc = (float)36 / (float)numberOfYearBrok;
            for (int i = 0; i < numberOfYearBrok; i++) {
                widthPercentages[8 + i] = perc;
            }
            widthPercentages[totHeadersCount - 1] = 8;
            TypeCode[] typeCodes = new TypeCode[totHeadersCount];
            typeCodes[0] = TypeCode.Int32;
            typeCodes[1] = TypeCode.String;
            typeCodes[2] = TypeCode.String;
            typeCodes[3] = TypeCode.DateTime;
            typeCodes[4] = TypeCode.Decimal;
            typeCodes[5] = TypeCode.Decimal;
            typeCodes[6] = TypeCode.Decimal;
            typeCodes[7] = TypeCode.Decimal;
            for (int i = 0; i < numberOfYearBrok; i++) {
                typeCodes[8 + i] = TypeCode.Decimal;
            }
            typeCodes[totHeadersCount - 1] = TypeCode.Decimal;
            creator.CreateTableWithFontSize(headers.Length, headers, widthPercentages, 8);
            object[] values;
            int counter = 1;
            decimal totalYearlyPremiumValueForLife = 0;
            decimal totalYearlyPremiumValueForAccident = 0;
            decimal totalYearlyPremiumValue = 0;
            decimal totalTotalPremumValue = 0;
            decimal totalBrokerage = 0;
            Dictionary<int, decimal> totalBrokValuesPerYear = new Dictionary<int, decimal>();
            foreach (LifePolicy lp in lstLifePolicies) {
                values = new object[headers.Length];
                values[0] = counter;
                values[1] = lp.PolicyNumber;
                values[2] = lp.OfferNumber;
                values[3] = lp.ApplicationDate.ToShortDateString();
                values[4] = String.Format("{0:#,0.00}", lp.YearlyPremiumValueForLife);
                totalYearlyPremiumValueForLife += lp.YearlyPremiumValueForLife;
                values[5] = String.Format("{0:#,0.00}", lp.YearlyPremiumValueForAccident);
                totalYearlyPremiumValueForAccident += lp.YearlyPremiumValueForAccident;
                values[6] = String.Format("{0:#,0.00}", (lp.YearlyPremiumValueForAccident + lp.YearlyPremiumValueForLife));
                totalYearlyPremiumValue += (lp.YearlyPremiumValueForAccident + lp.YearlyPremiumValueForLife);
                values[7] = String.Format("{0:#,0.00}", lp.TotalPremumValue);
                totalTotalPremumValue += lp.TotalPremumValue;
                List<LifePolicyBrokerage> lstLPBrok = LifePolicyBrokerage.GetByLifePolicy(lp.ID);
                decimal totalBrokeragePerPolicy = 0;
                for (int j = 0; j < lstLPBrok.Count; j++) {
                    values[8 + j] = String.Format("{0:#,0.00}", lstLPBrok[j].Value);
                    totalBrokeragePerPolicy += lstLPBrok[j].Value;
                    if (!totalBrokValuesPerYear.Keys.Contains(j)) {
                        totalBrokValuesPerYear.Add(j, lstLPBrok[j].Value);
                    } else {
                        totalBrokValuesPerYear[j] += lstLPBrok[j].Value;
                    }
                }
                values[totHeadersCount - 1] = String.Format("{0:#,0.00}", totalBrokeragePerPolicy);
                totalBrokerage += totalBrokeragePerPolicy;
                creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                counter++;
            }
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            values[2] = "";
            values[3] = "Вкупно";
            values[4] = String.Format("{0:#,0.00}", totalYearlyPremiumValueForLife);
            values[5] = String.Format("{0:#,0.00}", totalYearlyPremiumValueForAccident);
            values[6] = String.Format("{0:#,0.00}", totalYearlyPremiumValue);
            values[7] = String.Format("{0:#,0.00}", totalTotalPremumValue);
            int k = 0;
            foreach (KeyValuePair<int, decimal> kvp in totalBrokValuesPerYear) {
                values[8 + k] = String.Format("{0:#,0.00}", kvp.Value);
                k++;
            }
            values[totHeadersCount - 1] = String.Format("{0:#,0.00}", totalBrokerage);
            creator.AddDataRowForFactures(values, headers.Length, typeCodes);
            creator.AddTable();
            creator.FinishPDF_FileName("IzvestajZaZivotnoOsiguruvanje");
        }
    }
}
