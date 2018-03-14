using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for DocumentSubTypeController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class DocumentSubTypeController
{
    public static void ValidateInsertCode(string code) {
        DocumentSubType documentSubType = new DocumentSubType();
        documentSubType.Code = code;
        documentSubType.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        DocumentSubType documentSubType = new DocumentSubType();
        documentSubType.Code = code;
        documentSubType.ID = codeID;
        documentSubType.TestBeforeUpdate();
    }
}
}
