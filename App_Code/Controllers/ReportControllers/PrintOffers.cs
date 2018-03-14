using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using Broker.Controllers.ReportControllers;
using ASUC.Controllers.ConvertController;

/// <summary>
/// Summary description for PrintOffers
/// </summary>
/// 
namespace Broker.Controllers.ReportControllers
{
    public class PrintOffers
    {
        public static void PrintOffer(Offer o)
        {
            PDFCreators creator = new PDFCreators(true, 15, 15, 15, 15);
            creator.OpenPDF();
            creator.GetContentByte();
            creator.SetTitle("Понуда: " + o.OfferNumber);
            creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(o.Client.Name).ToUpper());
            creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(o.Client.Address).ToUpper());
            creator.SetTitleLeftWithFontSize10(ConvertToMacedonian.ConvertToMACEDONIAN(o.Client.Place.Municipality.Name).ToUpper());
            creator.SetTitleLeft("Датум на понуда: " + o.OfferDate.ToShortDateString());

            string[] headers = { "Ред. бр.", "Осигурителна компанија", "Класа на осигурување", "Подкласа на осигурување", "Износ" };
            float[] widthPercentages = { 10, 20, 25, 35, 10 };
            creator.CreateTable_Facture(5, headers, widthPercentages);
            object[] values;
            int i = 1;
            foreach (OfferItem oi in o.OfferItems)
            {
                values = new object[5];
                values[0] = i.ToString();
                values[1] = oi.InsuranceCompany.Name;
                values[2] = oi.InsuranceSubType.InsuranceType.Name;
                values[3] = oi.InsuranceSubType.Description;
                values[4] = oi.Cost;
                creator.AddDataRowForBillWithRigthAlligmentForValues(values, 5);
                i++;
            }
            values = new object[5];
            values[0] = "";
            values[1] = "";
            values[2] = "";
            values[3] = "Вкупно:";
            values[4] = o.Cost;
            creator.AddDataRowForBillWithRigthAlligmentForValues(values, 5);
            creator.AddTable();
            creator.FinishPDF_FileName("Ponuda" + o.OfferNumber);
        }
    }
}
