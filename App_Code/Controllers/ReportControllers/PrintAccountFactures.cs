using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using ASUC.Controllers.ConvertController;

/// <summary>
/// Summary description for PrintAccountFactures
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintAccountFactures {
        public static void PrintAccountFacture(AccountFacture f) {

            if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA) {
                PDFCreators creator = new PDFCreators(true, 15, 15, 15, 15);
                creator.OpenPDF();
                creator.GetContentByte();
                creator.AddJDBLogoForFactures(10, 760);
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeftBold14("  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.IME).Value);
                creator.SetTitle(" ");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft8("   Адреса :    " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ADRESA).Value);
                creator.SetTitleLeft8("   Место :     " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.MESTO).Value);
                creator.SetTitleLeft8("   Телефон :   " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.TELEFON).Value + "       " + "Факс :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.FAKS).Value);
                if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA) {
                    creator.SetTitleLeft8("   Жиро-сметка :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZIRO_SMETKA_ZA_OSIG_KOMPANII).Value);
                } else if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                    creator.SetTitleLeft8("   Жиро-сметка :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZIRO_SMETKA).Value);
                }
                creator.SetTitleLeft8("   Депонент :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.DEPONENT).Value);
                creator.SetTitleLeft8("   ЕДБ :   " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.EDB).Value + "       " + "Матичен број :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.MATICEN_BROJ).Value);
                creator.SetTitleLeft8("   Web-страна :    " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.WEB_STRANA).Value + "        " + "E-Mail :    " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.EMAIL).Value);
                creator.SetTitleLeft8("  ");
                if (f.Discard) {
                    creator.SetTitleCenterForFactureNumber("СТОРНИРАНА Ф-ра бр. " + f.FactureNumber);
                } else {
                    creator.SetTitleCenterForFactureNumber("Фактура бр. " + f.FactureNumber);
                }
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Name).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Address).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Place.Municipality.Name).ToUpper());
                creator.SetTitleLeft10("Датум на фактура: " + f.DateOfCreation.ToShortDateString());
                creator.SetTitleLeft10("Рок на плаќање: " + f.DateOfPayment.ToShortDateString());
                //creator.SetTitleLeft8("Период: " + f.FromDate.ToShortDateString() + " - " + f.ToDate.ToShortDateString()); 

                string[] headers = { "Ред. бр.", "Опис", "Количина", "Премија", "Брокеража" };
                float[] widthPercentages = { 8, 45, 15, 17, 15 };
                TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.Int32, TypeCode.Decimal, TypeCode.Decimal };
                creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
                object[] values;
                foreach (AccountFactureItem fi in f.AccountFactureItems) {
                    values = new object[headers.Length];
                    values[0] = fi.Number.ToString();
                    values[1] = fi.Description;
                    values[2] = fi.Count.ToString();
                    values[3] = String.Format("{0:#,0.00}", fi.PremiumValue);
                    values[4] = String.Format("{0:#,0.00}", fi.BrokerageValue);
                    creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                }
                values = new object[headers.Length];
                values[0] = "";
                values[1] = "";
                values[2] = "";
                values[3] = "Вкупно";
                values[4] = String.Format("{0:#,0.00}", f.BrokerageValue);
                creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                creator.AddTable();
                creator.SetTitleLeftBold14(" ");
                creator.SetTitleLeftItalic10("Со букви: " + NumberToTextController.Konvertiranje(f.BrokerageValue));
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft10("Ве молиме фактурираниот износ да го платите до назначениот рок на плаќање.");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft10("Доколку износот за фактурата не биде платен до наведениот датум, се пресметува затезна камата од денот на стасаност до денот на плаќањето.");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft10("Согласно со член 23, точка 6 од Законот за ДДВ дејноста осигурување е ослободена од плаќање данок без право на одбиток на претходниот данок.");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft10("                          Фактурирал                                              Примил                                                           Одобрил");
                creator.SetTitleLeft10("                    _________________                            __________________                                  ____________________");

                creator.AddPage();
                creator.SetTitleCenterForFactureNumber("Спецификација за фактура бр. " + f.FactureNumber);
                string[] headersSpecifications = { "Ред. бр.", "Број на полиса", "Почеток", "Истек", "Дата на издавање", "Премија" };
                float[] widthPercentagesSpecifications = { 10, 20, 19, 19, 19, 13 };
                TypeCode[] typeCodesSpecifications = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
                object[] valuesSpecifications;
                foreach (AccountFactureItem fi in f.AccountFactureItems) {
                    creator.SetTitleLeft10Bold(fi.InsuranceSubType.Description);
                    creator.CreateTable_Facture(headersSpecifications.Length, headersSpecifications, widthPercentagesSpecifications);
                    List<AccountFactureItemPolicyItem> pifiList = AccountFactureItemPolicyItem.GetByAccountFactureItem(fi.ID);
                    int ordinalNumber = 1;
                    foreach (AccountFactureItemPolicyItem pifi in pifiList) {
                        valuesSpecifications = new object[6];
                        valuesSpecifications[0] = ordinalNumber;
                        valuesSpecifications[1] = pifi.PolicyItem.PolicyNumber;
                        valuesSpecifications[2] = pifi.PolicyItem.Policy.StartDate.ToShortDateString();
                        valuesSpecifications[3] = pifi.PolicyItem.Policy.EndDate.ToShortDateString();
                        valuesSpecifications[4] = pifi.PolicyItem.Policy.ApplicationDate.ToShortDateString();
                        valuesSpecifications[5] = pifi.PolicyItem.PremiumValue;
                        creator.AddDataRowForFacturesExtend(valuesSpecifications, headersSpecifications.Length, typeCodesSpecifications, headersSpecifications, widthPercentagesSpecifications);
                        ordinalNumber++;
                    }
                    creator.AddTable();
                    creator.SetTitleCenterForFactureNumber(" ");
                }

                creator.FinishPDF_FileName("Factura" + f.FactureNumber);
            }
            //if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT)
            //{
            //    PDFCreators creator = new PDFCreators(true, 15, 15, 15, 15);
            //    creator.OpenPDF();
            //    creator.GetContentByte();
            //    creator.AddJDBLogoForFactures(10, 760);
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeftBold14("  " + Parameter.GetByCode(Parameter.IME).Value);
            //    creator.SetTitle(" ");
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft8("   Адреса :    " + Parameter.GetByCode(Parameter.ADRESA).Value);
            //    creator.SetTitleLeft8("   Место :     " + Parameter.GetByCode(Parameter.MESTO).Value);
            //    creator.SetTitleLeft8("   Телефон :   " + Parameter.GetByCode(Parameter.TELEFON).Value + "       " + "Факс :  " + Parameter.GetByCode(Parameter.FAKS).Value);
            //    creator.SetTitleLeft8("   Жиро-сметка :  " + Parameter.GetByCode(Parameter.ZIRO_SMETKA).Value);
            //    creator.SetTitleLeft8("   Депонент :  " + Parameter.GetByCode(Parameter.DEPONENT).Value);
            //    creator.SetTitleLeft8("   ЕДБ :   " + Parameter.GetByCode(Parameter.EDB).Value + "       " + "Матичен број :  " + Parameter.GetByCode(Parameter.MATICEN_BROJ).Value);
            //    creator.SetTitleLeft8("   Web-страна :    " + Parameter.GetByCode(Parameter.WEB_STRANA).Value + "        " + "E-Mail :    " + Parameter.GetByCode(Parameter.EMAIL).Value);
            //    creator.SetTitleLeft8("  ");
            //    creator.SetTitleCenterForFactureNumber("Фактура бр. " + f.FactureNumber);
            //    creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Name).ToUpper());
            //    creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Address).ToUpper());
            //    creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Place.Municipality.Name).ToUpper());
            //    creator.SetTitleLeft10("Датум на фактура: " + f.DateOfCreation.ToShortDateString());
            //    creator.SetTitleLeft10("Рок на плаќање: " + f.DateOfPayment.ToShortDateString());
            //    //creator.SetTitleLeft8("Период: " + f.FromDate.ToShortDateString() + " - " + f.ToDate.ToShortDateString()); 

            //    string[] headers = { "Ред. бр.", "Опис", "Количина", "Премија" };
            //    float[] widthPercentages = { 10, 50, 15, 25 };
            //    TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.Int32, TypeCode.Decimal };
            //    creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
            //    object[] values;
            //    foreach (FactureItem fi in f.FactureItems)
            //    {
            //        values = new object[headers.Length];
            //        values[0] = fi.Number.ToString();
            //        values[1] = fi.Description;
            //        values[2] = fi.Count.ToString();
            //        values[3] = String.Format("{0:#,0.00}", fi.PremiumValue);
            //        creator.AddDataRowForFactures(values, headers.Length, typeCodes);
            //    }
            //    values = new object[headers.Length];
            //    values[0] = "";
            //    values[1] = "";
            //    values[2] = "Вкупно";
            //    values[3] = String.Format("{0:#,0.00}", f.TotalCost);
            //    creator.AddDataRowForFactures(values, headers.Length, typeCodes);
            //    creator.AddTable();
            //    creator.SetTitleLeftBold14(" ");
            //    creator.SetTitleLeftItalic10("Со букви: " + NumberToTextController.Konvertiranje(f.TotalCost));
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft10("Ве молиме фактурираниот износ да го платите до назначениот рок на плаќање.");
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft10("Доколку износот за фактурата не биде платен до наведениот датум, се пресметува затезна камата од денот на стасаност до денот на плаќањето.");
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft10("Согласно со член 23, точка 6 од Законот за ДДВ дејноста осигурување е ослободена од плаќање данок без право на одбиток на претходниот данок.");
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft8(" ");
            //    creator.SetTitleLeft10("                          Фактурирал                                              Примил                                                          Одобрил");
            //    creator.SetTitleLeft10("                    _________________                            __________________                                  ____________________");

            //    string facItemDescriptionStart = "Полиса - ";
            //    creator.AddPage();
            //    foreach (FactureItem fi in f.FactureItems) {
            //        string polNumber = fi.Description.Substring(facItemDescriptionStart.Length);
            //        creator.SetTitleLeft10(facItemDescriptionStart + polNumber);
            //        string[] headersRates = { "Рата бр.", "Дата за плаќање", "Износ" };
            //        float[] widthPercentagesRates = { 15, 50, 35 };
            //        TypeCode[] typeCodesRates = { TypeCode.Int32, TypeCode.String, TypeCode.Decimal };
            //        creator.CreateTable_Facture(headersRates.Length, headersRates, widthPercentagesRates);
            //        PolicyItem pi = PolicyItem.GetByNumberAndInsuranceSubType(polNumber, fi.InsuranceSubTypeID, fi.Facture.InsuranceCompanyID);
            //        List<Rate> rates = Rate.GetByPolicyItemID(pi.ID);
            //        rates = rates.OrderBy(c => c.Number).ToList();
            //        foreach (Rate r in rates) {
            //            object[] valuesRates = new object[headersRates.Length];
            //            valuesRates[0] = r.Number;
            //            valuesRates[1] = r.Date.ToShortDateString();
            //            valuesRates[2] = String.Format("{0:#,0.00}", r.Value);
            //            creator.AddDataRowForFactures(valuesRates, headersRates.Length, typeCodes);
            //        }
            //        creator.AddTable();
            //    }

            //creator.FinishPDF_FileName("Factura" + f.FactureNumber);
        }
    }
}


