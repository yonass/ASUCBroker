using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using ASUC.Controllers.ConvertController;

/// <summary>
/// Summary description for PrintFactures
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintFactures {
        public static void PrintFacture(Broker.DataAccess.Facture f) {
            string brokerName = BrokerHouseInformation.GetBrokerHouseName();
            if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA || f.DocumentSubType.Code == DocumentSubType.IZLEZNA_FAKTURA_ZA_PROVZIJA_ZA_ZIVOTNO_OSUGURUVANjE) {
                PDFCreators creator = new PDFCreators(true, 25, 25, 15, 15);
                creator.SetDocumentHeaderFooter();
                creator.OpenPDF();
                creator.GetContentByte();
                creator.AddJDBLogoForFactures(10, 775);
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft8(" ");
                creator.SetTitleLeft8(" ");
                if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA) {
                    creator.SetTitleLeft8("   Жиро-сметка :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZIRO_SMETKA_ZA_OSIG_KOMPANII).Value);
                } else if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                    creator.SetTitleLeft8("   Жиро-сметка :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZIRO_SMETKA).Value);
                }
                creator.SetTitleLeft8("   Депонент :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.DEPONENT).Value);
                creator.SetTitleLeft8("   ЕДБ :   " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.EDB).Value + "       " + "Матичен број :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.MATICEN_BROJ).Value);

                creator.SetTitleLeft8("  ");
                if (f.Discard) {
                    creator.SetTitleCenterForFactureNumber("СТОРНИРАНА Ф-ра бр. " + f.FactureNumber);
                } else {
                    creator.SetTitleCenterForFactureNumber("Фактура бр. " + f.FactureNumber);
                }
                string internalCode = "";
                try
                {
                    FinansovoDataClassesDataContext fdc = new FinansovoDataClassesDataContext();
                    List<Client> listCLients = fdc.Clients.Where(c => c.EMBG == f.Client.EMBG).ToList();
                    if (listCLients.Count > 0)
                    {
                        internalCode = listCLients[0].Code;
                    }
                }
                catch { }
                if(internalCode != string.Empty){
                    creator.SetTitleLeftWithFontSize10(internalCode);
                }
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Name).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Address).ToUpper());
                //creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Place.Municipality.Name).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Place.Name).ToUpper());
                creator.SetTitleLeft10("Датум на фактура: " + f.DateOfCreation.ToShortDateString());
                creator.SetTitleLeft10("Рок на плаќање: " + f.DateOfPayment.ToShortDateString());
                //creator.SetTitleLeft8("Период: " + f.FromDate.ToShortDateString() + " - " + f.ToDate.ToShortDateString()); 

                string[] headers = { "Ред. бр.", "Опис", "Количина", "Премија", "Брокеража" };
                float[] widthPercentages = { 8, 45, 15, 17, 15 };
                TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.Int32, TypeCode.Decimal, TypeCode.Decimal };
                creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
                object[] values;
                foreach (FactureItem fi in f.FactureItems) {
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
                foreach (FactureItem fi in f.FactureItems) {
                    creator.SetTitleLeft10Bold(fi.InsuranceSubType.Description);
                    creator.CreateTable_Facture(headersSpecifications.Length, headersSpecifications, widthPercentagesSpecifications);
                    if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA) {
                        List<PolicyItemFactureItem> pifiList = PolicyItemFactureItem.GetByFactureItemID(fi.ID);
                        int ordinalNumber = 1;
                        foreach (PolicyItemFactureItem pifi in pifiList) {
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
                    }
                    if (f.DocumentSubType.Code == DocumentSubType.IZLEZNA_FAKTURA_ZA_PROVZIJA_ZA_ZIVOTNO_OSUGURUVANjE) {
                        List<LifePolicyFactureItem> pifiList = LifePolicyFactureItem.GetByFactureItemID(fi.ID);
                        int ordinalNumber = 1;
                        foreach (LifePolicyFactureItem pifi in pifiList) {
                            valuesSpecifications = new object[6];
                            valuesSpecifications[0] = ordinalNumber;
                            valuesSpecifications[1] = pifi.LifePolicy.PolicyNumber;
                            valuesSpecifications[2] = pifi.LifePolicy.StartDate.ToShortDateString();
                            valuesSpecifications[3] = pifi.LifePolicy.EndDate.ToShortDateString();
                            valuesSpecifications[4] = pifi.LifePolicy.ApplicationDate.ToShortDateString();
                            valuesSpecifications[5] = pifi.LifePolicy.TotalPremumValue;
                            creator.AddDataRowForFacturesExtend(valuesSpecifications, headersSpecifications.Length, typeCodesSpecifications, headersSpecifications, widthPercentagesSpecifications);
                            ordinalNumber++;
                        }
                    }
                    creator.AddTable();
                    creator.SetTitleCenterForFactureNumber(" ");
                }

                creator.FinishPDF_FileName("Factura" + f.FactureNumber);
            }
            if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
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
                if (f.Discard) {
                    creator.SetTitleCenterForFactureNumber("СТОРНИРАНА Ф-ра бр. " + f.FactureNumber);
                } else {
                    creator.SetTitleCenterForFactureNumber("Фактура бр. " + f.FactureNumber);
                }
                string internalCode = "";
                try
                {
                    FinansovoDataClassesDataContext fdc = new FinansovoDataClassesDataContext();
                    List<Client> listCLients = fdc.Clients.Where(c => c.EMBG == f.Client.EMBG).ToList();
                    if (listCLients.Count > 0)
                    {
                        internalCode = listCLients[0].Code;
                    }
                }
                catch { }
                if (internalCode != string.Empty)
                {
                    creator.SetTitleLeftWithFontSize10(internalCode);
                }
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Name).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Address).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Place.Municipality.Name).ToUpper());
                creator.SetTitleLeft10("Датум на фактура: " + f.DateOfCreation.ToShortDateString());
                creator.SetTitleLeft10("Рок на плаќање: " + f.DateOfPayment.ToShortDateString());
                //creator.SetTitleLeft8("Период: " + f.FromDate.ToShortDateString() + " - " + f.ToDate.ToShortDateString()); 

                string[] headers = { "Ред. бр.", "Број на полиса", "Осиг. комапнија", "Тип на осигурување", "Премија" };
                float[] widthPercentages = { 8, 17, 30, 30, 15 };
                TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
                creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
                object[] values;

                foreach (FactureItem fi in f.FactureItems) {
                    List<PolicyItemFactureItem> pifi = PolicyItemFactureItem.GetByFactureItemID(fi.ID);
                    values = new object[headers.Length];
                    values[0] = fi.Number.ToString();
                    values[1] = pifi[0].PolicyItem.PolicyNumber;
                    values[2] = pifi[0].PolicyItem.Policy.InsuranceCompany.ShortName;
                    values[3] = pifi[0].PolicyItem.InsuranceSubType.ShortDescription;
                    values[4] = String.Format("{0:#,0.00}", fi.PremiumValue) + " ДЕН.";
                    creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                }
                values = new object[headers.Length];
                values[0] = "";
                values[1] = "";
                values[2] = "";
                values[3] = "Вкупно";
                values[4] = String.Format("{0:#,0.00}", f.TotalCost) + " ДЕН.";
                creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                creator.AddTable();
                creator.SetTitleLeftBold14(" ");
                creator.SetTitleLeftItalic10("Со букви: " + NumberToTextController.Konvertiranje(f.TotalCost));
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
                creator.SetTitleLeft10("                          Фактурирал                                              Примил                                                          Одобрил");
                creator.SetTitleLeft10("                    _________________                            __________________                                  ____________________");

                Broker.DataAccess.Parameter pSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT);
                bool fpSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT = true;
                if (pSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT != null) {
                    if (Convert.ToBoolean(pSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT.Value) == false) {
                        fpSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT = false;
                    }
                }
                if (fpSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT) {
                    string facItemDescriptionStart = "Полиса - ";
                    creator.AddPage();
                    foreach (FactureItem fi in f.FactureItems) {
                        string polNumber = fi.Description.Substring(facItemDescriptionStart.Length);
                        creator.SetTitleLeft10(facItemDescriptionStart + polNumber);
                        string[] headersRates = { "Рата бр.", "Дата за плаќање", "Износ" };
                        float[] widthPercentagesRates = { 15, 50, 35 };
                        TypeCode[] typeCodesRates = { TypeCode.Int32, TypeCode.String, TypeCode.Decimal };
                        creator.CreateTable_Facture(headersRates.Length, headersRates, widthPercentagesRates);
                        PolicyItem pi = PolicyItem.GetByNumberAndInsuranceSubType(polNumber, fi.InsuranceSubTypeID, (int)fi.Facture.InsuranceCompanyID);
                        List<Rate> rates = Rate.GetByPolicyItemID(pi.ID);
                        rates = rates.OrderBy(c => c.Number).ToList();
                        foreach (Rate r in rates) {
                            object[] valuesRates = new object[headersRates.Length];
                            valuesRates[0] = r.Number;
                            valuesRates[1] = r.Date.ToShortDateString();
                            valuesRates[2] = String.Format("{0:#,0.00}", r.Value);
                            creator.AddDataRowForFactures(valuesRates, headersRates.Length, typeCodes);
                        }
                        creator.AddTable();
                    }
                }
                creator.FinishPDF_FileName("Factura" + f.FactureNumber);
            }
            if (f.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
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
	        if(f.Discard){
                      creator.SetTitleCenterForFactureNumber("СТОРНИРАНА Фактура бр. " + f.FactureNumber);
                } else {
		     creator.SetTitleCenterForFactureNumber("Фактура бр. " + f.FactureNumber);
                }
                string internalCode = "";
                try
                {
                    FinansovoDataClassesDataContext fdc = new FinansovoDataClassesDataContext();
                    List<Client> listCLients = fdc.Clients.Where(c => c.EMBG == f.Client.EMBG).ToList();
                    if (listCLients.Count > 0)
                    {
                        internalCode = listCLients[0].Code;
                    }
                }
                catch { }
                if (internalCode != string.Empty)
                {
                    creator.SetTitleLeftWithFontSize10(internalCode);
                }
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Name).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Address).ToUpper());
                creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(f.Client.Place.Municipality.Name).ToUpper());
                creator.SetTitleLeft10("Датум на фактура: " + f.DateOfCreation.ToShortDateString());
                creator.SetTitleLeft10("Рок на плаќање: " + f.DateOfPayment.ToShortDateString());
                string[] headers = { "Ред. бр.", "Број на полиса", "Осиг. компанија", "Тип на осигурување", "Премија" };
                float[] widthPercentages = { 8, 17, 30, 30, 15 };
                TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
                creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
                object[] values;
                foreach (FactureItem fi in f.FactureItems) {
                    List<PolicyItemFactureItem> pifi = PolicyItemFactureItem.GetByFactureItemID(fi.ID);
                    values = new object[headers.Length];
                    values[0] = fi.Number.ToString();
                    values[1] = pifi[0].PolicyItem.PolicyNumber;
                    values[2] = pifi[0].PolicyItem.Policy.InsuranceCompany.ShortName;
                    values[3] = pifi[0].PolicyItem.InsuranceSubType.ShortDescription;
                    values[4] = String.Format("{0:#,0.00}", fi.PremiumValue) + " ДЕН.";
                    creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                }
                values = new object[headers.Length];
                values[0] = "";
                values[1] = "";
                values[2] = "";
                values[3] = "Вкупно";
                values[4] = String.Format("{0:#,0.00}", f.TotalCost) + " ДЕН.";
                creator.AddDataRowForFactures(values, headers.Length, typeCodes);
                creator.AddTable();
                creator.SetTitleLeftBold14(" ");
                creator.SetTitleLeftItalic10("Со букви: " + NumberToTextController.Konvertiranje(f.TotalCost));
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
                creator.SetTitleLeft10("                          Фактурирал                                              Примил                                                          Одобрил");
                creator.SetTitleLeft10("                    _________________                            __________________                                  ____________________");

                bool fpSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT = true;
                if (Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT) != null)
                {
                    fpSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT = Boolean.Parse(Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.SE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT).Value);
                }


                if (fpSE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT) {
                    string facItemDescriptionStart = "Полиса бр. ";
                    creator.AddPage();
                    foreach (FactureItem fi in f.FactureItems) {
                        string polNumber = fi.Description.Substring(facItemDescriptionStart.Length);
                        creator.SetTitleLeft10(facItemDescriptionStart + polNumber);
                        string[] headersRates = { "Рата бр.", "Дата за плаќање", "Износ" };
                        float[] widthPercentagesRates = { 15, 50, 35 };
                        TypeCode[] typeCodesRates = { TypeCode.Int32, TypeCode.String, TypeCode.Decimal };
                        creator.CreateTable_Facture(headersRates.Length, headersRates, widthPercentagesRates);
                        PolicyItem pi = PolicyItem.GetByNumberAndInsuranceSubType(polNumber, fi.InsuranceSubTypeID, (int)fi.Facture.InsuranceCompanyID);
                        List<Rate> rates = Rate.GetByPolicyItemID(pi.ID);
                        rates = rates.OrderBy(c => c.Number).ToList();
                        foreach (Rate r in rates) {
                            object[] valuesRates = new object[headersRates.Length];
                            valuesRates[0] = r.Number;
                            valuesRates[1] = r.Date.ToShortDateString();
                            valuesRates[2] = String.Format("{0:#,0.00}", r.Value);
                            creator.AddDataRowForFactures(valuesRates, headersRates.Length, typeCodes);
                        }
                        creator.AddTable();
                    }
                }




                creator.FinishPDF_FileName("Factura" + f.FactureNumber);
            }
        }

        public static void PrintAnex(Broker.DataAccess.Facture f)
        {
            List<PolicyItemFactureItem> lstPIFI = PolicyItemFactureItem.GetByFactureID(f.ID);
            PDFCreators creator = new PDFCreators(true, 25, 25, 15, 15);
            creator.SetDocumentHeaderFooter();
            creator.OpenPDF();
            creator.GetContentByte();
            creator.AddJDBLogoForFactures(10, 775);
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            if (f.DocumentSubType.Code == Broker.DataAccess.DocumentSubType.FAKTURA_PROVIZIJA) {
                creator.SetTitleLeft8("   Жиро-сметка :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZIRO_SMETKA_ZA_OSIG_KOMPANII).Value);
            } else if (f.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                creator.SetTitleLeft8("   Жиро-сметка :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ZIRO_SMETKA).Value);
            }
            creator.SetTitleLeft8("   Депонент :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.DEPONENT).Value);
            creator.SetTitleLeft8("   ЕДБ :   " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.EDB).Value + "       " + "Матичен број :  " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.MATICEN_BROJ).Value);
            creator.SetTitleLeft8("  ");
            creator.SetTitleLeft8("Договорувач " + f.Client.Name + "                                                                                                             " + "Експозитура " + f.User.Branch.Name);
            creator.SetTitleLeft8("  ");
            creator.SetTitle("ДОГОВОР " + f.FactureNumber);
            creator.SetTitle("за плаќање на должна премија за осигурување");
            creator.SetTitle("(плаќање на рати)");
            creator.SetTitleLeft8("  ");
            creator.SetTitleLeft8("  ");
            creator.SetTitleLeft8("1.Премијата за осигурување ќе се плати на рати, како што следи:");
            string[] headersRates = { "Реден број", "Рок за плаќање", "Полиса", "Фактура број", "Износ" };
            float[] widthPercentagesRates = { 10, 25, 25, 20, 20 };
            TypeCode[] typeCodesRates = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
            creator.CreateTable_Facture(headersRates.Length, headersRates, widthPercentagesRates);
            List<Rate> ratesForPrint = new List<Rate>();
            foreach (FactureItem fi in f.FactureItems) {
                List<PolicyItemFactureItem> list = lstPIFI.Where(p => p.FactureItemID == fi.ID).ToList();
                foreach (PolicyItemFactureItem pifi in list) {
                    string polNumber = PolicyItem.Get(pifi.PolicyItemID).PolicyNumber;
                    List<Rate> rates = Rate.GetByPolicyItemID(pifi.PolicyItemID);
                    rates = rates.OrderBy(c => c.Number).ToList();
                    ratesForPrint.AddRange(rates);
                }
            }
            ratesForPrint = ratesForPrint.OrderBy(c => c.Date).ToList();
            int counter = 1;
            foreach (Rate r in ratesForPrint) {
                object[] valuesRates = new object[headersRates.Length];
                valuesRates[0] = counter;
                valuesRates[1] = r.Date.ToShortDateString();
                valuesRates[2] = r.PolicyItem.PolicyNumber;
                valuesRates[3] = f.FactureNumber;
                valuesRates[4] = String.Format("{0:#,0.00}", r.Value) + " ДЕН.";
                creator.AddDataRowForFactures(valuesRates, headersRates.Length, typeCodesRates);
                counter++;
            }
            object[] values = new object[headersRates.Length];
            values[0] = "";
            values[1] = "";
            values[2] = "";
            values[3] = "Вкупно";
            values[4] = String.Format("{0:#,0.00}", f.TotalCost) + " ДЕН.";
            creator.AddDataRowForFactures(values, headersRates.Length, typeCodesRates);
            creator.AddTable();
            creator.SetTitleLeft8("2.Доколку настане осигуран случај - штета, независно од тоа што е договорено плаќање на рати, премијата за осигурување стасува за наплата веднаш до висината на надоместокот што треба да се исплати и ќе се наплати по пат на компензација.");
            creator.SetTitleLeft8("3.Ако премијата не се плати во договорените рокови од овој Договор, ќе се пресметува камата.");
            creator.SetTitleLeft8("4.Договорот е составен од три еднообразни примероци, од кои еден за осигуреникот, а два за осигурувачот.");
            creator.SetTitle(" ");
            creator.SetTitle(" ");
            creator.SetTitleCenter8("Во " + f.User.Branch.Municipality.Name + " на " + f.DateOfCreation.ToShortDateString());
            creator.SetTitle(" ");
            creator.SetTitle(" ");
            string brokerName = BrokerHouseInformation.GetBrokerHouseName();
            creator.SetTitleLeft8("         " + brokerName + "                                                                                                                           ДОГОВОРУВАЧ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("_________________________                                                                                                           ____________________");
            creator.FinishPDF_FileName("Aneks" + f.FactureNumber);
        }


        public static void PrintAnex(Policy p) {
            List<Rate> ratesForPrint = Rate.GetByPolicyItemID(p.PolicyItems[0].ID);
            Broker.DataAccess.Facture f = PolicyItemFactureItem.GetByPolicyItemID(p.PolicyItems[0].ID);
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
            creator.SetTitleLeft8("Договорувач " + p.Client.Name + "                                                                                                             " + "Експозитура " + p.Branch.Name);
            creator.SetTitleLeft8("  ");
            creator.SetTitle("ДОГОВОР " + p.PolicyItems[0].PolicyNumber);
            creator.SetTitle("за плаќање на должна премија за осигурување");
            creator.SetTitle("(плаќање на рати)");
            creator.SetTitleLeft8("  ");
            creator.SetTitleLeft8("  ");
            creator.SetTitleLeft8("1.Премијата за осигурување ќе се плати на рати, како што следи:");
            string[] headersRates = { "Реден број", "Рок за плаќање", "Полиса", "Фактура број", "Износ" };
            float[] widthPercentagesRates = { 10, 25, 25, 20, 20 };
            TypeCode[] typeCodesRates = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Decimal };
            creator.CreateTable_Facture(headersRates.Length, headersRates, widthPercentagesRates);
            ratesForPrint = ratesForPrint.OrderBy(c => c.Date).ToList();
            int counter = 1;
            foreach (Rate r in ratesForPrint) {
                object[] valuesRates = new object[headersRates.Length];
                valuesRates[0] = counter;
                valuesRates[1] = r.Date.ToShortDateString();
                valuesRates[2] = r.PolicyItem.PolicyNumber;
                if (f != null) {
                    valuesRates[3] = f.FactureNumber;
                } else {
                    valuesRates[3] = "";
                }
                valuesRates[4] = String.Format("{0:#,0.00}", r.Value) + " ДЕН.";
                creator.AddDataRowForFactures(valuesRates, headersRates.Length, typeCodesRates);
                counter++;
            }
            object[] values = new object[headersRates.Length];
            values[0] = "";
            values[1] = "";
            values[2] = "";
            values[3] = "Вкупно";
            values[4] = String.Format("{0:#,0.00}", p.PolicyItems[0].PremiumValue) + " ДЕН.";
            creator.AddDataRowForFactures(values, headersRates.Length, typeCodesRates);
            creator.AddTable();
            creator.SetTitleLeft8("2.Доколку настане осигуран случај - штета, независно од тоа што е договорено плаќање на рати, премијата за осигурување стасува за наплата веднаш до висината на надоместокот што треба да се исплати и ќе се наплати по пат на компензација.");
            creator.SetTitleLeft8("3.Ако премијата не се плати во договорените рокови од овој Договор, ќе се пресметува камата.");
            creator.SetTitleLeft8("4.Договорот е составен од три еднообразни примероци, од кои еден за осигуреникот, а два за осигурувачот.");
            creator.SetTitle(" ");
            creator.SetTitle(" ");
            if (f != null) {
                creator.SetTitleCenter8("Во " + f.User.Branch.Municipality.Name + " на " + f.DateOfCreation.ToShortDateString());
            } else {
                creator.SetTitleCenter8("Во " + p.Branch.Municipality.Name + " на " + p.ApplicationDate.ToShortDateString());
            }
            creator.SetTitle(" ");
            creator.SetTitle(" ");
            string brokerName = BrokerHouseInformation.GetBrokerHouseName();
            creator.SetTitleLeft8("         " + brokerName + "                                                                                                                           ДОГОВОРУВАЧ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("_________________________                                                                                                           ____________________");
            creator.FinishPDF_FileName("Aneks" + p.PolicyItems[0].PolicyNumber);
        }

    }
}
