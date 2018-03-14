using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Broker.DataAccess;
/// <summary>
/// Summary description for PolicyController
/// </summary>
namespace Broker.Controllers.PolicyControllers {
    public class PolicyController {

        public static PolicyInfo GetPolicyInfo(int OfferItemID) {
            return new PolicyInfo(OfferItemID);
        }
        public static void UpdatePolicyInfo() {
        }

        //public static void UpdatePolicyInfo(PolicyInfo pi) {
        //}

        //public static void UpdatePolicyInfo(Policy p) {
        //}

        public static Policy Get(int id) {
            return new Policy();
        }
        public static List<InsuranceCompany> GetActiveInsuranceCompanies() {
            return InsuranceCompany.GetActiveInsuranceCompanies();
        }

        public static void Insert(PolicyInfo pi) {
            Policy p = pi.Policy;
            p.ApplicationDate = DateTime.Today;
            p.StatusID = 2;
            p.Insert();
        }


        public static string DeletePolicy(int piID)
        {
            DataClassesDataContext dc = new DataClassesDataContext();
            string retString = "Полисата е избришана.";
            PolicyItem pi = PolicyItem.Get(piID);
            Distribution d = Distribution.GetByNumberAndSubTypeAndCompany(pi.InsuranceSubTypeID, pi.PolicyNumber, pi.Policy.InsuranceCompanyID);
            Policy p = Policy.Get(pi.PolicyID);
            List<RollBackDistributionItem> listRDI = RollBackDistributionItem.Table.Where(c => c.PolicyItemID == pi.ID).ToList();
            if (listRDI.Count > 0)
            {
                retString = "Полисата е раздолжена и не може да се брише!";
                return retString;
            }

            List<Rate> lstRates = Rate.GetByPolicyItemID(pi.ID);
            if (lstRates.Count > 0)
            {
                List<Payment> lstPayments = Payment.GetByRateID(lstRates[0].ID);
                if (lstPayments.Count > 0)
                {
                    if (lstPayments[0].IsFactured)
                    {
                        retString = "Полисата е фактурирана за брокеража и не може да се брише!";
                        return retString;
                    }
                }
                foreach (Rate r in lstRates)
                {
                    List<Payment> lstP = Payment.GetByRateID(r.ID);
                    foreach (Payment pay in lstP)
                    {
                        List<PaymentsPerInsSubType> lstPPIST = PaymentsPerInsSubType.GetByPayment(pay.ID);
                        foreach (PaymentsPerInsSubType ppist in lstPPIST)
                        {
                            string queryppist = @"DELETE FROM PaymentsPerInsSubTypes WHERE ID = " + ppist.ID;
                            dc.ExecuteCommand(queryppist);
                            //ppist.Delete();
                        }
                        string queryppay = @"DELETE FROM Payments WHERE ID = " + pay.ID;
                        dc.ExecuteCommand(queryppay);
                        //pay.Delete();
                    }
                    string queryr = @"DELETE FROM Rates WHERE ID = " + r.ID;
                    dc.ExecuteCommand(queryr);
                    //r.Delete();
                }
                string querypei = @"DELETE FROM PolicyExtendInformations WHERE PolicyItemID = " + pi.ID;
                dc.ExecuteCommand(querypei);
                Broker.DataAccess.Facture facture  = PolicyItemFactureItem.GetByPolicyItemID(pi.ID);
                if (facture != null)
                {
                    List<Broker.DataAccess.FactureItem> listFI = FactureItem.GetByFacture(facture.ID);
                    foreach (FactureItem fi in listFI)
                    {
                        string querypifi = @"DELETE FROM PolicyItemFactureItems WHERE FactureItemID = " + fi.ID;
                        dc.ExecuteCommand(querypifi);
                        string queryfi = @"DELETE FROM FactureItems WHERE ID = " + fi.ID;
                        dc.ExecuteCommand(queryfi);
                    }
                    string queryf = @"DELETE FROM Factures WHERE ID = " + facture.ID;
                    dc.ExecuteCommand(queryf);
                }
               
                string querypi = @"DELETE FROM PolicyItems WHERE ID = " + pi.ID;
                dc.ExecuteCommand(querypi);
                //pi.Delete();
                string queryp = @"DELETE FROM Policies WHERE ID = " + p.ID;
                dc.ExecuteCommand(queryp);
                //p.Delete();
                string queryd = @"UPDATE Distributions SET IsUsed = 0 WHERE ID = " + d.ID;
                dc.ExecuteCommand(queryd);
                //d.IsUsed = false;
                //Distribution.Table.Context.SubmitChanges();
            }

            return retString;
        }

        public static void InsertClientInFinansovo(int clientID)
        {
            try
            {
                Broker.DataAccess.Client clientBroker = Broker.DataAccess.Client.Get(clientID);
                FinansovoDataClassesDataContext fdc = new FinansovoDataClassesDataContext();
                List<Client> list = fdc.Clients.Where(c => c.EMBG == clientBroker.EMBG).ToList();
                if (list.Count == 0)
                {
                    Client newClient = new Client();
                    newClient.Address = clientBroker.Address;
                    string oldCode = fdc.Clients.OrderBy(c => c.ID).ToList().Last().Code;
                    int newCode = 9999;
                    int.TryParse(oldCode, out newCode);
                    newCode++;
                    string newCodeString = newCode.ToString();
                    string leadingZeros = "";
                    for (int i = 0; i < 4 - newCode.ToString().Length; i++)
                    {
                        leadingZeros += "0";
                    }
                    newCodeString = leadingZeros + newCodeString;
                    newClient.Code = newCodeString;
                    newClient.CompanyID = fdc.Companies.First().ID;
                    newClient.EMBG = clientBroker.EMBG;
                    newClient.FaxNumber = clientBroker.Fax;
                    newClient.IsBuyer = true;
                    newClient.IsSeller = false;
                    newClient.MobileNumber = clientBroker.Mobile;
                    newClient.Name = clientBroker.Name;
                    newClient.PhoneNumber = clientBroker.Phone;
                    newClient.PlaceID = fdc.Places.First().ID;
                    newClient.TaxNumber = "";
                    fdc.Clients.InsertOnSubmit(newClient);
                    fdc.Clients.Context.SubmitChanges();
                }
            }
            catch { }
        }

    }
}


    

