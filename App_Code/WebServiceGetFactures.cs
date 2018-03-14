using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Broker.DataAccess;

/// <summary>
/// Summary description for WebServiceGetFactures
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebServiceGetFactures : System.Web.Services.WebService {

    public class CashPayments
    {
        public string PolicyNumber;
        public string SerialNumber;
        public string LocationCode;
        public string LocationName;
        public List<WebServiceGetFactures.CashPaymentItems> CashItems;
        public List<WebServiceGetFactures.CashPaymentSpecifications> CashSpecifications;
        public DateTime Date;
    }

    public class CashPaymentItems
    {
        public string PaymentTypeCode;
        public decimal Value;
    }

    public class CashPaymentSpecifications
    {
        public decimal DDVPercentage;
        public decimal TaxValue;
        public string DDVCode;
        public decimal IncomeValue;
    }

    public class Facture
    {
        public string LocationCode;
        public string LocationName;
        public string FactureNumber;
        public DateTime FactureDate;
        public decimal TotalCost;
        public decimal BrokerageValue;
        public WebServiceGetFactures.Client Client;
        public int StatusID;
        public List<WebServiceGetFactures.FactureDDVSpecifications> FactureSpecifications;
    }

    public class FactureDDVSpecifications
    {
        public decimal DDVPercentage;
        public decimal TaxValue;
        public string DDVCode;
        public decimal IncomeValue;
    }

    public class Client
    {
        public string EMBG;
        public string Name;
        public bool IsLaw;
        public string Address;
        public string Phone;
        public string Mobile;
        public string EMail;
        public string Fax;
        public string PlaceCode;
        public string PlaceName;
        public string MunicipalityCode;
        public string MunicipalityName;
    }

    public WebServiceGetFactures()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod(Description = "Metod koj vraka fakturi za daden period")]
    public List<Facture> GetFactures(DateTime fromDate, DateTime toDate)
    {
        List<Facture> returnList = new List<Facture>();
        List<Broker.DataAccess.Facture> listF = Broker.DataAccess.Facture.Table.Where(f => f.DateOfCreation.Date >= fromDate.Date && f.DateOfCreation.Date <= toDate.Date).ToList();
        foreach (Broker.DataAccess.Facture facture in listF)
        {
            Facture fac = new Facture();
            WebServiceGetFactures.Client c = new Client();
            c.Address = facture.Client.Address;
            c.EMBG = facture.Client.EMBG;
            c.EMail = facture.Client.EMail;
            c.Fax = facture.Client.Fax;
            c.IsLaw = facture.Client.IsLaw;
            c.Mobile = facture.Client.Mobile;
            c.Name = facture.Client.Name;
            c.Phone = facture.Client.Phone;
            c.PlaceName = facture.Client.Place.Name;
            c.PlaceCode = facture.Client.Place.Code;
            c.MunicipalityCode = facture.Client.Place.Municipality.Code;
            c.MunicipalityName = facture.Client.Place.Municipality.Name;
            fac.Client = c;
            fac.FactureNumber = facture.FactureNumber;
            fac.BrokerageValue = facture.BrokerageValue;
            fac.FactureDate = facture.DateOfCreation;
            fac.TotalCost = facture.TotalCost;
            fac.LocationCode = facture.User.Branch.Code;
            fac.LocationName = facture.User.Branch.Name;
            fac.StatusID = facture.StatusID;
            fac.FactureSpecifications = new List<WebServiceGetFactures.FactureDDVSpecifications>();
            //List<FactureSpecification> listFS = FactureSpecification.GetByFacture(facture.ID);
            //foreach (FactureSpecification fs in listFS) {
            FactureDDVSpecifications fddv = new FactureDDVSpecifications();
            fddv.DDVPercentage = 0;
            fddv.TaxValue = 0;
            fddv.IncomeValue = facture.TotalCost;
            fddv.DDVCode = "В";
            fac.FactureSpecifications.Add(fddv);
            //}
            returnList.Add(fac);
        }
        return returnList;
    }

    [WebMethod(Description = "Metod koj vraka plakanja vo gotovo (kesh i na platezni karticki) za daden period")]
    public List<WebServiceGetFactures.CashPayments> GetCashPayments(DateTime fromDate, DateTime toDate)
    {
        List<WebServiceGetFactures.CashPayments> returnList = new List<WebServiceGetFactures.CashPayments>();
        //List<Broker.DataAccess.CashPayment> listCP = Broker.DataAccess.CashPayment.Table.Where(cp => cp.Policy.ApplicationDate.Date >= fromDate.Date && cp.Policy.ApplicationDate.Date <= toDate.Date && cp.Discarded == false).ToList();
        //foreach (Broker.DataAccess.CashPayment cp in listCP)
        //{
        //    WebServiceGetFactures.CashPayments cashP = new CashPayments();
        //    if (cp.Policy.PacketID == null)
        //    {
        //        cashP.PolicyNumber = cp.Policy.PolicyItems[0].PolicyNumber;
        //    } else
        //    {
        //        cashP.PolicyNumber = cp.Policy.PolicyNumber;
        //    }
        //    cashP.SerialNumber = cp.SerialNumber;
        //    cashP.Date = cp.Policy.ApplicationDate;
        //    cashP.LocationCode = cp.User.Branch.Code;
        //    cashP.LocationName = cp.User.Branch.Name;
        //    cashP.CashItems = new List<WebServiceGetFactures.CashPaymentItems>();
        //    List<Broker.DataAccess.CashPaymentItem> listCPI = Broker.DataAccess.CashPaymentItem.GetByCashPayment(cp.ID);
        //    foreach (Broker.DataAccess.CashPaymentItem cpi in listCPI)
        //    {
        //        WebServiceGetFactures.CashPaymentItems cashPaymentItem = new CashPaymentItems();
        //        cashPaymentItem.PaymentTypeCode = cpi.PaymentType.Code;
        //        cashPaymentItem.Value = cpi.Value;
        //        cashP.CashItems.Add(cashPaymentItem);
        //    }

        //    decimal valueForTax = 0;
        //    foreach (Broker.DataAccess.CashPaymentItem cpi in listCPI)
        //    {
        //        valueForTax += cpi.Value;
        //    }
        //    cashP.CashSpecifications = new List<CashPaymentSpecifications>();
        //    CashPaymentSpecifications cashPaymentSpecificationItem = new CashPaymentSpecifications();
        //    cashPaymentSpecificationItem.DDVCode = "В";
        //    cashPaymentSpecificationItem.DDVPercentage = (decimal)0;
        //    cashPaymentSpecificationItem.IncomeValue = valueForTax;
        //    cashPaymentSpecificationItem.TaxValue = (decimal)0;
        //    cashP.CashSpecifications.Add(cashPaymentSpecificationItem);

        //    returnList.Add(cashP);
        //}
        return returnList;
    }

}

