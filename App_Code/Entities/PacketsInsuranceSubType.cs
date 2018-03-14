using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PacketInsuranceSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class PacketsInsuranceSubType : EntityBase<PacketsInsuranceSubType> {

        public static List<PacketsInsuranceSubType> GetByPacket(int packetID) {
            return Table.Where(pist => pist.PacketID == packetID).ToList();
        }

        public static decimal GetDefaultBrokerage(int companyID, int insuranceSubTypeID) {
            List<Brokerage> bList = Brokerage.GetActiveByInsuranceSubType(insuranceSubTypeID);
            foreach(Brokerage b in bList){
                if(b.Deal.InsuranceCompanyID==companyID){
                    return b.PercentageForPrivates;
                }
            }
            return 0;
        }

        public static decimal GetDefaultBrokerageForPrivates(int companyID, int insuranceSubTypeID)
        {
            List<Brokerage> bList = Brokerage.GetActiveByInsuranceSubType(insuranceSubTypeID);
            foreach (Brokerage b in bList)
            {
                if (b.Deal.InsuranceCompanyID == companyID)
                {
                    return b.PercentageForPrivates;
                }
            }
            return 0;
        }

        public static decimal GetDefaultBrokerageForLaws(int companyID, int insuranceSubTypeID)
        {
            List<Brokerage> bList = Brokerage.GetActiveByInsuranceSubType(insuranceSubTypeID);
            foreach (Brokerage b in bList)
            {
                if (b.Deal.InsuranceCompanyID == companyID)
                {
                    return b.PercentageForLaws;
                }
            }
            return 0;
        }

    }
}
