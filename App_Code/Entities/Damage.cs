using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Damage
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Damage : EntityBase<Damage> {

        public static bool ExistDamageNumberInDamages(string damageNumber) {
            return (Table.Where(d => d.DamageNumber == damageNumber).SingleOrDefault() != null);
            
        }

        public static bool ExistOtherDamageNumber(int damageID, string damageNumber) {
            return (Table.Where(d => d.DamageNumber == damageNumber && d.ID != damageID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert(string policyNumber, int insuranceSubTypeID, int insuranceCompanyID) {
            if (Damage.ExistDamageNumberInDamages(this.DamageNumber)) {
                ValidationErrors.Add("DAMAGENUMBER_INSERT_EXIST", "Постои штета со број " + this.DamageNumber);
            }
            if ((PolicyItem.Table.Where(p => p.PolicyNumber == policyNumber && p.InsuranceSubTypeID == insuranceSubTypeID && p.Policy.InsuranceCompanyID==insuranceCompanyID).SingleOrDefault() == null)) {
                ValidationErrors.Add("POLICYNUMBER_INSERT_DOESNOT_EXISTS", "Не постои полиса со број " + policyNumber);
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate(string policyNumber, int insuranceSubTypeID, int insuranceCompanyID, int policyItemID)
        {
            if (Damage.ExistOtherDamageNumber(this.ID, this.DamageNumber)) {
                ValidationErrors.Add("DAMAGENUMBER_UPDATE_EXIST", "Постои штета со број " + this.DamageNumber);
            }
            if ((PolicyItem.Table.Where(p => p.PolicyNumber == policyNumber && p.InsuranceSubTypeID == insuranceSubTypeID && p.Policy.InsuranceCompanyID == insuranceCompanyID && p.ID != policyItemID).SingleOrDefault() == null))
            {
                ValidationErrors.Add("POLICYNUMBER_UPDATE_DOESNOT_EXISTS", "Не постои полиса со број " + policyNumber);
            }
            this.PerformCustomValidation();
        }

    }
}