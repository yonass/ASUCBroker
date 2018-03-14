using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for StatusController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class StatusController
{
    public static void ValidateInsertCode(string code) {
        Statuse statuse = new Statuse();
        statuse.Code = code;
        statuse.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        Statuse statuse = new Statuse();
        statuse.Code = code;
        statuse.ID = codeID;
        statuse.TestBeforeUpdate();
    }
}
}