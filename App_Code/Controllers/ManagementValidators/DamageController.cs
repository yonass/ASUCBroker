using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for DamageController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class DamageController {
        public static void ValidateInsertDamageNumber(string damageNumber, string policyNumber, int insuranceSubTypeID, int insuranceCompanyID) {
            Damage damage = new Damage();
            damage.DamageNumber = damageNumber;
            damage.TestBeforeInsert(policyNumber, insuranceSubTypeID, insuranceCompanyID);
        }

        public static void ValidateUpdateDamageNumber(int damageID, string damageNumber, string policyNumber, int insuranceSubTypeID, int insuranceCompanyID, int policyItemID)
        {
            Damage damage = new Damage();
            damage.DamageNumber = damageNumber;
            damage.ID = damageID;
            damage.TestBeforeUpdate(policyNumber, insuranceSubTypeID, insuranceCompanyID, policyItemID);
        }
    }
}
