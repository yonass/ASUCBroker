using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;

/// <summary>
/// Summary description for PrintRepresentationDeal
/// </summary>
public class PrintRepresentationDeal
{
    public static void RepresentationDeal(int accreditationID)
    {
        Accreditation a = Accreditation.Get(accreditationID);
        PDFCreators creator = new PDFCreators(true, 15, 15, 15, 15);
        creator.OpenPDF();
        creator.GetContentByte();
        creator.SetTitle("Овластување за застапништво");
        creator.SetTitleLeftWithFontSize10(a.Title);
        creator.SetTitleLeftWithFontSize10(" ");
        BrokerHouseInformation bhi = BrokerHouseInformation.GetInformation();
        creator.SetTitleLeftWithFontSize10("Јас, " + a.Client.Name + " со овој договор потврдувам дека брокерското друштво" + bhi.BrokerHouseName + " може да ме застапува за следните типови на осигурување:");
        creator.SetTitleLeftWithFontSize10(" ");
        List<AccreditationInsuranceType> listAIT = AccreditationInsuranceType.GetByAccreditation(accreditationID);
        foreach (AccreditationInsuranceType ait in listAIT)
        {
            creator.SetTitleLeftWithFontSize10("- " + ait.InsuranceType.Name);
        }
        creator.SetTitleLeftWithFontSize10(" ");
        creator.SetTitleLeftWithFontSize10("Времетраење на застапништвото " + a.FromDate.ToShortDateString() + " - " +a.ToDate.ToShortDateString());
        creator.SetTitleLeftWithFontSize10(" ");
        creator.SetTitleLeftWithFontSize10("  ____________________");
        creator.SetTitleLeftWithFontSize10("         Потпис");
        creator.FinishPDF_FileName("Dogovor");
    }
}
