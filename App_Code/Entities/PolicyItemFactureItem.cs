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

/// <summary>
/// Summary description for PolicyItemFactureItem
/// </summary>
namespace Broker.DataAccess {
    public partial class PolicyItemFactureItem : EntityBase<PolicyItemFactureItem> {

        public static void InsertForSingleFacture(FactureItem fi, PolicyItem pi) {
            PolicyItemFactureItem pifi = new PolicyItemFactureItem();
            pifi.FactureItemID = fi.ID;
            pifi.PolicyItemID = pi.ID;
            pifi.Insert();
        }


        public static void InsertForFacture(List<FactureItem> fiList, List<PolicyItem> piList) {
            foreach (PolicyItem pi in piList) {
                FactureItem fi = fiList.Where(f => f.InsuranceSubTypeID == pi.InsuranceSubTypeID).SingleOrDefault();
                PolicyItemFactureItem pifi = new PolicyItemFactureItem();
                pifi.FactureItemID = fi.ID;
                pifi.PolicyItemID = pi.ID;
                pifi.Insert();
            }
        }
        public static void InsertForFacture(FactureItem fi, List<PolicyItem> piList) {
            foreach (PolicyItem pi in piList) {
                PolicyItemFactureItem pifi = new PolicyItemFactureItem();
                pifi.FactureItemID = fi.ID;
                pifi.PolicyItemID = pi.ID;
                pifi.Insert();
            }
        }

        public static List<PolicyItemFactureItem> GetByFactureItemID(int factureItemID)
        {
            return Table.Where(c => c.FactureItemID == factureItemID).ToList();
        }

        public static Facture GetByPolicyItemID(int policyItemID) {
            PolicyItemFactureItem pifi = Table.Where(c => c.PolicyItemID == policyItemID && c.FactureItem.Facture.Discard == false && (c.FactureItem.Facture.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT || c.FactureItem.Facture.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA)).SingleOrDefault();
            if (pifi != null) {
                return pifi.FactureItem.Facture;
            }
            return null;
        }

        public static Facture GetByPolicyItemIDForBrokerage(int policyItemID) {
            List<PolicyItemFactureItem> pifi = Table.Where(c => c.PolicyItemID == policyItemID && c.FactureItem.Facture.Discard == false && c.FactureItem.Facture.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA).ToList();
            if (pifi.Count>0) {
                return pifi[0].FactureItem.Facture;
            }
            return null;
        }

        public static List<PolicyItemFactureItem> GetByFactureID(int factureID) {
            return Table.Where(c => c.FactureItem.FactureID == factureID).ToList();
        }

        public static List<PolicyItem> GetByPeriodInsuranceSubTypeAndInsuranceCompany(DateTime startDate, DateTime endDate, int insuranceSubTypeID, int insuranceCompanyID) {
            List<PolicyItem> retList = new List<PolicyItem>();
            if (insuranceCompanyID > 0) {
                retList = Table.Where(c => c.PolicyItem.Policy.ApplicationDate.Date >= startDate.Date && c.PolicyItem.Policy.ApplicationDate.Date <= endDate.Date
                    && c.PolicyItem.Policy.Discard == false && c.PolicyItem.InsuranceSubTypeID == insuranceSubTypeID && c.FactureItem.Facture.Discard == false
                    && c.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID
                    && c.FactureItem.Facture.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA).Select(c => c.PolicyItem).Distinct().ToList();
            } else {
                retList = Table.Where(c => c.PolicyItem.Policy.ApplicationDate.Date >= startDate.Date && c.PolicyItem.Policy.ApplicationDate.Date <= endDate.Date
                    && c.PolicyItem.Policy.Discard == false && c.PolicyItem.InsuranceSubTypeID == insuranceSubTypeID && c.FactureItem.Facture.Discard == false
                    && c.FactureItem.Facture.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA).Select(c => c.PolicyItem).Distinct().ToList();
            }
            return retList;
        }

        public static List<FactureItem> GetFIByPeriodInsuranceSubTypeAndInsuranceCompany(DateTime startDate, DateTime endDate, int insuranceSubTypeID, int insuranceCompanyID) {
            List<FactureItem> retList = new List<FactureItem>();
            if (insuranceCompanyID > 0) {
                retList = Table.Where(c => c.PolicyItem.Policy.ApplicationDate.Date >= startDate.Date && c.PolicyItem.Policy.ApplicationDate.Date <= endDate.Date
                    && c.PolicyItem.Policy.Discard == false && c.FactureItem.InsuranceSubTypeID == insuranceSubTypeID && c.FactureItem.Facture.Discard == false
                    && c.PolicyItem.Policy.InsuranceCompanyID == insuranceCompanyID  && c.FactureItem.Facture.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA).Select(c => c.FactureItem).Distinct().ToList();
            } else {
                retList = Table.Where(c => c.PolicyItem.Policy.ApplicationDate.Date >= startDate.Date && c.PolicyItem.Policy.ApplicationDate.Date <= endDate.Date
                    && c.PolicyItem.Policy.Discard == false && c.FactureItem.InsuranceSubTypeID == insuranceSubTypeID && c.FactureItem.Facture.Discard == false
                    && c.FactureItem.Facture.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA).Select(c => c.FactureItem).Distinct().ToList();
            }
            return retList;
        }

    }
}
