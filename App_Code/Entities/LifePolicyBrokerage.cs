using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LifePolicyBrokerage
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class LifePolicyBrokerage : EntityBase<LifePolicyBrokerage> {
        public static List<LifePolicyBrokerage> GetByLifePolicy(int lifePolicyID) {
            return Table.Where(c => c.LifePolicyID == lifePolicyID).OrderBy(c => c.OrdinalNumber).ToList();
        }

        public static void SetFactured(List<LifePolicyBrokerage> paymentList, Facture f, List<LifePolicy> policiesToRemove) {
            List<LifePolicyBrokerage> notForSetting = new List<LifePolicyBrokerage>();
            foreach (LifePolicy pi in policiesToRemove) {
                notForSetting.AddRange(LifePolicyBrokerage.GetByLifePolicy(pi.ID));
            }
            List<LifePolicyBrokerage> validPaymentList = new List<LifePolicyBrokerage>();
            foreach (LifePolicyBrokerage p in paymentList) {
                if (!notForSetting.Contains(p)) {
                    validPaymentList.Add(p);
                }
            }
            foreach (LifePolicyBrokerage p in validPaymentList) {
                p.IsFactured = true;
                p.Update2();
                //Context.SubmitChanges();
                FacturePayment fp = new FacturePayment();
                fp.PaymentID = p.ID;
                fp.FactureID = f.ID;
                fp.PaymentTypeID = PaymentType.GetByCode(PaymentType.FACTURE).ID;
                fp.Insert();

            }
        }
    }
}