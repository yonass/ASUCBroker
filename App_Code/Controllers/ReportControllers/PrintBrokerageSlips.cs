using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using iTextSharp.text;

/// <summary>
/// Summary description for PrintBrokerageSlips
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintBrokerageSlips {
        public static void PrintBrokerageSlip(BrokerageSlip bs) {

            PDFCreators creator = new PDFCreators(true, 15, 15, 15, 15);
            creator.OpenPDF();
            creator.GetContentByte();
            creator.AddJDBLogoForFactures(10, 775);
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8(" ");
            creator.SetTitle("БРОКЕРСКИ СЛИП ЗА ОСИГУРУВАЊЕ");
            creator.SetTitle(bs.Number);
            string[] headers = { "", "" };
            float[] widthPercentages = { 30, 70 };
            TypeCode[] typeCodes = { TypeCode.String, TypeCode.String };
            creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
            object[] values;
            values = new object[headers.Length];
            values[0] = "ОСИГУРЕНИК";
            values[1] = bs.Insurer;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "ОСИГУРУВАЧ";
            values[1] = bs.InsuranceCompany.Name;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "ПЕРИОД НА ИЗДАДЕНИ ПОЛИСИ";
            values[1] = bs.FromDate.ToShortDateString() + " до " + bs.ToDate.ToShortDateString();
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "КЛАСИ НА ОСИГУРУВАЊЕ";
            int ordinalNumber = 1;
            string nested = "";
            foreach (BrokerageSlipInsuranceSubType bsist in bs.BrokerageSlipInsuranceSubTypes) {
                nested += (ordinalNumber + ". " + bsist.InsuranceSubType.ShortDescription + "\n");
                ordinalNumber++;
            }
            values[1] = nested;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "ОСИГУРЕНИ ОПАСНОСТИ";
            values[1] = bs.InsuranceRisks;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "ОСИГУРЕНИ СТВАРИ";
            values[1] = bs.InsuranceThings;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            int ordNumberPerInsSubType = 1;
            foreach (BrokerageSlipInsuranceSubType bsist in bs.BrokerageSlipInsuranceSubTypes) {
                object[] valINST = new object[2];
                valINST[0] = "ПОДКЛАСА НА ОСИГУРУВАЊЕ";
                valINST[1] = ordNumberPerInsSubType + ". " + bsist.InsuranceSubType.ShortDescription;
                creator.AddDataRowForBrokerageSlips(valINST, headers.Length, typeCodes);
                decimal totPrem = 0;
                List<BrokerageSlipPolicyItem> tmpList = BrokerageSlipPolicyItem.GetByBrokerageSlip(bs.ID);
                List<BrokerageSlipPolicyItem> tmpListPer = tmpList.Where(c => c.PolicyItem.InsuranceSubTypeID == bsist.InsuranceSubTypeID).ToList();
                List<PolicyItem> lstPolItems = tmpListPer.Select(c => c.PolicyItem).ToList();
                lstPolItems = lstPolItems.OrderBy(c => c.PolicyNumber).ToList();
                foreach (PolicyItem pi in lstPolItems) {
                    object[] valPol = new object[2];
                    string polPremValue = String.Format("{0:#,0.00}", pi.PremiumValue);
                    valPol[0] = "";
                    valPol[1] = pi.PolicyNumber + " - " + pi.Policy.Client.Name + " - " + polPremValue;
                    creator.AddDataRowForBrokerageSlips(valPol, headers.Length, typeCodes);
                    if (!pi.Policy.Discard) {
                        totPrem += pi.PremiumValue;
                    }
                }
                object[] valTotPerIST = new object[2];
                valTotPerIST[0] = "";
                valTotPerIST[1] = "ВКУПНО ПРЕМИЈА: " + String.Format("{0:#,0.00}", totPrem) + " ДЕНАРИ";
                creator.AddDataRowForBrokerageSlips(valTotPerIST, headers.Length, typeCodes);
                ordNumberPerInsSubType++;
            }
            //values = new object[headers.Length];
            //values[0] = "ПРЕМИЈА";
            //values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "ОСНОВ ЗА ОБЕШТЕТУВАЊЕ";
            values[1] = bs.BasisForCompensation;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "БРОКЕРАЖА";
            int ordNumber = 1;
            string nestedB = "";
            foreach (BrokerageSlipInsuranceSubType bsist in bs.BrokerageSlipInsuranceSubTypes) {
                nestedB += (ordNumber + ". " + bsist.InsuranceSubType.ShortDescription + " " + bsist.BrokeragePercentagePrivates + "% ЗА ФИЗИЧКИ ЛИЦА, " + bsist.BrokeragePercentageLaws + "% ЗА ПРАВНИ ЛИЦА ОД БРУТО ПРЕМИЈА" + "\n");
                ordNumber++;
            }
            values[1] = nestedB;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "ДРУГИ УСЛОВИ";
            values[1] = bs.OtherConditions;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "БРОКЕР";
            values[1] = BrokerHouseInformation.GetBrokerHouseNameByCode(BrokerHouseInformation.FIRST_ROW);
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.MESTO).Value;
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = bs.Date.ToShortDateString();
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            values = new object[headers.Length];
            values[0] = "";
            values[1] = "";
            creator.AddDataRowForBrokerageSlips(values, headers.Length, typeCodes);
            creator.AddTable();
            creator.SetTitle(" ");
            creator.SetTitle(" ");
            string brokerName = BrokerHouseInformation.GetBrokerHouseName();
            creator.SetTitleLeft8("         " + brokerName + "                                                                                                                           " + bs.InsuranceCompany.Name);
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("_________________________                                                                                                           ____________________");
            creator.FinishPDF_FileName("BrokerskiSlip" + bs.ID);
        }
    }
}

