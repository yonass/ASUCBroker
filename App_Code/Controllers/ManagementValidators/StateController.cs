using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for StateController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class StateController
{
    public static void ValidateInsertCode(string code) {
        Broker.DataAccess.State state = new Broker.DataAccess.State();
        state.Code = code;
        state.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        Broker.DataAccess.State state = new Broker.DataAccess.State();
        state.Code = code;
        state.ID = codeID;
        state.TestBeforeUpdate();
    }
}
}