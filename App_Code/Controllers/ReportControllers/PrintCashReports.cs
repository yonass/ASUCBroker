using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for PrintCashReports
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintCashReports {
        public static void PrintCashReport(CashReport cr) {

            PDFCreators creator = new PDFCreators(true, 15, 15, 15, 15);
            creator.OpenPDF();
            creator.GetContentByte();
            creator.AddJDBLogoForFactures(10, 775);
            creator.SetTitleLeft10(" ");
            creator.SetTitleLeft10(" ");
            creator.SetTitleLeft10(" ");
            if (cr.Discard) {
                creator.SetTitleCenterForFactureNumber("СТОРНИРАН КАСОВ ИЗВЕШТАЈ бр. " + cr.Number);
            } else {
                creator.SetTitleCenterForFactureNumber("КАСОВ ИЗВЕШТАЈ бр. " + cr.Number);
            }
            creator.SetTitleLeft10("Статус: " + cr.CashReportStatuse.Name);
            creator.SetTitleLeft10("Датум: " + cr.CashReportDate.ToShortDateString());
            creator.SetTitleLeft10("Филијала: " + cr.Branch.Code + " " + cr.Branch.Name);
            List<CashReportItem> lstCRI = CashReportItem.GetByCashReport(cr.ID);

            creator.SetTitleLeft10(" ");
            creator.SetTitleLeft10("Кеш уплата");
            Broker.DataAccess.PaymentType ptCash = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.CASH);
            List<CashReportItem> cashList = lstCRI.Where(c => c.Payment.PaymentTypeID == ptCash.ID).ToList();
            string[] headers = { "Ред. бр.", "Број на полиса", "Компанија", "Подкласа", "Договорувач", "Уплата" };
            float[] widthPercentages = { 8, 10, 17, 18, 32, 15 };
            TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
            creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
            object[] values;
            int ordinalNumber = 1;
            decimal totCash = 0;
            foreach (CashReportItem cri in cashList) {
                values = new object[headers.Length];
                values[0] = ordinalNumber.ToString();
                values[1] = cri.Payment.Rate.PolicyItem.PolicyNumber;
                values[2] = cri.Payment.Rate.PolicyItem.Policy.InsuranceCompany.ShortName;
                values[3] = cri.Payment.Rate.PolicyItem.InsuranceSubType.ShortDescription;
                values[4] = cri.Payment.Rate.PolicyItem.Policy.Client.Name;
                values[5] = String.Format("{0:#,0.00}", cri.Payment.Value);
                creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                ordinalNumber++;
                totCash += cri.Payment.Value;
            }
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            values[2] = "";
            values[3] = "";
            values[4] = "Вкупно";
            values[5] = String.Format("{0:#,0.00}", totCash);
            creator.AddDataRowForFactures(values, headers.Length, typeCodes);
            creator.AddTable();

            creator.SetTitleLeft10(" ");
            creator.SetTitleLeft10("Кредитни картички");
            Broker.DataAccess.PaymentType ptCreditCard = Broker.DataAccess.PaymentType.GetByCode(Broker.DataAccess.PaymentType.CREDITCARD);
            List<CashReportItem> creditCardList = lstCRI.Where(c => c.Payment.PaymentTypeID == ptCreditCard.ID).ToList();
            string[] headersCC = { "Ред. бр.", "Број на полиса", "Компанија", "Подкласа", "Договорувач", "Банка", "Уплата" };
            float[] widthPercentagesCC = { 7, 9, 16, 17, 27, 10, 14 };
            TypeCode[] typeCodesCC = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
            creator.CreateTable_Facture(headersCC.Length, headersCC, widthPercentagesCC);
            object[] valuesCC;
            int ordinalNumberCC = 1;
            decimal totCreditCard = 0;
            foreach (CashReportItem cri in creditCardList) {
                valuesCC = new object[headersCC.Length];
                valuesCC[0] = ordinalNumberCC.ToString();
                valuesCC[1] = cri.Payment.Rate.PolicyItem.PolicyNumber;
                valuesCC[2] = cri.Payment.Rate.PolicyItem.Policy.InsuranceCompany.ShortName;
                valuesCC[3] = cri.Payment.Rate.PolicyItem.InsuranceSubType.ShortDescription;
                valuesCC[4] = cri.Payment.Rate.PolicyItem.Policy.Client.Name;
                if(cri.Payment.BankCreditCardID!=null){
                    valuesCC[5] = cri.Payment.BankCreditCard.Bank.Name;
                } else{
                    valuesCC[5] = "";
                }
                valuesCC[6] = String.Format("{0:#,0.00}", cri.Payment.Value);
                creator.AddDataRowForFactures(valuesCC, headersCC.Length, typeCodesCC);
                ordinalNumberCC++;
                totCreditCard += cri.Payment.Value;
            }
            valuesCC = new object[headersCC.Length];
            valuesCC[0] = "";
            valuesCC[1] = "";
            valuesCC[2] = "";
            valuesCC[3] = "";
            valuesCC[4] = "";
            valuesCC[5] = "Вкупно";
            valuesCC[6] = String.Format("{0:#,0.00}", totCreditCard);
            creator.AddDataRowForFactures(valuesCC, headersCC.Length, typeCodesCC);
            creator.AddTable();
            creator.SetTitleLeft10(" ");
            creator.SetTitleRightBold14("Вкупно: " + String.Format("{0:#,0.00}", (totCreditCard+totCash)));
            creator.FinishPDF_FileName("KasovIzvestaj" + cr.ID);
        }
    }
}
