using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CashReportItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class CashReportItem : EntityBase<CashReportItem> {
        public static List<CashReportItem> GetByCashReport(int cashReportID) {
            return Table.Where(c => c.CashReportID == cashReportID).ToList();
        }
    }
}
