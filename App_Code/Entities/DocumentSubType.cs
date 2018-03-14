using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DocumentSubType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class DocumentSubType:EntityBase<DocumentSubType> {
        public static string POLISA = "003001";
        public static string FAKTURA_PROVIZIJA = "004001";
        public static string FAKTURA_KLIENT = "004002";
        public static string GRUPNA_FAKTURA = "004003";
        public static string IZLEZNA_FAKTURA_ZA_PROVZIJA_ZA_ZIVOTNO_OSUGURUVANjE = "004004";
        public static string DOGOVR_ZA_ZASTAPNISTVO = "001001";
        public static string VLEZNA_PONUDA = "006001";
        public static string IZLEZNA_PONUDA = "006002";
        public static string RABOTEN_NALOG = "002001";
        public static string STETA = "007001";
        public static string POLISA_ZIVOTNO_OSIGURUVANjE = "009001";

        public static bool ExistCodeInDocumentSubTypes(string code) {
            return (Table.Where(dst => dst.Code == code).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(dst => dst.Code == code && dst.ID != codeID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (DocumentSubType.ExistCodeInDocumentSubTypes(this.Code)) {
                ValidationErrors.Add("DOCUMENTSUBTYPE_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (DocumentSubType.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("DOCUMENTSUBTYPE_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public static DocumentSubType GetByCode(string code) {
            return Table.Where(d => d.Code == code).SingleOrDefault();
        }
    }
}
