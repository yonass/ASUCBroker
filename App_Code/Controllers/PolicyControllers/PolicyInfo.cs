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
using Broker.DataAccess;

/// <summary>
/// Summary description for PolicyInfo
/// </summary>
namespace Broker.Controllers.PolicyControllers {
    public class PolicyInfo {


        public string OfferID{get; set;}
        public string PolicyNumber { get; set;}
        //private string _startDate;
        //private string _endDate;
        //public string PremiumValue { get; set;}
        public string BrokeragePercentage { get; set;}
    //    public string BrokerageValue {get; set;}
        public string BrokerageID { get; set; }
        public bool Discard { get; set;}
        public string UserID { get; set;}
        public string BranchID { get; set;}
        public string ApplicationDate { get; set; }
        public string ClientID { get; set; }
        public string OwnerID { get; set; }
        
        //private string _insuranceSubTypeID;
        //private string _insuranceCompanyID;
        //private string _brokerageID;
        //private string _clientID;
        //private string _ownerID;
        //private string _applicationDate;
        //private string _dealID;
        //private string _policyID;
        //private bool _discard;
        //private string _userID;
        //private string _branchID;
        //private string _insuranceSubTypeID;
        //private string _insuranceCompanyID;
        //private string _brokerageID;
        //private string _clientID;
        //private string _ownerID;
        //private string _applicationDate;
        //private string _dealID;
        public string PolicyID { get; set; }
        private Broker.DataAccess.Client _client;
        private Broker.DataAccess.Client _owner;

        private Policy _policy;
        private string _offerID;
        private string _policyNumber;
        private string _startDate;
        private string _endDate;
        private string _premiumValue;
        private string _brokeragePercentage;
        private string _brokerageValue;

        private bool _discard;
        private string _userID;
        private string _branchID;
        private string _insuranceSubTypeID;
        private string _insuranceCompanyID;
        private string _brokerageID;
        private string _clientID;
        private string _ownerID;
        private string _applicationDate;
        private string _dealID;
        private string _policyID;
        private string _insuranceTypeID;


        private OfferItem _offerItem;

        public PolicyInfo(int OfferItemID) {
            if (OfferItemID != 0) {
                OfferItem oi = OfferItem.Get(OfferItemID);
                _offerItem = oi;
                BuildFromOfferItem(oi); 
            } else {
                _offerItem = null;
               // PolicyInfo();
            }
        }

        public void BuildFromOfferItem(OfferItem oi){
            Policy pi = new Policy();
            pi.Client = oi.Offer.Client;
            pi.InsuranceCompanyID = oi.InsuranceCompanyID;
           // pi.InsuranceSubTypeID = oi.InsuranceSubTypeID;
            pi.Client1= oi.Offer.Client1;
            _policy = pi;
            
        }

        public PolicyInfo() {
            _policy = new Policy();
        }

        public PolicyInfo(Policy p) {
            Policy = p;
        }

        public PolicyInfo(PolicyInfo pi) {
            _policy = pi.Policy;
        }

        public string StartDate {
            get {
                if (_startDate != null) {
                    return _startDate;
                } else {
                    return DateTime.Today.ToShortDateString();
                }
            }
            set {
                _startDate = value;
            }
        }
        public string EndDate {
            get {
                if (_endDate != null) {
                    return _endDate;
                } else {
                    return DateTime.Today.AddYears(1).ToShortDateString();
                }
            }
            set {
                _endDate= value;
            }
        }

        public string PremiumValue {
            get {
                if (_premiumValue != null) {
                    return _premiumValue;
                } else {
                    return "0";
                }
            }
            set {
                _premiumValue = value;
            }
        }
        public string BrokerageValue {
            get {
                if (_brokerageValue != null) {
                    return _brokerageValue;
                } else {
                    return "0";
                }
            }
            set {
                _brokerageValue = value.ToString();
            }
        }
        public string DealID {
            get {
                if (_offerItem != null) {
                    return Deal.GetActiveDealsForCompanyAndInsuranceSubType(_offerItem.InsuranceCompanyID, _offerItem.InsuranceSubTypeID).First().ID.ToString();
                } else {
                    return "1";
                }
            }
            set {
                _dealID = value;
            }
        }

        public Broker.DataAccess.Client Client
        {
            get {
                if (_offerItem != null) {
                    Offer O = _offerItem.Offer;
                    return O.Client;

                } else {
                    return null;
                }
            }
            set {
                _client = value;
            }
        }



