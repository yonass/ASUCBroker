using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bankslip
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Bankslip:EntityBase<Bankslip> {
        public static bool ExistBankslipNumberInBankslips(string bankslipNumber, int bankID) {
            return (Table.Where(b => b.BankslipNumber == bankslipNumber && b.BankID == bankID).SingleOrDefault() != null);

        }

        public static bool ExistOtherBankslipNumber(int bankslipID, string bankslipNumber, int bankID) {
            return (Table.Where(b => b.BankslipNumber == bankslipNumber && b.BankID == bankID && b.ID != bankslipID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void ValidateDeleteBankslip() {
            List<BankslipItem> lst = BankslipItem.GetByBankslip(this.ID);
            if (lst.Count > 0) {
                ValidationErrors.Add("BANKSLIP_DELETE", "Не може да се избрише извод доколку на него има ставки!");
            }
            this.PerformCustomValidation();
        }

        public static string GetNextNumber(int bankID) {
            string nextNumber = "1";
            List<Bankslip> lst = Table.Where(c => c.BankID == bankID).OrderBy(c => c.ID).ToList();
            if (lst.Count > 0) {
                try {
                    int newNumber = Convert.ToInt32(lst.Last().BankslipNumber) + 1;
                    nextNumber = newNumber.ToString();
                } catch {
                }
            }
            return nextNumber;
        }

        public void TestBeforeInsert() {
            if (Bankslip.ExistBankslipNumberInBankslips(this.BankslipNumber, this.BankID)) {
                ValidationErrors.Add("BANKSLIPNUMBER_INSERT_EXIST", "Постои извод со број " + this.BankslipNumber);
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Bankslip.ExistOtherBankslipNumber(this.ID, this.BankslipNumber, this.BankID)) {
                ValidationErrors.Add("BANKSLIPNUMBER_UPDATE_EXIST", "Постои извод со број " + this.BankslipNumber);
            }
            this.PerformCustomValidation();
        }
    }
}
