using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for State
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class State : EntityBase<State> {

        public static string MACEDONIA = "01";


        public static List<State> GetActiveStates() {
            return Table.Where(s => s.IsActive == true).ToList();
        }

        public static bool ExistCodeInStates(string code) {
            return (Table.Where(s => s.Code == code && s.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(s => s.Code == code && s.ID != codeID && s.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (State.ExistCodeInStates(this.Code)) {
                ValidationErrors.Add("STATE_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (State.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("STATE_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public static State GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }
    }
}
