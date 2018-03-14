using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrokerageSlip
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class BrokerageSlip : EntityBase<BrokerageSlip> {
        public static string GetNextNumber(int insuranceCompanyID) {
            string nextNumber = "";
            InsuranceCompany ic = InsuranceCompany.Get(insuranceCompanyID);
            List<BrokerageSlip> lstBrokerageSlips = Table.Where(c => c.InsuranceCompanyID == insuranceCompanyID).ToList();
            if (lstBrokerageSlips.Count > 0) {
                List<int> codes = new List<int>();
                foreach (BrokerageSlip bs in lstBrokerageSlips) {
                    string[] spliter = bs.Number.Split('/');
                    codes.Add(Convert.ToInt32(spliter[0]));
                }
                codes.Sort();
                nextNumber = (codes.Last() + 1).ToString() + "/" + ic.Code;
            } else {
                nextNumber = "1" + "/" + ic.Code;
            }
            return nextNumber;
        }

    }
}
