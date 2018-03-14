using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DocumentType
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class DocumentType:EntityBase<DocumentType> {

        public static string DOGOVOR = "001";
        public static string NALOG = "002";
        public static string POLISA = "003";
        public static string FAKTURA = "004";
        public static string USLUGA = "005";
        public static string PONUDA = "006";
        public static string STETA = "007";
        public static string DOGOVOR_ZA_RATI = "008";
        public static string POLISA_ZA_ZIV_OSI = "009";

        public static DocumentType GetByCode(string code) {
            return Table.Where(dt => dt.Code == code).SingleOrDefault();
        }


        public static bool ExistCodeInDocumentTypes(string code) {
            return (Table.Where(dt => dt.Code == code).SingleOrDefault() != null);
        }

        public static bool ExistOtherCode(int codeID, string code) {
            return (Table.Where(dt => dt.Code == code && dt.ID != codeID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (DocumentType.ExistCodeInDocumentTypes(this.Code)) {
                ValidationErrors.Add("DOCUMENTTYPE_CODE_INSERT_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (DocumentType.ExistOtherCode(this.ID, this.Code)) {
                ValidationErrors.Add("DOCUMENTTYPE_CODE_UPDATE_EXISTS", this.Code + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

    }
}
