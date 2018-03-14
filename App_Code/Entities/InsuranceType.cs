using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InsuranceType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class InsuranceType : EntityBase<InsuranceType> {


        public static bool ExistCodeInInsuranceTypes(string code) {
            return (Table.Where(it => it.Code == code).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(it => it.Code == code && it.ID != codeID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (InsuranceType.ExistCodeInInsuranceTypes(this.Code)) {
                ValidationErrors.Add("INSURANCETYPE_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (InsuranceType.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("INSURANCETYPE_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public static List<InsuranceType> GetByCompany(int companyID) {
            List<Deal> dealList = Deal.GetByCompanyID(companyID);
            List<InsuranceType> insList = new List<InsuranceType>();
            List<Brokerage> brokList = Brokerage.GetByDeals(dealList);

            foreach (Brokerage b in brokList) {
                if (b.IsActive && !insList.Contains(b.InsuranceSubType.InsuranceType)) {
                    insList.Add(b.InsuranceSubType.InsuranceType);
                }
            }
            return insList.OrderBy(c=>c.Code).ToList();
        }

        public static List<InsuranceType> GetByCompanyWithLife(int companyID) {
            List<Deal> dealList = Deal.GetByCompanyID(companyID);
            List<InsuranceType> insList = new List<InsuranceType>();
            List<Brokerage> brokList = Brokerage.GetByDeals(dealList);

            foreach (Brokerage b in brokList) {
                if (b.IsActive && !insList.Contains(b.InsuranceSubType.InsuranceType)) {
                    insList.Add(b.InsuranceSubType.InsuranceType);
                }
            }
            List<LifeDeal> lst = LifeDeal.GetByInsuranceCompany(companyID);
            foreach (LifeDeal ld in lst) {
                if (!insList.Contains(ld.InsuranceSubType.InsuranceType)) {
                    insList.Add(ld.InsuranceSubType.InsuranceType);
                }
            }
            return insList.OrderBy(c => c.Code).ToList();
        }

        public static List<InsuranceType> GetAllInsuranceTypesForCompany(int companyID) {
            List<Deal> dealList = Deal.GetByCompanyID(companyID);
            List<InsuranceType> insList = new List<InsuranceType>();
            List<Brokerage> brokList = Brokerage.GetByDeals(dealList);

            foreach (Brokerage b in brokList) {
                if (!insList.Contains(b.InsuranceSubType.InsuranceType)) {
                    insList.Add(b.InsuranceSubType.InsuranceType);
                }
            }
            return insList;
        }

        public static List<InsuranceType> GetPacketsByCompany(int companyID)
        {
            List<Packet> listPackets = Packet.GetActiveForCompany(companyID);
            List<InsuranceType> returnList = new List<InsuranceType>();
            foreach (Packet p in listPackets)
            {
                List<PacketsInsuranceSubType> listPacketsInsuranceSubType = PacketsInsuranceSubType.GetByPacket(p.ID);
                foreach (PacketsInsuranceSubType pist in listPacketsInsuranceSubType)
                {
                    if (!returnList.Contains(pist.InsuranceSubType.InsuranceType))
                    {
                        returnList.Add(pist.InsuranceSubType.InsuranceType);
                    }
                }
            }
            return returnList;
        }

        public static List<InsuranceType> GetForDistributionByCompany(int companyID)
        {
            List<InsuranceType> returnList = new List<InsuranceType>();
            List<InsuranceType> dealInsuranceTypes = GetByCompany(companyID);
            List<InsuranceType> packetInsuranceType = GetPacketsByCompany(companyID);
            returnList.AddRange(dealInsuranceTypes);
            foreach (InsuranceType it in packetInsuranceType)
            {
                if (!returnList.Contains(it))
                {
                    returnList.Add(it);
                }
            }
            returnList = returnList.OrderBy(r => r.Code).ToList();
            return returnList;
        }

        public static List<InsuranceType> GetForDistributionByCompanyWithLife(int companyID) {
            List<InsuranceType> returnList = new List<InsuranceType>();
            List<InsuranceType> dealInsuranceTypes = GetByCompanyWithLife(companyID);
            List<InsuranceType> packetInsuranceType = GetPacketsByCompany(companyID);
            returnList.AddRange(dealInsuranceTypes);
            foreach (InsuranceType it in packetInsuranceType) {
                if (!returnList.Contains(it)) {
                    returnList.Add(it);
                }
            }
            returnList = returnList.OrderBy(r => r.Code).ToList();
            return returnList;
        }



        public static List<InsuranceType> GetByDeal(int dealID) {
            List<Brokerage> bList = Brokerage.GetByDeal(dealID);
            List<InsuranceType> insType = new List<InsuranceType>();
            foreach (Brokerage b in bList) {
                if (!insType.Contains(b.InsuranceSubType.InsuranceType)) {
                    insType.Add(b.InsuranceSubType.InsuranceType);
                }
            }
            return insType;
        }

        public static InsuranceType GetByInsuranceSubType(int insuranceSubTypeID) {
            return InsuranceSubType.Get(insuranceSubTypeID).InsuranceType;
        }

        public static List<InsuranceType> GetSorted()
        {
            return Table.OrderBy(o => o.Code).ToList();
        }

        public static List<InsuranceType> GetForOrder() {
            List<Deal> dealList = Deal.GetActiveDeals();
            List<InsuranceType> insuranceTypeList = new List<InsuranceType>();
            List<Brokerage> bList = new List<Brokerage>();
            foreach (Deal d in dealList) {
                bList.AddRange(d.Brokerages.ToList());
            }
            foreach (Brokerage b in bList) {
                if (!insuranceTypeList.Contains(b.InsuranceSubType.InsuranceType)) {
                    insuranceTypeList.Add(b.InsuranceSubType.InsuranceType);
                }
            }
            return insuranceTypeList;
        }

        public static InsuranceType GetFromOrderItemID(int orderItemID) {
            OrderItem oi = OrderItem.Get(orderItemID);
            return oi.InsuranceSubType.InsuranceType;
        }
        public static List<InsuranceType> GetFromOrderItemID() {
            List<Brokerage> brokList =  Brokerage.GetActiveBrokerages();
            List<InsuranceType> istList = new List<InsuranceType>();
            foreach (Brokerage b in brokList) {
                if (!istList.Contains(b.InsuranceSubType.InsuranceType)) {
                    istList.Add(b.InsuranceSubType.InsuranceType);
                }
            }
            return istList;
        }


        public static List<InsuranceType> GetForPolicy() {
            List<Deal> dealList = Deal.GetActiveDeals();
            List<InsuranceType> insuranceTypeList = new List<InsuranceType>();
            List<Brokerage> bList = new List<Brokerage>();
            foreach (Deal d in dealList) {
                foreach (Brokerage b in d.Brokerages) {
                    if (b.IsActive) {
                        bList.Add(b);
                    }
                }
                //bList.AddRange(d.Brokerages.ToList());
            }

            foreach (Brokerage b in bList) {
                if (!insuranceTypeList.Contains(b.InsuranceSubType.InsuranceType)) {
                    insuranceTypeList.Add(b.InsuranceSubType.InsuranceType);
                }
            }
            return insuranceTypeList;
        }

    }
}