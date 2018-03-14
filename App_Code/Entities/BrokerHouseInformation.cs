using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerHouseInformation
/// </summary>
/// 
namespace Broker.DataAccess
{
    public partial class BrokerHouseInformation:EntityBase<BrokerHouseInformation>
    {
        public static string FIRST_ROW = "01";
        
        public static BrokerHouseInformation GetInformation()
        {
            return Table.ToList().First();
        }

        public static string GetBrokerHouseName()
        {
            return Table.ToList().First().BrokerHouseName;
        }

        public static string GetBrokerHouseNameByCode(string code)
        {
            return Table.Where(b => b.Code == code).SingleOrDefault().BrokerHouseName;
        }
    }
}
