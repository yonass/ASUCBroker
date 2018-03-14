using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InsuranceSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class InsuranceSubType : EntityBase<InsuranceSubType> {

        public static string AO = "1001";
        public static string ZK = "1099";

        public static string LAST_CODE = "99";

        public static List<InsuranceSubType> GetByInsuranceType(int insuranceTypeID) {
            return Table.Where(ist => ist.InsuranceTypeID == insuranceTypeID).ToList();
        }

        public static List<InsuranceSubType> GetByInsuranceTypeAndCompanyForLife(int companyID) {
            List<LifeDeal> dealList = LifeDeal.GetByInsuranceCompany(companyID);
            return dealList.Select(c => c.InsuranceSubType).OrderBy(c=>c.Code).ToList();
        }


        public static List<InsuranceSubType> GetByInsuranceTypeAndCompany(int insuranceTypeID, int companyID) {
            List<Deal> dealList = Deal.GetByInsuranceCompany(companyID);
            List<InsuranceSubType> insSubTypeList = new List<InsuranceSubType>();
            List<Brokerage> brokerageList = new List<Brokerage>();
            foreach (Deal d in dealList) {
                brokerageList.AddRange(d.Brokerages.ToList());
            }
            foreach (Brokerage b in brokerageList) {
                if (b.InsuranceSubType.InsuranceTypeID == insuranceTypeID) {
                    if (!insSubTypeList.Contains(b.InsuranceSubType)) {
                        insSubTypeList.Add(b.InsuranceSubType);
                    }
                }
            }
            return insSubTypeList.OrderBy(c=>c.Code).ToList();
        }

        public static List<InsuranceSubType> GetDealByInsuranceTypeAndCompany(int insuranceTypeID, int companyID)
        {
            List<Deal> dealList = Deal.GetByInsuranceCompany(companyID);
            List<InsuranceSubType> insSubTypeList = new List<InsuranceSubType>();
            List<Brokerage> brokerageList = new List<Brokerage>();
            foreach (Deal d in dealList)
            {
                brokerageList.AddRange(d.Brokerages.ToList());
            }
            foreach (Brokerage b in brokerageList)
            {
                if (b.InsuranceSubType.InsuranceTypeID == insuranceTypeID)
                {
                    if (!insSubTypeList.Contains(b.InsuranceSubType))
                    {
                        insSubTypeList.Add(b.InsuranceSubType);
                    }
                }
            }
            return insSubTypeList;
        }

        public static List<InsuranceSubType> GetPacketsByCompany(int insuranceTypeID, int companyID)
        {
            List<Packet> listPackets = Packet.GetActiveForCompany(companyID);
            List<InsuranceSubType> returnList = new List<InsuranceSubType>();
            foreach (Packet p in listPackets)
            {
                List<PacketsInsuranceSubType> listPacketsInsuranceSubType = PacketsInsuranceSubType.GetByPacket(p.ID);
                listPacketsInsuranceSubType = listPacketsInsuranceSubType.Where(i => i.InsuranceSubType.InsuranceTypeID == insuranceTypeID).ToList();
                foreach (PacketsInsuranceSubType pist in listPacketsInsuranceSubType)
                {
                    if (!returnList.Contains(pist.InsuranceSubType))
                    {
                        returnList.Add(pist.InsuranceSubType);
                    }
                }
            }
            return returnList;
        }

        public static List<InsuranceSubType> GetForDistributionByInsuranceTypeAndCompany(int insuranceTypeID, int companyID)
        {
            List<InsuranceSubType> returnList = new List<InsuranceSubType>();
            List<InsuranceSubType> dealInsuranceSubTypes = GetDealByInsuranceTypeAndCompany(insuranceTypeID, companyID);
            List<InsuranceSubType> packetInsuranceSubType = GetPacketsByCompany(insuranceTypeID, companyID);
            returnList.AddRange(dealInsuranceSubTypes);
            foreach (InsuranceSubType ist in packetInsuranceSubType)
            {
                if (!returnList.Contains(ist))
                {
                    returnList.Add(ist);
                }
            }
            returnList = returnList.OrderBy(r => r.Code).ToList();
            return returnList;
        }

        public static List<InsuranceSubType> GetForLife() {
            return Table.Where(c => c.InsuranceType.IsForLive == true).OrderBy(c => c.Code).ToList();
        }

        public static List<InsuranceSubType> GetForDistributionByInsuranceTypeAndCompanyWithLife(int insuranceTypeID, int companyID) {
            List<InsuranceSubType> returnList = new List<InsuranceSubType>();
            List<InsuranceSubType> dealInsuranceSubTypes = GetDealByInsuranceTypeAndCompany(insuranceTypeID, companyID);
            List<InsuranceSubType> packetInsuranceSubType = GetPacketsByCompany(insuranceTypeID, companyID);
            returnList.AddRange(dealInsuranceSubTypes);
            foreach (InsuranceSubType ist in packetInsuranceSubType) {
                if (!returnList.Contains(ist)) {
                    returnList.Add(ist);
                }
            }
            List<InsuranceSubType> lstWithLife = GetByInsuranceTypeAndCompanyForLife(companyID);
            foreach (InsuranceSubType ist in lstWithLife) {
                if (!returnList.Contains(ist)) {
                    returnList.Add(ist);
                }
            }
            returnList = returnList.OrderBy(r => r.Code).ToList();
            return returnList;
        }

        public static bool ExistCodeInInsuranceSubTypes(string code) {
            return (Table.Where(ist => ist.Code == code).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(ist => ist.Code == code && ist.ID != codeID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (InsuranceSubType.ExistCodeInInsuranceSubTypes(this.Code)) {
                ValidationErrors.Add("INSURANCESUBTYPE_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (InsuranceSubType.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("INSURANCESUBTYPE_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public static InsuranceSubType GetByCode(string code){
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }

        public static List<InsuranceSubType> GetByDeal(int dealID) {
            List<Brokerage> bList = Brokerage.GetByDeal(dealID);
            List<InsuranceSubType> insSubType = new List<InsuranceSubType>();
            foreach (Brokerage b in bList) {
                if (!insSubType.Contains(b.InsuranceSubType)) {
                    insSubType.Add(b.InsuranceSubType);
                }
            }
            return insSubType;
        }

        public static List<InsuranceSubType> GetByOrderItemID(int orderItemID,int insuranceTypeID) {
            List<InsuranceSubType> istList = new List<InsuranceSubType>();
            if (orderItemID != 0) {
                OrderItem oi = OrderItem.Get(orderItemID);
                istList.Add(oi.InsuranceSubType);
            } else {
                istList = Table.Where(i=>i.InsuranceTypeID==insuranceTypeID).ToList();
            }
            return istList;
        }

        public static List<InsuranceSubType> GetByOrderItemID(int insuranceTypeID) {
            return GetByInsuranceTypeAndExistingDeals(insuranceTypeID);
        }

        public static List<InsuranceSubType> GetByInsuranceTypeAndExistingDeals(int insuranceTypeID) {
            List<InsuranceSubType> istList = GetByInsuranceType(insuranceTypeID);
            List<InsuranceSubType> newIstList = new List<InsuranceSubType>();
            foreach (InsuranceSubType ist in istList) {
                List<Brokerage> bList = Brokerage.GetActiveByInsuranceSubType(ist.ID);
                if (bList.Count>0) {
                    newIstList.Add(ist);
                }
            }
            return newIstList;
        }

        public static List<InsuranceSubType> GetForOrder() {
            List<Brokerage> brokList = Brokerage.GetActiveBrokerages();
            List<InsuranceSubType> istList = new List<InsuranceSubType>();
            foreach (Brokerage b in brokList) {
                if(!istList.Contains(b.InsuranceSubType)){
                    istList.Add(b.InsuranceSubType);
                }
            }
            return istList;
        }

        public static List<InsuranceSubType> GetFromOffer(int offerID) {
            if (offerID == 0) {
                return null;
            }
            Offer o = Offer.Get(offerID);
            List<InsuranceSubType> istList = new List<InsuranceSubType>();
            if (o != null) {
                List<OfferItem> oItemsList = OfferItem.GetByOfferID(offerID);
                foreach (OfferItem oi in oItemsList) {
                    istList.Add(oi.InsuranceSubType);
                }
            }
            return istList;
        }

        public static List<InsuranceSubType> GetAllSorted() {
            return Table.OrderBy(c => c.InsuranceTypeID).OrderBy(c => c.Code).ToList();
        }

    }
}