using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for DocumentTypeController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class DocumentTypeController {
        public static void ValidateInsertCode(string code) {
            Broker.DataAccess.DocumentType documentType = new Broker.DataAccess.DocumentType();
            documentType.Code = code;
            documentType.TestBeforeInsert();
        }

        public static void ValidateUpdateCode(int codeID, string code) {
            Broker.DataAccess.DocumentType documentType = new Broker.DataAccess.DocumentType();
            documentType.Code = code;
            documentType.ID = codeID;
            documentType.TestBeforeUpdate();
        }
    }
}
