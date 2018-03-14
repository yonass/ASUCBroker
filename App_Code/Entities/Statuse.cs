using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Status
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Statuse:EntityBase<Statuse> {
        public static string ZAVRSENA_VLEZNA_PONUDA = "612";
        public static string VOIZRABOTKA_VLEZNA_PONUDA = "611";
        public static string ZAVRSENA_IZLEZNA_PONUDA = "622";
        public static string VOIZRABOTKA_IZLEZNA_PONUDA = "621";

        public static string NEPLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJA = "511";
        public static string PLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJA = "512";
        public static string DELUMNO_PLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJA = "513";
        public static string NEPLATENA_IZLEZNA_FAKTURA_KON_KLIENT = "521";
        public static string PLATENA_IZLEZNA_FAKTURA_KON_KLIENT = "522";
        public static string DELUMNO_PLATENA_IZLEZNA_FAKTURA_KON_KLIENT = "523";
        public static string NEPLATENA_GRUPNA_FAKTURA_KON_KLIENT = "531";
        public static string PLATENA_GRUPNA_FAKTURA_KON_KLIENT = "532";
        public static string DELUMNO_PLATENA_GRUPNA_FAKTURA_KON_KLIENT = "533";

        public static string NEPLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJA_ZA_ZIVOTNO = "541";
        public static string PLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJAZA_ZIVOTNO = "542";
        public static string DELUMNO_PLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJAZA_ZIVOTNO = "543";

        public static string ZAVRSENA_POLISA = "411";
        public static string NEZAVRSENA_POLISA = "412";

        public static int testID = 7;

        public static List<Statuse> GetActiveStatuses() {
            return Table.Where(s => s.IsActive == true).ToList();
        }

        public static bool ExistCodeInStatuses(string code) {
            return (Table.Where(s => s.Code == code && s.IsActive == true).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(s => s.Code == code && s.ID != codeID && s.IsActive == true).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Statuse.ExistCodeInStatuses(this.Code)) {
                ValidationErrors.Add("STATUS_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Statuse.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("STATUS_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public List<Statuse> GetActiveStatusesByDocumentType(int docTypeID) {
            return GetActiveStatuses().Where(s => s.DocumentSubType.DocumentTypeID == docTypeID).ToList();
        }
        public static Statuse GetByCode(string code) {
            return Table.Where(c => c.Code == code).SingleOrDefault();
        }

        public static List<Statuse> GetActiveStatusesForDocumentSubType(int documentSubTypeID) {
            return Table.Where(s => s.DocumentSubTypeID == documentSubTypeID&&s.IsActive==true).ToList();
        }

    }
}
