using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Broker.DataAccess;
using System.Reflection;

/// <summary>
/// Summary description for WebServiceReceiveFromNBO
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebServiceReceiveFromNBO : System.Web.Services.WebService {

    public class PolicyInformation
    {
        public string PolicyNumber;
        public DateTime StartDate;
        public DateTime EndDate;
        public DateTime ApplicationDate;
        public decimal PremiumValue;
        public decimal TotalValue;
        public decimal WindshieldValue;
        public decimal PolicyPositivePaymentsValue;
        public decimal PolicyNegativePaymentsValue;
        public string ClientName;
        public string ClientAddress;
        public string ClientMunicipalityCode;
        public string ClientEMBG;
        public string OwnerName;
        public string OwnerAddress;
        public string OwnerMunicipalityCode;
        public string OwnerEMBG;
    }

    public WebServiceReceiveFromNBO () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public PolicyInformation Request(int nboCompanyID, string insuranceSubTypeCode)
    {
        PolicyInformation pi = new PolicyInformation();
        return pi;
    }

    //[WebMethod]
    //public Dictionary<string, string> Request(int nboCompanyID, string insuranceSubTypeCode)
    //{
    //    Dictionary<string, string> policyInformations = new Dictionary<string, string>();
    //    PolicyInformation pi = new PolicyInformation();
    //    Type t = pi.GetType();
    //    PropertyInfo[] p = t.GetProperties();
    //    foreach (PropertyInfo pInfo in p)
    //    {
    //        policyInformations.Add(pInfo.Name, string.Empty);
    //    }
    //    return policyInformations;
    //}

    //[WebMethod]
    //public void InsertTemporaryPolicy(Dictionary<string, string> dictionary)
    //{
    //    TemporaryPolicy newTP = new TemporaryPolicy();
    //    newTP.PolicyNumber = dictionary["PolicyNumber"];
    //    newTP.PremiumValue = Convert.ToDecimal(dictionary["PremiumValue"]);
    //    newTP.ApplicationDate = Convert.ToDateTime(dictionary["ApplicationDate"]);
    //    newTP.ClientAddress = dictionary["ClientAddress"];
    //    newTP.ClientName = dictionary["ClientName"];
    //    newTP.EndDate = Convert.ToDateTime(dictionary["EndDate"]);
    //    newTP.StartDate = Convert.ToDateTime(dictionary["StartDate"]);
    //    newTP.WindshieldValue = Convert.ToDecimal(dictionary["WindshieldValue"]);
    //    newTP.Insert();
    //}

    [WebMethod]
    public void InsertTemporaryPolicy(object o)
    {
        PolicyInformation pi = (PolicyInformation)o;
        TemporaryPolicy newTP = new TemporaryPolicy();
        newTP.PolicyNumber = pi.PolicyNumber;
        newTP.PremiumValue = pi.PremiumValue;
        newTP.StartDate = pi.StartDate;
        newTP.WindshieldValue = pi.WindshieldValue;
        newTP.EndDate = pi.EndDate;
        newTP.ClientName = pi.ClientName;
        newTP.ClientAddress = pi.ClientAddress;
        newTP.ApplicationDate = pi.ApplicationDate;
        newTP.Insert();
    }
    
}

