using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Municipality
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Municipality:EntityBase<Municipality> {

        public static List<Municipality> GetActiveMunicipalities() {
            return Table.Where(m => m.IsActive == true).ToList();
        }


        public static bool ExistCodeInMunicipalities(string code) {
            return (Table.Where(m => m.Code == code && m.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(m => m.Code == code && m.ID != codeID && m.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Municipality.ExistCodeInMunicipalities(this.Code)) {
                ValidationErrors.Add("MUNICIPALITY_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Municipality.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("MUNICIPALITY_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }
        public static Municipality GetByCode(string code) {
            return Table.Where(m => m.Code == code).SingleOrDefault();
        }

    }
}
