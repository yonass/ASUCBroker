using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bank
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Bank : EntityBase<Bank> {

        public static string STOPANSKA_BANKA_ADSKOPJE = "00001";
        public static string KOMERCIJALNA_BANKA = "00002";

        public static bool ExistCodeInBanks(string code) {
            return (Table.Where(b => b.Code == code && b.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(b => b.Code == code && b.ID != codeID && b.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistAccountInBanks(string account) {
            return (Table.Where(b => b.BankAccount == account && b.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherAccount(int codeID, string account) {
            return (Table.Where(b => b.BankAccount == account && b.ID != codeID && b.IsActive == true).SingleOrDefault() != null);
        }
        public static List<Bank> GetActiveBanks() {
            return Table.Where(b => b.IsActive == true).ToList();
        }

        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Bank.ExistCodeInBanks(this.Code)) {
                ValidationErrors.Add("BANK_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            if (Bank.ExistAccountInBanks(this.BankAccount)) {
                ValidationErrors.Add("BANK_ACCOUNT_INSERT_EXIST", this.BankAccount + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Bank.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("BANK_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            if (Bank.ExistOtherAccount(this.ID, this.BankAccount)) {
                ValidationErrors.Add("BANK_ACCOUNT_UPDATE_EXISTS", this.BankAccount + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public static List<Bank> GetBanksWithGreditCards() {
            IEnumerable<BankCreditCard> bCCIenum = BankCreditCard.Select();
            List<Bank> bankList = new List<Bank>();
            foreach (BankCreditCard bCC in bCCIenum) {
                if (!bankList.Contains(bCC.Bank)) {
                    bankList.Add(bCC.Bank);
                }
            }
            return bankList;
        }
    }
}
