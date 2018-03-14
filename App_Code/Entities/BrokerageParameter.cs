using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageParameter
/// </summary>
namespace Broker.DataAccess {
    public partial class BrokerageParameter : EntityBase<BrokerageParameter> {


        public static List<BrokerageParameter> GetForBrokerageType(int brokerageTypeID) {
            List<BrokerageParametersBrokerageType> bpbtList = BrokerageParametersBrokerageType.GetForBrokerageType(brokerageTypeID);
            List<BrokerageParameter> bpList = new List<BrokerageParameter>();
            bpbtList = bpbtList.OrderBy(c => c.OrderNumber).ToList();
            foreach (BrokerageParametersBrokerageType bpbt in bpbtList) {
                bpList.Add(bpbt.BrokerageParameter);
            }
            return bpList;


        }

        public static decimal GetValueForParameter(BrokerageParameter bp) {
            if (bp.Code == "01") {
                return 55000;
            }
            if (bp.Code == "02") {
                return 200;
            }
            if (bp.Code == "03") {
                return 5000;
            }
            if (bp.Code == "04") {
                return 2000;
            }
            if (bp.Code == "05") {
                return 40000;
            } else {
                return 0;
            }
        }



        //public static List<BrokerageParameter> GetForBrokerageType(int brokerageTypeID) {
        //    List<BrokerageParametersBrokerageType> bpbtList = BrokerageParametersBrokerageType.GetForBrokerageType(brokerageTypeID);
        //    List<BrokerageParameter> bpList = new List<BrokerageParameter>();

        //    foreach (BrokerageParametersBrokerageType bpbt in bpbtList) {
        //        bpList.Add(bpbt.BrokerageParameter);
        //    }
        //    return bpList;


        //}

        //public static decimal GetValueForParameter(BrokerageParameter bp) {
        //    if (bp.Code == "01") {
        //        return 5500;
        //    } else {
        //        return 0;
        //    }
        //}



    }
}
