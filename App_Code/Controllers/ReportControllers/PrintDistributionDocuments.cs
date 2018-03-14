using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for PrintDistributionDocuments
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers {
    public class PrintDistributionDocuments {
        public static void PrintDistributionDocument(DistributionDocument dd) {
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

            creator.SetTitleLeft8("  ");
            if (dd.DistributionDocType.Code == DistributionDocType.ISPRATNICA) {
                creator.SetTitleCenterForFactureNumber("ИСПРАТНИЦА " + dd.DocumentNumber);
                creator.SetTitleLeftWithFontSize10("за филијала " + dd.Branch.Code + " " + dd.Branch.Name);
            } else if (dd.DistributionDocType.Code == DistributionDocType.POVRATNICA) {
                creator.SetTitleCenterForFactureNumber("ПОВРАТНИЦА " + dd.DocumentNumber);
                creator.SetTitleLeftWithFontSize10("од филијала " + dd.Branch.Code + " " + dd.Branch.Name);
            } else if (dd.DistributionDocType.Code == DistributionDocType.PRIEM) {
                creator.SetTitleCenterForFactureNumber("ПРИЕМ " + dd.DocumentNumber);
            }
            creator.SetTitleLeft10("Датум: " + dd.DocumentDate.ToShortDateString());

            List<DistributionDocumentItem> lstDDI = DistributionDocumentItem.GetByDistributionDocument(dd.ID);

            string[] headers = { "Ред. бр.", "Број на полиса", "Осиг. компанија", "Тип на осигурување", "Под на осигурување" };
            float[] widthPercentages = { 8, 17, 25, 25, 25 };
            TypeCode[] typeCodes = { TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.String };
            creator.CreateTable_Facture(headers.Length, headers, widthPercentages);
            int counter = 1;
            foreach (DistributionDocumentItem ddi in lstDDI) {
                object[] values;
                values = new object[headers.Length];
                values[0] = counter;
                values[1] = ddi.Distribution.PolicyNumber;
                values[2] = ddi.Distribution.InsuranceCompany.ShortName;
                values[3] = ddi.Distribution.InsuranceSubType.InsuranceType.ShortName;
                values[4] = ddi.Distribution.InsuranceSubType.ShortDescription;
                creator.AddDataRowForFacturesExtend(values, headers.Length, typeCodes, headers, widthPercentages);
                counter++;
            }
            creator.AddTable();
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("              Издал                                                                                                                                                                           Примил");
            creator.SetTitleLeft8(" ");
            creator.SetTitleLeft8("_________________________                                                                                                                                     ____________________");
            creator.FinishPDF_FileName("IspratnicaPovratnica" + dd.DocumentNumber);
        }
    }

}