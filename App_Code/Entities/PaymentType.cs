using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PaymentType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class PaymentType:EntityBase<PaymentType> {

        public static string CASH = "1";
        public static string VIRMAN = "2";
        public static string CREDITCARD = "3";
        public static string FACTURE = "4";
        public static string FACTURE_FOR_BROKER = "5";
        public static PaymentType GetByCode(string code) {
            return Table.Where(p => p.Code == code).SingleOrDefault();
        }

        public static bool ExistCodeInPaymentTypes(string code) {
            return (Table.Where(pt => pt.Code == code).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(pt => pt.Code == code && pt.ID != codeID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (PaymentType.ExistCodeInPaymentTypes(this.Code)) {
                ValidationErrors.Add("PAYMENTTYPE_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (PaymentType.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("PAYMENTTYPE_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

    }
}
