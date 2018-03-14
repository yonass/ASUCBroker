using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for BankslipController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class BankslipController {
        public static void ValidateInsertBankslipNumber(string bankslipNumber, int bankID) {
            Bankslip bankslip = new Bankslip();
            bankslip.BankslipNumber = bankslipNumber;
            bankslip.BankID = bankID;
            bankslip.TestBeforeInsert();
        }

        public static void ValidateUpdateBankslipNumber(int bankslipID, string bankslipNumber, int bankID) {
            Bankslip bankslip = new Bankslip();
            bankslip.BankslipNumber = bankslipNumber;
            bankslip.ID = bankslipID;
            bankslip.BankID = bankID;
            bankslip.TestBeforeUpdate();
        }
    }
}