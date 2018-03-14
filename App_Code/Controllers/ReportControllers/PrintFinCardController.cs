using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using ASUC.Controllers.ConvertController;
using Broker.Controllers.FinanceControllers;

/// <summary>
/// Summary description for PrintFinCardController
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintFinCardController {
        public static void PrintFinCard(Policy p, Broker.DataAccess.Client c, string itemsType)
        {
            string brokerName = BrokerHouseInformation.GetBrokerHouseName();
            PDFCreators creator = new PDFCreators(true, 25, 25, 15, 15);
            creator.SetDocumentHeaderFooter();
            creator.OpenPDF();
            creator.GetContentByte();
            creator.AddJDBLogoForFactures(10, 775);
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("   Жиро-сметка :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZIRO_SMETKA).Value);
            creator.SetTitleLeft8("   Депонент :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.DEPONENT).Value);
            creator.SetTitleLeft8("   ЕДБ :   " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.EDB).Value + "       " + "Матичен број :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.MATICEN_BROJ).Value);
            creator.SetTitleLeft8("  ");
            creator.SetTitleCenterForFactureNumber("ФИНАНСОВА КАРТИЦА");
            List<FinanceCardInfo> lst = new List<FinanceCardInfo>();
            if (p != null) {
                creator.SetTitleLeftWithFontSize10("за полиса " + p.PolicyItems[0].PolicyNumber);
                lst = FinanceControllers.FinanceCardController.GetByPolicy(p, 0);
            } else if (c != null) {
                creator.SetTitleLeftWithFontSize10("за договорувач: ");
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(c.Name).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(c.Address).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(c.Place.Municipality.Name).ToUpper());
                if (itemsType == "AllItems") {
                    lst = FinanceControllers.FinanceCardController.GetByClient(c);
                } else if(itemsType == "OpenItems") {
                    lst = FinanceControllers.FinanceCardController.GetByClientOpenItems(c);
                }
            }

            creator.SetTitleLeft10("Датум на печатење: " + DateTime.Today.ToShortDateString());
            //decimal totalSaldo = 0;
            decimal totalDebtValue = 0;
            decimal totalDemandValue = 0;
            string[] headers = { "Дата", "Доспева", "Опис", "Должи", "Побарува", "Салдо" };
            float[] widthPercentages = { 10, 10, 35, 15, 15, 15 };
            TypeCode[] typeCodes = { TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal, TypeCode.Decimal, TypeCode.Decimal };
            creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
            foreach (FinanceCardInfo fci in lst) {
                object[] values;
                values = new object[headers.Length];
                values[0] = fci.DocumentDate.ToShortDateString();
                values[1] = fci.PaidDate.ToShortDateString();
                values[2] = fci.Description;
                values[3] = String.Format("{0:#,0.00}", fci.DebtValue);
                values[4] = String.Format("{0:#,0.00}", fci.DemandValue);
                values[5] = String.Format("{0:#,0.00}", fci.SaldoValue);
                totalDebtValue += fci.DebtValue;
                totalDemandValue += fci.DemandValue;
                //totalSaldo += fci.SaldoValue;
                creator.AddDataRowForFactures(values, headers.Length, typeCodes);
            }
            object[] valuesTot;
            valuesTot = new object[headers.Length];
            valuesTot[0] = "";
            valuesTot[1] = "";
            valuesTot[2] = "";
            valuesTot[3] = String.Format("{0:#,0.00}", totalDebtValue);
            valuesTot[4] = String.Format("{0:#,0.00}", totalDemandValue);
            valuesTot[5] = String.Format("{0:#,0.00}", totalDebtValue - totalDemandValue);
            creator.AddDataRowForFactures(valuesTot, headers.Length, typeCodes);
            creator.AddTable();
            creator.FinishPDF_FileName("FinCard");
        }

        public static void PrintFinCardByApplicationDateReport(DateTime fromDate, DateTime toDate, List<int> lstBranhces,
            List<int> lstInsuranceCompanies, List<int> lstInsuranceSubTypes, List<int> lstUsers, List<int> lstMarketingAgents) {
            string brokerName = BrokerHouseInformation.GetBrokerHouseName();
            PDFCreators creator = new PDFCreators(true, 25, 25, 15, 15);
            creator.SetDocumentHeaderFooter();
            creator.OpenPDF();
            creator.GetContentByte();
            creator.AddJDBLogoForFactures(10, 775);
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitle("ФИНАНСОВА КАРТИЦА");
            creator.SetTitle("по датум на издавање на полиси");
            creator.SetTitle("за период " + fromDate.ToShortDateString() + " - " + toDate.ToShortDateString());
            creator.SetTitleLeft10("Датум на печатење: " + DateTime.Today.ToShortDateString());

            decimal overallSaldo = 0;
            decimal overallDebt = 0;
            decimal overallDemand = 0;

            DateTime dt1 = fromDate;
            DateTime dt2 = toDate;
            int d1 = dt1.Day;
            int m1 = dt1.Month;
            int y1 = dt1.Year;
            int d2 = dt2.Day;
            int m2 = dt2.Month;
            int y2 = dt2.Year;

            //List<Policy> lstPol = Policy.Table.Where(c => c.Discard == false && c.ApplicationDate.Date >= fromDate.Date && c.ApplicationDate.Date <= toDate.Date).ToList();
            DataClassesDataContext dc = new DataClassesDataContext();
            string query = @"SELECT p.* from policies p, policyitems pi where pi.policyid=p.id" +
                            @" and p.branchid in (" + ReportFinPolicies.GetFromIDs(lstBranhces) + ") " +
                            @" and p.userid in (" + ReportFinPolicies.GetFromIDs(lstUsers) + ") " +
                            @" and p.insurancecompanyid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceCompanies) + ") " +
                            @" and pi.insurancesubtypeid in (" + ReportFinPolicies.GetFromIDs(lstInsuranceSubTypes) + ") " +
                            @" and p.applicationdate>='" + y1 + -+m1 + -+d1 + "'" +
                            @" and p.applicationdate<='" + y2 + -+m2 + -+d2 + "'" +
                            @" and p.discard = 0 ";

            List<Policy> lstPol = dc.ExecuteQuery<Policy>(query).ToList();
            foreach (Policy p in lstPol) {
                creator.SetTitleLeft10("Број на полиса: " + p.PolicyItems[0].PolicyNumber);
                creator.SetTitleLeft10("Осигурителна компанија: " + p.InsuranceCompany.ShortName);
                creator.SetTitleLeft10("Подкласа на осигурување: " + p.PolicyItems[0].InsuranceSubType.ShortDescription);
                creator.SetTitleLeft10("Датум на полиса: " + p.ApplicationDate.ToShortDateString());
                //decimal totalSaldo = 0;
                decimal totalDebtValue = 0;
                decimal totalDemandValue = 0;
                List<FinanceCardInfo> lst = new List<FinanceCardInfo>();
                lst = FinanceCardController.GetByPolicy(p, 0);
                string[] headers = { "Дата", "Доспева", "Опис", "Должи", "Побарува", "Салдо" };
                float[] widthPercentages = { 10, 10, 35, 15, 15, 15 };
                TypeCode[] typeCodes = { TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal, TypeCode.Decimal, TypeCode.Decimal };
                creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
                foreach (FinanceCardInfo fci in lst) {
                    object[] values;
                    values = new object[headers.Length];
                    values[0] = fci.DocumentDate.ToShortDateString();
                    values[1] = fci.PaidDate.ToShortDateString();
                    values[2] = fci.Description;
                    values[3] = String.Format("{0:#,0.00}", fci.DebtValue);
                    values[4] = String.Format("{0:#,0.00}", fci.DemandValue);
                    values[5] = String.Format("{0:#,0.00}", fci.SaldoValue);
                    totalDebtValue += fci.DebtValue;
                    totalDemandValue += fci.DemandValue;
                    //totalSaldo += fci.SaldoValue;
                    creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                }
                object[] valuesTot;
                valuesTot = new object[headers.Length];
                valuesTot[0] = "";
                valuesTot[1] = "";
                valuesTot[2] = "";
                valuesTot[3] = String.Format("{0:#,0.00}", totalDebtValue);
                valuesTot[4] = String.Format("{0:#,0.00}", totalDemandValue);
                valuesTot[5] = String.Format("{0:#,0.00}", totalDebtValue - totalDemandValue);
                overallSaldo += (totalDebtValue - totalDemandValue);
                overallDemand += totalDemandValue;
                overallDebt += totalDebtValue;
                creator.AddDataRowForFactures(valuesTot, headers.Length, typeCodes);
                creator.AddTable();
            }
            creator.SetTitleRight("Должи: " + String.Format("{0:#,0.00}", overallDebt) + "  Побарува: " + String.Format("{0:#,0.00}", overallDemand) + "  Салдо: " + String.Format("{0:#,0.00}", overallSaldo));
            creator.FinishPDF_FileName("FinCard");
        }

        public static void PrintFinCardByPaidDates(DateTime fromDate, DateTime toDate, List<int> lstBranhces,
            List<int> lstInsuranceCompanies, List<int> lstInsuranceSubTypes, List<int> lstUsers, List<int> lstMarketingAgents) {
            string brokerName = BrokerHouseInformation.GetBrokerHouseName();
            PDFCreators creator = new PDFCreators(true, 25, 25, 15, 15);
            creator.SetDocumentHeaderFooter();
            creator.OpenPDF();
            creator.GetContentByte();
            creator.AddJDBLogoForFactures(10, 775);
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitle("ФИНАНСОВА КАРТИЦА");
            creator.SetTitle("по датум на доспевање и уплата");
            creator.SetTitle("за период " + fromDate.ToShortDateString() + " - " + toDate.ToShortDateString());
            creator.SetTitleLeft10("Датум на печатење: " + DateTime.Today.ToShortDateString());

            decimal overallSaldo = 0;
            decimal overallDebt = 0;
            decimal overallDemand = 0;

            List<FinanceCardInfo> lstFCIByRates = FinanceCardController.GetByRateInPeriod(fromDate, toDate, lstBranhces, lstInsuranceCompanies,
                lstInsuranceSubTypes, lstUsers, lstMarketingAgents);
            List<FinanceCardInfo> lstFCIByPayments = FinanceCardController.GetByPaymentInPeriod(fromDate, toDate, lstBranhces, lstInsuranceCompanies,
                lstInsuranceSubTypes, lstUsers, lstMarketingAgents);
            HashSet<int> lstPolicyIDs = new HashSet<int>();
            foreach (FinanceCardInfo fciR in lstFCIByRates) {
                if (!lstPolicyIDs.Contains(fciR.PolicyItemID)) {
                    lstPolicyIDs.Add(fciR.PolicyItemID);
                }
            }
            foreach (FinanceCardInfo fciP in lstFCIByPayments) {
                if (!lstPolicyIDs.Contains(fciP.PolicyItemID)) {
                    lstPolicyIDs.Add(fciP.PolicyItemID);
                }
            }

            foreach (int piID in lstPolicyIDs) {
                PolicyItem pi = PolicyItem.Get(piID);
                creator.SetTitleLeft10("Број на полиса: " + pi.Policy.PolicyItems[0].PolicyNumber);
                creator.SetTitleLeft10("Осигурителна компанија: " + pi.Policy.InsuranceCompany.ShortName);
                creator.SetTitleLeft10("Подкласа на осигурување: " + pi.Policy.PolicyItems[0].InsuranceSubType.ShortDescription);
                creator.SetTitleLeft10("Датум на полиса: " + pi.Policy.ApplicationDate.ToShortDateString());
                //decimal totalSaldo = 0;
                decimal totalDebtValue = 0;
                decimal totalDemandValue = 0;
                List<FinanceCardInfo> lst = new List<FinanceCardInfo>();
                List<FinanceCardInfo> lstRat = lstFCIByRates.Where(c => c.PolicyItemID == piID).ToList();
                List<FinanceCardInfo> lstPay = lstFCIByPayments.Where(c => c.PolicyItemID == piID).ToList();
                if (lstRat != null) {
                    if (lstRat.Count > 0) {
                        lst.AddRange(lstRat);
                    }
                }
                if (lstPay != null) {
                    if (lstPay.Count > 0) {
                        lst.AddRange(lstPay);
                    }
                }
                for (int i = 0; i < lst.Count; i++) {
                    if (i == 0) {
                        lst[i].SaldoValue = lst[i].DebtValue - lst[i].DemandValue;
                    }
                    if (i > 0) {
                        lst[i].SaldoValue = lst[i - 1].SaldoValue + (lst[i].DebtValue - lst[i].DemandValue);
                    }
                }
                string[] headers = { "Дата", "Доспева", "Опис", "Должи", "Побарува", "Салдо" };
                float[] widthPercentages = { 10, 10, 35, 15, 15, 15 };
                TypeCode[] typeCodes = { TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal, TypeCode.Decimal, TypeCode.Decimal };
                creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
                foreach (FinanceCardInfo fci in lst) {
                    object[] values;
                    values = new object[headers.Length];
                    values[0] = fci.DocumentDate.ToShortDateString();
                    values[1] = fci.PaidDate.ToShortDateString();
                    values[2] = fci.Description;
                    values[3] = String.Format("{0:#,0.00}", fci.DebtValue);
                    values[4] = String.Format("{0:#,0.00}", fci.DemandValue);
                    values[5] = String.Format("{0:#,0.00}", fci.SaldoValue);
                    totalDebtValue += fci.DebtValue;
                    totalDemandValue += fci.DemandValue;
                    //totalSaldo += fci.SaldoValue;
                    creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                }
                object[] valuesTot;
                valuesTot = new object[headers.Length];
                valuesTot[0] = "";
                valuesTot[1] = "";
                valuesTot[2] = "";
                valuesTot[3] = String.Format("{0:#,0.00}", totalDebtValue);
                valuesTot[4] = String.Format("{0:#,0.00}", totalDemandValue);
                valuesTot[5] = String.Format("{0:#,0.00}", totalDebtValue - totalDemandValue);
                overallSaldo += (totalDebtValue - totalDemandValue);
                overallDemand += totalDemandValue;
                overallDebt += totalDebtValue;
                creator.AddDataRowForFactures(valuesTot, headers.Length, typeCodes);
                creator.AddTable();
            }
            creator.SetTitleRight("Должи: " + String.Format("{0:#,0.00}", overallDebt) + "  Побарува: " + String.Format("{0:#,0.00}", overallDemand) + "  Салдо: " + String.Format("{0:#,0.00}", (overallDebt - overallDemand)));
            creator.FinishPDF_FileName("FinCard");
        }

    }
}