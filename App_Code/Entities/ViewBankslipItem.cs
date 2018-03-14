using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ViewBankslipItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class ViewBankslipItem : EntityBase<ViewBankslipItem> {

        public static List<ViewBankslipItem> GetByBankslip(int bankslipID) {
            List<ViewBankslipItem> retList = new List<ViewBankslipItem>();
            retList = Table.Where(c => c.BankslipID == bankslipID).ToList();
            return retList;
        }

    }
}