        public Broker.DataAccess.Client Owner
        {
            get {
                if (_offerItem != null) {
                    Offer O = _offerItem.Offer;
                    return O.Client1; 
                } else {
                    return null;
                }
            }
            set {
                _owner = value;
            }
        }

        public string ClientName {
            get {
                if (Client != null) {
                    return Client.Name;
                } else
                    return string.Empty;
            }
            set {
            }
        }
        public string ClientEMBG {
            get {
                if (Client != null) {
                    return Client.EMBG;
                } else
                    return string.Empty;
            }
            set {
            }
        }
        public string OwnerName {
            get {
                if (Owner != null) {
                    return Owner.Name;
                } else
                    return string.Empty;
            }
            set {
            }
        }
        public string OwnerEMBG {
            get {
                if (Owner != null) {
                    return Owner.EMBG;
                } else
                    return string.Empty;
            }
            set {
            }
        }

        public string InsuranceSubTypeID {
            get {
                if (_offerItem != null) {
                    return _offerItem.InsuranceSubTypeID.ToString();
                } else {
                    return InsuranceSubType.GetByInsuranceType(int.Parse(InsuranceTypeID)).First().ID.ToString();
                    //return 1;
                }
            }
            set {
                _insuranceSubTypeID = value;
            }
        }

        public string InsuranceCompanyID {
            get {
                if (_insuranceCompanyID == null) {
                    if (_offerItem != null) {
                        return _offerItem.InsuranceCompanyID.ToString();
                    } else {
                        return "1";
                    }
                }
                return _insuranceCompanyID;
            }
            set {
                _insuranceCompanyID = value;
            }
        }

        public string InsuranceTypeID {
            get {
                if (_offerItem != null) {
                    return _offerItem.InsuranceSubType.InsuranceTypeID.ToString();
                } else {
                    return "1";
                }
            }
            set {
                _insuranceTypeID = value;
            }
        }

        public Policy BuildPolicy() {
            int id = 0;
            if (PolicyID != null) {
                id = int.Parse(PolicyID);
            }
            
            Policy p;
            try {
                p = Policy.Get(id);
            } catch {
                p = Policy.Get(null);
            }
            
            p.ApplicationDate = DateTime.Today;
            p.BranchID = int.Parse(BranchID);
           // p.BrokerageID = 1;
            //p.BrokerageID = int.Parse(BrokerageID);
            //p.BrokeragePercentage = decimal.Parse(BrokeragePercentage);
            p.BrokeragePercentage = 5;
          //  p.BrokerageValue = decimal.Parse(BrokerageValue);
            p.ClientID = int.Parse(ClientID);
            p.Discard = Discard;
            p.EndDate = DateTime.Parse(EndDate);
            p.InsuranceCompanyID =int.Parse(InsuranceCompanyID);
          //  p.InsuranceSubTypeID =int.Parse(InsuranceSubTypeID);
            if (OfferID == null) {
                p.OfferID = null;
            } else {
                p.OfferID = int.Parse(OfferID);
            }
            p.OwnerID = int.Parse(OwnerID);
            p.PolicyNumber = PolicyNumber;
           // p.PremiumValue = decimal.Parse(PremiumValue);
            p.StartDate=DateTime.Parse(StartDate);
            p.UserID=int.Parse(UserID);
            Policy = p;
            return p;
        }

        public Policy Policy {
            get {
                return _policy;
            }
            set {
                _policy = value;
                ApplicationDate = value.ApplicationDate.ToShortDateString();
                BranchID = value.BranchID.ToString();
              //  BrokerageID = value.BrokerageID.ToString();
                BrokeragePercentage = value.BrokeragePercentage.ToString();
                //BrokerageValue = value.BrokerageValue.ToString();
                Client = value.Client;
                
                Discard = value.Discard;
                EndDate = value.EndDate.ToShortDateString();
                InsuranceCompanyID = value.InsuranceCompanyID.ToString();
               // InsuranceSubTypeID = value.InsuranceSubTypeID.ToString();
                OfferID = value.OfferID.ToString();
                Owner = value.Client1;
                PolicyNumber = value.PolicyNumber;
               // PremiumValue = value.PremiumValue.ToString();
                StartDate = value.StartDate.ToShortDateString();
                UserID = value.UserID.ToString();
            }

        }
        
    }
}