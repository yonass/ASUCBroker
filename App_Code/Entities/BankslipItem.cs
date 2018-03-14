using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BankslipItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class BankslipItem : EntityBase<BankslipItem> {

        public static List<BankslipItem> GetByBankslip(int bankslipID) {
            return Table.Where(c => c.BankslipID == bankslipID).ToList();
        }

        public void ValidateInsertBankslipItem() {
            if (this.DebtValue > 0 && this.DemandValue > 0) {
                ValidationErrors.Add("BANKSLIPITEM_INSERT_VALUES", "Не може истовремено и должи и побарува да имаат вредност 0!");
            }
            if (this.DebtValue <= 0 && this.DemandValue <= 0) {
                ValidationErrors.Add("BANKSLIPITEM_INSERT_VALUES", "Мора или вредноста за должи или вредноста за побарува да има позитивна вредност!");
            }
            this.PerformCustomValidation();
        }


        public void ValidateUpdateBankslipItem() {
            if (this.DebtValue > 0 && this.DemandValue > 0) {
                ValidationErrors.Add("BANKSLIPITEM_UPDATE_VALUES", "Не може истовремено и должи и побарува да имаат вредност 0!");
            }
            if (this.DebtValue <= 0 && this.DemandValue <= 0) {
                ValidationErrors.Add("BANKSLIPITEM_UPDATE_VALUES", "Мора или вредноста за должи или вредноста за побарува да има позитивна вредност!");
            }
            this.PerformCustomValidation();
        }

        public void ValidateDeleteBankslipItem() {
            if (this.IsPaid == true) {
                ValidationErrors.Add("BANKSLIPITEM_DELETE_VALUES", "Не може да се брише ставка според која се евидентирани плаќања на полиси!");
            }
            this.PerformCustomValidation();
        }
    }
}
