using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for BankController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class BankController
{
    public static void ValidateInsertCode(string code, string account) {
        Broker.DataAccess.Bank bank = new Broker.DataAccess.Bank();
        bank.Code = code;
        bank.BankAccount = account;
        bank.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code, string account) {
        Broker.DataAccess.Bank bank = new Broker.DataAccess.Bank();
        bank.Code = code;
        bank.BankAccount = account;
        bank.ID = codeID;
        bank.TestBeforeUpdate();
    }
}
}
	