using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using ASUC.Controllers.ConvertController;

/// <summary>
/// Summary description for PrintBillInfo
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintBillInfo {
        public static void PrintBillInfoForPolicy(PolicyItem pi) {
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
            creator.SetTitle("ПОТВРДА");
            creator.SetTitle("(за продадена полиса)");
            creator.SetTitle("Полиса бр. " + pi.PolicyNumber);
            creator.SetTitleLeft8("  ");
            creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(pi.Policy.Client.Name).ToUpper());
            creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(pi.Policy.Client.Address).ToUpper());
            creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(pi.Policy.Client.Place.Municipality.Name).ToUpper());
            creator.SetTitleLeft10("Датум на полиса: " + pi.Policy.ApplicationDate.ToShortDateString());
            string nacinNaPlakanje = "";

            if (pi.IsPaidInBrokerHouse) {
                nacinNaPlakanje = "ЕДНОКРАТНО";
                List<Rate> lstRates = Rate.GetByPolicyItemID(pi.ID);
                if (lstRates.Count > 1) {
                    nacinNaPlakanje = "НА РАТИ";
                }
            } else {
                nacinNaPlakanje = "ДИРЕКТНО ВО ОСИГУРИТЕЛНА КОМПАНИЈА";
            }
            creator.SetTitleLeft10("Начин на плаќање на полиса: " + nacinNaPlakanje);

            string[] headers = { "Ред. бр.", "Број на полиса", "Осиг. компанија", "Тип на осигурување", "Премија" };
            float[] widthPercentages = { 8, 17, 30, 30, 15 };
            TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
            creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
            object[] values;
            values = new object[headers.Length];
            values[0] = "1";
            values[1] = pi.PolicyNumber;
            values[2] = pi.Policy.InsuranceCompany.ShortName;
            values[3] = pi.InsuranceSubType.ShortDescription;
            values[4] = String.Format("{0:#,0.00}", pi.PremiumValue) + " ДЕН.";
            creator.AddDataRowForFactures(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            values[2] = "";
            values[3] = "Вкупно";
            values[4] = String.Format("{0:#,0.00}", pi.PremiumValue) + " ДЕН.";
            creator.AddDataRowForFactures(values, headers.Length, typeCodes);
            creator.AddTable();
            creator.SetTitleLeftBold14(" ");
            creator.SetTitleLeftItalic10("Со букви: " + NumberToTextController.Konvertiranje(pi.PremiumValue));
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft10("Согласно со член 23, точка 6 од Законот за ДДВ дејноста осигурување е ослободена од плаќање данок без право на одбиток на претходниот данок.");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("         " + brokerName + "                                                                                                                           ДОГОВОРУВАЧ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("_________________________                                                                                                           ____________________");
            creator.FinishPDF_FileName("Potvrda" + pi.PolicyNumber);
        }
    }
}
