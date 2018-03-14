using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CashReport
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class CashReport : EntityBase<CashReport> {
        public static string GetNextNumber(Branch branch) {
            string nextNumber = "";
            List<CashReport> lstCashReport = Table.Where(c => c.BranchID == branch.ID).ToList();
            if (lstCashReport.Count > 0) {
                List<int> codes = new List<int>();
                foreach (CashReport cr in lstCashReport) {
                    string[] spliter = cr.Number.Split('/');
                    codes.Add(Convert.ToInt32(spliter[0]));
                }
                codes.Sort();
                nextNumber = (codes.Last() + 1).ToString() + "/" + branch.Code;
            } else {
                nextNumber = "1" + "/" + branch.Code;
            }
            return nextNumber;
        }


        public static void DiscardCashReport(int id) {
            CashReport cr = CashReport.Get(id);
            cr.Discard = true;
            CashReport.Table.Context.SubmitChanges();
            List<CashReportItem> lst = CashReportItem.GetByCashReport(id);
            foreach (CashReportItem cri in lst) {
                Payment uPayment = Payment.Get(cri.PaymentID);
                uPayment.IsCashReported = false;
                Payment.Table.Context.SubmitChanges();
            }
        }
    }

}
